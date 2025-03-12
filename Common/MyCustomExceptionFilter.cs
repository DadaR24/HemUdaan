// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Common.MyCustomExceptionFilter
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;
using System.Web;
using System.Web.Mvc;

namespace HEMUdaan.Common
{
  public class MyCustomExceptionFilter : IExceptionFilter
  {
    public void OnException(ExceptionContext context)
    {
      string str1 = HttpContext.Current.Session["UserId"] as string;
      string str2 = ((ControllerContext) context).RouteData.Values["controller"] as string;
      string str3 = ((ControllerContext) context).RouteData.Values["action"] as string;
      string absoluteUri = ((ControllerContext) context).HttpContext.Request.Url.AbsoluteUri;
      new CommonUtilities().AddError(Convert.ToInt32(str1), context.Exception, "Error Message At " + str2 + "/" + str3, 0, str2 + "/" + str3, "HemUdaanDevRajan", absoluteUri);
      ((ControllerContext) context).HttpContext.Response.Redirect("~/Error");
      context.ExceptionHandled = true;
    }
  }
}
