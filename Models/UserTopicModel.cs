// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.UserTopicModel
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;

namespace HEMUdaan.Models
{
  public class UserTopicModel
  {
    public int TopicID { get; set; }

    public int CourseID { get; set; }

    public int? ParentTopicID { get; set; }

    public string TopicName { get; set; }

    public string Description { get; set; }

    public string TrainerVideo { get; set; }

    public string StudentVideo { get; set; }

    public string HindiVideo { get; set; }

    public string TrainerNote { get; set; }

    public string StudentNote { get; set; }

    public int? Order { get; set; }

    public bool? IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string TrainerResource { get; set; }

    public string StudentResourse { get; set; }

    public string TrainerExercise { get; set; }

    public string StudentExercise { get; set; }

    public int? languageID { get; set; }

    public int? NoOfQuestion { get; set; }

    public string ParentTopicName { get; set; }

    public int? SubTopics { get; set; }

    public string CourseName { get; set; }

    public int? NoOfSessions { get; set; }

    public string CourseDescription { get; set; }

    public string CourseFeaturedImage { get; set; }

    public string CourseAllocatedTo { get; set; }

    public string CourseApprovalStatus { get; set; }

    public string ParentTopicDescription { get; set; }

    public int? ParentTopicOrder { get; set; }

    public int Marks_Obtain { get; set; }

    public int TotalMarks { get; set; }

    public bool isAttempted { get; set; }

    public int ExamID { get; set; }
  }
}
