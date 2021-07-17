using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypProject.DB
{
    public class StockMaster
    {
        public int StockMasterId { get; set; }
        public int BookTotalQuantity { get; set; }
        public int BookTotalAmount { get; set; }
        public DateTime DateTime { get; set; }
        public string RecordBy { get; set; }
        public DateTime RecordAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }

    }
}