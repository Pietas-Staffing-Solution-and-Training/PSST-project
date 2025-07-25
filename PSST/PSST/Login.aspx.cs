﻿/*
CMPG223 2024 Distance Project – Group 1
Stefan Paetzold 45475288
Ruan Koekemoer 33538107
Jurie-Hannes Blom 48652865 
Daniël De Jager 41669436
Gaby Gloss 37562908
 */

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

        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //Get data from user
            string username = tbUsername.Text;
            string password = tbPassword.Text;
            int isAdmin = 0, userID = 0;

            //check if empty - redundant - CODE SHOULD NOT REACH HERE IF ITS EMPTY
            if ( stringIsEmpty(username) || stringIsEmpty(password) )
            {
                return;
            }

            //Create instance of security class
            security encryptPass = new security();

            //Encrypt password
            string encryptedPassword = encryptPass.encrypt(password);

            //Check if username is admin
            if ( CheckNameInDB(username,"ADMIN") )
            {
                isAdmin = CountFromDB(username, encryptedPassword);

                if ( isAdmin == 1 ) 
                {
                    //Valid session username
                    Session["username"] = username;

                    //Redirect user to dashboard page
                    Response.Redirect("~/Dashboard.aspx");

                    return;
                } 
                else
                {
                    //Login failed entirely.
                    loginFailed();
                    return;
                }

            }

            //Check if username is Resource
            if ( CheckNameInDB(username, "RESOURCE") )
            {
                userID = getUserID(username, encryptedPassword);

                if (userID != 0) 
                {
                    //Valid session username
                    Session["username"] = username;

                    //Set user ID
                    Session["userID"] = userID;

                    //Redirect user to dashboard page
                    Response.Redirect("~/Dashboard.aspx");

                    return;
                } 
                else
                {
                    //Login failed entirely.
                    loginFailed();
                    return;
                }

            }

            //Username incorrect
            lblLoginFailed.Text = "Incorrect username, please try again.";
            lblLoginFailed.Visible = true;

        }

        protected void CBshowPassword_CheckedChanged(object sender, EventArgs e)
        {
            //Gets inserted password
            string password = tbPassword.Text;

            if (CBshowPassword.Checked)
            {
                tbPassword.TextMode = TextBoxMode.SingleLine;
            }
            else
            {
                //Makes password field visible
                tbPassword.TextMode = TextBoxMode.Password;
            }

            //Injects password to front end
            tbPassword.Attributes.Add("value", password);
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
            lblLoginFailed.Text = "Incorrect password, please try again";

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

        private bool CheckNameInDB(string input, string tableName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();

                    //Initialise variables
                    string query = "";
                    string firstName = "";
                    string lastName = "";

                    //Adjust query if admin or resource
                    if ( tableName == "ADMIN" )
                    {
                        query = $"SELECT COUNT(*) FROM {tableName} WHERE Username = @safeUsername";
                    } 
                    else if ( tableName == "RESOURCE" )
                    {
                        string[] names = input.Split(' ');

                        firstName = names[0];
                        lastName = names[1];

                        query = $"SELECT COUNT(*) FROM {tableName} WHERE Fname = @safeFirstName AND LName = @safeLastName";
                    }

                    

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {

                        //Adjust query if admin or resource
                        if ( tableName == "ADMIN" )
                        {
                            cmd.Parameters.AddWithValue("@safeUsername", input);
                        } 
                        else if ( tableName == "RESOURCE" )
                        {
                            cmd.Parameters.AddWithValue("@safeFirstName", firstName);
                            cmd.Parameters.AddWithValue("@safeLastName", lastName);
                        }
                        
                        int amountOfHits = Convert.ToInt32(cmd.ExecuteScalar());

                        if (amountOfHits == 1)
                        {
                            return true;
                        }

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


            return false;
        }
    }
}