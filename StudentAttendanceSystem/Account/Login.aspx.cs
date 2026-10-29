using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentAttendanceSystem.Account
{
    public partial class Login : System.Web.UI.Page
    {
        string Conn = ConfigurationManager.ConnectionStrings["StudentAttendanceConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (isValid())
                {
                    string username = txtUsername.Text.Trim();
                    string password = txtPassword.Text.Trim();
                    string sql = "Select UserId,FullName,Role from Users where Username=@Username and Password=@Password and IsActive=1";
                    using (SqlConnection con = new SqlConnection(Conn))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@Username", username);
                            cmd.Parameters.AddWithValue("@Password", password);
                            using (SqlDataReader sdr = cmd.ExecuteReader())
                            {
                                if (sdr.Read())
                                {
                                    Session["UserId"] = sdr["UserId"].ToString();
                                    Session["FullName"] = sdr["FullName"].ToString();
                                    Session["Role"] = sdr["Role"].ToString();

                                    Response.Redirect("~/Admin/Dashboard.aspx");
                                }
                                else
                                {
                                    sdr.Close();
                                    LoginStudent(username, password);
                                }
                            }
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void LoginStudent(string username, string password)
        {
            try
            {
                string stusql = "Select StudentId,StudentName,ClassId from Students where Username=@username and Password=@Password and IsActive=1";
                using(SqlConnection con=new SqlConnection(Conn))
                {
                    con.Open();
                    using(SqlCommand cmd= new SqlCommand(stusql, con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if(sdr.Read())
                            {
                                Session["StudentId"] = sdr["StudentId"].ToString();
                                Session["StudentName"] = sdr["StudentName"].ToString();
                                Session["ClassId"] = sdr["ClassId"].ToString();
                                Session["Role"] = "Student";

                                Response.Redirect("~/Student/Dashboard.aspx");
                            }
                            else
                            {
                                lblMessage.Text = "Invalid Username or Password.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text= ex.Message;
            }
        }

        private bool isValid()
        {
            if(txtUsername.Text.Trim()==""||txtPassword.Text.Trim()=="")
            {
                lblMessage.Text = "Please enter username and password.";
                return false;
            }
            return true;
        }
    }
}