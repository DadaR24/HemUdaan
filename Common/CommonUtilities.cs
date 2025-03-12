// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Common.CommonUtilities
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;

namespace HEMUdaan.Common
{
  public class CommonUtilities
  {
    public HttpResponseMessage AddError(
      int Userid,
      Exception ex,
      string errorMessage,
      int FeatureID,
      string MethodID,
      string Appname,
      string url)
    {
      long num = 0;
      SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
      SqlCommand sqlCommand = new SqlCommand();
      if (sqlConnection.State == ConnectionState.Closed)
        sqlConnection.Open();
      SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
      try
      {
        sqlCommand.CommandText = "HF_SP_ExceptionLoggingToDataBase";
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.Add("@ExceptionMsg", SqlDbType.VarChar, 100).Value = (object) ex.Message;
        sqlCommand.Parameters["@ExceptionMsg"].Direction = ParameterDirection.Input;
        sqlCommand.Parameters.Add("@ExceptionType", SqlDbType.VarChar, 100).Value = (object) ex.GetType().Name;
        sqlCommand.Parameters["@ExceptionType"].Direction = ParameterDirection.Input;
        sqlCommand.Parameters.Add("@ExceptionSource", SqlDbType.VarChar, 100).Value = (object) ex.Source;
        sqlCommand.Parameters["@ExceptionSource"].Direction = ParameterDirection.Input;
        sqlCommand.Parameters.Add("@ExceptionURL", SqlDbType.VarChar, 100).Value = (object) url;
        sqlCommand.Parameters["@ExceptionURL"].Direction = ParameterDirection.Input;
        sqlCommand.Parameters.Add("@Feature_ID", SqlDbType.BigInt).Value = (object) FeatureID;
        sqlCommand.Parameters["@Feature_ID"].Direction = ParameterDirection.Input;
        sqlCommand.Parameters.Add("@ProjectName", SqlDbType.VarChar, int.MaxValue).Value = (object) Appname;
        sqlCommand.Parameters["@ProjectName"].Direction = ParameterDirection.Input;
        sqlCommand.Parameters.Add("@Methods", SqlDbType.VarChar, int.MaxValue).Value = (object) MethodID;
        sqlCommand.Parameters["@Methods"].Direction = ParameterDirection.Input;
        sqlCommand.Parameters.Add("@User_ID", SqlDbType.BigInt).Value = (object) Userid;
        sqlCommand.Parameters["@User_ID"].Direction = ParameterDirection.Input;
        sqlCommand.Parameters.Add("@ErrorID", SqlDbType.VarChar, 2).Direction = ParameterDirection.Output;
        sqlCommand.Transaction = sqlTransaction;
        sqlCommand.Connection = sqlConnection;
        sqlCommand.ExecuteNonQuery();
        sqlTransaction.Commit();
        num = Convert.ToInt64(sqlCommand.Parameters["@ErrorID"].Value);
      }
      catch (Exception ex1)
      {
      }
      finally
      {
        if (sqlConnection.State == ConnectionState.Open)
          sqlConnection.Close();
      }
      return new HttpResponseMessage(HttpStatusCode.Found);
    }
  }
}
