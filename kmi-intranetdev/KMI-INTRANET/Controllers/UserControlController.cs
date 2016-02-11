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
    public class UserControlController : Controller
    {
        //
        // GET: /UserControl/
        string sb;
        private Popup db2 = new Popup();
        public List<EmpidPopup> ModalPopup = new List<EmpidPopup>();
        private UserControlling db = new UserControlling();
        private List<UserControlling> UserControlling = new List<UserControlling>();
        OracleConnection con;
        OracleConnection cmd;
        OracleDataAdapter da;
        DataSet ds = new DataSet();
        private bool cek_data(string field, string tbl, string key)
        {
            bool functionReturnValue = false;
            try
            {

                string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = "select " + field + " from " + tbl + " where " + key + "";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    connection.Open();
                    using (OracleDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        if (rdr.HasRows)
                        {
                            functionReturnValue = true;
                        }
                        else
                        {
                            functionReturnValue = false;
                        }
                    }
                    connection.Close();
                }

            }
            catch (Exception)
            {
                //MessageBoxShow(Me, ex.Message)

            }
            return functionReturnValue;
        }
        public ActionResult Index(UserControlling usercontrol)
        {
            if (Session["USER"] == null)
            {
                RedirectToAction("Login", "Account");
            }
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            da = new OracleDataAdapter("select USERNAME,EMP_NAME,LEV,AUTORIZED from TAMPIL_KMIINTRANET_USER", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                UserControlling.Add(new UserControlling() { username = dr[0].ToString(), empnm = dr[1].ToString(), level = dr[2].ToString(), autorized = dr[3].ToString() });
            }
            return View(UserControlling);
        }
        public ActionResult Create()
        {
            NewUser model = new NewUser();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(NewUser CreateUser)
        {
            if (ModelState.IsValid)
            {
                if (cek_data("*", "KMIINTRANET_USER", "username='" + CreateUser.User + "'") == false)
                {

                    if (CreateUser.Autorized != null)
                    {
                        for (int i = 0; i < CreateUser.Autorized.Count(); i++)
                        {
                            if (i == 0)
                            {
                                sb += CreateUser.Autorized.GetValue(i).ToString();
                            }
                            else
                            {
                                sb += "," + CreateUser.Autorized.GetValue(i).ToString();
                            }
                        }
                    }
                    else
                    {
                        sb = "";
                    }
                    string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                    
                    using (OracleConnection connection = new OracleConnection(connectionString))
                    {
                        OracleCommand cmd1 = new OracleCommand();
                        cmd1.CommandText = "INSERT INTO KMIINTRANET_USER (USERNAME,PASS,LEV,AUTORIZED) VALUES ('" + CreateUser.User + "','" + CreateUser.Pass + "','" + CreateUser.Level + "','" + sb + "')";
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = connection;
                        connection.Open();
                        cmd1.ExecuteNonQuery();
                        
                        connection.Close();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Username Already Exist!');location.href = 'Index'; ;</script>", "WARNING");

                }

            }

            return View(CreateUser);
        }
        public ActionResult Delete(string user)
        {
            if (cek_data("*", "KMIINTRANET_USER", "username='" + user.Trim() + "'") == true)
            {

                string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    OracleCommand cmd = new OracleCommand();

                    cmd.CommandText = "delete from KMIINTRANET_USER where username='" + user.Trim() + "'";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('You Cannot Delete, Your Request Has Been Processed!');location.href = 'Index'; ;</script>", "WARNING");

            }

            return RedirectToAction("Index");
        }
        public ActionResult Details(string user)
        {
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            da = new OracleDataAdapter("select USERNAME,PASS,LEV,AUTORIZED from KMIINTRANET_USER WHERE USERNAME='" + user.Trim() + "'", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                UserControlling.Add(new UserControlling() { username = dr[0].ToString(), pass = dr[1].ToString(), level = dr[2].ToString(), autorized = dr[3].ToString() });

            }
            return View(UserControlling);
        }
        public ActionResult Popup()
        {
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            da = new OracleDataAdapter("select EMP_ID,EMP_NAME from CTM_EMPLOYEE_MASTER_TAB WHERE STATUS='ACTIVE' AND EMP_TITLE_ID IN ('J002','J003','J004','J005','J007','J008','J009','J010','J011') AND EMP_TYPE IN ('P01')", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                ModalPopup.Add(new EmpidPopup() { Employeeid = dr[0].ToString(), Employeenm = dr[1].ToString() });
            }
            return View(ModalPopup);
        }
    }
}
