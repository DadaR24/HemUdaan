// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.QuestionDTO
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System.Collections.Generic;

namespace HEMUdaan.Models
{
  public class QuestionDTO
  {
    public QuestionDTO() => this.QuestionOption = new List<QuestionOptionDTO>();

    public int QuestionID { get; set; }

    public string QuestionName { get; set; }

    public int? Marks { get; set; }

    public string Type { get; set; }

    public string ComplexityLevel { get; set; }

    public string AnswerDescription { get; set; }

    public int QuestionLevel { get; set; }

    public int? CourseID { get; set; }

    public int? TopicID { get; set; }

    public int? SubTopicID { get; set; }

    public List<QuestionOptionDTO> QuestionOption { get; set; }

    public bool isAdminStatus { get; set; }

    public int? languageID { get; set; }
  }
}
