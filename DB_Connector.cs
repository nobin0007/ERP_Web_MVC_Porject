using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace ERP_Web_MVC_Porject.Models.DB_Connectior_File
{
    public class DB_Connector
    {
        private SqlConnection conn = new SqlConnection();
        private DataTable dt = new DataTable();
        private SqlCommand cmd = new SqlCommand();
        private SqlDataAdapter da = new SqlDataAdapter();
        private SqlTransaction trans = null;

        public void SqlconOpen()
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

        public void SqlconClose()
        {
           
                DB_Conn_String_Class ConClass = new DB_Conn_String_Class();
                conn = new SqlConnection(ConClass.Sql_DBCon);

                ConnectionState state = conn.State;
                if (state == ConnectionState.Open)
                {
                    conn.Close();
                }
         
        }

        public DataTable Gat_DT(string SqlQuery)
        {
            DataTable dt = new DataTable();
          
                SqlconOpen();
                cmd = new SqlCommand(SqlQuery.ToString(), conn);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                SqlconClose();

              return dt;

        }


        public bool POST_DATA(string qurry)
        {        SqlconOpen();

                trans = conn.BeginTransaction();
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = trans;
                cmd.CommandText = qurry;

                cmd.ExecuteNonQuery();

                trans.Commit();
                SqlconClose();
                return true;          
        }

        public void POST_DT(string qurry)
        {
            try
            {
                SqlconOpen();
                cmd = new SqlCommand(qurry, conn);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException )
            {
                SqlconClose();
               
            }
            finally
            {
                SqlconClose();
            }
        }

        public void CallProcedure(Array sqlpra, string procidurname)  // call Procedur whith Paramiter and Procidure Name
        {
            try
            {
                SqlconOpen();

                cmd = new SqlCommand(procidurname, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (sqlpra != null)
                {
                    cmd.Parameters.AddRange(sqlpra);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception )
            {
                SqlconClose();
             
            }

        }

        public bool ProExecut(String Procedoure_Name) // call Procedur whithout Paramiter 
        {
            try
            {
                SqlconOpen();

                SqlCommand cmd = new SqlCommand(Procedoure_Name, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public DataTable Procedur_Re_DT(Array sqlpra, string procidurname)  // call Procedur whith Paramiter and Procidure Name Return Tabel 
        {
            try
            {
                SqlconOpen();

                cmd = new SqlCommand(procidurname, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (sqlpra != null)
                {
                    cmd.Parameters.AddRange(sqlpra);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                else
                {
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                SqlconClose();

                return dt;

            }
          
            finally
            {
                SqlconClose();
            }

        }


    }
}