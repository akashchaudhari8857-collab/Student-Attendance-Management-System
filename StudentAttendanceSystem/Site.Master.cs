using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentAttendanceSystem
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            phAdminMenu.Visible = false;
            phStudentMenu.Visible = false;


            // Check Admin
            if (Session["UserId"] != null && Session["Role"] != null && Session["Role"].ToString() == "Admin")
            {
                phAdminMenu.Visible = true;

                lblUserName.Text = Session["FullName"].ToString();
            }


            // Check Student
            else if (Session["StudentId"] != null && Session["Role"] != null && Session["Role"].ToString() == "Student")
            {
                phStudentMenu.Visible = true;

                lblUserName.Text = Session["StudentName"].ToString();
            }

            // If no user is logged in
            else
            {
                Response.Redirect("~/Account/Login.aspx");
            }
        }
    }
}