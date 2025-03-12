// Decompiled with JetBrains decompiler
// Type: HEMUdaan.FilterConfig
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using HEMUdaan.Common;
using System.Web.Mvc;

namespace HEMUdaan
{
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add((object) new HandleErrorAttribute());
      filters.Add((object) new MyCustomExceptionFilter());
    }
  }
}
