// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.CourseDTO
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System.ComponentModel.DataAnnotations;

namespace HEMUdaan.Models
{
  public class CourseDTO
  {
    public string Text { get; set; }

    public string Value { get; set; }

    public bool Selected { get; set; }

    [Key]
    public int CourseID { get; set; }

    public string CourseName { get; set; }

    public string Description { get; set; }

    public string FeaturedImage { get; set; }
  }
}
