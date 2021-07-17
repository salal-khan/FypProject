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
    public class UserRepository
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["fyp"].ToString());
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataTable dt;

        public string Create(CreateUserVM model)
        {
            con.Open();
            cmd = new SqlCommand($@"insert into Users (UserName,FirstName,LastName,EmailAddress,Address,Cnic,PhoneNumber,Password,RecordBy,RecordAt,FranchiseId) values 
              ('{model.UserName}','{model.FirstName}','{model.LastName}','{model.Email}','{model.Address}','{model.CNIC}',
              '{model.PhoneNumber}','{model.Password}','{HttpContext.Current.Session["Id"].ToString()}',GetDate(),'{model.FranchiseId}')", con);
            cmd.ExecuteNonQuery();


            cmd = new SqlCommand($@"select Id from Users where UserName = '{model.UserName}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                cmd = new SqlCommand($@"insert into UserRole (RoleId,UserId) values ('{model.RoleId}','{dt.Rows[0]["Id"]}')", con);
                cmd.ExecuteNonQuery();


            }
            con.Close();
            return "User Add Sucessfull..";
        }

        public List<IndexUserVM> UserList()
        {
            con.Open();

            List<IndexUserVM> userList = new List<IndexUserVM>();
            cmd = new SqlCommand($@"  select  f.FranchiseName , u.Id,u.UserName,u.FirstName,u.LastName,u.EmailAddress,r.RoleName 
 from Users u inner join UserRole ur on ur.UserId = u.Id inner join Roles r on r.RoleId = ur.RoleId
  left join Franchise f on f.FranchiseId = u.FranchiseId
               where u.UserName != 'superAdmin'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                userList = (from DataRow row in dt.Rows

                            select new IndexUserVM
                            {
                                UserName = row["UserName"].ToString(),
                                RoleName = row["RoleName"].ToString(),
                                FirstName = row["FirstName"].ToString(),
                                LastName = row["LastName"].ToString(),
                                EmailAddress = row["EmailAddress"].ToString(),
                                FranchiseName = row["FranchiseName"].ToString(),
                                Id = Convert.ToInt32(row["Id"])


                            }).ToList();
            }
            con.Close();
            return userList;
        }


        public DetailUserVM UserDetail(int userId)
        {
            con.Open();
            DetailUserVM detail = new DetailUserVM();
            cmd = new SqlCommand($@"select  f.FranchiseName , u.Id,u.UserName,u.FirstName,u.LastName,u.EmailAddress,r.RoleName,u.Address,u.PhoneNumber,u.Cnic 
 from Users u inner join UserRole ur on ur.UserId = u.Id inner join Roles r on r.RoleId = ur.RoleId
  left join Franchise f on f.FranchiseId = u.FranchiseId where u.id = '{userId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                detail = (from DataRow row in dt.Rows

                          select new DetailUserVM
                          {
                              UserName = row["UserName"].ToString(),
                              RoleName = row["RoleName"].ToString(),
                              FirstName = row["FirstName"].ToString(),
                              LastName = row["LastName"].ToString(),
                              EmailAddress = row["EmailAddress"].ToString(),
                              FranchiseName = row["FranchiseName"].ToString(),
                              Id = Convert.ToInt32(row["Id"]),
                              Address = row["Address"].ToString(),
                              Cnic = row["Cnic"].ToString(),
                              PhoneNumber = row["PhoneNumber"].ToString(),
                          }).FirstOrDefault();
            }
            con.Close();
            return detail;
        }

        public EditUserVM GetEditDetail(int userId)
        {
            con.Open();
            EditUserVM edit = new EditUserVM();
            cmd = new SqlCommand($@" select  ur.RoleId,u.FranchiseId,f.FranchiseName , u.Id,u.UserName,u.FirstName,u.LastName,u.EmailAddress,r.RoleName,u.Address,u.PhoneNumber,u.Cnic,u.Password 
 from Users u inner join UserRole ur on ur.UserId = u.Id inner join Roles r on r.RoleId = ur.RoleId
  left join Franchise f on f.FranchiseId = u.FranchiseId where u.id = '{userId}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                edit = (from DataRow row in dt.Rows

                        select new EditUserVM
                        {
                            UserName = row["UserName"].ToString(),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            Email = row["EmailAddress"].ToString(),
                            RoleId = Convert.ToInt32(row["RoleId"]),
                            FranchiseId = Convert.ToInt32(row["FranchiseId"]),
                            Id = Convert.ToInt32(row["Id"]),
                            Address = row["Address"].ToString(),
                            CNIC = row["Cnic"].ToString(),
                            PhoneNumber = row["PhoneNumber"].ToString(),
                            Password = row["Password"].ToString(),
                            ConfirmPassword = row["Password"].ToString()
                        }).FirstOrDefault();
            }
            con.Close();
            return edit;
        }


        public EditUserVM PostEditDetail(EditUserVM model)
        {
            con.Open();
            EditUserVM edit = new EditUserVM();
            string updatePassowrd = string.Empty;

            if (!string.IsNullOrEmpty(model.Password))
                updatePassowrd = $@",Password = '{model.Password}'";


            cmd = new SqlCommand($@"update Users set UpdateAt = GetDate(), UpdateBy = '{HttpContext.Current.Session["Id"].ToString()}', UserName = '{model.UserName}',FirstName = '{model.FirstName}',
LastName = '{model.LastName}', EmailAddress = '{model.Email}', Address = '{model.Address}',Cnic = '{model.CNIC}',PhoneNumber = '{model.PhoneNumber}',FranchiseId = '{model.FranchiseId}' {updatePassowrd} where Id = '{model.Id}' ", con);
            cmd.ExecuteNonQuery();


            cmd = new SqlCommand($@"select Id from Users where UserName = '{model.UserName}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                cmd = new SqlCommand($@"update UserRole set RoleId = '{model.RoleId}' where UserId = '{model.Id}'", con);
                cmd.ExecuteNonQuery();

            }


            con.Close();
            return edit;
        }

        public bool DostEmailExist(string email)
        {
            bool existEmail = false;
            con.Open();
            cmd = new SqlCommand($@"select * from Users where EmailAddress = '{email}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                existEmail = true;

            }
            con.Close();
            return existEmail;
        }
        public bool DoesUserNameExist(string UserName)
        {
            bool existEmail = false;
            con.Open();
            cmd = new SqlCommand($@"select * from Users where UserName = '{UserName}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                existEmail = true;

            }
            con.Close();
            return existEmail;
        }

        public string DeleteUser(int userId)
        {
            con.Open();
            cmd = new SqlCommand($@"delete from Users where Id = '{userId}'", con);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand($@"delete from UserRole where UserId = '{userId}'", con);
            cmd.ExecuteNonQuery();
            con.Close();

            return "Record Delete Sucessfull...";

        }




    }
}