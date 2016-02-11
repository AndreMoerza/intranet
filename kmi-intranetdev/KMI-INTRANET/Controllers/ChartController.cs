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
    public class ChartController : Controller
    {
        private Chart db = new Chart();
        private List<Chart> Chart = new List<Chart>();
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

            da = new OracleDataAdapter("select chart_id,chart_title from KMIINTRANET_MASTER_CHART", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                Chart.Add(new Chart() { idchart = dr[0].ToString(), title = dr[1].ToString() });
            }
            return View(Chart);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateChart Chart)
        {
            if (ModelState.IsValid)
            {

                string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                string CHART_ID ;
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                   
                    string number = null;
                    number = "";
                    OracleCommand cmd1 = new OracleCommand();
                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = "select nvl(max(substr(CHART_ID,7,3)),0) from KMIINTRANET_MASTER_CHART where substr(CHART_ID,1,6)='" + Convert.ToDateTime(System.DateTime.Now).ToString("yyyyMM") + "'";
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
                    CHART_ID = Convert.ToDateTime(System.DateTime.Now).ToString("yyyyMM") + number;
                    byte[] tempFile = new byte[Chart.ChartFile.InputStream.Length];
                    Chart.ChartFile.InputStream.Read(tempFile, 0, tempFile.Length);
                    cmd2.CommandText = "INSERT INTO KMIINTRANET_MASTER_CHART(CHART_ID,CHART_TITLE,CHART_FILE,CREATE_BY,CREATE_DATE) VALUES (:chartid,:title,:chartfile,'" + Session["USER"] + "',sysdate)";
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Connection = connection;
                    connection.Open();
                    cmd2.Parameters.Add(":chartfile", OracleType.Blob).Value = tempFile;
                    cmd2.Parameters.Add(":title", OracleType.VarChar).Value = Chart.title.ToString();
                    cmd2.Parameters.Add(":chartid", OracleType.VarChar).Value = CHART_ID;

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
                cmd.CommandText = "delete from KMIINTRANET_MASTER_CHART where CHART_ID='" + id + "'";
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
                string sql = "Select CHART_FILE from KMIINTRANET_MASTER_CHART where CHART_ID='" + id + "'";
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

            da = new OracleDataAdapter("select CHART_ID,CHART_TITLE from KMIINTRANET_MASTER_CHART where CHART_ID='" + id + "'", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                db.idchart = dr[0].ToString();
                db.title = dr[1].ToString();
                //db.ChartFile = dr[2].ToString();

            }
            return View(db);
        }
        [HttpPost]
        public ActionResult Edit(Chart chart)
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
                    if (chart.ChartFile != null)
                    {
                        byte[] tempFile = new byte[chart.ChartFile.InputStream.Length];
                        chart.ChartFile.InputStream.Read(tempFile, 0, tempFile.Length);
                        cmd2.CommandText = "Update KMIINTRANET_MASTER_CHART set CHART_TITLE=:title,CHART_FILE=:chartfile,MODIFY_BY='" + Session["USER"] + "',MODIFY_DATE=SYSDATE where CHART_ID=:chartid";
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Parameters.Add(":chartfile", OracleType.Blob).Value = tempFile;

                        cmd2.Parameters.Add(":title", OracleType.VarChar).Value = chart.title.ToString();
                        cmd2.Parameters.Add(":chartid", OracleType.VarChar).Value = chart.idchart;
                    }
                    else
                    {
                        cmd2.CommandText = "Update KMIINTRANET_MASTER_CHART set CHART_TITLE=:title,MODIFY_BY='" + Session["USER"] + "',MODIFY_DATE=SYSDATE where CHART_ID=:chartid";
                        cmd2.CommandType = CommandType.Text;

                        cmd2.Parameters.Add(":title", OracleType.VarChar).Value = chart.title.ToString();
                        cmd2.Parameters.Add(":chartid", OracleType.VarChar).Value = chart.idchart.ToString();

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
