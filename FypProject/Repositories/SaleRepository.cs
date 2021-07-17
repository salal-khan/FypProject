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
    public class SaleRepository
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["fyp"].ToString());
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataTable dt;






        public string GetInvoiceNumber()
        {



            var dateTimeNow = DateTime.Now;

            string newInvoiceNo = string.Empty;


            try
            {
                con.Open();
                cmd = new SqlCommand($@"select * from SalesMaster order by SalesMasterId Desc", con);
                cmd.CommandType = CommandType.Text;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var LastInvoice = dt.Rows[0]["SalesMasterId"];
                    if (LastInvoice != null)
                    {

                        long currentCount_ = 0;
                        string currentCount = string.Empty;

                        if (Convert.ToDateTime(dt.Rows[0]["DateTime"]).Date == dateTimeNow.Date)
                            currentCount = dt.Rows[0]["InvoiceNumber"].ToString().Substring(9);
                        else
                            currentCount = "0";

                        long newCount = Convert.ToInt64(currentCount) + 1;

                        if (long.TryParse(currentCount, out currentCount_))
                            newInvoiceNo = $@"{dateTimeNow.ToString("yyyyMMdd")}{dateTimeNow.DayOfWeek.ToString().FirstOrDefault()}{newCount}";

                    }
                    else
                        newInvoiceNo = $@"{dateTimeNow.ToString("yyyyMMdd")}{dateTimeNow.DayOfWeek.ToString().FirstOrDefault()}1";
                }
                else
                    newInvoiceNo = $@"{dateTimeNow.ToString("yyyyMMdd")}{dateTimeNow.DayOfWeek.ToString().FirstOrDefault()}0";

            }
            catch (Exception ex)
            {
                return GlobalHelpers.GetLastInnerMessage(ex);
            }
            con.Close();
            return newInvoiceNo;

        }

        public int AddSale(AddSalesVM model)
        {
            con.Open();

            string condition = string.Empty;
            var roleName = HttpContext.Current.Session["RoleName"].ToString();
            if (roleName == constant.Roles.SuperAdmin)
                condition = $@"NULL";
            else
                condition = $@"'{HttpContext.Current.Session["FranchiseId"].ToString()}'";



            cmd = new SqlCommand($@"insert into SalesMaster (InvoiceNumber,NoOfQuantity,TotalAmount,DateTime,UserId,RecivedAmount,RecordAt,RecordBy,FranchiseId) values
           ('{model.InvoiceNumber}','{model.NoQuantity}','{model.TotalAmount}',GetDate(),'{HttpContext.Current.Session["Id"].ToString()}','{model.RecivedAmount}',GetDate(),'{HttpContext.Current.Session["Id"].ToString()}',{condition})", con);
            cmd.ExecuteNonQuery();


            cmd = new SqlCommand($@"select top 1 SalesMasterId from SalesMaster order by RecordAt desc", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            var insertedId = Convert.ToInt32(dt.Rows[0]["SalesMasterId"]);


            foreach (var item in model.salesDetailList)
            {
                var acutalAmount = GlobalHelpers.GetActualAmount(item.ProductId);
                cmd = new SqlCommand($@"insert into SalesDetail (SalesMasterId,ProductId,Quantity,SellingPrice,SellingSubTotal,ActualPrice,ActualSubTotal,RecordAt,RecordBy) values
                ('{insertedId}','{item.ProductId}','{item.Quantity}','{item.SellingPrice}','{item.SellingSubTotal}','{acutalAmount}','{acutalAmount * item.Quantity}',GetDate(),'{HttpContext.Current.Session["Id"].ToString()}')", con);
                cmd.ExecuteNonQuery();



                // remove headoffice 

                string availcountcondition = string.Empty;

                if (roleName == constant.Roles.SuperAdmin)
                    availcountcondition = $@"FranchiseId is NULL";
                else
                    availcountcondition = $@"FranchiseId = '{HttpContext.Current.Session["FranchiseId"].ToString()}'";



                cmd = new SqlCommand($@"select Quantity from AvailableProductCount where ProductId = '{item.ProductId}' and {availcountcondition}", con);
                cmd.CommandType = CommandType.Text;
                sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                var headofficeCount = Convert.ToInt32(dt.Rows[0]["Quantity"]) - item.Quantity;
                cmd = new SqlCommand($@"update AvailableProductCount set Quantity = '{headofficeCount}' where
                    ProductId = '{item.ProductId}' and {availcountcondition}", con);
                cmd.ExecuteNonQuery();




            }
            con.Close();
            return insertedId;
        }

        public EditSalesVM GetEditDetail(int SaleMasterId)
        {
            EditSalesVM edit = new EditSalesVM();
            con.Open();
            cmd = new SqlCommand($@"select * from SalesMaster where SalesMasterId = '{SaleMasterId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                edit = (from DataRow row in dt.Rows

                        select new EditSalesVM
                        {
                            SalesMasterId = Convert.ToInt32(row["SalesMasterId"]),
                            InvoiceNumber = row["InvoiceNumber"].ToString(),
                            NoQuantity = Convert.ToDouble(row["NoOfQuantity"]),
                            TotalAmount = Convert.ToDouble(row["TotalAmount"]),
                            RecivedAmount = Convert.ToDouble(row["RecivedAmount"]),
                            salesDetailList = GlobalHelpers.GetEditSaleDetail(Convert.ToInt32(row["SalesMasterId"]))
                        }).FirstOrDefault();
            }

            con.Close();
            return edit;


        }

        public string PostEditDetail(EditSalesVM model)
        {
            con.Open();
            cmd = new SqlCommand($@"update SalesMaster set  UpdateAt = GetDate(), UpdateBy = '{HttpContext.Current.Session["Id"].ToString()}', NoOfQuantity = '{model.NoQuantity}', TotalAmount = '{model.TotalAmount}',UserId = '{HttpContext.Current.Session["Id"].ToString()}',RecivedAmount = '{model.RecivedAmount}'
                            where SalesMasterId = '{model.SalesMasterId}'", con);
            cmd.ExecuteNonQuery();


            cmd = new SqlCommand($"delete from SalesDetail where SalesMasterId = '{model.SalesMasterId}'", con);
            cmd.ExecuteNonQuery();

            foreach (var item in model.salesDetailList)
            {
                var acutalAmount = GlobalHelpers.GetActualAmount(item.ProductId);
                cmd = new SqlCommand($@"insert into SalesDetail (SalesMasterId,ProductId,Quantity,SellingPrice,SellingSubTotal,ActualPrice,ActualSubTotal,RecordAt,RecordBy) values
                ('{model.SalesMasterId}','{item.ProductId}','{item.Quantity}','{item.SellingPrice}','{item.SellingSubTotal}','{acutalAmount}','{acutalAmount * item.Quantity}',GetDate(),'{HttpContext.Current.Session["Id"].ToString()}')", con);
                cmd.ExecuteNonQuery();
            }

            con.Close();

            return "Sales Updated Sucessfull..";

        }

        public List<SalesMasterIdnexVM> SalesList()
        {
            con.Open();
            string condition = string.Empty;
            var roleName = HttpContext.Current.Session["RoleName"].ToString();
            if (roleName == constant.Roles.SuperAdmin)
                condition = $@"";
            else
                condition = $@"where sm.FranchiseId = '{HttpContext.Current.Session["FranchiseId"].ToString()}'";



            List<SalesMasterIdnexVM> saleslist = new List<SalesMasterIdnexVM>();
            cmd = new SqlCommand($@"select u.UserName,sm.InvoiceNumber,sm.NoOfQuantity,sm.SalesMasterId,sm.TotalAmount,sm.Datetime from SalesMaster sm inner join Users u on u.Id = sm.UserId {condition} order by Datetime Desc", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                saleslist = (from DataRow row in dt.Rows

                             select new SalesMasterIdnexVM
                             {
                                 SalesMasterId = Convert.ToInt32(row["SalesMasterId"]),
                                 InvoiceNumber = row["InvoiceNumber"].ToString(),
                                 UserName = row["UserName"].ToString(),
                                 NoOfQuantity = Convert.ToDouble(row["NoOfQuantity"]),
                                 TotalAmount = Convert.ToDouble(row["TotalAmount"]),
                                 Datetime = Convert.ToDateTime(row["Datetime"])
                             }).ToList();
            }
            con.Close();
            return saleslist;
        }


    }
}