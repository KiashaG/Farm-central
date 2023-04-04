using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PROG7311_PART2.Models
{
    public class LoginInfo
    {
        public static string currentuser;
        public string User_Name { get; set; }
        [Required]
        public string User_Password { get; set; }



        // COMPARES STRING VARIBLE OF THE PASSWORD IN THE DATABASE AND CONVERTS INTO BYTES
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));
            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        // STORED USERS NAME AND PASSWORD AND CHECKS TO SEE IF IT IS VAILD OR NOT
        public static Boolean login(string user_name, string user_password)
        {


            string compare = MD5Hash(user_password);



            LogicDAL logic = new LogicDAL();
            List<RegisterInfo> login = new List<RegisterInfo>();
            login = logic.GetAllRegisters().ToList();

            foreach (RegisterInfo log in login)
            {
                if (log.User_Name.Equals(user_name))
                {

                    if (log.User_Password.Equals(compare))
                    {
                        currentuser = user_name;


                        return true;


                    }
                    else
                    {

                        //MessageBox.Show("Invaild user email or password", "INFORMATION", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                }
            }

            // MessageBox.Show("Invaild user email or password", "INFORMATION", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;

        }
    }
}

