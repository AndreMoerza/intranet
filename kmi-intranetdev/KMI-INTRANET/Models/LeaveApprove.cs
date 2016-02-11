using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace KMI_INTRANET.Models
{
    public class LeaveApprove
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
}