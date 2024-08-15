using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/*using System.Security.Cryptography;*/

namespace PSST.Resources.lib
{
    public class security
    {

        public string encrypt(string input)
        {

            try
            {

                using (SHA256 encryption = SHA256.Create())
                {

                    //make string an array of bytes
                    byte[] inputB = Encoding.UTF8.GetBytes(input);

                    //Hash byte array
                    byte[] hashB = encryption.ComputeHash(inputB);

                    //Init string builder
                    StringBuilder hexaString = new StringBuilder();

                    //Convert hashed byte array to hexa string
                    for (int i = 0; i < hashB.Length; i++)
                    {
                        hexaString.Append(hashB[i].ToString("X2"));
                    }

                    //Return encrypted item as string
                    return hexaString.ToString();

                }

            } catch ( CryptographicException ex )
            {
                return $"Encryption failed: {ex.Message}";
            } catch ( Exception ex )
            {
                return $"Encryption failed: {ex.Message}";
            }
            
        }

        public bool isValidPassword( string password )
        {
            //Regex for password:
            /* 
             * 8 chars or more
             * 1 uppercase
             * 1 lowercase
             * 1 numebr
             * 1 special character
             * 
             * */
            string regexForPasswordVal = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

            //Check that password has input
            if ( string.IsNullOrEmpty(password) )
            {
                return false;
            }

            //check that password matches requirements
            if (Regex.IsMatch(password, regexForPasswordVal))
            {
                return true;
            }
            
            return false;
        }

        public bool isValidEmailAddress( string email )
        {
            //Regex for email address
            string regexForEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            //check that input contains a string
            if ( string.IsNullOrEmpty(email) )
            {
                return false;
            }

            //Check if the input is an email
            if (Regex.IsMatch(email, regexForEmail))
            {
                return true;
            }

            return false;
        }

    }
}