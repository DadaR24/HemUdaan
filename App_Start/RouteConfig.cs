// Decompiled with JetBrains decompiler
// Type: HEMUdaan.RouteConfig
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System.Web.Mvc;
using System.Web.Routing;

namespace HEMUdaan
{
  public class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      RouteCollectionExtensions.IgnoreRoute(routes, "{resource}.axd/{*pathInfo}");
      RouteCollectionExtensions.MapRoute(routes, "Default", "{controller}/{action}/{id}", (object) new
      {
        controller = "Home",
        action = "Index",
        id = UrlParameter.Optional
      });
    }
  }
}
