

using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace HEMUdaan.Models
{
    public class User
    {

        [Key]
        [Required]
        public int UserID { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string GuardianName { get; set; }
        public string EmergencyContact { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Std { get; set; }
        public int Specify { get; set; }
        public string SchoolName { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public string EmailID { get; set; }
        public string Location { get; set; }
        //public string State { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }

        public string Pincode { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FilePath { get; set; }

        public string GroupName { get; set; }
        public Dictionary<string, string> CityList { get; set; }

        public Dictionary<string, string> StdList { get; set; }

        public Dictionary<string, string> stateList { get; set; }

        public string UserType { get; set; }


    }
}
