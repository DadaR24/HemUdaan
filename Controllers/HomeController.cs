
using HEMUdaan.EMail;
using HEMUdaan.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HEMUdaan.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        // Define AppDbContext only once
        public class AppDbContext : DbContext
        {
            public AppDbContext()
                : base("name=DefaultConnection") // You may want to use a config value instead of hardcoding
            {
            }

            public DbSet<User> Users { get; set; }
            public DbSet<Course> Courses { get; set; }
            public DbSet<Topics> Topics { get; set; }
            public DbSet<SyllabusModel> HF_SurveyData { get; set; }
            public DbSet<HF_MCQData> HF_MCQData { get; set; }
            public DbSet<ExamResponse> MCQResponse { get; set; }
            public DbSet<WJ_Practical> WJ_Practicals { get; set; }
            public DbSet<QuestionViewModel> Questions { get; set; }
            public DbSet<Volunteer> Volunteers { get; set; }
            public IEnumerable<UserTopicModel> UserTopics { get; internal set; }

            protected virtual void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Ignore<SelectListItem>();
            }
        }

        public ActionResult Index() => View();

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            User model = new User();
            model.CityList = GetCityList();
            model.StdList = GetStdList();
            model.stateList = GetStateList();
            return View(model);
          
        }


        public static Dictionary<string, string> GetCityList()
        {
            Dictionary<string, string> cityList = new Dictionary<string, string>
            {
            { "", "-- Select City --" } // Default option
           };

            string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("HF_SP_MAS_GetCities", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string cityName = reader.GetString(0);
                            if (!cityList.ContainsKey(cityName)) // Prevent duplicate entries
                            {
                                cityList.Add(cityName, cityName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error if needed (consider using a logging framework)
                Console.WriteLine("Error fetching cities: " + ex.Message);
            }

            return cityList;
        }

        public static Dictionary<string, string> GetStateList()
        {
            Dictionary<string, string> stateList = new Dictionary<string, string>
            {
            { "", "-- Select State --" } // Default option
           };

            string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("HF_SP_MAS_Getstates", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader.GetString(0);
                            if (!stateList.ContainsKey(name)) // Prevent duplicate entries
                            {
                                stateList.Add(name, name);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error if needed (consider using a logging framework)
                Console.WriteLine("Error fetching cities: " + ex.Message);
            }

            return stateList;
        }
        public static Dictionary<string, string> GetStdList()
        {
            Dictionary<string, string> stdList = new Dictionary<string, string>
            {
            { "", "-- Select Standard --" } // Default option
           };

            string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("HF_SP_MAS_GetStd", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Assuming the first column is an integer representing the Standard ID
                            int ID = reader.GetInt32(0);
                            // Assuming the second column is a string representing the Standard name
                            string Standards = reader.GetString(1);

                            // Adding the standard ID and name to the dictionary (ID as key, name as value)
                            if (!stdList.ContainsKey(ID.ToString())) // Prevent duplicate entries
                            {
                                stdList.Add(Standards, Standards);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error if needed (consider using a logging framework)
                Console.WriteLine("Error fetching cities: " + ex.Message);
            }

            return stdList;

        }

        [HttpPost]
        public ActionResult Register(User obj, HttpPostedFileBase FilePath)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            try
            {
                // Check if the mobile number already exists
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string checkMobileQuery = "SELECT COUNT(1) FROM Users WHERE MobileNumber = @MobileNumber";
                    using (var cmd = new SqlCommand(checkMobileQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);

                        int mobileCount = Convert.ToInt32(cmd.ExecuteScalar());

                        if (mobileCount > 0)
                        {
                            ModelState.AddModelError("MobileNumber", "Mobile number already exists.");
                            return View(obj);
                        }
                    }
                }

                // File upload handling
                string filePath = null;
                if (FilePath != null && FilePath.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(FilePath.FileName);
                    string directoryPath = "~/Content/Payment Slips";
                    filePath = Path.Combine(directoryPath, fileName);
                    FilePath.SaveAs(Server.MapPath(filePath));
                }

                // Insert user data using stored procedure
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("Sp_RegisterUser", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        cmd.Parameters.AddWithValue("@FirstName", obj.FirstName);
                        cmd.Parameters.AddWithValue("@MiddleName", obj.MiddleName);
                        cmd.Parameters.AddWithValue("@LastName", obj.LastName);
                        cmd.Parameters.AddWithValue("@GuardianName", obj.GuardianName);
                        cmd.Parameters.AddWithValue("@Std", obj.Std);
                        cmd.Parameters.AddWithValue("@SchoolName", obj.SchoolName);
                        cmd.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);
                        cmd.Parameters.AddWithValue("@EmergencyContact", obj.EmergencyContact);
                        cmd.Parameters.AddWithValue("@EmailID", obj.EmailID);
                        cmd.Parameters.AddWithValue("@Location", obj.Location);
                        cmd.Parameters.AddWithValue("@City", obj.City);
                        cmd.Parameters.AddWithValue("@state", obj.State);
                        cmd.Parameters.AddWithValue("@Username", obj.MobileNumber);
                        cmd.Parameters.AddWithValue("@Password", obj.MobileNumber); // Consider hashing the password
                        cmd.Parameters.AddWithValue("@Pincode", obj.Pincode);
                        cmd.Parameters.AddWithValue("@Specify", obj.Specify);
                        cmd.Parameters.AddWithValue("@FilePath", filePath);  // Save file path if any
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UserType", obj.UserType);

                        // Execute the stored procedure
                        cmd.ExecuteNonQuery();
                    }
                }

                // SweetAlert Success Message
                TempData["UseSweetAlert7"] = true;
                TempData["SweetAlertTitle"] = "Success";
                TempData["SweetAlertMessage"] = $"{obj.FirstName} Registered Successfully";
                TempData["SweetAlertType"] = "success";


                // Generate and send email after successful submission
                StringBuilder body = new StringBuilder(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\template\\HVMailTemplates\\UserRegistration.html"));
                body.Replace("<%FirstName%>", obj.FirstName);
                body.Replace("<%UserName%>", obj.MobileNumber);
                body.Replace("<%Password%>", obj.MobileNumber);
                EMailManager.SendEmailAws(obj.EmailID, "User Registration", body.ToString(), null, "", "");
              //  SMSManager.UserRegistration(obj.MobileNumber, obj.FirstName, obj.Username, obj.Password);

                return RedirectToAction("SweetAlert", "Home");
            }
            catch (Exception ex)
            {
                // Handle error (logging, etc.)
                return View("Error", new HandleErrorInfo(ex, "User", "Register"));
            }
        }

        public ActionResult Dashboard()
        {
            TempData["UserID"] = TempData["UserID"]; // Assuming UserID is already set somewhere else
            return View();
        }

        public ActionResult SweetAlert(int ?UserID,int?CourseID)
        {
            ViewBag.UserID = UserID;  // Pass UserID to View
            ViewBag.CourseID = CourseID;
            return View();
        }


        public ActionResult ExamSummary() => View();

        public ActionResult LogOffUdaan()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
      
        public ActionResult LogOffUdaan(string CompetitionButton)
        {
            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now);
                Response.Cache.SetNoServerCaching();
                Response.Cache.SetNoStore();
                Logout();
                //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                //HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                //Response.Cookies.Clear();
                SessionUser.Flush();

               


                return RedirectToAction("Index", "Home");

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}