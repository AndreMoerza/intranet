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
    public class Riwayat
    {
        [Display(Name = "Autorized Hirarchy")]
        public string[] Autorized { get; set; }
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

                    cmd.CommandText = "select hirarki_code,description from HIRARKI_CUTI";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;

                    connection.Open();
                    using (OracleDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            _items.Add(new SelectListItem { Text = rdr.GetString(0) + "  " + rdr.GetString(1), Value = rdr.GetString(0) });
                        }
                    }
                }
                return _items;
            }
        }
    }
    public class FilterTable
    {
        public string empid { get; set; }
        public string empnm { get; set; }
        public string leavetype { get; set; }
        public DateTime fromleave { get; set; }
        public DateTime toleave { get; set; }
        public string days { get; set; }
    }
    public class SearchModel
    {
        public List<FilterTable> FilterTable { get; set; }
        public string empid { get; set; }
        public string tgl { get; set; }
        public string[] autorized { get; set; }
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

                    cmd.CommandText = "select hirarki_code,description from HIRARKI_CUTI";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;

                    connection.Open();
                    using (OracleDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            _items.Add(new SelectListItem { Text = rdr.GetString(0) + "  " + rdr.GetString(1), Value = rdr.GetString(0) });
                        }
                    }
                }
                return _items;
            }
        }
    }
}