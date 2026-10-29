using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentAttendanceSystem.Admin
{
    public partial class Classes : System.Web.UI.Page
    {
        string conn = ConfigurationManager.ConnectionStrings["StudentAttendanceConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null || Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                LoadClasses();
            }

        }

        private void LoadClasses()
        {
            try
            {
                string sql = "Select ClassId, ClassName, IsActive from Classes order by ClassId desc";
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvClasses.DataSource = dt;
                        gvClasses.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text= ex.Message;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string className = txtClassName.Text.Trim();

            if (className == "")
            {
                lblMessage.Text = "Please enter class name.";
                return;
            }

            if (hfClassId.Value == "")
            {
                AddClass(className);
            }
            else
            {
                UpdateClass(className);
            }
        }

        private void UpdateClass(string className)
        {
            int classId = Convert.ToInt32(hfClassId.Value);
            using (SqlConnection connection = new SqlConnection(conn))
            {
                string query = @"UPDATE Classes SET ClassName = @ClassName WHERE ClassId = @ClassId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassName",className);
                    command.Parameters.AddWithValue("@ClassId",classId);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            lblMessage.Text = "Class updated successfully.";
            hfClassId.Value = "";
            txtClassName.Text = "";
            btnSave.Text = "Add Class";
            btnCancel.Visible = false;

            LoadClasses();
        }

        private void AddClass(string className)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                string checkQuery = @"SELECT COUNT(*) FROM Classes WHERE ClassName = @ClassName";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@ClassName",className);
                    connection.Open();
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                    connection.Close();

                    if (count > 0)
                    {
                        lblMessage.Text = "This class already exists.";
                        return;
                    }
                }
                string query = @"INSERT INTO Classes (ClassName, IsActive) VALUES (@ClassName,1)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassName", className);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }


            lblMessage.Text = "Class added successfully.";

            txtClassName.Text = "";

            LoadClasses();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            hfClassId.Value = "";

            txtClassName.Text = "";

            btnSave.Text = "Add Class";

            btnCancel.Visible = false;

            lblMessage.Text = "";
        }

        protected void gvClasses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int classId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditClass")
            {
                EditClass(classId);
            }
            if (e.CommandName == "DeleteClass")
            {
                DeleteClass(classId);
            }
            else if (e.CommandName == "ActivateClass")
            {

                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = @"
            UPDATE Classes
            SET IsActive = 1
            WHERE ClassId = @ClassId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ClassId", classId);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                lblMessage.Text = "Class activated successfully.";
                lblMessage.ForeColor = System.Drawing.Color.Green;

                LoadClasses();
            }
        }

        private void DeleteClass(int classId)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                string query = @"UPDATE Classes SET IsActive = 0 WHERE ClassId = @ClassId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassId",classId);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            lblMessage.Text = "Class deactivated successfully.";

            LoadClasses();
        }

        private void EditClass(int classId)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                string query = @"SELECT ClassId, ClassName FROM Classes WHERE ClassId = @ClassId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassId",classId);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        hfClassId.Value = reader["ClassId"].ToString();
                        txtClassName.Text = reader["ClassName"].ToString();
                        btnSave.Text = "Update Class";
                        btnCancel.Visible = true;
                        lblMessage.Text = "";
                    }

                    reader.Close();
                }
            }
        }
    }
}