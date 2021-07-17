using FypProject.Extensions;
using FypProject.Models;
using FypProject.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypProject.Controllers
{
    [UserAuthenticationFilter]
    public class FranchiseController : Controller
    {
        private FranchiseRepository franchiseRepository;
        public FranchiseController()
        {
            franchiseRepository = new FranchiseRepository();
        }


        // GET: Franchise
        public ActionResult Index()
        {
            try
            {
                var IndexList = franchiseRepository.GetIndexList();

                FranchiseVM model = new FranchiseVM();
                model.FranchiseList = IndexList;


                return View(model);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        // GET: Franchise/Create
        public ActionResult Create()
        {
            return View(new FranchiseList());
        }

        // POST: Franchise/Create
        [HttpPost]
        public ActionResult Create(FranchiseList model)
        {
            try
            {

                var result = franchiseRepository.Create(model);

                if (result == "1")
                {
                    this.AddNotification("Record Save Successfully", NotificationType.SUCCESS);
                }
                else if (result == "0")
                {
                    this.AddNotification("No new record insert", NotificationType.INFO);
                }
                else
                {
                    this.AddNotification("There is an error to save the record", NotificationType.ERROR);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.AddNotification("There is an error to save the record" + ex.Message, NotificationType.ERROR);
                return View();
            }
        }

        // GET: Franchise/Edit/5
        public ActionResult Edit(int id)
        {

            var GetObj = franchiseRepository.GetEdit(id);
            return View(GetObj);
        }

        // POST: Franchise/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FranchiseList model)
        {
            try
            {

                var result = franchiseRepository.Edit(id, model);

                if (result == "1")
                {
                    this.AddNotification("Record Update Successfully", NotificationType.SUCCESS);
                }
                else if (result == "0")
                {
                    this.AddNotification("No new record Updated", NotificationType.INFO);
                }
                else
                {
                    this.AddNotification("There is an error to update the record", NotificationType.ERROR);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var result = franchiseRepository.Delete(id);
                if(result == "1")
                {
                    return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
                }
                else if(result == "0")
                {
                    return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 2}, JsonRequestBehavior.AllowGet);
                }
 
            }
            catch (Exception)
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}
