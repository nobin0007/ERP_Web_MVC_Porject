
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
    public class ReceivedPaymentController : Controller
    {
        DB_Connector Db_conn = new DB_Connector();
        Json_Get_Post JsonGet = new Json_Get_Post();
        LocolModel LM = new LocolModel();



        public ActionResult PaymentVoucher()
        {
            Session["LogUserName"] = "Nobin";
            Session["LogUserID"] = "01";
            Session["VoucherTyp"] = "Payments";
            
            LM.DrCrHead_List("");
            LM.Cost_Center("");
            LM.Project("");
            LM.SubHead_List("");
            LM.SubSubHead_List("1101000");
            //  PV_RV_Table(""); 
           
            return View(LM);
        }
       
        public PartialViewResult GetPaymentDT(string VoucherNo)
        {
            GET_Voucer_Table(VoucherNo);         

            return PartialView("GetPaymentDT", LM);
        }



        [HttpPost]
        public JsonResult PaymentVoucher(FormCollection collection)
        {
            string Query = string.Empty;

            var Status = "";
            var Message = "";

            try
            {
                #region
                Query = @"INSERT INTO AccountsTransaction 
                                   (Dr_Cr_Code, CC_Name, PDate, VoucherNo,PrjCode, DTSLNO,  Account_Sub_SubCode, DrAmt, CrAmt,Cash_Cheque, ChqDate, ChqNo, TransactionType, Dr_Cr, Comments, UserID, AddDate, ComputerName,  Received_by ,PYear,DBStatus )

                                 VALUES('" + collection["CR_AccHead_List"].ToString() + @"',
                                        '" + collection["CostCenter"].ToString() + @"' ,
                                        '" + collection["VouceherDate"].ToString() + @"',
                                        '" + collection["VoucherNo"].ToString() + @"',
                                        '" + collection["Prj_list"].ToString() + @"',
                                        '" + collection["SlNo"].ToString() + @"',
                                        '" + collection["SubSubHead_List"].ToString() + @"',
                                        '" + collection["DrAmt"].ToString() + @"',
                                        '" + 0.00 + @"',
                                        '" + collection["Cash_Chq"].ToString() + @"',
                                        '" + collection["ChqDate"].ToString() + @"',
                                        '" + collection["ChqNumberTxt"].ToString() + @"',
                                        '" + LM.Trans_Type + @"',
                                        '" + "Cr" + @"',
                                        '" + collection["commend"].ToString() + @"',
                                        '" + LM.User_Id + @"',
                                        '" + LM.Current_Date + @"',
                                        '" + LM.User_Id + @"',
                                        '" + collection["PaytoTxt"].ToString() + @"',
                                        '" + LM.Current_Year + @"',
                                        'Pending' 
                                       )";

                #endregion

               Db_conn.POST_DT(Query);

               Status = "0";
               Message = "Data Save successfully!!";
            }
            catch
            {
                Status = "103";
                Message = " Unexpected Error !!";
            }

            return Json(new { Status , Message }, JsonRequestBehavior.AllowGet);
            
        }



        // GET: ReciveVoucher
        public ActionResult ReceiveVoucher()
        {
            Session["LogUserName"] = "Nobin";
            Session["LogUserID"] = "01";
            Session["VoucherTyp"] = "Receive";

            LM.DrCrHead_List("");
            LM.Cost_Center("");
            LM.Project("");
            LM.SubHead_List("");
            LM.SubSubHead_List("1101000");

            return View(LM);
        }

        public PartialViewResult GetReceivDT(string VoucherNo)
        {
            GET_Voucer_Table(VoucherNo);

            return PartialView("GetReceivDT", LM);
        }

        private void GET_Voucer_Table(string VoucherNo)
        {
            LM.Data_Table(@"SELECT  At.DTSLNO,CONVERT(varchar, At.PDate, 101) AS PDate, Prj.PrjName, At.PrjCode, ASSH.Acc_Name, At.Account_Sub_SubCode, At.VoucherNo,
                                     At.CC_Name, At.DrAmt, At.CrAmt, At.Cash_Cheque, CONVERT(varchar,At.ChqDate, 101) AS ChqDate, At.ChqNo, At.Comments, At.TransactionType
                                  FROM AccountsTransaction AS At INNER JOIN
                                        Reg_Project AS Prj ON At.PrjCode = Prj.PrjCode INNER JOIN
                                        Acc_Sub_SubHead AS ASSH ON At.Account_Sub_SubCode = ASSH.Account_Sub_SubCode
                                   WHERE (At.TransactionType =  '" + LM.Trans_Type + "') and (VoucherNo LIKE '" + VoucherNo + "%') and (At.PYear =" + LM.Current_Year+" ) ORDER BY PDate DESC, At.VoucherNo DESC, At.DTSLNO, At.Auto_SLNo DESC");
        }

        [HttpPost]
        public JsonResult ReceiveVoucher(FormCollection collection)
        {
            string Query = string.Empty;

            var Status = "";
            var Message = "";

            try
            {
                #region
                Query = @"INSERT INTO AccountsTransaction 
                                   (Dr_Cr_Code, CC_Name, PDate, VoucherNo,PrjCode, DTSLNO,  Account_Sub_SubCode, DrAmt, CrAmt,Cash_Cheque, ChqDate, ChqNo, TransactionType, Dr_Cr, Comments, UserID, AddDate, ComputerName,  Received_by ,PYear,DBStatus )

                                 VALUES('" + collection["CR_AccHead_List"].ToString() + @"',
                                        '" + collection["CostCenter"].ToString() + @"' ,
                                        '" + collection["VouceherDate"].ToString() + @"',
                                        '" + collection["VoucherNo"].ToString() + @"',
                                        '" + collection["Prj_list"].ToString() + @"',
                                        '" + collection["SlNo"].ToString() + @"',
                                        '" + collection["SubSubHead_List"].ToString() + @"',
                                        '" + 0.00 + @"',
                                        '" + collection["CrAmt"].ToString() + @"',                                        
                                        '" + collection["Cash_Chq"].ToString() + @"',
                                        '" + collection["ChqDate"].ToString() + @"',
                                        '" + collection["ChqNumberTxt"].ToString() + @"',
                                        '" + LM.Trans_Type + @"',
                                        '" + "Cr" + @"',
                                        '" + collection["commend"].ToString() + @"',
                                        '" + LM.User_Id + @"',
                                        '" + LM.Current_Date + @"',
                                        '" + LM.User_Id + @"',
                                        '" + collection["PaytoTxt"].ToString() + @"',
                                         '" + LM.Current_Year + @"',
                                        'Pending'
                                       )";

                #endregion

                Db_conn.POST_DT(Query);

                Status = "0";
                Message = "Data Save successfully!!";
            }
            catch
            {
                Status = "103";
                Message = " Unexpected Error !!";
            }

            return Json(new { Status, Message }, JsonRequestBehavior.AllowGet);

        }     


        #region  /// Common function whose return Data json and other, ( Auto_Voucher,Voucher_SL,SubSubHead_List)

        public ActionResult SubSubHead_List(string Acc_HeadCode)
        {
            LM.AccSubSubHead_List = new SelectList(LM.SubSubHead_List(Acc_HeadCode), "Value", "Text").ToList();
            //  return View(LM.AccSubSubHead_List);

            return Json(LM.AccSubSubHead_List, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Auto_Voucher(string VoucherType)
        {
          //  string VPrifix = ViewBag.Title as String;
            string PCode = "1";

            //if (VPrifix == "Receive")
            //{
            //    VPrifix = "RV";
            //}
            //else if (VPrifix == "Payments")
            //{
            //    VPrifix = "PV";
            //}

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