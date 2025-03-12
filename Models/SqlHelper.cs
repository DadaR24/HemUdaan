// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.SqlHelper
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HEMUdaan.Models
{
  public static class SqlHelper
  {
    private static void AttachParameters(
      SqlCommand command,
      IEnumerable<SqlParameter> commandParameters)
    {
      if (command == null)
        throw new ArgumentNullException(nameof (command));
      if (commandParameters == null)
        return;
      foreach (SqlParameter sqlParameter in commandParameters.Where<SqlParameter>((System.Func<SqlParameter, bool>) (p => p != null)))
      {
        if ((sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Input) && sqlParameter.Value == null)
          sqlParameter.Value = (object) DBNull.Value;
        command.Parameters.Add(sqlParameter);
      }
    }

    private static void AssignParameterValues(
      IEnumerable<SqlParameter> commandParameters,
      DataRow dataRow)
    {
      if (commandParameters == null || dataRow == null)
        return;
      int num = 0;
      foreach (SqlParameter commandParameter in commandParameters)
      {
        if (commandParameter.ParameterName == null || commandParameter.ParameterName.Length <= 1)
          throw new Exception(string.Format("Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.", (object) num, (object) commandParameter.ParameterName));
        if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
          commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
        ++num;
      }
    }

    private static void AssignParameterValues(
      SqlParameter[] commandParameters,
      object[] parameterValues)
    {
      if (commandParameters == null || parameterValues == null)
        return;
      if (commandParameters.Length != parameterValues.Length)
        throw new ArgumentException("Parameter count does not match Parameter Value count.");
      int index = 0;
      for (int length = commandParameters.Length; index < length; ++index)
      {
        if (parameterValues[index] is IDbDataParameter parameterValue)
          commandParameters[index].Value = parameterValue.Value ?? (object) DBNull.Value;
        else if (parameterValues[index] == null)
          commandParameters[index].Value = (object) DBNull.Value;
        else
          commandParameters[index].Value = parameterValues[index];
      }
    }

    private static void PrepareCommand(
      SqlCommand command,
      SqlConnection connection,
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      IEnumerable<SqlParameter> commandParameters,
      out bool mustCloseConnection)
    {
      if (command == null)
        throw new ArgumentNullException(nameof (command));
      if (string.IsNullOrEmpty(commandText))
        throw new ArgumentNullException(nameof (commandText));
      if (connection.State != ConnectionState.Open)
      {
        mustCloseConnection = true;
        connection.Open();
      }
      else
        mustCloseConnection = false;
      command.Connection = connection;
      command.CommandText = commandText;
      if (transaction != null)
        command.Transaction = transaction.Connection != null ? transaction : throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      command.CommandType = commandType;
      if (commandParameters == null)
        return;
      SqlHelper.AttachParameters(command, commandParameters);
    }

    private static async Task<bool> PrepareCommandAsync(
      SqlCommand command,
      SqlConnection connection,
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      IEnumerable<SqlParameter> commandParameters)
    {
      if (command == null)
        throw new ArgumentNullException(nameof (command));
      if (string.IsNullOrEmpty(commandText))
        throw new ArgumentNullException(nameof (commandText));
      bool mustCloseConnection = false;
      if (connection.State != ConnectionState.Open)
      {
        mustCloseConnection = true;
        await connection.OpenAsync().ConfigureAwait(false);
      }
      command.Connection = connection;
      command.CommandText = commandText;
      if (transaction != null)
        command.Transaction = transaction.Connection != null ? transaction : throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      command.CommandType = commandType;
      if (commandParameters != null)
        SqlHelper.AttachParameters(command, commandParameters);
      return mustCloseConnection;
    }

    public static int ExecuteNonQuery( string connectionString, CommandType commandType, string commandText)
    {
      return SqlHelper.ExecuteNonQuery(connectionString, commandType, commandText, (SqlParameter[]) null);
    }

    public static int ExecuteNonQuery( string connectionString,CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        return SqlHelper.ExecuteNonQuery(connection, commandType, commandText, commandParameters);
      }
    }

    public static int ExecuteNonQuery(
      string connectionString,
      string spName,
      params object[] parameterValues)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static int ExecuteNonQuery(
      SqlConnection connection,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteNonQuery(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static int ExecuteNonQuery(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      SqlCommand command = new SqlCommand();
      bool mustCloseConnection;
      SqlHelper.PrepareCommand(command, connection, (SqlTransaction) null, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters, out mustCloseConnection);
      int num = command.ExecuteNonQuery();
      command.Parameters.Clear();
      if (mustCloseConnection)
        connection.Close();
      return num;
    }

    public static int ExecuteNonQuery(
      SqlConnection connection,
      string spName,
      params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static int ExecuteNonQuery(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteNonQuery(transaction, commandType, commandText, (SqlParameter[]) null);
    }

    public static int ExecuteNonQuery(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      SqlCommand command = new SqlCommand();
      SqlHelper.PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters, out bool _);
      int num = command.ExecuteNonQuery();
      command.Parameters.Clear();
      return num;
    }

    public static int ExecuteNonQuery(
      SqlTransaction transaction,
      string spName,
      params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<int> ExecuteNonQueryAsync(
      string connectionString,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteNonQueryAsync(connectionString, commandType, commandText, (SqlParameter[]) null);
    }

    public static async Task<int> ExecuteNonQueryAsync(
      string connectionString,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      int num;
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        await connection.OpenAsync().ConfigureAwait(false);
        num = await SqlHelper.ExecuteNonQueryAsync(connection, commandType, commandText, commandParameters).ConfigureAwait(false);
      }
      return num;
    }

    public static Task<int> ExecuteNonQueryAsync(
      string connectionString,
      string spName,
      params object[] parameterValues)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteNonQueryAsync(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteNonQueryAsync(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<int> ExecuteNonQueryAsync(
      SqlConnection connection,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteNonQueryAsync(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static async Task<int> ExecuteNonQueryAsync(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      SqlCommand cmd = new SqlCommand();
      bool mustCloseConnection = await SqlHelper.PrepareCommandAsync(cmd, connection, (SqlTransaction) null, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters).ConfigureAwait(false);
      int num1 = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
      cmd.Parameters.Clear();
      if (mustCloseConnection)
        connection.Close();
      int num2 = num1;
      cmd = (SqlCommand) null;
      return num2;
    }

    public static Task<int> ExecuteNonQueryAsync(
      SqlConnection connection,
      string spName,
      params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteNonQueryAsync(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteNonQueryAsync(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<int> ExecuteNonQueryAsync(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteNonQueryAsync(transaction, commandType, commandText, (SqlParameter[]) null);
    }

    public static async Task<int> ExecuteNonQueryAsync(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      SqlCommand cmd = new SqlCommand();
      int num1 = await SqlHelper.PrepareCommandAsync(cmd, transaction.Connection, transaction, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters).ConfigureAwait(false) ? 1 : 0;
      int num2 = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
      cmd.Parameters.Clear();
      int num3 = num2;
      cmd = (SqlCommand) null;
      return num3;
    }

    public static Task<int> ExecuteNonQueryAsync(
      SqlTransaction transaction,
      string spName,
      params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteNonQueryAsync(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteNonQueryAsync(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static DataSet ExecuteDataset(
      string connectionString,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteDataset(connectionString, commandType, commandText, (SqlParameter[]) null);
    }

    public static DataSet ExecuteDataset(
      string connectionString,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        return SqlHelper.ExecuteDataset(connection, commandType, commandText, commandParameters);
      }
    }

    public static DataSet ExecuteDataset(
      string connectionString,
      string spName,
      params object[] parameterValues)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static DataSet ExecuteDataset(
      SqlConnection connection,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteDataset(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static DataSet ExecuteDataset(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      SqlCommand sqlCommand = new SqlCommand();
      bool mustCloseConnection;
      SqlHelper.PrepareCommand(sqlCommand, connection, (SqlTransaction) null, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters, out mustCloseConnection);
      using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
      {
        DataSet dataSet = new DataSet();
        sqlDataAdapter.Fill(dataSet);
        sqlCommand.Parameters.Clear();
        if (mustCloseConnection)
          connection.Close();
        return dataSet;
      }
    }

    public static DataSet ExecuteDataset(
      SqlConnection connection,
      string spName,
      params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static DataSet ExecuteDataset(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteDataset(transaction, commandType, commandText, (SqlParameter[]) null);
    }

    public static DataSet ExecuteDataset(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      SqlCommand sqlCommand = new SqlCommand();
      SqlHelper.PrepareCommand(sqlCommand, transaction.Connection, transaction, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters, out bool _);
      using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
      {
        DataSet dataSet = new DataSet();
        sqlDataAdapter.Fill(dataSet);
        sqlCommand.Parameters.Clear();
        return dataSet;
      }
    }

    public static DataSet ExecuteDataset(
      SqlTransaction transaction,
      string spName,
      params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<DataSet> ExecuteDatasetAsync(
      string connectionString,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteDatasetAsync(connectionString, commandType, commandText, (SqlParameter[]) null);
    }

    public static async Task<DataSet> ExecuteDatasetAsync(
      string connectionString,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      DataSet dataSet;
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        await connection.OpenAsync().ConfigureAwait(false);
        dataSet = await SqlHelper.ExecuteDatasetAsync(connection, commandType, commandText, commandParameters).ConfigureAwait(false);
      }
      return dataSet;
    }

    public static Task<DataSet> ExecuteDatasetAsync(
      string connectionString,
      string spName,
      params object[] parameterValues)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteDatasetAsync(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteDatasetAsync(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<DataSet> ExecuteDatasetAsync(
      SqlConnection connection,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteDatasetAsync(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static async Task<DataSet> ExecuteDatasetAsync(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      SqlCommand cmd = new SqlCommand();
      bool flag = await SqlHelper.PrepareCommandAsync(cmd, connection, (SqlTransaction) null, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters).ConfigureAwait(false);
      DataSet dataSet1;
      using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd))
      {
        DataSet dataSet2 = new DataSet();
        sqlDataAdapter.Fill(dataSet2);
        cmd.Parameters.Clear();
        if (flag)
          connection.Close();
        dataSet1 = dataSet2;
      }
      cmd = (SqlCommand) null;
      return dataSet1;
    }

    public static Task<DataSet> ExecuteDatasetAsync(
      SqlConnection connection,
      string spName,
      params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteDatasetAsync(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteDatasetAsync(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<DataSet> ExecuteDatasetAsync(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteDatasetAsync(transaction, commandType, commandText, (SqlParameter[]) null);
    }

    public static async Task<DataSet> ExecuteDatasetAsync(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      SqlCommand cmd = new SqlCommand();
      int num = await SqlHelper.PrepareCommandAsync(cmd, transaction.Connection, transaction, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters).ConfigureAwait(false) ? 1 : 0;
      DataSet dataSet1;
      using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd))
      {
        DataSet dataSet2 = new DataSet();
        sqlDataAdapter.Fill(dataSet2);
        cmd.Parameters.Clear();
        dataSet1 = dataSet2;
      }
      cmd = (SqlCommand) null;
      return dataSet1;
    }

    public static Task<DataSet> ExecuteDatasetAsync(
      SqlTransaction transaction,
      string spName,
      params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteDatasetAsync(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteDatasetAsync(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    private static SqlDataReader ExecuteReader(
      SqlConnection connection,
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      IEnumerable<SqlParameter> commandParameters,
      SqlHelper.SqlConnectionOwnership connectionOwnership)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      bool mustCloseConnection = false;
      SqlCommand command = new SqlCommand();
      try
      {
        SqlHelper.PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
        SqlDataReader sqlDataReader = connectionOwnership == SqlHelper.SqlConnectionOwnership.External ? command.ExecuteReader() : command.ExecuteReader(CommandBehavior.CloseConnection);
        bool flag = true;
        foreach (DbParameter parameter in (DbParameterCollection) command.Parameters)
        {
          if (parameter.Direction != ParameterDirection.Input)
            flag = false;
        }
        if (flag)
          command.Parameters.Clear();
        return sqlDataReader;
      }
      catch
      {
        if (mustCloseConnection)
          connection.Close();
        throw;
      }
    }

    public static SqlDataReader ExecuteReader(
      string connectionString,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteReader(connectionString, commandType, commandText, (SqlParameter[]) null);
    }

    public static SqlDataReader ExecuteReader(
      string connectionString,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      SqlConnection connection = (SqlConnection) null;
      try
      {
        connection = new SqlConnection(connectionString);
        connection.Open();
        return SqlHelper.ExecuteReader(connection, (SqlTransaction) null, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters, SqlHelper.SqlConnectionOwnership.Internal);
      }
      catch
      {
        connection?.Close();
        throw;
      }
    }

    public static SqlDataReader ExecuteReader(
      string connectionString,
      string spName,
      params object[] parameterValues)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static SqlDataReader ExecuteReader(
      SqlConnection connection,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteReader(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static SqlDataReader ExecuteReader(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      return SqlHelper.ExecuteReader(connection, (SqlTransaction) null, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters, SqlHelper.SqlConnectionOwnership.External);
    }

    public static SqlDataReader ExecuteReader(
      SqlConnection connection,
      string spName,
      params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static SqlDataReader ExecuteReader(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteReader(transaction, commandType, commandText, (SqlParameter[]) null);
    }

    public static SqlDataReader ExecuteReader(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      return SqlHelper.ExecuteReader(transaction.Connection, transaction, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters, SqlHelper.SqlConnectionOwnership.External);
    }

    public static SqlDataReader ExecuteReader(
      SqlTransaction transaction,
      string spName,
      params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    private static async Task<SqlDataReader> ExecuteReaderAsync(
      SqlConnection connection,
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      IEnumerable<SqlParameter> commandParameters,
      SqlHelper.SqlConnectionOwnership connectionOwnership)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      bool mustCloseConnection = false;
      SqlCommand cmd = new SqlCommand();
      SqlDataReader sqlDataReader1;
      try
      {
        mustCloseConnection = await SqlHelper.PrepareCommandAsync(cmd, connection, transaction, commandType, commandText, commandParameters).ConfigureAwait(false);
        SqlDataReader sqlDataReader2;
        if (connectionOwnership == SqlHelper.SqlConnectionOwnership.External)
          sqlDataReader2 = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
        else
          sqlDataReader2 = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection).ConfigureAwait(false);
        bool flag = true;
        foreach (DbParameter parameter in (DbParameterCollection) cmd.Parameters)
        {
          if (parameter.Direction != ParameterDirection.Input)
            flag = false;
        }
        if (flag)
          cmd.Parameters.Clear();
        sqlDataReader1 = sqlDataReader2;
      }
      catch
      {
        if (mustCloseConnection)
          connection.Close();
        throw;
      }
      cmd = (SqlCommand) null;
      return sqlDataReader1;
    }

    public static Task<SqlDataReader> ExecuteReaderAsync(
      string connectionString,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteReaderAsync(connectionString, commandType, commandText, (SqlParameter[]) null);
    }

    public static async Task<SqlDataReader> ExecuteReaderAsync(
      string connectionString,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      SqlConnection connection = (SqlConnection) null;
      SqlDataReader sqlDataReader;
      try
      {
        connection = new SqlConnection(connectionString);
        await connection.OpenAsync().ConfigureAwait(false);
        sqlDataReader = await SqlHelper.ExecuteReaderAsync(connection, (SqlTransaction) null, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters, SqlHelper.SqlConnectionOwnership.Internal).ConfigureAwait(false);
      }
      catch
      {
        connection?.Close();
        throw;
      }
      connection = (SqlConnection) null;
      return sqlDataReader;
    }

    public static Task<SqlDataReader> ExecuteReaderAsync(
      string connectionString,
      string spName,
      params object[] parameterValues)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteReaderAsync(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteReaderAsync(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<SqlDataReader> ExecuteReaderAsync(
      SqlConnection connection,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteReaderAsync(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static Task<SqlDataReader> ExecuteReaderAsync(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      return SqlHelper.ExecuteReaderAsync(connection, (SqlTransaction) null, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters, SqlHelper.SqlConnectionOwnership.External);
    }

    public static Task<SqlDataReader> ExecuteReaderAsync(
      SqlConnection connection,
      string spName,
      params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteReaderAsync(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteReaderAsync(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<SqlDataReader> ExecuteReaderAsync(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteReaderAsync(transaction, commandType, commandText, (SqlParameter[]) null);
    }

    public static Task<SqlDataReader> ExecuteReaderAsync(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      return SqlHelper.ExecuteReaderAsync(transaction.Connection, transaction, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters, SqlHelper.SqlConnectionOwnership.External);
    }

    public static Task<SqlDataReader> ExecuteReaderAsync(
      SqlTransaction transaction,
      string spName,
      params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteReaderAsync(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteReaderAsync(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static object ExecuteScalar(
      string connectionString,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteScalar(connectionString, commandType, commandText, (SqlParameter[]) null);
    }

    public static object ExecuteScalar(
      string connectionString,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        return SqlHelper.ExecuteScalar(connection, commandType, commandText, commandParameters);
      }
    }

    public static object ExecuteScalar(
      string connectionString,
      string spName,
      params object[] parameterValues)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static object ExecuteScalar(
      SqlConnection connection,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteScalar(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static object ExecuteScalar(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      SqlCommand command = new SqlCommand();
      bool mustCloseConnection;
      SqlHelper.PrepareCommand(command, connection, (SqlTransaction) null, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters, out mustCloseConnection);
      object obj = command.ExecuteScalar();
      command.Parameters.Clear();
      if (mustCloseConnection)
        connection.Close();
      return obj;
    }

    public static object ExecuteScalar(
      SqlConnection connection,
      string spName,
      params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static object ExecuteScalar(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteScalar(transaction, commandType, commandText, (SqlParameter[]) null);
    }

    public static object ExecuteScalar(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      SqlCommand command = new SqlCommand();
      SqlHelper.PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters, out bool _);
      object obj = command.ExecuteScalar();
      command.Parameters.Clear();
      return obj;
    }

    public static object ExecuteScalar(
      SqlTransaction transaction,
      string spName,
      params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<object> ExecuteScalarAsync(
      string connectionString,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteScalarAsync(connectionString, commandType, commandText, (SqlParameter[]) null);
    }

    public static async Task<object> ExecuteScalarAsync(
      string connectionString,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      object obj;
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        await connection.OpenAsync().ConfigureAwait(false);
        obj = await SqlHelper.ExecuteScalarAsync(connection, commandType, commandText, commandParameters).ConfigureAwait(false);
      }
      return obj;
    }

    public static Task<object> ExecuteScalarAsync(
      string connectionString,
      string spName,
      params object[] parameterValues)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteScalarAsync(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteScalarAsync(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<object> ExecuteScalarAsync(
      SqlConnection connection,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteScalarAsync(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static async Task<object> ExecuteScalarAsync(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      SqlCommand cmd = new SqlCommand();
      bool mustCloseConnection = await SqlHelper.PrepareCommandAsync(cmd, connection, (SqlTransaction) null, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters).ConfigureAwait(false);
      object obj1 = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
      cmd.Parameters.Clear();
      if (mustCloseConnection)
        connection.Close();
      object obj2 = obj1;
      cmd = (SqlCommand) null;
      return obj2;
    }

    public static Task<object> ExecuteScalarAsync(
      SqlConnection connection,
      string spName,
      params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteScalarAsync(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteScalarAsync(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<object> ExecuteScalarAsync(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteScalarAsync(transaction, commandType, commandText, (SqlParameter[]) null);
    }

    public static async Task<object> ExecuteScalarAsync(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      SqlCommand cmd = new SqlCommand();
      int num = await SqlHelper.PrepareCommandAsync(cmd, transaction.Connection, transaction, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters).ConfigureAwait(false) ? 1 : 0;
      object obj1 = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
      cmd.Parameters.Clear();
      object obj2 = obj1;
      cmd = (SqlCommand) null;
      return obj2;
    }

    public static Task<object> ExecuteScalarAsync(
      SqlTransaction transaction,
      string spName,
      params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteScalarAsync(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteScalarAsync(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static XmlReader ExecuteXmlReader(
      SqlConnection connection,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteXmlReader(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static XmlReader ExecuteXmlReader(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      bool mustCloseConnection = false;
      SqlCommand command = new SqlCommand();
      try
      {
        SqlHelper.PrepareCommand(command, connection, (SqlTransaction) null, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters, out mustCloseConnection);
        XmlReader xmlReader = command.ExecuteXmlReader();
        command.Parameters.Clear();
        return xmlReader;
      }
      catch
      {
        if (mustCloseConnection)
          connection.Close();
        throw;
      }
    }

    public static XmlReader ExecuteXmlReader(
      SqlConnection connection,
      string spName,
      params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static XmlReader ExecuteXmlReader(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteXmlReader(transaction, commandType, commandText, (SqlParameter[]) null);
    }

    public static XmlReader ExecuteXmlReader(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      SqlCommand command = new SqlCommand();
      SqlHelper.PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters, out bool _);
      XmlReader xmlReader = command.ExecuteXmlReader();
      command.Parameters.Clear();
      return xmlReader;
    }

    public static XmlReader ExecuteXmlReader(
      SqlTransaction transaction,
      string spName,
      params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<XmlReader> ExecuteXmlReaderAsync(
      SqlConnection connection,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteXmlReaderAsync(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static async Task<XmlReader> ExecuteXmlReaderAsync(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      bool mustCloseConnection = false;
      SqlCommand cmd = new SqlCommand();
      XmlReader xmlReader1;
      try
      {
        mustCloseConnection = await SqlHelper.PrepareCommandAsync(cmd, connection, (SqlTransaction) null, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters).ConfigureAwait(false);
        XmlReader xmlReader2 = await cmd.ExecuteXmlReaderAsync().ConfigureAwait(false);
        cmd.Parameters.Clear();
        xmlReader1 = xmlReader2;
      }
      catch
      {
        if (mustCloseConnection)
          connection.Close();
        throw;
      }
      cmd = (SqlCommand) null;
      return xmlReader1;
    }

    public static Task<XmlReader> ExecuteXmlReaderAsync(
      SqlConnection connection,
      string spName,
      params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteXmlReaderAsync(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteXmlReaderAsync(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<XmlReader> ExecuteXmlReaderAsync(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteXmlReaderAsync(transaction, commandType, commandText, (SqlParameter[]) null);
    }

    public static async Task<XmlReader> ExecuteXmlReaderAsync(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      SqlCommand cmd = new SqlCommand();
      int num = await SqlHelper.PrepareCommandAsync(cmd, transaction.Connection, transaction, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters).ConfigureAwait(false) ? 1 : 0;
      XmlReader xmlReader1 = await cmd.ExecuteXmlReaderAsync().ConfigureAwait(false);
      cmd.Parameters.Clear();
      XmlReader xmlReader2 = xmlReader1;
      cmd = (SqlCommand) null;
      return xmlReader2;
    }

    public static Task<XmlReader> ExecuteXmlReaderAsync(
      SqlTransaction transaction,
      string spName,
      params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteXmlReaderAsync(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteXmlReaderAsync(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static void FillDataset(
      string connectionString,
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (dataSet == null)
        throw new ArgumentNullException(nameof (dataSet));
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        SqlHelper.FillDataset(connection, commandType, commandText, dataSet, tableNames);
      }
    }

    public static void FillDataset(
      string connectionString,
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames,
      params SqlParameter[] commandParameters)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (dataSet == null)
        throw new ArgumentNullException(nameof (dataSet));
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        SqlHelper.FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
      }
    }

    public static void FillDataset(
      string connectionString,
      string spName,
      DataSet dataSet,
      string[] tableNames,
      params object[] parameterValues)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (dataSet == null)
        throw new ArgumentNullException(nameof (dataSet));
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        SqlHelper.FillDataset(connection, spName, dataSet, tableNames, parameterValues);
      }
    }

    public static void FillDataset(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames)
    {
      SqlHelper.FillDataset(connection, commandType, commandText, dataSet, tableNames, (SqlParameter[]) null);
    }

    public static void FillDataset(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames,
      params SqlParameter[] commandParameters)
    {
      SqlHelper.FillDataset(connection, (SqlTransaction) null, commandType, commandText, dataSet, tableNames, commandParameters);
    }

    public static void FillDataset(
      SqlConnection connection,
      string spName,
      DataSet dataSet,
      string[] tableNames,
      params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (dataSet == null)
        throw new ArgumentNullException(nameof (dataSet));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues != null && parameterValues.Length != 0)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
        SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
        SqlHelper.FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, spParameterSet);
      }
      else
        SqlHelper.FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
    }

    public static void FillDataset(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames)
    {
      SqlHelper.FillDataset(transaction, commandType, commandText, dataSet, tableNames, (SqlParameter[]) null);
    }

    public static void FillDataset(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames,
      params SqlParameter[] commandParameters)
    {
      SqlHelper.FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
    }

    public static void FillDataset(
      SqlTransaction transaction,
      string spName,
      DataSet dataSet,
      string[] tableNames,
      params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (dataSet == null)
        throw new ArgumentNullException(nameof (dataSet));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues != null && parameterValues.Length != 0)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
        SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
        SqlHelper.FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, spParameterSet);
      }
      else
        SqlHelper.FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
    }

    private static void FillDataset(
      SqlConnection connection,
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames,
      params SqlParameter[] commandParameters)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (dataSet == null)
        throw new ArgumentNullException(nameof (dataSet));
      SqlCommand sqlCommand = new SqlCommand();
      bool mustCloseConnection;
      SqlHelper.PrepareCommand(sqlCommand, connection, transaction, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters, out mustCloseConnection);
      using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
      {
        if (tableNames != null && tableNames.Length != 0)
        {
          string sourceTable = "Table";
          for (int index = 0; index < tableNames.Length; ++index)
          {
            if (tableNames[index] == null || tableNames[index].Length == 0)
              throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", nameof (tableNames));
            sqlDataAdapter.TableMappings.Add(sourceTable, tableNames[index]);
            sourceTable += (index + 1).ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
        }
        sqlDataAdapter.Fill(dataSet);
        sqlCommand.Parameters.Clear();
      }
      if (!mustCloseConnection)
        return;
      connection.Close();
    }

    public static async Task FillDatasetAsync(
      string connectionString,
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (dataSet == null)
        throw new ArgumentNullException(nameof (dataSet));
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        await connection.OpenAsync().ConfigureAwait(false);
        await SqlHelper.FillDatasetAsync(connection, commandType, commandText, dataSet, tableNames).ConfigureAwait(false);
      }
    }

    public static async Task FillDatasetAsync(
      string connectionString,
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames,
      params SqlParameter[] commandParameters)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (dataSet == null)
        throw new ArgumentNullException(nameof (dataSet));
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        await connection.OpenAsync().ConfigureAwait(false);
        await SqlHelper.FillDatasetAsync(connection, commandType, commandText, dataSet, tableNames, commandParameters).ConfigureAwait(false);
      }
    }

    public static async Task FillDatasetAsync(
      string connectionString,
      string spName,
      DataSet dataSet,
      string[] tableNames,
      params object[] parameterValues)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (dataSet == null)
        throw new ArgumentNullException(nameof (dataSet));
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        await connection.OpenAsync().ConfigureAwait(false);
        await SqlHelper.FillDatasetAsync(connection, spName, dataSet, tableNames, parameterValues).ConfigureAwait(false);
      }
    }

    public static Task FillDatasetAsync(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames)
    {
      return SqlHelper.FillDatasetAsync(connection, commandType, commandText, dataSet, tableNames, (SqlParameter[]) null);
    }

    public static Task FillDatasetAsync(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames,
      params SqlParameter[] commandParameters)
    {
      return SqlHelper.FillDatasetAsync(connection, (SqlTransaction) null, commandType, commandText, dataSet, tableNames, commandParameters);
    }

    public static Task FillDatasetAsync(
      SqlConnection connection,
      string spName,
      DataSet dataSet,
      string[] tableNames,
      params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (dataSet == null)
        throw new ArgumentNullException(nameof (dataSet));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.FillDatasetAsync(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.FillDatasetAsync(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, spParameterSet);
    }

    public static Task FillDatasetAsync(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames)
    {
      return SqlHelper.FillDatasetAsync(transaction, commandType, commandText, dataSet, tableNames, (SqlParameter[]) null);
    }

    public static Task FillDatasetAsync(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames,
      params SqlParameter[] commandParameters)
    {
      return SqlHelper.FillDatasetAsync(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
    }

    public static Task FillDatasetAsync(
      SqlTransaction transaction,
      string spName,
      DataSet dataSet,
      string[] tableNames,
      params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (dataSet == null)
        throw new ArgumentNullException(nameof (dataSet));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.FillDatasetAsync(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.FillDatasetAsync(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, spParameterSet);
    }

    private static async Task FillDatasetAsync(
      SqlConnection connection,
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames,
      params SqlParameter[] commandParameters)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (dataSet == null)
        throw new ArgumentNullException(nameof (dataSet));
      SqlCommand command = new SqlCommand();
      bool flag = await SqlHelper.PrepareCommandAsync(command, connection, transaction, commandType, commandText, (IEnumerable<SqlParameter>) commandParameters).ConfigureAwait(false);
      using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command))
      {
        if (tableNames != null && tableNames.Length != 0)
        {
          string sourceTable = "Table";
          for (int index = 0; index < tableNames.Length; ++index)
          {
            if (tableNames[index] == null || tableNames[index].Length == 0)
              throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", nameof (tableNames));
            sqlDataAdapter.TableMappings.Add(sourceTable, tableNames[index]);
            sourceTable += (index + 1).ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
        }
        sqlDataAdapter.Fill(dataSet);
        command.Parameters.Clear();
      }
      if (!flag)
      {
        command = (SqlCommand) null;
      }
      else
      {
        connection.Close();
        command = (SqlCommand) null;
      }
    }

    public static void UpdateDataset(
      SqlCommand insertCommand,
      SqlCommand deleteCommand,
      SqlCommand updateCommand,
      DataSet dataSet,
      string tableName)
    {
      if (insertCommand == null)
        throw new ArgumentNullException(nameof (insertCommand));
      if (deleteCommand == null)
        throw new ArgumentNullException(nameof (deleteCommand));
      if (updateCommand == null)
        throw new ArgumentNullException(nameof (updateCommand));
      if (string.IsNullOrEmpty(tableName))
        throw new ArgumentNullException(nameof (tableName));
      using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter())
      {
        sqlDataAdapter.UpdateCommand = updateCommand;
        sqlDataAdapter.InsertCommand = insertCommand;
        sqlDataAdapter.DeleteCommand = deleteCommand;
        sqlDataAdapter.Update(dataSet, tableName);
        dataSet.AcceptChanges();
      }
    }

    public static SqlCommand CreateCommand(
      SqlConnection connection,
      string spName,
      params string[] sourceColumns)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      SqlCommand sqlCommand = !string.IsNullOrEmpty(spName) ? new SqlCommand(spName, connection) : throw new ArgumentNullException(nameof (spName));
      sqlCommand.CommandType = CommandType.StoredProcedure;
      SqlCommand command = sqlCommand;
      if (sourceColumns != null && sourceColumns.Length != 0)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
        for (int index = 0; index < sourceColumns.Length; ++index)
          spParameterSet[index].SourceColumn = sourceColumns[index];
        SqlHelper.AttachParameters(command, (IEnumerable<SqlParameter>) spParameterSet);
      }
      return command;
    }

    public static int ExecuteNonQueryTypedParams(
      string connectionString,
      string spName,
      DataRow dataRow)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static int ExecuteNonQueryTypedParams(
      SqlConnection connection,
      string spName,
      DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static int ExecuteNonQueryTypedParams(
      SqlTransaction transaction,
      string spName,
      DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<int> ExecuteNonQueryTypedParamsAsync(
      string connectionString,
      string spName,
      DataRow dataRow)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteNonQueryAsync(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteNonQueryAsync(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<int> ExecuteNonQueryTypedParamsAsync(
      SqlConnection connection,
      string spName,
      DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteNonQueryAsync(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteNonQueryAsync(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<int> ExecuteNonQueryTypedParamsAsync(
      SqlTransaction transaction,
      string spName,
      DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteNonQueryAsync(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteNonQueryAsync(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static DataSet ExecuteDatasetTypedParams(
      string connectionString,
      string spName,
      DataRow dataRow)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static DataSet ExecuteDatasetTypedParams(
      SqlConnection connection,
      string spName,
      DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static DataSet ExecuteDatasetTypedParams(
      SqlTransaction transaction,
      string spName,
      DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<DataSet> ExecuteDatasetTypedParamsAsync(
      string connectionString,
      string spName,
      DataRow dataRow)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteDatasetAsync(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteDatasetAsync(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<DataSet> ExecuteDatasetTypedParamsAsync(
      SqlConnection connection,
      string spName,
      DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteDatasetAsync(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteDatasetAsync(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<DataSet> ExecuteDatasetTypedParamsAsync(
      SqlTransaction transaction,
      string spName,
      DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteDatasetAsync(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteDatasetAsync(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static SqlDataReader ExecuteReaderTypedParams(
      string connectionString,
      string spName,
      DataRow dataRow)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static SqlDataReader ExecuteReaderTypedParams(
      SqlConnection connection,
      string spName,
      DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static SqlDataReader ExecuteReaderTypedParams(
      SqlTransaction transaction,
      string spName,
      DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<SqlDataReader> ExecuteReaderTypedParamsAsync(
      string connectionString,
      string spName,
      DataRow dataRow)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteReaderAsync(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteReaderAsync(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<SqlDataReader> ExecuteReaderTypedParamsAsync(
      SqlConnection connection,
      string spName,
      DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteReaderAsync(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteReaderAsync(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<SqlDataReader> ExecuteReaderTypedParamsAsync(
      SqlTransaction transaction,
      string spName,
      DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteReaderAsync(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteReaderAsync(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static object ExecuteScalarTypedParams(
      string connectionString,
      string spName,
      DataRow dataRow)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static object ExecuteScalarTypedParams(
      SqlConnection connection,
      string spName,
      DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static object ExecuteScalarTypedParams(
      SqlTransaction transaction,
      string spName,
      DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<object> ExecuteScalarTypedParamsAsync(
      string connectionString,
      string spName,
      DataRow dataRow)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof (connectionString));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteScalarAsync(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteScalarAsync(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<object> ExecuteScalarTypedParamsAsync(
      SqlConnection connection,
      string spName,
      DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteScalarAsync(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteScalarAsync(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<object> ExecuteScalarTypedParamsAsync(
      SqlTransaction transaction,
      string spName,
      DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteScalarAsync(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteScalarAsync(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static XmlReader ExecuteXmlReaderTypedParams(
      SqlConnection connection,
      string spName,
      DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static XmlReader ExecuteXmlReaderTypedParams(
      SqlTransaction transaction,
      string spName,
      DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<XmlReader> ExecuteXmlReaderTypedParamsAsync(
      SqlConnection connection,
      string spName,
      DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteXmlReaderAsync(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteXmlReaderAsync(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static Task<XmlReader> ExecuteXmlReaderTypedParamsAsync(
      SqlTransaction transaction,
      string spName,
      DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException(nameof (transaction));
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", nameof (transaction));
      if (string.IsNullOrEmpty(spName))
        throw new ArgumentNullException(nameof (spName));
      if (dataRow == null || dataRow.ItemArray.Length == 0)
        return SqlHelper.ExecuteXmlReaderAsync(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues((IEnumerable<SqlParameter>) spParameterSet, dataRow);
      return SqlHelper.ExecuteXmlReaderAsync(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

        public static DataTable GameExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();

            // Create and open the SQL connection
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                connection.Open();

                // Create the SQL command with the provided query
                using (SqlCommand selectCommand = new SqlCommand(query, connection))
                {
                    // Add the parameters to the SQL command if any
                    if (parameters != null && parameters.Length > 0)
                    {
                        selectCommand.Parameters.AddRange(parameters);
                    }

                    // Use SqlDataAdapter to fill the DataTable with the query result
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        sqlDataAdapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }


        public static DataTable ExecuteQuery(string Query)
    {
      DataTable dataTable = new DataTable();
      string empty = string.Empty;
      using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
      {
        connection.Open();
        using (SqlCommand selectCommand = new SqlCommand(Query, connection))
        {
          using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
            sqlDataAdapter.Fill(dataTable);
        }
        connection.Close();
      }
      return dataTable;
    }

    public static string SingleExecuteQuery(string Query)
    {
      string str = string.Empty;
      DataTable dataTable = new DataTable();
      string empty = string.Empty;
      using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
      {
        connection.Open();
        using (SqlCommand sqlCommand = new SqlCommand(Query, connection))
        {
          str = Convert.ToString(sqlCommand.ExecuteScalar());
          str = !(str == "") ? "-1" : "0";
        }
        connection.Close();
      }
      return str;
    }

    public static int GameExecuteScaler(string Query)
    {
      int num = 0;
      string empty = string.Empty;
      using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
      {
        sqlConnection.Open();
        SqlCommand command = sqlConnection.CreateCommand();
        command.CommandText = Query;
        object obj = (object) (int) command.ExecuteScalar();
        if (obj != null)
          num = (int) obj;
        sqlConnection.Close();
      }
      return num;
    }

    public static string DataTableToJSONWithStringBuilder(DataTable table)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (table.Rows.Count > 0)
      {
        stringBuilder.Append("[");
        for (int index1 = 0; index1 < table.Rows.Count; ++index1)
        {
          stringBuilder.Append("{");
          for (int index2 = 0; index2 < table.Columns.Count; ++index2)
          {
            if (index2 < table.Columns.Count - 1)
              stringBuilder.Append("\"" + table.Columns[index2].ColumnName.ToString() + "\":\"" + table.Rows[index1][index2].ToString() + "\",");
            else if (index2 == table.Columns.Count - 1)
              stringBuilder.Append("\"" + table.Columns[index2].ColumnName.ToString() + "\":\"" + table.Rows[index1][index2].ToString() + "\"");
          }
          if (index1 == table.Rows.Count - 1)
            stringBuilder.Append("}");
          else
            stringBuilder.Append("},");
        }
        stringBuilder.Append("]");
      }
      return stringBuilder.ToString();
    }

        public static int GameExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            int rowsAffected = 0;

            // Create and open the SQL connection
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                connection.Open();

                // Create the SQL command with the provided query
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add the parameters to the SQL command if any
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    // Execute the command and get the number of affected rows
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected;
        }


        public static DataTable GameExecuteQuery(
      string v,
      SqlParameter sqlParameter1,
      SqlParameter sqlParameter2)
    {
      throw new NotImplementedException();
    }

        internal static int ExecuteNonQuery(string query, SqlParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        private enum SqlConnectionOwnership
    {
      Internal,
      External,
    }
  }
}
