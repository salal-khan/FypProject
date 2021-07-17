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
    public class SubCategoryRespository
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["fyp"].ToString());
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataTable dt;


        public string Create(CreateSubCategoryVM model)

        {
            con.Open();
            cmd = new SqlCommand($@"select * from SubCategory where SubCategoryName = '{model.SubCategoryName}' AND CategoryId = '{model.CategoryId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                con.Close();
                return "This Sub-Category Already Exist in Same Category..";
            }
            else
            {
                cmd = new SqlCommand($@"insert into SubCategory (SubCategoryName,CategoryId,RecordBy,RecordAt) values 
              ('{model.SubCategoryName}','{model.CategoryId}','{HttpContext.Current.Session["Id"].ToString()}',GetDate())", con);
                cmd.ExecuteNonQuery();

                con.Close();
                return "Sub-Category Added Sucessfull..";
            }



        }
        public List<IndexSubCategoryVM> SubCategoryList()
        {
            con.Open();

            List<IndexSubCategoryVM> subcategorylist = new List<IndexSubCategoryVM>();
            cmd = new SqlCommand($@"select sc.SubCategoryId,sc.SubCategoryName,c.CategoryName from SubCategory sc inner join Category c on c.CategoryId = sc.CategoryId", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                subcategorylist = (from DataRow row in dt.Rows

                                   select new IndexSubCategoryVM
                                   {
                                       SubCategoryId = Convert.ToInt32(row["SubCategoryId"]),
                                       SubCategoryName = row["SubCategoryName"].ToString(),
                                       CategoryName = row["CategoryName"].ToString()

                                   }).ToList();
            }
            con.Close();
            return subcategorylist;
        }

        public EditSubCategoryVM GetEditDetail(int subcategoryId)
        {
            con.Open();
            EditSubCategoryVM edit = new EditSubCategoryVM();
            cmd = new SqlCommand($@"select * from SubCategory where SubCategoryId = '{subcategoryId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                edit = (from DataRow row in dt.Rows

                        select new EditSubCategoryVM
                        {
                            SubCategoryId = Convert.ToInt32(row["SubCategoryId"]),
                            SubCategoryName = row["SubCategoryName"].ToString(),
                            CategoryId = Convert.ToInt32(row["CategoryId"])


                        }).FirstOrDefault();
            }
            con.Close();
            return edit;
        }

        public string PostEditDetail(EditSubCategoryVM model)
        {
            con.Open();
            EditSubCategoryVM edit = new EditSubCategoryVM();




            cmd = new SqlCommand($@"select * from SubCategory where SubCategoryName = '{model.SubCategoryName}' AND subCategoryId != '{model.SubCategoryId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                con.Close();
                return "This Sub-Category Already Exist..";
            }
            else
            {
                cmd = new SqlCommand($@"update SubCategory set UpdateAt = GetDate(), UpdateBy = '{HttpContext.Current.Session["Id"].ToString()}',
                            CategoryId = '{model.CategoryId}', SubCategoryName = '{model.SubCategoryName}' where SubCategoryId = '{model.SubCategoryId}' ", con);

                cmd.ExecuteNonQuery();
                con.Close();
                return "Sub-Category Updated Sucessfull..";
            }




        }
    }
}