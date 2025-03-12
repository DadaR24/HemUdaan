// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.QuestionOptionViewModel
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System.ComponentModel.DataAnnotations;

namespace HEMUdaan.Models
{
  public class QuestionOptionViewModel
  {
    [Key]
    public int? OptionID { get; set; }

    public string Options { get; set; }

    public bool IsAnswer { get; set; }

    public int DisplayOrder { get; set; }
  }
}
