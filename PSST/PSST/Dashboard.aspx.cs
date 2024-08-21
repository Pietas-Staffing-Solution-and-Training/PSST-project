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
            //Get session value - returns null if doesn't exist
            string username = Session["username"]?.ToString();

            //If string is null
            if (username == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblWelcome.Text = $"Welcome to PSST, {username}";
            }
        }
    }
}