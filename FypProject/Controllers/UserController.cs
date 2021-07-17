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
    public class UserController : Controller
    {
        private UserRepository userRepository;
        public UserController()
        {
            userRepository = new UserRepository();
        }
        // GET: User
        public ActionResult Index()
        {
            List<IndexUserVM> userList = new List<IndexUserVM>();
            try
            {
                userList = userRepository.UserList();

            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }
            return View(userList);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreateUserVM());
        }

        [HttpPost]
        public ActionResult Create(CreateUserVM model)
        {
            try
            {
                var roleName = GlobalHelpers.GetRoleById(model.RoleId);
                if (roleName == constant.Roles.Admin)
                {
                    ModelState.Remove("FranchiseId");
                    model.FranchiseId = 0;

                }
                if (ModelState.IsValid)
                {
                    var result = userRepository.Create(model);
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


        public ActionResult DoesEmailExist(string email, string oldEmail)
        {
            if (email == oldEmail)
                return Json(true, JsonRequestBehavior.AllowGet);

            var userExist = userRepository.DostEmailExist(email);

            return Json(userExist == false);

        }

        public ActionResult DoesUserNameExist(string UserName, string OldUserName)
        {
            if (OldUserName == UserName)
                return Json(true, JsonRequestBehavior.AllowGet);

            var userExist = userRepository.DoesUserNameExist(UserName);

            return Json(userExist == false);

        }


        [HttpGet]
        public ActionResult Detail(int id)
        {
            var model = new DetailUserVM();
            try
            {
                model = userRepository.UserDetail(id);
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
            var model = new EditUserVM();
            try
            {
                model = userRepository.GetEditDetail(id);
            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(EditUserVM model)
        {
            var roleName = GlobalHelpers.GetRoleById(model.RoleId);
            if (roleName == constant.Roles.Admin)
            {
                ModelState.Remove("FranchiseId");
                model.FranchiseId = 0;

            }

            try
            {
                model = userRepository.PostEditDetail(model);
                this.AddNotification($@"Record Updated Sucessfull...", NotificationType.SUCCESS);
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }

            return View(model);
        }


        public ActionResult Delete(int id)
        {
            try
            {
                var result = userRepository.DeleteUser(id);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);

            }

        }



    }
}