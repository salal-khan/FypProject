using FypProject.Extensions;
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

    public class StockRepository
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["fyp"].ToString());
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataTable dt;

        public string AddStock(List<StockDetailVM> model)
        {
            con.Open();
            cmd = new SqlCommand($@"insert into StockMaster (ProductTotalQuantity,ProductTotalAmount,DateTime,RecordAt,RecordBy) values
           ('{model.Sum(x => x.ProductQuantity)}','{model.Sum(x => x.ProductActualPrice)}',GetDate(),GetDate(),'{HttpContext.Current.Session["Id"].ToString()}')", con);
            cmd.ExecuteNonQuery();


            cmd = new SqlCommand($@"select top 1 StockMasterId from StockMaster order by RecordAt desc", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            var insertedId = Convert.ToInt32(dt.Rows[0]["StockMasterId"]);


            foreach (var item in model)
            {
                cmd = new SqlCommand($@"insert into StockDetail (StockMasterId,ProductId,ProductQuantity,ProductActualPrice,SubTotalAmount,RecordAt,RecordBy) values
                ('{insertedId}','{item.ProductId}','{item.ProductQuantity}','{item.ProductActualPrice}','{item.SubTotalAmount}',GetDate(),'{HttpContext.Current.Session["Id"].ToString()}')", con);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand($@"select Quantity from AvailableProductCount where ProductId = '{item.ProductId}'", con);
                cmd.CommandType = CommandType.Text;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var newproductCount = item.ProductQuantity + Convert.ToDouble(dt.Rows[0]["Quantity"]);
                    cmd = new SqlCommand($@"update AvailableProductCount set Quantity = '{newproductCount}' where ProductId = '{item.ProductId}'", con);
                    cmd.ExecuteNonQuery();

                }
                else
                {
                    cmd = new SqlCommand($@"insert into AvailableProductCount (ProductId,Quantity,CategoryId,SubCategoryId) values ('{item.ProductId}', '{item.ProductQuantity}','{item.CategoryId}', '{item.SubCategoryId}')", con);
                    cmd.ExecuteNonQuery();
                }


            }
            con.Close();
            return "Stock Added Sucessfully...";
        }
        public List<EditStockDetailVM> GetEditDetail(int StockMasterId)
        {
            con.Open();
            List<EditStockDetailVM> edit = new List<EditStockDetailVM>();
            cmd = new SqlCommand($@"select sd.*,sc.SubCategoryId,sc.SubCategoryName,c.CategoryId,c.CategoryName,p.ProductId,p.ProductName
 from StockDetail sd inner join Product p on p.ProductId = sd.ProductId
inner join SubCategory sc on sc.SubCategoryId = p.SubCategoryId
inner join Category c on c.CategoryId = sc.CategoryId where sd.StockMasterId = '{StockMasterId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                edit = (from DataRow row in dt.Rows

                        select new EditStockDetailVM
                        {
                            StockMasterId = Convert.ToInt32(row["StockMasterId"]),
                            ProductId = Convert.ToInt32(row["ProductId"]),
                            ProductName = row["ProductName"].ToString(),
                            CategoryId = Convert.ToInt32(row["CategoryId"]),
                            CategoryName = row["CategoryName"].ToString(),
                            SubCategoryId = Convert.ToInt32(row["SubCategoryId"]),
                            SubCategoryName = row["CategoryName"].ToString(),
                            ProductActualPrice = Convert.ToDouble(row["ProductActualPrice"]),
                            ProductQuantity = Convert.ToInt32(row["ProductQuantity"]),
                            SubTotalAmount = Convert.ToDouble(row["SubTotalAmount"])
                        }).ToList();
            }
            con.Close();

            return edit;
        }
         
        public string PostEditDetail(List<EditStockDetailVM> model)
        {
            con.Open();

            // check current count available for edit or not..
            //cmd = new SqlCommand($@"select ProductId,ProductQuantity from StockDetail where StockMasterId = '{model.FirstOrDefault().StockMasterId}' ", con);
            //cmd.CommandType = CommandType.Text;
            //sda = new SqlDataAdapter(cmd);
            //dt = new DataTable();
            //sda.Fill(dt);
            //if (dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        cmd = new SqlCommand($@"select Quantity from AvailableProductCount where ProductId = '{Convert.ToInt32(dt.Rows[i]["ProductId"])}'", con);
            //        cmd.CommandType = CommandType.Text;
            //        sda = new SqlDataAdapter(cmd);
            //        DataTable dtt = new DataTable();
            //        sda.Fill(dtt);
            //        if (dtt.Rows.Count > 0)
            //        {
            //            var stockCount = Convert.ToDouble(dt.Rows[i]["ProductQuantity"]);
            //            var availableCount = Convert.ToDouble(dtt.Rows[i]["Quantity"]);
            //            var updateAvailableCount = availableCount - stockCount;
            //            if (updateAvailableCount <= 0)
            //            {
            //                return $@"Availble Count less to deduct previous stock data this product '{GlobalHelpers.GetProductById(Convert.ToInt32(dt.Rows[i]["ProductId"]))}'...";
            //            }
            //            else
            //            {
            //                cmd = new SqlCommand($@"update AvailableProductCount set Quantity = '{updateAvailableCount}' where ProductId = '{Convert.ToInt32(dt.Rows[i]["ProductId"])}'", con);
            //                cmd.ExecuteNonQuery();
            //            }
            //        }
            //    }
            //}






            cmd = new SqlCommand($@"update StockMaster set UpdateAt = GetDate(), UpdateBy = '{HttpContext.Current.Session["Id"].ToString()}', ProductTotalQuantity = '{model.Sum(x => x.ProductQuantity)}', ProductTotalAmount = '{model.Sum(x => x.ProductActualPrice)}'
                            where StockMasterId = '{model.FirstOrDefault().StockMasterId}'", con);
            cmd.ExecuteNonQuery();


            cmd = new SqlCommand($"delete from StockDetail where StockMasterId = '{model.FirstOrDefault().StockMasterId}'", con);
            cmd.ExecuteNonQuery();

            foreach (var item in model)
            {
                cmd = new SqlCommand($@"insert into StockDetail (StockMasterId,ProductId,ProductQuantity,ProductActualPrice,SubTotalAmount,RecordAt,RecordBy) values
                ('{item.StockMasterId}','{item.ProductId}','{item.ProductQuantity}','{item.ProductActualPrice}','{item.SubTotalAmount}',GetDate(),'{HttpContext.Current.Session["Id"].ToString()}')", con);
                cmd.ExecuteNonQuery();
            }

            con.Close();

            return "Stock Updated Sucessfull..";

        }

        public List<StockMasterIndexVM> StockList()
        {
            con.Open();

            List<StockMasterIndexVM> stocklist = new List<StockMasterIndexVM>();
            cmd = new SqlCommand($@"select StockMasterId,ProductTotalQuantity,ProductTotalAmount,Datetime from StockMaster", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                stocklist = (from DataRow row in dt.Rows

                             select new StockMasterIndexVM
                             {
                                 StockMasterId = Convert.ToInt32(row["StockMasterId"]),
                                 ProductTotalAmount = Convert.ToDouble(row["ProductTotalAmount"]),
                                 ProductTotalQuantity = Convert.ToInt32(row["ProductTotalQuantity"]),
                                 Datetime = Convert.ToDateTime(row["Datetime"])
                             }).ToList();
            }
            con.Close();
            return stocklist;
        }



    }
}