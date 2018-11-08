using System;

using System.Linq;

using System.Web.Mvc;
using ERP_Web_MVC_Porject.Models.DB_Connectior_File;
using System.Data;
using ERP_Web_MVC_Porject.Models;

namespace ERP_Web_MVC_Porject.Controllers
{
    public class ChartOfAccController : Controller
    {
        DB_Connector db_conn = new DB_Connector();
        Json_Get_Post JsonGet = new Json_Get_Post();
        //  GolobalClass GL = new GolobalClass();
        LocolModel G_Model = new LocolModel();

        // GET: ChartOfAcc
        public ActionResult ChartOfAccount()
        {
            System.Web.HttpContext.Current.Session["LogUserName"] = "Nobin";

            AccHead_Data();
            SubHead_Data();

            string Query = @"SELECT MainCode, Account_Code, Account_Head FROM Acc_Head";
            ViewData["AccHeadList"] = G_Model.ToSelectList(Query, "Account_Head", "Account_Code");

            return View();
        }



        //Post: ChartOfAcc
        [HttpPost]
        public ActionResult ChartOfAccount(FormCollection collection, string FormID)
        {
            string Query = string.Empty;
            try
            {

                if (FormID == "HeadForm")
                {
                    Query = @"INSERT INTO Acc_Head  (MainCode, Account_Code, Account_Head)
                                      VALUES('" + collection["options"].ToString() + "','" + collection["Account_Code"].ToString() + "','" + collection["Account_Name"].ToString() + "')";
                    // db_conn.POST_DT(Query);
                }

                else if (FormID == "SubHeadForm")
                {
                    Query = @"INSERT INTO Acc_SubHead  (Account_Code, Account_SubCode, Account_Header)
                                      VALUES ('" + collection["AccHeadSelector"].ToString() + "','" + collection["SubHeadCode"].ToString() + "','" + collection["SubHedName"].ToString() + "') ";
                    // db_conn.POST_DT(Query);
                }

                ViewData["msg"] = "Succesfull";
                ChartOfAccount();
            }
            catch
            {
                ViewData["msg"] = "UnSuccesfull";
            }
            return View();
        }

        public void AccHead_Data()
        {
            string Query = @"SELECT Main_Head.MainCode, Main_Head.Account_Head AS MinHade, Acc_Head.Account_Code, Acc_Head.Account_Head
                                     FROM Acc_Head INNER JOIN
                                           Main_Head ON Acc_Head.MainCode = Main_Head.MainCode";

            DataTable dt = db_conn.Gat_DT(Query);
            ViewBag.AccHeadDT = dt.AsEnumerable();

        }

        public void SubHead_Data()
        {
            string Query = @"SELECT Acc_Head.Account_Code, Acc_Head.Account_Head, Acc_SubHead.Account_SubCode, Acc_SubHead.Account_Header
                                     FROM  Acc_Head INNER JOIN
                                           Acc_SubHead ON Acc_Head.Account_Code = Acc_SubHead.Account_Code";

            DataTable dt = db_conn.Gat_DT(Query);
            ViewBag.SubHeadDT = dt.AsEnumerable();
        }

        [HttpPost]
        public JsonResult AccHeadMaxCoed(String MainHeadCode)
        {
            string Query = @"SELECT MAX(Account_Code) + 100000 AS HeadMaxCode   FROM   Acc_Head GROUP BY MainCode HAVING  (MainCode = '" + MainHeadCode + "')";

            string Data = JsonGet.Gat_DT(Query);
            return Json(new { Data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubHeadMaxCoed(string SubHeadCode)
        {
            string Query = @"SELECT  isnull( MAX(Account_SubCode) + 1000 ,0)AS MaxSubHeadCode
                                FROM  Acc_SubHead
                                 GROUP BY Account_Code
                                        HAVING (Account_Code = '" + SubHeadCode + "')";

            string Data = JsonGet.Gat_DT(Query);
            return Json(new { Data }, JsonRequestBehavior.AllowGet);

        }


        // GET: ChartOfAcc
        public ActionResult AccLedger()
        {
            System.Web.HttpContext.Current.Session["LogUserName"] = "Nobin";

            ViewData["HeadList"] = G_Model.HeadList("");
            

           string Query = @"SELECT  ASSH.Account_SubCode, ASH.Account_Header, ASSH.Account_Sub_SubCode, ASSH.Acc_Name, ASSH.Address + ASSH.City AS Address, City,ASSH.PostCode, ASSH.Phone, ASSH.MobileNo, ASSH.EMail, ASSH.Comments, ASSH.SubLedger FROM dbo.Acc_Sub_SubHead AS ASSH INNER JOIN dbo.Acc_SubHead AS ASH ON ASSH.Account_SubCode = ASH.Account_SubCode";

            DataTable dt = db_conn.Gat_DT(Query);
            ViewBag.LedgerDT = dt.AsEnumerable();



            return View();
        }
        
        public JsonResult SubHead_List(string Acc_HeadCode)
        {
            string Query = @"SELECT  Account_SubCode as Value , Account_Header as Text
                                     FROM  Acc_SubHead where Account_Code = '" + Acc_HeadCode + "' ";            

            string Data = JsonGet.Gat_DT(Query);
            return Json(new { Data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LedgerMaxCoed(string Acc_SubCode)
        {
            string Query = @"SELECT  MAX(ISNULL(Account_Sub_SubCode, 1)) + 1 AS MaxAcc_Sub_SubCode
                                FROM  Acc_Sub_SubHead
                                    GROUP BY Account_SubCode HAVING (Account_SubCode = '" + Acc_SubCode + "') ";

            string Data = JsonGet.Gat_DT(Query);
            return Json(new { Data }, JsonRequestBehavior.AllowGet);
        }


        //Post: AccLedger
        [HttpPost]
        public ActionResult AccLedger(FormCollection collection, string FormID)
        {
            string Query = string.Empty;
            try
            {

                if (FormID == "LedgerForm")
                {
                    Query = @"INSERT INTO Acc_Sub_SubHead  (Account_SubCode, Account_Sub_SubCode, Acc_Name, Address, City, PostCode, Phone, MobileNo, EMail, Comments)
                                          VALUES (" + collection["Acc_SubCode"].ToString() + "," +
                                                      collection["Acc_Sub_SubCode"].ToString() + ",'" +
                                                      collection["Acc_LedgerName"].ToString() + "','" +
                                                      collection["Address"].ToString() + "','" +
                                                      collection["City"].ToString() + "','" +
                                                      collection["Post_Code"].ToString() + "','" +
                                                      collection["Phone"].ToString() + "','" +
                                                      collection["MobileNo"].ToString() + "','" +
                                                      collection["EMail"].ToString() + "','" +
                                                      collection["Comments"].ToString() + "')";
                    // db_conn.POST_DT(Query); 
                    ViewData["msg"] = "Succesfull";
                }
                else if (FormID == "LedgerFormUpdate")
                {
                    Query = @"UPDATE Acc_Sub_SubHead SET 
                                    Acc_Name ='"+collection["Acc_LedgerName"].ToString()+ @"',
                                    Address ='" +collection["Address"].ToString() + @"',
                                    City ='"+collection["City"].ToString() + @"', 
                                    PostCode ='" +collection["Post_Code"].ToString() + @"', 
                                    Phone ='"+collection["Phone"].ToString() + @"', 
                                    MobileNo ='"+collection["MobileNo"].ToString() + @"', 
                                    EMail ='"+collection["EMail"].ToString() + @"', 
                                    Comments ='"+collection["Comments"].ToString() + @"'
                             WHERE(Account_Sub_SubCode =" + collection["Comments"].ToString() + @")";

                    //db_conn.POST_DT(Query);
                    ViewData["msg"] = "Succesfull";
                    
                }
                               
                AccLedger();
            }
            catch
            {
                ViewData["msg"] = "UnSuccesfull";
            }
            return View();
        }

    }
}