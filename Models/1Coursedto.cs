// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.Coursedto
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;
using System.Collections.Generic;

namespace HEMUdaan.Models
{
  public class Coursedto
  {
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

    public List<CourseDTO> Courses { get; set; }
  }
}
