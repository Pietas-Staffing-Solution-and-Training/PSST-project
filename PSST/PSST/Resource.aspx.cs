using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using PSST.Resources.lib;
using static QuestPDF.Helpers.Colors;
using System.Xml.Linq;
using System.Web.Security;
using Org.BouncyCastle.Asn1.Cms;

namespace PSST
{
    public partial class Resource : System.Web.UI.Page
    {
        MySqlConnection con;
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        bool admin;
        string username;
        int userID; 

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get session value - returns null if doesn't exist
            username = Session["username"]?.ToString();
            userID = Convert.ToInt32(Session["userID"]?.ToString());

            // If string is null, user not logged in
            if (username == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // If userID is not null but user has username, user is admin
            if(Session["userID"] == null)
            {
                admin = true;
                adminPanel.Visible = true;
                resourcePanel.Visible = false;
                btnAddResource.Text = "Add Resource";
            // If user is normal user (not admin)
            } else {
                admin= false;
                adminPanel.Visible = false;
                resourcePanel.Visible = true;
                btnAddResource.Text = "Change Password";
                txtSearch.Visible = false;
                btnSearch.Visible = false;
                btnSearchClear.Visible = false;
            }

            if (!IsPostBack)
            {
                BindGridView();
                FillIDBox();
                divError.Visible = false;
            }
        }

        protected void ResourceData_SelectedIndexChanged(object sender, EventArgs e) // Gets the GridView selection
        {
            int selectedRow = ResourceData.SelectedIndex;
            GridViewRow row = ResourceData.Rows[selectedRow];

            int id = Convert.ToInt32(row.Cells[3].Text);
        }

        private void BindGridView(string optQuery = "") // Bind the DataTable to the GridView
        {
            string query;

            if (admin)
            {
                query = "SELECT Resource_ID, FName AS 'First Name', LName AS 'Last Name', Phone_Num AS 'Phone Number', ROUND(Wage, 2) AS 'Wage p/h', Competencies FROM RESOURCE";
                
                if (optQuery.Length > 0)
                {
                    query = optQuery;
                }
            }
            else
            {
                query = "SELECT Resource_ID, FName AS 'First Name', LName AS 'Last Name', Phone_Num AS 'Phone Number', ROUND(Wage, 2) AS 'Wage p/h', Competencies FROM RESOURCE WHERE Resource_ID = @userID";
            }

            try
            {
                using (con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        if (!admin)
                        {
                            cmd.Parameters.AddWithValue("@userID", userID);  // Parameterized userID
                        }

                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            ResourceData.DataSource = dt;
                            ResourceData.DataBind();
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
        }

        private void FillIDBox() // Gets the next ID
        {
            string query = "SELECT MAX(Resource_ID) FROM RESOURCE";
            try
            {
                txtFName.Text = string.Empty;
                txtLName.Text = string.Empty;
                txtPhoneNum.Text = string.Empty;
                txtWage.Text = string.Empty;
                txtCompetencies.Text = string.Empty;

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    con.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int highestID = reader.GetInt32(0);
                            int nextID = highestID + 1;

                            // Set the new ID in the TextBox
                            txtID.Text = nextID.ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
        }

        protected void ResourceData_RowDeleting(object sender, GridViewDeleteEventArgs e) // Delete a row button
        {
            int id = Convert.ToInt32(ResourceData.DataKeys[e.RowIndex].Value);

            deleteRecord(id);

            BindGridView();  
        }

        protected void ResourceData_RowEditing(object sender, GridViewEditEventArgs e) // Edit a row button
        {
            ResourceData.EditIndex = e.NewEditIndex;
            BindGridView();

            GridViewRow row = ResourceData.Rows[e.NewEditIndex];
            TextBox tbName = (TextBox)row.Cells[4].Controls[0];
            tbName.Focus();
        }

        protected void ResourceData_RowUpdating(object sender, GridViewUpdateEventArgs e) // Update a row button
        {
            try
            {
                GridViewRow row = ResourceData.Rows[e.RowIndex];

                int id = Convert.ToInt32(ResourceData.DataKeys[e.RowIndex].Value);
                string name = ((TextBox)row.Cells[3].Controls[0]).Text;
                string surname = ((TextBox)row.Cells[4].Controls[0]).Text;
                string number = ((TextBox)row.Cells[5].Controls[0]).Text;
                string wage = ((TextBox)row.Cells[6].Controls[0]).Text;
                string competencies = ((TextBox)row.Cells[7].Controls[0]).Text;

                updateRecord(id, name, surname, number, wage, competencies );

                ResourceData.EditIndex = -1;
                BindGridView();
            } catch(Exception ex)
            {
                showError(ex.Message);
            }
        }

        protected void ResourceData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) // Cancel the edit process
        {
            ResourceData.EditIndex = -1;  
            BindGridView();  
        }



        protected void txtSearch_TextChanged(object sender, EventArgs e) // Searches when the text in Search textbox changes
        {
            divError.Visible = false; // Hides errors when searching again
            string search = txtSearch.Text;
            string query = $"SELECT Resource_ID, FName AS 'First Name', LName AS 'Last Name', Phone_Num AS 'Phone Number', ROUND(Wage, 2) AS 'Wage p/h', Competencies FROM RESOURCE WHERE Resource_ID LIKE @SearchTerm OR FName LIKE @SearchTerm OR LName LIKE @SearchTerm OR Phone_Num LIKE @SearchTerm OR Wage LIKE @SearchTerm OR Competencies LIKE @SearchTerm";

            try
            {
                using (con = new MySqlConnection(connectionString))
                {
                   
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SearchTerm", "%" + search + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ResourceData.DataSource = dt;
                        ResourceData.DataBind();
                    }
                    else
                    {
                        // Show error message if no items are found
                        showError($"No item found for {search}");
                        ResourceData.DataSource = null;
                        ResourceData.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
        }

        protected void updateRecord(int id, string name, string surname, string number, string wage, string competencies) // Updates a Resource record in the GridView
        {
            string query = @"UPDATE RESOURCE SET FName = @FName, LName = @LName, Phone_Num = @PhoneNum, Wage = @Wage, Competencies = @Competencies WHERE Resource_ID = @ResourceID";

            try
            {
                if (!(Regex.IsMatch(number, @"^(\+27|0)[6-8][0-9]{8}$")))
                {
                    throw new Exception("Invalid Phone Number.");
                }

                if (!(decimal.TryParse(wage, out decimal outWage)))
                {
                    throw new Exception("Invalid Wage.");
                }

                using (con = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FName", name);
                        cmd.Parameters.AddWithValue("@LName", surname);
                        cmd.Parameters.AddWithValue("@PhoneNum", number);
                        cmd.Parameters.AddWithValue("@Wage", outWage);
                        cmd.Parameters.AddWithValue("@Competencies", competencies);
                        cmd.Parameters.AddWithValue("@ResourceID", id);

                        con.Open();

                        int rowsAffected = cmd.ExecuteNonQuery();
                        adapter.UpdateCommand = cmd;
                        adapter.UpdateCommand.ExecuteNonQuery();

                        con.Close();

                        BindGridView();
                    }
                }
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
        }

        protected void deleteRecord(int id) // Deletes a record from the DB
        {
            string query = @"DELETE FROM RESOURCE WHERE Resource_ID = @ResourceID";

            try
            {
                using (con = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ResourceID", id);

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();


                        adapter.DeleteCommand = cmd;
                        adapter.DeleteCommand.ExecuteNonQuery();

                        con.Close();

                        BindGridView();
                    }
                }
                clearError();
                FillIDBox();
            }
            catch (MySqlException)
            {
               string jobId = ifRhasJob(id);
               showError($"Cannot delete resource because they are currently assigned a Job (ID: {jobId})");
            }
        }

        protected string ifRhasJob(int id) // Get the id of the job a resource is connected (If Resource Has Job = ifRhasJob)
        {
            string jobId = "";
            string jobQuery = @"SELECT Job_ID FROM JOB WHERE Resource_ID = @Resource_ID";

            using (con = new MySqlConnection(connectionString))
            {

                MySqlCommand command = new MySqlCommand(jobQuery, con);
                command.Parameters.AddWithValue("@Resource_ID", id);

                con.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        jobId = reader["Job_ID"].ToString();
                    }
                }

                con.Close();
            }
            return jobId;
        }

        protected void showError(string error) // Shows an error
        {
            divError.Visible = true;
            lblError.Text = error;
        }

        protected void clearError() // Clears the error
        {
            divError.Visible = false;
        }

        protected void btnExitErr_Click(object sender, EventArgs e) // Button to close the error
        {
            clearError();
        }

        protected void btnSearch_Click(object sender, EventArgs e) // Acts as if text changed in textbox for search
        {
            txtSearch_TextChanged(sender, e);
        }

        protected void btnSearchClear_Click(object sender, EventArgs e) // Clears the search bar
        {
            txtSearch.Text = "";
            txtSearch_TextChanged(sender, e);
        }

        protected void btnAddDB_Click(object sender, EventArgs e) // Adds new Resource to DB
        {
            string query = @"INSERT INTO RESOURCE (Resource_ID, FName, LName, Phone_Num, Password, Wage, Competencies) 
                 VALUES (@ResourceID, @FName, @LName, @PhoneNum, @Password, @Wage, @Competencies)";

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        int resourceID;
                        string fName = txtFName.Text;
                        string lName = txtLName.Text;
                        string phoneNum = txtPhoneNum.Text;
                        decimal wage;
                        string competencies = txtCompetencies.Text;
                        
                        if (!(int.TryParse(txtID.Text, out resourceID)))
                        {
                            throw new Exception("Invalid Resource ID.");
                        }

                        if (!(Regex.IsMatch(phoneNum, @"^(\+27|0)[6-8][0-9]{8}$")))
                        {
                            throw new Exception("Invalid Phone Number.");
                        }

                        if (!(decimal.TryParse(txtWage.Text, out wage)))
                        {
                            throw new Exception("Invalid Wage.");
                        }

                        string password = resourceID.ToString() + fName + "password1!";

                        password = validatePassword(password);

                        cmd.Parameters.AddWithValue("@ResourceID", txtID.Text);
                        cmd.Parameters.AddWithValue("@FName", fName);
                        cmd.Parameters.AddWithValue("@LName", lName);
                        cmd.Parameters.AddWithValue("@PhoneNum", phoneNum);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@Wage", wage);
                        cmd.Parameters.AddWithValue("@Competencies", competencies);
                        
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if the insert was successful
                        if (rowsAffected > 0) {
                            BindGridView();
                        }
                        else {
                            showError("Insert operation failed."); // Insert failed or no rows affected
                        }

                        con.Close();
                    }
                }

                FillIDBox();
                clearError();
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
        }

        protected void btnChangePass_Click(object sender, EventArgs e) // Allows changing password as Resource (user)
        {
            try { 
                string password = txtPassword.Text;
                string encryptedPassword = validatePassword(password);
                string query = @"UPDATE RESOURCE SET Password = @Password WHERE Resource_ID = @ResourceID";

                using (con = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Password", encryptedPassword);
                        cmd.Parameters.AddWithValue("@ResourceID", userID);

                        con.Open();

                        int rowsAffected = cmd.ExecuteNonQuery();
                        adapter.UpdateCommand = cmd;
                        adapter.UpdateCommand.ExecuteNonQuery();

                        con.Close();

                        BindGridView();
                    }
                }
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
        }  
        
        protected string validatePassword(string password) // Validates correct password format
        {
            string encryptedPassword = "";

            security encryptPass = new security();

            bool isvalid = encryptPass.isValidPassword(password);

            if (isvalid)
            {
                encryptedPassword = encryptPass.encrypt(password);
            }
            else
            {
                showError("Invalid password. Password needs 8 chars or more, 1 uppercase, 1 lowercase, 1 number, 1 special character");
            }

            return encryptedPassword;
        }

        protected void ResourceData_Sorting(object sender, GridViewSortEventArgs e) // Sorts GridView
        {
            string sortExpression = e.SortExpression;
            string columnName = "Resource_ID";
            string sortDirection = "ASC";

            // Check if the current column is the same as the last sorted column, then toggle
            if (ViewState["SortExpression"] != null && ViewState["SortExpression"].ToString() == sortExpression)
            {
                sortDirection = ViewState["SortDirection"].ToString() == "ASC" ? "DESC" : "ASC";
            }

            // Update ViewState to store the sort expression and direction
            ViewState["SortExpression"] = sortExpression;
            ViewState["SortDirection"] = sortDirection;

            // Select correct column to sort
            switch (sortExpression)
            {
                case "First Name":
                    columnName = "FName";
                    break;
                case "Last Name":
                    columnName = "LName";
                    break;
                case "Phone Number":
                    columnName = "Phone_Num";
                    break;
                case "Wage p/h":
                    columnName = "Wage";
                    break;
                case "Competencies":
                    columnName = "Competencies";
                    break;
            }

            string query = $"SELECT Resource_ID, FName AS 'First Name', LName AS 'Last Name', Phone_Num AS 'Phone Number', ROUND(Wage, 2) AS 'Wage p/h', Competencies FROM RESOURCE ORDER BY {columnName} {sortDirection}";

            BindGridView(query);
        }
    }                                                        
}