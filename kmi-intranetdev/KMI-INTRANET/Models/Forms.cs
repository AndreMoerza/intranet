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
    public class Forms
    {
        public string idform { get; set; }
        public string group { get; set; }
        public string formname { get; set; }
        public HttpPostedFileBase formFile { get; set; }
    }
    public class HRForms
    {
        public string idform { get; set; }
        public string group { get; set; }
        public string formname { get; set; }
        public HttpPostedFileBase formFile { get; set; }
    }
    public class ISForms
    {
        public string idform { get; set; }
        public string group { get; set; }
        public string formname { get; set; }
        public HttpPostedFileBase formFile { get; set; }
    }
    public class PRForms
    {
        public string idform { get; set; }
        public string group { get; set; }
        public string formname { get; set; }
        public HttpPostedFileBase formFile { get; set; }
    }
    public class CreateForms
    {
        public string idform { get; set; }
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Required(ErrorMessage = "Nama Form tidak boleh kosong")]
        [Display(Name = "Nama Form")]
        public string formname { get; set; }
        [Required(ErrorMessage = "Group tidak boleh kosong")]
        [Display(Name = "Group")]
        public string group { get; set; }
        [Required(ErrorMessage = "File Form tidak boleh kosong")]
        [Display(Name = "Forms (.Xls/.Pdf/.Docs)")]
        public HttpPostedFileBase formFile { get; set; }
       
    }
    public class EditForms
    {
        public string idform { get; set; }
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Required(ErrorMessage = "Nama Form tidak boleh kosong")]
        [Display(Name = "Nama Form")]
        public string formname { get; set; }
        [Required(ErrorMessage = "Group tidak boleh kosong")]
        [Display(Name = "Group")]
        public string group { get; set; }
        [Required(ErrorMessage = "File Form tidak boleh kosong")]
        [Display(Name = "Form (.Xls/.Pdf/.Docs)")]
        public HttpPostedFileBase formFile { get; set; }
       
    }
}