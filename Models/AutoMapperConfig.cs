// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.AutoMapperConfig
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using AutoMapper;
using System;

namespace HEMUdaan.Models
{
  public class AutoMapperConfig
  {
    public static MapperConfiguration Initialize()
    {
      return new MapperConfiguration((Action<IMapperConfigurationExpression>) (cfg =>
      {
        ((IProfileExpression) cfg).CreateMap<QuestionViewModel, QuestionDTO>();
        ((IProfileExpression) cfg).CreateMap<QuestionOptionViewModel, QuestionOptionDTO>();
      }));
    }
  }
}
