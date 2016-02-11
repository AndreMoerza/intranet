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
    public class NewsController : Controller
    {
        private News db = new News();
        private List<News> News = new List<News>();
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

            da = new OracleDataAdapter("select NEWS_ID,NEWS_TITLE,NEWS from KMIINTRANET_MASTER_NEWS", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                News.Add(new News() { idnews = dr[0].ToString(), title = dr[1].ToString(), isi = dr[2].ToString() });
            }
            return View(News);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateNews News)
        {
            if (ModelState.IsValid)
            {

                string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                string NEWS_ID;
                using (OracleConnection connection = new OracleConnection(connectionString))
                {

                    string number = null;
                    number = "";
                    OracleCommand cmd1 = new OracleCommand();
                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = "select nvl(max(substr(NEWS_ID,7,3)),0) from KMIINTRANET_MASTER_NEWS where substr(NEWS_ID,1,6)='" + Convert.ToDateTime(System.DateTime.Now).ToString("yyyyMM") + "'";
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
                    NEWS_ID = Convert.ToDateTime(System.DateTime.Now).ToString("yyyyMM") + number;
                    byte[] tempFile = new byte[News.NewsFile.InputStream.Length];
                    News.NewsFile.InputStream.Read(tempFile, 0, tempFile.Length);
                    cmd2.CommandText = "INSERT INTO KMIINTRANET_MASTER_NEWS(NEWS_ID,NEWS_TITLE,NEWS,NEWS_FILE,STATUS,CREATE_BY,CREATE_DATE) VALUES (:newsid,:title,:news,:newsfile,:stat,'" + Session["USER"] + "',sysdate)";
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Connection = connection;
                    connection.Open();
                    cmd2.Parameters.Add(":stat", OracleType.VarChar).Value = News.stat.ToString();
                    cmd2.Parameters.Add(":newsfile", OracleType.Blob).Value = tempFile;
                    cmd2.Parameters.Add(":news", OracleType.VarChar).Value = News.isi.ToString();
                    cmd2.Parameters.Add(":title", OracleType.VarChar).Value = News.title.ToString();
                    cmd2.Parameters.Add(":newsid", OracleType.VarChar).Value = NEWS_ID;

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
                cmd.CommandText = "delete from KMIINTRANET_MASTER_NEWS where NEWS_ID='" + id + "'";
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
                string sql = "Select NEWS_FILE from KMIINTRANET_MASTER_NEWS where NEWS_ID='" + id + "'";
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

            da = new OracleDataAdapter("select NEWS_ID,NEWS_TITLE,NEWS,STATUS from KMIINTRANET_MASTER_NEWS where NEWS_ID='" + id + "'", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                db.idnews = dr[0].ToString();
                db.title = dr[1].ToString();
                db.isi = dr[2].ToString();
                db.stat = dr[3].ToString();
            }
            return View(db);
        }
        [HttpPost]
        public ActionResult Edit(News News)
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
                    if (News.NewsFile != null)
                    {
                        byte[] tempFile = new byte[News.NewsFile.InputStream.Length];
                        News.NewsFile.InputStream.Read(tempFile, 0, tempFile.Length);
                        cmd2.CommandText = "Update KMIINTRANET_MASTER_NEWS set NEWS_TITLE=:title,NEWS=:news,NEWS_FILE=:newsfile,STATUS=:stat,MODIFY_BY='" + Session["USER"] + "',MODIFY_DATE=SYSDATE where NEWS_ID=:newsid";
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Parameters.Add(":stat", OracleType.VarChar).Value = News.stat.ToString();
                        cmd2.Parameters.Add(":newsfile", OracleType.Blob).Value = tempFile;
                        cmd2.Parameters.Add(":news", OracleType.VarChar).Value = News.isi.ToString();
                        cmd2.Parameters.Add(":title", OracleType.VarChar).Value = News.title.ToString();
                        cmd2.Parameters.Add(":newsid", OracleType.VarChar).Value = News.idnews;
                    }
                    else
                    {
                        cmd2.CommandText = "Update KMIINTRANET_MASTER_NEWS set NEWS_TITLE=:title,NEWS=:news,STATUS=:stat,MODIFY_BY='" + Session["USER"] + "',MODIFY_DATE=SYSDATE where NEWS_ID=:newsid";
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Parameters.Add(":stat", OracleType.VarChar).Value = News.stat.ToString();
                        cmd2.Parameters.Add(":news", OracleType.VarChar).Value = News.isi.ToString();
                        cmd2.Parameters.Add(":title", OracleType.VarChar).Value = News.title.ToString();
                        cmd2.Parameters.Add(":newsid", OracleType.VarChar).Value = News.idnews;

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

            da = new OracleDataAdapter("select NEWS_ID,NEWS_TITLE,NEWS,STATUS from KMIINTRANET_MASTER_NEWS WHERE NEWS_ID='" + ID + "' ", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                News.Add(new News() { idnews = dr[0].ToString(), title = dr[1].ToString(), isi = dr[2].ToString(), stat = dr[3].ToString() });

            }
            return View(News);
        }
    }
}
