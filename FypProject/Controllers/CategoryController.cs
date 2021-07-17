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
    public class CategoryController : Controller
    {
        private CategoryRepository categoryRepository;
        public CategoryController()
        {
            categoryRepository = new CategoryRepository();
        }

        public ActionResult Index()
        {
            List<IndexCategoryVM> categoryList = new List<IndexCategoryVM>();
            try
            {
                categoryList = categoryRepository.CategoryList();

            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }
            return View(categoryList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateCategoryVM model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var result = categoryRepository.Create(model);
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
            var model = new EditCategoryVM();
            try
            {
                model = categoryRepository.GetEditDetail(id);
            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditCategoryVM model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var result = categoryRepository.PostEditDetail(model);
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