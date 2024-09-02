using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PSST.Resources.lib
{
    public partial class footer : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            footerClients.Visible = false;

            if (Session["username"] == null)
            {
                footer_Navigation.Visible = false;
                return;
            }
            if (Session["userID"] == null)
            {
                footerClients.Visible = true;
            }
        }
    }
}