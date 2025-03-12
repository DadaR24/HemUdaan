using ClosedXML.Excel;
using HEMUdaan.EMail;
using HEMUdaan.Models;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.EntityClient;
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

namespace HEMUdaan.Controllers
{
  public class volunteerController : Controller
  {

        public ActionResult Index()
        {
            QuizModel obj = new QuizModel();
            try
            {
                int? UserID = (int)Session["UserID"];
                ViewBag.UserID = UserID;    
                // Connection string - adjust as needed based on your configuration
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Fetching CourseID for 'Health'
                    SqlCommand cmdHealth = new SqlCommand("Sp_GetCourseIDByName", connection);
                    cmdHealth.CommandType = CommandType.StoredProcedure;
                    cmdHealth.Parameters.AddWithValue("@CourseName", "Health");

                    var healthMCID = cmdHealth.ExecuteScalar(); // ExecuteScalar returns the first column of the first row
                    if (healthMCID != DBNull.Value)
                    {
                        ViewBag.HMCID = healthMCID.ToString();
                    }

                    // Fetching CourseID for 'Courage'
                    SqlCommand cmdCourage = new SqlCommand("Sp_GetCourseIDByName", connection);
                    cmdCourage.CommandType = CommandType.StoredProcedure;
                    cmdCourage.Parameters.AddWithValue("@CourseName", "Courage");

                    var courageMCID = cmdCourage.ExecuteScalar();
                    if (courageMCID != DBNull.Value)
                    {
                        ViewBag.CMCID = courageMCID.ToString();
                    }

                    // Fetching CourseID for 'Gratitude'
                    SqlCommand cmdGratitude = new SqlCommand("Sp_GetCourseIDByName", connection);
                    cmdGratitude.CommandType = CommandType.StoredProcedure;
                    cmdGratitude.Parameters.AddWithValue("@CourseName", "Gratitude");

                    var gratitudeMCID = cmdGratitude.ExecuteScalar();
                    if (gratitudeMCID != DBNull.Value)
                    {
                        ViewBag.GMCID = gratitudeMCID.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (optional logging, etc.)
                return View("Error", new HandleErrorInfo(ex, "Quiz", "Index"));
            }

            return View();
        }


        public ActionResult Creativity(string CID)
        {
            try
            {

                if (CID != null)
                {
                    Session["MCID"] = CID;
                   
                    TempData["ID"] = CID.ToString();
                    Session["MCID"] = CID;
                    ViewBag.MCID = CID.ToString();
                };

            }
            catch (Exception ex)
            {

            }

            return View();
        }

        [HttpPost]
        public ActionResult Creativity(HttpPostedFileBase UploadImageDoc, HttpPostedFileBase UploadVideoDoc, FormCollection formCollection, int MCID)
        {
            try
            {
                int VolunteerID = (int)Session["UserID"];
                int UserID = (int)Session["UserID"];
                string saveImagePath = null;
                string saveVideoPath = null;
                string user = DBManager.ExecuteScalar($"EXEC UserdetailsbyID @UserID = {VolunteerID}")?.ToString();
                if (string.IsNullOrEmpty(user))
                {
                    TempData["Error"] = "user not found.";
                    return RedirectToAction("Creativity", "volunteer", new { CID = MCID, UserID = UserID });
                }
              

                // Use stored procedure to fetch course name
                string Course = DBManager.ExecuteScalar($"EXEC GetCourseNameById @CourseID = {MCID}")?.ToString();

                if (string.IsNullOrEmpty(Course))
                {
                    TempData["Error"] = "Main course not found.";
                    return RedirectToAction("Creativity", "volunteer", new { CID = MCID, UserID = UserID });
                }

                // Check if the group is allocated using stored procedure
                string groupSelectionStatus = DBManager.ExecuteScalar($"EXEC CheckGroupAllocation @VolunteerID = {VolunteerID}, @CourseID = {MCID}")?.ToString() ?? "Pending";

                // Check for existing image activity using stored procedure
                string imageUploadStatus = DBManager.ExecuteScalar($"EXEC CheckActivitySubmission @VolunteerID = {VolunteerID}, @CourseID = {MCID}, @ActivityType = 'ImageActivity'")?.ToString() ?? "Pending";

                // Check for existing video activity using stored procedure
                string videoUploadStatus = DBManager.ExecuteScalar($"EXEC CheckActivitySubmission @VolunteerID = {VolunteerID}, @CourseID = {MCID}, @ActivityType = 'VideoActivity'")?.ToString() ?? "Pending";

                if (groupSelectionStatus == "Done")
                {
                    // Handle Image Upload
                    if (UploadImageDoc != null)
                    {
                        if (IsCreativityActivityAlreadySubmitted(VolunteerID, MCID, "ImageActivity"))
                        {
                            TempData["Error"] = "Image record already exists.";
                            return RedirectToAction("Creativity", "volunteer", new { CID = MCID, UserID = UserID });
                        }

                        saveImagePath = SaveActivityFile(UploadImageDoc, MCID, VolunteerID, "image");
                        if (!string.IsNullOrEmpty(saveImagePath))
                        {
                            imageUploadStatus = "Done";
                        }
                        else
                        {
                            TempData["Error"] = "Only .jpg, .jpeg, .png, .txt, and .pdf files are allowed.";
                            return RedirectToAction("Creativity", "volunteer", new { CID = MCID, UserID = UserID });
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Please upload an image activity.";
                        return RedirectToAction("Creativity", "volunteer", new { CID = MCID, UserID = UserID });
                    }

                    // Handle Video Upload
                    if (UploadVideoDoc != null)
                    {
                        if (IsCreativityActivityAlreadySubmitted(VolunteerID, MCID, "VideoActivity"))
                        {
                            TempData["Error"] = "Video record already exists.";
                            return RedirectToAction("Creativity", "volunteer", new { CID = MCID, UserID = UserID });
                        }

                        saveVideoPath = SaveActivityFile(UploadVideoDoc, MCID, VolunteerID, "video");
                        if (!string.IsNullOrEmpty(saveVideoPath))
                        {
                            videoUploadStatus = "Done";
                        }
                        else
                        {
                            TempData["Error"] = "Only .mp4, .mpeg4, and .mov files are allowed.";
                            return RedirectToAction("Creativity", "volunteer", new { CID = MCID, UserID = UserID });
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Please upload a video activity.";
                        return RedirectToAction("Creativity", "volunteer", new { CID = MCID, UserID = UserID });
                    }

                    // Handle Comments Submission
                    string suggestionComment = formCollection["SuggestionComment"];
                    string projectExplanationStatus = !string.IsNullOrEmpty(suggestionComment) ? "Done" : "Pending";

                    if (!string.IsNullOrEmpty(suggestionComment))
                    {
                        if (!IsCreativityActivityAlreadySubmitted(VolunteerID, MCID, "ShortDescription"))
                        {
                            SaveCreativitySubmission(VolunteerID, saveImagePath, saveVideoPath, suggestionComment, MCID);
                           // TempData["Success"] = "Submission saved successfully!";
                        }
                        else
                        {
                            TempData["Error"] = "Comments already submitted.";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Please enter a valid comment.";
                        return RedirectToAction("Creativity", "volunteer", new { CID = MCID, UserID = UserID });
                    }

                    // Generate and send email after successful submission
                    StringBuilder body = new StringBuilder(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\template\\HVMailTemplates\\CreativitySubmission.html"));
                    body.Replace("<%Course%>", Course);
                    body.Replace("<%groupSelectionStatus%>", groupSelectionStatus);
                    body.Replace("<%imageUploadStatus%>", imageUploadStatus);
                    body.Replace("<%videoUploadStatus%>", videoUploadStatus);
                    body.Replace("<%projectExplanationStatus%>", projectExplanationStatus);

                     EMailManager.SendEmailAws(user, "Creativity Submission", body.ToString(), null, "", "");
                }
                else
                {
                    TempData["Error"] = "Please create at least one group.";
                    return RedirectToAction("Creativity", "volunteer", new { CID = MCID, UserID = UserID });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred during submission.";
                // Log exception here
            }

            return RedirectToAction("Message", "volunteer");
        }

        private bool IsCreativityActivityAlreadySubmitted(int VolunteerID, int MCID, string activityType)
        {
            string query = $"SELECT * FROM HemUdaanActivitesSubmission2024 WHERE VolunteerID = {VolunteerID} AND {activityType} IS NOT NULL AND CourseID = {MCID}";
            var result = DBManager.ExecuteScalar(query);
            return result != null && result.ToString() != "";
        }

        private string SaveActivityFile(HttpPostedFileBase file, int MCID, int VolunteerID, string fileType)
        {
            string filePath = null;

            try
            {
                // Get Course Name using stored procedure
                string mcName = DBManager.ExecuteScalar1("EXEC GetCourseNameById @CourseID = @MCID", new SqlParameter("@MCID", MCID))?.ToString();

                if (string.IsNullOrEmpty(mcName)) return null;

                string guid = Guid.NewGuid().ToString();
                string instituteName = guid;
                string extension = Path.GetExtension(file.FileName).ToLower();

                if ((fileType == "image" && (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".txt" || extension == ".pdf")) ||
                    (fileType == "video" && (extension == ".mp4" || extension == ".mpeg4" || extension == ".mov")))
                {
                    string fileName = file.FileName.Substring(0, file.FileName.Length - extension.Length).Replace(" ", "_");
                    string uploadPath = $"~/Uploads/Values_of_the_year/{mcName}/{VolunteerID}";
                    string fullPath = Server.MapPath(uploadPath);

                    if (!Directory.Exists(fullPath))
                    {
                        Directory.CreateDirectory(fullPath);
                    }

                    filePath = Path.Combine(fullPath, fileName + extension);
                    file.SaveAs(filePath);
                    return filePath.Replace("~", "www.hemvirtues.com");
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return null;
            }

            return null;
        }


        private void SaveCreativitySubmission(int VolunteerID, string imagePath, string videoPath, string comment, int MCID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SP_CreativitySubmissionData_2024", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@InstID", VolunteerID));
                    cmd.Parameters.Add(new SqlParameter("@ImgAct", imagePath ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@VideoAct", videoPath ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@ShortDesc", comment));
                    cmd.Parameters.Add(new SqlParameter("@MCID", MCID));
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }


        public ActionResult Message()
        {
            int VolunteerID = (int)Session["UserID"];
            ViewBag.UserID = VolunteerID;
            return View();
        }

        public ActionResult StudentList(string TeamName,string CID)
        {
            int VolunteerID = (int)Session["UserID"];
            ViewBag.UserID = VolunteerID;
            ViewBag.GroupName = TeamName;
            //Session["ATY"] = ATY;
            Session["MCID"] = CID;

            //ViewBag.ATY = ATY;
            ViewBag.MCID = CID;

            return View();
        }
        public string StudentListMaster()

        {
            int VolunteerID = (int)Session["UserID"];
            ViewBag.UserID = VolunteerID;
            List<User> model = new List<User>();
            try
            {
                string query = "EXEC Userslist";
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

    public static string ExecuteScalar(string strSql, string connectionString = "")
    {
      try
      {
        if (string.IsNullOrEmpty(connectionString))
          connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          SqlCommand sqlCommand = new SqlCommand(strSql, connection);
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

   
    public ActionResult Voluteerlist()
    {
      using (HomeController.AppDbContext appDbContext = new HomeController.AppDbContext())
        return (ActionResult) this.View((object) ((IQueryable<Volunteer>) appDbContext.Volunteers).OrderByDescending<Volunteer, int>((Expression<System.Func<Volunteer, int>>) (u => u.VolunteerId)).ToList<Volunteer>());
    }

  
    public static List<T> ConvertToList<T>(DataTable dt)
    {
      List<string> columnNames = dt.Columns.Cast<DataColumn>().Select<DataColumn, string>((System.Func<DataColumn, string>) (c => c.ColumnName.ToLower())).ToList<string>();
      PropertyInfo[] properties = typeof (T).GetProperties();
      return dt.AsEnumerable().Select<DataRow, T>((System.Func<DataRow, T>) (row =>
      {
        T instance = Activator.CreateInstance<T>();
        foreach (PropertyInfo propertyInfo in properties)
        {
          if (columnNames.Contains(propertyInfo.Name.ToLower()))
          {
            try
            {
              propertyInfo.SetValue((object) instance, row[propertyInfo.Name]);
            }
            catch (Exception ex)
            {
            }
          }
        }
        return instance;
      })).ToList<T>();
    }

    public static DataTable ExecuteDataTable(string strSql, string connectionString = "")
    {
      SqlConnection connection = new SqlConnection();
      try
      {
        if (string.IsNullOrEmpty(connectionString))
          connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        connection = new SqlConnection(connectionString);
        SqlCommand selectCommand = new SqlCommand(strSql, connection);
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
        DataTable dataTable = new DataTable();
        connection.Open();
        selectCommand.CommandTimeout = 120;
        sqlDataAdapter.Fill(dataTable);
        connection.Close();
        return dataTable;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
      finally
      {
        if (connection != null && connection.State == ConnectionState.Open)
          connection.Close();
      }
    }

    [HttpPost]
    public JsonResult ValueCheck(string CID, int? UserID)
    {
      List<string> stringList = new List<string>();
      try
      {
        this.Session["KeepAlive"] = (object) DateTime.Now;
        ((ControllerBase) this).TempData["MCID"] = (object) CID;
        string str1 = DBManager.ExecuteScalar("SELECT * FROM HF_Rel_Manage_Group_List WHERE IsAllocatedBy = " + UserID.ToString() + " AND CourseID = '" + CID + "'");
        string str2 = DBManager.ExecuteScalar("SELECT * FROM HemUdaanActivitesSubmission2024 WHERE volunteerID = " + UserID.ToString() + " AND CourseID = '" + CID + "'  AND (ImageActivity IS NOT NULL OR VideoActivity IS NOT NULL)");
        string str3 = DBManager.ExecuteScalar("SELECT * FROM HemUdaanActivitesSubmission2024 WHERE volunteerID = " + UserID.ToString() + " AND CourseID = '" + CID + "' AND ShortDescription IS NOT NULL");
        if (!string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2) && !string.IsNullOrEmpty(str3))
          stringList.Add("0");
        else
          stringList.Add("1");
      }
      catch (Exception ex)
      {
        stringList.Add(ex.Message);
      }
      return this.Json((object) stringList, (JsonRequestBehavior) 0);
    }

        [HttpPost]
        public JsonResult CreativitySave(CreativityList[] listValues, string CID, string GroupName)
        {
            int? UserID = (int)Session["UserID"];
            List<object> list = new List<object>();
            ViewBag.GroupName = GroupName;
            Session["MCID"] = CID;
            ViewBag.MCID = CID;

            try
            {
                if (listValues.Length > 0)
                {
                    for (int i = 0; i < listValues.Length; i++)
                    {
                        var G = listValues[i].GroupName;
                        var STID = listValues[i].SchoolID;
                        var IsAllocated = listValues[i].IsAllocated;

                        // Call stored procedure to check if the student is already allocated
                        var existingRecord = DBManager.ExecuteScalar("EXEC usp_CheckExistingAllocation @StudentID='" + STID + "', @MainCourseID='" + CID + "', @GroupName='" + G + "'");

                        if (existingRecord == "")
                        {
                            // Insert the record using stored procedure
                            DBManager.ExecuteProcedure("usp_InsertCreativityAllocation", new Dictionary<string, object>
                            {
                          { "@GroupName", G },
                          { "@StudentID", STID },
                          { "@IsAllocated", IsAllocated },
                          { "@IsAllocatedBy", UserID },
                          { "@MainCourseID", CID }
                    });

                            list.Add("0");
                            list.Add("OK");
                            list.Add(CID);
                        }
                        else
                        {
                            list.Add("-1");
                            list.Add("Student is already allocated to the same MainCourse in another group.");
                            TempData["Error"] = "Student is already allocated to the same MainCourse in another group.";
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "Please upload any activity......";
                }
            }
            catch (Exception ex)
            {
                
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CASReport()
        {
           DataTable dataTable1 = new DataTable();
           DataTable dataTable2 = DBManager.ExecuteDataTable("EXEC CAS_Report ");
           dataTable2.TableName = "CreativitySubmissionReport_01";
          FileContentResult fileContentResult;
          using (XLWorkbook xlWorkbook = new XLWorkbook())
          {
             xlWorkbook.Worksheets.Add(dataTable2);
             using (MemoryStream memoryStream = new MemoryStream())
             {
                xlWorkbook.SaveAs((Stream) memoryStream);
                fileContentResult = this.File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "myFileName.xlsx");
             }
          }
            return this.Json((object) fileContentResult, (JsonRequestBehavior) 0);
           }
        }

    public class CreativityList
    {
        public string GroupName { get; set; }
        public int IsAllocated { get; set; }
        public int SchoolID { get; set; }
        public string ActivityType { get; set; }
    }
}
