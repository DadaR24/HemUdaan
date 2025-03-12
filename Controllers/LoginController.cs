

using HEMUdaan.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
namespace HEMUdaan.Controllers
{
    public class LoginController : Controller
    {

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
        //public ActionResult Index() => (ActionResult)this.View();

        public ActionResult Index()
        {
            var user = new User(); 
            Session["LoggedInUser"] = user;
            return View();
        }

        [HttpPost]
        public ActionResult Index(User model)
        {

            
            // Check if the user is an admin
            if (model.Username == "7208111337")
            {

                // return RedirectToAction("Index", "Home");

                return RedirectToAction("Index", "Admin");
            }

            // Authenticate the user
            User user = AuthenticateUser(model.Username, model.Password);
            Session["LoggedInUser"] = user;
            if (user != null)
            {
                int userId = user.UserID;

                // Store user data in TempData and Session
                TempData["UserID"] = userId;
                Session["Username"] = user.Username;
                Session["UserID"] = user.UserID;
               /// Session["UserID"] = 
                if(user.UserType =="User")
                {
                    return RedirectToAction("Index", "User", new { UserID = userId });
                }
                else if (user.UserType == "Volunteer")
                {
                    return RedirectToAction("Index", "Volunteer", new { UserID = userId });

                }
                //else
                //{
                //    return RedirectToAction("Index", "User", new { UserID = userId });

                //}
                // Define the list of usernames for specific redirection
               // var specialUsernames = new List<string>
               // {
               //   "9870002011", "9820348870", "9819540279", "8076608526",
               //   "9892104311", "9892727705", "8971799230", "7318248634",
               //   "9837007165", "9619916613", "9324024888", "8169933685"
               //};

               // // Check if the username is in the special user list and redirect accordingly
               // if (specialUsernames.Contains(model.Username))
               // {
               //     return RedirectToAction("Index", "Volunteer", new { UserID = userId });
               // }

               // // Default redirect for regular users
               // return RedirectToAction("Index", "User", new { UserID = userId });
            }

            // Invalid username or password
            ModelState.AddModelError("", "Invalid username or password");
            return View(model);
        }


        public User AuthenticateUser(string username, string password)
        {
            // Define the connection string
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            // Initialize the user to return
            User user = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Define the stored procedure and parameters
                SqlCommand command = new SqlCommand("sp_AuthenticateUser", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add parameters to the command
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                // Open the connection
                connection.Open();

                // Execute the command and read the result
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        // Read the first row if available
                        reader.Read();
                        user = new User
                        {
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                          
                            Username = reader.GetString(reader.GetOrdinal("Username")),
                            Password = reader.GetString(reader.GetOrdinal("Password")),
                            UserType = reader.GetString(reader.GetOrdinal("UserType"))
                        };
                    }
                }
            }

            return user;
        }


    }
}
