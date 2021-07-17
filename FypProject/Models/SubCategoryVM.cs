using FypProject.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypProject.Models
{
    public class SubCategoryVM
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
    }
    public class CreateSubCategoryVM
    {
        [Required, Display(Name = "Sub-Category")]
        public string SubCategoryName { get; set; }

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
    }
    public class EditSubCategoryVM
    {
        public int SubCategoryId { get; set; }

        [Required, Display(Name = "Sub-Category")]
        public string SubCategoryName { get; set; }

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

    }
    public class IndexSubCategoryVM
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string CategoryName { get; set; }
    }
}