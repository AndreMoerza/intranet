using System.Data;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.OracleClient;
using System.Web.Security;
using KMI_INTRANET.Models;
namespace KMI_INTRANET.Models
{
    public class LoginModel
    {
        public List<News> GetNews()
        {
            List<News> Newss = new List<News>();
            DataSet ds = new DataSet();
            OracleConnection con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            OracleDataAdapter da = new OracleDataAdapter("select NEWS_ID,NEWS_TITLE,NEWS,CREATE_DATE from KMIINTRANET_MASTER_NEWS WHERE rownum<='3' AND STATUS='Active' order by NEWS_ID desc", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Newss.Add(new News() { idnews = dr[0].ToString(), title = dr[1].ToString(), isi = dr[2].ToString(), newsdate = String.Format("{0:d MMMM yyyy}", dr[3])});
            }
            return Newss;
        }
        public List<Chart> GetCharts()
        {
            List<Chart> Chart = new List<Chart>();
            DataSet ds = new DataSet();
            OracleConnection con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            OracleDataAdapter da = new OracleDataAdapter("select CHART_ID,CHART_TITLE from KMIINTRANET_MASTER_CHART order by chart_id asc", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Chart.Add(new Chart() { idchart = dr[0].ToString(), title = dr[1].ToString()});
            }
            return Chart;
        }
        public List<Memo> GetMemos()
        {
            List<Memo> Memo = new List<Memo>();
            DataSet ds = new DataSet();
            OracleConnection con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            OracleDataAdapter da = new OracleDataAdapter("select MEMO_ID,MEMO_THEME from KMIINTRANET_MASTER_MEMO", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Memo.Add(new Memo() { idmemo = dr[0].ToString(), Theme = dr[1].ToString() });
            }
            return  Memo ;
        }
        public List<P2K3> Getp2k3s()
        {
            List<P2K3> P2K3 = new List<P2K3>();
            DataSet ds = new DataSet();
            OracleConnection con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");

            OracleDataAdapter da = new OracleDataAdapter("select P2K3_ID,P2K3_TITLE,P2K3_INFO,CREATE_DATE from KMIINTRANET_MASTER_P2K3 where rownum<='3' AND STATUS='Active' order by P2K3_ID desc", con);

            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                P2K3.Add(new P2K3() { idP2K3 = dr[0].ToString(), title = dr[1].ToString(), isi = dr[2].ToString(), datep2k3 = String.Format("{0:d MMMM yyyy}", dr[3]) });
            }
            return P2K3;
        }
        public class MyViewModel
        {
            public List<Memo> Memos { get; set; }
            public List<Chart> Charts { get; set; }
            public List<News> Newss { get; set; }
            public List<P2K3> p2k3s { get; set; }
            [Required]
            [Display(Name = "User name")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
            public bool IsValid(string _username, string _pwd)
            {
                string _sql = "Select UserName From KMIINTRANET_USER Where Username='" + _username + "' And Pass='" + _pwd + "'";
                OracleConnection cn = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");
                cn.Open();
                OracleCommand cmd = new OracleCommand(_sql, cn);
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                    return true;

                else
                    return false;

            }
        }
    }
    public class ChangePasswordModel
    {
        //[Required]
        [StringLength(8, ErrorMessage = "Its too long, Characters must be less than 8.")]
        [Display(Name = "Nickname")]
        public string Nickname { get; set; }
        //[Required]
        //[DataType(DataType.Password)]
        //[Display(Name = "Current password")]
        //public string OldPassword { get; set; }

        //[Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
   
}