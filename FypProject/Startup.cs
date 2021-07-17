using FypProject.DB;
using Microsoft.Owin;
using Owin;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

[assembly: OwinStartupAttribute(typeof(FypProject.Startup))]
namespace FypProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            FirstRunSetup();
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["fyp"].ToString());
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        private void FirstRunSetup()
        {
            con.Open();
            //cmd = new SqlCommand($@"select * from Roles where RoleName = '{constant.Roles.Admin}'", con);
            //cmd.CommandType = CommandType.Text;
            //sda = new SqlDataAdapter(cmd);
            //dt = new DataTable();
            //sda.Fill(dt);
            //if (dt.Rows.Count <= 0)
            //{
            //    cmd = new SqlCommand($@"insert into Roles (RoleName) values ('{constant.Roles.Admin}')", con);
            //    cmd.ExecuteNonQuery();
            //}


            cmd = new SqlCommand($@"select * from Roles where RoleName = '{constant.Roles.SuperAdmin}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count <= 0)
            {
                cmd = new SqlCommand($@"insert into Roles (RoleName) values ('{constant.Roles.SuperAdmin}')", con);
                cmd.ExecuteNonQuery();
            }

            cmd = new SqlCommand($@"select * from Roles where RoleName = '{constant.Roles.User}'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count <= 0)
            {
                cmd = new SqlCommand($@"insert into Roles (RoleName) values ('{constant.Roles.User}')", con);
                cmd.ExecuteNonQuery();
            }


            cmd = new SqlCommand($@"select * from Users where UserName = 'superadmin'", con);
            cmd.CommandType = CommandType.Text;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count <= 0)
            {
                cmd = new SqlCommand($@"insert into Users (UserName,FirstName,LastName,EmailAddress,Password,RecordAt) values ('superadmin','application','admin','admin@gmail.com','superAdmin',GetDate())", con);
                cmd.ExecuteNonQuery();


                cmd = new SqlCommand($@"select Id from Users where UserName = 'superadmin'", con);
                cmd.CommandType = CommandType.Text;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    cmd = new SqlCommand($@"select RoleId from Roles where RoleName = '{constant.Roles.SuperAdmin}'", con);
                    cmd.CommandType = CommandType.Text;
                    sda = new SqlDataAdapter(cmd);
                    DataTable dtt = new DataTable();
                    sda.Fill(dtt);
                    if (dtt.Rows.Count > 0)
                    {
                        cmd = new SqlCommand($@"insert into UserRole (RoleId,UserId) values ('{dtt.Rows[0]["RoleId"]}','{dt.Rows[0]["Id"]}')", con);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            con.Close();



        }
    }
}
