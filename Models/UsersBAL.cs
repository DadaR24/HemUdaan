// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.UsersBAL
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using HEMUdaan.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HEMUdaan.Models
{
  public class UsersBAL
  {
    public User GetUserByID(int UserID)
    {
      using (HomeController.AppDbContext appDbContext = new HomeController.AppDbContext())
        return ((IQueryable<User>) appDbContext.Users).Where<User>((Expression<Func<User, bool>>) (x => x.UserID == UserID)).FirstOrDefault<User>();
    }

    public List<User> GetAllTrainersUsers() => throw new NotImplementedException();
  }
}
