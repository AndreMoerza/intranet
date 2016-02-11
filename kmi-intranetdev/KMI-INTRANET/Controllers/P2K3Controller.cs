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
    public class P2K3Controller : Controller
    {
        private P2K3 db = new P2K3();
        private List<P2K3> P2K3 = new List<P2K3>();
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

            da = new OracleDataAdapter("select P2K3_ID,P2K3_TITLE,P2K3_INFO,STATUS from KMIINTRANET_MASTER_P2K3", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                P2K3.Add(new P2K3() { idP2K3 = dr[0].ToString(), title = dr[1].ToString(), isi = dr[2].ToString(), stat = dr[3].ToString(), });
            }
            return View(P2K3);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(P2K3 P2K3)
        {
            if (ModelState.IsValid)
            {

                string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                string P2K3_ID;
                using (OracleConnection connection = new OracleConnection(connectionString))
                {

                    string number = null;
                    number = "";
                    OracleCommand cmd1 = new OracleCommand();
                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = "select nvl(max(substr(P2K3_ID,7,3)),0) from KMIINTRANET_MASTER_P2K3 where substr(P2K3_ID,1,6)='" + Convert.ToDateTime(System.DateTime.Now).ToString("yyyyMM") + "'";
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
                    P2K3_ID = Convert.ToDateTime(System.DateTime.Now).ToString("yyyyMM") + number;
                    byte[] tempFile = new byte[P2K3.P2K3File.InputStream.Length];
                    P2K3.P2K3File.InputStream.Read(tempFile, 0, tempFile.Length);
                    cmd2.CommandText = "INSERT INTO KMIINTRANET_MASTER_P2K3(P2K3_ID,P2K3_TITLE,P2K3_INFO,P2K3_FILE,STATUS,CREATE_BY,CREATE_DATE) VALUES (:p2k3id,:title,:info,:p2k3file,:stat,'" + Session["USER"] + "',sysdate)";
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Connection = connection;
                    connection.Open();
                    cmd2.Parameters.Add(":stat", OracleType.VarChar).Value = P2K3.stat.ToString();
                    cmd2.Parameters.Add(":p2k3file", OracleType.Blob).Value = tempFile;
                    cmd2.Parameters.Add(":info", OracleType.VarChar).Value = P2K3.isi.ToString();
                    cmd2.Parameters.Add(":title", OracleType.VarChar).Value = P2K3.title.ToString();
                    cmd2.Parameters.Add(":p2k3id", OracleType.VarChar).Value = P2K3_ID;

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
                cmd.CommandText = "delete from KMIINTRANET_MASTER_P2K3 where P2K3_ID='" + id + "'";
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
                string sql = "Select P2K3_FILE from KMIINTRANET_MASTER_P2K3 where P2K3_ID='" + id + "'";
                OracleCommand cmd = new OracleCommand(sql, connection);
                cmd.CommandType = CommandType.Text;
                connection.Open();
                object img = cmd.ExecuteScalar();
                try
                {
                    return File((byte[])img, "image/jpg");

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

            da = new OracleDataAdapter("select P2K3_ID,P2K3_TITLE,P2K3_INFO,STATUS from KMIINTRANET_MASTER_P2K3 where P2K3_ID='" + id + "'", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                db.idP2K3 = dr[0].ToString();
                db.title = dr[1].ToString();
                db.isi = dr[2].ToString();
                db.stat = dr[3].ToString();
            }
            return View(db);
        }
        [HttpPost]
        public ActionResult Edit(P2K3 P2K3)
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
                    if (P2K3.P2K3File != null)
                    {
                        byte[] tempFile = new byte[P2K3.P2K3File.InputStream.Length];
                        P2K3.P2K3File.InputStream.Read(tempFile, 0, tempFile.Length);
                        cmd2.CommandText = "Update KMIINTRANET_MASTER_P2K3 set P2K3_TITLE=:title,P2K3_INFO=:info,P2K3_FILE=:p2k3file,STATUS=:stat,MODIFY_BY='" + Session["USER"] + "',MODIFY_DATE=SYSDATE where P2K3_ID=:p2k3id";
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Parameters.Add(":stat", OracleType.VarChar).Value = P2K3.stat.ToString();
                        cmd2.Parameters.Add(":newsfile", OracleType.Blob).Value = tempFile;
                        cmd2.Parameters.Add(":info", OracleType.VarChar).Value = P2K3.isi.ToString();
                        cmd2.Parameters.Add(":title", OracleType.VarChar).Value = P2K3.title.ToString();
                        cmd2.Parameters.Add(":p2k3id", OracleType.VarChar).Value = P2K3.idP2K3;
                    }
                    else
                    {
                        cmd2.CommandText = "Update KMIINTRANET_MASTER_P2K3 set P2K3_TITLE=:title,P2K3_INFO=:info,STATUS=:stat,MODIFY_BY='" + Session["USER"] + "',MODIFY_DATE=SYSDATE where P2K3_ID=:p2k3id";
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Parameters.Add(":stat", OracleType.VarChar).Value = P2K3.stat.ToString();
                        cmd2.Parameters.Add(":info", OracleType.VarChar).Value = P2K3.isi.ToString();
                        cmd2.Parameters.Add(":title", OracleType.VarChar).Value = P2K3.title.ToString();
                        cmd2.Parameters.Add(":p2k3id", OracleType.VarChar).Value = P2K3.idP2K3;

                    }
                    cmd2.ExecuteNonQuery();

                    cmd2 = null;
                    connection.Close();
                    return RedirectToAction("Index");
                }

            }

            return View();
        }
        public ActionResult Details(string ID)
        {
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            da = new OracleDataAdapter("select P2K3_ID,P2K3_TITLE,P2K3_INFO,STATUS from KMIINTRANET_MASTER_P2K3 WHERE P2K3_ID='" + ID + "' ", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                P2K3.Add(new P2K3() { idP2K3 = dr[0].ToString(), title = dr[1].ToString(), isi = dr[2].ToString(), stat = dr[3].ToString() });

            }
            return View(P2K3);
        }
    }
}
