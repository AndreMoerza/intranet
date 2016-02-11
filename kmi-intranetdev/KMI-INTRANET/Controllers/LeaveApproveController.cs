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
    //[Authorize(Roles = "20133272")]
    public class LeaveApproveController : Controller
    {
        private LeaveApprove db = new LeaveApprove();
        private List<LeaveApprove> LeaveApprove = new List<LeaveApprove>();
        OracleConnection con;
        OracleConnection cmd;
        OracleDataAdapter da;
        DataSet ds = new DataSet();
        string status, Approved, Rejected, namaapproved, emailapproved;
        string empid, empname, bagian, namabagian;
        //
        // GET: /LeaveApprove/
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
       
        public ActionResult Index()
        {
            if (Session["USER"] == null)
            {
                RedirectToAction("Login", "Account");
            }  
            else {
                con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");
                if (Session["UserSecurity"].ToString() == "SECTION HEAD")
                {
                    da = new OracleDataAdapter("select EMPID,EMP_NAME,LEAVE_NAME,FROMLEAVE,TOLEAVE,DAYS,STATUS,ADDRESS,REASON from TAMPIL_DETAIL_LEAVE WHERE STATUS IN ('Release') AND AREA_CODE IN (" + Session["Autorized"] + ")", con);
                }
                else if (Session["UserSecurity"].ToString() == "DEPARTMENT HEAD")
                {
                    da = new OracleDataAdapter("select EMPID,EMP_NAME,LEAVE_NAME,FROMLEAVE,TOLEAVE,DAYS,STATUS,ADDRESS,REASON from TAMPIL_DETAIL_LEAVE WHERE STATUS IN ('App. Section Head') AND AREA_CODE IN (" + Session["Autorized"] + ")", con);
                }
                else if (Session["UserSecurity"].ToString() == "ADMIN")
                {
                    da = new OracleDataAdapter("select EMPID,EMP_NAME,LEAVE_NAME,FROMLEAVE,TOLEAVE,DAYS,STATUS,ADDRESS,REASON from TAMPIL_DETAIL_LEAVE WHERE AREA_CODE IN (" + Session["Autorized"] + ")", con);
                }
                

                da.Fill(ds);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    LeaveApprove.Add(new LeaveApprove() { nik = dr[0].ToString(), empnm = dr[1].ToString(), namacuti = dr[2].ToString(), fromleave = DateTime.Parse(dr[3].ToString()), toleave = DateTime.Parse(dr[4].ToString()), totaldays = Convert.ToDouble(dr[5].ToString()), deskripsi = dr[6].ToString(), alamat = dr[7].ToString(), reason = dr[8].ToString() });
                }
            }
            return View(LeaveApprove);
        }
        public ViewResult Details(string nik, DateTime fromdate, DateTime todate)
        {
            con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            da = new OracleDataAdapter("select KDL.EMPID,PM.EMP_NAME,PM.CC_DESC,to_char(KDL.FROMLEAVE,'dd/mm/yyyy'),to_char(KDL.TOLEAVE,'dd/mm/yyyy'),KDL.STATUS,KDL.REASON,KDL.ADDRESS,KDL.LEAVE_TYPE,KDL.DAYS,KDL.STATUS from KMIHRMS_DETAIL_LEAVE KDL inner join PERSONAL_MASTER PM on KDL.EMPID=PM.EMP_ID " +
                "WHERE KDL.EMPID='" + nik + "' and to_char(KDL.FROMLEAVE,'yyyy/mm/dd')='" + fromdate.ToString("yyyy/MM/dd") + "' and to_char(KDL.TOLEAVE,'yyyy/mm/dd')='" + todate.ToString("yyyy/MM/dd") + "' AND PM.AREA_CODE IN (" + Session["Autorized"] + ")", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                LeaveApprove.Add(new LeaveApprove() { nik = dr[0].ToString(), empnm = dr[1].ToString(), dept = dr[2].ToString(), Detfrom = String.Format("{0:d MMM yyyy}", dr[3]), Detto = String.Format("{0:d MMM yyyy}", dr[4]), deskripsi = dr[5].ToString(), reason = dr[6].ToString(), alamat = dr[7].ToString(), type = dr[8].ToString(), totaldays = Convert.ToDouble(dr[9].ToString()), status = dr[10].ToString() });

            }
            return View(LeaveApprove);
        }
        public ActionResult Approve(string nik, DateTime fromdate, DateTime todate, string reason, string selectitem, string alamat)
        {
            
            if (Session["UserSecurity"].ToString() == "SECTION HEAD")
            {
                status = "App. Section Head";
            }
            else if (Session["UserSecurity"].ToString() == "DEPARTMENT HEAD")
            {
                status = "App. Department Head";
            }
            string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand();

                cmd.CommandText = "update KMIHRMS_DETAIL_LEAVE set status='" + status + "' where EMPID='" + nik + "' and to_char(FROMLEAVE,'yyyy/mm/dd')='" + fromdate.ToString("yyyy/MM/dd") + "' and to_char(TOLEAVE,'yyyy/mm/dd')='" + todate.ToString("yyyy/MM/dd") + "'";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;

                connection.Open();
                cmd.ExecuteNonQuery();
                OracleCommand cmd2 = new OracleCommand();
                cmd2.CommandText = "select t.EMP_ID,t.EMP_NAME,T.COST_CENTER,t.CC_DESC from person.personal_master t where t.emp_id='" + nik + "'";
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
            }
            
            if (status == "App. Section Head")
            {
                sendmail(empid, empname, bagian, namabagian, fromdate, todate, reason, selectitem, alamat, status); 
            }
            else if (status == "App. Department Head")
            {
                sendmailcomplete(empid, empname, bagian, namabagian, fromdate, todate, reason, selectitem, alamat);
            }

            return RedirectToAction("Index");
        }
        public ActionResult Reject(string nik, DateTime fromdate, DateTime todate, string reason, string selectitem, string alamat)
        {
            if (Session["UserSecurity"].ToString() == "SECTION HEAD")
            {
                status = "Rejected Section Head";
            }
            else if (Session["UserSecurity"].ToString() == "DEPARTMENT HEAD")
            {
                status = "Rejected Department Head";
            }
                string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    OracleCommand cmd = new OracleCommand();

                    cmd.CommandText = "update KMIHRMS_DETAIL_LEAVE set status='" + status + "' where EMPID='" + nik + "' and to_char(FROMLEAVE,'yyyy/mm/dd')='" + fromdate.ToString("yyyy/MM/dd") + "' and to_char(TOLEAVE,'yyyy/mm/dd')='" + todate.ToString("yyyy/MM/dd") + "'";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    OracleCommand cmd2 = new OracleCommand();
                    cmd2.CommandText = "select t.EMP_ID,t.EMP_NAME,T.COST_CENTER,t.CC_DESC from person.personal_master t where t.emp_id='" + nik + "'";
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
                }
                sendmailreject(empid, empname, bagian, namabagian, fromdate, todate, reason, selectitem, alamat ,status);

            return RedirectToAction("Index");
        }
        void sendmail(string employee, string namaemp, string bagian, string namabagian, DateTime fromdate, DateTime todate, string reason, string type, string alamat,string cond)
        {
            try
            {
                string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                    using (OracleConnection connection = new OracleConnection(connectionString))
                    {
                        OracleCommand cmd = new OracleCommand();
                        
                        cmd.CommandText = "select KLAT.app2_id app_id,CEMT.EMP_NAME,cemt.email,KLAT.app2_as app_as from person.KMIHRMS_LEAVE_APP_TAB KLAT " +
                            "INNER JOIN PERSON.CTM_EMPLOYEE_MASTER_TAB CEMT ON CEMT.EMP_ID=KLAT.APP2_ID " +
                            "where requsioner_id='" + bagian + "' and app_type='HR'";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = connection;
                        connection.Open();
                        using (OracleDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            namaapproved = rdr.GetString(1);
                            emailapproved = rdr.GetString(2);
                        }

                    }
                    SmtpClient SmtpServer = new SmtpClient();
                    MailMessage mail = new MailMessage();
                    SmtpServer.Host = "mail.kawasaki.co.id";
                    mail.From = new MailAddress("no_reply@kawasaki.co.id", "KMI-INTRANET");
                Approved = "<a href = 'http://157.116.62.33/HRMS/ConfirmationLeave.aspx?status=Approve&con=" + cond + "&empnm=" + namaemp + "&bagian=" + bagian + "&namabagian=" + namabagian + "&fromdate=" + fromdate + "&todate=" + todate + "&reason=" + reason + "&empid=" + employee + "&type=" + type + "&alamat=" + alamat + "' style=background-color:#EB7035;color:black;font-size:16px;line-height:40px;text-align:center;width:110px;border-radius: 3px 3px 3px 3px;><span>Approve</span></a>";

                Rejected = "<a href = 'http://157.116.62.33/HRMS/ConfirmationLeave.aspx?status=Reject&con=" + cond + "&empnm=" + namaemp + "&bagian=" + bagian + "&namabagian=" + namabagian + "&fromdate=" + fromdate + "&todate=" + todate + "&reason=" + reason + "&empid=" + employee + "&type=" + type + "&alamat=" + alamat + "' style=background-color:#EB7035;color:black;font-size:16px;line-height:40px;text-align:center;width:110px;border-radius: 3px 3px 3px 3px;><span>Reject</span></a>";
                                
                mail.To.Add(new MailAddress(emailapproved));
                mail.Subject = "NOTIFICATION : LEAVE APPROVAL";
                mail.Body += "Dear " + namaapproved + ", <br/><br/>";
                mail.Body += "Please " + cond + " on KMI-INTRANET Application for :<br/><br/>";
                mail.Body += "<table style='color:black;'><tr><td>Employee id<td><td>:</td><td>" + employee + "</td></tr>";
                mail.Body += "<tr><td>Employee name<td><td>:</td><td>" + namaemp + "</td></tr>";
                mail.Body += "<tr><td>Costcenter<td><td>:</td><td>" + bagian + " (" + namabagian + ")</td></tr>";
                mail.Body += "<tr><td>Leave date<td><td>:</td><td>" + String.Format("{0:d MMM yyyy}", fromdate) + " - " + String.Format("{0:d MMM yyyy}", todate) + "</td></tr>";
                mail.Body += "<tr><td>Leave Type<td><td>:</td><td>" + type + "</td></tr>";
                mail.Body += "<tr><td>Address<td><td>:</td><td>" + alamat + "</td></tr>";
                if (type != "CUTI HAID")
                {
                    mail.Body += "<tr><td>Reason<td><td>:</td><td>" + reason + "</td></tr>";
                }
                mail.Body += "</table><br/><br/>Please click below button for approve/reject <br/><br/><br/>";
                mail.Body += Approved + " " + Rejected + "<br/><br/><br/><br/>";
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
        void sendmailcomplete(string employee, string namaemp, string bagian, string namabagian, DateTime fromdate, DateTime todate, string reason, string type, string alamat)
        {
            try
            {
                
                SmtpClient SmtpServer = new SmtpClient();
                MailMessage mail = new MailMessage();
                SmtpServer.Host = "mail.kawasaki.co.id";
                mail.From = new MailAddress("no_reply@kawasaki.co.id", "KMI-INTRANET");
               
               
                mail.To.Add(new MailAddress("swdevelopment@kawasaki.co.id"));
                mail.Subject = "NOTIFICATION : LEAVE COMPLETE";
                mail.Body += "<p>Dear Human Resources Department, <br/><br/>";
                mail.Body += "Please complete on KMI-HRMS Application for :<br/><br/>";
                mail.Body += "<table style='color:black;'><tr><td>Employee id<td><td>:</td><td>" + employee + "</td></tr>";
                mail.Body += "<tr><td>Employee name<td><td>:</td><td>" + namaemp + "</td></tr>";
                mail.Body += "<tr><td>Costcenter<td><td>:</td><td>" + bagian + " (" + namabagian + ")</td></tr>";
                mail.Body += "<tr><td>Leave date<td><td>:</td><td>" + String.Format("{0:d MMM yyyy}", fromdate) + " - " + String.Format("{0:d MMM yyyy}", todate) + "</td></tr>";
                mail.Body += "<tr><td>Leave Type<td><td>:</td><td>" + type + "</td></tr>";
                mail.Body += "<tr><td>Address<td><td>:</td><td>" + alamat + "</td></tr>";
                if (type != "CUTI HAID") { 
                    mail.Body += "<tr><td>Reason<td><td>:</td><td>" + reason + "</td></tr>";
                }
                mail.Body += "</table><br/><br/>Please Visit http://KMIAPP/HRMS for complete it <br/><br/><br/>";
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
        void sendmailreject(string employee, string namaemp, string bagian, string namabagian, DateTime fromdate, DateTime todate, string reason, string type, string alamat, string status)
        {
            try
            {

                SmtpClient SmtpServer = new SmtpClient();
                MailMessage mail = new MailMessage();
                SmtpServer.Host = "mail.kawasaki.co.id";
                mail.From = new MailAddress("no_reply@kawasaki.co.id", "KMI-INTRANET");


                mail.To.Add(new MailAddress("swdevelopment@kawasaki.co.id"));
                mail.Subject = "NOTIFICATION : LEAVE REJECTED";
                mail.Body += "<p>Dear Human Resources Department, <br/><br/>";
                mail.Body += "Leave Request Has Rejected for :<br/><br/>";
                mail.Body += "<table style='color:black;'><tr><td>Employee id<td><td>:</td><td>" + employee + "</td></tr>";
                mail.Body += "<tr><td>Employee name<td><td>:</td><td>" + namaemp + "</td></tr>";
                mail.Body += "<tr><td>Costcenter<td><td>:</td><td>" + bagian + " (" + namabagian + ")</td></tr>";
                mail.Body += "<tr><td>Leave date<td><td>:</td><td>" + String.Format("{0:d MMM yyyy}", fromdate) + " - " + String.Format("{0:d MMM yyyy}", todate) + "</td></tr>";
                mail.Body += "<tr><td>Leave Type<td><td>:</td><td>" + type + "</td></tr>";
                mail.Body += "<tr><td>Address<td><td>:</td><td>" + alamat + "</td></tr>";
                if (type != "CUTI HAID")
                {
                    mail.Body += "<tr><td>Reason<td><td>:</td><td>" + reason + "</td></tr>";
                }
                mail.Body += "<tr><td>Status<td><td>:</td><td>" + status + "</td></tr>";
                mail.Body += "</table><br/><br/><br/><br/>";
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
    }
}
