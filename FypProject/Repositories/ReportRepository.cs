using FypProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FypProject.Repositories
{
    public class ReportRepository
    {

        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["fyp"].ToString());
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataTable dt;



        public DataTable GetInventoryReportDT(ReportVM model)
        {
            dt = new DataTable();
            string condition = string.Empty;



 


            string FromDate = model.FromDate.ToString("yyyy-MM-dd");
            string ToDate = model.ToDate.ToString("yyyy-MM-dd");

            string query = $@"select sm.StockMasterId, sm.DateTime, sd.StockDetailId, sm.ProductTotalQuantity, sm.ProductTotalAmount 
            , sd.ProductQuantity, sd.ProductActualPrice, sd.SubTotalAmount, pro.ProductName, pro.Description, suCat.SubCategoryName
            from dbo.StockMaster sm
            inner join StockDetail sd on sd.StockMasterId = sm.StockMasterId
            inner join Product pro on pro.ProductId = sd.ProductId
            inner join SubCategory suCat on suCat.SubCategoryId = pro.SubCategoryId
            where CONVERT(varchar(10), sm.DateTime,121) between '{FromDate}' and '{ToDate}' ";


            cmd = new SqlCommand(query, con);
            con.Open();
            sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }

            return null;
        }



        public DataTable GetSalesReportDT(ReportVM model)
        {
            dt = new DataTable();
            string condition = string.Empty;
            if (model.FranchiseId > 0)
                condition += $@"and sm.FranchiseId = '{model.FranchiseId}'";


            string FromDate = model.FromDate.ToString("yyyy-MM-dd");
            string ToDate = model.ToDate.ToString("yyyy-MM-dd");

            string query = $@"
select   sm.SalesMasterId, convert(varchar(10),sm.Datetime,121) as SaleDate , pro.ProductName ,sm.TotalAmount,
sum(convert(float,sd.SellingSubTotal)) over (partition by convert(varchar(10),sm.datetime,121),sm.SalesMasterId) as DatewiseAmount,
sd.Quantity,sd.SellingPrice,sd.SellingSubTotal,sm.InvoiceNumber,f.FranchiseName
from SalesMaster sm
                    inner join SalesDetail sd on sd.SalesMasterId = sm.SalesMasterId
                    inner join Product pro on pro.ProductId = sd.ProductId
					inner join Franchise f on f.FranchiseId = sm.FranchiseId


                    where CONVERT(varchar(10), sm.Datetime,121) between '{FromDate}' and '{ToDate}'  {condition} order by convert(varchar(10),sm.datetime,121) desc ";


            cmd = new SqlCommand(query, con);
            con.Open();
            sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }

            return null;
        }









    }
    
}