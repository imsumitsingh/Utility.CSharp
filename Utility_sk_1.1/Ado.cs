﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CSharp.Utility
{
  public static class Ado
    {
        private static string _connectionString=null;
        private static SqlConnection con=null;
        private static void SetConnection(string ConnectionStringName)
        {
            _connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
            con = new SqlConnection(_connectionString);
        }
       static  DataSet ds1;
       static SqlDataAdapter da1;
       static SqlCommand cmd1;


        
       

        


       
       
        public static string SetData(string query)
        {
            SqlCommand com = new SqlCommand(query, con);
            try
            {
                if (con.State == ConnectionState.Closed)
                { con.Open(); }
                com.CommandTimeout = 0;
                com.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                { con.Close(); }
                return "SUSS";
            }
            catch (Exception)
            {
                return "";
            }

        }
        public static DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandText = query;
                com.CommandTimeout = 0;
                if (con.State == ConnectionState.Closed)
                { con.Open(); }

                SqlDataAdapter dad = new SqlDataAdapter(com);
                using (dad)
                {

                    dad.Fill(dt);
                }
                if (con.State == ConnectionState.Open)
                { con.Close(); }
            }
            catch
            {
                dt = null;
            }
            return dt;

        }
     
        public static Decimal GetScalerDecimal(string QueryString)
        {
            Decimal Value;

            SqlCommand cmd = new SqlCommand(QueryString, con);
            if (con.State == ConnectionState.Closed)
            { con.Open(); }
            cmd.CommandTimeout = 0;
            object ob;
            try
            {
                ob = cmd.ExecuteScalar();
            }
            catch (SqlException)
            {
                ob = 0;
            }


            if (con.State == ConnectionState.Open)
            { con.Close(); }
            if (ob == DBNull.Value)
            {
                Value = 0;
            }
            else
            {
                try
                {
                    Value = Decimal.Parse(ob.ToString());
                }
                catch (Exception)
                {
                    Value = 0;
                }
            }
            return (Value);


        }
        public static int GetScalerInt(string QueryString)
        {
            int Value;

            SqlCommand cmd = new SqlCommand(QueryString, con);
            if (con.State == ConnectionState.Closed)
            { con.Open(); }
            cmd.CommandTimeout = 0;
            object ob;
            try
            {
                ob = cmd.ExecuteScalar();
            }
            catch (SqlException)
            {
                ob = 0;
            }


            if (con.State == ConnectionState.Open)
            { con.Close(); }
            if (ob == DBNull.Value)
            {
                Value = 0;
            }
            else
            {
                try
                {
                    Value = int.Parse(ob.ToString());
                }
                catch (Exception)
                {
                    Value = 0;
                }
            }
            return (Value);


        }
        public static string GetScalerString(string QueryString)
        {
            string Value;
            SqlCommand cmd = new SqlCommand(QueryString, con);
            if (con.State == ConnectionState.Closed)
            { con.Open(); }
            cmd.CommandTimeout = 0;
            object ob = cmd.ExecuteScalar();
            if (con.State == ConnectionState.Open)
            { con.Close(); }
            try
            {
                if (ob == DBNull.Value)
                {
                    Value = "";
                }
                else
                {
                    Value = Convert.ToString(ob);
                }
                return (Value);
            }
            catch (System.NullReferenceException)
            {
                Value = "";
                return (Value);
            }

        }
        public static Double GetScalerDouble(string QueryString)
        {
            Double Value;

            SqlCommand cmd = new SqlCommand(QueryString, con);
            if (con.State == ConnectionState.Closed)
            { con.Open(); }
            cmd.CommandTimeout = 0;
            object ob;
            try
            {
                ob = cmd.ExecuteScalar();
            }
            catch (SqlException)
            {
                ob = 0;
            }


            if (con.State == ConnectionState.Open)
            { con.Close(); }
            if (ob == DBNull.Value)
            {
                Value = 0;
            }
            else
            {
                try
                {
                    Value = Double.Parse(ob.ToString());
                }
                catch (Exception)
                {
                    Value = 0;
                }
            }
            return (Value);


        }
        public static DateTime GetScalerDateTime(string QueryString)
        {

            DateTime Value = DateTime.Now;
            SqlCommand cmd = new SqlCommand(QueryString, con);
            cmd.CommandTimeout = 0;
            if (con.State == ConnectionState.Closed)
            { con.Open(); }
            object ob = cmd.ExecuteScalar();
            if (con.State == ConnectionState.Open)
            { con.Close(); }
            if (ob == DBNull.Value)
            {
                //Value = DateTime.Parse(date.ToString());
            }
            else
            {
                try
                {
                    Value = DateTime.Parse(ob.ToString());
                }
                catch
                {

                }
            }
            return Value;
        }
      

        //--------------------My Functions----------------------------//
       
       
      
      
        
        public static bool IsExists(string TableName, string WhereColumn, string WhereValue)
        {
            bool f = false;
            try
            {
                if (string.IsNullOrEmpty(WhereColumn) || string.IsNullOrWhiteSpace(WhereColumn))
                {
                    f = false;
                    return f;
                }
               string sql = @"select " + WhereColumn + " from  " + TableName + "  where  " + WhereColumn + " ='" + WhereValue + "' ";
                sql = Ado.GetScalerString(sql);
                if (sql != "")
                {
                    f = true;
                }
            }
            catch (Exception)
            {
            }
            return f;
        }


       
      



        public static DataTable GetAllRecords(string tablename)
        {
           DataTable dt = Ado.GetData("select * from " + tablename);
            return dt;
        }

        public static bool IsEmpty(string tablename)
        {
            if (Ado.GetScalerInt("select count(*) from " + tablename) <1)
            {
                return true;
            }
            return false;
        }


        


     

    }
}
