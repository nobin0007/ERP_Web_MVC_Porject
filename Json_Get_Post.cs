using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;


namespace ERP_Web_MVC_Porject.Models.DB_Connectior_File
{
    public class Json_Get_Post
    {
        private SqlConnection conn = new SqlConnection();
        private DataTable dt = new DataTable();
        private SqlCommand cmd = new SqlCommand();
        private SqlDataAdapter da = new SqlDataAdapter();
        private SqlTransaction trans = null;

        public void SqlconOpen()
        {
            try
            {
                DB_Conn_String_Class ConClass = new DB_Conn_String_Class();
                conn = new SqlConnection(ConClass.Sql_DBCon);

                ConnectionState state = conn.State;
                if (state == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                else
                {
                    conn.Open();
                }
            }
            catch (Exception )
            {
              
            }

        }

        public void SqlconClose()
        {
            try
            {
                DB_Conn_String_Class ConClass = new DB_Conn_String_Class();
                conn = new SqlConnection(ConClass.Sql_DBCon);

                ConnectionState state = conn.State;
                if (state == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (Exception )
            {
                
            }

        }

        public String Gat_DT(string sql)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlconOpen();
                cmd = new SqlCommand(sql, conn);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return  JsonConvert.SerializeObject(dt, Formatting.Indented);

            }
            catch (Exception ex)
            {
                conn.Close();
                throw ;
               
            }
            finally
            {
                SqlconClose();
            }

        }

        public bool POST_DATA(string qurry)
        {
            try
            {
                SqlconOpen();

                trans = conn.BeginTransaction();
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = trans;
                cmd.CommandText = qurry;

                cmd.ExecuteNonQuery();

                trans.Commit();

                return true;

            }
            catch (Exception)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                return false;
            }
            finally
            {
                SqlconClose();
            }
        }

        public void POST_DT(string Qstring)
        {
            try
            {
                SqlconOpen();
                cmd = new SqlCommand(Qstring, conn);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                SqlconClose();
               
            }
            finally
            {
                SqlconClose();
            }
        }

        //public void CallProcedure(Array sqlpra, string procidurname)  // call Procedur whith Paramiter and Procidure Name
        //{
        //    try
        //    {
        //        SqlconOpen();

        //        cmd = new SqlCommand(procidurname, conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        if (sqlpra != null)
        //        {
        //            cmd.Parameters.AddRange(sqlpra);
        //            cmd.ExecuteNonQuery();
        //        }
        //        else
        //        {
        //            cmd.ExecuteNonQuery();
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        SqlconClose();
               
        //    }

        //}

        //public bool ProExecut(String Procedoure_Name) // call Procedur whithout Paramiter 
        //{
        //    try
        //    {
        //        SqlconOpen();

        //        SqlCommand cmd = new SqlCommand(Procedoure_Name, conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.ExecuteNonQuery();
        //        cmd.Dispose();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //}

        //public string  Procedur_Re_DT(Array sqlpra, string procidurname)  // call Procedur whith Paramiter and Procidure Name Return Tabel 
        //{
        //    try
        //    {
        //        SqlconOpen();

        //        cmd = new SqlCommand(procidurname.ToString(), conn);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        if (sqlpra != null)
        //        {
        //            cmd.Parameters.AddRange(sqlpra);
        //            da = new SqlDataAdapter(cmd);
        //            da.Fill(dt);
        //        }
        //        else
        //        {
        //            da = new SqlDataAdapter(cmd);
        //            da.Fill(dt);
        //        }
        //        SqlconClose();

        //        return JsonConvert.SerializeObject(dt, Formatting.Indented);

        //    }
            
        //    finally
        //    {
        //        SqlconClose();
        //    }

        //}

    }
}