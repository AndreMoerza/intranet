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
    public class Chart
    {
        public string idchart { get; set; }
        public string title { get; set; }
        public HttpPostedFileBase ChartFile { get; set; }
    }
    public class CreateChart
    {
        public string idchart { get; set; }
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters long.")]
        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Title")]
        public string title { get; set; }
        [Required(ErrorMessage = "Image is required")]
        [Display(Name = "Image (JPG/PNG*)")]
        public HttpPostedFileBase ChartFile { get; set; }
        
    }
    public class EditChart
    {
        public string idchart { get; set; }
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters long.")]
        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Title")]
        public string title { get; set; }
        [Required(ErrorMessage = "Image is required")]
        [Display(Name = "Image (JPG/PNG*)")]
        public HttpPostedFileBase ChartFile { get; set; }
    }
}