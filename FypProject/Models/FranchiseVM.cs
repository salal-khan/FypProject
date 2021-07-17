using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FypProject.Models
{
    public class FranchiseVM
    {
        public FranchiseVM()
        {
            FranchiseList = new List<FranchiseList>();
        }

        public int FranchiseId { get; set; }

        [Required]
        public string FranchiseName { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }

        public List<FranchiseList> FranchiseList { get; set; }

    }

    public class FranchiseList
    {
        public int FranchiseId { get; set; }

        [Required]
        public string FranchiseName { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string RecordBy { get; set; }
        public DateTime RecordAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        
    }




}