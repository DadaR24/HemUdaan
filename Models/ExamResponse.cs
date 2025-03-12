// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.ExamResponse
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System.ComponentModel.DataAnnotations;

namespace HEMUdaan.Models
{
  public class ExamResponse
  {
    [Key]
    public int QuestionNumber { get; set; }

    public string QuestionText { get; set; }

    public string SelectedOption { get; set; }

    public int CourseID { get; set; }

    public int TopicID { get; set; }

    public string TopicName { get; set; }

    public int UserID { get; set; }

    public int Point { get; set; }
  }
}
