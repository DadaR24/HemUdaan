// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.QuizCourseLevels
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

namespace HEMUdaan.Models
{
  public class QuizCourseLevels
  {
    public int QuizID { get; set; }

    public int LevelID { get; set; }

    public int CourseID { get; set; }

    public virtual Courses Courses { get; set; }

    public virtual Levels Levels { get; set; }
  }
}
