using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace PSST
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        protected void ClientData_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedRow = ClientData.SelectedIndex;
            //int selectedID = (int)ClientData.DataKeys[ClientData.SelectedIndex].Value;
            lblWelcome.Text = $"You selected row: {selectedRow}";
        }

        private void BindGridView()
        {
            // Create a DataTable with three columns
            DataTable dt = new DataTable();
            dt.Columns.Add("Client_ID");
            dt.Columns.Add("FName");
            dt.Columns.Add("LName");

            // Add some sample data rows
            dt.Rows.Add("1", "John", "Smith");
            dt.Rows.Add("2", "Jane", "Carlisle");
            dt.Rows.Add("3", "Joe", "Mama");

            // Bind the DataTable to the GridView
            ClientData.DataSource = dt;
            ClientData.DataBind();
        }

        protected void ClientData_EditRow(object sender, GridViewEditEventArgs e)
        {

        }
    }
}