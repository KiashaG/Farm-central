using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PROG7311_PART2.Models
{
    public class LogicDAL
    {
        string connectionStringDEV = "Data Source=KIASHA-LAPTOP-H;Initial Catalog=FARMERSDB;Integrated Security=True";



        // GET REGISTER BY ID
        public RegisterInfo GetRegisterById(string Register_ID)
        {
            RegisterInfo reg = new RegisterInfo();

            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetRegisterById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@REGISTERID", Register_ID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    reg.User_Name = dr["USERS_NAME"].ToString();
                    reg.User_Email = dr["USERS_EMAIL"].ToString();
                    reg.User_Role = dr["USERS_ROLES"].ToString();
                    reg.User_Password = dr["USERS_PASSWORD"].ToString();

                }
                con.Close();
            }
            return reg;
        }

        // GET ALL REGISTERS
        public IEnumerable<RegisterInfo> GetAllRegisters()
        {
            List<RegisterInfo> registerList = new List<RegisterInfo>();
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllRegisters", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    RegisterInfo reg = new RegisterInfo();

                    reg.User_Name = dr["USERS_NAME"].ToString();
                    reg.User_Role = dr["USERS_ROLE"].ToString();
                    reg.User_Password = dr["USERS_PASSWORD"].ToString();


                    registerList.Add(reg);
                }
                con.Close();
            }
            return registerList;
        }

        // INSERT REGISTER
        public void AddREGISTER(RegisterInfo reg)
        {
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertRegisters", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@USERS_NAME", reg.User_Name);
                cmd.Parameters.AddWithValue("@USERS_EMAIL", reg.User_Email);
                cmd.Parameters.AddWithValue("@USERS_PASSWORD", reg.User_Password);
                cmd.Parameters.AddWithValue("@USERS_ROLE", reg.User_Role);


                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }


    }
}
