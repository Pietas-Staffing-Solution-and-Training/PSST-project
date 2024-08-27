using PSST.Resources.lib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MySql.Data.MySqlClient;
using System.Collections;

namespace PSST
{
    public partial class Login : System.Web.UI.Page
    {

        string connString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Comment this in for always successfull connection
                tbUsername.Text = "Ruan";
                tbPassword.TextMode = TextBoxMode.SingleLine;
                tbPassword.Text = "TestThisP@s5W0rD!";
            }
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Get user input
            string userInput = args.Value;

            //Create instance of security class
            security passwordVal = new security();

            //Check if email is valid
            if (passwordVal.isValidPassword(userInput))
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
            int isAdmin, userID = 0;

            //check if emtpy - redundant - CODE SHOULD NOT REACH HERE IF ITS EMPTY
            if ( stringIsEmpty(username) || stringIsEmpty(password) )
            {
                return;
            }

            //Create instance of security class
            security encryptPass = new security();

            //Encrypt password
            string encryptedPassword = encryptPass.encrypt(password);

            //Check if account exists in admin DB
            isAdmin = CountFromDB(username, encryptedPassword);

            //IF not admin - Check if resource
            if ( isAdmin != 1 )
            {
                userID = getUserID(username, encryptedPassword);

            }
            else
            {
                //Valid session username
                Session["username"] = username;

                //Redirect user to dashboard page
                Response.Redirect("~/Dashboard.aspx");
            }

            if ( userID == 0 )
            {
                loginFailed();
                return;
            } else
            {
                //Valid session username
                Session["username"] = username;

                //Set user ID
                Session["userID"] = userID;

                //Redirect user to dashboard page
                Response.Redirect("~/Dashboard.aspx");
            }

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

        //Displays failed to login message
        private void loginFailed()
        {
            lblLoginFailed.Visible = true;
        }

        //Returns number of instances of userInput in DB column
        private int CountFromDB( string USERNAME, string PASSWORD )
        {
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM ADMIN WHERE Username = @cleanUsername AND Password = @cleanPassword";
                    

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        //Add parameters to query
                        cmd.Parameters.AddWithValue("@cleanUsername", USERNAME);
                        cmd.Parameters.AddWithValue("@cleanPassword", PASSWORD);


                        int itemToReturn = Convert.ToInt32(cmd.ExecuteScalar());

                        return itemToReturn;

                    }

                }


            } catch ( SqlException ex )
            {
                //Comment this in for testing
                Console.WriteLine($"Failed: {ex.Message}");

            } catch ( Exception ex )
            {
                //Comment this in for testing
                Console.WriteLine($"Failed: {ex.Message}");
            }

            //Query failed if you get this
            return 0;
        }

        //Get user ID from database
        private int getUserID(string USERNAME, string PASSWORD)
        {

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();

                    string query = "SELECT Resource_ID FROM RESOURCE WHERE FName = @cleanFName AND LName = @cleanLName AND Password = @cleanPassword";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        string[] names = USERNAME.Split(' ');

                        string firstName = names[0];
                        string lastName = names[1];

                        cmd.Parameters.AddWithValue("@cleanFName", firstName);
                        cmd.Parameters.AddWithValue("@cleanLName", lastName);
                        cmd.Parameters.AddWithValue("@cleanPassword", PASSWORD);

                        int itemToReturn = Convert.ToInt32(cmd.ExecuteScalar());

                        return itemToReturn;

                    }

                }

            }
            catch (SqlException ex)
            {
                //Comment this in for testing
                Console.WriteLine($"Failed: {ex.Message}");

            }
            catch (Exception ex)
            {
                //Comment this in for testing
                Console.WriteLine($"Failed: {ex.Message}");
            }

            return 0;
        }

    }
}