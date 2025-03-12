

using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using HEMUdaan.EMail;
using HEMUdaan.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace HEMUdaan.Controllers
{
  public class AdminController : Controller
  {
    private readonly HomeController.AppDbContext _dbContext;

    public AdminController() => this._dbContext = new HomeController.AppDbContext();

    public ActionResult Index() => (ActionResult) this.View();

        public ActionResult CourseSurvey()
        {
            int num = 1;

            // Create a new SyllabusModel and set the LanguageID
            SyllabusModel syllabusModel = new SyllabusModel()
            {
                LanguageID = num // Set language ID to num (1)
            };

            // Fetch survey list using AdminController method
            List<SyllabusModel.SurveyModelList> list1 = AdminController.GetList();
            ViewBag.SurveyListData = list1;

            // Fetch course data using stored procedure
            List<CourseDTO> courseList = new List<CourseDTO>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetCourses", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courseList.Add(new CourseDTO
                            {
                                Text = reader["CourseName"].ToString(),
                                Value = reader["CourseID"].ToString(),
                                Selected = false
                            });
                        }
                    }
                }
            }

            syllabusModel.course = courseList.Select(c => new SelectListItem
            {
                Text = c.Text,
                Value = c.Value,
                Selected = c.Selected
            }).ToList(); // Create a list of SelectListItem for the dropdown

            return View(syllabusModel);
        }


        public string GetTopicListSurvey(string CourseID)
        {
            List<SelectListItem> selectListItemList = new List<SelectListItem>();

            try
            {
                int ConvertCourseId = Convert.ToInt32(CourseID);

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetTopicsByCourseID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CourseID", ConvertCourseId);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                selectListItemList.Add(new SelectListItem
                                {
                                    Text = reader["TopicName"].ToString(),
                                    Value = reader["TopicID"].ToString(),
                                    Selected = false
                                });
                            }
                        }
                    }
                }

                return new JavaScriptSerializer().Serialize(selectListItemList);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving topics: " + ex.Message);
            }
        }

        public string GetSubTopicList(string TopicID)
        {
            List<SelectListItem> selectListItemList = new List<SelectListItem>();

            try
            {
                int ConvertParentTopicID = Convert.ToInt32(TopicID);

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetSubTopicsByTopicID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ParentTopicID", ConvertParentTopicID);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                selectListItemList.Add(new SelectListItem
                                {
                                    Text = reader["TopicName"].ToString(),
                                    Value = reader["TopicID"].ToString(),
                                    Selected = false
                                });
                            }
                        }
                    }
                }

                return new JavaScriptSerializer().Serialize(selectListItemList);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving subtopics: " + ex.Message);
            }
        }



        [HttpPost]
        public JsonResult SurveyData(int CourseID, int TopicID, int SubTopicID, string Question, string Option1, string Option2, string Option3, string Option4, string Option5)
        {
            List<object> objectList = new List<object>();

            DataTable dataTable = this.Session["PuzzleTissues"] as DataTable;
            if (dataTable == null)
            {
                objectList.Add("Error: DataTable is not found in session.");
                return Json(objectList, JsonRequestBehavior.AllowGet);
            }

            TempData[nameof(TopicID)] = TopicID.ToString();

            try
            {
                var syllabusModel = new SyllabusModel { LanguageID = 1 };
                string topicName = string.Empty;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetTopicNameByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TopicID", TopicID);
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            topicName = result.ToString();
                        }
                        else
                        {
                            objectList.Add("Error: Topic not found.");
                            return Json(objectList, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                TempData["TopicName"] = topicName;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SaveSurveyData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CourseID", CourseID);
                        cmd.Parameters.AddWithValue("@TopicID", TopicID);
                        cmd.Parameters.AddWithValue("@TopicName", topicName);
                        cmd.Parameters.AddWithValue("@SubTopicID", SubTopicID);
                        cmd.Parameters.AddWithValue("@Option1", Option1);
                        cmd.Parameters.AddWithValue("@Option2", Option2);
                        cmd.Parameters.AddWithValue("@Option3", Option3);
                        cmd.Parameters.AddWithValue("@Option4", Option4);
                        cmd.Parameters.AddWithValue("@Option5", Option5);
                        cmd.Parameters.AddWithValue("@Question", Question);
                        cmd.Parameters.AddWithValue("@LanguageID", syllabusModel.LanguageID);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            objectList.Add("1"); // Success
                        }
                        else
                        {
                            objectList.Add("2"); // Failure
                            TempData["Error"] = "Something went wrong during the survey submission.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objectList.Add($"Error: {ex.Message}");
            }

            return Json(objectList, JsonRequestBehavior.AllowGet);
        }

        public static List<SyllabusModel.SurveyModelList> GetList()
        {
            List<SyllabusModel.SurveyModelList> list = new List<SyllabusModel.SurveyModelList>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT ID, CourseID, TopicID, TopicName, SubTopicID, Question, Option1, Option2, Option3, Option4, Option5 FROM HF_SurveyData ORDER BY ID DESC", conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Dictionary<int, string> courseDict = new Dictionary<int, string>();
                            while (reader.Read())
                            {
                                int courseID = Convert.ToInt32(reader["CourseID"]);
                                string courseName;

                                if (!courseDict.TryGetValue(courseID, out courseName))
                                {
                                    using (SqlCommand courseCmd = new SqlCommand("SELECT CourseName FROM Courses WHERE CourseID = @CourseID", conn))
                                    {
                                        courseCmd.Parameters.AddWithValue("@CourseID", courseID);
                                        object courseResult = courseCmd.ExecuteScalar();
                                        courseName = courseResult != null ? courseResult.ToString() : string.Empty;
                                        courseDict[courseID] = courseName;
                                    }
                                }

                                list.Add(new SyllabusModel.SurveyModelList()
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    CourseID = courseID,
                                    CourseName = courseName,
                                    TopicID = Convert.ToInt32(reader["TopicID"]),
                                    TopicName = reader["TopicName"].ToString(),
                                    SubTopicID = Convert.ToInt32(reader["SubTopicID"]),
                                    Question = reader["Question"].ToString(),
                                    Option1 = reader["Option1"].ToString(),
                                    Option2 = reader["Option2"].ToString(),
                                    Option3 = reader["Option3"].ToString(),
                                    Option4 = reader["Option4"].ToString(),
                                    Option5 = reader["Option5"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return list;
        }



        public string DeleteSurvey(int ID)
        {
            JsonResponse jsonResponse = new JsonResponse();

            try
            {
               
                string query = "DELETE FROM HF_SurveyData WHERE ID = @ID";

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@ID", SqlDbType.Int) { Value = ID }
                };

                DBManager.ExecuteNonQuery(query, parameters);

                jsonResponse.Data = "Survey deleted successfully";
                jsonResponse.Message = "Survey deleted";
                jsonResponse.Success = true;
            }
            catch (Exception ex)
            {
               
                jsonResponse.Data = "";
                jsonResponse.Message = ex.Message; // You can log the full stack trace if needed
                jsonResponse.Success = false;
            }

            return new JavaScriptSerializer().Serialize(jsonResponse);
        }

       
        public class JsonResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }

        [HttpPost]
        public JsonResult EditSurvey(int ID)
        {
            try
            {
                TempData["IR"] = ID.ToString();

                var surveyDataById = GetSurveyDataByID(ID);

                if (surveyDataById != null)
                {
                    return Json(new
                    {
                        success = true,
                        data = surveyDataById
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Survey not found"
                    });
                }
            }
            catch (Exception ex)
            {
               
                return Json(new
                {
                    success = false,
                    message = $"An error occurred: {ex.Message}"
                });
            }
        }


        private SurveyData GetSurveyDataByID(int ID)
        {
            SurveyData surveyDataById = null; 
            string cmdText = "SELECT ID, CourseID, TopicID, TopicName, SubTopicID, Question, Option1, Option2, Option3, Option4, Option5 FROM HF_SurveyData WHERE ID = @ID";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(cmdText, connection))
                    {
                      
                        sqlCommand.Parameters.Add("@ID", SqlDbType.Int).Value = ID;

                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            if (sqlDataReader.Read()) // If a row is found
                            {
                                surveyDataById = new SurveyData()
                                {
                                    ID = sqlDataReader.GetInt32(0),
                                    CourseID = sqlDataReader.GetInt32(1),
                                    TopicID = sqlDataReader.GetInt32(2),
                                    TopicName = sqlDataReader.GetString(3),
                                    SubTopicID = sqlDataReader.GetInt32(4),
                                    Question = sqlDataReader.GetString(5),
                                    Option1 = sqlDataReader.GetString(6),
                                    Option2 = sqlDataReader.GetString(7),
                                    Option3 = sqlDataReader.GetString(8),
                                    Option4 = sqlDataReader.GetString(9),
                                    Option5 = sqlDataReader.GetString(10)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
            }

            return surveyDataById; 
        }


        [HttpPost]
        public ActionResult UpdateSurvey( string CourseID,string TopicID, string SubTopicID, string Question, string Option1, string Option2, string Option3, string Option4,  string Option5)
        {
            try
            {
                // Retrieve the ID from TempData
                int int32 = Convert.ToInt32(TempData["IR"]);
                SurveyData surveyDataById = GetSurveyDataByID(int32);

                // If survey not found, return failure response
                if (surveyDataById == null)
                {
                    return Json(new { success = false, message = "Survey not found." });
                }

                // Update the survey data
                surveyDataById.CourseID = Convert.ToInt32(CourseID);
                surveyDataById.TopicID = Convert.ToInt32(TopicID);
                surveyDataById.Question = Question;
                surveyDataById.Option1 = Option1;
                surveyDataById.Option2 = Option2;
                surveyDataById.Option3 = Option3;
                surveyDataById.Option4 = Option4;
                surveyDataById.Option5 = Option5;

                // Prepare the SQL query with parameterized values to prevent SQL injection
                string cmdText = "UPDATE HF_SurveyData SET Option1 = @Option1, Option2 = @Option2, Option3 = @Option3, Option4 = @Option4, Option5 = @Option5, Question = @Question WHERE ID = @ID";

                // Execute the query with parameters
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdText, connection))
                    {
                        // Add parameters to the command to prevent SQL injection
                        cmd.Parameters.AddWithValue("@Option1", Option1);
                        cmd.Parameters.AddWithValue("@Option2", Option2);
                        cmd.Parameters.AddWithValue("@Option3", Option3);
                        cmd.Parameters.AddWithValue("@Option4", Option4);
                        cmd.Parameters.AddWithValue("@Option5", Option5);
                        cmd.Parameters.AddWithValue("@Question", Question);
                        cmd.Parameters.AddWithValue("@ID", int32);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if any rows were affected
                        if (rowsAffected > 0)
                        {
                            TempData["Success"] = "You have edited the survey successfully";
                            return Json(new { success = true });
                        }
                        else
                        {
                            TempData["Error"] = "Something went wrong.";
                            return Json(new { success = false, message = "Failed to update the survey." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               

                return Json(new { success = false, message = ex.Message });
            }
        }


        //public ActionResult Userlist()
        //{
        //    // Get the list of users sorted by UserID in descending order
        //    var userList = _dbContext.Users
        //                                  .OrderByDescending(u => u.UserID)
        //                                  .ToList();  // Execute the query and get the data

        //    // Return the list to the view
        //    return View(userList);
        //}

        public ActionResult Userlist()
        {
          
            return View();
        }

        public string UserlistMaster()

        {
            
            List<User> model = new List<User>();
            try
            {
                string query = "EXEC Userslists";
                using (DataTable ResultDT = DBManager.ExecuteDataTable(query))
                {
                    model = DBManager.ConvertToList<User>(ResultDT);
                }
            }
            catch (Exception ex)
            {

            }
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }; // Set a high value
            return serializer.Serialize(model);

        }


        public ActionResult CourseMCQ()
        {
            int num = 1;
            SyllabusModel syllabusModel = new SyllabusModel()
            {
                LanguageID = num
            };

            List<SyllabusModel.SurveyModelList> list1 = GetList();
            ViewBag.MCQListData = list1;

            List<CourseDTO> courseList = new List<CourseDTO>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetCourses", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courseList.Add(new CourseDTO
                            {
                                Text = reader["CourseName"].ToString(),
                                Value = reader["CourseID"].ToString(),
                                Selected = false
                            });
                        }
                    }
                }
            }

            syllabusModel.course = courseList.Select(c => new SelectListItem
            {
                Text = c.Text,
                Value = c.Value,
                Selected = c.Selected
            }).ToList();

            return View(syllabusModel);
        }


        [HttpPost]
        public ActionResult Edit(int ID)
        {
            try
            {
                if (ID <= 0)
                {
                    return Json(new { success = false, message = "Invalid ID provided." });
                }

                TempData["IR"] = ID.ToString();

                var dataById = GetDataByID(ID);

                if (dataById != null)
                {
                    return Json(new
                    {
                        success = true,
                        data = dataById
                    });
                }

                return Json(new
                {
                    success = false,
                    message = "Data not found."
                });
            }
            catch (SqlException sqlEx)
            {
                return Json(new
                {
                    success = false,
                    message = "Database error: " + sqlEx.Message
                });
            }
            catch (Exception ex)
            {
               
                return Json(new
                {
                    success = false,
                    message = "An error occurred: " + ex.Message
                });
            }
        }


        private SurveyData GetDataByID(int ID)
        {
            SurveyData dataById = null;
            string cmdText = "SELECT ID, CourseID, TopicID, TopicName, SubTopicID, Question, Option1, Option2, Option3, Option4, Option5 FROM HF_MCQData WHERE ID = @ID";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(cmdText, connection))
                    {
                        // Use Add method to ensure parameter is added with correct type
                        sqlCommand.Parameters.Add("@ID", SqlDbType.Int).Value = ID;

                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            if (sqlDataReader.Read())
                            {
                                // Handle possible NULL values in database fields, e.g., TopicName, Question
                                dataById = new SurveyData()
                                {
                                    ID = sqlDataReader.GetInt32(0),
                                    CourseID = sqlDataReader.GetInt32(1),
                                    TopicID = sqlDataReader.GetInt32(2),
                                    TopicName = sqlDataReader.IsDBNull(3) ? null : sqlDataReader.GetString(3),
                                    SubTopicID = sqlDataReader.GetInt32(4),
                                    Question = sqlDataReader.IsDBNull(5) ? null : sqlDataReader.GetString(5),
                                    Option1 = sqlDataReader.IsDBNull(6) ? null : sqlDataReader.GetString(6),
                                    Option2 = sqlDataReader.IsDBNull(7) ? null : sqlDataReader.GetString(7),
                                    Option3 = sqlDataReader.IsDBNull(8) ? null : sqlDataReader.GetString(8),
                                    Option4 = sqlDataReader.IsDBNull(9) ? null : sqlDataReader.GetString(9),
                                    Option5 = sqlDataReader.IsDBNull(10) ? null : sqlDataReader.GetString(10)
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
              
            }
            catch (Exception ex)
            {
                
            }

            return dataById;
        }


        public string Delete(int ID)
        {
            JsonResponse jsonResponse = new JsonResponse();

            try
            {
                string cmdText = "DELETE FROM HF_MCQData WHERE ID = @ID";

                var parameters = new SqlParameter[]
                {
                  new SqlParameter("@ID", SqlDbType.Int) { Value = ID }
                };

                DBManager.ExecuteNonQuery(cmdText, parameters);

                // Populate response for success
                jsonResponse.Data = "MCQ deleted successfully";
                jsonResponse.Message = "MCQ deleted";
                jsonResponse.Success = true;
            }
            catch (SqlException sqlEx)
            {
                jsonResponse.Data = "";
                jsonResponse.Message = "Database error: " + sqlEx.Message;
                jsonResponse.Success = false;
            }
            catch (Exception ex)
            {
                jsonResponse.Data = "";
                jsonResponse.Message = "An error occurred: " + ex.Message;
                jsonResponse.Success = false;
            }

            return new JavaScriptSerializer().Serialize(jsonResponse);
        }

        [HttpPost]
        public JsonResult MCQData(int CourseID, int TopicID, int SubTopicID, string Question, string Option1, string Option2, string Option3, string Option4, string Option5, string CorrectAnswer)
        {
            List<object> objectList = new List<object>();
            TempData[nameof(TopicID)] = TopicID.ToString();

            try
            {
                SyllabusModel syllabusModel = new SyllabusModel()
                {
                    LanguageID = 1
                };

                string topicName = string.Empty;
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetTopicNameByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TopicID", TopicID);
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            topicName = result.ToString();
                            TempData["TopicName"] = topicName;
                        }
                        else
                        {
                            objectList.Add("2");
                            TempData["Error"] = "Topic not found.";
                            return Json(objectList, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SaveMCQData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CourseID", CourseID);
                        cmd.Parameters.AddWithValue("@TopicID", TopicID);
                        cmd.Parameters.AddWithValue("@TopicName", topicName);
                        cmd.Parameters.AddWithValue("@SubTopicID", SubTopicID);
                        cmd.Parameters.AddWithValue("@Option1", Option1);
                        cmd.Parameters.AddWithValue("@Option2", Option2);
                        cmd.Parameters.AddWithValue("@Option3", Option3);
                        cmd.Parameters.AddWithValue("@Option4", Option4);
                        cmd.Parameters.AddWithValue("@Option5", Option5);
                        cmd.Parameters.AddWithValue("@Question", Question);
                        cmd.Parameters.AddWithValue("@LanguageID", syllabusModel.LanguageID);
                        cmd.Parameters.AddWithValue("@CorrectAnswer", CorrectAnswer);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            objectList.Add("1"); // Success
                        }
                        else
                        {
                            objectList.Add("2"); // Failure
                            TempData["Error"] = "Something went wrong.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objectList.Add("2");
                TempData["Error"] = "An error occurred: " + ex.Message;
            }

            return Json(objectList, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult SendEmail(string email)
        {
            try
            {
                User userByEmail = GetUserByEmail(email);

                if (userByEmail != null)
                {
                    // Read the email template
                    string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template", "HVMailTemplates", "UserRegistration.html");
                    string emailTemplate = System.IO.File.ReadAllText(templatePath);

                    // Replace placeholders with actual user data
                    string emailBody = emailTemplate
                        .Replace("<%FirstName%>", userByEmail.FirstName)
                        .Replace("<%UserName%>", userByEmail.MobileNumber)
                        .Replace("<%Password%>", userByEmail.MobileNumber); // Note: You might want to change this if using MobileNumber as password is incorrect.

                    // Send email using the Email Manager
                    EMailManager.SendEmailAws(userByEmail.EmailID, "Registration Confirmation", emailBody, null, "", "");
                }

                return RedirectToAction("Userlist", "Admin");
            }
            catch (Exception ex)
            {
                // Log exception (Optional)
                return RedirectToAction("Error", "Home");
            }
        }




        public ActionResult SurveyReport()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
                List<SelectListItem> courseList = new List<SelectListItem>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetCourseList", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                courseList.Add(new SelectListItem
                                {
                                    Value = reader["CourseID"].ToString(),
                                    Text = reader["CourseName"].ToString(),
                                    Selected = false
                                });
                            }
                        }
                    }
                }

                ViewBag.CourseList = courseList;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

            return View();
        }


     
        public string SurveyReportMaster(string CoursesDDL)
        {
            List<ReportModel> modelList = new List<ReportModel>();
            TempData["ID"] = CoursesDDL;

            try
            {
                string query = "EXEC Survey_Report " + CoursesDDL;
                using (DataTable ResultDT = DBManager.ExecuteDataTable(query))
                {
                    modelList = DBManager.ConvertToList<ReportModel>(ResultDT);
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            return serializer.Serialize(modelList);
        }


       

        [HttpPost]
        public JsonResult ExportSurveyReport()
        {
            if (TempData["ID"] == null)
                return Json(new { error = "No ID found in TempData" }, JsonRequestBehavior.AllowGet);

            int courseId;
            if (!int.TryParse(TempData["ID"].ToString(), out courseId))
                return Json(new { error = "Invalid Course ID" }, JsonRequestBehavior.AllowGet);

            DataSet dataSet;
            try
            {
                dataSet = DBManager.ExecuteDataSet("EXEC Survey_Report @CourseID", new SqlParameter("@CourseID", courseId));
            }
            catch (Exception ex)
            {
                return Json(new { error = "Error executing stored procedure: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }

            if (dataSet.Tables.Count == 0)
                return Json(new { error = "No data found" }, JsonRequestBehavior.AllowGet);

            dataSet.Tables[0].TableName = "StudentReport";

            using (XLWorkbook xlWorkbook = new XLWorkbook())
            {
                foreach (DataTable table in dataSet.Tables)
                {
                    xlWorkbook.Worksheets.Add(table);
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    xlWorkbook.SaveAs(memoryStream);
                    return Json(new { file = Convert.ToBase64String(memoryStream.ToArray()) }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult PBLReport()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
                List<SelectListItem> courseList = new List<SelectListItem>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetCourseList", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                courseList.Add(new SelectListItem
                                {
                                    Value = reader["CourseID"].ToString(),
                                    Text = reader["CourseName"].ToString(),
                                    Selected = false
                                });
                            }
                        }
                    }
                }

                ViewBag.CourseList = courseList;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

            return View();
        }

        public string PBLReportMaster(string CoursesDDL)
        {
            List<PBLReport> modelList = new List<PBLReport>();
            TempData["ID"] = CoursesDDL;

            try
            {
                string query = "EXEC PBL_Report_Coursewise " + CoursesDDL;
                using (DataTable ResultDT = DBManager.ExecuteDataTable(query))
                {
                    modelList = DBManager.ConvertToList<PBLReport>(ResultDT);
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            return serializer.Serialize(modelList);
        }


        //public FileResult PBLReport()
        //{
        //    DataSet dataSet = DBManager.ExecuteDataSet("EXEC PBL_Report");

        //    // Handle null or empty dataset scenario
        //    if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables.Cast<DataTable>().All(t => t.Rows.Count == 0))
        //    {
        //        return null; // You may return a message or empty file instead
        //    }

        //    using (XLWorkbook xlWorkbook = new XLWorkbook())
        //    {
        //        foreach (DataTable table in dataSet.Tables)
        //        {
        //            if (table.Rows.Count > 0) // Only add non-empty tables
        //            {
        //                xlWorkbook.Worksheets.Add(table);
        //            }
        //        }

        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            xlWorkbook.SaveAs(memoryStream);
        //            return File(memoryStream.ToArray(),
        //                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                        "PBLReport.xlsx");
        //        }
        //    }
        //}


        public ActionResult MCQReport()
        {
            try
            {
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
                    List<SelectListItem> courseList = new List<SelectListItem>();

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("GetCourseList", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            con.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    courseList.Add(new SelectListItem
                                    {
                                        Value = reader["CourseID"].ToString(),
                                        Text = reader["CourseName"].ToString(),
                                        Selected = false
                                    });
                                }
                            }
                        }
                    }

                    ViewBag.CourseList = courseList;
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", "Home");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while fetching the course list.");
            }
            return View();
        }

      
        public string MCQReportMaster(string CoursesDDL)
        {
            List<ReportModel> modelList = new List<ReportModel>();
            TempData["ID"] = CoursesDDL;

            try
            {
                string query = "EXEC MCQ_Report " + CoursesDDL;
                using (DataTable ResultDT = DBManager.ExecuteDataTable(query))
                {
                    modelList = DBManager.ConvertToList<ReportModel>(ResultDT);
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            return serializer.Serialize(modelList);
          
        }

        public DataTable MCQ(int CourseID)
        {
            using (var appDbContext = new HomeController.AppDbContext())
            {
                TempData["ID"] = CourseID;
                DataTable dataTable = new DataTable();
                var connection = appDbContext.Database.Connection;
                var state = connection.State;

                try
                {
                    if (state != ConnectionState.Open)
                        connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "MCQ_Report";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@CourseID", CourseID));

                        using (var reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log exception properly
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (state != ConnectionState.Closed)
                        connection.Close();
                }

                return dataTable;
            }
        }

        [HttpPost]
        public ActionResult ExportMCQReport()
        {
            try
            {
                if (TempData["ID"] == null)
                    return Json(new { success = false, message = "Invalid Course ID" });

                int courseId;
                if (!int.TryParse(TempData["ID"].ToString(), out courseId))
                    return Json(new { success = false, message = "Invalid Course ID format" });

                TempData.Keep("ID"); // Preserve TempData value for subsequent requests.

                DataSet dataSet = DBManager.ExecuteDataSet("EXEC MCQ_Report " + courseId);

                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                    return Json(new { success = false, message = "No data available" });

                dataSet.Tables[0].TableName = "StudentReport";

                using (XLWorkbook xlWorkbook = new XLWorkbook())
                {
                    foreach (DataTable table in dataSet.Tables)
                        xlWorkbook.Worksheets.Add(table);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        xlWorkbook.SaveAs(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);

                        return File(memoryStream.ToArray(),
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "MCQReport.xlsx");
                    }
                }
            }
            catch (FormatException ex)
            {
                return Json(new { success = false, message = "Invalid Course ID format: " + ex.Message });
            }
            catch (SqlException ex)
            {
                return Json(new { success = false, message = "Database error: " + ex.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An unexpected error occurred: " + ex.Message });
            }
        }

        private User GetUserByEmail(string email)
        {
            using (var appDbContext = new HomeController.AppDbContext())
            {
                return appDbContext.Users.FirstOrDefault(u => u.EmailID == email);
            }
        }


        public ActionResult CreativityReports()
        {
            try
            {
                DataTable source = DBManager.ExecuteDataTable("exec CAS_Report");

                if (source != null && source.Rows.Count > 0)
                {
                    ViewBag.ListOfSchoolsParticipated = source.AsEnumerable();
                }
                else
                {
                    ViewBag.MessageData = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                // Log the error properly (You can replace this with your logger)
                ViewBag.MessageData = "An error occurred while fetching the report.";
                Console.WriteLine("Error in CreativityReports: " + ex.Message);
            }

            return View();
        }



    }

    internal class SurveyData
    {
        public int ID { get; set; }

        public int CourseID { get; set; }

        public string CourseName { get; set; }

        public int TopicID { get; set; }

        public int SubTopicID { get; set; }

        public string TopicName { get; set; }

        public string Question { get; set; }

        public string Option1 { get; set; }

        public string Option2 { get; set; }

        public string Option3 { get; set; }

        public string Option4 { get; set; }

        public string Option5 { get; set; }
    }

}
