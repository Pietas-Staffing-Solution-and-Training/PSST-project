using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PSST.Resources.lib
{
    public partial class navigation : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isAdmin = false;
            HtmlGenericControl clientsLink = (HtmlGenericControl)topPanel.FindControl("navclients"); // Find the Clients nav link

            if (!IsPostBack)
            {
                //
            }

            if (Session["username"] == null)
            {
                clientsLink.Visible = false;
                logoutButton.Visible = false;
                nav_Dashboard.Visible = false;
                nav_Jobs.Visible = false;
                nav_Resource.Visible = false;
                return;
            }

            if (Session["userID"] == null)
            {
                isAdmin = true;
            }

            if (isAdmin)
            {
                clientsLink.Visible = true;
            }
            else
            {
                clientsLink.Visible = false;
            }
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            // Remove the username from the session
            Session["username"] = null;
            Session["userID"] = null;

            // Optionally abandon the session to clear all session data
            Session.Abandon();

            // Redirect to the login page
            Response.Redirect("~/Login.aspx");
        }

        protected void Help_Click(object sender, EventArgs e)
        {
            // Redirect to the help page
            Response.Redirect("~/Help.aspx");
        }
    }
}