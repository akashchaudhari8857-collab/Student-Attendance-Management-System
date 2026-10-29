using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentAttendanceSystem.Admin
{
    public partial class Subjects : System.Web.UI.Page
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
                LoadSubjects();
            }
        }
        private void LoadClasses()
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                string query = @"SELECT ClassId, ClassName FROM Classes WHERE IsActive = 1 ORDER BY ClassName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();

                    adapter.Fill(table);
                    ddlClass.DataSource = table;
                    ddlClass.DataTextField = "ClassName";
                    ddlClass.DataValueField = "ClassId";
                    ddlClass.DataBind();
                }
            }
            ddlClass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Class --", ""));
        }
        private void LoadSubjects()
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                string query = @"
                    SELECT S.SubjectId, S.SubjectName, S.ClassId, C.ClassName, S.IsActive FROM Subjects S
                    INNER JOIN Classes C ON S.ClassId = C.ClassId
                    ORDER BY S.SubjectId DESC";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    gvSubjects.DataSource = table;
                    gvSubjects.DataBind();
                }
            }
        }

        protected void gvSubjects_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int subjectId = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "EditSubject")
            {
                EditSubject(subjectId);
            }
            if (e.CommandName == "DeleteSubject")
            {
                DeleteSubject(subjectId);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            lblMessage.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string subjectName = txtSubjectName.Text.Trim();
            string classId = ddlClass.SelectedValue;

            if (subjectName == "")
            {
                lblMessage.Text = "Please enter subject name.";
                return;
            }
            if (classId == "")
            {
                lblMessage.Text = "Please select a class.";
                return;
            }
            if (hfSubjectId.Value == "")
            {
                AddSubject(subjectName, Convert.ToInt32(classId));
            }
            else
            {
                UpdateSubject(subjectName, Convert.ToInt32(classId));
            }
        }
        private void AddSubject(string subjectName, int classId)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                string checkQuery = @"SELECT COUNT(*) FROM Subjects WHERE SubjectName = @SubjectName AND ClassId = @ClassId";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@SubjectName", subjectName);
                    checkCommand.Parameters.AddWithValue("@ClassId", classId);
                    connection.Open();
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    connection.Close();
                    if (count > 0)
                    {
                        lblMessage.Text = "This subject already exists for the selected class.";

                        return;
                    }
                }
                string query = @"INSERT INTO Subjects (SubjectName, ClassId, IsActive ) VALUES (@SubjectName,@ClassId,1 )";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SubjectName", subjectName);
                    command.Parameters.AddWithValue("@ClassId", classId);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            lblMessage.Text = "Subject added successfully.";

            txtSubjectName.Text = "";

            ddlClass.SelectedIndex = 0;

            LoadSubjects();
        }
        private void UpdateSubject(string subjectName, int classId)
        {
            int subjectId = Convert.ToInt32(hfSubjectId.Value);


            using (SqlConnection connection = new SqlConnection(conn))
            {
                string query = @" UPDATE Subjects SET SubjectName = @SubjectName, ClassId = @ClassId WHERE SubjectId = @SubjectId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SubjectName", subjectName);

                    command.Parameters.AddWithValue("@ClassId", classId);

                    command.Parameters.AddWithValue("@SubjectId", subjectId);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            lblMessage.Text = "Subject updated successfully.";
            ClearForm();
            LoadSubjects();
        }
        private void EditSubject(int subjectId)
        {
            using (SqlConnection connection =
                   new SqlConnection(conn))
            {
                string query = @"
                    SELECT
                        SubjectId,
                        SubjectName,
                        ClassId
                    FROM Subjects
                    WHERE SubjectId = @SubjectId";


                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(
                        "@SubjectId",
                        subjectId);

                    connection.Open();

                    SqlDataReader reader =
                        command.ExecuteReader();


                    if (reader.Read())
                    {
                        hfSubjectId.Value =
                            reader["SubjectId"].ToString();

                        txtSubjectName.Text =
                            reader["SubjectName"].ToString();

                        ddlClass.SelectedValue =
                            reader["ClassId"].ToString();

                        btnSave.Text =
                            "Update Subject";

                        btnCancel.Visible =
                            true;

                        lblMessage.Text = "";
                    }


                    reader.Close();
                }
            }
        }


        // Deactivate subject
        private void DeleteSubject(int subjectId)
        {
            using (SqlConnection connection =
                   new SqlConnection(conn))
            {
                string query = @"
                    UPDATE Subjects
                    SET IsActive = 0
                    WHERE SubjectId = @SubjectId";


                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(
                        "@SubjectId",
                        subjectId);

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }


            lblMessage.Text =
                "Subject deactivated successfully.";

            LoadSubjects();
        }


        // Clear form
        private void ClearForm()
        {
            hfSubjectId.Value = "";

            txtSubjectName.Text = "";

            if (ddlClass.Items.Count > 0)
            {
                ddlClass.SelectedIndex = 0;
            }

            btnSave.Text =
                "Add Subject";

            btnCancel.Visible =
                false;
        }
    }
}