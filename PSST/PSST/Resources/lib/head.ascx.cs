﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuestPDF.Infrastructure;

namespace PSST.Resources.lib
{
    public partial class head : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                QuestPDF.Settings.License = LicenseType.Community;
            }
        }
    }
}