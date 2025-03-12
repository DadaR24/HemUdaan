using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HEMUdaan.Models
{
    public class ReportModel
    {
        public int UserID { get; set; }
        public int QuestionNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Std {  get; set; }
        public string SchoolName { get; set; }
        public string MobileNumber { get; set; }
        public string CourseName { get; set; }
        public string TopicName { get; set; }
        public string Questions { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string CorrectAnswer { get; set; }
        public string UserSelectedAnswer { get; set; }
        public int Points {  get; set; }
    }
}