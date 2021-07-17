using CrystalDecisions.CrystalReports.Engine;
using FypProject.Extensions;
using FypProject.Models;
using FypProject.Repositories;
using FypProject.Views.Report;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypProject.Controllers
{
    public class ReportController : Controller
    {
        private ReportRepository reportRepository;
        public ReportController()
        {
            reportRepository = new ReportRepository();
        }


        // GET: Report
        public ActionResult Index()
        {

            SelectPdf.GlobalProperties.HtmlEngineFullPath = HttpContext.ApplicationInstance.Server.MapPath("~/App_Data/Select.Html.dep");
            try
            {
                string pdf_page_size = "A4";
                PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                    pdf_page_size, true);

                string pdf_orientation = "Portrait";
                PdfPageOrientation pdfOrientation =
                    (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                        pdf_orientation, true);

                int webPageWidth = 1600;

                int webPageHeight = 0;

                // instantiate a html to pdf converter object
                HtmlToPdf converter = new HtmlToPdf();

                // set converter options
                converter.Options.PdfPageSize = pageSize;
                converter.Options.PdfPageOrientation = pdfOrientation;
                converter.Options.WebPageWidth = webPageWidth;
                converter.Options.WebPageHeight = webPageHeight;

                // create a new pdf document converting an url
                PdfDocument doc =
                    converter.ConvertUrl(string.Format("https://www.tutorialspoint.com/upsc_ias_exams.htm"));

                // save pdf document
                doc.Save(HttpContext.ApplicationInstance.Response, false, "Sample.pdf");

                // close pdf document
                doc.Close();

                return Content("");
            }
            catch (Exception e)
            {
                return Content(string.Format("{0} {1} ", e.Message, (null != e.InnerException) ? e.InnerException.Message : ""));
            }

        }



        // GET: Report/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult InventoryReport()
        {
            return View(new ReportVM());
        }

        [HttpPost]
        public ActionResult InventoryReport(ReportVM model)
        {

            var ds = new DataSet("RepDS");
            var dt = reportRepository.GetInventoryReportDT(model);

            if (dt != null && dt.Rows.Count > 0)
            {
                dt.TableName = "InventoryDT";
                if (Request["type"].ToLower().Contains("summary"))
                {
                    using (var report = new Views.Report.rptIventoryReport())
                    {
                        report.FileName = Server.MapPath("~/Views/Report/rptIventoryReport.rpt");
                        report.Load();
                        ds.Tables.Add(dt);
                        report.SetDataSource(ds);
                        string fileName = $@"InventoryReport";
                        report.SetParameterValue("FromDate", model.FromDate.ToString("dd-MM-yyyy"));
                        report.SetParameterValue("ToDate", model.ToDate.ToString("dd-MM-yyyy"));


                        Stream stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        return File(stream, "application/pdf");

                    }
                }
            }
            else
            {
                this.AddNotification("There is no data of selected date range", NotificationType.INFO);
                return View(model);
            }

            return View(model);
        }



        public ActionResult Download(ReportDocument rpt, string fileName = "Report")
        {
            string type = "";
            if (Request["type"] != null)
            {
                type = Request["type"];
            }

            if (type.ToLower().Contains("excel"))
            {
                Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                rpt.Dispose();
                return File(stream, "application/vnd.ms-excel", fileName + ".xls");
            }
            else if (type.ToLower().Contains("word"))
            {
                Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
                rpt.Dispose();
                return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName + ".docx");
            }
            else
            {
                Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                rpt.Dispose();
                return File(stream, "application/pdf");
            }


        }
        public ActionResult DownloadInvoicePDF(ReportDocument rpt, string fileName = "Invoice")
        {

            Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            rpt.Dispose();
            return File(stream, "application/pdf");

        }


        [HttpGet]
        public ActionResult SalesReport()
        {
            return View(new ReportVM());
        }

        [HttpPost]
        public ActionResult SalesReport(ReportVM model)
        {
            try
            {
                var ds = new DataSet("RepDS");
                var dt = reportRepository.GetSalesReportDT(model);

                if (dt != null && dt.Rows.Count > 0)
                {
                    dt.TableName = "SalesReportDT";
                    if (Request["type"].ToLower().Contains("summary"))
                    {
                        using (var report = new Views.Report.rptSalesReport())
                        {
                            report.FileName = Server.MapPath("~/Views/Report/rptSalesReport.rpt");
                            report.Load();
                            ds.Tables.Add(dt);
                            report.SetDataSource(ds);
                            string fileName = $@"SalesReportDT";
                            report.SetParameterValue("FromDate", model.FromDate.ToString("dd-MM-yyyy"));
                            report.SetParameterValue("ToDate", model.ToDate.ToString("dd-MM-yyyy"));

                            return Download(report, fileName);

                        }
                    }
                }
                else
                {
                    this.AddNotification("There is no data of selected date range", NotificationType.INFO);
                    return View(model);
                }

            }
            catch (Exception ex)
            {

                throw;
            }


            return View();

        }







    }
}
