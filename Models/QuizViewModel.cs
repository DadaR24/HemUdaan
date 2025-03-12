// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.QuizViewModel
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HEMUdaan.Models
{
  public class QuizViewModel
  {
    public QuizViewModel()
    {
      this.CourseID = new List<int>();
      this.LevelID = new List<int>();
      this.CourseAllocationID = new List<int>();
    }

    public int QuizID { get; set; }

    [DisplayName("Name")]
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime StartDate { get; set; }

    public string StartDateDisplay => this.StartDate.ToString("yyyy-MM-dd");

    public DateTime EndDate { get; set; }

    public string EndDateDisplay => this.EndDate.ToString("yyyy-MM-dd");

    public DateTime StartTime { get; set; }

    public string StartTimeDisplay => this.StartTime.ToString("HH:mm");

    public DateTime EndTime { get; set; }

    public string EndTimeDisplay => this.EndTime.ToString("HH:mm");

    public DateTime? EndDateTime { get; set; }

    public int? Duration { get; set; }

    [Display(Name = "Duration Type")]
    public string Type { get; set; }

    [Required]
    [Display(Name = "Passing")]
    public int? PassMarks { get; set; }

    [Required]
    [Display(Name = "Total Marks")]
    public int? TotalMarks { get; set; }

    [Required]
    [Display(Name = "No. of Attempts Allowed")]
    public int? Attempts { get; set; }

    [Display(Name = "Exam Active?")]
    public bool IsActive { get; set; }

    public int SingleLevelID { get; set; }

    public string LevelName { get; set; }

    public bool WhizJuniorSyllabus { get; set; }

    public string CourseAllocatedTo { get; set; }

    public string MCQLevel { get; set; }

    public int Participants { get; set; }

    public List<int> CourseID { get; set; }

    public List<int> LevelID { get; set; }

    public List<int> CourseAllocationID { get; set; }

    public List<CourseModel> Courses { get; set; }

    public List<CourseModel> CourseAllocation { get; set; }

    public List<LevelModel> Levels { get; set; }
  }
}
