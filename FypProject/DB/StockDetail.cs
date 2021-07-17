using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypProject.DB
{
    public class StockDetail
    {
        public int StockDetailId { get; set; }
        public int StockMasterId { get; set; }
        public int BookId { get; set; }
        public int BookQuantity { get; set; }
        public double BookActualPrice { get; set; }
        public double SubTotalAmount { get; set; }
        public string RecordBy { get; set; }
        public DateTime RecordAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }

    }
}