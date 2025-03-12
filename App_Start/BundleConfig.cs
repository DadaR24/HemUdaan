// Decompiled with JetBrains decompiler
// Type: HEMUdaan.BundleConfig
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;
using System.Web.Optimization;

namespace HEMUdaan
{
    public class BundleConfig
  {
    public static void RegisterBundles(BundleCollection bundles)
    {
      bundles.Add(((Bundle) new ScriptBundle("~/bundles/jquery")).Include("~/Scripts/jquery-{version}.js", Array.Empty<IItemTransform>()));
      bundles.Add(((Bundle) new ScriptBundle("~/bundles/jqueryval")).Include("~/Scripts/jquery.validate*", Array.Empty<IItemTransform>()));
      bundles.Add(((Bundle) new ScriptBundle("~/bundles/modernizr")).Include("~/Scripts/modernizr-*", Array.Empty<IItemTransform>()));
      bundles.Add(new Bundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", Array.Empty<IItemTransform>()));
      bundles.Add(((Bundle) new StyleBundle("~/Content/css")).Include(new string[2]
      {
        "~/Content/css/bootstrap.css",
        "~/Content/css/site.css"
      }));
    }
  }
}
