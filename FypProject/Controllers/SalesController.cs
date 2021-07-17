using FypProject.Extensions;
using FypProject.Models;
using FypProject.Repositories;
using FypProject.Views.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypProject.Controllers
{
    [UserAuthenticationFilter]
    public class SalesController : Controller
    {
        private SaleRepository salesRepository;
        public SalesController()
        {
            salesRepository = new SaleRepository();
        }



        public ActionResult Index()
        {
            List<SalesMasterIdnexVM> salesList = new List<SalesMasterIdnexVM>();
            try
            {
                salesList = salesRepository.SalesList();

            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }
            return View(salesList);
        }


        [HttpGet]
        public ActionResult PointOfSale()
        {   
            AddSalesVM model = new AddSalesVM();
            return View(model);
        }

        [HttpPost]
        public ActionResult PointOfSale(List<AddSalesDetailVM> salesDetails, string totalQty, string grandTotal, string cashRec)
        {

            try
            {
                double _grandTotal = 0;
                double.TryParse(grandTotal, out _grandTotal);

                double _totalQty = 0;
                double.TryParse(totalQty, out _totalQty);

                double _cashRec = 0;
                double.TryParse(cashRec, out _cashRec);


                if (_grandTotal == 0 || _grandTotal < 0)
                {
                    return Json(new { message = "Grand Total 0 Kindly Check Product Summary Table", status = 0 }, JsonRequestBehavior.AllowGet);
                }

                if (_totalQty == 0 || _totalQty < 0)
                {
                    return Json(new { message = "Total Quantity 0 Kindly Check Product Summary Table", status = 0 }, JsonRequestBehavior.AllowGet);
                }

                if (_cashRec == 0 || _cashRec < 0)
                {
                    return Json(new { message = "Cash Received 0 Kindly Fill Cash Receive", status = 0 }, JsonRequestBehavior.AllowGet);
                }


                if (salesDetails.Count == 0 || salesDetails.Count < 0)
                {
                    return Json(new { message = "Product Details Error Kindly Check Product Product Table", status = 0 }, JsonRequestBehavior.AllowGet);
                }




                var model = new AddSalesVM();
                model.InvoiceNumber = salesRepository.GetInvoiceNumber();
                model.NoQuantity = _totalQty;
                model.TotalAmount = _grandTotal;
                model.RecivedAmount = _cashRec;
                model.salesDetailList = salesDetails;



                var result = salesRepository.AddSale(model);
                return Json(new { message = $@"Sale Sucessfully..", saleMasterId = result, status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { message = $@"Something went wrong .. '{GlobalHelpers.GetLastInnerMessage(ex)}'", status = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int id)
        {
            var model = new EditSalesVM();
            try
            {
                model = salesRepository.GetEditDetail(id);
            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(List<EditSalesDetailVM> salesDetails, string totalQty, string grandTotal, string cashRec, int SalesMasterId)
        {

            try
            {
                double _grandTotal = 0;
                double.TryParse(grandTotal, out _grandTotal);

                double _totalQty = 0;
                double.TryParse(totalQty, out _totalQty);

                double _cashRec = 0;
                double.TryParse(cashRec, out _cashRec);


                if (_grandTotal == 0 || _grandTotal < 0)
                {
                    return Json(new { message = "Grand Total 0 Kindly Check Product Summary Table", status = 0 }, JsonRequestBehavior.AllowGet);
                }

                if (_totalQty == 0 || _totalQty < 0)
                {
                    return Json(new { message = "Total Quantity 0 Kindly Check Product Summary Table", status = 0 }, JsonRequestBehavior.AllowGet);
                }

                if (_cashRec == 0 || _cashRec < 0)
                {
                    return Json(new { message = "Cash Received 0 Kindly Fill Cash Receive", status = 0 }, JsonRequestBehavior.AllowGet);
                }


                if (salesDetails.Count == 0 || salesDetails.Count < 0)
                {
                    return Json(new { message = "Product Details Error Kindly Check Product Product Table", status = 0 }, JsonRequestBehavior.AllowGet);
                }




                var model = new EditSalesVM();
                model.NoQuantity = _totalQty;
                model.TotalAmount = _grandTotal;
                model.RecivedAmount = _cashRec;
                model.SalesMasterId = SalesMasterId;
                model.salesDetailList = salesDetails;



                var result = salesRepository.PostEditDetail(model);
                return Json(new { message = $@"'{result}'", status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { message = $@"Something went wrong .. '{GlobalHelpers.GetLastInnerMessage(ex)}'", status = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult LoadSalesDetail(int salesMasterId)
        {
            List<SalesDetailIndexVM> detail = new List<SalesDetailIndexVM>();
            detail = GlobalHelpers.GetSalesDetail(salesMasterId);
            return this.PartialView("_LoadSalesDetail", detail);
        }


        
        [HttpGet]
        public ActionResult PrintInvoice(int saleMasterId)
        {
            try
            {
                if (saleMasterId <= 0)
                {
                    this.AddNotification("Master Id Cannot be Empty...", NotificationType.WARNING);
                    return RedirectToAction("Index");
                }

                if (saleMasterId <= 0)
                {
                    this.AddNotification("Master Id Cannot be zero...", NotificationType.WARNING);
                    return RedirectToAction("Index");
                }


                var masterDetail = GlobalHelpers.SalesMasterIdExistInDb(saleMasterId);
                if (!masterDetail)
                {
                    this.AddNotification("Master Record Not Found Provided Id..", NotificationType.WARNING);
                    return RedirectToAction("Index");
                }




                if (ModelState.IsValid)
                {
                    DataSet ds = new DataSet("ReportDS");
                    DataTable dt = GlobalHelpers.GetInvoiceData(saleMasterId);
                    dt.TableName = "InvoiceReport";
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        using (Invoice invoice = new Invoice())
                        {
                            invoice.FileName = Server.MapPath("~/Views/Report/Invoice.rpt");
                            invoice.Load();
                            ds.Tables.Add(dt);
                            invoice.SetDataSource(ds);
                            //    this.AddNotification("Print Sucessfull", NotificationType.SUCCESS);
                            return (new ReportController()).DownloadInvoicePDF(invoice, "Invoice");

                        }
                    }
                    else
                    {


                        this.AddNotification("No Record Found", NotificationType.INFO);
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    this.AddNotification("Model validation Error", NotificationType.INFO);
                    return RedirectToAction("Index");
                }




            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
                return RedirectToAction("Index");
            }
        }

        
        public void Print(int saleMasterId)
        {
            DataSet ds = new DataSet("ReportDS");
            DataTable dt = GlobalHelpers.GetInvoiceData(saleMasterId);

            using (Invoice invoice = new Invoice())
            {
                invoice.FileName = Server.MapPath("~/Views/Report/Invoice.rpt");
                invoice.Load();
                ds.Tables.Add(dt);
                invoice.SetDataSource(ds);
                //      invoice.PrintToPrinter(1, true, 1, 1);
                //   System.Threading.Thread.Sleep(3000);
            }

        }


    }
}
