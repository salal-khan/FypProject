using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypProject.Models
{
    public class DashBoardVM
    {

        public int TotalUsers { get; set; }
        public int TotalFranchies { get; set; }
        

        public LineChartMasterVM LineChartMasterVM { get; set; }
        public BarChartMasterVM BarChartMasterVM { get; set; }


    }



    public class LineChartMasterVM
    {
        public LineChartMasterVM()
        {
            lables = new List<string>();
            datasets = new List<LineChartDataSetsVM>();
        }
        
        public List<string> lables { get; set; }
        public List<LineChartDataSetsVM> datasets { get; set; }

    }

    public class LineChartDataSetsVM
    {
        public string label { get; set; }
        public List<int> data { get; set; }
        public double borderWidth { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string pointBackgroundColor { get; set; }
        public int pointRadius { get; set; }
    }



    public class BarChartMasterVM
    {
        public BarChartMasterVM()
        {
            labels = new List<string>();
            datasets = new List<BarChartDataSetVM>();
        }

        public List<string> labels { get; set; }
        public List<BarChartDataSetVM> datasets { get; set; }

    }
    public class BarChartDataSetVM
    {
        public BarChartDataSetVM()
        {
            data = new List<int>();
        }

        public List<int> data { get; set; }
        public string label { get; set; }
        public int borderWidth { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string pointBackgroundColor { get; set; }
        public int pointRadius { get; set; }
        public int data_ { get; set; }

    }

    public class BarCharQuery
    {
        public int data { get; set; }
        public string label { get; set; }
    }

}