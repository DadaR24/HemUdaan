// Decompiled with JetBrains decompiler
// Type: HEMUdaan.App.DTO.CourseCategoryLookupDTO
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

namespace HEMUdaan.App.DTO
{
  public class CourseCategoryLookupDTO
  {
    public int CategoryID { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public int? ParentID { get; set; }

    public bool IsActive { get; set; }
  }
}
