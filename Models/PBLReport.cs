using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HEMUdaan.Models
{
    public class PBLReport
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string Std {  get; set; }
        public string InstituteName { get; set; }
        public string CourseName { get; set; }  
        public string Title { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileLink { get; set; }
        public int IsCreatedDate { get; set; }
    }
}