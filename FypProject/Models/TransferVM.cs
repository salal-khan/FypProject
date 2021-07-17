using FypProject.Extensions;
using FypProject.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypProject.Models
{
    public class TransferVM
    {
    }
    public class CreateTransferFormVM
    { 
        [Required, Display(Name ="Franchise")]
        public int FranchiseId { get; set; }

        private SelectList _FranchiseList { get; set; }
        public SelectList FranchiseList
        {
            get
            {
                if (_FranchiseList != null) return _FranchiseList;
                return new SelectList(GlobalHelpers.GetFranchiseList(), "FranchiseId", "FranchiseName", this.FranchiseId);
            }
            set { _FranchiseList = value; }
        }


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
                return new SelectList(GlobalHelpers.GetStockProductList(), "ProductId", "ProductName", this.ProductId);

            }
            set { _ProductLIst = value; }
        }

  


    }


    public class TransferDetailVM
    {

        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int ProductQuantity { get; set; }
        public int SubCategoryId { get; set; }
        public double ProductActualPrice { get; set; }
        public double SubTotalAmount { get; set; }
        public int FranchiseId { get; set; }
    }

    public class EditPostTransferDetailVM
    {
        public int TransferId { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int ProductQuantity { get; set; }
        public int SubCategoryId { get; set; }
        public double ProductActualPrice { get; set; }
        public double SubTotalAmount { get; set; }
        public int FranchiseId { get; set; }
    }

    public class EditTransferVM
    {
        public EditTransferVM()
        {
            transferDetailList = new List<EditTranferDetailVM>();
        }
        public int TransferId { get; set; }
        [Required, Display(Name = "Product")]
        public int ProductId { get; set; }

        private SelectList _ProductLIst { get; set; }
        public SelectList ProductList
        {
            get
            {

                if (_ProductLIst != null) return _ProductLIst;
                return new SelectList(GlobalHelpers.GetStockProductList(), "ProductId", "ProductName", this.ProductId);

            }
            set { _ProductLIst = value; }
        }

        [Required, Display(Name = "Franchise")]
        public int FranchiseId { get; set; }

        private SelectList _FranchiseList { get; set; }
        public SelectList FranchiseList
        {
            get
            {
                if (_FranchiseList != null) return _FranchiseList;
                return new SelectList(GlobalHelpers.GetFranchiseList(), "FranchiseId", "FranchiseName", this.FranchiseId);
            }
            set { _FranchiseList = value; }
        }


        public double ProductQuantity { get; set; }
        public List<EditTranferDetailVM> transferDetailList { get; set; }

    }

    public class EditTranferDetailVM
    {
        public int TransferMasterId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public double ProductQuantity { get; set; }
        public int SubCategoryId { get; set; }
        public double ProductActualPrice { get; set; }
        public double SubTotalAmount { get; set; }
        public int FranchiseId { get; set; }
    }


    public class TransferMasterIndexVM
    {

        public int TransferMasterId { get; set; }
        public string Franchise { get; set; }
        public double TotalProductQuantity { get; set; }
        public DateTime Datetime { get; set; }

    }

    public class TransferDetailIndexVM
    {

        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public int ProductQuantity { get; set; }
        


    }



}