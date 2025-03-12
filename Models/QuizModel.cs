// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.QuizModel
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;
using System.Collections.Generic;

namespace HEMUdaan.Models
{
  public class QuizModel
  {
    public List<WJ_Practical_ThumbnailView> pracs = new List<WJ_Practical_ThumbnailView>();

    public QuizModel()
    {
      this.QuizCourseLevels = (ICollection<HEMUdaan.Models.QuizCourseLevels>) new HashSet<HEMUdaan.Models.QuizCourseLevels>();
      this.Courses = (ICollection<HEMUdaan.Models.Courses>) new HashSet<HEMUdaan.Models.Courses>();
    }

    public List<Levels> levels { get; set; }

    public List<HEMUdaan.Models.Topics> Topics { get; set; }

    public List<HEMUdaan.Models.Topics> Topicss { get; set; }

    public string CourseName1 { get; set; }

    public int LanguageID { get; set; }

    public int Gems { get; set; }

    public int InstituteId { get; set; }

    public int CertificationQuizID { get; set; }

    public string Wait { get; set; }

    public int StudentID { get; set; }

    public int CourseID { get; set; }

    public int Credit { get; set; }

    public string Free { get; set; }

    public string UseCredit { get; set; }

    public string Error { get; set; }

    public string CourseName { get; set; }

    public string Description { get; set; }

    public string FeaturedImage { get; set; }

    public string Video { get; set; }

    public bool IsStarted { get; set; }

    public string IsStartMessage { get; set; }

    public int Stage { get; set; }

    public string IsStartCertification { get; set; }

    public string IsExistCertification { get; set; }

    public int LevelID { get; set; }

    public string LevelName { get; set; }

    public string IsInstituteQuiz { get; set; }

    public int getcup1 { get; set; }

    public int getcup2 { get; set; }

    public int getcup3 { get; set; }

    public int StdInstituteID { get; set; }

    public int PracticalID { get; set; }

    public int QuizID { get; set; }

    public string Name { get; set; }

    public string TopicName { get; set; }

    public string Standard { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? EndDateTime { get; set; }

    public int? Duration { get; set; }

    public string Type { get; set; }

    public int? PassMarks { get; set; }

    public int? TotalMarks { get; set; }

    public int? Attempts { get; set; }

    public bool? IsActive { get; set; }

    public bool IsPractical { get; set; }

    public bool? WhizJuniorSyllabus { get; set; }

    public string CourseAllocatedTo { get; set; }

    public string MCQLevel { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string TrainerExercise { get; set; }

    public string TrainerResources { get; set; }

    public bool ISTask { get; set; }

    public int TopicID { get; set; }

    public string ThumbnailLink { get; set; }

    public string Title { get; set; }

    public string Resources { get; set; }

    public int? Order { get; set; }

    public int ParentTopicID { get; set; }

    public string SubTopic { get; set; }

    public List<HEMUdaan.Models.Topics> topics { get; set; }

    public List<UserTopicModel> UserTopic { get; set; }

    public int subtopics { get; set; }

    public virtual ICollection<HEMUdaan.Models.QuizCourseLevels> QuizCourseLevels { get; set; }

    public virtual ICollection<HEMUdaan.Models.Courses> Courses { get; set; }
  }
}
