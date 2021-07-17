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
    public class FranchiseRepository
    {

        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["fyp"].ToString());
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataTable dt;




        public string Create(FranchiseList model)
        {

            string message = string.Empty;
            try
            {
                cmd = new SqlCommand($@"Insert into Franchise (FranchiseName, ContactPerson, ContactNumber,Address, RecordAt, RecordBy) values ('{model.FranchiseName}','{model.ContactPerson}','{model.ContactNumber}', '{model.Address}',GETDATE(), '{HttpContext.Current.Session["Id"].ToString()}')", con);

                
                con.Open();
                int count_ = cmd.ExecuteNonQuery();
                if (count_ > 0)
                {
                    con.Close();
                    return message = "1";
                }
                else
                {
                    con.Close();
                    return message = "0";
                }

            }
            catch (Exception ex)
            {
                con.Close();
                return message = "2";
            }


        }

        public FranchiseVM GetEdit(int id)
        {
            string message = string.Empty;
            try
            {
                cmd = new SqlCommand($@"select * from Franchise where FranchiseId ='{id}' ", con);
                con.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {

                    var DatatableintoList = dt.AsEnumerable();

                    FranchiseVM EditObj =
                        (from item in DatatableintoList
                         select new FranchiseVM
                         {
                             FranchiseId = item.Field<int>("FranchiseId"),
                             FranchiseName = item.Field<string>("FranchiseName"),
                             ContactPerson = item.Field<string>("ContactPerson"),
                             Address = item.Field<string>("Address"),
                             ContactNumber = item.Field<string>("ContactNumber")
                         }).FirstOrDefault();
                    
                    return EditObj;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public string Edit(int id, FranchiseList model)
        {
            string message = string.Empty;
            try
            {
               
                cmd = new SqlCommand($@"update Franchise set FranchiseName = '{model.FranchiseName}' ,ContactPerson = '{model.ContactPerson}', ContactNumber = '{model.ContactNumber}',Address='{model.Address}', UpdateBy = '{HttpContext.Current.Session["Id"].ToString()}', UpdateAt = '{DateTime.Now}' where FranchiseId = '{id}'", con);
                con.Open();
                int count =  cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    con.Close();
                    return message = "1";
                }
                else
                {
                    con.Close();
                    return message = "0";
                }

            }
            catch (Exception ex)
            {
                con.Close();
                return message = "2";   
            }
        }

      
        public List<FranchiseList> GetIndexList()
        {

            string message = string.Empty;
            try
            {
                List<FranchiseList> IndexList = new List<FranchiseList>();
                cmd = new SqlCommand($@"select * from Franchise", con);
                con.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    con.Close();

                    var indexList = dt.AsEnumerable();

                     IndexList = (from item in indexList
                                                     select new FranchiseList
                                                     {
                                                         FranchiseId = item.Field<int>("FranchiseId"),
                                                         FranchiseName = item.Field<string>("FranchiseName"),
                                                         ContactPerson = item.Field<string>("ContactPerson"),
                                                         ContactNumber = item.Field<string>("ContactNumber"),
                                                         Address = item.Field<string>("Address")

                                                     }).ToList();

                    
                }
                return IndexList;

            }
            catch (Exception ex)
            {
                return null;
            }


        }


        public string Delete(int id)
        {
            string message = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                cmd = new SqlCommand($@"select * from users where franchiseId = '{id}'", con);
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return message = "2";
                }


                cmd = new SqlCommand($@"delete from Franchise where FranchiseId = '{id}'", con);
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    con.Close();
                    return message = "1";
                }
                else
                {
                    con.Close();
                    return message = "0";
                }


            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }





    }
}