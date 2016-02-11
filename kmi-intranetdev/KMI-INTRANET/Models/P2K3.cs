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
    public class P2K3
    {
        public string idP2K3 { get; set; }
        public string title { get; set; }
        [StringLength(500, ErrorMessage = "{0} must be maximum {1} characters long.")]
        public string isi { get; set; }
        public HttpPostedFileBase P2K3File { get; set; }
        public string stat { get; set; }
        public string datep2k3 { get; set; }
    }
    public class CreateP2K3
    {
        public string idP2K3 { get; set; }
       [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters long.")]
        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Title")]
        public string title { get; set; }
        [StringLength(500, ErrorMessage = "{0} must be maximum {1} characters long.")]
        [Required(ErrorMessage = "Information is required")]
        [Display(Name = "Information")]
        public string isi { get; set; }
        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        public string stat { get; set; }
        [Required(ErrorMessage = "Image is required")]
        [Display(Name = "Image (JPG/PNG*)")]
        public HttpPostedFileBase P2K3File { get; set; }

    }
}