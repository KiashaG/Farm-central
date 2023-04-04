using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PROG7311_PART2.Models
{
    public class FarmerDAL
    {
        string connectionStringDEV = "Data Source=KIASHA-LAPTOP-H;Initial Catalog=FARMERDB;Integrated Security=True";
      


        // GET ALL FARMERS
        public IEnumerable<FarmerInfo> GetAllFarmers()
        {
            List<FarmerInfo> farmersList = new List<FarmerInfo>();
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllfarmers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    FarmerInfo farm = new FarmerInfo();
                    farm.Farmer_Name = dr["Farmer_Name"].ToString();



                    farmersList.Add(farm);
                }
                con.Close();
            }
            return farmersList;
        }

        // INSERT FARMERS
        public void AddFarmer(FarmerInfo farm)
        {
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertFarmer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Farmer_Name", farm.Farmer_Name);



                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        // GET FARMER BY ID
        public FarmerInfo GetFarmerById(string Farmer_ID)
        {
            FarmerInfo farm = new FarmerInfo();

            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetFarmerById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FARMER_NAME", Farmer_ID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    farm.Farmer_Name = dr["Farmer_Name"].ToString();


                }
                con.Close();
            }
            return farm;
        }
    }
}
