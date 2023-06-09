﻿
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DAL
{
    /// <summary>
    ///  Database Manager : Call the Database by calling all ADO functions with StoredProcedure in the SQL Server
    /// </summary>
    public class DBManager
    {
        private readonly SqlConnection sqlConn;
        private readonly SqlCommand sqlCommand;
        private SqlDataAdapter dataAdapter;
        private DataTable DT;

        public DBManager()
        {
            sqlConn = new SqlConnection(@ConfigurationManager.ConnectionStrings["NorthwindCS"].ConnectionString);

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConn;
            sqlCommand.CommandType = CommandType.StoredProcedure;

            dataAdapter = new SqlDataAdapter(sqlCommand);

            DT = new DataTable();
        }

        public int ExecuteNonQuery(string storedProcedureName)
        {
            int R = -1;
            try
            {
                if (sqlConn.State == ConnectionState.Closed)
                    sqlConn.Open();

                sqlCommand.CommandText = storedProcedureName;
                sqlCommand.Parameters.Clear();
                R = sqlCommand.ExecuteNonQuery();

                sqlConn.Close();

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            return R;
        }

        public object? ExecuteScalar(string storedProcedureName)
        {
            object? o = null;
            try
            {
                if (sqlConn.State == ConnectionState.Closed)
                    sqlConn.Open();

                sqlCommand.CommandText = storedProcedureName;
                sqlCommand.Parameters.Clear();

                o = sqlCommand.ExecuteScalar();

                sqlConn.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            return o;
        }

        public DataTable ExecuteDataTable(string storedProcedureName , CommandType type = CommandType.StoredProcedure)
        {
            DT.Clear();
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandType = type;
                sqlCommand.CommandText = storedProcedureName;

                dataAdapter.Fill(DT);

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }

            return DT;
        }


        // Overloading functions for parameters 
        public  int ExecuteNonQuery(string storedProcedureName , Dictionary<string , object> Params)
        {
            int R = -1;
            try
            {
                if (sqlConn.State == ConnectionState.Closed && Params.Count > 0)
                    sqlConn.Open();

                sqlCommand.CommandText = storedProcedureName;
                sqlCommand.Parameters.Clear();
                foreach (var param in Params)
                {
                    sqlCommand.Parameters.AddWithValue(param.Key, param.Value);
                }
                R = sqlCommand.ExecuteNonQuery(); // this will throw exception because of ForeignKey  

                sqlConn.Close();

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            return R;
        }

    }
}