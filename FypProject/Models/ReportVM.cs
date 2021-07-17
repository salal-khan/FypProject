using FypProject.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypProject.Models
{
    public class ReportVM
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


        [Display(Name = "Franchise")]
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


    }

    public class InvoiveVM
    {


    }
}