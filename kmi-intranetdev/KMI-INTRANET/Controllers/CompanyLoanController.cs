using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using System.Data.OracleClient;
using KMI_INTRANET.Models;
using System.Net.Mail;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.ApplicationServices;


namespace KMI_INTRANET.Controllers
{
    public class CompanyLoanController : Controller
    {
        //
        // GET: /CompanyLoan/

        public ActionResult Index()
        {
            return View();
        }

    }
}
