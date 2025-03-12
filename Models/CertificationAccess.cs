// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.CertificationAccess
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System.Collections.Generic;

namespace HEMUdaan.Models
{
  public class CertificationAccess
  {
    public WJ_Certification_Access Certification { get; set; }

    public WJ_student_Certification_Access studentCertificationAccess { get; set; }

    public List<WJ_Certification_Access> CertificationList { get; set; }

    public string EndtO { get; set; }

    public CertificationAccess()
    {
      this.Certification = new WJ_Certification_Access();
      this.studentCertificationAccess = new WJ_student_Certification_Access();
      this.CertificationList = new List<WJ_Certification_Access>();
    }

    public int CertificationCount { get; set; }

    public int VipID { get; set; }

    public int PercentComplete { get; set; }
  }
}
