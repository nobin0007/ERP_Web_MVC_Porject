using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ERP_Web_MVC_Porject.Models.DB_Connectior_File
{
    public class DB_Conn_String_Class
    {
       public string Sql_DBCon1
        {
            get
            {
                //return ("SERVER=ADSERVER/SQLEXPRESS;DRIVER=SQL SERVER;DATABASE=AMG;UID=AMG_ERP;PWD=Passkey1000");

                string xx = @"Server = ADSERVER\SQLExpress; DATABASE = AMG; UID = AMG_ERP; PWD = Passkey1000 ";

                return (xx);
               
            }

        }

        public string Sql_DBCon
        {
            get
            {
                //return ("SERVER=ADSERVER/SQLEXPRESS;DRIVER=SQL SERVER;DATABASE=AMG;UID=AMG_ERP;PWD=Passkey1000");

                string Constring = @"Server=localhost; DATABASE = ERP_Database; UID = sa; PWD = 123 ";

                return (Constring);

            }

        }


    }
}