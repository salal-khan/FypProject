using FypProject.Extensions;
using FypProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypProject.Controllers
{
    
    public class AccountController : Controller
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["fyp"].ToString());
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataTable dt;


        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                con.Open();
                cmd = new SqlCommand($@"select * from Users where UserName = '{model.UserName}' AND Password = '{model.Password}'", con);
                cmd.CommandType = CommandType.Text;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var UserID = dt.Rows[0]["Id"].ToString();
                    Session["Id"] = dt.Rows[0]["Id"].ToString();
                    Session["UserName"] = dt.Rows[0]["UserName"].ToString();
                    Session["FranchiseId"] = dt.Rows[0]["FranchiseId"].ToString();

                    cmd = new SqlCommand($@"select rol.RoleName from UserRole ur inner join Roles rol on rol.RoleId = ur.RoleId where ur.UserId = '{UserID}'", con);
                    cmd.CommandType = CommandType.Text;
                    sda = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    sda.Fill(dt);

                    Session["RoleName"] = dt.Rows[0]["RoleName"].ToString();


                    con.Close();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    con.Close();
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["Id"] = null;
            Session["UserName"] = null;
            return RedirectToAction("Login");
        }
    }
}