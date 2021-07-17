using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypProject.DB
{
    public class Books
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string PublisherName { get; set; }
        public string WriterName { get; set; }
        public string Edition { get; set; }
        public double BookActualPrice { get; set; }
        public double BookSellingPrice { get; set; }
        public string BookBarCode { get; set; }
        public string RecordBy { get; set; }
        public DateTime? RecordAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}