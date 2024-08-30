using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PSST
{
    public partial class Help : System.Web.UI.Page
    {
        bool isAdmin;
        protected void Page_Load(object sender, EventArgs e)
        {
            isAdmin = false;

            string username = Session["username"]?.ToString();

            if (!IsPostBack)
            {
                notlogged.Visible = false;
            }

            if (Session["userID"] == null)
            {
                isAdmin = true;
            }

            // If username is null
            if (username == null)
            {
                notlogged.Visible = true;
                loggedAdmin.Visible = false;
            }

            if (isAdmin)
            {
                loggedAdmin.Visible = true;
            }
            else
            {
                loggedAdmin.Visible = false;
                loggedUser.Visible = true;
            }
        }
    }
}