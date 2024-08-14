using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using MySql.Data.MySqlClient;

namespace PSST
{
    public partial class Dashboard : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlDataAdapter adapter;
        SqlCommand comm;
        DataTable dataTable;
        string sql;
        int selectedID;
        string connectionString = "Server=sql15.cpt2.host-h.net;Database=Ruans_Testing;User Id=onlingxwzp_35;Persist Security Info=True;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadClientData();
            }
        }

        protected void ClientData_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedRow = ClientData.SelectedIndex;
            lblWelcome.Text = $"You selected row: {selectedRow}";
        }

        private void LoadClientData()
        {
            string connectionString = "Server=sql15.cpt2.host-h.net;Database=Ruans_Testing;User Id=onlingxwzp_35;Password=SvJv71kTn519WDCTreS8; Persist Security Info=True;";
            string query = "SELECT * FROM CLIENT";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ClientData.DataSource = dt;
                    ClientData.DataBind();
                }
            }
        }

        protected void ClientData_EditRow(object sender, GridViewEditEventArgs e)
        {

        }
    }
}