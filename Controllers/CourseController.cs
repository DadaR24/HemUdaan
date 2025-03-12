
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2016.Presentation.Command;
using DocumentFormat.OpenXml.Spreadsheet;
using HEMUdaan.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HEMUdaan.Controllers
{
  public class CourseController : Controller
  {
    private readonly HomeController.AppDbContext _dbContext;

    public CourseController() => this._dbContext = new HomeController.AppDbContext();

    public ActionResult Index() => (ActionResult) this.View((object) new Course());

    [HttpPost]
        public ActionResult Index(Course obj)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("Sp_AddCourse", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        command.Parameters.Add(new SqlParameter("@CourseName", obj.CourseName));
                        command.Parameters.Add(new SqlParameter("@NoOfSessions", obj.NoOfSessions));
                        command.Parameters.Add(new SqlParameter("@Description", obj.Description));
                        command.Parameters.Add(new SqlParameter("@IsActive", true)); // Set IsActive to true
                        command.Parameters.Add(new SqlParameter("@CreatedBy", 1));  // Set CreatedBy to 1 (or appropriate user)
                        command.Parameters.Add(new SqlParameter("@CreatedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("@Author", obj.Author));
                        command.Parameters.Add(new SqlParameter("@Manager", obj.Manager));
                        command.Parameters.Add(new SqlParameter("@ExpectedDeliveryDate", obj.ExpectedDeliveryDate));
                        command.Parameters.Add(new SqlParameter("@CourseAllocatedTo", obj.CourseAllocatedTo));
                        command.Parameters.Add(new SqlParameter("@FromAge", obj.FromAge));
                        command.Parameters.Add(new SqlParameter("@ToAge", obj.ToAge));
                        command.Parameters.Add(new SqlParameter("@MainCategory", obj.MainCategory));

                        // Execute the stored procedure
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction(nameof(Index), "Course");
            }
            catch (Exception ex)
            {
                // Handle exception (optional logging, etc.)
                return View("Error", new HandleErrorInfo(ex, "Course", "Index"));
            }
        }


        public ActionResult Create(int? ID)
        {
            Course course = new Course();

            ViewBag.FeaturedImagePath = "~/Images/file-icon.png";

            if (ID.HasValue)
            {
                course = this.GetCourseByID(ID.Value);
            }

            return View(course);
        }


        public Course GetCourseByID(int courseID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Course course = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Execute the stored procedure to retrieve the course by CourseID
                    using (SqlCommand command = new SqlCommand("Sp_GetCourseByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add the CourseID parameter
                        command.Parameters.Add(new SqlParameter("@CourseID", courseID));

                        // Execute the command and retrieve the data
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                course = new Course
                                {
                                    CourseID = Convert.ToInt32(reader["CourseID"]),
                                    CourseName = reader["CourseName"].ToString(),
                                    // Add other properties here based on your Course model
                                    // For example: FeaturedImage, TrainerResource, etc.
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors (optional logging)
                throw new Exception("An error occurred while retrieving the course: " + ex.Message);
            }

            return course;
        }



        [HttpPost]
        public ActionResult Create(Course obj, HttpPostedFileBase fileFeaturedImage, HttpPostedFileBase fileCourseResource, HttpPostedFileBase fileExerciseResource)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Generate the file paths for each uploaded file
                string featuredImagePath = null, courseResourcePath = null, exerciseResourcePath = null;

                if (fileFeaturedImage != null && fileFeaturedImage.ContentLength > 0)
                {
                    string guid = obj.CourseID.ToString();
                    featuredImagePath = this.UploadFile(fileFeaturedImage, "~/Images/Courses/FeaturedImage/", guid);
                }

                if (fileCourseResource != null && fileCourseResource.ContentLength > 0)
                {
                    string guid = obj.CourseID.ToString();
                    courseResourcePath = this.UploadFile(fileCourseResource, "~/Images/Courses/TrainerResource/", guid);
                }

                if (fileExerciseResource != null && fileExerciseResource.ContentLength > 0)
                {
                    string guid = obj.CourseID.ToString();
                    exerciseResourcePath = this.UploadFile(fileExerciseResource, "~/Images/Courses/TrainerExercise/", guid);
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Execute the stored procedure to update the course
                    using (SqlCommand command = new SqlCommand("Sp_UpdateCourse", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@CourseID", obj.CourseID));
                        command.Parameters.Add(new SqlParameter("@CourseName", obj.CourseName));
                        command.Parameters.Add(new SqlParameter("@ModifiedDate", DateTime.Now));

                        // Pass the file paths (if they exist) as parameters
                        if (!string.IsNullOrEmpty(featuredImagePath))
                            command.Parameters.Add(new SqlParameter("@FeaturedImage", featuredImagePath));
                        else
                            command.Parameters.Add(new SqlParameter("@FeaturedImage", DBNull.Value));

                        if (!string.IsNullOrEmpty(courseResourcePath))
                            command.Parameters.Add(new SqlParameter("@TrainerResource", courseResourcePath));
                        else
                            command.Parameters.Add(new SqlParameter("@TrainerResource", DBNull.Value));

                        if (!string.IsNullOrEmpty(exerciseResourcePath))
                            command.Parameters.Add(new SqlParameter("@TrainerExercise", exerciseResourcePath));
                        else
                            command.Parameters.Add(new SqlParameter("@TrainerExercise", DBNull.Value));

                        // Execute the stored procedure
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index", "Course");
            }
            catch (Exception ex)
            {
                // Log the error (optional logging)
                return View("Error", new HandleErrorInfo(ex, "Course", "Create"));
            }
        }

        public ActionResult CourseList()
        {
            try
            {
                List<Course> courseList = new List<Course>();
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a command to execute the stored procedure
                    using (SqlCommand command = new SqlCommand("Sp_GetAllCourses", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Course course = new Course
                                {
                                    CourseID = reader.GetInt32(reader.GetOrdinal("CourseID")),
                                    CourseName = reader.GetString(reader.GetOrdinal("CourseName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    ExpectedDeliveryDate = reader.GetDateTime(reader.GetOrdinal("ExpectedDeliveryDate")),
                                };

                                courseList.Add(course);
                            }
                        }
                    }
                }

                return View(courseList);
            }
            catch (Exception ex)
            {
                // Handle the exception (optional logging, etc.)
                return View("Error", new HandleErrorInfo(ex, "Course", "CourseList"));
            }
        }



        public ActionResult Details(int ID)
        {
            Course course = new Course();

            course = GetCourseByID(ID);

            // Store the ID in TempData
            TempData[nameof(ID)] = ID;

            return View(course);
        }


        [HttpPost]
        public ActionResult AddTopic(Topics obj, HttpPostedFileBase fileTopicResource, HttpPostedFileBase fileTopicExercise)
        {
            object obj1 = ((ControllerBase)this).TempData["ID"];
            UserController.JsonResponse jsonResponse = new UserController.JsonResponse();
            string message = obj.TopicID >= 1 ? "Topic Updated Successfully" : "Topic Added Successfully";
            string guid = Course.GenerateGuid();

            if (fileTopicResource != null && fileTopicResource.ContentLength > 0)
                obj.TrainerResource = this.UploadFile(fileTopicResource, "~/Images/Courses/Topics/TrainerResource/", guid);

            if (fileTopicExercise != null && fileTopicExercise.ContentLength > 0)
                obj.TrainerExercise = this.UploadFile(fileTopicExercise, "~/Images/Courses/Topics/TrainerExercise/", guid);

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_AddOrUpdateTopic", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TopicID", obj.TopicID);
                        cmd.Parameters.AddWithValue("@CourseID", Convert.ToInt32(obj1));
                        cmd.Parameters.AddWithValue("@ParentTopicID", obj.ParentTopicID);
                        cmd.Parameters.AddWithValue("@TopicName", obj.TopicName);
                        cmd.Parameters.AddWithValue("@Description", obj.Description);
                        cmd.Parameters.AddWithValue("@Order", obj.Order);
                        cmd.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        cmd.Parameters.AddWithValue("@ModifiedBy", obj.ModifiedBy);
                        cmd.Parameters.AddWithValue("@TrainerResource", obj.TrainerResource ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TrainerExercise", obj.TrainerExercise ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@LanguageID", obj.languageID);
                        cmd.Parameters.AddWithValue("@NoOfQuestion", obj.NoOfQuestion);
                        cmd.Parameters.AddWithValue("@TrainerVideo", obj.TrainerVideo ?? (object)DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Create", "Course");
            }
            catch (Exception ex)
            {
                // Log or handle the error as needed
                return View("Error");
            }
        }

        public string UploadFile(HttpPostedFileBase file, string basePath, string guid)
    {
      string str1 = "";
      if (file != null && file.ContentLength > 0)
      {
        string fileName = Path.GetFileName(file.FileName);
        string str2 = Path.Combine(basePath, guid);
        if (!Directory.Exists(this.Server.MapPath(str2)))
          Directory.CreateDirectory(this.Server.MapPath(str2));
        string filename = Path.Combine(this.Server.MapPath(str2), fileName);
        file.SaveAs(filename);
        str1 = Path.Combine(str2, fileName);
      }
      return str1;
    }

        public ActionResult GetTopicByID(int TopicID)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                Topics topic = null;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_GetTopicByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TopicID", TopicID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    topic = new Topics
                                    {
                                        TopicID = Convert.ToInt32(reader["TopicID"]),
                                        TopicName = reader["TopicName"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        // Add other columns as needed
                                    };
                                }
                            }
                        }
                    }
                }

                return topic != null
                    ? Json(topic, JsonRequestBehavior.AllowGet)
                    : Json(new { error = "Topic not found" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occurred: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetSubTopic(int ID)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                Topics topic = null;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_GetTopicByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TopicID", ID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    topic = new Topics
                                    {
                                        TopicID = Convert.ToInt32(reader["TopicID"]),
                                        TopicName = reader["TopicName"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        // Include other fields if necessary
                                    };
                                }
                            }
                        }
                    }
                }

                return topic != null
                    ? Json(topic, JsonRequestBehavior.AllowGet)
                    : Json(new { error = "Topic not found" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occurred: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public string GetSubTopicList(string TopicID)
        {
            List<SelectListItem> selectListItemList = new List<SelectListItem>();

            try
            {
                int parentTopicID = Convert.ToInt32(TopicID); // Convert TopicID to int
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_GetSubTopics", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ParentTopicID", parentTopicID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                selectListItemList.Add(new SelectListItem
                                {
                                    Text = reader["TopicName"].ToString(),
                                    Value = reader["TopicID"].ToString(),
                                    Selected = false // Default to not selected
                                });
                            }
                        }
                    }
                }

                return new JavaScriptSerializer().Serialize(selectListItemList); // Serialize the list to JSON
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching subtopics: " + ex.Message);
            }
        }



        public string ListAllCourseTopic(string CourseId)
        {
            List<SelectListItem> selectListItemList = new List<SelectListItem>();
            try
            {
                int courseId = Convert.ToInt32(CourseId); // Convert CourseId to int
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_GetCourseTopics", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CourseID", courseId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                selectListItemList.Add(new SelectListItem
                                {
                                    Text = reader["TopicName"].ToString(),
                                    Value = reader["TopicID"].ToString(),
                                    Selected = false // Default to not selected
                                });
                            }
                        }
                    }
                }

                return new JavaScriptSerializer().Serialize(selectListItemList);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching course topics: " + ex.Message);
            }
        }


        [HttpGet]
        public ActionResult ListAllScrambleTopic()
        {
            try
            {
                List<object> topicsList = new List<object>();
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_GetAllScrambleTopics", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                topicsList.Add(new
                                {
                                    Id = reader["TopicID"],
                                    ScrambleTopic = reader["TopicName"]
                                });
                            }
                        }
                    }
                }

                if (topicsList.Any())
                {
                    return Json(new
                    {
                        Success = true,
                        Data = topicsList
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "No topics found"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred while retrieving topics: " + ex.Message);
                return Json(new
                {
                    Success = false,
                    Message = "An error occurred while retrieving topics"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ListCourseTopic(string courseId)
        {
            List<SelectListItem> selectListItemList = new List<SelectListItem>();

            try
            {
                int courseIdValue = Convert.ToInt32(courseId);
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_GetCourseTopics", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CourseID", courseIdValue);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                selectListItemList.Add(new SelectListItem
                                {
                                    Text = reader["TopicName"].ToString(),
                                    Value = reader["TopicID"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return Json(selectListItemList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult WJPractical(WJ_PracticalViewModel model, HttpPostedFileBase Thumbnail, HttpPostedFileBase Resources)
        {
            try
            {
                string guid = Course.GenerateGuid();

                if (Thumbnail != null && Thumbnail.ContentLength > 0)
                    model.Thumbnail = this.UploadFile(Thumbnail, "~/Images/Practical/Thumbnail/", guid);

                if (Resources != null && Resources.ContentLength > 0)
                    model.Resources = this.UploadFile(Resources, "~/Images/Practical/Resources/", guid);

                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_InsertWJPractical", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        cmd.Parameters.AddWithValue("@CourseID", model.CourseID);
                        cmd.Parameters.AddWithValue("@Title", model.Title);
                        cmd.Parameters.AddWithValue("@PracticalLevel", model.PracticalLevel);
                        cmd.Parameters.AddWithValue("@Description", model.Description);
                        cmd.Parameters.AddWithValue("@Coins", model.Coins);
                        cmd.Parameters.AddWithValue("@PracticalTopicID", model.PracticalTopicID);
                        cmd.Parameters.AddWithValue("@LanguageID", 1);
                        cmd.Parameters.AddWithValue("@OrderNo", model.OrderNo);
                        cmd.Parameters.AddWithValue("@FileType", model.FileType);
                        cmd.Parameters.AddWithValue("@Student_Group", model.Student_Group);
                        cmd.Parameters.AddWithValue("@Code", model.Code);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@isTask", model.isTask);
                        cmd.Parameters.AddWithValue("@Resources", (object)model.Resources ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Thumbnail", (object)model.Thumbnail ?? DBNull.Value);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            return RedirectToAction("Create", "Course");
                        }
                        else
                        {
                            TempData["Error"] = "Failed to insert WJ Practical.";
                            return View("Error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred: " + ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Approve(Course.CourseApprovalViewModel model)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("Sp_ApproveCourse", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CourseID", model.CourseID);
                        cmd.Parameters.AddWithValue("@ApprovalStatus", model.ApprovalStatus);
                        cmd.Parameters.AddWithValue("@ApprovalRemark", model.ApprovalRemark);
                        cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                        SqlParameter outputParam = new SqlParameter("@SuccessMessage", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        int result = cmd.ExecuteNonQuery();
                        string successMessage = outputParam.Value.ToString();

                        if (result > 0)
                        {
                            return Json(new { Success = true, Message = successMessage });
                        }
                        else
                        {
                            return Json(new { Success = false, Message = "Course not found or update failed." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CourseController.Approve: " + ex.Message);
                return Json(new { Success = false, Message = "An error occurred while processing your request. Please try again later." });
            }
        }

        public ActionResult Learn(int? courseID)
        {
            if (courseID == null)
            {
                return RedirectToAction("Index");  // Or another page if courseID is invalid
            }

            QuizModel quizModel = new QuizModel();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_GetCourseAndTopics", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add the CourseID parameter
                command.Parameters.AddWithValue("@CourseID", courseID);

                connection.Open();

                // Initialize objects to hold the data
                string courseName = null, description = null, featuredImage = null;
                int parentTopicsCount = 0, subTopicsCount = 0;

                List<Topics> surveyTopics = new List<Topics>();
                List<Topics> psychologicalExpressionTopics = new List<Topics>();
                List<Topics> introductionTopics = new List<Topics>();
                List<WJ_Practical>practicals = new List<WJ_Practical>();

                // Execute the first query (Course details)
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        
                        courseName = reader["CourseName"].ToString();
                        description = reader["Description"].ToString();
                        featuredImage = reader["FeaturedImage"].ToString();
                    }

                    // Move to the next result set (Parent topics count)
                    if (reader.NextResult() && reader.Read())
                    {
                        parentTopicsCount = reader["ParentTopicsCount"] != DBNull.Value ? (int)reader["ParentTopicsCount"] : 0;
                    }

                    // Move to the next result set (Subtopics count)
                    if (reader.NextResult() && reader.Read())
                    {
                        subTopicsCount = reader["SubTopicsCount"] != DBNull.Value ? (int)reader["SubTopicsCount"] : 0;
                    }

                    // Move to the next result set (Survey topics)
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            surveyTopics.Add(new Topics
                            {
                                
                                TopicName = reader["TopicName"].ToString(),
                                Order = (int)reader["Order"],
                                TopicID = (int)reader["TopicID"],
                                languageID = (int)reader["languageID"],
                                TrainerExercise = reader["TrainerExercise"].ToString()
                            });
                        }
                    }

                    // Move to the next result set (Psychological Expression topics)
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            psychologicalExpressionTopics.Add(new Topics
                            {
                                TopicName = reader["TopicName"].ToString(),
                                Order = (int)reader["Order"],
                                TopicID = (int)reader["TopicID"],
                                languageID = (int)reader["languageID"],
                                TrainerExercise = reader["TrainerExercise"].ToString()
                            });
                        }
                    }

                    // Move to the next result set (Introduction topics)
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            introductionTopics.Add(new Topics
                            {
                                TopicName = reader["TopicName"].ToString(),
                                Order = (int)reader["Order"],
                                TopicID = (int)reader["TopicID"],
                                languageID = (int)reader["languageID"],
                                TrainerExercise = reader["TrainerExercise"].ToString()
                            });
                        }
                    }
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            practicals.Add(new WJ_Practical
                            {
                                PracticalID = (int)reader["PracticalID"],
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"].ToString(),
                                Thumbnail = reader["Thumbnail"].ToString(),
                                Resources = reader["Resources"].ToString(),
                                OrderNo = (int)reader["OrderNo"],
                                languageID = (int)reader["languageID"]

                            });
                        }
                    }
                }

                // Populate the quiz model with data
                quizModel.CourseName = courseName;
                TempData["CourseName"] = quizModel.CourseName;
                quizModel.Description = description;
                quizModel.FeaturedImage = featuredImage;

                ViewBag.TopicsCount = parentTopicsCount;
                ViewBag.SubtopicsCount = subTopicsCount;

                ViewBag.Topics = surveyTopics;
               
                ViewBag.Topicss = psychologicalExpressionTopics;
                ViewBag.Sub = introductionTopics;
                ViewBag.Practicals = practicals;
                ViewBag.CourseID = courseID;
               
                return View(quizModel);
            }

        }



        public ActionResult Survey(int CourseID, int TopicID, int LanguageID = 1)
        {
            int? UserID = (int)Session["UserID"];
            //int userID = Convert.ToInt32(TempData["UserID"]);
            //TempData["UserID"] = userID;

            QuizModel quizModel = new QuizModel();
            quizModel.LanguageID = LanguageID != 0 ? LanguageID : 1;

            var SurveyCheck = ExecuteScalar("Select * from SurveyResponse where UserID = " + UserID + " AND CourseID = " + CourseID + "");
            if (SurveyCheck == null || SurveyCheck == "")
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Call stored procedure to get survey data for the user
                    using (SqlCommand cmd = new SqlCommand("GetSurveyDataForUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@UserID", UserID);
                        cmd.Parameters.AddWithValue("@CourseID", CourseID);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable surveyData = new DataTable();
                            adapter.Fill(surveyData);

                            ViewData["SurveyData"] = surveyData.Rows.Count > 0 ? surveyData.AsEnumerable() : null;
                        }
                    }


                    using (SqlCommand cmd = new SqlCommand("GetCourseName", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CourseID", CourseID);

                        var courseName = cmd.ExecuteScalar();
                        TempData["Name"] = courseName;
                        quizModel.CourseName1 = courseName.ToString();
                    }

                    TempData["ID"] = CourseID.ToString();
                    TempData[nameof(TopicID)] = TopicID.ToString();
                }
            }
            else
            {
                TempData["Error"] = "You have already submitted the survey.";
                TempData["UseSweetAlert1"] = true;
                TempData["SweetAlertTitle"] = "Success";
                TempData["SweetAlertMessage"] = "You have already submitted the survey.";
                TempData["SweetAlertType"] = "success";
                return RedirectToAction("SweetAlert", "Home",new {UserID = UserID,CourseID = CourseID});
            }

            return View(quizModel);
        }


        private int GetUserIdFromSession()
        {
              User user = ((IQueryable<User>) this._dbContext.Users).FirstOrDefault<User>();
              return user != null ? user.UserID : -1;
        }

    public static string ExecuteScalar(string strSql, params SqlParameter[] parameters)
    {
      try
      {
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
        {
          SqlCommand sqlCommand = new SqlCommand(strSql, connection);
          sqlCommand.Parameters.AddRange(parameters);
          sqlCommand.Connection.Open();
          sqlCommand.CommandTimeout = 21600;
          string str = Convert.ToString(sqlCommand.ExecuteScalar());
          sqlCommand.Connection.Close();
          return str;
        }
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    [HttpPost]
        public ActionResult SaveSurvey(IEnumerable<SurveyResponse> responses)
        {
            int courseId = Convert.ToInt32(TempData["ID"]);
            int topicId = Convert.ToInt32(TempData["TopicID"]);
            int? UserID = (int)Session["UserID"];

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Get the TopicName from the database using a stored procedure
                    string topicName = string.Empty;
                    using (SqlCommand cmd = new SqlCommand("GetTopicName", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TopicID", topicId);

                        topicName = cmd.ExecuteScalar()?.ToString();
                    }

                    TempData["TopicName"] = topicName;

                    if (responses != null && responses.Any())
                    {
                        int questionNumber = 1;
                        bool isSurveySaved = false;

                        foreach (var response in responses)
                        {
                            using (SqlCommand cmd = new SqlCommand("InsertSurveyResponse", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@CourseID", courseId);
                                cmd.Parameters.AddWithValue("@TopicID", topicId);
                                cmd.Parameters.AddWithValue("@TopicName", topicName);
                                cmd.Parameters.AddWithValue("@QuestionNumber", questionNumber);
                                cmd.Parameters.AddWithValue("@QuestionText", response.QuestionText);
                                cmd.Parameters.AddWithValue("@SelectedOption", response.SelectedOption);
                                cmd.Parameters.AddWithValue("@UserID", UserID);

                                var result = cmd.ExecuteScalar();  // Get the result from the stored procedure
                                if (result != null && result.ToString() == "1")
                                {
                                    isSurveySaved = true;
                                }
                                else
                                {
                                    // If an error message is returned
                                    TempData["Error"] = "Error saving response for question " + questionNumber + ": " + result.ToString();
                                    return RedirectToAction("SweetAlert", "Home", new { UserID = UserID, CourseID = courseId });
                                }

                                questionNumber++;
                            }
                        }

                        if (isSurveySaved)
                        {
                            TempData["Success"] = "You have submitted the survey.";
                            TempData["UseSweetAlert3"] = true;
                            TempData["SweetAlertTitle"] = "Success";
                            TempData["SweetAlertMessage"] = "Congratulations! You have submitted the survey.";
                            TempData["SweetAlertType"] = "success";
                            return RedirectToAction("SweetAlert", "Home", new { UserID = UserID, CourseID = courseId });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred: " + ex.Message;
                return RedirectToAction("SweetAlert", "Home");
            }

            TempData["Error"] = "No survey responses received.";
            return RedirectToAction("SweetAlert", "Home");
        }

        [HttpPost]
        public ActionResult Creativity(HttpPostedFileBase UploadDoc, FormCollection formCollection)
        {
            try
            {
                int? UserID = (int)Session["UserID"];
                string courseName = TempData["CourseName"] as string;

                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Retrieve CourseID
                    int CourseID = 0;
                    using (SqlCommand cmd = new SqlCommand("GetCourseID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CourseName", courseName);
                        object result = cmd.ExecuteScalar();
                        CourseID = result != null ? Convert.ToInt32(result) : 0;
                    }

                    // Retrieve User Details (FirstName, LastName)
                    string FirstName = "", LastName = "";
                    using (SqlCommand cmd = new SqlCommand("GetUserdetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", UserID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                FirstName = reader["FirstName"].ToString();
                                LastName = reader["LastName"].ToString();
                            }
                        }
                    }

                    // Check if file already exists for the user and course
                    using (SqlCommand checkCmd = new SqlCommand("SELECT COUNT(1) FROM HF_UploadAnubhuti WHERE IsCreatedBy = @UserID AND CourseName = @CourseName", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@UserID", UserID);
                        checkCmd.Parameters.AddWithValue("@CourseName", courseName);
                        int existingCount = (int)checkCmd.ExecuteScalar();

                        if (existingCount > 0)
                        {
                            SetSweetAlert("File Already Uploaded", "You have already uploaded a file for this course.", "info");
                            return RedirectToAction("SweetAlert", "Home", new { UserID = UserID, CourseID = CourseID });
                        }
                    }

                    // Validate uploaded file
                    if (UploadDoc != null && UploadDoc.ContentLength > 0)
                    {
                        string fileExtension = Path.GetExtension(UploadDoc.FileName).ToLower();
                        if (IsExtensionAllowed(fileExtension))
                        {
                            string safeFileName = GetSafeFileName(UploadDoc.FileName);
                            string filePath = GetFilePath(UserID);

                            // Ensure directory exists
                            if (!Directory.Exists(filePath))
                            {
                                Directory.CreateDirectory(filePath);
                            }

                            string fullFilePath = Path.Combine(filePath, safeFileName + fileExtension);
                            UploadDoc.SaveAs(fullFilePath);

                            byte[] fileBytes;
                            using (BinaryReader binaryReader = new BinaryReader(UploadDoc.InputStream))
                            {
                                fileBytes = binaryReader.ReadBytes(UploadDoc.ContentLength);
                            }

                            // Save file data using stored procedure
                            using (SqlCommand cmd = new SqlCommand("Sp_SaveUplodedFiles", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@CourseID", CourseID);
                                cmd.Parameters.AddWithValue("@UplodedFiles", fileBytes);
                                cmd.Parameters.AddWithValue("@IsCreatedBy", UserID);
                                cmd.Parameters.AddWithValue("@FileName", safeFileName);
                                cmd.Parameters.AddWithValue("@FileExtension", fileExtension);
                                cmd.Parameters.AddWithValue("@FileLink", fullFilePath);
                                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                                cmd.Parameters.AddWithValue("@LastName", LastName);
                                cmd.Parameters.AddWithValue("@CourseName", courseName);

                                int result = cmd.ExecuteNonQuery();

                                if (result > 0)
                                {
                                    TempData["Success"] = "Your file has been successfully uploaded.";
                                    TempData["UseSweetAlert4"] = true;
                                    TempData["SweetAlertTitle"] = "Success";
                                    TempData["SweetAlertMessage"] = "Your file has been successfully uploaded.";
                                    TempData["SweetAlertType"] = "success";
                                    return RedirectToAction("SweetAlert", "Home", new { UserID = UserID, CourseID = CourseID });
                                }
                                else
                                {
                                    SetSweetAlert("Error", "An error occurred while saving your file.", "error");
                                }
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Only .jpg, .jpeg, .png, .txt, and .pdf files are allowed.";
                            TempData["UseSweetAlert5"] = true;
                            TempData["SweetAlertTitle"] = "Invalid File Type";
                            TempData["SweetAlertMessage"] = "Only .jpg, .jpeg, .png, .txt, and .pdf extensions are allowed.";
                            TempData["SweetAlertType"] = "error";
                            return RedirectToAction("SweetAlert", "Home", new { UserID = UserID, CourseID = CourseID });
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Please select at least one file to upload.";
                        TempData["UseSweetAlert6"] = true;
                        TempData["SweetAlertTitle"] = "No File Selected";
                        TempData["SweetAlertMessage"] = "Please select a file before uploading.";
                        TempData["SweetAlertType"] = "warning";
                        return RedirectToAction("SweetAlert", "Home", new { UserID = UserID, CourseID = CourseID });
                    }
                }

                return RedirectToAction("SweetAlert", "Home", new { UserID = UserID });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred: " + ex.Message;
                return RedirectToAction("Index", "User");
            }
        }

        // Helper method to set SweetAlert messages
        private void SetSweetAlert(string title, string message, string type)
        {
            TempData["UseSweetAlert6"] = true;
            TempData["SweetAlertTitle"] = title;
            TempData["SweetAlertMessage"] = message;
            TempData["SweetAlertType"] = type;
        }


        private bool IsExtensionAllowed(string extension)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".txt", ".pdf" };
            return allowedExtensions.Contains(extension.ToLower());
        }


        private string GetSafeFileName(string originalFileName)
        {
            return originalFileName.Substring(0, originalFileName.Length - Path.GetExtension(originalFileName).Length).Replace(" ", "_");
        }

    private string GetFilePath(int ?userID)
    {
      return this.Server.MapPath(string.Format("~/Images/Anubhuti/{0}", (object) userID));
    }

    private void ConfigureSqlCommandParameters(SqlCommand cmd,int courseID,byte[] bytes,int userID,string firstName,string lastName,string courseName, string fileName,  string filePath, string extension)
    {
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@CourseID", (object) courseID));
      cmd.Parameters.Add(new SqlParameter("@UplodedFiles", (object) bytes));
      cmd.Parameters.Add(new SqlParameter("@IsCreatedBy", (object) userID));
      cmd.Parameters.Add(new SqlParameter("@FirstName", (object) firstName));
      cmd.Parameters.Add(new SqlParameter("@LastName", (object) lastName));
      cmd.Parameters.Add(new SqlParameter("@CourseName", (object) courseName));
      cmd.Parameters.Add(new SqlParameter("@FileName", (object) fileName));
      cmd.Parameters.Add(new SqlParameter("@FileLink", (object) filePath));
      cmd.Parameters.Add(new SqlParameter("@FileExtension", (object) extension));
    }

    private void SetSuccessSweetAlert(int result)
    {
      string str = "Congratulations! Successfully Upload File.";
      TempData["UseSweetAlert"] =  true;
      TempData["SweetAlertTitle"] =  "Success";
      TempData["SweetAlertMessage"] = str;
      TempData["SweetAlertType"] = "success";
    }

        public string ListAllCourseSubTopic(string CourseId)
        {
            List<SelectListItem> selectListItemList = new List<SelectListItem>();

            try
            {
                int ConvertCourseId = Convert.ToInt32(CourseId);
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetCourseSubTopics", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CourseID", ConvertCourseId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                selectListItemList.Add(new SelectListItem
                                {
                                    Text = reader["TopicName"].ToString(),
                                    Value = reader["TopicID"].ToString(),
                                    Selected = false  // Adjust this if you need to select any item
                                });
                            }
                        }
                    }
                }

                return new JavaScriptSerializer().Serialize(selectListItemList);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching course sub-topics: " + ex.Message);
            }
        }



        public string AddEditQuestion(QuestionViewModel model)
        {
            JsonResponse jsonResponse = new JsonResponse();

            if (this.ModelState.IsValid)
            {
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        int? questionId = model.QuestionID;
                        string successMessage;

                        using (SqlCommand cmd = new SqlCommand("AddOrUpdateQuestion", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@QuestionID", questionId ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@QuestionName", model.QuestionName);
                            cmd.Parameters.AddWithValue("@QuestionLevel", model.QuestionLevel);
                            cmd.Parameters.AddWithValue("@Marks", model.Marks);
                            cmd.Parameters.AddWithValue("@ComplexityLevel", model.ComplexityLevel);
                            cmd.Parameters.AddWithValue("@CourseID", model.CourseID);
                            cmd.Parameters.AddWithValue("@TopicID", model.TopicID);
                            cmd.Parameters.AddWithValue("@SubTopicID", model.SubTopicID);
                            cmd.Parameters.AddWithValue("@Type", model.Type);
                            cmd.Parameters.AddWithValue("@isAdminStatus", model.isAdminStatus);
                            cmd.Parameters.AddWithValue("@languageID", model.languageID);
                            cmd.Parameters.AddWithValue("@QuestionOption", model.QuestionOption ?? (object)DBNull.Value);

                            SqlParameter outputParam = new SqlParameter("@SuccessMessage", SqlDbType.NVarChar, 255)
                            {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(outputParam);

                            cmd.ExecuteNonQuery();

                            successMessage = outputParam.Value.ToString();
                        }

                        jsonResponse.Data = successMessage;
                        jsonResponse.Message = successMessage;
                        jsonResponse.Success = true;
                    }
                }
                catch (Exception ex)
                {
                    jsonResponse.Data = "";
                    jsonResponse.Message = ex.Message;
                    jsonResponse.Success = false;
                }
            }
            else
            {
                var errorMessages = string.Join("; ", this.ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));

                jsonResponse.Data = errorMessages;
                jsonResponse.Message = "Validation Failed";
                jsonResponse.Success = false;
            }

            return new JavaScriptSerializer().Serialize(jsonResponse);
        }

        public ActionResult MCQ(int CourseID, int TopicID, int LanguageID = 1)
        {
            // Retrieve UserID from TempData (ensure it's an integer)
            int? UserID = (int)Session["UserID"];

            QuizModel quizModel = new QuizModel
            {
                LanguageID = LanguageID != 0 ? LanguageID : 1
            };

            // Establish a SQL connection using your connection string
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Check if the user has already submitted the MCQ response for the given topic
                using (SqlCommand cmd = new SqlCommand("CheckMCQSubmission", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@TopicID", TopicID);

                    var submissionResult = cmd.ExecuteScalar();
                    if (submissionResult == null || Convert.ToInt32(submissionResult) == 0)
                    {
                        // No submission found, fetch MCQ data for the given topic
                        using (SqlCommand mcqCmd = new SqlCommand("GetMCQData", conn))
                        {
                            mcqCmd.CommandType = CommandType.StoredProcedure;
                            mcqCmd.Parameters.AddWithValue("@TopicID", TopicID);

                            var mcqData = new DataTable();
                            using (SqlDataAdapter adapter = new SqlDataAdapter(mcqCmd))
                            {
                                adapter.Fill(mcqData);
                            }

                            if (mcqData.Rows.Count > 0)
                            {
                                ViewData["MCQData"] = mcqData.AsEnumerable();
                            }
                            else
                            {
                                ViewData["MCQData"] = null;
                            }
                        }

                        // Get course information and populate quizModel
                        using (SqlCommand courseCmd = new SqlCommand("SELECT CourseName FROM Courses WHERE CourseID = @CourseID", conn))
                        {
                            courseCmd.Parameters.AddWithValue("@CourseID", CourseID);
                            var courseName = courseCmd.ExecuteScalar()?.ToString();

                            if (!string.IsNullOrEmpty(courseName))
                            {
                                TempData["Name"] = courseName;
                                quizModel.CourseID = CourseID;
                                quizModel.CourseName1 = courseName;
                            }
                        }

                        // Store additional TempData for CourseID and TopicID
                        TempData["ID"] = CourseID.ToString();
                        TempData["TopicID"] = TopicID.ToString();
                    }
                    else
                    {
                        // If already submitted, show an alert and redirect
                        TempData["Error"] = "You have already submitted!";
                        TempData["UseSweetAlert3"] = true;
                        TempData["SweetAlertTitle"] = "Success";
                        TempData["SweetAlertMessage"] = "You have already submitted!";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction("SweetAlert", "Home", new { UserID = UserID, CourseID = CourseID });

                    }
                }
            }

            return View(quizModel); // Return the QuizModel to the view
        }


        public ActionResult SaveMCQ(IEnumerable<ExamResponse> responses)
        {
            int courseId = Convert.ToInt32(TempData["ID"]);
            int topicId = Convert.ToInt32(TempData["TopicID"]);
            TempData["ID"] = topicId; // Updating ID with topicId
            int? UserID = (int)Session["UserID"];

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Fetch Topic Name & Course Name using a stored procedure
                    using (SqlCommand cmd = new SqlCommand("GetCourseAndTopicName", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CourseID", courseId);
                        cmd.Parameters.AddWithValue("@TopicID", topicId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TempData["CourseName"] = reader["CourseName"].ToString();
                                TempData["TopicName"] = reader["TopicName"].ToString();
                            }
                        }
                    }

                    // Initialize result tracking variables
                    int correctCount = 0;
                    int incorrectCount = 0;
                    List<QuestionReview> questionReviewList = new List<QuestionReview>();

                    foreach (var response in responses)
                    {
                        string correctAnswer = null;

                        // Fetch correct answer using stored procedure
                        using (SqlCommand cmd = new SqlCommand("GetCorrectAnswer", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@QuestionText", response.QuestionText);

                            object result = cmd.ExecuteScalar();
                            correctAnswer = result?.ToString();
                        }

                        bool isCorrect = correctAnswer == response.SelectedOption;
                        int points = isCorrect ? 1 : 0;

                        if (isCorrect) correctCount++;
                        else incorrectCount++;

                        // Create a QuestionReview object
                        var questionReview = new QuestionReview
                        {
                            QuestionText = response.QuestionText,
                            CorrectAnswer = correctAnswer,
                            SelectedAnswer = response.SelectedOption,
                            IsCorrect = isCorrect,
                            Points = points
                        };
                        questionReviewList.Add(questionReview);

                        // Save response using a stored procedure
                        using (SqlCommand cmd = new SqlCommand("InsertMCQResponse", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CourseID", courseId);
                            cmd.Parameters.AddWithValue("@TopicID", topicId);
                            cmd.Parameters.AddWithValue("@TopicName", TempData["TopicName"]);
                            cmd.Parameters.AddWithValue("@QuestionNumber", response.QuestionNumber);
                            cmd.Parameters.AddWithValue("@QuestionText", response.QuestionText);
                            cmd.Parameters.AddWithValue("@SelectedOption", response.SelectedOption);
                            cmd.Parameters.AddWithValue("@UserID", UserID);
                            cmd.Parameters.AddWithValue("@Points", points);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    TempData["CorrectQuestions"] = correctCount;
                    TempData["IncorrectQuestions"] = incorrectCount;
                    TempData["TotalPoints"] = correctCount;
                    TempData["QuestionReviews"] = questionReviewList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Inner Exception: " + ex.InnerException?.Message);
                TempData["Error"] = "Something went wrong.";
            }

            return RedirectToAction("Review");
        }


        public ActionResult Review()
        {
            try
            {
                // Retrieve necessary TempData values
                int? UserID = (int)Session["UserID"];
                int courseId = (int)TempData["ID"];

                // Retrieve the list of QuestionReviews from TempData
                List<QuestionReview> questionReviewList = TempData["QuestionReviews"] as List<QuestionReview>;

                // Retrieve other TempData values related to the results
                int correctQuestions = (int)TempData["CorrectQuestions"];
                int incorrectQuestions = (int)TempData["IncorrectQuestions"];
                int totalPoints = (int)TempData["TotalPoints"];

                // Pass values to ViewBag for usage in the View
                ViewBag.UserId = UserID;
                ViewBag.CourseId = courseId;
                ViewBag.CorrectQuestions = correctQuestions;
                ViewBag.IncorrectQuestions = incorrectQuestions;
                ViewBag.TotalPoints = totalPoints;

                // Return the view along with the question review list
                return View(questionReviewList);
            }
            catch (Exception ex)
            {
                // Handle any errors and set error message in TempData
                TempData["Error"] = "Something went wrong.";
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Inner Exception: " + ex.InnerException?.Message);

                // Redirect to error page
                return RedirectToAction("Error", "Home");
            }
        }


        internal class JsonResponse
        {
            public bool Success { get; set; }

            public string Message { get; set; }

            public string Data { get; internal set; }
        }
  }
}
