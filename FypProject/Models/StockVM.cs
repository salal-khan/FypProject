using FypProject.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypProject.Models
{
    public class StockVM
    {

    }
    public class AddStockVM
    {
        public AddStockVM()
        {
            StockDetail = new List<StockDetailVM>();
        }

        [Required, Display(Name = "Product Quantity")]
        public int ProductQuantity { get; set; }


        [Required, Display(Name = "Product Barcode")]
        public string Barcode { get; set; }

        [Required, Display(Name = "Product")]
        public int ProductId { get; set; }

        private SelectList _ProductLIst { get; set; }
        public SelectList ProductList
        {
            get
            {

                if (_ProductLIst != null) return _ProductLIst;
                return new SelectList(GlobalHelpers.GetProductList(), "ProductId", "ProductName", this.ProductId);

            }
            set { _ProductLIst = value; }
        }


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


        public List<StockDetailVM> StockDetail { get; set; }


    }
    public class StockDetailVM
    {
        
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int ProductQuantity { get; set; }
        public int SubCategoryId { get; set; }
        public double ProductActualPrice { get; set; }
        public double SubTotalAmount { get; set; }
    }

    public class EditStockVM
    {
        public EditStockVM()
        {
            EditStockDetail = new List<EditStockDetailVM>();
        }

        public int StockMasterId { get; set; }

        [Required, Display(Name = "Product Barcode")]
        public string Barcode { get; set; }

        [Required, Display(Name = "Product Quantity")]
        public int ProductQuantity { get; set; }

        [Required, Display(Name = "Product")]
        public int ProductId { get; set; }

        private SelectList _ProductLIst { get; set; }
        public SelectList ProductList
        {
            get
            {

                if (_ProductLIst != null) return _ProductLIst;
                return new SelectList(GlobalHelpers.GetProductList(), "ProductId", "ProductName", this.ProductId);

            }
            set { _ProductLIst = value; }
        }


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



        public List<EditStockDetailVM> EditStockDetail { get; set; }


    }

    public class EditStockDetailVM
    {

        public int StockMasterId { get; set; }
        public int ProductId { get; set; }
       public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductQuantity { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public double ProductActualPrice { get; set; }
        public double SubTotalAmount { get; set; }
        
    }
    


    public class StockMasterIndexVM
    {
       
        public int StockMasterId { get; set; }
        public int ProductTotalQuantity { get; set; }
        public double ProductTotalAmount { get; set; }
        public DateTime Datetime { get; set; }
      
    }
    public class StockDetailIndexVM
    {
         
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public int ProductQuantity { get; set; }
        public double ProductActualPrice { get; set; }
        public double ProductSubTotalAmount { get; set; }


    }

    public class GetDetailBarcodeVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public double ProductActualPrice { get; set; }
        public double ProductSellingPrice { get; set; }


    }

    public class GetDetailProductIdVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public double ProductActualPrice { get; set; }
        public double ProductSellingPrice { get; set; }


    }


}