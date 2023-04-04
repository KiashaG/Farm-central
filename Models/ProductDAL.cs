using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PROG7311_PART2.Models
{
    public class ProductDAL
    {

        string connectionStringDEV = "Data Source=KIASHA-LAPTOP-H;Initial Catalog=FARMERSDB;Integrated Security=True";
       
       
        // GET ALL PRODUCTS
        public IEnumerable<ProductInfo> GetAllProducts()
        {
            List<ProductInfo> productsList = new List<ProductInfo>();
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllProducts", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ProductInfo pro = new ProductInfo();
                    pro.Product_Name = dr["Product_Name"].ToString();
                    pro.Date_Accquired = Convert.ToDateTime(dr["Date_Range"].ToString());
                    pro.Product_Type = dr["Product_Type"].ToString();
                    pro.Users_Name = dr["Users_Name"].ToString();


                    productsList.Add(pro);
                }
                con.Close();
            }
            return productsList;
        }

        // INSERT PRODUCTS
        public void AddProduct(ProductInfo pro, String user_name)
        {
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Product_Name", pro.Product_Name);
                cmd.Parameters.AddWithValue("@Date_Range", pro.Date_Accquired);
                cmd.Parameters.AddWithValue("@Product_Type", pro.Product_Type);
                cmd.Parameters.AddWithValue("@Users_Name", user_name);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        // GET PRODUCT BY ID
        public ProductInfo GetProductById(string Product_ID)
        {
            ProductInfo pro = new ProductInfo();

            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetProductById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@USERS_NAME", Product_ID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    pro.Product_Name = dr["Product_Name"].ToString();
                    pro.Date_Accquired = Convert.ToDateTime(dr["Date_Range"].ToString());
                    pro.Product_Name = dr["Product_Type"].ToString();
                    pro.Users_Name = dr["Users_Name"].ToString();

                }
                con.Close();
            }
            return pro;
        }
    }
}
