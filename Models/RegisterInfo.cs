using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PROG7311_PART2.Models
{
    public class RegisterInfo
    {
        public string User_Name { get; set; }
        [Required]
        public string User_Email { get; set; }
        [Required]
        public string User_Password { get; set; }
        [Required]
        public string User_Role { get; set; }

        // RETRIVING DATA FROM ADD REGISTER STORED PROCEDURE
        public static void register(string user_name, string user_email, string user_password, string user_role)
        {

            LogicDAL logic = new LogicDAL();
            RegisterInfo reg = new RegisterInfo();
            reg.User_Name = user_name;
            reg.User_Email = user_email;
            reg.User_Password = user_password;
            reg.User_Role = user_role;

            logic.AddREGISTER(reg);
        }
    }
}

