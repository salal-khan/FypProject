using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypProject.DB
{
    public class Franchise
    {
        public int FranchiseId { get; set; }
        public string FranchiseName { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string RecordBy { get; set; }
        public DateTime RecordAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }


    }
}