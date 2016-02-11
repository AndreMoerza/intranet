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
            String filename;

            DataSet objDataSet = new DataSet();
            filename = "~/UploadedExcel/RiwayatCuti.xls";
            System.IO.FileInfo file = new System.IO.FileInfo(Server.MapPath(filename));
            System.IO.StreamWriter fs = new System.IO.StreamWriter(Server.MapPath(filename), false);

            fs.WriteLine("<?xml version=\"1.0\"?>");
            fs.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
            fs.WriteLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            fs.WriteLine("xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            fs.WriteLine("xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
            fs.WriteLine("xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");

            fs.WriteLine(" <Styles>");
            fs.WriteLine(" <Style ss:ID=\"HEAD\">");
            fs.WriteLine(" <Font ss:Size=\"12\" ss:Bold=\"1\"/>");
            fs.WriteLine(" <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/>");
            fs.WriteLine(" </Style>");
            fs.WriteLine(" <Style ss:ID=\"HeadField\">");
            fs.WriteLine(" <Font ss:Size=\"12\" ss:Bold=\"1\"/>");
            fs.WriteLine(" <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/>");
            fs.WriteLine(" <Borders>");
            fs.WriteLine(" <Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");;
            fs.WriteLine(" <Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
            fs.WriteLine(" <Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
            fs.WriteLine(" <Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
            fs.WriteLine(" </Borders>");
            fs.WriteLine(" <Interior ss:Color=\"#FFFF00\" ss:Pattern=\"Solid\"/>");
            fs.WriteLine(" </Style>");
       

            fs.WriteLine(" </Styles>");

            fs.WriteLine("<Worksheet ss:Name=\"Riwayat Cuti\">");
            fs.WriteLine(" <Table>");

            fs.WriteLine(" <Row ss:Index='1'>");
            fs.WriteLine(" <Cell ss:Index='1' ss:MergeAcross='9'  ss:StyleID='HEAD'>");
            fs.WriteLine(" <Data ss:Type=\"String\">KAWASAKI MOTOR INDONESIA</Data>");
            fs.WriteLine(" </Cell>");
            fs.WriteLine(" </Row>");
            fs.WriteLine(" <Row ss:Index='2'>");
            fs.WriteLine(" <Cell ss:Index='1' ss:MergeAcross='9'  ss:StyleID='HEAD'>");
            fs.WriteLine(" <Data ss:Type=\"String\">CKD PART FOR KAWASAKI MOTORCYCLES</Data>");
            fs.WriteLine(" </Cell>");
            fs.WriteLine(" </Row>");
            fs.WriteLine(" <Row ss:Index='3'>");
            fs.WriteLine(" <Cell ss:Index='1' ss:MergeAcross='9'  ss:StyleID='HEAD'>");
            fs.WriteLine(" <Data ss:Type=\"String\">Nah Loh</Data>");
            fs.WriteLine(" </Cell>");
            fs.WriteLine(" </Row>");

            fs.WriteLine(" <Row ss:Index='5'>");
            fs.WriteLine(" <Cell ss:Index='1' ss:StyleID='HeadField'>");
            fs.WriteLine(" <Data ss:Type=\"String\">NO</Data>");
            fs.WriteLine(" </Cell>");
            fs.WriteLine(" <Cell ss:Index='2' ss:StyleID='HeadField'>");
            fs.WriteLine(" <Data ss:Type=\"String\">PART NO</Data>");
            fs.WriteLine(" </Cell>");
            fs.WriteLine(" <Cell ss:Index='3' ss:StyleID='HeadField'>");
            fs.WriteLine(" <Data ss:Type=\"String\">DESCRIPTION</Data>");
            fs.WriteLine(" </Cell>");
            fs.WriteLine(" <Cell ss:Index='4' ss:StyleID='HeadField'>");
            fs.WriteLine(" <Data ss:Type=\"String\">QTY(PCS)</Data>");
            fs.WriteLine(" </Cell>");
            fs.WriteLine(" <Cell ss:Index='5' ss:StyleID='HeadField'>");
            fs.WriteLine(" <Data ss:Type=\"String\">H.S.NO</Data>");
            fs.WriteLine(" </Cell>");
            fs.WriteLine(" <Cell ss:Index='6' ss:StyleID='HeadField'>");
            fs.WriteLine(" <Data ss:Type=\"String\">H.S.10</Data>");
            fs.WriteLine(" </Cell>");
            fs.WriteLine(" <Cell ss:Index='7' ss:StyleID='HeadField'>");
            fs.WriteLine(" <Data ss:Type=\"String\">RVC</Data>");
            fs.WriteLine(" </Cell>");

            fs.WriteLine(" <Cell ss:Index='8' ss:StyleID='HeadField'>");
            fs.WriteLine(" <Data ss:Type=\"String\">GROSS WEIGHT</Data>");
            fs.WriteLine(" </Cell>");
            fs.WriteLine(" <Cell ss:Index='9' ss:StyleID='HeadField'>");
            fs.WriteLine(" <Data ss:Type=\"String\">NET WEIGHT</Data>");
            fs.WriteLine(" </Cell>");
            fs.WriteLine(" <Cell ss:Index='10' ss:StyleID='HeadField'>");
            fs.WriteLine(" <Data ss:Type=\"String\">FOB</Data>");
            fs.WriteLine(" </Cell>");
            fs.WriteLine(" </Row>");

            fs.WriteLine(" </Table>");
            fs.WriteLine("</Worksheet>");
            fs.WriteLine("</Workbook>");
            fs.Close();

            Response.Clear();
            Response.ContentType = "application/vnd.excel";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name + "");
            Response.TransmitFile(Server.MapPath("~/UploadedExcel/RiwayatCuti.xls"));
            Response.End();
            return View();
        }
    }
}
