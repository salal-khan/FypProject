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
    public class HomeController : Controller
    {
        private DashBoardRepository dashBoardRepository;
        public HomeController()
        {
            dashBoardRepository = new DashBoardRepository();
        }


        public ActionResult Index()
        {
            try
            {
                var UserCount = dashBoardRepository.MainWeigtsUserCount();
                var FranchiseCount = dashBoardRepository.MainWeigtsFranchiseCount();

                DashBoardVM model = new DashBoardVM();

                model.TotalUsers = UserCount.TotalUsers;
                model.TotalFranchies = FranchiseCount.TotalFranchies;

                model.BarChartMasterVM = dashBoardRepository.GetBarChart();

                model.LineChartMasterVM = dashBoardRepository.GetlineChart();



                return View(model);


            }
            catch (Exception ex)
            {

                throw;
            }
            
        }



    












        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}