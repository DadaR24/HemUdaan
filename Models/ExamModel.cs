// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.ExamModel
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HEMUdaan.Models
{
  public class ExamModel
  {
    public Dictionary<string, string> Standard = new Dictionary<string, string>();

    [Key]
    public int? SyllabusID { get; set; }

    public string CompetitionYear { get; set; }

    public string Standard_ { get; set; }

    public int? MainCourseID { get; set; }

    public int? CourseID { get; set; }

    public int? QuizID { get; set; }

    public int? TopicID { get; set; }

    public int? ParentTopicID { get; set; }

    public int? InstituteID { get; set; }

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public int? OrderNo { get; set; }

    public string TopicName { get; set; }

    public int SubTopicID { get; set; }

    public string Question { get; set; }

    public string Option1 { get; set; }

    public string Option2 { get; set; }

    public string Option3 { get; set; }

    public string Option4 { get; set; }

    public string Option5 { get; set; }

    public string CorrectAnswer { get; set; }

    public int LangugeID { get; set; }

    public List<SelectListItem> quiz { get; set; }

    public List<SelectListItem> maincourse { get; set; }

    public List<SelectListItem> course { get; set; }

    public List<SelectListItem> Topic { get; set; }

    public List<SelectListItem> ParentTopic { get; set; }

    public List<SelectListItem> Institutes { get; set; }

    public List<SelectListItem> Category { get; set; }

    public List<SelectListItem> SubCategory { get; set; }

    public int ID { get; set; }

    public string Mode { get; set; }

    public int LanguageID { get; set; }

    public string CourseName1 { get; set; }

    public int StudentID { get; set; }

    public class ExamModelList
    {
      public int ID { get; set; }

      public int CourseID { get; set; }

      public int TopicID { get; set; }

      public string CourseName { get; set; }

      public string TopicName { get; set; }

      public int SubTopicID { get; set; }

      public string Question { get; set; }

      public string Option1 { get; set; }

      public string Option2 { get; set; }

      public string Option3 { get; set; }

      public string Option4 { get; set; }

      public string Option5 { get; set; }

      public int LangugeID { get; set; }
    }
  }
}
