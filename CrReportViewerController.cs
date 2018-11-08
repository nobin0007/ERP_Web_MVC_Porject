
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Web.Mvc;


namespace ERP_Web_MVC_Porject.Controllers
{
    public class CrReportViewerController : Controller
    {

        #region   Report paramiter object

       ReportDocument RptDoc = new ReportDocument();


        private ReportDocument _RptDoc;

        public ReportDocument RptDocs
        {
            get { return _RptDoc; }
            set { _RptDoc = value; }
        }

        private string _RptFormula;
        public string RptFormula
        {
            get { return _RptFormula; }
            set { _RptFormula = value; }
        }

        private string _RptName;
        public string RptName
        {
            get { return _RptName; }
            set { _RptName = value; }
        }

        private string _Rpt_PrinterName;
        private readonly object ClientScript;

        public string Rpt_PrinterName
        {
            get { return _Rpt_PrinterName; }
            set { _Rpt_PrinterName = value; }
        }


        #endregion



        Stream stream = null;


        public FileStreamResult GetReportPDF1(string RptName, string RptFormula)
        {
            
          //  if(Session[""])

            RptDoc.Load(Server.MapPath("~/Cr_Report/" + RptName));

            if (RptDoc != null && CrRpt_DBLoging() == true)
            {
                if (RptFormula != string.Empty && RptFormula != null)
                {
                    RptDoc.RecordSelectionFormula = RptFormula;
                }

                RptFormula = "";
                RptName = "";

               stream = RptDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            


            }
            else
            {
                RptDoc = null;
                stream = null;
            }


            return File(stream, "Application/PDF");

        }


        public Stream GetReportPDF(string RptName, string RptFormula)
        {

            //  if(Session[""])

            RptDoc.Load(Server.MapPath("~/Cr_Report/" + RptName));

            if (RptDoc != null && CrRpt_DBLoging() == true)
            {
                if (RptFormula != string.Empty && RptFormula != null)
                {
                    RptDoc.RecordSelectionFormula = RptFormula;
                }

                RptFormula = "";
                RptName = "";

                stream = RptDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                
            }
            else
            {
                RptDoc = null;
                stream = null;
            }


            return stream ;

        }






        private bool CrRpt_DBLoging()
        {
            try
            {
                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(@"Server=localhost; DATABASE = ERP_Database; UID = sa; PWD = 123 ");

                // SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Session["SQL_CONN_String"].ToString());
                ConnectionInfo ci = new ConnectionInfo();

                ci.UserID = builder.UserID;
                ci.Password = builder.Password;
                ci.ServerName = builder.DataSource;
                ci.DatabaseName = builder.InitialCatalog;

                foreach (CrystalDecisions.CrystalReports.Engine.Table tbl in RptDoc.Database.Tables)
                {
                    TableLogOnInfo logon = tbl.LogOnInfo;
                    logon.ConnectionInfo = ci;
                    tbl.ApplyLogOnInfo(logon);
                    tbl.Location = tbl.Location;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }





    }
}