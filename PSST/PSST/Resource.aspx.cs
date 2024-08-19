using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace PSST
{
    public partial class Resource : System.Web.UI.Page
    {
        MySqlConnection con;
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        protected void ResourceData_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedRow = ResourceData.SelectedIndex;
            GridViewRow row = ResourceData.Rows[selectedRow];

            int id = Convert.ToInt32(row.Cells[3].Text);
            lblWelcome.Text = $"You selected row: {selectedRow} + {id}";

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
            // Create a DataTable with three columns

            
            string query = "SELECT Resource_ID, FName, LName, Phone_Num, Wage, Competencies FROM RESOURCE";

            try
            {
                using (con = new MySqlConnection(connectionString))
                {
                   con.Open();

                    MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ResourceData.DataSource = dt;
                    ResourceData.DataBind();

                    con.Close();
                }
            } catch (Exception ex)
            {
                lblWelcome.Text = ex.Message;
            }

            // Bind the DataTable to the GridView
            
        }

                    //Adding tooltips but messed a bit with the button clicks
        //protected void ResourceData_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    // Add tootip to table icons
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
              
        //        ImageButton selectButton = e.Row.Cells[0].Controls[0] as ImageButton;
        //        if (selectButton != null)
        //        {
        //            selectButton.ToolTip = "Select";
                  
        //        }

              
        //        ImageButton editButton = e.Row.Cells[1].Controls[0] as ImageButton;
        //        if (editButton != null)
        //        {
        //            editButton.ToolTip = "Edit";
                  
        //        }

              
        //        ImageButton deleteButton = e.Row.Cells[2].Controls[0] as ImageButton;
        //        if (deleteButton != null)
        //        {
        //            deleteButton.ToolTip = "Delete";
        //        }
        //    }
        //}

        protected void ResourceData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(ResourceData.DataKeys[e.RowIndex].Value);

            deleteRecord(id);

            BindGridView();  
        }

        protected void ResourceData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ResourceData.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void ResourceData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                //int id = Convert.ToInt32(ResourceData.DataKeys[e.RowIndex].Value);
                GridViewRow row = ResourceData.Rows[e.RowIndex];

                int id = Convert.ToInt32(ResourceData.DataKeys[e.RowIndex].Value);
                string name = ((TextBox)row.Cells[4].Controls[0]).Text;
                string surname = ((TextBox)row.Cells[5].Controls[0]).Text;
                string number = ((TextBox)row.Cells[6].Controls[0]).Text;
                string wage = ((TextBox)row.Cells[7].Controls[0]).Text;
                string competencies = ((TextBox)row.Cells[8].Controls[0]).Text;

                lblWelcome.Text = id + " " + name + " " + surname;

                updateRecord(id, name, surname, number, wage, competencies );

                ResourceData.EditIndex = -1;
                BindGridView();
            } catch(Exception ex)
            {
                lblWelcome.Text = ex.Message;
            }
        }

        protected void ResourceData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ResourceData.EditIndex = -1;  
            BindGridView();  
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {

            string search = txtSearch.Text;
            lblWelcome.Text = search;

           

            string query = $"SELECT Resource_ID, FName, LName, Phone_Num, Wage, Competencies FROM RESOURCE WHERE Resource_ID LIKE @SearchTerm OR FName LIKE @SearchTerm OR LName LIKE @SearchTerm OR Phone_Num LIKE @SearchTerm OR Wage LIKE @SearchTerm OR Competencies LIKE @SearchTerm";

            try
            {
                using (con = new MySqlConnection(connectionString))
                {
                   

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SearchTerm", "%" + search + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ResourceData.DataSource = dt;
                    ResourceData.DataBind();

                   

                }
            }
            catch (Exception ex)
            {
                lblWelcome.Text = ex.Message;
            }

        }

        protected void updateRecord(int id, string name, string surname, string number, string wage, string competencies)
        {
            string query = @"UPDATE RESOURCE SET FName = @FName, LName = @LName, Phone_Num = @PhoneNum, Wage = @Wage, Competencies = @Competencies WHERE Resource_ID = @ResourceID";

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
                lblWelcome.Text = ex.Message;
            }
        }

        protected void deleteRecord(int id)
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
            }
            catch (MySqlException)
            {
               string jobId = ifRhasJob(id);
               lblWelcome.Text = $"Cannot delete resource because they are currently assigned a Job (ID: {jobId})";
                
            }
        }

        protected string ifRhasJob(int id)
        {
            // Get the id of the job a resource is connected (If Resource Has Job = ifRhasJob)
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
    }
}