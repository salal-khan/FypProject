using FypProject.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypProject.Models
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }

    public class ProductValueBarcodeVM
    {
        public string ProductBarcode { get; set; }
        public string ProductName { get; set; }
    }

    public class CreateProductVM
    {
        [Required, Display(Name = "Product")]
        public string ProductName { get; set; }

        [Required, Display(Name = "Description")]
        public string Description { get; set; }

        [Required, Display(Name = "Actual Price")]
        public double ProductActualPrice { get; set; }

        [Required, Display(Name = "Selling Price")]
        public double ProductSellingPrice { get; set; }

        [Required, Display(Name = "Barcode")]
        public string Barcode { get; set; }

        [Required, Display(Name = "Category")]
        public int CategoryId { get; set; }

        private SelectList _CategoryLIst { get; set; }
        public SelectList CategoryList
        {
            get
            {

                if (_CategoryLIst != null) return _CategoryLIst;
                return new SelectList(GlobalHelpers.GetCategoryList(), "CategoryId", "CategoryName", this.CategoryId);

            }
            set { _CategoryLIst = value; }
        }

        [Required, Display(Name = "Sub-Category")]
        public int SubCategoryId { get; set; }

        private SelectList _SubCategoryLIst { get; set; }
        public SelectList SubCategoryList
        {
            get
            {

                if (_SubCategoryLIst != null) return _SubCategoryLIst;
                return new SelectList(GlobalHelpers.GetSubCategoryList(), "SubCategoryId", "SubCategoryName", this.SubCategoryId);

            }
            set { _SubCategoryLIst = value; }
        }




    }
    public class EditProductVM
    {

        public int ProductId { get; set; }
        [Required, Display(Name = "Product")]
        public string ProductName { get; set; }

        [Required, Display(Name = "Description")]
        public string Description { get; set; }

        [Required, Display(Name = "Actual Price")]
        public double ProductActualPrice { get; set; }

        [Required, Display(Name = "Selling Price")]
        public double ProductSellingPrice { get; set; }

        [Required, Display(Name = "Barcode")]
        public string Barcode { get; set; }

        [Required, Display(Name = "Category")]
        public int CategoryId { get; set; }

        private SelectList _CategoryLIst { get; set; }
        public SelectList CategoryList
        {
            get
            {

                if (_CategoryLIst != null) return _CategoryLIst;
                return new SelectList(GlobalHelpers.GetCategoryList(), "CategoryId", "CategoryName", this.CategoryId);

            }
            set { _CategoryLIst = value; }
        }


        [Required, Display(Name = "Sub-Category")]
        public int SubCategoryId { get; set; }

        private SelectList _SubCategoryLIst { get; set; }
        public SelectList SubCategoryList
        {
            get
            {

                if (_SubCategoryLIst != null) return _SubCategoryLIst;
                return new SelectList(GlobalHelpers.GetSubCategoryList(), "SubCategoryId", "SubCategoryName", this.SubCategoryId);

            }
            set { _SubCategoryLIst = value; }
        }
    }

    public class IndexProductVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double ProductActualPrice { get; set; }
        public double ProductSellingPrice { get; set; }
        public string Barcode { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }

    }


    public class BarCodeGenratVM
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
