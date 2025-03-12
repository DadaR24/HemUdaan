// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.WJ_EventList
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;

namespace HEMUdaan.Models
{
  public class WJ_EventList
  {
    public int EventID { get; set; }

    public int FK_EventTypeID { get; set; }

    public string Name { get; set; }

    public bool IsPaid { get; set; }

    public bool IsActive { get; set; }

    public string Message { get; set; }

    public DateTime? Created_On { get; set; }

    public DateTime? EventStartDate { get; set; }

    public DateTime? EventEndDate { get; set; }

    public Decimal? EventAmount { get; set; }

    public bool? Status { get; set; }

    public DateTime? EventParticipantStartDate { get; set; }

    public DateTime? EventParticipantEndDate { get; set; }

    public DateTime? EventMarketingEndDate { get; set; }

    public bool isParticipated { get; set; }
  }
}
