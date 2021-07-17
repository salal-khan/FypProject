using FypProject.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FypProject.Extensions
{
    public static class GlobalHelpers
    {

        public static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["fyp"].ToString());
        public static SqlCommand cmd;
        public static SqlDataAdapter sda;
        public static DataTable dt;


        public static string GetLastInnerMessage(Exception ex)
        {
            string message = "";
            for (int i = 0; ex != null; i++)
            {
                message += $@" {1 + i}:-{ex.Message }. {Environment.NewLine}";
                ex = ex.InnerException;
            }
            return message;
        }


        public static List<RoleList> GetRoleList()
        {
            List<RoleList> roleList = new List<RoleList>();
            cmd = new SqlCommand($@"select * from Roles where RoleName != 'superadmin' ", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                roleList = (from DataRow dr in dt.Rows
                            select new RoleList()
                            {
                                RoleId = Convert.ToInt32(dr["RoleId"]),
                                RoleName = dr["RoleName"].ToString(),
                            }).ToList();
            }
            return roleList;


        }

        public static List<FranchiseList> GetFranchiseList()
        {
            List<FranchiseList> franchiseList = new List<FranchiseList>();
            cmd = new SqlCommand($@"select FranchiseId,FranchiseName from Franchise", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                franchiseList = (from DataRow dr in dt.Rows
                                 select new FranchiseList()
                                 {
                                     FranchiseId = Convert.ToInt32(dr["FranchiseId"]),
                                     FranchiseName = dr["FranchiseName"].ToString(),
                                 }).ToList();
            }
            return franchiseList;


        }



        public static string GetRoleById(int RoleId)
        {
            cmd = new SqlCommand($@"select RoleName from Roles where RoleId = '{RoleId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["RoleName"].ToString();
            }
            return string.Empty;

        }

        public static List<CategoryVM> GetCategoryList()
        {
            List<CategoryVM> categoryList = new List<CategoryVM>();
            cmd = new SqlCommand($@"select CategoryId,CategoryName from Category", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                categoryList = (from DataRow dr in dt.Rows
                                select new CategoryVM()
                                {
                                    CategoryId = Convert.ToInt32(dr["CategoryId"]),
                                    CategoryName = dr["CategoryName"].ToString(),
                                }).ToList();
            }
            return categoryList;


        }

        public static List<SubCategoryVM> GetSubCategoryList()
        {
            List<SubCategoryVM> subcategoryList = new List<SubCategoryVM>();
            cmd = new SqlCommand($@"select SubCategoryId,SubCategoryName from SubCategory", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                subcategoryList = (from DataRow dr in dt.Rows
                                   select new SubCategoryVM()
                                   {
                                       SubCategoryId = Convert.ToInt32(dr["SubCategoryId"]),
                                       SubCategoryName = dr["SubCategoryName"].ToString(),
                                   }).ToList();
            }
            return subcategoryList;


        }

        public static List<SubCategoryVM> GetSubCategoryListByCategoryId(int categoryId)
        {
            List<SubCategoryVM> subcategoryList = new List<SubCategoryVM>();
            cmd = new SqlCommand($@"select SubCategoryId,SubCategoryName from SubCategory where CategoryId = '{categoryId}' ", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                subcategoryList = (from DataRow dr in dt.Rows
                                   select new SubCategoryVM()
                                   {
                                       SubCategoryId = Convert.ToInt32(dr["SubCategoryId"]),
                                       SubCategoryName = dr["SubCategoryName"].ToString(),
                                   }).ToList();
            }
            return subcategoryList;


        }


        public static List<ProductVM> GetProductList()
        {
            List<ProductVM> productList = new List<ProductVM>();
            cmd = new SqlCommand($@"select ProductId,ProductName from Product", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                productList = (from DataRow dr in dt.Rows
                               select new ProductVM()
                               {
                                   ProductId = Convert.ToInt32(dr["ProductId"]),
                                   ProductName = dr["ProductName"].ToString(),
                               }).ToList();
            }
            return productList;


        }



        public static List<ProductValueBarcodeVM> GetProductListValueBarcode()
        {
            string condition = string.Empty;
            var roleName = HttpContext.Current.Session["RoleName"].ToString();
            if (roleName == constant.Roles.SuperAdmin)
            {
                condition = $@"FranchiseId is NULL";
            }
            else
            {
                condition = $@"FranchiseId = '{HttpContext.Current.Session["FranchiseId"].ToString()}'";
            }


            List<ProductValueBarcodeVM> productList = new List<ProductValueBarcodeVM>();
            cmd = new SqlCommand($@"select  P.Barcode as ProductBarcode,P.ProductName + ' (' + CAST(ac.Quantity as varchar) + ')'  as ProductName from AvailableProductCount ac
inner join Product p on p.ProductId = ac.ProductId where {condition} ", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                productList = (from DataRow dr in dt.Rows
                               select new ProductValueBarcodeVM()
                               {
                                   ProductBarcode = dr["ProductBarcode"].ToString(),
                                   ProductName = dr["ProductName"].ToString(),
                               }).ToList();
            }
            return productList;


        }


        public static List<ProductVM> GetProductListBySubCategoryId(int subcategoryId)
        {
            List<ProductVM> productList = new List<ProductVM>();
            cmd = new SqlCommand($@"select ProductId,ProductName from Product where SubCategoryId = '{subcategoryId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                productList = (from DataRow dr in dt.Rows
                               select new ProductVM()
                               {
                                   ProductId = Convert.ToInt32(dr["ProductId"]),
                                   ProductName = dr["ProductName"].ToString(),
                               }).ToList();
            }
            return productList;


        }

        public static string GetProductById(int productId)
        {
            cmd = new SqlCommand($@"select ProductName from Product where ProductId = '{productId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["ProductName"].ToString();
            }
            return string.Empty;

        }
        public static string GetCategoryByProductId(int productId)
        {
            cmd = new SqlCommand($@"select c.CategoryName from Category c inner join SubCategory sc on c.CategoryId = sc.SubCategoryId
                                inner join Product p on sc.SubCategoryId = p.SubCategoryId
                                where p.ProductId = '{productId}' ", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["ProductName"].ToString();
            }
            return string.Empty;

        }

        public static string GetSubCategoryByProductId(int productId)
        {
            cmd = new SqlCommand($@"select sc.SubCategoryName from SubCategory sc 
                                inner join Product p on sc.SubCategoryId = p.SubCategoryId
                                where p.ProductId = '{productId}' ", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["ProductName"].ToString();
            }
            return string.Empty;

        }

        public static List<StockDetailIndexVM> GetStockDetail(int StockMasterId)
        {
            List<StockDetailIndexVM> stockDetailList = new List<StockDetailIndexVM>();

            cmd = new SqlCommand($@"select sd.SubTotalAmount,sd.ProductQuantity,sd.ProductActualPrice,sd.ProductId,p.ProductName,sc.SubCategoryName,sc.SubCategoryId,c.CategoryName,c.CategoryId 
from StockDetail sd inner join Product p on p.ProductId = sd.ProductId 
inner join SubCategory sc on sc.SubCategoryId = p.SubCategoryId inner join Category c on c.CategoryId = sc.CategoryId
where sd.StockMasterId = '{StockMasterId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                stockDetailList = (from DataRow dr in dt.Rows
                                   select new StockDetailIndexVM()
                                   {
                                       ProductId = Convert.ToInt32(dr["ProductId"]),
                                       ProductName = dr["ProductName"].ToString(),
                                       CategoryId = Convert.ToInt32(dr["CategoryId"]),
                                       SubCategoryName = dr["SubCategoryName"].ToString(),
                                       SubCategoryId = Convert.ToInt32(dr["SubCategoryId"]),
                                       CategoryName = dr["CategoryName"].ToString(),
                                       ProductQuantity = Convert.ToInt32(dr["ProductQuantity"]),
                                       ProductActualPrice = Convert.ToDouble(dr["ProductActualPrice"]),
                                       ProductSubTotalAmount = Convert.ToDouble(dr["SubTotalAmount"])
                                   }).ToList();
            }

            return stockDetailList;

        }

        public static List<SalesDetailIndexVM> GetSalesDetail(int SalesMasterId)
        {
            List<SalesDetailIndexVM> salesDetailList = new List<SalesDetailIndexVM>();

            cmd = new SqlCommand($@"select sd.SalesMasterId,sd.Quantity,SellingPrice,SellingSubTotal,p.ProductName,sd.ProductId,c.CategoryName,c.CategoryId,sc.SubCategoryId,sc.SubCategoryName from SalesDetail sd
inner join Product p on p.ProductId = sd.ProductId 
inner join SubCategory sc on sc.SubCategoryId = p.SubCategoryId inner join Category c on c.CategoryId = sc.CategoryId
where sd.SalesMasterId  = '{SalesMasterId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                salesDetailList = (from DataRow dr in dt.Rows
                                   select new SalesDetailIndexVM()
                                   {
                                       SalesMasterId = Convert.ToInt32(dr["SalesMasterId"]),
                                       ProductId = Convert.ToInt32(dr["ProductId"]),
                                       ProductName = dr["ProductName"].ToString(),
                                       CategoryId = Convert.ToInt32(dr["CategoryId"]),
                                       SubCategoryName = dr["SubCategoryName"].ToString(),
                                       SubCategoryId = Convert.ToInt32(dr["SubCategoryId"]),
                                       CategoryName = dr["CategoryName"].ToString(),
                                       Quantity = Convert.ToInt32(dr["Quantity"]),
                                       SellingPrice = Convert.ToDouble(dr["SellingPrice"]),
                                       SellingSubTotal = Convert.ToDouble(dr["SellingSubTotal"])
                                   }).ToList();
            }

            return salesDetailList;

        }


        public static List<EditTranferDetailVM> GetEditTransferDetail(int TransferId)
        {
            con.Open();
            List<EditTranferDetailVM> edit = new List<EditTranferDetailVM>();
            cmd = new SqlCommand($@"
select tm.FranchiseId,tm.TransferId,td.ProductId,td.ProductQuantity,P.ProductActualPrice, P.ProductName,c.CategoryId,c.CategoryName,sc.SubCategoryId,sc.SubCategoryName
From TransferDetail td  inner join Product P on P.ProductId = td.ProductId  inner join SubCategory sc on sc.SubCategoryId = p.SubCategoryId
inner join Category c on c.CategoryId = sc.CategoryId inner join TransferMaster tm on tm.TransferId = td.TransferMasterId   where td.TransferMasterId = '{TransferId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                edit = (from DataRow row in dt.Rows

                        select new EditTranferDetailVM
                        {

                            TransferMasterId = Convert.ToInt32(row["TransferId"]),
                            ProductId = Convert.ToInt32(row["ProductId"]),
                            ProductName = row["ProductName"].ToString(),
                            CategoryId = Convert.ToInt32(row["CategoryId"]),
                            CategoryName = row["Categoryname"].ToString(),
                            SubCategoryId = Convert.ToInt32(row["SubCategoryId"]),
                            SubCategoryName = row["SubCategoryName"].ToString(),
                            FranchiseId = Convert.ToInt32(row["FranchiseId"]),
                            ProductQuantity = Convert.ToDouble(row["ProductQuantity"]),
                            ProductActualPrice = Convert.ToDouble(row["ProductActualPrice"]),
                            SubTotalAmount = Convert.ToDouble(row["ProductQuantity"]) * Convert.ToDouble(row["ProductActualPrice"])

                        }).ToList();
            }
            con.Close();

            return edit;
        }



        public static List<EditSalesDetailVM> GetEditSaleDetail(int SalesMasterId)
        {
            con.Open();
            List<EditSalesDetailVM> edit = new List<EditSalesDetailVM>();
            cmd = new SqlCommand($@"
select sd.SalesMasterId,sd.ProductId,sd.Quantity,sd.SellingPrice,sd.SellingSubTotal,P.ProductName
From SalesDetail sd 
inner join Product P on P.ProductId = sd.ProductId where sd.SalesMasterId = '{SalesMasterId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                edit = (from DataRow row in dt.Rows

                        select new EditSalesDetailVM
                        {

                            SalesMasterId = Convert.ToInt32(row["SalesMasterId"]),
                            ProductId = Convert.ToInt32(row["ProductId"]),
                            ProductName = row["ProductName"].ToString(),
                            Quantity = Convert.ToDouble(row["Quantity"]),
                            SellingPrice = Convert.ToDouble(row["SellingPrice"]),
                            SellingSubTotal = Convert.ToDouble(row["SellingSubTotal"]),
                        }).ToList();
            }
            con.Close();

            return edit;
        }
        public static GetDetailBarcodeVM GetDetailByBarcode(string Barcode)
        {
            GetDetailBarcodeVM detail = new GetDetailBarcodeVM();
            cmd = new SqlCommand($@"select p.ProductSellingPrice, p.ProductId, p.ProductName , p.ProductActualPrice, c.CategoryId,c.CategoryName, sc.SubCategoryId,sc.SubCategoryName
                                   from Product p
                                    inner join SubCategory sc on p.SubCategoryId = sc.SubCategoryId
                                     inner join Category c on c.CategoryId = sc.CategoryId where p.barcode = '{Barcode}' ", con);

            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {

                detail = (from DataRow dr in dt.Rows
                          select new GetDetailBarcodeVM()
                          {
                              ProductId = Convert.ToInt32(dr["ProductId"]),
                              ProductName = dr["ProductName"].ToString(),
                              CategoryId = Convert.ToInt32(dr["CategoryId"]),
                              SubCategoryName = dr["SubCategoryName"].ToString(),
                              SubCategoryId = Convert.ToInt32(dr["SubCategoryId"]),
                              CategoryName = dr["CategoryName"].ToString(),
                              ProductActualPrice = Convert.ToDouble(dr["ProductActualPrice"]),
                              ProductSellingPrice = Convert.ToDouble(dr["ProductSellingPrice"])

                          }).FirstOrDefault();
            }


            return detail;
        }


        public static GetDetailProductIdVM GetDetailByProductId(int productId)
        {
            GetDetailProductIdVM detail = new GetDetailProductIdVM();
            cmd = new SqlCommand($@"select p.ProductSellingPrice, p.ProductId, p.ProductName , p.ProductActualPrice, c.CategoryId,c.CategoryName, sc.SubCategoryId,sc.SubCategoryName
                                   from Product p
                                    inner join SubCategory sc on p.SubCategoryId = sc.SubCategoryId
                                     inner join Category c on c.CategoryId = sc.CategoryId where p.productId = '{productId}' ", con);

            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {

                detail = (from DataRow dr in dt.Rows
                          select new GetDetailProductIdVM()
                          {
                              ProductId = Convert.ToInt32(dr["ProductId"]),
                              ProductName = dr["ProductName"].ToString(),
                              CategoryId = Convert.ToInt32(dr["CategoryId"]),
                              SubCategoryName = dr["SubCategoryName"].ToString(),
                              SubCategoryId = Convert.ToInt32(dr["SubCategoryId"]),
                              CategoryName = dr["CategoryName"].ToString(),
                              ProductActualPrice = Convert.ToDouble(dr["ProductActualPrice"]),
                              ProductSellingPrice = Convert.ToDouble(dr["ProductSellingPrice"])

                          }).FirstOrDefault();
            }


            return detail;
        }

        public static string GetUserName(int userId)
        {
            cmd = new SqlCommand($@"select UserName from Users where Id = '{userId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["UserName"].ToString();
            }
            return string.Empty;
        }

        public static double GetActualAmount(int productId)
        {
            cmd = new SqlCommand($@"select ProductActualPrice from Product where ProductId = '{productId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return Convert.ToDouble(dt.Rows[0]["ProductActualPrice"]);
            }
            return 0;
        }

        public static bool SalesMasterIdExistInDb(int salesMasterId)
        {

            cmd = new SqlCommand($@"select * From SalesMaster where SalesMasterId = '{salesMasterId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static DataTable GetInvoiceData(int SalesMasterId)
        {
            cmd = new SqlCommand($@"select u.UserName,sm.InvoiceNumber,Convert(int,sm.NoOfQuantity) as NoOfQuantity,convert(int,sm.TotalAmount) as TotalAmount,convert(int,sm.RecivedAmount) as RecivedAmount,convert(int,sd.SellingPrice) as SellingPrice,convert(int,sd.SellingSubTotal) as SellingSubTotal,sd.ProductId
,p.ProductName,sd.Quantity,convert(varchar(11),sm.Datetime,13) as Date,right(convert(varchar,sm.Datetime,100),7)InvoiceTime 
from SalesMaster sm
inner join SalesDetail sd on sm.SalesMasterId = sd.SalesMasterId
inner join Users u on u.Id = sm.UserId
inner join Product P on p.ProductId = sd.ProductId where sm.SalesMasterId = '{SalesMasterId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }


        public static List<ProductVM> GetStockProductList()
        {
            List<ProductVM> productList = new List<ProductVM>();
            cmd = new SqlCommand($@"select p.ProductId, p.ProductName + ' (' + CAST(ac.Quantity as varchar) + ')'  as ProductName from AvailableProductCount ac inner join Product p on p.ProductId = ac.ProductId
where ac.FranchiseId is null", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                productList = (from DataRow dr in dt.Rows
                               select new ProductVM()
                               {
                                   ProductId = Convert.ToInt32(dr["ProductId"]),
                                   ProductName = dr["ProductName"].ToString(),
                               }).ToList();
            }
            return productList;


        }

        public static List<TransferDetailIndexVM> GetTransferDetail(int TrasnferMasterId)
        {
            List<TransferDetailIndexVM> transferDetailList = new List<TransferDetailIndexVM>();

            cmd = new SqlCommand($@"select td.ProductQuantity,td.ProductId,p.ProductName,sc.SubCategoryName,sc.SubCategoryId,c.CategoryName,c.CategoryId 
from TransferDetail td inner join Product p on p.ProductId = td.ProductId 
inner join SubCategory sc on sc.SubCategoryId = p.SubCategoryId inner join Category c on c.CategoryId = sc.CategoryId
where td.TransferMasterId = '{TrasnferMasterId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                transferDetailList = (from DataRow dr in dt.Rows
                                      select new TransferDetailIndexVM()
                                      {
                                          ProductId = Convert.ToInt32(dr["ProductId"]),
                                          ProductName = dr["ProductName"].ToString(),
                                          CategoryId = Convert.ToInt32(dr["CategoryId"]),
                                          SubCategoryName = dr["SubCategoryName"].ToString(),
                                          SubCategoryId = Convert.ToInt32(dr["SubCategoryId"]),
                                          CategoryName = dr["CategoryName"].ToString(),
                                          ProductQuantity = Convert.ToInt32(dr["ProductQuantity"])

                                      }).ToList();
            }

            return transferDetailList;

        }


        public static ArrayList GetAvailableCountByBarcode(string Barcode)
        {
            string condition = string.Empty;
            var roleName = HttpContext.Current.Session["RoleName"].ToString();
            if (roleName == constant.Roles.SuperAdmin)
                condition = $@"ac.FranchiseId is NULL";
            else
                condition = $@"ac.FranchiseId = '{HttpContext.Current.Session["FranchiseId"].ToString()}'";


            ArrayList array = new ArrayList();
            double quantity = 0;
            cmd = new SqlCommand($@" select ac.Quantity,P.ProductId from AvailableProductCount ac inner join Product p on ac.ProductId = p.ProductId 
                where p.Barcode = '{Barcode}' and {condition} ", con);

            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                array.Add(Convert.ToDouble(dt.Rows[0]["Quantity"]));
                array.Add(Convert.ToInt32(dt.Rows[0]["ProductId"]));

            }


            return array;
        }



        public static double GetAvailableCountByProductId(string productId)
        {
            double quantity = 0;
            cmd = new SqlCommand($@" select ac.Quantity from AvailableProductCount ac inner join Product p on ac.ProductId = p.ProductId where p.ProductId = '{productId}' and ac.FranchiseId is null ", con);

            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                quantity = Convert.ToDouble(dt.Rows[0]["Quantity"]);
            }


            return quantity;
        }



        public static double GetPredictQuantity(string barcode, int thresholdValue)
        {
            double quantity = 0;
            int dbMonthCount = 0;

            cmd = new SqlCommand($@"select distinct MONTH(sd.recordAt) from SalesDetail sd  inner join Product p on p.ProductId = sd.ProductId  where Barcode = '{barcode}'", con);

            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dbMonthCount = dt.Rows.Count;
            }

            if (thresholdValue >= dbMonthCount)
                thresholdValue = dbMonthCount;


            cmd = new SqlCommand($@" select distinct Round(sum(Cast(a.Quantity as decimal)) over (partition by a.row) / {thresholdValue} ,0) as PredictQuantity  from (
                                     select sd.Quantity , '' as row from SalesMaster sm inner join SalesDetail sd on sm.SalesMasterId = sd.SalesMasterId
                                     inner join Product p on p.ProductId = sd.ProductId
                                     where sm.FranchiseId is null and p.Barcode = '{barcode}' 
                                     and CONVERT(varchar(10),sd.RecordAt,121) between Convert(varchar(10),DATEADD(Month,-{thresholdValue},GetDate()),121) and convert(varchar(10),Getdate(),121)
                                     ) a", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                quantity = Convert.ToDouble(dt.Rows[0]["PredictQuantity"]);
            }
            return quantity;

        }




    }
}