
using ERP_Web_MVC_Porject.Models;
using ERP_Web_MVC_Porject.Models.DB_Connectior_File;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ERP_Web_MVC_Porject.Controllers
{
    public class JournalVrController : Controller
    {
        DB_Connector Db_conn = new DB_Connector();
        Json_Get_Post JsonGet = new Json_Get_Post();
        LocolModel LM = new LocolModel();

        DataTable dt =new DataTable();

        string Status = "";
        string Message = "";

        // GET: JournaVoucher
        public ActionResult JournalVr()
        {
            Session["JV_Table"] = null;

            Session["LogUserName"] = "Nobin";
            Session["LogUserID"] = "01";
            Session["VoucherTyp"] = "Journal";

          
            LM.Cost_Center("");
            LM.Project("");
            LM.SubHead_List("");
            LM.SubSubHead_List("");

            return View(LM);
        }

        public PartialViewResult GetJournalDT(string VoucherNo)
         {
            dt = (DataTable)Session["JV_Table"];

            if (dt == null)
            {
                LM.Data_Table(@"SELECT  At.DTSLNO,CONVERT(varchar, At.PDate, 101) AS PDate, Prj.PrjName, At.PrjCode, ASSH.Acc_Name, At.Account_Sub_SubCode, At.VoucherNo,
                                     At.CC_Name, At.CrAmt, At.DrAmt, At.Comments, At.TransactionType,At.Dr_Cr
                                  FROM AccountsTransaction AS At INNER JOIN
                                        Reg_Project AS Prj ON At.PrjCode = Prj.PrjCode INNER JOIN
                                        Acc_Sub_SubHead AS ASSH ON At.Account_Sub_SubCode = ASSH.Account_Sub_SubCode
                                   WHERE (At.TransactionType =  '" + LM.Trans_Type + "') and (VoucherNo LIKE '" + VoucherNo + "%') and (At.PYear =" + LM.Current_Year + " ) ORDER BY At.PDate DESC, At.VoucherNo, At.DTSLNO, At.Auto_SLNo ");

            }
            else
            {            

                LM.DataTable = dt.AsEnumerable();
            }
            
            return PartialView("GetJournalDT", LM);
        }



        [HttpPost]
        public JsonResult JournalVr(FormCollection collection)
        {
            decimal DrAmt = 0;
            decimal CrAmt = 0;


            try
            {
               
                if (collection["DrCr_Radio"].ToString() == "Dr")
                {
                     DrAmt =Convert.ToDecimal( collection["Amount"].ToString());

                }
                else 
                {
                    CrAmt = Convert.ToDecimal(collection["Amount"].ToString());
                }

                CreatTable();
                
                var Prj_list = LM.Project(collection["Prj_list"].ToString()).ToList()[0];              
                var CostCentert = LM.Cost_Center(collection["CostCenter"].ToString()).ToList()[0];             
                var SubSubHead = LM.SubSubHead_List(collection["SubSubHead_List"].ToString()).ToList()[0];            

                dt.Rows.Add(
                        collection["SlNo"].ToString(),
                        collection["VouceherDate"].ToString(),
                        collection["VoucherNo"].ToString(),
                        Prj_list.Text,
                        Prj_list.Value,
                        CostCentert.Value,
                        CostCentert.Text,
                        SubSubHead.Text,
                        SubSubHead.Value,
                        DrAmt,
                        CrAmt,
                        collection["DrCr_Radio"].ToString(),
                        LM.Trans_Type,
                        collection["commend"].ToString()                        
                    );

                Session["JV_Table"] = dt;

                PartialView("GetJournalDT", LM);

                Status = "0";
                Message = "Data Add ";
            }
            catch (Exception ex)
            {
                Status = "103";
                Message = " Unexpected Error !!  :" + ex;
            }

            return Json(new { Status, Message }, JsonRequestBehavior.AllowGet);

        }

        private void CreatTable()
        {
            if ((DataTable)Session["JV_Table"] == null)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("DTSLNO", typeof(string)));
                dt.Columns.Add(new DataColumn("PDate", typeof(string)));
                dt.Columns.Add(new DataColumn("VoucherNo", typeof(string)));
                dt.Columns.Add(new DataColumn("PrjName", typeof(string)));
                dt.Columns.Add(new DataColumn("PrjCode", typeof(string)));
                dt.Columns.Add(new DataColumn("CC_Name", typeof(string)));
                dt.Columns.Add(new DataColumn("CC_Code", typeof(string)));
                dt.Columns.Add(new DataColumn("Acc_Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Account_Sub_SubCode", typeof(string)));
                dt.Columns.Add(new DataColumn("DrAmt", typeof(decimal)));
                dt.Columns.Add(new DataColumn("CrAmt", typeof(decimal)));
                dt.Columns.Add(new DataColumn("Dr_Cr", typeof(string)));
                dt.Columns.Add(new DataColumn("TransactionType", typeof(string)));
                dt.Columns.Add(new DataColumn("Comments", typeof(string)));               
            }

            else
            {
                dt = (DataTable)Session["JV_Table"];
            }
           
        }

        [HttpPost]
        public ActionResult JVPOST(FormCollection collection)
        {
            dt = (DataTable)Session["JV_Table"];

            if ( dt != null)
            {
                try
                {
                    string QureyValu = "";

                    if (dt.Rows.Count > 1)
                    {

                       string Query = @"INSERT INTO AccountsTransaction  (PYear, PDate, DTSLNO, VoucherNo, PrjCode, CC_Name, Account_Sub_SubCode, DrAmt, CrAmt, TransactionType, Dr_Cr, Comments,
                                                                            UserID,  AddDate, Received_by, DBStatus)";
                        
                        foreach (DataRow dr in dt.Rows)
                        {
                            QureyValu += Query + @" VALUES  (                '" + LM.Current_Year + @"',
                                                                            '" + dr["PDate"].ToString() + @"',
                                                                            '" + dr["DTSLNO"].ToString() + @"',
                                                                            '" + dr["VoucherNo"].ToString() + @"',
                                                                            '" + dr["PrjCode"].ToString() + @"',
                                                                            '" + dr["CC_Code"].ToString() + @"',
                                                                            '" + dr["Account_Sub_SubCode"].ToString() + @"',
                                                                            '" + dr["DrAmt"].ToString() + @"',
                                                                            '" + dr["CrAmt"].ToString() + @"',
                                                                            '" + dr["TransactionType"].ToString() + @"',
                                                                            '" + dr["Dr_Cr"].ToString() + @"',
                                                                            '" + dr["Comments"].ToString() + @"',
                                                                            '" + LM.User_Id + @"',                                                                            
                                                                            '" + LM.Current_Date + @"',
                                                                            '" + LM.User_Id + @"',
                                                                            'Pending'
                                                                         )";


                         

                        }
                     Db_conn.POST_DT(QureyValu);
                   
                    }
                  
                }
                catch (Exception ex)
                {
                    Status = "103";
                    Message = " Unexpected Error !!  :" + ex;
                }


            }
            else
            {
                Status = "103";
                Message = " NO Vouecher well add !!  :" ;
            }

            return RedirectToAction("JournalVr");
            

        }
        

        #region  /// Common function whose return Data json and other, ( Auto_Voucher,Voucher_SL,SubSubHead_List)

        public ActionResult SubSubHead_List(string Acc_HeadCode)
        {
            LM.AccSubSubHead_List = new SelectList(LM.SubSubHead_List(Acc_HeadCode), "Value", "Text").ToList();           

            return Json(LM.AccSubSubHead_List, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Auto_Voucher(string VoucherType)
        {
            string PCode = "1";

            String Data = LM.Auto_Voucher(VoucherType, PCode);
            return Json(new { Data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Voucher_SL(string Voucher_ID)
        {
            String Data = LM.Voucher_SL(Voucher_ID);
            return Json(new { Data }, JsonRequestBehavior.AllowGet);
        }


        #endregion





    }
}