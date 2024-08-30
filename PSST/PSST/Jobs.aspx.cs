using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Data.SqlClient;
using static QuestPDF.Helpers.Colors;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using MySqlX.XDevAPI;

namespace PSST
{
    public partial class Jobs : System.Web.UI.Page
    {
        MySqlConnection con;
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        bool admin;
        int user_ID, jobID;

        protected void Page_Load(object sender, EventArgs e)
        {

            //tbUsername.Text = "Ruan@email.com";
            //tbPassword.Text = "TestThisP@s5W0rD!";

            //Get session value - returns null if doesn't exist
            string username = Session["username"]?.ToString();
            

            //If string is null
            if (username == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (Session["userID"] == null)
            {
                admin = true;
                userPanel.Visible = false;
            }
            else
            {
                adminPanel.Visible = false;
                admin = false;
                btnAddJob.Text = "Add Time";
                btnSearch.Visible = false;
                btnSearchClear.Visible = false;
                txtSearch.Visible = false;
                user_ID = Convert.ToInt32(Session["userID"]);               
            }

            if (!IsPostBack)
            {
                BindGridView();
                FillIDBox();
                divError.Visible = false;
            }


        }

        protected void JobData_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedRow = JobData.SelectedIndex;
            GridViewRow row = JobData.Rows[selectedRow];

            int id = Convert.ToInt32(row.Cells[3].Text);

            if (admin)
            {
                lblWelcome.Text = admin.ToString();
                Session["Job_Id"] = id;
                Response.Redirect("InvoiceForm.aspx");
            }
            else
            {
                showError("Access denied");
            }
        }

        private void BindGridView(string optQuery = "")
        {
            string query;

            if (admin)
            {
                query = "SELECT Job_ID, Status, Description, Resource_ID, Client_ID, ROUND(Budget, 2) AS 'Budget' FROM JOB";

                if (optQuery.Length > 0)
                {
                    query = optQuery;
                }
            }
            else
            {
                query = $"SELECT Job_ID, Status, Description, Resource_ID, Client_ID, ROUND(Budget, 2) AS 'Budget' FROM JOB WHERE Resource_ID = '{user_ID}'";
            }           

            try
            {
                using (con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    JobData.DataSource = dt;
                    JobData.DataBind();

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
            string query = "SELECT MAX(Job_ID) FROM JOB";
            try
            {
                txtJobID.Text = string.Empty;
                txtClientID.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtBudget.Text = string.Empty;

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
                            txtJobID.Text = nextID.ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
        }

        protected void JobData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(JobData.DataKeys[e.RowIndex].Value);

            deleteRecord(id);

            BindGridView();
        }

        protected void JobData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            JobData.EditIndex = e.NewEditIndex;
            BindGridView();

            GridViewRow row = JobData.Rows[e.NewEditIndex];
            TextBox tbName = (TextBox)row.Cells[4].Controls[0];
            tbName.Focus();
        }

        protected void JobData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = JobData.Rows[e.RowIndex];

                jobID = Convert.ToInt32(row.Cells[3].Text);
                string status = ((TextBox)row.Cells[4].Controls[0]).Text;
                string description = ((TextBox)row.Cells[5].Controls[0]).Text;
                int resourceID = convertStringToInt( ( (TextBox)row.Cells[6].Controls[0]).Text);
                int clientID = convertStringToInt( ( (TextBox)row.Cells[7].Controls[0]).Text);
                decimal budget = convertStringToDecimal(((TextBox)row.Cells[8].Controls[0]).Text);

                updateRecord(jobID, description, resourceID, clientID, budget, status);

                JobData.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
        }

        protected void JobData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            JobData.EditIndex = -1;
            BindGridView();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            divError.Visible = false; // Hides errors when searching again
            string search = txtSearch.Text;

            string query = $"SELECT Job_ID, Description, Resource_ID, Client_ID, ROUND(Budget, 2) AS Budget FROM JOB WHERE Job_ID LIKE @SearchTerm OR Description LIKE @SearchTerm OR Resource_ID LIKE @SearchTerm OR Client_ID LIKE @SearchTerm OR Budget LIKE @SearchTerm";

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
                        JobData.DataSource = dt;
                        JobData.DataBind();
                    }
                    else
                    {
                        // Show error message if no items are found
                        showError($"No item found for {search}");
                        JobData.DataSource = null;
                        JobData.DataBind();
                    };

                }
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
        }

        private int convertStringToInt(string stringInput)
        {
            
            int output;
            bool success = int.TryParse(stringInput, out output);

            if (success)
            {
                return output;
            }
            
            return 0;
            
        }

        private decimal convertStringToDecimal(string stringInput)
        {
            decimal output;
            bool success = decimal.TryParse(stringInput, out output);

            if (success)
            {
                return output; 
            }

            return 0;
        }

        protected void updateRecord(int jobID, string description, int resourceID, int clientID, decimal budget, string status)
        {
            string query = @"UPDATE JOB SET Status = @Status, Description = @Description, Resource_ID = @ResourceID, Client_ID = @ClientID, Budget = @Budget WHERE Job_ID = @JobID";

            try
            {
                using (con = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {

                        cmd.Parameters.AddWithValue("@JobID", jobID);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@ResourceID", resourceID);
                        cmd.Parameters.AddWithValue("@ClientID", clientID);
                        cmd.Parameters.AddWithValue("@Budget", budget);                        

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

        protected void deleteRecord(int id)
        {

            string query = @"DELETE FROM JOB WHERE Job_ID = @JobID";

            try
            {
                using (con = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@JobID", id);

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
            catch (MySqlException e)
            {
                showError($"{e.ToString()}");
            }
        }

        protected void showError(string error)
        {
            divError.Visible = true;
            lblError.Text = error;
        }

        protected void clearError()
        {
            divError.Visible = false;
        }

        protected void btnExitErr_Click(object sender, EventArgs e)
        {
            divError.Visible = false;
        }

        protected void btnSearch_Click(object sender, EventArgs e) // Acts as if text changed in textbox for search
        {
            txtSearch_TextChanged(sender, e);
        }

        protected void btnSearchClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            txtSearch_TextChanged(sender, e);
        }

        protected void btnAddDB_Click(object sender, EventArgs e)
        {
            string query = @"INSERT INTO JOB (Description, Resource_ID, Client_ID,Budget) 
                 VALUES (@Description, @ResourceID, @ClientID, @Budget)";

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        int jobID;
                        string description = txtDescription.Text;
                        int resourceID; ;
                        int clientID;
                        decimal budget;

                        if (!(int.TryParse(txtJobID.Text, out jobID)))
                        {
                            throw new Exception("Invalid Job ID.");
                        }

                        if (!(int.TryParse(txtResourceID.Text, out resourceID)))
                        {
                            throw new Exception("Invalid Resource ID.");
                        }

                        if (!(int.TryParse(txtClientID.Text, out clientID)))
                        {
                            throw new Exception("Invalid Client ID.");
                        }

                        if (!(decimal.TryParse(txtBudget.Text, out budget)))
                        {
                            throw new Exception("Invalid Wage.");
                        }

                        cmd.Parameters.AddWithValue("@JobID", jobID);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@ResourceID", resourceID);
                        cmd.Parameters.AddWithValue("@ClientID", clientID);
                        cmd.Parameters.AddWithValue("@Budget", budget);

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if the insert was successful
                        if (rowsAffected > 0)
                        {
                            BindGridView();
                        }
                        else
                        {
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

        protected void JobData_Sorting(object sender, GridViewSortEventArgs e) // Sorts GridView
        {
            string sortExpression = e.SortExpression;
            string columnName = "Job_ID";
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
                case "Status":
                    columnName = "Status";
                    break;
                case "Description":
                    columnName = "Description";
                    break;
                case "Resource_ID":
                    columnName = "Resource_ID";
                    break;
                case "Client_ID":
                    columnName = "Client_ID";
                    break;
                case "Budget":
                    columnName = "Budget";
                    break;
            }

            string query = $"SELECT Job_ID, Status, Description, Resource_ID, Client_ID, ROUND(Budget, 2) AS 'Budget' FROM JOB ORDER BY {columnName} {sortDirection}";

            BindGridView(query);
        }

        protected void btnAddTime_Click(object sender, EventArgs e)
        {
            GridViewRow row = JobData.Rows[0];
            string jobID = row.Cells[3].Text;
            float hours_worked;
            if (!(float.TryParse(txtTime.Text, out hours_worked))) {
                showError("Insert a valid time.");
            }
            else
            {
                string query = $"UPDATE INVOICE SET Hours_Worked = '{hours_worked}' WHERE Job_ID = '{jobID}'";
                try
                {
                    using (con = new MySqlConnection(connectionString))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);

                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            

                            con.Open();

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
            
            
        }
    }
}