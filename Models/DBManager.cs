// Decompiled with JetBrains decompiler
// Type: HEMUdaan.Models.DBManager
// Assembly: HEMUdaan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F757A9B-7BB0-4710-887C-617B716CA126
// Assembly location: C:\Users\rajankumar.thakur\Desktop\unsorted\HemUdaan\HEMUdaan.dll

using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HEMUdaan.Models
{
  public static class DBManager
  {
        public static DataSet ExecuteDataSet(string strSql, params SqlParameter[] parameters)
        {
            DataSet dataSet = new DataSet();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand selectCommand = new SqlCommand(strSql, connection);

                    if (parameters != null)
                        selectCommand.Parameters.AddRange(parameters);

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);

                    connection.Open();
                    selectCommand.CommandTimeout = 120;
                    sqlDataAdapter.Fill(dataSet);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging framework if you have one)
                throw new Exception("Error executing SQL query: " + ex.Message);
            }

            return dataSet;
        }

        public static object ExecuteScalar1(string commandText, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
        }


        public static void ExecuteNonQuery(string strSql, SqlParameter[] parameters, string connectionString = "")
{
    try
    {
        if (string.IsNullOrEmpty(connectionString))
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand sqlCommand = new SqlCommand(strSql, connection))
            {
                sqlCommand.Parameters.AddRange(parameters); // Adding parameters to the SqlCommand

                connection.Open();
                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
    catch (Exception ex)
    {
        throw new Exception($"Error executing SQL query: {ex.Message}", ex); // Improved exception message
    }
}


    public static List<T> ExecuteDapper<T>(string strSql, string connectionString = "")
    {
      SqlConnection sqlConnection = new SqlConnection();
      try
      {
        if (string.IsNullOrEmpty(connectionString))
          connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        sqlConnection = new SqlConnection(connectionString);
        sqlConnection.Open();
        IList<T> list = (IList<T>) SqlMapper.Query<T>((IDbConnection) sqlConnection, strSql, (object) null, (IDbTransaction) null, true, new int?(), new CommandType?()).ToList<T>();
        sqlConnection.Close();
        return list.ToList<T>();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
      finally
      {
        if (sqlConnection != null && sqlConnection.State == ConnectionState.Open)
          sqlConnection.Close();
      }
    }

    public static string ExecuteScalar(string strSql, string connectionString = "")
    {
      try
      {
        if (string.IsNullOrEmpty(connectionString))
          connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          SqlCommand sqlCommand = new SqlCommand(strSql, connection);
          sqlCommand.Connection.Open();
          sqlCommand.CommandTimeout = 21600;
          string str = Convert.ToString(sqlCommand.ExecuteScalar());
          sqlCommand.Connection.Close();
          return str;
        }
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }
        public static List<T> ConvertToList<T>(DataTable dt)
        {

            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex) { }
                    }
                }
                return objT;
            }).ToList();
        }
        public static DataTable ExecuteDataTable(string strSql, string connectionString = "")
    {
      SqlConnection connection = new SqlConnection();
      try
      {
        if (string.IsNullOrEmpty(connectionString))
          connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        connection = new SqlConnection(connectionString);
        SqlCommand selectCommand = new SqlCommand(strSql, connection);
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
        DataTable dataTable = new DataTable();
        connection.Open();
        selectCommand.CommandTimeout = 120;
        sqlDataAdapter.Fill(dataTable);
        connection.Close();
        return dataTable;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
      finally
      {
        if (connection != null && connection.State == ConnectionState.Open)
          connection.Close();
      }
    }

        public static void ExecuteProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the command
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
