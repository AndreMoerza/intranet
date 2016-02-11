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
    public class FormsController : Controller
    {
        private Forms db = new Forms();
        private List<Forms> Forms = new List<Forms>();
        private List<HRForms> HRForms = new List<HRForms>();
        private List<ISForms> ISForms = new List<ISForms>();
        private List<PRForms> PRForms = new List<PRForms>();
        OracleConnection con;
        OracleConnection cmd;
        OracleDataAdapter da;
        public OracleDataReader objDataReader;
        DataSet ds = new DataSet();
       

        public ActionResult Index()
        {
            if (Session["USER"] == null)
            {
                RedirectToAction("Login", "Account");
            }
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            da = new OracleDataAdapter("select FORM_ID,GROUP_,FORM_NAME from KMIINTRANET_MASTER_FORMS ORDER BY GROUP_,FORM_NAME", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                Forms.Add(new Forms() { idform = dr[0].ToString(), group = dr[1].ToString() , formname = dr[2].ToString()});
            }
            return View(Forms);
        }
        public ActionResult IndexHR()
        {
            if (Session["USER"] == null)
            {
                RedirectToAction("Login", "Account");
            }
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            da = new OracleDataAdapter("select FORM_ID,GROUP_,FORM_NAME from KMIINTRANET_MASTER_FORMS where GROUP_='HR' ORDER BY FORM_NAME", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                HRForms.Add(new HRForms() { idform = dr[0].ToString(), group = dr[1].ToString(), formname = dr[2].ToString() });
            }
            return View(HRForms);
        }
        public ActionResult IndexIS()
        {
            if (Session["USER"] == null)
            {
                RedirectToAction("Login", "Account");
            }
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            da = new OracleDataAdapter("select FORM_ID,GROUP_,FORM_NAME from KMIINTRANET_MASTER_FORMS where GROUP_='IS' ORDER BY FORM_NAME", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                ISForms.Add(new ISForms() { idform = dr[0].ToString(), group = dr[1].ToString(), formname = dr[2].ToString() });
            }
            return View(ISForms);
        }
        public ActionResult IndexPR()
        {
            if (Session["USER"] == null)
            {
                RedirectToAction("Login", "Account");
            }
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            da = new OracleDataAdapter("select FORM_ID,GROUP_,FORM_NAME from KMIINTRANET_MASTER_FORMS where GROUP_='PR' ORDER BY FORM_NAME", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                PRForms.Add(new PRForms() { idform = dr[0].ToString(), group = dr[1].ToString(), formname = dr[2].ToString() });
            }
            return View(PRForms);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateForms Form)
        {
            if (ModelState.IsValid)
            {
            //    if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
            //&& Path.GetExtension(postedFile.FileName).ToLower() != ".png"
            //&& Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
            //&& Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            //    {
            //        return false;
            //    }

                    string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                    string FORM_ID;
                    using (OracleConnection connection = new OracleConnection(connectionString))
                    {
                        
                         string number = null;
                         number = "";
                         OracleCommand cmd1 = new OracleCommand();
                         cmd1.CommandType = CommandType.Text;
                         cmd1.CommandText = "select nvl(max(substr(FORM_ID,7,3)),0) from KMIINTRANET_MASTER_FORMS where substr(FORM_ID,1,6)='" + Convert.ToDateTime(System.DateTime.Now).ToString("yyyyMM") + "'";
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
                        FORM_ID = Convert.ToDateTime(System.DateTime.Now).ToString("yyyyMM") + number;
                        byte[] tempFile = new byte[Form.formFile.InputStream.Length];
                        Form.formFile.InputStream.Read(tempFile, 0, tempFile.Length);
                        cmd2.CommandText = "INSERT INTO KMIINTRANET_MASTER_FORMS(FORM_ID,GROUP_,FORM_NAME,FORM_FILE,CREATE_BY,CREATE_DATE) VALUES (:formid,:groupp,:formname,:formfile,'" + Session["USER"] + "',sysdate)";
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection = connection;
                        connection.Open();
                        cmd2.Parameters.Add(":formfile", OracleType.Blob).Value = tempFile;
                        cmd2.Parameters.Add(":formname", OracleType.VarChar).Value = Form.formname.ToString();
                        cmd2.Parameters.Add(":groupp", OracleType.VarChar).Value = Form.group.ToString();
                        cmd2.Parameters.Add(":formid", OracleType.VarChar).Value = FORM_ID;

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
                    cmd.CommandText = "delete from KMIINTRANET_MASTER_FORMS where FORM_ID='" + id + "'";
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
                string sql = "Select FORM_FILE from KMIINTRANET_MASTER_FORMS where FORM_ID='" + id + "'";
                OracleCommand cmd = new OracleCommand(sql, connection);
                cmd.CommandType = CommandType.Text;
                connection.Open();
                object xls = cmd.ExecuteScalar();
                try
                {
                    return File((byte[])xls, "application/vnd.ms-excel");

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

            da = new OracleDataAdapter("select FORM_ID,GROUP_,FORM_NAME from KMIINTRANET_MASTER_FORMS where FORM_ID='" + id + "'", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                db.idform = dr[0].ToString();
                db.group = dr[1].ToString();
                db.formname = dr[2].ToString();

            }
            return View(db);
        }
        [HttpPost]
        public ActionResult Edit(Forms Form)
        {
            if (ModelState.IsValid)
            {

                string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    
                    string number = null;
                    number = "";

                    OracleCommand cmd2 = new OracleCommand();
                    cmd2.Connection = connection;
                    connection.Open();
                    if (Form.formFile != null)
                    {
                        byte[] tempFile = new byte[Form.formFile.InputStream.Length];
                        Form.formFile.InputStream.Read(tempFile, 0, tempFile.Length);
                        cmd2.CommandText = "Update KMIINTRANET_MASTER_FORMS set FORM_NAME=:formname,GROUP_=:groupp,FORM_FILE=:formfile where FORM_ID=:formid";
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Parameters.Add(":formfile", OracleType.Blob).Value = tempFile;
                        cmd2.Parameters.Add(":formname", OracleType.VarChar).Value = Form.formname.ToString();
                        cmd2.Parameters.Add(":groupp", OracleType.VarChar).Value = Form.group.ToString();
                        cmd2.Parameters.Add(":formid", OracleType.VarChar).Value = Form.idform;
                    }
                    else
                    {
                        cmd2.CommandText = "Update KMIINTRANET_MASTER_FORMS set FORM_NAME=:formname,GROUP_=:groupp where FORM_ID=:formid";
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Parameters.Add(":formname", OracleType.VarChar).Value = Form.formname.ToString();
                        cmd2.Parameters.Add(":groupp", OracleType.VarChar).Value = Form.group.ToString();
                        cmd2.Parameters.Add(":formid", OracleType.VarChar).Value = Form.idform.ToString();
                        
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

