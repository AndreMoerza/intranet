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
    
    public class LeaveModel
    {
        public string nik { get; set; }
        public string empnm { get; set; }
        public string dept { get; set; }
        public string type { get; set; }
        public DateTime fromleave { get; set; }
        public DateTime toleave { get; set; }
        public string typecuti { get; set; }
        public string namacuti { get; set; }
        public string deskripsi { get; set; }
        public string reason { get; set; }
        public string alamat { get; set; }
        public string Detfrom { get; set; }
        public string Detto { get; set; }
        public Double totaldays { get; set; }
        public string status { get; set; }
    }
    
    public class RestLeave
    {
        public string yearleave { get; set; }
        public string leaveright { get; set; }
        public string hasgotten { get; set; }
        public string massleave { get; set; }
        public int restleave { get; set; }
        public string on_going { get; set; }
    }
    public class HAID
    {
        public string ONELAST { get; set; }
        public string TWOLAST { get; set; }
    }
    public class LeaveRequest
    {
        [Required(ErrorMessage = "Sifat Cuti Wajib Diisi")]
        [Display(Name = "Sifat Cuti")]
        public string SifatCuti { get; set; }
        [Required(ErrorMessage = "Type Cuti Wajib Diisi")]
        [Display(Name = "TYPE CUTI")]
        public string SelectedItem { get; set; }
        private List<SelectListItem> _items;
        public List<SelectListItem> Items
        {
            get
            {
                _items = new List<SelectListItem>();
                string connectionString = "Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;";
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    OracleCommand cmd = new OracleCommand();

                    cmd.CommandText = "select t.LEAVE_ID,t.LEAVE_NAME from person.CTM_LEAVE_TYPE_TAB t where t.LEAVE_ID in ('CH','CT','DI','I','CM','CI') order by t.LEAVE_ID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;

                    connection.Open();
                    using (OracleDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            _items.Add(new SelectListItem { Text = rdr.GetString(1), Value = rdr.GetString(0) });
                            // _items.Add(rdr.GetString(1));
                        }
                    }
                }
                return _items;
            }
        }
        [Required(ErrorMessage = "Silahkan pilih karyawan")]
        [Display(Name = "NIK")]
        public string Empid { get; set; }
        [Required(ErrorMessage = "Tanggal Mulai - Selesai")]
        [Display(Name = "TANGGAL MULAI - SELESAI")]
        public string fromleave { get; set; }
        [Required(ErrorMessage = "Tanggal Selesai")]
        [Display(Name = "TANGGAL SELESAI")]
        public string toleave { get; set; }
        [Display(Name = "ALASAN")]
        public string reasonpost { get; set; }
        [Display(Name = "ALASAN")]
        public string reasonpost1 { get; set; }
        [Required(ErrorMessage = "Alamat Wajib Diisi")]
        [Display(Name = "ALAMAT")]
        public string Alamat { get; set; }
        public string detail1 { get; set; }
        public string detail { get; set; }
        public string hari { get; set; }
        public string tglkej { get; set; }
        public string times { get; set; }
        public string hpl { get; set; }
    }
    public class Popup
    {
        public string Employeeid { get; set; }
        public string Employeenm { get; set; }
        public string Department { get; set; }
        public string gender { get; set; }
        public string statuta { get; set; }
        public string sisacuti { get; set; }
    }
 
}