// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.UserViewModel
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

namespace HEMUdaan.Models
{
  public class UserViewModel
  {
    public UserViewModel() => this.User = new User();

    public User User { get; set; }

    public bool isWhizCurriculum { get; set; }

    public string Name
    {
      get => this.User.FirstName + this.User.LastName != null ? " " + this.User.FirstName : " ";
    }

    public string FullName
    {
      get
      {
        return this.User.FirstName + this.User.LastName != null ? this.User.FirstName + " " + this.User.LastName : " ";
      }
    }

    public int GetUserID { get; set; }
  }
}
