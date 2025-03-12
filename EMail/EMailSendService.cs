// Decompiled with JetBrains decompiler
// Type: HEMUdaan.EMail.EMailSendService
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using HEMUdaan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace HEMUdaan.EMail
{
  public class EMailSendService
  {
    public static void UserRegistration(User obj)
    {
      try
      {
        StringBuilder stringBuilder1 = new StringBuilder();
        stringBuilder1.Append(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\template\\HVMailTemplates\\UserRegistration.html"));
        if (obj == null)
          return;
        StringBuilder stringBuilder2 = stringBuilder1.Replace("<%FirstName%>", obj.FirstName).Replace("<%UserName%>", obj.MobileNumber).Replace("<%Password%>", obj.MobileNumber);
        EMailManager.SendEmailAws(obj.EmailID, "Registration Confirmation", stringBuilder2.ToString(), (List<HttpPostedFileBase>) null, "", "");
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }
  }
}
