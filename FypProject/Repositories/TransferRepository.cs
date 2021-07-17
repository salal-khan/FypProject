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
    public class TransferRepository
    {
        public static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["fyp"].ToString());
        public static SqlCommand cmd;
        public static SqlDataAdapter sda;
        public static DataTable dt;

        public string TransferForm(List<TransferDetailVM> model)
        {
            con.Open();
            cmd = new SqlCommand($@"insert into TransferMaster (FranchiseId,TotalProductQuantity,DateTime,RecordAt,RecordBy) values
           ('{model.FirstOrDefault().FranchiseId}','{model.Sum(x => x.ProductQuantity)}',GetDate(),GetDate(),'{HttpContext.Current.Session["Id"].ToString()}')", con);
            cmd.ExecuteNonQuery();


            cmd = new SqlCommand($@"select top 1 TransferId from TransferMaster order by RecordAt desc", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            var insertedId = Convert.ToInt32(dt.Rows[0]["TransferId"]);


            foreach (var item in model)
            {
                cmd = new SqlCommand($@"insert into TransferDetail (TransferMasterId,ProductId,ProductQuantity,RecordAt,RecordBy) values
                ('{insertedId}','{item.ProductId}','{item.ProductQuantity}',GetDate(),'{HttpContext.Current.Session["Id"].ToString()}')", con);
                cmd.ExecuteNonQuery();




                #region remove headoffice count

                cmd = new SqlCommand($@"select Quantity from AvailableProductCount where ProductId = '{item.ProductId}' and FranchiseId is NULL", con);
                cmd.CommandType = CommandType.Text;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var newproductCount = Convert.ToDouble(dt.Rows[0]["Quantity"]) - item.ProductQuantity;
                    cmd = new SqlCommand($@"update AvailableProductCount set Quantity = '{newproductCount}' where ProductId = '{item.ProductId}' and FranchiseId is NULL", con);
                    cmd.ExecuteNonQuery();

                }


                #endregion


                cmd = new SqlCommand($@"select Quantity from AvailableProductCount where ProductId = '{item.ProductId}' and FranchiseId = '{model.FirstOrDefault().FranchiseId}'", con);
                cmd.CommandType = CommandType.Text;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var newproductCount = item.ProductQuantity + Convert.ToDouble(dt.Rows[0]["Quantity"]);
                    cmd = new SqlCommand($@"update AvailableProductCount set Quantity = '{newproductCount}' where ProductId = '{item.ProductId}' and FranchiseId = '{model.FirstOrDefault().FranchiseId}'", con);
                    cmd.ExecuteNonQuery();

                }
                else
                {
                    cmd = new SqlCommand($@"insert into AvailableProductCount (ProductId,Quantity,CategoryId,SubCategoryId,FranchiseId) values ('{item.ProductId}', '{item.ProductQuantity}','{item.CategoryId}', '{item.SubCategoryId}','{model.FirstOrDefault().FranchiseId}')", con);
                    cmd.ExecuteNonQuery();
                }


            }
            con.Close();
            return "Transfer Form Submit Sucessfully...";
        }


        public List<TransferMasterIndexVM> TransferList()
        {
            con.Open();

            List<TransferMasterIndexVM> transferlist = new List<TransferMasterIndexVM>();
            cmd = new SqlCommand($@"select tm.TransferId,tm.TotalProductQuantity,tm.Datetime,f.FranchiseName from TransferMaster tm inner join Franchise f on tm.FranchiseId = f.FranchiseId
order by tm.RecordAt", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                transferlist = (from DataRow row in dt.Rows

                                select new TransferMasterIndexVM
                                {
                                    TransferMasterId = Convert.ToInt32(row["TransferId"]),
                                    Franchise = row["FranchiseName"].ToString(),
                                    TotalProductQuantity = Convert.ToDouble(row["TotalProductQuantity"]),
                                    Datetime = Convert.ToDateTime(row["Datetime"])
                                }).ToList();
            }
            con.Close();
            return transferlist;
        }


        public EditTransferVM GetEditDetail(int TransferMasterId)
        {
            EditTransferVM edit = new EditTransferVM();
            con.Open();
            cmd = new SqlCommand($@"select * from TransferMaster where TransferId = '{TransferMasterId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                edit = (from DataRow row in dt.Rows

                        select new EditTransferVM
                        {
                            TransferId = Convert.ToInt32(row["TransferId"]),
                            FranchiseId = Convert.ToInt32(row["FranchiseId"]),
                            //ProductQuantity = Convert.ToDouble(row["TotalProductQuantity"]),
                            transferDetailList = GlobalHelpers.GetEditTransferDetail(Convert.ToInt32(row["TransferId"]))
                        }).FirstOrDefault();
            }

            con.Close();
            return edit;


        }

        public string EditPostTransferForm(List<EditPostTransferDetailVM> model)
        {
            con.Open();
            var quantitysum = model.Sum(x => x.ProductQuantity);
            cmd = new SqlCommand($@"update TransferMaster set FranchiseId = '{model.First().FranchiseId}', TotalProductQuantity = '{quantitysum}', UpdateAt = GetDate(),
                                    UpdateBy = '{HttpContext.Current.Session["Id"].ToString()}' where TransferId = '{model.FirstOrDefault().TransferId}'", con);
            cmd.ExecuteNonQuery();
            var insertedId = model.First().TransferId;



            #region we need to manage availablecount 
            cmd = new SqlCommand($@"select ProductId,ProductQuantity from TransferDetail where TransferMasterId = '{insertedId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // add headoffice 
                    cmd = new SqlCommand($@"select Quantity from AvailableProductCount where ProductId = '{Convert.ToInt32(dt.Rows[i]["ProductId"])}' and FranchiseId is NULL", con);
                    cmd.CommandType = CommandType.Text;
                    sda = new SqlDataAdapter(cmd);
                    DataTable dtt = new DataTable();
                    sda.Fill(dtt);
                    var headofficeCount = Convert.ToInt32(dtt.Rows[0]["Quantity"]) + Convert.ToInt32(dt.Rows[i]["ProductQuantity"]);
                    cmd = new SqlCommand($@"update AvailableProductCount set Quantity = '{headofficeCount}' where
                    ProductId = '{Convert.ToInt32(dt.Rows[i]["ProductId"])}' and FranchiseId is NULL", con);
                    cmd.ExecuteNonQuery();

                    // remove franchise

                    cmd = new SqlCommand($@"select Quantity from AvailableProductCount where ProductId = '{Convert.ToInt32(dt.Rows[i]["ProductId"])}' and FranchiseId = '{model.First().FranchiseId}'", con);
                    cmd.CommandType = CommandType.Text;
                    sda = new SqlDataAdapter(cmd);
                    DataTable dttt = new DataTable();
                    sda.Fill(dttt);
                    var franchisecount = Convert.ToInt32(dttt.Rows[0]["Quantity"]) - Convert.ToInt32(dt.Rows[i]["ProductQuantity"]);
                    cmd = new SqlCommand($@"update AvailableProductCount set Quantity = '{franchisecount}' where
                    ProductId = '{Convert.ToInt32(dt.Rows[i]["ProductId"])}' and FranchiseId = '{model.First().FranchiseId}'", con);
                    cmd.ExecuteNonQuery();



                }
            }


                #endregion


                #region Delete Detail because we added new detail same transfermasterId
                cmd = new SqlCommand($@"delete from TransferDetail where TransferMasterId = '{insertedId}'", con);
            cmd.ExecuteNonQuery();
            #endregion



            foreach (var item 
                in model)
            {
                cmd = new SqlCommand($@"insert into TransferDetail (TransferMasterId,ProductId,ProductQuantity,RecordAt,RecordBy) values
                ('{insertedId}','{item.ProductId}','{item.ProductQuantity}',GetDate(),'{HttpContext.Current.Session["Id"].ToString()}')", con);
                cmd.ExecuteNonQuery();





                #region remove headoffice count

                cmd = new SqlCommand($@"select Quantity from AvailableProductCount where ProductId = '{item.ProductId}' and FranchiseId is NULL", con);
                cmd.CommandType = CommandType.Text;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var newproductCount = Convert.ToDouble(dt.Rows[0]["Quantity"]) - item.ProductQuantity;
                    cmd = new SqlCommand($@"update AvailableProductCount set Quantity = '{newproductCount}' where ProductId = '{item.ProductId}' and FranchiseId is NULL", con);
                    cmd.ExecuteNonQuery();

                }


                #endregion


                cmd = new SqlCommand($@"select Quantity from AvailableProductCount where ProductId = '{item.ProductId}' and FranchiseId = '{model.FirstOrDefault().FranchiseId}'", con);
                cmd.CommandType = CommandType.Text;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var newproductCount = item.ProductQuantity + Convert.ToDouble(dt.Rows[0]["Quantity"]);
                    cmd = new SqlCommand($@"update AvailableProductCount set Quantity = '{newproductCount}' where ProductId = '{item.ProductId}' and FranchiseId = '{model.FirstOrDefault().FranchiseId}'", con);
                    cmd.ExecuteNonQuery();

                }
                else
                {
                    cmd = new SqlCommand($@"insert into AvailableProductCount (ProductId,Quantity,CategoryId,SubCategoryId,FranchiseId) values ('{item.ProductId}', '{item.ProductQuantity}','{item.CategoryId}', '{item.SubCategoryId}','{model.FirstOrDefault().FranchiseId}')", con);
                    cmd.ExecuteNonQuery();
                }


            }
            con.Close();
            return "Edit Transfer Form Submit Sucessfully...";
        }


    }

}