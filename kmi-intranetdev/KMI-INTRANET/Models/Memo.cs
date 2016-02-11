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
    public class Memo
    {
        public string idmemo { get; set; }
        public string Theme { get; set; }
        public string Autorize { get; set; }
        public string Autorize_detail { get; set; }
        public HttpPostedFileBase MemoFile { get; set; }
        public string ValidFrom { get; set; }
        public string ValidUntil { get; set; }
    }
    public class CreateMemo
    {
        public string idmemo { get; set; }
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Required(ErrorMessage = "Theme is required")]
        [Display(Name = "Theme")]
        public string Theme { get; set; }
        [Required(ErrorMessage = "Autorize are required")]
        [Display(Name = "Autorize")]
        public string Autorize { get; set; }
        public string Autorize_detail { get; set; }
        [Required(ErrorMessage = "File Memo is required")]
        [Display(Name = "Memo (Pdf*)")]
        public HttpPostedFileBase MemoFile { get; set; }
        [Required(ErrorMessage = "Valid From is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Valid From")]
        public DateTime ValidFrom { get; set; }
         [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Valid Until is required")]
        [Display(Name = "Valid Until")]
        public DateTime ValidUntil { get; set; }
    }
    public class EditMemo
    {
        public string idmemo { get; set; }
        [Required(ErrorMessage = "Theme is required")]
        [Display(Name = "Theme")]
        public string Theme { get; set; }
        [Required(ErrorMessage = "Autorize are required")]
        [Display(Name = "Autorize")]
        public string Autorize { get; set; }
        public string Autorize_detail { get; set; }
        [Required(ErrorMessage = "File Memo is required")]
        [Display(Name = "Memo (Pdf*)")]
        public HttpPostedFileBase MemoFile { get; set; }
        [Required(ErrorMessage = "Valid From is required")]
        [Display(Name = "Valid From")]
        public DateTime ValidFrom { get; set; }
        [Required(ErrorMessage = "Valid Until is required")]
        [Display(Name = "Valid Until")]
        public DateTime ValidUntil { get; set; }
    }
}