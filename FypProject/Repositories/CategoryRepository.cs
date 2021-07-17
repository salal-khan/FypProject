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
    public class CategoryRepository
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["fyp"].ToString());
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataTable dt;


        public string Create(CreateCategoryVM model)

        {
            con.Open();
            cmd = new SqlCommand($@"select * from Category where CategoryName = '{model.CategoryName}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                con.Close();
                return "This Category Already Exist..";
            }
            else
            {
                cmd = new SqlCommand($@"insert into Category (CategoryName,RecordBy,RecordAt) values 
              ('{model.CategoryName}','{HttpContext.Current.Session["Id"].ToString()}',GetDate())", con);
                cmd.ExecuteNonQuery();

                con.Close();
                return "Category" +
                    " Added Sucessfull..";
            }



        }
        public List<IndexCategoryVM> CategoryList()
        {
            con.Open();

            List<IndexCategoryVM> categorylist = new List<IndexCategoryVM>();
            cmd = new SqlCommand($@"select CategoryId,CategoryName from Category", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                categorylist = (from DataRow row in dt.Rows

                                select new IndexCategoryVM
                                {
                                    CategoryId = Convert.ToInt32(row["CategoryId"]),
                                    CategoryName = row["CategoryName"].ToString(),

                                }).ToList();
            }
            con.Close();
            return categorylist;
        }

        public EditCategoryVM GetEditDetail(int categoryId)
        {
            con.Open();
            EditCategoryVM edit = new EditCategoryVM();
            cmd = new SqlCommand($@"select * from Category where CategoryId = '{categoryId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                edit = (from DataRow row in dt.Rows

                        select new EditCategoryVM
                        {
                            CategoryId = Convert.ToInt32(row["CategoryId"]),
                            CategoryName = row["CategoryName"].ToString(),

                        }).FirstOrDefault();
            }
            con.Close();
            return edit;
        }

        public string PostEditDetail(EditCategoryVM model)
        {
            con.Open();
            EditCategoryVM edit = new EditCategoryVM();




            cmd = new SqlCommand($@"select * from Category where CategoryName = '{model.CategoryName}' AND CategoryId != '{model.CategoryId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                con.Close();
                return "This Category Already Exist..";
            }
            else
            {
                cmd = new SqlCommand($@"update Category set UpdateAt = GetDate(), UpdateBy = '{HttpContext.Current.Session["Id"].ToString()}',
                            CategoryName = '{model.CategoryName}' where CategoryId = '{model.CategoryId}' ", con);

                cmd.ExecuteNonQuery();
                con.Close();
                return "Category Updated Sucessfull..";
            }




        }


    }
}