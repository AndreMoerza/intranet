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
using System.Globalization;
namespace KMI_INTRANET.Controllers
{
    [Authorize]
    public class LeaveHistoryController : Controller
    {
        //private RestLeave db1 = new RestLeave();
        private List<RestLeave> RestLeave = new List<RestLeave>();
        private List<HAID> HaidLeave = new List<HAID>();
        private LeaveModel db = new LeaveModel();
        private List<LeaveModel> LeaveModel = new List<LeaveModel>();
        private Popup db2 = new Popup();
        public List<Popup> ModalPopup = new List<Popup>();
        int ongoing;
        OracleConnection con;
        OracleConnection cmd;
        OracleDataAdapter da;
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        String Approve, Reject, leave_name;
        DateTime eventdate, birthdate;
        string onelast, twolast;
       public void MessageBoxShow(string strMessage)
        {
            var page = new Page();
            Literal ltr1 = new Literal();
            ltr1.Text = "<script type='text/javascript'> alert('" + strMessage + "');  </script>";

            page.Controls.Add(ltr1);
        }
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
       
        public ActionResult Index(LeaveModel model)

        {
            if (Session["USER"] == null)
            {
                RedirectToAction("Login", "Account");
            }   
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");
            if (Session["UserSecurity"].ToString() == "ADMIN")
            {
                da = new OracleDataAdapter("select EMPID,EMP_NAME,LEAVE_NAME,FROMLEAVE,TOLEAVE,DAYS,STATUS from TAMPIL_DETAIL_LEAVE WHERE AREA_CODE IN (" + Session["Autorized"] + ") ", con);
            }
            else
            {
                da = new OracleDataAdapter("select EMPID,EMP_NAME,LEAVE_NAME,FROMLEAVE,TOLEAVE,DAYS,STATUS from TAMPIL_DETAIL_LEAVE WHERE EMPID='" + Session["USER"] + "'", con);
            }
            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                LeaveModel.Add(new LeaveModel() { nik = dr[0].ToString(), empnm = dr[1].ToString(), namacuti = dr[2].ToString(), fromleave = DateTime.Parse(dr[3].ToString()), toleave = DateTime.Parse(dr[4].ToString()), totaldays = Convert.ToDouble(dr[5].ToString()), deskripsi = dr[6].ToString() });
            }
            return View(LeaveModel);
        }

        public ActionResult Details(string nik, DateTime fromdate, DateTime todate)
        {
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            da = new OracleDataAdapter("select KDL.EMPID,PM.EMP_NAME,PM.CC_DESC,to_char(KDL.FROMLEAVE,'dd/mm/yyyy'),to_char(KDL.TOLEAVE,'dd/mm/yyyy'),KDL.STATUS,KDL.REASON,KDL.ADDRESS,KDL.LEAVE_TYPE,KDL.DAYS,KDL.STATUS from KMIHRMS_DETAIL_LEAVE KDL inner join PERSONAL_MASTER PM on KDL.EMPID=PM.EMP_ID " +
                "WHERE KDL.EMPID='" + nik + "' and to_char(KDL.FROMLEAVE,'yyyy/mm/dd')='" + fromdate.ToString("yyyy/MM/dd") + "' and to_char(KDL.TOLEAVE,'yyyy/mm/dd')='" + todate.ToString("yyyy/MM/dd") + "' AND PM.AREA_CODE IN (" + Session["Autorized"]  + ")", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //TimeSpan date = (DateTime.ParseExact(dr[4].ToString(), "dd/MM/yyyy", null) - DateTime.ParseExact(dr[3].ToString(), "dd/MM/yyyy", null));
                //num = Math.Round(date.TotalDays + 1);
                LeaveModel.Add(new LeaveModel() { nik = dr[0].ToString(), empnm = dr[1].ToString(), dept = dr[2].ToString(), Detfrom = String.Format("{0:d MMM yyyy}", dr[3]), Detto = String.Format("{0:d MMM yyyy}", dr[4]), deskripsi = dr[5].ToString(), reason = dr[6].ToString(), alamat = dr[7].ToString(), type = dr[8].ToString(), totaldays = Convert.ToDouble(dr[9].ToString()) , status = dr[10].ToString()});

            }
            return View(LeaveModel);
        }
        public ActionResult Create()
        {
            LeaveRequest model = new LeaveRequest();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(LeaveRequest Createleave)
        {
            string reason;
            if (ModelState.IsValid)
            {
                if (cek_data("*", "KMIHRMS_DETAIL_LEAVE", "EMPID='" + Createleave.Empid + "' and leave_type='" + Createleave.SelectedItem + "' and to_char(FROMLEAVE,'dd/mm/yyyy')='" + Createleave.fromleave + "'and to_char(TOLEAVE,'dd/mm/yyyy')='" + Createleave.toleave + "'") == false)
                {
                    string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                    string empid, empname, bagian, namabagian, namaapproved, emailapproved;
                    using (OracleConnection connection = new OracleConnection(connectionString))
                    {
                        if (Createleave.SelectedItem == "CT")
                        {
                                OracleCommand cmd1 = new OracleCommand();
                                cmd1.CommandText = "INSERT INTO KMIHRMS_DETAIL_LEAVE (EMPID,LEAVE_TYPE,FROMLEAVE,TOLEAVE,REASON,ADDRESS,STATUS,DAYS) VALUES ('" + Createleave.Empid + "','" + Createleave.SelectedItem + "',to_date('" + Createleave.fromleave + "','DD/MM/YYYY'),to_date('" + Createleave.toleave + "','DD/MM/YYYY'),'" + Createleave.reasonpost1 + "','" + Createleave.Alamat + "','Release','" + Createleave.hari + "')";
                                cmd1.CommandType = CommandType.Text;
                                cmd1.Connection = connection;
                                connection.Open();
                                cmd1.ExecuteNonQuery();
                                OracleCommand cmd2 = new OracleCommand();
                                cmd2.CommandText = "select t.EMP_ID,t.EMP_NAME,T.COST_CENTER,t.CC_DESC from person.personal_master t where t.emp_id='" + Createleave.Empid + "'";
                                cmd2.CommandType = CommandType.Text;
                                cmd2.Connection = connection;
                                using (OracleDataReader rdr1 = cmd2.ExecuteReader())
                                {
                                    rdr1.Read();
                                    empid = rdr1.GetString(0);
                                    empname = rdr1.GetString(1);
                                    bagian = rdr1.GetString(2);
                                    namabagian = rdr1.GetString(3);
                                }
                                OracleCommand cmd3 = new OracleCommand();
                                cmd3.CommandText = "select KLAT.app1_id app_id,CEMT.EMP_NAME,cemt.email,KLAT.app1_as app_as from person.KMIHRMS_LEAVE_APP_TAB KLAT " +
                                    "INNER JOIN PERSON.CTM_EMPLOYEE_MASTER_TAB CEMT ON CEMT.EMP_ID=KLAT.APP1_ID " +
                                    "where requsioner_id='" + bagian + "' and app_type='HR'";
                                cmd3.CommandType = CommandType.Text;
                                cmd3.Connection = connection;
                                using (OracleDataReader rdr2 = cmd3.ExecuteReader())
                                {
                                    rdr2.Read();
                                    namaapproved = rdr2.GetString(1);
                                    emailapproved = rdr2.GetString(2);
                                }
                                sendmail(namaapproved, emailapproved, empid, empname, bagian, namabagian, DateTime.ParseExact(Createleave.fromleave, "dd/MM/yyyy", null), DateTime.ParseExact(Createleave.toleave, "dd/MM/yyyy", null), Createleave.reasonpost1, Createleave.SelectedItem, Createleave.Alamat, Createleave.hpl, Createleave.tglkej, Createleave.times);
                                connection.Close();
                            /*}*/
                           
                        }
                        else if (Createleave.SelectedItem == "CH")
                        {
                            TimeSpan date = (DateTime.ParseExact(Createleave.toleave, "dd/MM/yyyy", null) - DateTime.ParseExact(Createleave.fromleave, "dd/MM/yyyy", null));
                            if (Math.Round(date.TotalDays + 1) > 2)
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Your Request More Then 2 Days !');location.href = 'Create'; ;</script>", "WARNING");
                            }
                            else
                            {
                                OracleCommand cmd1 = new OracleCommand();
                                cmd1.CommandText = "INSERT INTO KMIHRMS_DETAIL_LEAVE (EMPID,LEAVE_TYPE,FROMLEAVE,TOLEAVE,ADDRESS,STATUS,DAYS) VALUES ('" + Createleave.Empid + "','" + Createleave.SelectedItem + "',to_date('" + Createleave.fromleave + "','DD/MM/YYYY'),to_date('" + Createleave.toleave + "','DD/MM/YYYY'),'" + Createleave.Alamat + "','Release','" + Createleave.hari + "')";
                                cmd1.CommandType = CommandType.Text;
                                cmd1.Connection = connection;
                                connection.Open();
                                cmd1.ExecuteNonQuery();
                                OracleCommand cmd2 = new OracleCommand();
                                cmd2.CommandText = "select t.EMP_ID,t.EMP_NAME,T.COST_CENTER,t.CC_DESC from person.personal_master t where t.emp_id='" + Createleave.Empid + "'";
                                cmd2.CommandType = CommandType.Text;
                                cmd2.Connection = connection;
                                using (OracleDataReader rdr1 = cmd2.ExecuteReader())
                                {
                                    rdr1.Read();
                                    empid = rdr1.GetString(0);
                                    empname = rdr1.GetString(1);
                                    bagian = rdr1.GetString(2);
                                    namabagian = rdr1.GetString(3);
                                }
                                OracleCommand cmd3 = new OracleCommand();
                                cmd3.CommandText = "select KLAT.app1_id app_id,CEMT.EMP_NAME,cemt.email,KLAT.app1_as app_as from person.KMIHRMS_LEAVE_APP_TAB KLAT " +
                                    "INNER JOIN PERSON.CTM_EMPLOYEE_MASTER_TAB CEMT ON CEMT.EMP_ID=KLAT.APP1_ID " +
                                    "where requsioner_id='" + bagian + "' and app_type='HR'";
                                cmd3.CommandType = CommandType.Text;
                                cmd3.Connection = connection;
                                using (OracleDataReader rdr2 = cmd3.ExecuteReader())
                                {
                                    rdr2.Read();
                                    namaapproved = rdr2.GetString(1);
                                    emailapproved = rdr2.GetString(2);
                                }
                                sendmail(namaapproved, emailapproved, empid, empname, bagian, namabagian, DateTime.ParseExact(Createleave.fromleave, "dd/MM/yyyy", null), DateTime.ParseExact(Createleave.toleave, "dd/MM/yyyy", null), Createleave.reasonpost, Createleave.SelectedItem, Createleave.Alamat, Createleave.hpl, Createleave.tglkej, Createleave.times);
                                connection.Close();
                            }
                        }
                        else if (Createleave.SelectedItem == "DI")
                        {
                            
                            if (Createleave.reasonpost== "Perkawinan")
                            {
                                reason = Createleave.reasonpost + " " + Createleave.detail;
                            }
                            else if (Createleave.reasonpost == "Meninggal Dunia")
                            {
                                reason = Createleave.reasonpost + " " + Createleave.detail1;
                            }
                            else
                            {
                                reason = Createleave.reasonpost;
                            }
                            
                            OracleCommand cmd1 = new OracleCommand();
                            cmd1.CommandText = "INSERT INTO KMIHRMS_DETAIL_LEAVE (EMPID,LEAVE_TYPE,FROMLEAVE,TOLEAVE,ADDRESS,STATUS,REASON,DAYS,REALDATE,TIMESREAL) VALUES ('" + Createleave.Empid + "','" + Createleave.SelectedItem + "',to_date('" + Createleave.fromleave + "','DD/MM/YYYY'),to_date('" + Createleave.toleave + "','DD/MM/YYYY'),'" + Createleave.Alamat + "','Release','" + reason + "','" + Createleave.hari + "',to_date('" + Createleave.tglkej + "','DD/MM/YYYY'),to_date('" + Createleave.times + "','HH24:MI'))";
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Connection = connection;
                            connection.Open();
                            cmd1.ExecuteNonQuery();
                            OracleCommand cmd2 = new OracleCommand();
                            cmd2.CommandText = "select t.EMP_ID,t.EMP_NAME,T.COST_CENTER,t.CC_DESC from person.personal_master t where t.emp_id='" + Createleave.Empid + "'";
                            cmd2.CommandType = CommandType.Text;
                            cmd2.Connection = connection;
                            using (OracleDataReader rdr1 = cmd2.ExecuteReader())
                            {
                                rdr1.Read();
                                empid = rdr1.GetString(0);
                                empname = rdr1.GetString(1);
                                bagian = rdr1.GetString(2);
                                namabagian = rdr1.GetString(3);
                            }
                            OracleCommand cmd3 = new OracleCommand();
                            cmd3.CommandText = "select KLAT.app1_id app_id,CEMT.EMP_NAME,cemt.email,KLAT.app1_as app_as from person.KMIHRMS_LEAVE_APP_TAB KLAT " +
                                "INNER JOIN PERSON.CTM_EMPLOYEE_MASTER_TAB CEMT ON CEMT.EMP_ID=KLAT.APP1_ID " +
                                "where requsioner_id='" + bagian + "' and app_type='HR'";
                            cmd3.CommandType = CommandType.Text;
                            cmd3.Connection = connection;
                            using (OracleDataReader rdr2 = cmd3.ExecuteReader())
                            {
                                rdr2.Read();
                                namaapproved = rdr2.GetString(1);
                                emailapproved = rdr2.GetString(2);
                            }
                            sendmail(namaapproved, emailapproved, empid, empname, bagian, namabagian, DateTime.ParseExact(Createleave.fromleave, "dd/MM/yyyy", null), DateTime.ParseExact(Createleave.toleave, "dd/MM/yyyy", null), reason, Createleave.SelectedItem, Createleave.Alamat, Createleave.hpl, Createleave.tglkej, Createleave.times);
                            connection.Close();
                        }
                        else if (Createleave.SelectedItem == "I")
                        {
                            OracleCommand cmd1 = new OracleCommand();
                            cmd1.CommandText = "INSERT INTO KMIHRMS_DETAIL_LEAVE (EMPID,LEAVE_TYPE,FROMLEAVE,TOLEAVE,REASON,ADDRESS,STATUS,DAYS) VALUES ('" + Createleave.Empid + "','" + Createleave.SelectedItem + "',to_date('" + Createleave.fromleave + "','DD/MM/YYYY'),to_date('" + Createleave.toleave + "','DD/MM/YYYY'),'" + Createleave.reasonpost1 + "','" + Createleave.Alamat + "','Release','" + Createleave.hari + "')";
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Connection = connection;
                            connection.Open();
                            cmd1.ExecuteNonQuery();
                            OracleCommand cmd2 = new OracleCommand();
                            cmd2.CommandText = "select t.EMP_ID,t.EMP_NAME,T.COST_CENTER,t.CC_DESC from person.personal_master t where t.emp_id='" + Createleave.Empid + "'";
                            cmd2.CommandType = CommandType.Text;
                            cmd2.Connection = connection;
                            using (OracleDataReader rdr1 = cmd2.ExecuteReader())
                            {
                                rdr1.Read();
                                empid = rdr1.GetString(0);
                                empname = rdr1.GetString(1);
                                bagian = rdr1.GetString(2);
                                namabagian = rdr1.GetString(3);
                            }
                            OracleCommand cmd3 = new OracleCommand();
                            cmd3.CommandText = "select KLAT.app1_id app_id,CEMT.EMP_NAME,cemt.email,KLAT.app1_as app_as from person.KMIHRMS_LEAVE_APP_TAB KLAT " +
                                "INNER JOIN PERSON.CTM_EMPLOYEE_MASTER_TAB CEMT ON CEMT.EMP_ID=KLAT.APP1_ID " +
                                "where requsioner_id='" + bagian + "' and app_type='HR'";
                            cmd3.CommandType = CommandType.Text;
                            cmd3.Connection = connection;
                            using (OracleDataReader rdr2 = cmd3.ExecuteReader())
                            {
                                rdr2.Read();
                                namaapproved = rdr2.GetString(1);
                                emailapproved = rdr2.GetString(2);
                            }
                            sendmail(namaapproved, emailapproved, empid, empname, bagian, namabagian, DateTime.ParseExact(Createleave.fromleave, "dd/MM/yyyy", null), DateTime.ParseExact(Createleave.toleave, "dd/MM/yyyy", null), Createleave.reasonpost1, Createleave.SelectedItem, Createleave.Alamat, Createleave.hpl, Createleave.tglkej, Createleave.times);
                            connection.Close();
                        }
                        else if (Createleave.SelectedItem == "CI")
                        {
                            OracleCommand cmd1 = new OracleCommand();
                            cmd1.CommandText = "INSERT INTO KMIHRMS_DETAIL_LEAVE (EMPID,LEAVE_TYPE,FROMLEAVE,TOLEAVE,REASON,ADDRESS,STATUS,DAYS) VALUES ('" + Createleave.Empid + "','" + Createleave.SelectedItem + "',to_date('" + Createleave.fromleave + "','DD/MM/YYYY'),to_date('" + Createleave.toleave + "','DD/MM/YYYY'),'" + Createleave.reasonpost1 + "','" + Createleave.Alamat + "','Release','" + Createleave.hari + "')";
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Connection = connection;
                            connection.Open();
                            cmd1.ExecuteNonQuery();
                            OracleCommand cmd2 = new OracleCommand();
                            cmd2.CommandText = "select t.EMP_ID,t.EMP_NAME,T.COST_CENTER,t.CC_DESC from person.personal_master t where t.emp_id='" + Createleave.Empid + "'";
                            cmd2.CommandType = CommandType.Text;
                            cmd2.Connection = connection;
                            using (OracleDataReader rdr1 = cmd2.ExecuteReader())
                            {
                                rdr1.Read();
                                empid = rdr1.GetString(0);
                                empname = rdr1.GetString(1);
                                bagian = rdr1.GetString(2);
                                namabagian = rdr1.GetString(3);
                            }
                            OracleCommand cmd3 = new OracleCommand();
                            cmd3.CommandText = "select KLAT.app1_id app_id,CEMT.EMP_NAME,cemt.email,KLAT.app1_as app_as from person.KMIHRMS_LEAVE_APP_TAB KLAT " +
                                "INNER JOIN PERSON.CTM_EMPLOYEE_MASTER_TAB CEMT ON CEMT.EMP_ID=KLAT.APP1_ID " +
                                "where requsioner_id='" + bagian + "' and app_type='HR'";
                            cmd3.CommandType = CommandType.Text;
                            cmd3.Connection = connection;
                            using (OracleDataReader rdr2 = cmd3.ExecuteReader())
                            {
                                rdr2.Read();
                                namaapproved = rdr2.GetString(1);
                                emailapproved = rdr2.GetString(2);
                            }
                            sendmail(namaapproved, emailapproved, empid, empname, bagian, namabagian, DateTime.ParseExact(Createleave.fromleave, "dd/MM/yyyy", null), DateTime.ParseExact(Createleave.toleave, "dd/MM/yyyy", null), Createleave.reasonpost1, Createleave.SelectedItem, Createleave.Alamat, Createleave.hpl, Createleave.tglkej, Createleave.times);
                            connection.Close();
                        }
                        else if (Createleave.SelectedItem == "CM")
                        {
                            OracleCommand cmd1 = new OracleCommand();
                            cmd1.CommandText = "INSERT INTO KMIHRMS_DETAIL_LEAVE (EMPID,LEAVE_TYPE,FROMLEAVE,TOLEAVE,REASON,ADDRESS,STATUS,DAYS,REALDATE) VALUES ('" + Createleave.Empid + "','" + Createleave.SelectedItem + "',to_date('" + Createleave.fromleave + "','DD/MM/YYYY'),to_date('" + Createleave.toleave + "','DD/MM/YYYY'),'Melahirkan','" + Createleave.Alamat + "','Release','" + Createleave.hari + "',to_date('" + Createleave.hpl + "','DD/MM/YYYY'))";
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Connection = connection;
                            connection.Open();
                            cmd1.ExecuteNonQuery();
                            OracleCommand cmd2 = new OracleCommand();
                            cmd2.CommandText = "select t.EMP_ID,t.EMP_NAME,T.COST_CENTER,t.CC_DESC from person.personal_master t where t.emp_id='" + Createleave.Empid + "'";
                            cmd2.CommandType = CommandType.Text;
                            cmd2.Connection = connection;
                            using (OracleDataReader rdr1 = cmd2.ExecuteReader())
                            {
                                rdr1.Read();
                                empid = rdr1.GetString(0);
                                empname = rdr1.GetString(1);
                                bagian = rdr1.GetString(2);
                                namabagian = rdr1.GetString(3);
                            }
                            OracleCommand cmd3 = new OracleCommand();
                            cmd3.CommandText = "select KLAT.app1_id app_id,CEMT.EMP_NAME,cemt.email,KLAT.app1_as app_as from person.KMIHRMS_LEAVE_APP_TAB KLAT " +
                                "INNER JOIN PERSON.CTM_EMPLOYEE_MASTER_TAB CEMT ON CEMT.EMP_ID=KLAT.APP1_ID " +
                                "where requsioner_id='" + bagian + "' and app_type='HR'";
                            cmd3.CommandType = CommandType.Text;
                            cmd3.Connection = connection;
                            using (OracleDataReader rdr2 = cmd3.ExecuteReader())
                            {
                                rdr2.Read();
                                namaapproved = rdr2.GetString(1);
                                emailapproved = rdr2.GetString(2);
                            }
                            sendmail(namaapproved, emailapproved, empid, empname, bagian, namabagian, DateTime.ParseExact(Createleave.fromleave, "dd/MM/yyyy", null), DateTime.ParseExact(Createleave.toleave, "dd/MM/yyyy", null), "Melahirkan", Createleave.SelectedItem, Createleave.Alamat, Createleave.hpl, Createleave.tglkej, Createleave.times);
                            connection.Close();
                        }
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Your Request Already Exist!');location.href = 'Index'; ;</script>", "WARNING");

                }
            }
            return View(Createleave);
        }

        public ActionResult CheckRestLeave(string Id)
        {
            List<RestLeave> RestList = new List<RestLeave>();
            string tgl_skrg;
            
            tgl_skrg = DateTime.Now.ToString("MM/dd/yyyy");
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");
            OracleDataAdapter da = new OracleDataAdapter("select nvl(sum(days),'0') from person.KMIHRMS_DETAIL_LEAVE t where status in ('Release','App. Section Head','App. Department Head') " +
                                                          "and EMPID='" + Id + "' and leave_type in ('CT')", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                ongoing = Convert.ToInt32(dr[0].ToString());

            }
            OracleDataAdapter da1 = new OracleDataAdapter("Select KML.YEARLEAVE AS YEAR,CLTT.LEAVERIGHT AS HAK_CUTI,CLTT.MASSLEAVE AS CUTI_MASAL," +
                                                          "nvl(sum(kdl.days),0) as HASGOTTEN, KML.REST_LEAVE " +
                                                          "from person.KMIHRMS_MASTER_LEAVE KML INNER JOIN ctm_leave_tahunan_tab CLTT " +
                                                          "ON to_char(CLTT.YEAR_,'yyyy')=KML.YEARLEAVE left join PERSON.KMIHRMS_DETAIL_LEAVE KDL " +
                                                          "ON KDL.EMPID=KML.EMPID AND STATUS='Completed' " +
                                                          "WHERE KML.EMPID='" + Id + "' and KML.valid_from <= to_date('" + tgl_skrg + "','MM/DD/YYYY') and KML.valid_to >= to_date('" + tgl_skrg + "','MM/DD/YYYY') " +
                                                          "group by KML.YEARLEAVE,CLTT.LEAVERIGHT,CLTT.MASSLEAVE,kdl.days,KML.REST_LEAVE order by KML.YEARLEAVE asc", con);

            da1.Fill(ds1);

            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                if (Convert.ToInt32(dr1[4].ToString()) == 0)
                {
                    RestList.Add(new RestLeave() { on_going = "", yearleave = dr1[0].ToString(), leaveright = dr1[1].ToString(), massleave = dr1[2].ToString(), hasgotten = dr1[3].ToString(), restleave = Convert.ToInt32(dr1[4].ToString()) });
                }
                else if (ongoing == 0)
                {
                    RestList.Add(new RestLeave() { on_going = "", yearleave = dr1[0].ToString(), leaveright = dr1[1].ToString(), massleave = dr1[2].ToString(), hasgotten = dr1[3].ToString(), restleave = Convert.ToInt32(dr1[4].ToString()) });
                }
                else
                {
                    if (ongoing == Convert.ToInt32(dr1[4].ToString()))
                    {
                        RestList.Add(new RestLeave() { on_going = ongoing.ToString(), yearleave = dr1[0].ToString(), leaveright = dr1[1].ToString(), massleave = dr1[2].ToString(), hasgotten = dr1[3].ToString(), restleave = Convert.ToInt32(dr1[4].ToString()) - ongoing });
                        ongoing = 0;
                    }
                    else
                    {
                        if (ongoing > Convert.ToInt32(dr1[4].ToString()))
                        {
                            
                            RestList.Add(new RestLeave() { on_going = dr1[4].ToString(), yearleave = dr1[0].ToString(), leaveright = dr1[1].ToString(), massleave = dr1[2].ToString(), hasgotten = dr1[3].ToString(), restleave = 0 });
                            ongoing = ongoing - Convert.ToInt32(dr1[4].ToString());
                        }
                        else
                        {
                            RestList.Add(new RestLeave() { on_going = ongoing.ToString(), yearleave = dr1[0].ToString(), leaveright = dr1[1].ToString(), massleave = dr1[2].ToString(), hasgotten = dr1[3].ToString(), restleave = Convert.ToInt32(dr1[4].ToString()) - ongoing });
                            ongoing = 0;
                        }
                    } 
                }
            }
            return Json(RestList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult HaidHistory(string Id)
        {
            string tgl_skrg;
            
            tgl_skrg = DateTime.Now.ToString("MM/dd/yyyy");
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            OracleDataAdapter da = new OracleDataAdapter("Select FROMLEAVE from person.KMIHRMS_DETAIL_LEAVE KDL " +
                                                          "WHERE KDL.EMPID='" + Id + "' and KDL.LEAVE_TYPE='CH' ORDER BY FROMLEAVE asc", con);

            da.Fill(ds);
            onelast = "-";
            twolast = "-";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                if (DateTime.Parse(dr[0].ToString()).ToString("MM") == DateTime.Now.AddMonths(-1).ToString("MM"))
                {
                    if (dr[0].ToString() != "")
                    {
                        if (onelast == "-")
                        {
                            onelast = DateTime.Parse(dr[0].ToString()).ToString("d MMM yyyy");
                        }
                        else
                        {
                            onelast = onelast + " , " + DateTime.Parse(dr[0].ToString()).ToString("d MMM yyyy");
                        }
                    }
                }
                if (DateTime.Parse(dr[0].ToString()).ToString("MM") == DateTime.Now.AddMonths(-2).ToString("MM"))
                 {
                    if (dr[0].ToString() != "")
                    {
                        if (twolast == "-")
                        {
                            twolast = DateTime.Parse(dr[0].ToString()).ToString("d MMM yyyy");
                        }
                        else
                        {
                            twolast = twolast + " , " + DateTime.Parse(dr[0].ToString()).ToString("d MMM yyyy");
                        }
                    }
                    
                }
                

            }
            //return View(RestLeave);
            HaidLeave.Add(new HAID() { ONELAST = onelast, TWOLAST = twolast });
            return Json(HaidLeave, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Delete(string nik, DateTime fromdate, DateTime todate)
        {
            if (cek_data("*", "KMIHRMS_DETAIL_LEAVE", "EMPID='" + nik + "' and to_char(FROMLEAVE,'yyyy/mm/dd')='" + fromdate.ToString("yyyy/MM/dd") + "' and to_char(TOLEAVE,'yyyy/mm/dd')='" + todate.ToString("yyyy/MM/dd") + "' and status='Release'") == true)
            {

                string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = "delete from KMIHRMS_DETAIL_LEAVE where EMPID='" + nik + "' and to_char(FROMLEAVE,'yyyy/mm/dd')='" + fromdate.ToString("yyyy/MM/dd") + "' and to_char(TOLEAVE,'yyyy/mm/dd')='" + todate.ToString("yyyy/MM/dd") + "'";
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
        
        void sendmail(string namaapproved, string emailapproved, string employee, string namaemp, string bagian, string namabagian, DateTime fromdate, DateTime todate, string reason, string type, string alamat, string hpl, string tglkej, string jam)
        {
             try
             {
            SmtpClient SmtpServer =  new SmtpClient();
            MailMessage mail = new MailMessage();
            SmtpServer.Host = "mail.kawasaki.co.id";
            mail.From = new MailAddress("no_reply@kawasaki.co.id","KMI-INTRANET");
            string cond = "App. Section Head";
            
            if (tglkej != null)
            {
                eventdate = DateTime.ParseExact(tglkej, "dd/MM/yyyy", null);
            }
            if (hpl != null)
            {
                birthdate = DateTime.ParseExact(hpl, "dd/MM/yyyy", null);
            }
            
            Approve = "<a href = 'http://157.116.62.33/HRMS/ConfirmationLeave.aspx?status=Approve&con=" + cond + "&empnm=" + namaemp + "&bagian=" + bagian + "&namabagian=" + namabagian + "&fromdate=" + fromdate + "&todate=" + todate + "&reason=" + reason + "&empid=" + employee + "&type=" + type + "&alamat=" + alamat + "&hpl=" + hpl + "&tglkej=" + tglkej + "&jam=" + jam + "' style=background-color:#EB7035;color:black;font-size:16px;line-height:40px;text-align:center;width:110px;border-radius: 3px 3px 3px 3px;><span>Approve</span></a>";

            Reject = "<a href = 'http://157.116.62.33/HRMS/ConfirmationLeave.aspx?status=Reject&con=" + cond + "&empnm=" + namaemp + "&bagian=" + bagian + "&namabagian=" + namabagian + "&fromdate=" + fromdate + "&todate=" + todate + "&reason=" + reason + "&empid=" + employee + "&type=" + type + "&alamat=" + alamat + "&hpl=" + hpl + "&tglkej=" + tglkej + "&jam=" + jam + "' style=background-color:#EB7035;color:black;font-size:16px;line-height:40px;text-align:center;width:110px;border-radius: 3px 3px 3px 3px;><span>Reject</span></a>";
            
            if (type == "CH")
            {
                leave_name= "CUTI HAID";
            }
            else if (type == "CT")
            {
                leave_name= "CUTI TAHUNAN";
            }
            else if (type == "DI")
            {
                leave_name = "DENGAN IZIN";
            }
            else if (type == "I")
            {
                leave_name = "IZIN";
            }
            else if (type == "CM")
            {
                leave_name = "CUTI MELAHIRKAN";
            }
            else if (type == "CI")
            {
                leave_name = "CUTI IBADAH";
            }
            mail.To.Add(new MailAddress(emailapproved));
            mail.Subject = "NOTIFICATION : LEAVE APPROVAL";
            mail.Body += "Dear " + namaapproved + ", <br/><br/>";
            mail.Body += "Please " + cond + " on KMI-INTRANET Application for :<br/><br/>";
            mail.Body += "<table style='color:black;'><tr><td>Employee id<td><td>:</td><td>" + employee + "</td></tr>";
            mail.Body += "<tr><td>Employee name<td><td>:</td><td>" + namaemp + "</td></tr>";
            mail.Body += "<tr><td>Costcenter<td><td>:</td><td>" + bagian + " (" + namabagian + ")</td></tr>";
            
            mail.Body += "<tr><td>Leave Type<td><td>:</td><td>" + leave_name + "</td></tr>";
            if (type == "CM")
            {
                mail.Body += "<tr><td>Birth Date Plan<td><td>:</td><td>" + String.Format("{0:d MMM yyyy}", birthdate) + "</td></tr>";
            }
            if (type == "DI")
            {
                mail.Body += "<tr><td>Event Date<td><td>:</td><td>" + String.Format("{0:d MMM yyyy}", eventdate) + " " + jam + "</td></tr>";
            }
            
            mail.Body += "<tr><td>Leave date<td><td>:</td><td>" + String.Format("{0:d MMM yyyy}", fromdate) + " - " + String.Format("{0:d MMM yyyy}", todate) + "</td></tr>";
            mail.Body += "<tr><td>Address<td><td>:</td><td>" + alamat + "</td></tr>";
            if (type != "CH" || type != "CM")
            {
                mail.Body += "<tr><td>Reason<td><td>:</td><td>" + reason + "</td></tr>";
            }
            mail.Body += "</table>";
            if (type == "DI")
            {
                mail.Body += "<br/><p style=color:red;>Note : Tolong periksa Lampiran dan diserahkan ke HRD(*) </p><br/>";
            }
            mail.Body += "<br/><br/>Please click below button for approve/reject <br/><br/><br/>";
            mail.Body += Approve + " " + Reject + "<br/><br/><br/><br/>";
            mail.Body += "To See All Approval Data, Please visit http://kmiapp/kmi-intranet/LeaveApprove/Index <br/><br/><br/>";
            mail.Body += "Best Regards <br/><br/>";
            mail.Body += "KMI-INTRANET </p><br/><br/>";
            mail.IsBodyHtml = true;
            SmtpServer.Send(mail);
        }
        catch (Exception)
        {
            MessageBoxShow("Lotus Notes Has Not Been Installed");

        }

        }
        [HttpPost]
        public ViewResult Index(LeaveRequest model)
        {

            return View(model);
        }
        public ActionResult Popup()
        {
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");
            if (Session["UserSecurity"].ToString() == "ADMIN")
            {
                da = new OracleDataAdapter("select p.EMP_ID,p.EMP_NAME,p.CC_DESC,p.CTM_EMP_SEX,case when p.CTM_EMP_MARITAL_STATUS='TK' then 'BELUM MENIKAH' ELSE 'MENIKAH' END STATUTA,sum(k.REST_LEAVE) from PERSONAL_MASTER p " +
                    "inner join KMIHRMS_MASTER_LEAVE k on k.empid=p.EMP_ID " +
                    "WHERE p.AREA_CODE IN (" + Session["Autorized"] + ") AND p.STATUS='ACTIVE' AND p.EMP_TITLE_ID IN ('J007','J008','J009','J010','J011') AND p.EMP_TYPE IN ('M01','P01','C01') group by EMP_ID,EMP_NAME,CC_DESC,CTM_EMP_SEX,CTM_EMP_MARITAL_STATUS", con);
            }
            else
            {
                da = new OracleDataAdapter("select p.EMP_ID,p.EMP_NAME,p.CC_DESC,p.CTM_EMP_SEX,case when p.CTM_EMP_MARITAL_STATUS='TK' then 'BELUM MENIKAH' ELSE 'MENIKAH' END STATUTA,sum(k.REST_LEAVE) from PERSONAL_MASTER p " +
                    "inner join KMIHRMS_MASTER_LEAVE k on k.empid=p.EMP_ID " +
                    "WHERE p.EMP_ID='" + Session["USER"] + "' AND p.STATUS='ACTIVE' AND p.EMP_TITLE_ID IN ('J007','J008','J009','J010','J011') AND p.EMP_TYPE IN ('M01','P01','C01') group by EMP_ID,EMP_NAME,CC_DESC,CTM_EMP_SEX,CTM_EMP_MARITAL_STATUS", con);
            }
            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ModalPopup.Add(new Popup() { Employeeid = dr[0].ToString(), Employeenm = dr[1].ToString(), Department = dr[2].ToString(), gender = dr[3].ToString(), statuta = dr[4].ToString(), sisacuti = dr[5].ToString() });
            }
            return View(ModalPopup);
        }
        public ActionResult Popup1()
        {
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            da = new OracleDataAdapter("select p.EMP_ID,p.EMP_NAME,p.CC_DESC,p.CTM_EMP_SEX,case when p.CTM_EMP_MARITAL_STATUS='TK' then 'BELUM MENIKAH' ELSE 'MENIKAH' END STATUTA,sum(k.REST_LEAVE) from PERSONAL_MASTER p " +
                   "inner join KMIHRMS_MASTER_LEAVE k on k.empid=p.EMP_ID " +
                   "WHERE p.EMP_ID='" + Session["USER"] + "' AND p.STATUS='ACTIVE' AND p.EMP_TITLE_ID IN ('J007','J008','J009','J010','J011') AND p.EMP_TYPE IN ('M01','P01','C01') group by EMP_ID,EMP_NAME,CC_DESC,CTM_EMP_SEX,CTM_EMP_MARITAL_STATUS", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                ModalPopup.Add(new Popup() { Employeeid = dr[0].ToString(), Employeenm = dr[1].ToString(), Department = dr[2].ToString(), gender = dr[3].ToString(), statuta = dr[4].ToString(), sisacuti = dr[5].ToString() });
            }
            return Json(ModalPopup, JsonRequestBehavior.AllowGet);
        }
    }
}
