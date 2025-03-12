// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.SessionUser
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace HEMUdaan.Models
{
  public static class SessionUser
  {
    private static string _userSessionName = nameof (SessionUser);

    public static UserViewModel User
    {
      set => HttpContext.Current.Session[SessionUser._userSessionName] = (object) value;
      get
      {
        UserViewModel user = new UserViewModel();
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
          if (HttpContext.Current.Session[SessionUser._userSessionName] != null)
          {
            user = (UserViewModel) HttpContext.Current.Session[SessionUser._userSessionName];
          }
          else
          {
            IdentityExtensions.GetUserId(HttpContext.Current.User.Identity);
            UsersBAL usersBal = new UsersBAL();
            UserViewModel userViewModel = new UserViewModel();
            HttpContext.Current.Session[SessionUser._userSessionName] = (object) userViewModel;
            user = userViewModel;
          }
        }
        if (user == null)
          user = new UserViewModel();
        return user;
      }
    }

    public static List<string> Roles
    {
      get
      {
        return ((ClaimsIdentity) HttpContext.Current.User.Identity).Claims.Where<Claim>((Func<Claim, bool>) (c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")).Select<Claim, string>((Func<Claim, string>) (c => c.Value)).ToList<string>();
      }
    }

    public static bool isUserInAnyOfRoles(params string[] roles)
    {
      foreach (string role in roles)
      {
        if (HttpContext.Current.User.IsInRole(role))
          return true;
      }
      return false;
    }

    public static bool isUserInAllRoles(params string[] roles)
    {
      foreach (string role in roles)
      {
        if (!HttpContext.Current.User.IsInRole(role))
          return false;
      }
      return true;
    }

    public static List<string> getUserRoles()
    {
      string[] allRoles = System.Web.Security.Roles.GetAllRoles();
      List<string> userRoles = new List<string>();
      foreach (string role in allRoles)
      {
        if (HttpContext.Current.User.IsInRole(role))
          userRoles.Add(role);
      }
      return userRoles;
    }

    public static void Flush()
    {
      HttpContext.Current.Session[SessionUser._userSessionName] = (object) null;
      HttpContext.Current.Session.Abandon();
    }
  }
}
