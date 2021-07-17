using FypProject.Extensions;
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
    public class GeneralController : Controller
    {

        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["fyp"].ToString());
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataTable dt;


        public ActionResult GetActualPriceByProductId(string ProductId)
        {

            if (string.IsNullOrEmpty(ProductId))
                return Json(new { status = 0, message = "Product Id cannot be empty or null." }, JsonRequestBehavior.AllowGet);

            con.Open();
            cmd = new SqlCommand($@"select ProductActualPrice from Product where ProductId = '{ProductId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                var actualPrice = Convert.ToDouble(dt.Rows[0]["ProductActualPrice"]);
                return Json(new { status = 1, message = "Success", actualPrice = actualPrice },
                   JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = 0, message = "No Product Actual Price Is Available this Book-Id (Id is invalid)." }, JsonRequestBehavior.AllowGet);



        }


        public ActionResult GetSubCategoryByCategoryId(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
                return Json(new { status = 0, message = "Category Id cannot be empty or null." }, JsonRequestBehavior.AllowGet);

            var subcategoryList = GlobalHelpers.GetSubCategoryListByCategoryId(Convert.ToInt32(categoryId));
            return Json(new { status = 1, message = "Success", subcategoryList = subcategoryList },
                  JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetProductBySubCategoryId(string subcategoryId)
        {
            if (string.IsNullOrEmpty(subcategoryId))
                return Json(new { status = 0, message = "Sub Category Id cannot be empty or null." }, JsonRequestBehavior.AllowGet);

            var productList = GlobalHelpers.GetProductListBySubCategoryId(Convert.ToInt32(subcategoryId));
            return Json(new { status = 1, message = "Success", productList = productList },
                  JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetDetailByBarcode(string barcode)
        {
            if (string.IsNullOrEmpty(barcode))
                return Json(new { status = 0, message = "Barcode cannot be empty or null." }, JsonRequestBehavior.AllowGet);

            var productdetail = GlobalHelpers.GetDetailByBarcode(barcode);
            if (productdetail.ProductId > 0)
            {
                return Json(new { status = 1, message = "Success", productdetail = productdetail },
                      JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = 0, message = "Error", productdetail = productdetail },
                 JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetDetailByProduct(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                return Json(new { status = 0, message = "Product cannot be empty or null." }, JsonRequestBehavior.AllowGet);

            var productdetail = GlobalHelpers.GetDetailByProductId(Convert.ToInt32(productId));
            if (productdetail.ProductId > 0)
            {
                return Json(new { status = 1, message = "Success", productdetail = productdetail },
                      JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = 0, message = "Error", productdetail = productdetail },
                 JsonRequestBehavior.AllowGet);
            }

        }



        public ActionResult GetQuantityByBarcodeAvailableCount(string barcode)
        {
            if (string.IsNullOrEmpty(barcode))
                return Json(new { status = 0, message = "Barcode cannot be empty or null." }, JsonRequestBehavior.AllowGet);
            var Array = GlobalHelpers.GetAvailableCountByBarcode(barcode);
            double Quantity = Convert.ToDouble(Array[0]);
            int ProductId = Convert.ToInt32(Array[1]);


            if (Quantity <= 0)
            {
                return Json(new { status = 0, message = "This Product Not Available in Stock.." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 1, Quantity = Quantity, ProductId = ProductId }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetQuantityByProductAvailableCount(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                return Json(new { status = 0, message = "Prodduct cannot be empty or null." }, JsonRequestBehavior.AllowGet);
            var Quantity = GlobalHelpers.GetAvailableCountByProductId(productId);
            if (Quantity <= 0)
            {
                return Json(new { status = 0, message = "This Product Not Available in Stock.." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 1, Quantity = Quantity }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GeTPredictQuantity(string barcode)
        {
            cmd = new SqlCommand($@"select * from ApplicationSetting where name = '{constant.ApplicationSetting.Prediction}' and Status = 'Y' ", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {


                int thresholdValue = Convert.ToInt32(dt.Rows[0]["ThresholdValue"]);
                if (string.IsNullOrEmpty(barcode))
                    return Json(new { status = 0, message = "Barcode cannot be empty or null." }, JsonRequestBehavior.AllowGet);
                var Quantity = GlobalHelpers.GetPredictQuantity(barcode, thresholdValue);
                if (Quantity <= 0)
                {
                    return Json(new { status = 0, message = "No Record Found To Predict Quantity.." }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = 1, message = $@"Your Predicted Quantity is : '{Quantity}' Last '{thresholdValue}' Months Record." }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { status = 0, message = "Prediction Setting is not Active" }, JsonRequestBehavior.AllowGet);
            }




              
        }


     


    }
}