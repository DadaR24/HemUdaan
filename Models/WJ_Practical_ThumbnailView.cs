// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.WJ_Practical_ThumbnailView
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;

namespace HEMUdaan.Models
{
  public class WJ_Practical_ThumbnailView
  {
    public WJ_Practical_ThumbnailView()
    {
    }

    public WJ_Practical_ThumbnailView(int practID) => this.PractID = practID;

    public int PracticalID { get; set; }

    public int CourseID { get; set; }

    public int? LevelId { get; set; }

    public int? SubmissionID { get; set; }

    public string CourseName { get; set; }

    public string LevelName { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Videolink { get; set; }

    public string ThumbnailLink { get; set; }

    public int? Order { get; set; }

    public string FileName { get; set; }

    public string FileLink { get; set; }

    public string Status { get; set; }

    public int Coins { get; set; }

    public int Gems { get; set; }

    public int? SummerCoins { get; set; }

    public string Resources { get; set; }

    public int PracticalSubtopicID { get; set; }

    public string Remark { get; set; }

    public int ApprovedByID { get; set; }

    public int PackageID { get; set; }

    public DateTime ApprovedOn { get; set; }

    public bool? ISAprroved { get; set; }

    public bool ISTask { get; set; }

    public bool IsPracticalUnlocked { get; set; }

    public DateTime CreatedOn { get; set; }

    public int Fk_SubmissionID { get; set; }

    public string FileType { get; set; }

    public string Code { get; set; }

    public bool? IsDelete { get; set; }

    public int? languageID { get; set; }

    public int PractID { get; }
  }
}
