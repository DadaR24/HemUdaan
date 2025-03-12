// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Controllers.UserController
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using ExcelDataReader;
using HEMUdaan.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
namespace HEMUdaan.Controllers
{
  public class UserController : Controller
  {
    private readonly string _connectionString;

    public UserController()
    {
      this._connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }

        //public ActionResult Index(int? UserId)
        //{
        //    if (TempData.ContainsKey("UserID"))
        //        TempData["UserID"] = UserId ?? (int)TempData["UserID"];
        //    else
        //        TempData["UserID"] = UserId;

        //    UserProfileModel userProfileModel = new UserProfileModel();

        //    using (var appDbContext = new HomeController.AppDbContext())
        //    {
        //        var courseList = appDbContext.Courses
        //                                     .Select(course => new CourseDTO
        //                                     {
        //                                         CourseID = course.CourseID,
        //                                         CourseName = course.CourseName,
        //                                         // Add other properties if needed
        //                                     })
        //                                     .ToList(); // Execute the query and return a list

        //        userProfileModel.Courses = courseList;
        //    }

        //    return View(userProfileModel);
        //}

        public ActionResult Index(int? UserId)
        {
            int? UserID = (int)Session["UserID"];
           

            UserProfileModel userProfileModel = new UserProfileModel();
            List<CourseDTO> courseList = new List<CourseDTO>();

            // Database connection string
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_GetCourses", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Open the connection
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Map the data to CourseDTO
                        var course = new CourseDTO
                        {
                            CourseID = reader.GetInt32(reader.GetOrdinal("CourseID")),
                            CourseName = reader.GetString(reader.GetOrdinal("CourseName")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            FeaturedImage = reader.GetString(reader.GetOrdinal("FeaturedImage")),


                            // Add other properties if needed
                        };

                        courseList.Add(course);
                    }
                }
            }

            // Assign the list of courses to the user profile model
            userProfileModel.Courses = courseList;

            return View(userProfileModel);
        }

        public class JsonResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }

        public JsonResult GetCoursesByCategory(string category, int UserID, string courseparam)
        {
            JsonResponse jsonResponse = new JsonResponse();

            try
            {
                if (category == "Continue")
                {
                    // Use parameterized queries to prevent SQL injection
                    var parameters = new SqlParameter[]
                    {
                       new SqlParameter("@UserID", SqlDbType.Int) { Value = UserID }
                    };
                    DataTable continueCoursesTable = DBManager.ExecuteDataSet("EXEC GetContinueCoursesByID @UserID", parameters).Tables[0];

                    List<Dictionary<string, object>> list = continueCoursesTable.AsEnumerable()
                        .Select(r => r.Table.Columns.Cast<DataColumn>()
                            .ToDictionary(c => c.ColumnName, c => r[c.Ordinal]))
                        .ToList();

                    if (list.Count > 0)
                        return Json(list, JsonRequestBehavior.AllowGet);

                    category = "Recommended"; // Default to Recommended if no continue courses found
                }

                var courseCategoryFilter = category == "Recommended" ? "IsSyllabus = 1" : "Category = @Category and IsSyllabus = 0";

                var courseParameters = new SqlParameter[]
                {
                     new SqlParameter("@UserID", SqlDbType.Int) { Value = UserID },
                     new SqlParameter("@Category", SqlDbType.VarChar) { Value = category }
                };

                // Execute the query for courses
                DataTable courseTable = DBManager.ExecuteDataSet("EXEC GetCourseBycategories @UserID, @Category", courseParameters).Tables[0];

                string filterExpression = !string.IsNullOrEmpty(courseparam)
                    ? $"(CourseName LIKE '%{courseparam.ToLower()}%' OR Category LIKE '%{courseparam.ToLower()}%') AND IsSyllabus = 0 AND Category IS NOT NULL"
                    : courseCategoryFilter;

                DataRow[] filteredRows = courseTable.Select(filterExpression);

                if (filteredRows.Length > 0)
                {
                    DataTable filteredCourses = filteredRows.CopyToDataTable();

                    var courseList = filteredCourses.AsEnumerable()
                        .Select(r => r.Table.Columns.Cast<DataColumn>()
                            .ToDictionary(c => c.ColumnName, c => r[c.Ordinal]))
                        .ToList();

                    return Json(courseList, JsonRequestBehavior.AllowGet);
                }

                return Json(new { error = "No courses found" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult BulkUserAddition()
        {
            return View();
        }

        public ActionResult RegisterUser()
        {
            return View(new User());
        }

    public void BulkRegisterUser(User obj)
    {
      using (HomeController.AppDbContext appDbContext = new HomeController.AppDbContext())
      {
        User user = new User()
        {
          FirstName = obj.FirstName,
          MiddleName = obj.MiddleName,
          LastName = obj.LastName,
          GuardianName = obj.GuardianName,
          Std = obj.Std,
          SchoolName = obj.SchoolName,
          MobileNumber = obj.MobileNumber,
          EmergencyContact = obj.EmergencyContact,
          EmailID = obj.EmailID,
          Location = obj.Location,
         // State = obj.State,
          City = obj.City,
          Username = obj.MobileNumber,
          Password = obj.MobileNumber,
          Pincode = obj.Pincode,
          Specify = obj.Specify,
        //  PaymentDetails = obj.PaymentDetails,
       //   Agree = new bool?(true)
        };
        appDbContext.Users.Add(user);
        appDbContext.SaveChanges();
      }
    }

    [HttpPost]
        public ActionResult UploadExcel()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    List<User> userList = new List<User>();

                    using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                    {
                        sqlConnection.Open();

                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(file.InputStream))
                        {
                            DataTable table = reader.AsDataSet().Tables[0];

                            using (var appDbContext = new HomeController.AppDbContext())
                            {
                                for (int index = 1; index < table.Rows.Count; ++index)
                                {
                                    var users = new User()
                                    {
                                        MobileNumber = table.Rows[index][3].ToString()
                                    };

                                    if (appDbContext.Users.Any(u => u.MobileNumber == users.MobileNumber))
                                    {
                                        ViewData["ErrorMessage"] = $"Following Mobile number already exists: {table.Rows[index][3]}";
                                        return View("BulkUserAddition");
                                    }
                                }
                            }

                            for (int index = 1; index < table.Rows.Count; ++index)
                            {
                                var user = new User()
                                {
                                    FirstName = table.Rows[index][0].ToString(),
                                    MiddleName = table.Rows[index][1].ToString(),
                                    LastName = table.Rows[index][2].ToString(),
                                    MobileNumber = table.Rows[index][3].ToString(),
                                    EmailID = table.Rows[index][4].ToString(),
                                    SchoolName = table.Rows[index][5].ToString(),
                                    City = table.Rows[index][6].ToString(),
                                  //  State = table.Rows[index][7].ToString(),
                                    Pincode = table.Rows[index][8].ToString()
                                };
                                BulkRegisterUser(user);
                            }
                        }
                    }

                    ViewData["Success"] = "Data Uploaded Successfully!!";
                    return View("BulkUserAddition");
                }

                ViewData["ErrorMessage"] = "Please Select File!!";
                return View("BulkUserAddition");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Invalid file signature"))
                    ViewData["ErrorMessage"] = "Invalid file format. Please upload a valid Excel file.";
                else
                    ViewData["ErrorMessage"] = $"Error occurred: {ex.Message}";

                return View("BulkUserAddition");
            }
        }

    }
}
