// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.Courses
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;

namespace HEMUdaan.Models
{
  public class Courses
  {
    public int CourseID { get; set; }

    public string CourseName { get; set; }

    public int NoOfSessions { get; set; }

    public string Description { get; set; }

    public Decimal? Fees { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public string FeaturedImage { get; set; }

    public int? Author { get; set; }

    public bool? isPublished { get; set; }

    public DateTime? PublishDate { get; set; }

    public DateTime? ExpectedDeliveryDate { get; set; }

    public string ApprovalRemark { get; set; }

    public string ApprovalStatus { get; set; }

    public DateTime? ApprovedOn { get; set; }

    public int? ApprovedBy { get; set; }

    public string TrainerResource { get; set; }

    public string StudentResourse { get; set; }

    public string TrainerExercise { get; set; }

    public string StudentExercise { get; set; }

    public int? Manager { get; set; }

    public string CourseAllocatedTo { get; set; }

    public int? CourseParentID { get; set; }

    public int? FromAge { get; set; }

    public int? ToAge { get; set; }

    public string MainCategory { get; set; }

    public int mcqcount { get; set; }

    public int Cup1 { get; set; }

    public int practicalpercent { get; set; }
  }
}
