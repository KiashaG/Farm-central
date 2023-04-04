using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PROG7311_PART2.Models
{
    public class ProductInfo
    {
        public string Product_Name { get; set; }
        [Required]
        public DateTime Date_Accquired { get; set; }
        [Required]
        public string Product_Type { get; set; }
        [Required]
        public string Users_Name { get; set; }
    }

    public class ProductInfoList
    {
       public  List<ProductInfo> productsList { get; set; }
    }
}

