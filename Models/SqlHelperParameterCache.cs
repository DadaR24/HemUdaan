// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.SqlHelperParameterCache
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HEMUdaan.Models
{
  public static class SqlHelperParameterCache
  {
    private static readonly Hashtable ParamCache = Hashtable.Synchronized(new Hashtable());

    private static SqlParameter[] DiscoverSpParameterSet(
      SqlConnection connection,
      string spName,
      bool includeReturnValueParameter)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      SqlCommand sqlCommand = !string.IsNullOrEmpty(spName) ? new SqlCommand(spName, connection) : throw new ArgumentNullException(nameof (spName));
      sqlCommand.CommandType = CommandType.StoredProcedure;
      SqlCommand command = sqlCommand;
      connection.Open();
      SqlCommandBuilder.DeriveParameters(command);
      connection.Close();
      if (!includeReturnValueParameter)
        command.Parameters.RemoveAt(0);
      SqlParameter[] array = new SqlParameter[command.Parameters.Count];
      command.Parameters.CopyTo(array, 0);
      foreach (DbParameter dbParameter in array)
        dbParameter.Value = (object) DBNull.Value;
      return array;
    }

    private static SqlParameter[] CloneParameters(IList<SqlParameter> originalParameters)
    {
      SqlParameter[] sqlParameterArray = new SqlParameter[originalParameters.Count];
      int index = 0;
      for (int count = originalParameters.Count; index < count; ++index)
        sqlParameterArray[index] = (SqlParameter) ((ICloneable) originalParameters[index]).Clone();
      return sqlParameterArray;
    }

    public static void CacheParameterSet(
      string connectionString,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(commandText))
        throw new ArgumentNullException(nameof (commandText));
      string key = connectionString + ":" + commandText;
      SqlHelperParameterCache.ParamCache[(object) key] = (object) commandParameters;
    }

    public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(commandText))
        throw new ArgumentNullException(nameof (commandText));
      string key = connectionString + ":" + commandText;
      return !(SqlHelperParameterCache.ParamCache[(object) key] is SqlParameter[] originalParameters) ? (SqlParameter[]) null : SqlHelperParameterCache.CloneParameters((IList<SqlParameter>) originalParameters);
    }

    public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
    {
      return SqlHelperParameterCache.GetSpParameterSet(connectionString, spName, false);
    }

    public static SqlParameter[] GetSpParameterSet(
      string connectionString,
      string spName,
      bool includeReturnValueParameter)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      using (SqlConnection connection = new SqlConnection(connectionString))
        return SqlHelperParameterCache.GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
    }

    internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
    {
      return SqlHelperParameterCache.GetSpParameterSet(connection, spName, false);
    }

    internal static SqlParameter[] GetSpParameterSet(
      SqlConnection connection,
      string spName,
      bool includeReturnValueParameter)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      using (SqlConnection connection1 = (SqlConnection) ((ICloneable) connection).Clone())
        return SqlHelperParameterCache.GetSpParameterSetInternal(connection1, spName, includeReturnValueParameter);
    }

    private static SqlParameter[] GetSpParameterSetInternal(
      SqlConnection connection,
      string spName,
      bool includeReturnValueParameter)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      string key = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");
      if (!(SqlHelperParameterCache.ParamCache[(object) key] is SqlParameter[] originalParameters))
      {
        SqlParameter[] sqlParameterArray = SqlHelperParameterCache.DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
        SqlHelperParameterCache.ParamCache[(object) key] = (object) sqlParameterArray;
        originalParameters = sqlParameterArray;
      }
      return SqlHelperParameterCache.CloneParameters((IList<SqlParameter>) originalParameters);
    }
  }
}
