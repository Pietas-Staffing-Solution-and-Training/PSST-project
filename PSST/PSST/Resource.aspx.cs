using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System;

namespace PSST
{
    public partial class Resource : System.Web.UI.Page
    {
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
            lblWelcome.Text = $"You selected row: {selectedRow}";
        }

        private void BindGridView()
        {
            // Create a DataTable with three columns
            DataTable dt = new DataTable();
            dt.Columns.Add("Resource ID");
            dt.Columns.Add("First Name");
            dt.Columns.Add("Last Name");

            // Add some sample data rows
            dt.Rows.Add("1", "John", "Smith");
            dt.Rows.Add("2", "Jane", "Carlisle");
            dt.Rows.Add("3", "Joe", "Mama");

            // Bind the DataTable to the GridView
            ResourceData.DataSource = dt;
            ResourceData.DataBind();
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

            
            //DeleteRecord(id);

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

                lblWelcome.Text = id + " " + name + " " + surname;

                //UpdateRecord(id, surname, age);

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
    }
}