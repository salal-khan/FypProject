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
    public class DashBoardRepository
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["fyp"].ToString());
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataTable dt;

        
        public DashBoardVM MainWeigtsUserCount()
        {

            try
            {
                con.Open();

                var UserId = HttpContext.Current.Session["Id"].ToString();
                var FranchiseID = HttpContext.Current.Session["FranchiseId"].ToString();
                var UserRole = HttpContext.Current.Session["RoleName"].ToString();

                if (UserRole == constant.Roles.SuperAdmin)
                {
                    cmd = new SqlCommand($@"select COUNT(Id) as usercount from Users where UserName not in ('{constant.Roles.SuperAdmin}')", con);
                }
                else
                {
                    cmd = new SqlCommand($@"select COUNT(Id) as usercount from Users where UserName not in ('{constant.Roles.SuperAdmin}') and FranchiseId = '{FranchiseID}'", con);
                }
               
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var userCount = dt.AsEnumerable();

                    DashBoardVM Count_ =
                       (from item in userCount
                        select new DashBoardVM
                        {
                            TotalUsers = item.Field<int>("usercount"),
                        }).FirstOrDefault();
                    con.Close();
                    return Count_;
                }


            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }

            return null;
        }




        public DashBoardVM MainWeigtsFranchiseCount()
        {

            try
            {
                con.Open();
                var UserId = HttpContext.Current.Session["Id"].ToString();
                var FranchiseID = HttpContext.Current.Session["FranchiseId"].ToString();
                var UserRole = HttpContext.Current.Session["RoleName"].ToString();

                if (UserRole == constant.Roles.SuperAdmin)
                {
                    cmd = new SqlCommand("select COUNT(FranchiseId) as franchiseCount from Franchise", con);
                }
                else
                {
                    cmd = new SqlCommand($@"select COUNT(FranchiseId) as franchiseCount from Franchise where FranchiseId = '{FranchiseID}'", con);
                }
                   
                

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var franchiseCount = dt.AsEnumerable();

                    DashBoardVM Count_ =
                       (from item in franchiseCount
                        select new DashBoardVM
                        {
                            TotalFranchies = item.Field<int>("franchiseCount"),
                        }).FirstOrDefault();
                    con.Close();
                    return Count_;
                }

            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }

            return null;
        }



        public BarChartMasterVM GetBarChart()
        {

            var userId = HttpContext.Current.Session["Id"].ToString();
            var RoleName = HttpContext.Current.Session["RoleName"].ToString(); 

            BarChartMasterVM model = new BarChartMasterVM();
            List<BarChartMasterVM> BarList = new List<BarChartMasterVM>();
            BarChartDataSetVM datasets = new BarChartDataSetVM();

            if (RoleName == constant.Roles.SuperAdmin)
            {
                cmd = new SqlCommand($@"select distinct  pro.ProductName label, SUM(CAST(saldet.Quantity as int)) over(partition by pro.ProductId) data
                            from SalesMaster salmas
                            inner join SalesDetail saldet on saldet.SalesMasterId = salmas.SalesMasterId
                            inner join Product pro on pro.ProductId = saldet.ProductId", con);
            }
            else
            {
                cmd = new SqlCommand($@"select distinct  pro.ProductName label, SUM(CAST(saldet.Quantity as int)) over(partition by pro.ProductId) data
                            from SalesMaster salmas
                            inner join SalesDetail saldet on saldet.SalesMasterId = salmas.SalesMasterId
                            inner join Product pro on pro.ProductId = saldet.ProductId where saldet.RecordBy = '{userId}'", con);
            }

            con.Open();

            List<int> data = new List<int>();
            List<string> label = new List<string>();
            dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            
            if (dt.Rows.Count > 0)
            {
                var barChartList = dt.AsEnumerable();
                foreach (var item in barChartList)
                {
                    var Label =  item.Field<string>("label");
                    var Data = item.Field<int>("data");
                    label.Add(Label);
                    data.Add(Data);

                }
                model.labels = label;
                datasets.data = data;
                datasets.borderWidth = 2;
                datasets.backgroundColor = "#6777ef";
                datasets.borderColor = "#6777ef";
                datasets.pointBackgroundColor = "#ffffff";
                datasets.pointRadius = 4;
                
                model.datasets.Add(datasets);

            }
            con.Close();
            return model;
        }





        public LineChartMasterVM GetlineChart()
        {
            LineChartMasterVM model = new LineChartMasterVM();
            List<LineChartMasterVM> BarList = new List<LineChartMasterVM>();
            LineChartDataSetsVM datasets = new LineChartDataSetsVM();
            cmd = new SqlCommand($@"select distinct  pro.ProductName label, SUM(CAST(saldet.Quantity as int)) over(partition by pro.ProductId) data
                            from SalesMaster salmas
                            inner join SalesDetail saldet on saldet.SalesMasterId = salmas.SalesMasterId
                            inner join Product pro on pro.ProductId = saldet.ProductId", con);

            con.Open();

            List<int> data = new List<int>();
            List<string> label = new List<string>();
            dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                var barChartList = dt.AsEnumerable();
                foreach (var item in barChartList)
                {
                    var Label = item.Field<string>("label");
                    var Data = item.Field<int>("data");
                    label.Add(Label);
                    data.Add(Data);

                }
                model.lables = label;
                datasets.data = data;
                datasets.borderWidth = 2;
                datasets.backgroundColor = "#6777ef";
                datasets.borderColor = "#6777ef";
                datasets.pointBackgroundColor = "#ffffff";
                datasets.pointRadius = 4;

                model.datasets.Add(datasets);

            }
            con.Close();
            return model;
        }



    }
}