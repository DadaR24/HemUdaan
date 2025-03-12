// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Controllers.ErrorController
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace HEMUdaan.Controllers
{
  public class ErrorController : Controller
  {
        public ActionResult Index()
        {
            string str = this.Session["Username"] as string;

            // Define the list of special user phone numbers
            List<string> specialUsers = new List<string>
            {
           "9870002011", "9820348870", "9819540279", "8076608526",
           "9892104311", "9892727705", "8971799230", "7318248634",
           "9837007165", "9619916613", "9324024888", "8169933685"
            };

            // Set DashboardUrl in ViewBag based on whether the user is in the special list
            if (specialUsers.Contains(str))
            {
                ViewBag.DashboardUrl = Url.Action("Index", "volunteer");
            }
            else
            {
                ViewBag.DashboardUrl = Url.Action("Index", "user");
            }

            return View();
        }

    }
}
