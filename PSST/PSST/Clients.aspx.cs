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

namespace PSST
{
    public partial class Clients : System.Web.UI.Page
    {
        MySqlConnection con;
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

            //Get session value - returns null if doesn't exist
            string username = Session["username"]?.ToString();
            string type = Session["type"]?.ToString();
            type = "admin"; // REMOVE IN PRODUCTION

            //If string is null
            if (username == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (type == "admin")
            {

            }
            else
            {
                adminPanel.Visible = false;
                Response.Redirect("Dashboard.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindGridView();
                FillIDBox();
                divError.Visible = false;
            }
        }

        protected void ClientsData_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedRow = ClientsData.SelectedIndex;
            GridViewRow row = ClientsData.Rows[selectedRow];

            int id = Convert.ToInt32(row.Cells[3].Text);

            //In Jobs.aspx when making the invoice: 

            //Response.Redirect("Invoice.aspx?value=" + id);
            // then in invoice Page_Load you do the following:
            // string JobId = Request.QueryString[value];

            //OR using a session

            //Session["JobId"] = id;
            //Response.Redirect("Invoice.aspx)
            // then in invoice Page_Load you do the following:
            // string JobId = Session["JobID"] as string;
        }

        private void BindGridView()
        {

            string query = "SELECT Client_ID, FName AS 'First Name', LName AS 'Last Name', Phone_Num AS 'Phone Number', Address, Email FROM CLIENT";

            try
            {
                using (con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ClientsData.DataSource = dt;
                    ClientsData.DataBind();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }

            // Bind the DataTable to the GridView

        }

        private void FillIDBox() // Gets the next ID
        {
            string query = "SELECT MAX(Client_ID) FROM CLIENT";
            try
            {
                txtFName.Text = string.Empty;
                txtLName.Text = string.Empty;
                txtPhoneNum.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtEmail.Text = string.Empty;

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

        protected void ClientsData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(ClientsData.DataKeys[e.RowIndex].Value);

            deleteRecord(id);

            BindGridView();
        }

        protected void ClientsData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ClientsData.EditIndex = e.NewEditIndex;
            BindGridView();

            GridViewRow row = ClientsData.Rows[e.NewEditIndex];
            TextBox tbName = (TextBox)row.Cells[4].Controls[0];
            tbName.Focus();
        }

        protected void ClientsData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = ClientsData.Rows[e.RowIndex];

                int id = Convert.ToInt32(ClientsData.DataKeys[e.RowIndex].Value);
                string name = ((TextBox)row.Cells[4].Controls[0]).Text;
                string surname = ((TextBox)row.Cells[5].Controls[0]).Text;
                string number = ((TextBox)row.Cells[6].Controls[0]).Text;
                string wage = ((TextBox)row.Cells[7].Controls[0]).Text;
                string competencies = ((TextBox)row.Cells[8].Controls[0]).Text;

                updateRecord(id, name, surname, number, wage, competencies);

                ClientsData.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
        }

        protected void ClientsData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ClientsData.EditIndex = -1;
            BindGridView();
        }



        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            divError.Visible = false; // Hides errors when searching again
            string search = txtSearch.Text;

            string query = $"SELECT Client_ID, FName AS 'First Name', LName AS 'Last Name', Phone_Num AS 'Phone Number', Address, Email FROM CLIENT WHERE Client_ID LIKE @SearchTerm OR FName LIKE @SearchTerm OR LName LIKE @SearchTerm OR Phone_Num LIKE @SearchTerm OR Address LIKE @SearchTerm OR Email LIKE @SearchTerm";

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
                        ClientsData.DataSource = dt;
                        ClientsData.DataBind();
                    }
                    else
                    {
                        // Show error message if no items are found
                        showError($"No item found for {search}");
                        ClientsData.DataSource = null;
                        ClientsData.DataBind();
                    };

                }
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
        }

        protected void updateRecord(int id, string name, string surname, string number, string wage, string competencies)
        {
            string query = @"UPDATE CLIENT SET FName = @FName, LName = @LName, Phone_Num = @PhoneNum, Address = @Address, Email = @Email WHERE Client_ID = @ClientID";

            try
            {
                using (con = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FName", name);
                        cmd.Parameters.AddWithValue("@LName", surname);
                        cmd.Parameters.AddWithValue("@PhoneNum", number);
                        cmd.Parameters.AddWithValue("@Wage", wage);
                        cmd.Parameters.AddWithValue("@Competencies", competencies);
                        cmd.Parameters.AddWithValue("@ClientID", id);

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

            string query = @"DELETE FROM CLIENT WHERE Client_ID = @ClientID";

            try
            {
                using (con = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ClientID", id);

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
                string jobId = ifChasJob(id);
                showError($"Cannot delete client because they currently issue a Job (ID: {jobId})");

            }
        }

        protected string ifChasJob(int id)
        {
            // Get the id of the job a client is connected (If Client Has Job = ifRhasJob)
            string jobId = "";
            string jobQuery = @"SELECT Job_ID FROM JOB WHERE Client_ID = @Client_ID";

            using (con = new MySqlConnection(connectionString))
            {

                MySqlCommand command = new MySqlCommand(jobQuery, con);
                command.Parameters.AddWithValue("@Client_ID", id);

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
            string query = @"INSERT INTO CLIENT (Client_ID, FName, LName, Phone_Num, Address, Email) 
                 VALUES (@ClientID, @FName, @LName, @PhoneNum, @Wage, @Competencies)";

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        int clientID;
                        string fName = txtFName.Text;
                        string lName = txtLName.Text;
                        string phoneNum = txtPhoneNum.Text;
                        string address = txtAddress.Text;
                        string email = txtEmail.Text;

                        if (!(int.TryParse(txtID.Text, out clientID)))
                        {
                            throw new Exception("Invalid Client ID.");
                        }

                        if (!(Regex.IsMatch(phoneNum, @"^(\+27|0)[6-8][0-9]{8}$")))
                        {
                            throw new Exception("Invalid Phone Number.");
                        }

                        if (!(Regex.IsMatch(email, @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$")))
                        {
                            throw new Exception("Invalid Email Address.");
                        }

                        cmd.Parameters.AddWithValue("@ClientID", txtID.Text);
                        cmd.Parameters.AddWithValue("@FName", fName);
                        cmd.Parameters.AddWithValue("@LName", lName);
                        cmd.Parameters.AddWithValue("@PhoneNum", phoneNum);
                        cmd.Parameters.AddWithValue("@Wage", address);
                        cmd.Parameters.AddWithValue("@Competencies", email);

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
    }
}