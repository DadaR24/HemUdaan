// Decompiled with JetBrains decompiler
// Type: HEMUdaan.EMail.EMailManager
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace HEMUdaan.EMail
{
  public class EMailManager
  {
    private static string Host = "smtp.gmail.com";
    private static string UserName = ConfigurationManager.AppSettings["SMTPUserName"];
    private static string Password = ConfigurationManager.AppSettings["SMTPPassword"];
    private static string Title = "HEM-Virtues";
    private static string GmailPassword = "";

    public static void SendEmail(
      string to,
      string subject,
      string usermessage,
      string adminmessage)
    {
      using (MailMessage message = new MailMessage())
      {
        message.From = new MailAddress(EMailManager.UserName);
        message.To.Add(to);
        message.Subject = subject;
        message.Body = usermessage;
        message.IsBodyHtml = true;
        using (SmtpClient smtpClient = new SmtpClient(EMailManager.Host, 587))
        {
          smtpClient.UseDefaultCredentials = true;
          smtpClient.Credentials = (ICredentialsByHost) new NetworkCredential(EMailManager.UserName, EMailManager.Password);
          smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
          smtpClient.EnableSsl = true;
          smtpClient.Send(message);
        }
      }
    }

    public static void SendEmailAws(
      string to,
      string subject,
      string Body,
      List<HttpPostedFileBase> attachments,
      string CC,
      string BCC)
    {
      try
      {
        string str = "hemvirtues@hemafoundation.org";
        using (MailMessage message = new MailMessage())
        {
          SmtpClient smtpClient = new SmtpClient(Convert.ToString(EMailManager.Host));
          message.From = new MailAddress(Convert.ToString(str));
          message.To.Add(to);
          smtpClient.Port = 587;
          smtpClient.Credentials = (ICredentialsByHost) new NetworkCredential(Convert.ToString(EMailManager.UserName), Convert.ToString(EMailManager.Password));
          smtpClient.EnableSsl = true;
          if (attachments != null && attachments.Count > 0)
          {
            foreach (HttpPostedFileBase attachment in attachments)
            {
              if (attachment != null)
              {
                string fileName = Path.GetFileName(attachment.FileName);
                message.Attachments.Add(new Attachment(attachment.InputStream, fileName));
              }
            }
          }
          if (!string.IsNullOrEmpty(CC))
            message.CC.Add(CC);
          if (!string.IsNullOrEmpty(BCC))
            message.Bcc.Add(BCC);
          message.Subject = subject;
          message.Body = Body;
          message.IsBodyHtml = true;
          smtpClient.Send(message);
          message.Dispose();
        }
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    public static string SendEmailwithImageBody(
      string to,
      string subject,
      string htmlMessage,
      string Image)
    {
      string str1 = string.Empty;
      bool flag = false;
      try
      {
        string empty = string.Empty;
        string str2 = "hemvirtues@hemafoundation.org";
        using (MailMessage message = new MailMessage())
        {
          SmtpClient smtpClient = new SmtpClient(Convert.ToString(EMailManager.Host));
          message.From = new MailAddress(Convert.ToString(str2));
          message.To.Add(to);
          AlternateView alternateViewFromString = AlternateView.CreateAlternateViewFromString(htmlMessage, Encoding.UTF8, "text/html");
          string str3 = ConfigurationManager.AppSettings["ImagePath"] + "\\" + string.Format("{0}", (object) Guid.NewGuid()) + ".jpg";
          byte[] bytes = Convert.FromBase64String(Image);
          if (System.IO.File.Exists(str3))
            System.IO.File.Delete(str3);
          System.IO.File.WriteAllBytes(str3, bytes);
          string mediaType = "image/jpeg";
          LinkedResource linkedResource = new LinkedResource(str3, mediaType);
          linkedResource.ContentId = "EmbeddedContent_1";
          linkedResource.ContentType.MediaType = mediaType;
          linkedResource.TransferEncoding = TransferEncoding.Base64;
          linkedResource.ContentType.Name = linkedResource.ContentId;
          linkedResource.ContentLink = new Uri("cid:" + linkedResource.ContentId);
          alternateViewFromString.LinkedResources.Add(linkedResource);
          message.AlternateViews.Add(alternateViewFromString);
          message.IsBodyHtml = true;
          smtpClient.Port = 587;
          smtpClient.Credentials = (ICredentialsByHost) new NetworkCredential(Convert.ToString(EMailManager.UserName), Convert.ToString(EMailManager.Password));
          smtpClient.EnableSsl = true;
          message.Subject = subject;
          smtpClient.Send(message);
          flag = true;
          str1 = str3;
        }
      }
      catch (Exception ex)
      {
        flag = false;
      }
      return str1;
    }

    public static string SendEmail_Certification_Result(
      string to,
      string subject,
      string usermessage,
      string adminmessage,
      string SchoolEmail)
    {
      try
      {
        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Port = 587;
        smtpClient.Host = "smtp.gmail.com";
        smtpClient.EnableSsl = true;
        smtpClient.Timeout = 10000;
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = (ICredentialsByHost) new NetworkCredential(EMailManager.UserName, EMailManager.GmailPassword);
        MailMessage message = new MailMessage(EMailManager.UserName, to, subject, usermessage);
        message.From = new MailAddress(EMailManager.UserName, EMailManager.Title);
        if (SchoolEmail != null && SchoolEmail != "")
          message.CC.Add(SchoolEmail);
        message.Bcc.Add("hemafoundation@rrglobal.com");
        message.IsBodyHtml = true;
        message.BodyEncoding = Encoding.UTF8;
        message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        smtpClient.Send(message);
        return "1";
      }
      catch (Exception ex)
      {
        return "0";
      }
    }
  }
}
