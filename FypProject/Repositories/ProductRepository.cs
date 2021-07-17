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
    public class ProductRepository
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["fyp"].ToString());
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataTable dt;


        public string Create(CreateProductVM model)

        {
            con.Open();
            #region BarcodeExistConditon
            

            cmd = new SqlCommand($@"select * from Product where Barcode = '{model.Barcode}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                

                return "This Barcode Already Exist in any Product...";
            }
            #endregion


            cmd = new SqlCommand($@"select * from Product where ProductName = '{model.ProductName}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                con.Close();
                return "This Product Already Exist..";
            }
            else
            {
                cmd = new SqlCommand($@"insert into Product (ProductName,Description,ProductActualPrice,ProductSellingPrice,Barcode,SubCategoryId,RecordBy,RecordAt) values 
              ('{model.ProductName}','{model.Description}','{model.ProductActualPrice}','{model.ProductSellingPrice}','{model.Barcode}','{model.SubCategoryId}','{HttpContext.Current.Session["Id"].ToString()}',GetDate())", con);
                cmd.ExecuteNonQuery();

                con.Close();
                return "Product Added Sucessfull..";
            }



        }

        public EditProductVM GetEditDetail(int productId)
        {
            con.Open();
            EditProductVM edit = new EditProductVM();
            cmd = new SqlCommand($@"select p.ProductId,p.ProductName,p.Description,p.ProductActualPrice,p.ProductSellingPrice,p.Barcode,sc.Subcategoryname,c.CategoryName,sc.SubCategoryId,c.CategoryId 
From Product p inner join SubCategory sc on sc.SubCategoryId = p.SubCategoryId inner join Category c on c.CategoryId = sc.CategoryId where p.ProductId = '{productId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                edit = (from DataRow row in dt.Rows

                        select new EditProductVM
                        {
                            ProductId = Convert.ToInt32(row["ProductId"]),
                            ProductName = row["ProductName"].ToString(),
                            Description = row["Description"].ToString(),
                            ProductActualPrice = Convert.ToDouble(row["ProductActualPrice"]),
                            ProductSellingPrice = Convert.ToDouble(row["ProductSellingPrice"]),
                            Barcode = row["Barcode"].ToString(),
                            SubCategoryId = Convert.ToInt32(row["SubCategoryId"]),
                            CategoryId = Convert.ToInt32(row["CategoryId"])

                        }).FirstOrDefault();
            }
            con.Close();
            return edit;
        }

        public string PostEditDetail(EditProductVM model)
        {

            #region BarcodeExistConditon
            con.Open();
            cmd = new SqlCommand($@"select * from Product where Barcode = '{model.Barcode}' AND Productd != '{model.ProductId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                con.Close();
                return "This Barcode Already Exist in any Product...";
            }
            #endregion



            con.Open();
            EditProductVM edit = new EditProductVM();

            cmd = new SqlCommand($@"select * from Product where ProductName = '{model.ProductName}' AND ProductId != '{model.ProductId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                con.Close();
                return "This Product Already Exist..";
            }
            else
            {
                cmd = new SqlCommand($@"update Product set UpdateAt = GetDate(), UpdateBy = '{HttpContext.Current.Session["Id"].ToString()}',
                            SubCategoryId = '{model.SubCategoryId}', ProductName = '{model.ProductName}', Description = '{model.Description}'
                            , ProductActualPrice = '{model.ProductActualPrice}', ProductSellingPrice = '{model.ProductSellingPrice}'  where ProductId = '{model.ProductId}' ", con);

                cmd.ExecuteNonQuery();
                con.Close();
                return "Product Updated Sucessfull..";
            }




        }

        public List<IndexProductVM> ProductList()
        {
            con.Open();

            List<IndexProductVM> productList = new List<IndexProductVM>();
            cmd = new SqlCommand($@"select p.ProductId,p.ProductName,p.Description,p.ProductActualPrice,p.ProductSellingPrice,p.Barcode,sc.Subcategoryname,c.CategoryName 
From Product p inner join SubCategory sc on sc.SubCategoryId = p.SubCategoryId inner join Category c on c.CategoryId = sc.CategoryId", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                productList = (from DataRow row in dt.Rows

                               select new IndexProductVM
                               {
                                   ProductId = Convert.ToInt32(row["ProductId"]),
                                   SubCategoryName = row["SubCategoryName"].ToString(),
                                   CategoryName = row["CategoryName"].ToString(),
                                   ProductName = row["ProductName"].ToString(),
                                   Description = row["Description"].ToString(),
                                   ProductSellingPrice = Convert.ToDouble(row["ProductSellingPrice"]),
                                   ProductActualPrice = Convert.ToDouble(row["ProductActualPrice"]),
                                   Barcode = row["Barcode"].ToString()


                               }).ToList();
            }
            con.Close();
            return productList;
        }

        public string GetBarCodeByProductId(int productId)
        {
            string barcode = string.Empty;
            con.Open();
            cmd = new SqlCommand($@"select Barcode from Product where ProductId = '{productId}' ", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                barcode = dt.Rows[0]["Barcode"].ToString();
            }
            con.Close();
            return barcode;
        }

        public string GenrateBarCode()
        {
            string barcode = string.Empty;
            con.Open();
            x:
            Random r = new Random();
            int randNum = r.Next(1000000000);
            string sixDigitNumber = randNum.ToString("D6");

            barcode = sixDigitNumber;



            cmd = new SqlCommand($@"select * from Product where Barcode = '{barcode}' ", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                goto x;
            }
            con.Close();
            return barcode;
        }
    }
}