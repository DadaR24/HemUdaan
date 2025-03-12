// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.EventDetails
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;
using System.Collections.Generic;

namespace HEMUdaan.Models
{
  public class EventDetails
  {
    public List<WJ_EventList> eventparticpatedlist { get; set; }

    public bool EventStatus { get; set; }

    public bool IsUserEventExist { get; set; }

    public bool IsUserParticipated { get; set; }

    public string UserEventStatus { get; set; }

    public DateTime EventParticpantStartDate { get; set; }

    public DateTime EventParticpantEndDate { get; set; }

    public DateTime EventStartDate { get; set; }

    public DateTime EventEndDate { get; set; }

    public DateTime EventNextStartDate { get; set; }

    public DateTime EventNextEndDate { get; set; }

    public DateTime EventmarketingDate { get; set; }

    public DateTime EventNextParticpantStartDate { get; set; }

    public DateTime EventNextParticpantEndDate { get; set; }

    public DateTime GooglerValidity { get; set; }

    public int EventID { get; set; }

    public Decimal EventAmount { get; set; }

    public int Coins { get; set; }

    public int Rank { get; set; }
  }
}
