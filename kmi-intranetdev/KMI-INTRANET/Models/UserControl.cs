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
    public class UserControlling
    {
        public string username { get; set; }
        public string empnm { get; set; }
        public string pass { get; set; }
        public string level { get; set; }
        public string autorized { get; set; }
        private List<SelectListItem> _items;
        public IEnumerable<SelectListItem> Items
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

    public class NewUser : IValidatableObject
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string User { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public string Pass { get; set; }

        [Required(ErrorMessage = "Level is required")]
        [Display(Name = "Level")]
        public string Level { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Level != "USER" && Autorized == null)
            {
                yield return new ValidationResult("Autorized Hirarchy is required");
            }
        }
        //[Required(ErrorMessage = "Autorized Hirarchy is required")]
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
    public class EmpidPopup
    {
        public string Employeeid { get; set; }
        public string Employeenm { get; set; }
    }
   
}
 