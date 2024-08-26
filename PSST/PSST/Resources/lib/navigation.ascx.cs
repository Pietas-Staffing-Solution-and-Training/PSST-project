using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PSST.Resources.lib
{
    public partial class navigation : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            // Remove the username from the session
            Session["username"] = null;

            // Optionally abandon the session to clear all session data
            Session.Abandon();

            // Redirect to the login page
            Response.Redirect("~/Login.aspx");
        }
    }
}