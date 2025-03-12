// Decompiled with JetBrains decompiler
// Type: HEMUdaan.MvcApplication
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using HEMUdaan.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HEMUdaan
{
  public class MvcApplication : HttpApplication
  {
    protected void Application_Start(object sender, EventArgs e)
    {
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      AutoMapperConfig.Initialize();
      AreaRegistration.RegisterAllAreas();
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);

    }
  }
}
