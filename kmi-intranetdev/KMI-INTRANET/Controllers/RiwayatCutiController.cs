using System;
using System.Collections.Generic;
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
    public class RiwayatCutiController : Controller
    {
        //
        // GET: /RiwayatCuti/
        string query;
        string sb;
        private List<FilterTable> FilterTable = new List<FilterTable>();
        OracleConnection con;
        OracleConnection cmd;
        OracleDataAdapter da;
        DataSet ds = new DataSet();
  
        public ActionResult Filter(SearchModel model)
        {
            string str = Request.Params["btn1"];
            if (str == "Tampilkan")
            {
                if (model.autorized != null)
                {
                    for (int i = 0; i < model.autorized.Count(); i++)
                    {
                        if (i == 0)
                        {
                            sb += model.autorized.GetValue(i).ToString();
                        }
                        else
                        {
                            sb += "," + model.autorized.GetValue(i).ToString();
                        }
                    }
                }
                else
                {
                    sb = "";
                }
                if (!string.IsNullOrEmpty(model.empid) || !string.IsNullOrEmpty(model.tgl) || !string.IsNullOrEmpty(sb))
                {

                    con = new OracleConnection("Data Source=HRMSDEV;user id=PERSON;password=PERSON;Unicode=True;");
                    if (!string.IsNullOrEmpty(model.empid) && string.IsNullOrEmpty(model.tgl) && string.IsNullOrEmpty(sb))
                    {
                        query = "select EMPID,EMP_NAME,LEAVE_NAME,FROMLEAVE,TOLEAVE,DAYS " +
                                "from TAMPIL_DETAIL_LEAVE WHERE EMPID like '" + model.empid + "'";
                    }
                    else if (string.IsNullOrEmpty(model.empid) && !string.IsNullOrEmpty(model.tgl) && string.IsNullOrEmpty(sb))
                    {
                        query = "select EMPID,EMP_NAME,LEAVE_NAME,FROMLEAVE,TOLEAVE,DAYS " +
                               "from TAMPIL_DETAIL_LEAVE WHERE TO_DATE('" + model.tgl + "','DD/MM/YYYY') BETWEEN FROMLEAVE and TOLEAVE";
                    }
                    else if (string.IsNullOrEmpty(model.empid) && string.IsNullOrEmpty(model.tgl) && !string.IsNullOrEmpty(sb))
                    {
                        query = "select EMPID,EMP_NAME,LEAVE_NAME,FROMLEAVE,TOLEAVE,DAYS " +
                               "from TAMPIL_DETAIL_LEAVE WHERE AREA_CODE IN (" + sb + ")";
                    }
                    else if (!string.IsNullOrEmpty(model.empid) && !string.IsNullOrEmpty(model.tgl) && string.IsNullOrEmpty(sb))
                    {
                        query = "select EMPID,EMP_NAME,LEAVE_NAME,FROMLEAVE,TOLEAVE,DAYS " +
                               "from TAMPIL_DETAIL_LEAVE WHERE EMPID like '" + model.empid + "' " +
                               "AND (TO_DATE('" + model.tgl + "','DD/MM/YYYY') BETWEEN FROMLEAVE and TOLEAVE)";
                    }
                    else if (!string.IsNullOrEmpty(model.empid) && string.IsNullOrEmpty(model.tgl) && !string.IsNullOrEmpty(sb))
                    {
                        query = "select EMPID,EMP_NAME,LEAVE_NAME,FROMLEAVE,TOLEAVE,DAYS " +
                               "from TAMPIL_DETAIL_LEAVE WHERE EMPID like '" + model.empid + "' " +
                               "AND AREA_CODE IN (" + sb + ")";
                    }
                    else if (string.IsNullOrEmpty(model.empid) && !string.IsNullOrEmpty(model.tgl) && !string.IsNullOrEmpty(sb))
                    {
                        query = "select EMPID,EMP_NAME,LEAVE_NAME,FROMLEAVE,TOLEAVE,DAYS " +
                                "from TAMPIL_DETAIL_LEAVE WHERE " +
                                "(TO_DATE('" + model.tgl + "','DD/MM/YYYY') BETWEEN FROMLEAVE and TOLEAVE) " +
                                "AND AREA_CODE IN (" + sb + ")";
                    }
                    else if (!string.IsNullOrEmpty(model.empid) && !string.IsNullOrEmpty(model.tgl) && !string.IsNullOrEmpty(sb))
                    {
                        query = "select EMPID,EMP_NAME,LEAVE_NAME,FROMLEAVE,TOLEAVE,DAYS " +
                                "from TAMPIL_DETAIL_LEAVE WHERE EMPID like '" + model.empid + "' " +
                                "AND (TO_DATE('" + model.tgl + "','DD/MM/YYYY') BETWEEN FROMLEAVE and TOLEAVE) " +
                                "AND AREA_CODE IN (" + sb + ")";
                    }
                    OracleDataAdapter da = new OracleDataAdapter(query, con);

                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        FilterTable.Add(new FilterTable() { empid = dr[0].ToString(), empnm = dr[1].ToString(), leavetype = dr[2].ToString(), fromleave = DateTime.Parse(dr[3].ToString()), toleave = DateTime.Parse(dr[4].ToString()), days = dr[5].ToString() });
                    }
                    model.FilterTable = FilterTable;
                }
            }
            else if (str == "Bersihkan")
            {
                ModelState.Clear();
            }
            return View(model);
        }
        public ActionResult ExportToExcel()
        {
            var products = new System.Data.DataTable("teste");
            products.Columns.Add("col1", typeof(int));
            products.Columns.Add("col2", typeof(string));

            products.Rows.Add(1, "product 1");
            products.Rows.Add(2, "product 2");
            products.Rows.Add(3, "product 3");
            products.Rows.Add(4, "product 4");
            products.Rows.Add(5, "product 5");
            products.Rows.Add(6, "product 6");
            products.Rows.Add(7, "product 7");


            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View();
        }
    }
}
