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
    public class News
    {
        public string idnews { get; set; }
        public string title { get; set; }
        public string isi { get; set; }
        public string stat { get; set; }
        public string newsdate { get; set; }
        public HttpPostedFileBase NewsFile { get; set; }
    }
    public class CreateNews
    {
        public string idnews { get; set; }
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters long.")]
        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Title")]
        public string title { get; set; }
        [StringLength(500, ErrorMessage = "{0} must be maximum {1} characters long.")]
        [Required(ErrorMessage = "News is required")]
        [Display(Name = "News")]
        public string isi { get; set; }
        [Required(ErrorMessage = "Image is required")]
        [Display(Name = "Image (JPG/PNG*)")]
        public HttpPostedFileBase NewsFile { get; set; }
        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        public string stat { get; set; }

    }
}