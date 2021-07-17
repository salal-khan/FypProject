using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FypProject.Models
{
    public class CategoryVM
    {
        public int CategoryId { get; set; }

 
        public string CategoryName { get; set; }

    }
    public class CreateCategoryVM
    {
        public string CategoryName { get; set; }

    }
    public class EditCategoryVM
    {
        public int CategoryId { get; set; }

        [Required, Display(Name = "Category")]
        public string CategoryName { get; set; }
    }
    public class IndexCategoryVM
    {
        public int CategoryId { get; set; }
        [Required, Display(Name = "Category")]
        public string CategoryName { get; set; }
    }
}