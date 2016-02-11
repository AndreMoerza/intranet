using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using System.Data.OracleClient;
using KMI_INTRANET.Models;
using System.Net.Mail;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.ApplicationServices;

namespace KMI_INTRANET.Controllers
{
    public class MemoController : Controller
    {
        private Memo db = new Memo();
        private List<Memo> Memo = new List<Memo>();
        OracleConnection con;
        OracleConnection cmd;
        OracleDataAdapter da;
        public OracleDataReader objDataReader;
        DataSet ds = new DataSet();
        string format = "MM/dd/yyyy";
       

        public ActionResult Index()
        {
            if (Session["USER"] == null)
            {
                RedirectToAction("Login", "Account");
            }
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            da = new OracleDataAdapter("select MEMO_ID,MEMO_THEME,AUTORIZE,VALID_FROM,VALID_UNTIL from KMIINTRANET_MASTER_MEMO", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                Memo.Add(new Memo() { idmemo = dr[0].ToString(), Theme = dr[1].ToString(), Autorize = dr[2].ToString(), ValidFrom = DateTime.Parse(dr[3].ToString()).ToString(format), ValidUntil = DateTime.Parse(dr[4].ToString()).ToString(format) });
            }
            return View(Memo);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateMemo Memo)
        {
            if (ModelState.IsValid)
            {
                
                    string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                    string MEMO_ID,AUTORIZE_DETAIL;
                    using (OracleConnection connection = new OracleConnection(connectionString))
                    {
                        if (Memo.Autorize.ToString() == "Department")
                        {
                            AUTORIZE_DETAIL = Memo.Autorize_detail;
                        }
                        else
                        {
                            AUTORIZE_DETAIL = "";
                        }
                         string number = null;
                         number = "";
                         OracleCommand cmd1 = new OracleCommand();
                         cmd1.CommandType = CommandType.Text;
                         cmd1.CommandText = "select nvl(max(substr(MEMO_ID,7,3)),0) from KMIINTRANET_MASTER_MEMO where substr(MEMO_ID,1,6)='" + Convert.ToDateTime(System.DateTime.Now).ToString("yyyyMM") + "'";
                         cmd1.Connection = connection;
                         connection.Open();
                         objDataReader = cmd1.ExecuteReader();
                         if (objDataReader.HasRows)
                         {
                             objDataReader.Read();
                             number = Convert.ToString(Convert.ToInt32(objDataReader[0]) + 1);
                             if (number.Length == 1)
                             {
                                 number = "00" + number;
                             }
                             else if (number.Length == 2)
                             {
                                 number = "0" + number;
                             }
                             else if (number.Length == 3)
                             {
                                 number = number;
                             }
                         }
                         cmd1 = null;
                         connection.Close();
                         OracleCommand cmd2 = new OracleCommand();
                        MEMO_ID = Convert.ToDateTime(System.DateTime.Now).ToString("yyyyMM") + number;
                        byte[] tempFile = new byte[Memo.MemoFile.InputStream.Length];
                        Memo.MemoFile.InputStream.Read(tempFile, 0, tempFile.Length);
                        cmd2.CommandText = "INSERT INTO KMIINTRANET_MASTER_MEMO(MEMO_ID,MEMO_THEME,AUTORIZE,AUTORIZE_DETAIL,MEMO_FILE,VALID_FROM,VALID_UNTIL,CREATE_BY,CREATE_DATE) VALUES (:memoid,:theme,:autorize,:detail,:memofile,:validfrom,:validuntil,'" + Session["USER"] + "',sysdate)";
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection = connection;
                        connection.Open();
                        cmd2.Parameters.Add(":memofile", OracleType.Blob).Value = tempFile;
                        cmd2.Parameters.Add(":validuntil", OracleType.DateTime).Value = Memo.ValidUntil.ToString("MM/dd/yyyy");
                        cmd2.Parameters.Add(":validfrom", OracleType.DateTime).Value = Memo.ValidFrom.ToString("MM/dd/yyyy");
                        cmd2.Parameters.Add(":detail", OracleType.VarChar).Value = AUTORIZE_DETAIL;
                        cmd2.Parameters.Add(":autorize", OracleType.VarChar).Value = Memo.Autorize.ToString();
                        cmd2.Parameters.Add(":theme", OracleType.VarChar).Value = Memo.Theme.ToString();
                        cmd2.Parameters.Add(":memoid", OracleType.VarChar).Value = MEMO_ID;

                        cmd2.ExecuteNonQuery();

                        cmd2 = null;
                        connection.Close();
                        return RedirectToAction("Index");
                    }

            }

            return View();
        }
        public ActionResult Delete(string id)
        {
                string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = "delete from KMIINTRANET_MASTER_MEMO where MEMO_ID='" + id + "'";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            return RedirectToAction("Index");
        }
        public ActionResult Show(string id)
        {
            string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                string sql = "Select MEMO_FILE from KMIINTRANET_MASTER_MEMO where MEMO_ID='" + id + "'";
                OracleCommand cmd = new OracleCommand(sql, connection);
                cmd.CommandType = CommandType.Text;
                connection.Open();
                object pdf = cmd.ExecuteScalar();
                try
                {
                    return File((byte[])pdf, "application/pdf");

                }
                catch
                {
                    return null;
                }
                finally
                {
                    connection.Close();
                }


            }
        }
        public ActionResult Edit(string id)
        {
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            da = new OracleDataAdapter("select MEMO_ID,MEMO_THEME,AUTORIZE,VALID_FROM,VALID_UNTIL from KMIINTRANET_MASTER_MEMO where MEMO_ID='" + id + "'", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                db.idmemo = dr[0].ToString();
                db.Theme = dr[1].ToString();
                db.Autorize = dr[2].ToString();
                db.ValidFrom = DateTime.Parse(dr[3].ToString()).ToString(format);
                db.ValidUntil = DateTime.Parse(dr[4].ToString()).ToString(format);

            }
            return View(db);
        }
        [HttpPost]
        public ActionResult Edit(Memo Memo)
        {
            if (ModelState.IsValid)
            {

                string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                string AUTORIZE_DETAIL;
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    if (Memo.Autorize.ToString() == "Department")
                    {
                        AUTORIZE_DETAIL = Memo.Autorize_detail;
                    }
                    else
                    {
                        AUTORIZE_DETAIL = "";
                    }
                    string number = null;
                    number = "";

                    OracleCommand cmd2 = new OracleCommand();
                    cmd2.Connection = connection;
                    connection.Open();
                    if (Memo.MemoFile != null)
                    {
                        byte[] tempFile = new byte[Memo.MemoFile.InputStream.Length];
                        Memo.MemoFile.InputStream.Read(tempFile, 0, tempFile.Length);
                        cmd2.CommandText = "Update KMIINTRANET_MASTER_MEMO set MEMO_THEME=:theme,AUTORIZE=:autorize,AUTORIZE_DETAIL=:detail,MEMO_FILE=:memofile,VALID_FROM=:validfrom,VALID_UNTIL=:validuntil,MODIFY_BY='" + Session["USER"] + "',MODIFY_DATE=SYSDATE where MEMO_ID=:memoid";
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Parameters.Add(":memofile", OracleType.Blob).Value = tempFile;
                        cmd2.Parameters.Add(":validuntil", OracleType.DateTime).Value = Memo.ValidUntil;
                        cmd2.Parameters.Add(":validfrom", OracleType.DateTime).Value = Memo.ValidFrom;
                        cmd2.Parameters.Add(":detail", OracleType.VarChar).Value = AUTORIZE_DETAIL;
                        cmd2.Parameters.Add(":autorize", OracleType.VarChar).Value = Memo.Autorize.ToString();
                        cmd2.Parameters.Add(":theme", OracleType.VarChar).Value = Memo.Theme.ToString();
                        cmd2.Parameters.Add(":memoid", OracleType.VarChar).Value = Memo.idmemo;
                    }
                    else
                    {
                        cmd2.CommandText = "Update KMIINTRANET_MASTER_MEMO set MEMO_THEME=:theme,AUTORIZE=:autorize,AUTORIZE_DETAIL=:detail,VALID_FROM=:validfrom,VALID_UNTIL=:validuntil,MODIFY_BY='" + Session["USER"] + "',MODIFY_DATE=SYSDATE where MEMO_ID=:memoid";
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Parameters.Add(":validuntil", OracleType.DateTime).Value = Memo.ValidUntil;
                        cmd2.Parameters.Add(":validfrom", OracleType.DateTime).Value = Memo.ValidFrom;
                        cmd2.Parameters.Add(":detail", OracleType.VarChar).Value = AUTORIZE_DETAIL;
                        cmd2.Parameters.Add(":autorize", OracleType.VarChar).Value = Memo.Autorize.ToString();
                        cmd2.Parameters.Add(":theme", OracleType.VarChar).Value = Memo.Theme.ToString();
                        cmd2.Parameters.Add(":memoid", OracleType.VarChar).Value = Memo.idmemo.ToString();
                        
                    }
                    cmd2.ExecuteNonQuery();

                    cmd2 = null;
                    connection.Close();
                    return RedirectToAction("Index");
                }

            }

            return View();
        }
    }
}
