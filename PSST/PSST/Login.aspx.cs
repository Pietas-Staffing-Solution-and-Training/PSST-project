using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PSST
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void valUsername_ServerValidate(object source, ServerValidateEventArgs args)
        {

            //Get input from user
            string userInput = args.Value;
            string regexForEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            //check that input contains a string
            if ( stringIsEmpty(userInput)) {
                args.IsValid = false;
                return;
            }

            //Check if the input is an email
            if ( Regex.IsMatch( userInput, regexForEmail ) ) {
                args.IsValid = true;
            } else
            {
                args.IsValid = false;
                return;
            }

        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Get input from user
            string userInput = args.Value;
            string regexForPasswordVal = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

            //Check that password has input
            if (stringIsEmpty(userInput))
            {
                args.IsValid = false;
                return;
            }

            //check that password matches requirements
            if ( Regex.IsMatch( userInput, regexForPasswordVal ) )
            {
                args.IsValid = true;
            } else
            {
                args.IsValid = false;
                return;
            }

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

        }

        //Check if a string is empty
        private bool stringIsEmpty(string input)
        {

            if (!string.IsNullOrEmpty(input))
            {
                return false;
            }

            return true;
        }

    }
}