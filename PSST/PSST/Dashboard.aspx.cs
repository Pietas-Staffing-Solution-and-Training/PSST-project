using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace PSST
{
    public partial class Dashboard : System.Web.UI.Page
    {
        int selectedID;
        string username = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            bool isAdmin = false;

            // Get session value - returns null if doesn't exist
            string username = Session["username"]?.ToString();

            if (!IsPostBack)
            {
                lblWelcome.Text = $"Welcome to PSST, {username}";
                btnClients.Visible = false;
            }

            // If username is null
            if (username == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (Session["userID"] == null)
            {
                isAdmin = true;
            }

            if (isAdmin)
            {
                btnClients.Visible = true;
            }
            else
            {
                btnClients.Visible = false;
            }
        }
    }
}