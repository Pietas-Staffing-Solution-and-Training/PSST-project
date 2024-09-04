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
using System.Collections.Generic;

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

                string css = "#editIcons { visibility: hidden; }</style>"; // Hides the icon buttons
                ClientScript.RegisterStartupScript(this.GetType(), "hideImageInput", css, false);
            }

            if (!IsPostBack)
            {
                BindGridView();
                FillIDBox();
                divError.Visible = false;
                PopulateClientDropdown();
                PopulateResourceDropdown();
            }


        }

        protected void JobData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!admin)
            {
                // Find the Edit button in the CommandField
                CommandField invoiceField = (CommandField)JobData.Columns[0];
                CommandField editField = (CommandField)JobData.Columns[1];
                ImageButton deleteButton = (ImageButton)e.Row.FindControl("DeleteButton");

                if (invoiceField != null)
                {
                    invoiceField.ShowSelectButton = false;
                }
                if (editField != null)
                {
                    editField.ShowEditButton = false;
                }
                if (deleteButton != null)
                {
                    deleteButton.Visible = false;
                }
            }
        }

        protected void JobData_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = null;
            if (JobData.SelectedRow != null)
            {
                row = JobData.SelectedRow;
            }
            //int selectedRow = JobData.SelectedIndex;
            int selectedRow = 2;
            //row = JobData.Rows[selectedRow];

            int id = Convert.ToInt32(row.Cells[3].Text);

            //int id = 1;

            if (admin)
            {
                lblWelcome.Text = admin.ToString();
                Session["Job_Id"] = id;
                Response.Redirect("InvoiceForm.aspx");
            }
            else
            {
                showError("You cannot create an invoice for this Job. Contact Admin for assistance.");
            }
        }

        private void BindGridView(string optQuery = "")
        {
            string query;

            if (admin)
            {
                //query = "SELECT Job_ID, Status, Description, Resource_ID, Client_ID, ROUND(Budget, 2) AS 'Budget' FROM JOB";
                query = $@"
                    SELECT 
                        j.Job_ID, 
                        j.Status, 
                        j.Description, 
                        CONCAT(r.FName, ' ', r.LName) AS 'Resource Name',
                        j.Resource_ID,
                        CONCAT(c.FName, ' ', c.LName) AS 'Client Name', 
                        j.Client_ID,
                        ROUND(j.Budget, 2) AS 'Budget', 
                        i.Hours_Worked AS 'Hours Worked'
                    FROM 
                        JOB j
                    LEFT JOIN
                        INVOICE i ON j.Job_ID = i.Job_ID
                    LEFT JOIN
                        RESOURCE r ON j.Resource_ID = r.Resource_ID
                    LEFT JOIN
                        CLIENT c ON j.Client_ID = c.Client_ID;
                ";

                if (optQuery.Length > 0)
                {
                    query = optQuery;
                }
            }
            else
            {
                //query = $"SELECT Job_ID, Status, Description, Resource_ID, Client_ID, ROUND(Budget, 2) AS 'Budget' FROM JOB WHERE Resource_ID = '{user_ID}'";
                query = $@"
                    SELECT 
                        j.Job_ID, 
                        j.Status, 
                        j.Description, 
                        CONCAT(r.FName, ' ', r.LName) AS 'Resource Name',
                        j.Resource_ID,
                        CONCAT(c.FName, ' ', c.LName) AS 'Client Name', 
                        j.Client_ID,
                        ROUND(j.Budget, 2) AS 'Budget', 
                        i.Hours_Worked AS 'Hours Worked'
                    FROM 
                        JOB j
                    LEFT JOIN
                        INVOICE i ON j.Job_ID = i.Job_ID
                    LEFT JOIN
                        RESOURCE r ON j.Resource_ID = r.Resource_ID
                    LEFT JOIN
                        CLIENT c ON j.Client_ID = c.Client_ID
                    WHERE 
                        j.Resource_ID = '{user_ID}';
                ";

                if (optQuery.Length > 0)
                {
                    query = optQuery;
                }
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

        // Run this in the page_load to delete a job
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
            catch (MySqlException)
            {

            }
        }

        protected void JobData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (admin)
            {
                int id = Convert.ToInt32(JobData.DataKeys[e.RowIndex].Value);

                completeRecord(id);

                BindGridView();
            } else
            {
                showError("You cannot edit this record. Contact Admin for assistance.");
            }
        }

        protected void JobData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (admin)
            {
                JobData.EditIndex = e.NewEditIndex;
                BindGridView();

                GridViewRow row = JobData.Rows[e.NewEditIndex];
                TextBox tbName = (TextBox)row.Cells[4].Controls[0];
                tbName.Focus();

            } else
            {
                showError("You cannot edit this record. Contact Admin for assistance.");
            }
}

        protected void JobData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = JobData.Rows[e.RowIndex];

                jobID = Convert.ToInt32(row.Cells[3].Text);
                string status = row.Cells[4].Text;
                string description = ((TextBox)row.Cells[5].Controls[0]).Text;
                int resourceID = convertStringToInt( ( (TextBox)row.Cells[7].Controls[0]).Text);
                int clientID = convertStringToInt( ( (TextBox)row.Cells[9].Controls[0]).Text);
                decimal budget = convertStringToDecimal(((TextBox)row.Cells[10].Controls[0]).Text);

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

            //string query = $"SELECT Job_ID, Description, Resource_ID, Client_ID, ROUND(Budget, 2) AS Budget FROM JOB WHERE Job_ID LIKE @SearchTerm OR Description LIKE @SearchTerm OR Resource_ID LIKE @SearchTerm OR Client_ID LIKE @SearchTerm OR Budget LIKE @SearchTerm";
            /*string query = $@"
                SELECT 
                    j.Job_ID, 
                    j.Status, 
                    j.Description, 
                    j.Resource_ID, 
                    j.Client_ID, 
                    ROUND(j.Budget, 2) AS 'Budget', 
                    i.Hours_Worked as 'Hours Worked'
                FROM 
                    JOB j
                LEFT JOIN 
                    INVOICE i ON j.Job_ID = i.Job_ID
                WHERE 
                    j.Job_ID LIKE @SearchTerm 
                    OR j.Description LIKE @SearchTerm 
                    OR j.Resource_ID LIKE @SearchTerm 
                    OR j.Client_ID LIKE @SearchTerm 
                    OR ROUND(j.Budget, 2) LIKE @SearchTerm;
            ";*/
            string query = $@"
                SELECT 
                    j.Job_ID, 
                    j.Status, 
                    j.Description, 
                    CONCAT(r.FName, ' ', r.LName) AS 'Resource Name',
                    j.Resource_ID,
                    CONCAT(c.FName, ' ', c.LName) AS 'Client Name', 
                    j.Client_ID,
                    ROUND(j.Budget, 2) AS 'Budget', 
                    i.Hours_Worked AS 'Hours Worked'
                FROM 
                    JOB j
                LEFT JOIN 
                    INVOICE i ON j.Job_ID = i.Job_ID
                LEFT JOIN 
                    RESOURCE r ON j.Resource_ID = r.Resource_ID
                LEFT JOIN 
                    CLIENT c ON j.Client_ID = c.Client_ID
                WHERE 
                    j.Job_ID LIKE @SearchTerm 
                    OR j.Description LIKE @SearchTerm 
                    OR j.Resource_ID LIKE @SearchTerm 
                    OR j.Client_ID LIKE @SearchTerm 
                    OR ROUND(j.Budget, 2) LIKE @SearchTerm
                    OR CONCAT(r.FName, ' ', r.LName) LIKE @SearchTerm
                    OR CONCAT(c.FName, ' ', c.LName) LIKE @SearchTerm;
            ";

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

        protected void completeRecord(int id)
        {

            string query = @"UPDATE JOB SET Status = 'Complete' WHERE Job_ID = @JobID";

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


                        adapter.UpdateCommand = cmd;
                        adapter.UpdateCommand.ExecuteNonQuery();

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
            string query;
            int jobCount;
            int jobID;
            string description = txtDescription.Text;
            int resourceID;
            int clientID;
            decimal budget;

            try
            {

                if (!(int.TryParse(txtJobID.Text, out jobID)))
                {
                    throw new Exception("Invalid Job ID.");
                }

                if (!(int.TryParse(ddlResource.SelectedValue, out resourceID)))
                {
                    throw new Exception("Invalid Resource ID.");
                }

                //if (!(int.TryParse(txtResourceID.Text, out resourceID)))
                //{
                //    throw new Exception("Invalid Resource ID.");
                //}

                if (!(int.TryParse(ddlClient.SelectedValue, out clientID)))
                {
                    throw new Exception("Invalid Client ID.");
                }

                //if (!(int.TryParse(txtClientID.Text, out clientID)))
                //{
                //    throw new Exception("Invalid Client ID.");
                //}

                if (!(decimal.TryParse(txtBudget.Text, out budget)))
                {
                    throw new Exception("Invalid Wage.");
                }


                

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    query = @"SELECT COUNT(*) FROM JOB WHERE Resource_ID = @ResourceID";

                    MySqlCommand command = new MySqlCommand(query, con);
                    command.Parameters.AddWithValue("@ResourceID", resourceID);

                    con.Open();
                    jobCount = Convert.ToInt32(command.ExecuteScalar());
                    con.Close();


                    if (jobCount < 1)
                    {
                        query = @"INSERT INTO JOB (Description, Resource_ID, Client_ID,Budget) 
                         VALUES (@Description, @ResourceID, @ClientID, @Budget)";

                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                     
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
                    else
                    {
                        throw new Exception("Cannot have a resource on more than 1 job");
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

        protected void PopulateClientDropdown() // Makes selecting client easier by showing name then parsing to ID later
        {
            string query = "SELECT Client_ID, CONCAT(FName, ' ', LName, ' (', Client_ID, ')') AS FullName FROM CLIENT";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                con.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) // Create a ListItem for each client
                    {
                        string clientId = reader["Client_ID"].ToString();
                        string fullName = reader["FullName"].ToString();

                        ListItem item = new ListItem(fullName, clientId);
                        ddlClient.Items.Add(item);
                    }
                }

                con.Close();
            }
        }

        protected void PopulateResourceDropdown() // Makes selecting resource easier by showing name then parsing to ID later
        {
            string query = "SELECT Resource_ID, CONCAT(FName, ' ', LName, ' (', Resource_ID, ')') AS FullName FROM RESOURCE";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                con.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) // Create a ListItem for each client
                    {
                        string clientId = reader["Resource_ID"].ToString();
                        string fullName = reader["FullName"].ToString();

                        ListItem item = new ListItem(fullName, clientId);
                        ddlResource.Items.Add(item);
                    }
                }

                con.Close();
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
            int numRows = JobData.Rows.Count;
            GridViewRow row = null;
            bool found = false;

            for(int i = 0; i < numRows; i++)
            {
                row = JobData.Rows[i];
                if (row.Cells[4].Text == "Active")
                {
                    found = true;
                }
            }

            if (!found)
            {
                showError("No active job found to edit time.");
                return;
            }

            string jobID = row.Cells[3].Text;
            float hours_worked;
            if (!(float.TryParse(txtTime.Text, out hours_worked))) {
                showError("Insert a valid time (in hours).");
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