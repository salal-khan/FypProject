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
    public class SubCategoryController : Controller
    {
        private SubCategoryRespository subcategoryRepository;
        public SubCategoryController()
        {
            subcategoryRepository = new SubCategoryRespository();
        }

        public ActionResult Index()
        {
            List<IndexSubCategoryVM> subcategoryList = new List<IndexSubCategoryVM>();
            try
            {
                subcategoryList = subcategoryRepository.SubCategoryList();

            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }
            return View(subcategoryList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreateSubCategoryVM());
        }

        [HttpPost]
        public ActionResult Create(CreateSubCategoryVM model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var result = subcategoryRepository.Create(model);
                    this.AddNotification($@"{result}", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Model Validation Error..", NotificationType.ERROR);
                }
            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new EditSubCategoryVM();
            try
            {
                model = subcategoryRepository.GetEditDetail(id);
            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditSubCategoryVM model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var result = subcategoryRepository.PostEditDetail(model);
                    this.AddNotification($@"{result}", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Model Validation Error..", NotificationType.ERROR);
                }
            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }
            return View(model);
        }
    }
}