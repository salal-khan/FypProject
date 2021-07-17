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

    public class TransferController : Controller
    {
        private TransferRepository transferRepository;
        public TransferController()
        {
            transferRepository = new TransferRepository();
        }

        public ActionResult TransferForm()
        {
            CreateTransferFormVM model = new CreateTransferFormVM();
            return View(model);
        }

        [HttpPost]
        public ActionResult TransferForm(List<TransferDetailVM> transferDetail)
        {
            try
            {
                var result = transferRepository.TransferForm(transferDetail);
                return Json(new { message = $@"'{result}'", status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { message = $@"Something went wrong .. '{GlobalHelpers.GetLastInnerMessage(ex)}'", status = 0 }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Index()
        {
            List<TransferMasterIndexVM> transferList = new List<TransferMasterIndexVM>();
            try
            {
                transferList = transferRepository.TransferList();

            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }
            return View(transferList);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new EditTransferVM();
            try
            {
                model = transferRepository.GetEditDetail(id);
            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(List<EditPostTransferDetailVM> transferDetail)
        {
            try
            {
                var result = transferRepository.EditPostTransferForm(transferDetail);
                return Json(new { message = $@"'{result}'", status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { message = $@"Something went wrong .. '{GlobalHelpers.GetLastInnerMessage(ex)}'", status = 0 }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult LoadTransferFormDetail(int transferMasterId)
        {
            List<TransferDetailIndexVM> detail = new List<TransferDetailIndexVM>();
            detail = GlobalHelpers.GetTransferDetail(transferMasterId);
            return this.PartialView("_LoadTransferFormDetail", detail);
        }

    }
}
