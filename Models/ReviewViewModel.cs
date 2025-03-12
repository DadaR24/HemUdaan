// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.ReviewViewModel
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System.Collections.Generic;

namespace HEMUdaan.Models
{
  public class ReviewViewModel
  {
    public List<ReviewItem> ReviewItems { get; set; }

    public ReviewViewModel() => this.ReviewItems = new List<ReviewItem>();
  }
}
