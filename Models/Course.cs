// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.Course
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Web.Mvc;

namespace HEMUdaan.Models
{
  public class Course
  {
    [Key]
    public int CourseID { get; set; }

    public string CourseName { get; set; }

    public string Description { get; set; }

    public int NoOfSessions { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public string Author { get; set; }

    public DateTime? ExpectedDeliveryDate { get; set; }

    public string Manager { get; set; }

    public string CourseAllocatedTo { get; set; }

    public int CourseParentID { get; set; }

    public int FromAge { get; set; }

    public int ToAge { get; set; }

    public string MainCategory { get; set; }

    public string FeaturedImage { get; set; }

    public string TrainerResource { get; set; }

    public string StudentResourse { get; set; }

    public string TrainerExercise { get; set; }

    public string StudentExercise { get; set; }

    public string ApprovalStatus { get; set; }

    public string ApprovalRemark { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public Dictionary<string, string> authorlist { get; private set; }

    public Dictionary<string, string> Managerlist { get; private set; }

    public Dictionary<string, string> PrerequisiteCourselist { get; private set; }

    public Dictionary<string, string> Courseisforlist { get; private set; }

    public Dictionary<string, string> MainCategorylist { get; private set; }

    public Dictionary<string, string> Categorylist { get; private set; }

    public Dictionary<string, string> CourseActiveList { get; private set; }

    public DbSet<Course> Courses { get; set; }

    public Course()
    {
      this.Initializeauthorlist();
      this.InitializeManagerlist();
      this.InitializePrerequisiteCourselist();
      this.InitializeCourseisforlist();
      this.InitializeMainCategoryList();
      this.InitializeCategoryList();
      this.InitializeCourseActiveList();
    }

    public void Initializeauthorlist()
    {
      this.authorlist = new Dictionary<string, string>()
      {
        {
          "",
          "-- Author --"
        },
        {
          "Arvind Pandey",
          "Arvind Pandey"
        },
        {
          "Veena Apte",
          "Veena Apte"
        }
      };
    }

    public void InitializeManagerlist()
    {
      this.Managerlist = new Dictionary<string, string>()
      {
        {
          "",
          "-- Manager --"
        },
        {
          "Naveen Manthena",
          "Naveen Manthena"
        },
        {
          "Veena Apte",
          "Veena Apte"
        }
      };
    }

    public void InitializeCourseisforlist()
    {
      this.Courseisforlist = new Dictionary<string, string>()
      {
        {
          "",
          "-- Please Select --"
        },
        {
          "Students/Teacher",
          "Students/Teacher"
        },
        {
          "Teacher",
          "Teacher"
        }
      };
    }

    public void InitializePrerequisiteCourselist()
    {
      this.PrerequisiteCourselist = new Dictionary<string, string>()
      {
        {
          "",
          "-- PrerequisiteCourselist --"
        },
        {
          "HEMA FOUNDATION",
          "HEMA FOUNDATION"
        }
      };
    }

    public void InitializeMainCategoryList()
    {
      this.MainCategorylist = new Dictionary<string, string>()
      {
        {
          "",
          "-- MainCategoryList --"
        },
        {
          " Productivity",
          " Productivity"
        },
        {
          "  Creativity",
          "  Creativity"
        },
        {
          " Coding",
          " Coding"
        }
      };
    }

    public void InitializeCategoryList()
    {
      this.Categorylist = new Dictionary<string, string>()
      {
        {
          "",
          "-- CategoryList --"
        },
        {
          " Animations",
          "Animations"
        },
        {
          "HemVirtue",
          "HemVirtue"
        }
      };
    }

    public void InitializeCourseActiveList()
    {
      this.CourseActiveList = new Dictionary<string, string>()
      {
        {
          "",
          "-- CourseActiveList --"
        },
        {
          " In-Active",
          "In-Active"
        },
        {
          "Active",
          "Active"
        }
      };
    }

    public static string GenerateGuid() => Guid.NewGuid().ToString();

    public class CourseApprovalViewModel
    {
      public int CourseID { get; set; }

      public string ApprovalRemark { get; set; }

      public string ApprovalStatus { get; set; }

      public string CourseName { get; set; }
    }

    public class CourseViewModel
    {
      public Course Course { get; set; }

      public List<SelectListItem> Topics { get; set; }

      public int TopicID { get; set; }
    }
  }
}
