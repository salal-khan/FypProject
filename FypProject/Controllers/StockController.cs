using FypProject.Extensions;
using FypProject.Models;
using FypProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypProject.Controllers
{
    [UserAuthenticationFilter]
    public class StockController : Controller
    {
        private StockRepository StockRepository;
        public StockController()
        {
            StockRepository = new StockRepository();
        }
        public ActionResult Index()
        {
            List<StockMasterIndexVM> stockList = new List<StockMasterIndexVM>();
            try
            {
                stockList = StockRepository.StockList();

            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }
            return View(stockList);
        }

        [HttpGet]
        public ActionResult AddStock()
        {
            AddStockVM model = new AddStockVM();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddStock(List<StockDetailVM> stockDetail)
        {
            try
            {
                var result = StockRepository.AddStock(stockDetail);
                return Json(new { message = $@"'{result}'", status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { message = $@"Something went wrong .. '{GlobalHelpers.GetLastInnerMessage(ex)}'", status = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int id)
        {
            var model = new EditStockVM();
            var stockdetailList = new List<EditStockDetailVM>();
            try
            {
                stockdetailList = StockRepository.GetEditDetail(id);
            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }
            model.EditStockDetail = stockdetailList;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(List<EditStockDetailVM> stockDetail)
        {
            try
            {

                var result = StockRepository.PostEditDetail(stockDetail);
                return Json(new { message = $@"'{result}'", status = 1 }, JsonRequestBehavior.AllowGet);



            }
            catch (Exception ex)
            {
                return Json(new { message = $@"Something went wrong .. '{GlobalHelpers.GetLastInnerMessage(ex)}'", status = 0 }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult LoadDetail(int stockMasterId)
        {
            List<StockDetailIndexVM> detail = new List<StockDetailIndexVM>();
            detail = GlobalHelpers.GetStockDetail(stockMasterId);
            return this.PartialView("_LoadDetail", detail);
        }
    }
}