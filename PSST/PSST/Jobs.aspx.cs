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
    public partial class Jobs : System.Web.UI.Page
    {
        MySqlConnection con;
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

            ////tbUsername.Text = "Ruan@email.com";
            ////tbPassword.Text = "TestThisP@s5W0rD!";

            ////Get session value - returns null if doesn't exist
            //string username = Session["username"]?.ToString();
            //string type = Session["type"]?.ToString();
            //type = "admin"; // REMOVE IN PRODUCTION

            ////If string is null
            //if (username == null)
            //{
            //    Response.Redirect("Login.aspx");
            //    return;
            //}

            //if (type == "admin")
            //{

            //}
            //else
            //{
            //    adminPanel.Visible = false;
            //}

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

            Session["JobId"] = id;
            Response.Redirect("InvoiceForm.aspx");
             //then in invoice Page_Load you do the following:
             //string JobId = Session["JobID"] as string;
        }

        private void BindGridView()
        {

            string query = "SELECT * FROM JOB";

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

            // Bind the DataTable to the GridView

        }

        private void FillIDBox() // Gets the next ID
        {
            string query = "SELECT MAX(Job_ID) FROM JOB";
            try
            {
                txtResourceID.Text = string.Empty;
                txtClientID.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtBudget.Text = string.Empty;
                txtRequiredResources.Text = string.Empty;

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

                int jobID = Convert.ToInt32(row.Cells[3].Text);
                string description = ((TextBox)row.Cells[4].Controls[0]).Text;

                int resourceID = convertStringToInt( ( (TextBox)row.Cells[5].Controls[0]).Text);


                int clientID = convertStringToInt( ( (TextBox)row.Cells[6].Controls[0]).Text);
                decimal budget = convertStringToDecimal(((TextBox)row.Cells[7].Controls[0]).Text);

                updateRecord(jobID, description, resourceID, clientID, budget);

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

        protected void updateRecord(int jobID, string description, int resourceID, int clientID, decimal budget)
        {
            string query = @"UPDATE JOB SET Description = @Description, Resource_ID = @ResourceID, Client_ID = @ClientID, Budget = @Budget WHERE Job_ID = @JobID";

            try
            {
                using (con = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {

                        cmd.Parameters.AddWithValue("@JobID", jobID);                        
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@ResourceID", resourceID);
                        cmd.Parameters.AddWithValue("@ClientID", clientID);
                        cmd.Parameters.AddWithValue("@Budget", budget);
                        //cmd.Parameters.AddWithValue("@RequiredResources", requiredResources);
                        

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
            string query = @"INSERT INTO JOB (Job_ID, Description, Resource_ID, Client_ID, Budget, Required_Resources) 
                 VALUES (@JobID, @Description, @ResourceID, @ClientID, @Budget, @RequiredResources)";

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
                        string required_resources = txtRequiredResources.Text;

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
                        cmd.Parameters.AddWithValue("@RequiredResources", required_resources);

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

        protected void txtResourceID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}