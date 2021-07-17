using FypProject.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypProject.Models
{
    public class SalesVM
    {
    }
    public class AddSalesVM
    {
        public AddSalesVM()
        {
            salesDetailList = new List<AddSalesDetailVM>();
        }

        [Display(Name = "Product")]
        public string ProductBarcode { get; set; }

        private SelectList _ProductLIst { get; set; }
        public SelectList ProductList
        {
            get
            {

                if (_ProductLIst != null) return _ProductLIst;
                return new SelectList(GlobalHelpers.GetProductListValueBarcode(), "ProductBarcode", "ProductName", this.ProductBarcode);

            }
            set { _ProductLIst = value; }
        }

        public string InvoiceNumber { get; set; }
        public double NoQuantity { get; set; }
        public double TotalAmount { get; set; }
        public double RecivedAmount { get; set; }
        public List<AddSalesDetailVM> salesDetailList { get; set; }

    }
    public class AddSalesDetailVM
    {
        public int SalesMasterId { get; set; }
        public int ProductId { get; set; }
        public double Quantity { get; set; }
        public double  SellingPrice { get; set; }
        public double SellingSubTotal { get; set; }
        public double ActualPrice { get; set; }
        public double ActualSubTotal { get; set; }
    }


    public class EditSalesVM
    {
        public EditSalesVM()
        {
            salesDetailList = new List<EditSalesDetailVM>();
        }
        public int SalesMasterId { get; set; }
        public string InvoiceNumber { get; set; }
        public double NoQuantity { get; set; }
        public double TotalAmount { get; set; }
        public double RecivedAmount { get; set; }
        [Display(Name = "Product")]
        public string ProductBarcode { get; set; }

        private SelectList _ProductLIst { get; set; }
        public SelectList ProductList
        {
            get
            {

                if (_ProductLIst != null) return _ProductLIst;
                return new SelectList(GlobalHelpers.GetProductListValueBarcode(), "ProductBarcode", "ProductName", this.ProductBarcode);

            }
            set { _ProductLIst = value; }
        }
        public List<EditSalesDetailVM> salesDetailList { get; set; }

    }
    public class EditSalesDetailVM
    {
        
        public int SalesMasterId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public double SellingPrice { get; set; }
        public double SellingSubTotal { get; set; }
        
    }

    public class SalesMasterIdnexVM
    {
        public int SalesMasterId { get; set; }
        public string InvoiceNumber { get; set; }
        public double NoOfQuantity { get; set; }
        public double TotalAmount { get; set; }    
        public string UserName { get; set; }
        public DateTime Datetime { get; set; }
    }

    public class SalesDetailIndexVM
    {
        public int SalesMasterId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double SellingPrice { get; set; }
        public double SellingSubTotal { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
    }


}