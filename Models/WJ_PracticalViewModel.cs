// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.WJ_PracticalViewModel
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace HEMUdaan.Models
{
  public class WJ_PracticalViewModel
  {
    [Key]
    public int? PracticalID { get; set; }

    public int CourseID { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public int? OrderNo { get; set; }

    public string FileType { get; set; }

    public int? Coins { get; set; }

    public int? Gems { get; set; }

    public string Thumbnail { get; set; }

    public string Resources { get; set; }

    public string VideoLink { get; set; }

    public int? PracticalLevel { get; set; }

    public string Code { get; set; }

    public string Student_Group { get; set; }

    public int? PracticalTopicID { get; set; }

    public int? PracticalSubtopicID { get; set; }

    public string PracticalStandard { get; set; }

    public bool isTask { get; set; }

    public int? languageID { get; set; }

    public int? ScrambleTopicID { get; set; }

    public string Type { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int CreatedBy { get; set; }
  }
}
