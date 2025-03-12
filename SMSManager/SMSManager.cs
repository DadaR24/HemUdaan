// Decompiled with JetBrains decompiler
// Type: HEMUdaan.SMSManager.SMSManager
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
namespace HEMUdaan.SMSManager
{
  public class SMSManager
  {
    private static string _OTPMessage = "";
    private static string _APIKey = "";
    private static string SMS_ApiKey = ConfigurationSettings.AppSettings[nameof (SMS_ApiKey)].ToString();
    private static string SMS_Sender = ConfigurationSettings.AppSettings[nameof (SMS_Sender)].ToString();
    private static string template_id = ConfigurationSettings.AppSettings[nameof (template_id)].ToString();
    private static string entity_id = ConfigurationSettings.AppSettings[nameof (entity_id)].ToString();

    public static string sendSMS(string mobile, string message)
    {
      string str = string.Empty;
      try
      {
        str = new StreamReader(new WebClient().OpenRead("https://api-alerts.kaleyra.com/v4/?api_key=" + HEMUdaan.SMSManager.SMSManager.SMS_ApiKey + "&method=sms&message=" + message + "&to=" + mobile + "&sender=" + HEMUdaan.SMSManager.SMSManager.SMS_Sender + "&entity_id=" + HEMUdaan.SMSManager.SMSManager.entity_id + "&template_id=" + HEMUdaan.SMSManager.SMSManager.template_id)).ReadToEnd();
      }
      catch (Exception ex)
      {
      }
      return str;
    }

    public static void UserRegistration( string mobileNumber, string firstName,string userName,string password)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Namaste " + firstName);
        stringBuilder.Append(", Welcome You for registration on Hemvirtues.com - A Learning Revolution. Learn value based courses through various Activities with rewards and recognition. ");
        stringBuilder.Append("Your Login Details ");
        stringBuilder.Append("UserName : " + userName + " ");
        stringBuilder.Append("Password : " + password + " ");
        stringBuilder.Append("Click Now - www.hemvirtues.com ");
        stringBuilder.Append("Call/ W'App - 7228001342 HEMA Foundation");
        HEMUdaan.SMSManager.SMSManager.sendSMS(mobileNumber, stringBuilder.ToString());
      }
      catch (Exception ex)
      {
      }
    }
  }
}
