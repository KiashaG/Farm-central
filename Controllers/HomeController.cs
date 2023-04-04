using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PROG7311_PART2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PROG7311_PART2.Controllers
{
    public class HomeController : Controller
    {

        static RegisterInfo currentuser = new RegisterInfo();


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult ProductListIndex()
        {
            if (Checkpermissions())
            {
                return RedirectToAction("AccessDenied");
            }
            else
            {

                ProductDAL productDAL = new ProductDAL();

                ProductInfoList productInfoList = new ProductInfoList();
                productInfoList.productsList = (List<ProductInfo>)productDAL.GetAllProducts();

                return View(productInfoList);

            }

        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult FilteredBySpecificFarmer(string id)
        {
            if (Checkpermissions())
            {
                return RedirectToAction("AccessDenied");
            }
            else
            {

                ProductDAL productDAL = new ProductDAL();
                List<ProductInfo> productsList = (List<ProductInfo>)productDAL.GetAllProducts();
                ProductInfoList productInfoList = new ProductInfoList();
                productInfoList.productsList = new List<ProductInfo>();
                foreach (var product in productsList )
                {
                    if (product.Users_Name.Equals(id))
                    {
                        productInfoList.productsList.Add(product);
                    }
                }
             

                return View(productInfoList);

            }

        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // REGISTER
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(IFormCollection Registerform)
        {
            string username = Registerform["User_Name"];
            string email = Registerform["User_Email"] ;
            string role = Registerform["User_Role"] ;
            string password = Registerform["User_Password"];




            string converter = LoginInfo.MD5Hash(password);

            RegisterInfo.register(username, email, converter, role);
            LoginInfo.currentuser = username;


            return RedirectToAction("Login");
        }

        // LOGIN
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(IFormCollection loginform)
        {
            string username = loginform["User_Name"];
            string password = loginform["User_Password"];

            LogicDAL logic = new LogicDAL();
            if (LoginInfo.login(username, password))
            {
                LoginInfo.currentuser = username;


                List<RegisterInfo> login = new List<RegisterInfo>();
                login = logic.GetAllRegisters().ToList();

                foreach (RegisterInfo log in login)
                {
                    if (log.User_Name.Equals(username))
                    {

                        Console.WriteLine("PLEASE WORK");
                        currentuser = log;

                        if (log.User_Role.Equals("Employee"))
                        {
                            return RedirectToAction("FarmerIndex");
                        }
                        else
                        {
                            return RedirectToAction("ProductIndex");
                        }

                    }
                }

            }

            // determine if employee or farmer
            // username


            return RedirectToAction("Login");
        }

        // FARMER CONTROLLER
        FarmerDAL farmerDAL = new FarmerDAL();
        LogicDAL logicDAL = new LogicDAL();
        public IActionResult FarmerIndex()
        {
            if (Checkpermissions())
            {
                return RedirectToAction("AccessDenied");

            }
            else
            {
                // GET ALL Farmers
                //List<EmployeeInfo> employeeList = new List<EmployeeInfo>();
                List<RegisterInfo> registerList = new List<RegisterInfo>();
                registerList = logicDAL.GetAllRegisters().ToList();

                List<RegisterInfo> farmerList = new List<RegisterInfo>();
                foreach (var registerUser in registerList)
                {
                    if (registerUser.User_Role.Equals("Farmer"))
                    {
                        farmerList.Add(registerUser);
                    }

                }

                return View(farmerList);
            }
        }

        [HttpGet]
        public IActionResult FarmerCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FarmerCreate([Bind] FarmerInfo objFarm)
        {
            if (ModelState.IsValid)
            {
                farmerDAL.AddFarmer(objFarm);

                return RedirectToAction("FarmerIndex");
            }

            return View(objFarm);
        }

        

        // PRODUCT CONTROLLER
        ProductDAL ProductDAL = new ProductDAL();
        public IActionResult ProductIndex()
        {
            if (Checkpermissions())
            {
                // GET ALL PRODUCTS
                List<ProductInfo> farmerList = new List<ProductInfo>();
                farmerList = ProductDAL.GetAllProducts().ToList();

                List<ProductInfo> productList = new List<ProductInfo>();

                foreach (var farmer in farmerList)
                {
                    if (farmer.Users_Name.Equals(currentuser.User_Name))
                    {
                        productList.Add(farmer);
                    }
                }
                return View(productList);
            }
            else
            {
                return RedirectToAction("AccessDenied"); // DESIGN ERROR PAGE
            }

        }
        [HttpGet]
        public IActionResult ProductCreate()
        {
            if (Checkpermissions())
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied"); // Index
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProductCreate([Bind] ProductInfo objPro)
        {
     
            if (!String.IsNullOrEmpty(objPro.Product_Name)) 
            {
                if (!String.IsNullOrEmpty(objPro.Product_Type))
                    if (!String.IsNullOrEmpty(objPro.Date_Accquired.ToString()))
                    {
                        ProductDAL.AddProduct(objPro, currentuser.User_Name);

                        return RedirectToAction("ProductIndex");
                    }

            }

            return View(objPro);
        }

        // Check permissions of user role

        public Boolean Checkpermissions()
        {
            if (currentuser.User_Role.Equals("Farmer"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
