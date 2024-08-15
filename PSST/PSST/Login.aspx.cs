using PSST.Resources.lib;
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
            //Get user input
            string userInput = args.Value;

            //Create instance of security class
            security usernameVal = new security();

            //Check if email is valid
            if ( usernameVal.isValidEmailAddress( userInput ) )
            {
                args.IsValid = true;
            } else
            {
                args.IsValid = false;
                return;
            }

        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Get user input
            string userInput = args.Value;

            //Create instance of security class
            security passwordVal = new security();

            //Check if email is valid
            if (passwordVal.isValidEmailAddress(userInput))
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
                return;
            }

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            //Get data from user
            string username = tbUsername.Text;
            string password = tbPassword.Text;

            //check if emtpy - redundant - CODE SHOULD NOT REACH HERE IF ITS EMPTY
            if ( stringIsEmpty(username) || stringIsEmpty(password) )
            {
                return;
            }

            //Create instance of security class
            security encryptPass = new security();

            //Encrypt password
            string encryptedPassword = encryptPass.encrypt(password);

            

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