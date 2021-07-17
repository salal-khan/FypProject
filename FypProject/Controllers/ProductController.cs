using FypProject.Extensions;
using FypProject.Models;
using FypProject.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypProject.Controllers
{
    [UserAuthenticationFilter]
    public class ProductController : Controller
    {
        private ProductRepository productRepository;
        public ProductController()
        {
            productRepository = new ProductRepository();
        }

        public ActionResult Index()
        {
            List<IndexProductVM> productList = new List<IndexProductVM>();
            try
            {
                productList = productRepository.ProductList();

            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }
            return View(productList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            CreateProductVM model = new CreateProductVM();
           // model.Barcode = productRepository.GenrateBarCode();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateProductVM model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var result = productRepository.Create(model);
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
            var model = new EditProductVM();
            try
            {
                model = productRepository.GetEditDetail(id);
            }
            catch (Exception ex)
            {
                this.AddNotification($@"{GlobalHelpers.GetLastInnerMessage(ex)}", NotificationType.ERROR);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditProductVM model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var result = productRepository.PostEditDetail(model);
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
        public ActionResult GenrateBarCode(int productId)
        {
            BarCodeGenratVM model = new BarCodeGenratVM();
            model.ProductId = productId;
            return this.PartialView("_GenrateBarCode", model);
        }

        public ActionResult BarcodeGenerate(string productId)
        {

            var splitdata = productId.Split('~');
            int _productId = Convert.ToInt32(splitdata[0]);





            var bookCode = productRepository.GetBarCodeByProductId(_productId);
            int qauntity = Convert.ToInt32(splitdata[1]);
            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            using (Bitmap bitMap = new Bitmap(bookCode.Length * 40, 80))
            {
                using (Graphics graphics = Graphics.FromImage(bitMap))
                {
                    Font oFont = new Font("IDAutomationHC39M", 16);
                    PointF point = new PointF(2f, 2f);
                    SolidBrush blackBrush = new SolidBrush(Color.Black);
                    SolidBrush whiteBrush = new SolidBrush(Color.White);
                    graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                    graphics.DrawString(bookCode, oFont, blackBrush, point);
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();

                    Convert.ToBase64String(byteImage);
                    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
                ViewBag.url = imgBarCode.ImageUrl;
                ViewBag.qauntity = qauntity;

            }

            return View();
        }

    }
}