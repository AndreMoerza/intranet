using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using KMI_INTRANET.Models;
using System.Data;
using System.Web.Routing;
using System.Data.OracleClient;
namespace KMI_INTRANET.Controllers
{
    public class AccountController : Controller
    {
        private LoginModel db = new LoginModel();
        LoginModel loginmodel = new LoginModel();
        private List<LoginModel> LoginModel = new List<LoginModel>();
        OracleConnection con;
        OracleConnection cmd;
        OracleDataAdapter da;
        public OracleDataReader objDataReader;
        DataSet ds = new DataSet();
        [HttpGet]
        public ActionResult Login(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                 RedirectToAction("Index", "LeaveApprove");
            }
            var viewModels = new LoginModel.MyViewModel
            {
                Memos = db.GetMemos(),
                Charts = db.GetCharts(),
                Newss = db.GetNews(),
                p2k3s = db.Getp2k3s()
            };
            return View(viewModels);
            
        }

        [HttpPost]
        public ActionResult Login(KMI_INTRANET.Models.LoginModel.MyViewModel model)
        {
                if (ModelState.IsValid)
                {
                    if (model.IsValid(model.UserName, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        Session["USER"] = model.UserName;
                        string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";

                        using (OracleConnection connection = new OracleConnection(connectionString))
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.CommandText = "select t.nickname from person.CTM_EMPLOYEE_MASTER_TAB t where t.emp_id='" + Session["USER"] + "'";
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = connection;

                            connection.Open();
                            using (OracleDataReader rdr = cmd.ExecuteReader())
                            {
                                rdr.Read();
                                if (DBNull.Value.Equals(rdr.GetString(0)))
                                {
                                    Session["fullname"] = "-";
                                }
                                else
                                {
                                    Session["fullname"] = rdr.GetString(0);
                                }

                            }
                            connection.Close();
                            OracleCommand cmd1 = new OracleCommand();
                            cmd1.CommandText = "select t.LEV,t.autorized from person.KMIINTRANET_USER t where t.USERNAME='" + Session["USER"] + "'";
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Connection = connection;

                            connection.Open();
                            using (OracleDataReader rdr1 = cmd1.ExecuteReader())
                            {
                                rdr1.Read();
                                if (rdr1.GetString(0) == "")
                                {
                                    Session["UserSecurity"] = "-";
                                }
                                else
                                {
                                    Session["UserSecurity"] = rdr1.GetString(0);
                                }
                                
                                if (rdr1.GetString(0) == "USER")
                                {
                                    Session["Autorized"] = "";
                                }
                                else
                                {
                                    Session["Autorized"] = rdr1.GetString(1);
                                }

                            }
                            connection.Close();
                            //if (!String.IsNullOrEmpty(id))
                            //{
                            //    string param1 = this.Request.QueryString["param.1"];
                            //    string param2 = this.Request.QueryString["param.2"];
                            //    Redirect(Url.Action("Index", "LeaveApprove") + "?id=" + id);
                            //}
                            //else
                            //{
                                //return RedirectToAction("Index", "Home");
                            //}
                            string returnUrl = Request.QueryString["ReturnUrl"];
                                if (!String.IsNullOrEmpty(returnUrl))
                                {
                                    return Redirect(returnUrl);
                                }
                                else
                                {
                                    return RedirectToAction("Index", "Home");
                                }

                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user name or password incorrect.");
                    }
                
            }
                var viewModels = new LoginModel.MyViewModel
                {
                    Memos = db.GetMemos(),
                    Charts = db.GetCharts(),
                    Newss = db.GetNews(),
                    p2k3s = db.Getp2k3s()
                };
                return View(viewModels);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["USER"] = null;
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
               bool changePasswordSucceeded;
               try
               {
                    //MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                
                    using (OracleConnection connection = new OracleConnection(connectionString))
                    {
                        if (model.NewPassword != null)
                        {
                            OracleCommand cmd1 = new OracleCommand();
                            cmd1.CommandText = "update KMIINTRANET_USER set pass='" + model.NewPassword + "' where username='" + Session["USER"] + "'";
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Connection = connection;
                            connection.Open();
                            cmd1.ExecuteNonQuery();
                            connection.Close();
                        }
                        if (model.Nickname != null)
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.CommandText = "update CTM_EMPLOYEE_MASTER_TAB set nickname='" + model.Nickname + "' where emp_id='" + Session["USER"] + "'";
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = connection;
                            connection.Open();
                            cmd.ExecuteNonQuery();
                            connection.Close();
                        }
                            OracleCommand cmd2 = new OracleCommand();
                            cmd2.CommandText = "select t.nickname from person.CTM_EMPLOYEE_MASTER_TAB t where t.emp_id='" + Session["USER"] + "'";
                            cmd2.CommandType = CommandType.Text;
                            cmd2.Connection = connection;

                            connection.Open();
                            using (OracleDataReader rdr = cmd2.ExecuteReader())
                            {
                                rdr.Read();
                                if (DBNull.Value.Equals(rdr.GetString(0)))
                                {
                                    Session["fullname"] = "-";
                                }
                                else
                                {
                                    Session["fullname"] = rdr.GetString(0);
                                }

                            }
                        changePasswordSucceeded = true;
                    }
                
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

               if (changePasswordSucceeded = true)
               {
                   //return RedirectToAction("ChangePasswordSuccess");
               }
               else
               {
                   ModelState.AddModelError("", "The new password is invalid.");
               }
            }

            return View(model);
        }
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult Show(string id)
        {
            string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                string sql = "Select img from person.ctm_emp_img_tab where emp_id='" + id + "'";
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
        public ActionResult Showmemo(string id)
        {
            string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                string sql = "Select MEMO_FILE from KMIINTRANET_MASTER_MEMO where MEMO_ID ='" + id + "'";
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
        public ActionResult Showchart(string id)
        {
            string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                string sql = "Select CHART_FILE from KMIINTRANET_MASTER_CHART where CHART_ID ='" + id + "'";
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
        public ActionResult Shownews(string id)
        {
            string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                string sql = "Select NEWS_FILE from KMIINTRANET_MASTER_NEWS where NEWS_ID ='" + id + "'";
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
        public ActionResult Showp2k3(string id)
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
    }
}
