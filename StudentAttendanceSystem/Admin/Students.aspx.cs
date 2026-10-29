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
    public partial class Students : System.Web.UI.Page
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

                LoadStudents();
            }
        }
        private void LoadClasses()
        {
            using (SqlConnection connection =
                   new SqlConnection(conn))
            {
                string query = @"
                    SELECT ClassId, ClassName
                    FROM Classes
                    WHERE IsActive = 1
                    ORDER BY ClassName";


                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter =
                        new SqlDataAdapter(command);

                    DataTable table =
                        new DataTable();

                    adapter.Fill(table);


                    ddlClass.DataSource = table;

                    ddlClass.DataTextField =
                        "ClassName";

                    ddlClass.DataValueField =
                        "ClassId";

                    ddlClass.DataBind();
                }
            }


            ddlClass.Items.Insert(
                0,
                new System.Web.UI.WebControls.ListItem(
                    "-- Select Class --",
                    ""));
        }


        // Load all students
        private void LoadStudents()
        {
            using (SqlConnection connection =
                   new SqlConnection(conn))
            {
                string query = @"
                    SELECT
                        S.StudentId,
                        S.RollNo,
                        S.StudentName,
                        S.Email,
                        S.Mobile,
                        S.Username,
                        S.ClassId,
                        C.ClassName,
                        S.IsActive
                    FROM Students S
                    INNER JOIN Classes C
                        ON S.ClassId = C.ClassId
                    WHERE S.IsActive = 1
                    ORDER BY S.StudentId DESC";


                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter =
                        new SqlDataAdapter(command);

                    DataTable table =
                        new DataTable();

                    adapter.Fill(table);


                    gvStudents.DataSource =
                        table;

                    gvStudents.DataBind();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();

            lblMessage.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string rollNo =
                txtRollNo.Text.Trim();

            string studentName =
                txtStudentName.Text.Trim();

            string email =
                txtEmail.Text.Trim();

            string mobile =
                txtMobile.Text.Trim();

            string classId =
                ddlClass.SelectedValue;

            string username =
                txtUsername.Text.Trim();

            string password =
                txtPassword.Text.Trim();


            // Validation

            if (rollNo == "")
            {
                lblMessage.Text =
                    "Please enter roll number.";

                return;
            }


            if (studentName == "")
            {
                lblMessage.Text =
                    "Please enter student name.";

                return;
            }


            if (classId == "")
            {
                lblMessage.Text =
                    "Please select class.";

                return;
            }


            if (username == "")
            {
                lblMessage.Text =
                    "Please enter username.";

                return;
            }


            // Password is required only while adding
            if (hfStudentId.Value == ""
                && password == "")
            {
                lblMessage.Text =
                    "Please enter password.";

                return;
            }


            if (hfStudentId.Value == "")
            {
                AddStudent(
                    rollNo,
                    studentName,
                    email,
                    mobile,
                    Convert.ToInt32(classId),
                    username,
                    password);
            }
            else
            {
                UpdateStudent(
                    rollNo,
                    studentName,
                    email,
                    mobile,
                    Convert.ToInt32(classId),
                    username,
                    password);
            }
        }
        private void AddStudent(
            string rollNo,
            string studentName,
            string email,
            string mobile,
            int classId,
            string username,
            string password)
        {
            using (SqlConnection connection =
                   new SqlConnection(conn))
            {
                connection.Open();


                // Check duplicate roll number
                string rollCheckQuery = @"
                    SELECT COUNT(*)
                    FROM Students
                    WHERE RollNo = @RollNo
                    AND ClassId = @ClassId";


                using (SqlCommand command =
                       new SqlCommand(
                           rollCheckQuery,
                           connection))
                {
                    command.Parameters.AddWithValue(
                        "@RollNo",
                        rollNo);

                    command.Parameters.AddWithValue(
                        "@ClassId",
                        classId);


                    int count =
                        Convert.ToInt32(
                            command.ExecuteScalar());


                    if (count > 0)
                    {
                        lblMessage.Text =
                            "This roll number already exists in the selected class.";

                        return;
                    }
                }


                // Check duplicate username
                string usernameCheckQuery = @"
                    SELECT COUNT(*)
                    FROM Students
                    WHERE Username = @Username";


                using (SqlCommand command =
                       new SqlCommand(
                           usernameCheckQuery,
                           connection))
                {
                    command.Parameters.AddWithValue(
                        "@Username",
                        username);


                    int count =
                        Convert.ToInt32(
                            command.ExecuteScalar());


                    if (count > 0)
                    {
                        lblMessage.Text =
                            "This username is already in use.";

                        return;
                    }
                }


                // Insert student
                string insertQuery = @"
                    INSERT INTO Students
                    (
                        RollNo,
                        StudentName,
                        Email,
                        Mobile,
                        ClassId,
                        Username,
                        Password,
                        IsActive
                    )
                    VALUES
                    (
                        @RollNo,
                        @StudentName,
                        @Email,
                        @Mobile,
                        @ClassId,
                        @Username,
                        @Password,
                        1
                    )";


                using (SqlCommand command =
                       new SqlCommand(
                           insertQuery,
                           connection))
                {
                    command.Parameters.AddWithValue(
                        "@RollNo",
                        rollNo);

                    command.Parameters.AddWithValue(
                        "@StudentName",
                        studentName);

                    command.Parameters.AddWithValue(
                        "@Email",
                        email);

                    command.Parameters.AddWithValue(
                        "@Mobile",
                        mobile);

                    command.Parameters.AddWithValue(
                        "@ClassId",
                        classId);

                    command.Parameters.AddWithValue(
                        "@Username",
                        username);

                    command.Parameters.AddWithValue(
                        "@Password",
                        password);


                    command.ExecuteNonQuery();
                }
            }


            lblMessage.Text =
                "Student added successfully.";

            ClearForm();

            LoadStudents();
        }


        // Update student
        private void UpdateStudent(
            string rollNo,
            string studentName,
            string email,
            string mobile,
            int classId,
            string username,
            string password)
        {
            int studentId =
                Convert.ToInt32(
                    hfStudentId.Value);


            using (SqlConnection connection =
                   new SqlConnection(conn))
            {
                connection.Open();


                // Check roll number
                string rollCheckQuery = @"
                    SELECT COUNT(*)
                    FROM Students
                    WHERE RollNo = @RollNo
                    AND ClassId = @ClassId
                    AND StudentId <> @StudentId";


                using (SqlCommand command =
                       new SqlCommand(
                           rollCheckQuery,
                           connection))
                {
                    command.Parameters.AddWithValue(
                        "@RollNo",
                        rollNo);

                    command.Parameters.AddWithValue(
                        "@ClassId",
                        classId);

                    command.Parameters.AddWithValue(
                        "@StudentId",
                        studentId);


                    int count =
                        Convert.ToInt32(
                            command.ExecuteScalar());


                    if (count > 0)
                    {
                        lblMessage.Text =
                            "This roll number already exists in the selected class.";

                        return;
                    }
                }


                // Check username
                string usernameCheckQuery = @"
                    SELECT COUNT(*)
                    FROM Students
                    WHERE Username = @Username
                    AND StudentId <> @StudentId";


                using (SqlCommand command =
                       new SqlCommand(
                           usernameCheckQuery,
                           connection))
                {
                    command.Parameters.AddWithValue(
                        "@Username",
                        username);

                    command.Parameters.AddWithValue(
                        "@StudentId",
                        studentId);


                    int count =
                        Convert.ToInt32(
                            command.ExecuteScalar());


                    if (count > 0)
                    {
                        lblMessage.Text =
                            "This username is already in use.";

                        return;
                    }
                }


                // Update student
                string updateQuery;


                if (password == "")
                {
                    updateQuery = @"
                        UPDATE Students
                        SET
                            RollNo = @RollNo,
                            StudentName = @StudentName,
                            Email = @Email,
                            Mobile = @Mobile,
                            ClassId = @ClassId,
                            Username = @Username
                        WHERE StudentId = @StudentId";
                }
                else
                {
                    updateQuery = @"
                        UPDATE Students
                        SET
                            RollNo = @RollNo,
                            StudentName = @StudentName,
                            Email = @Email,
                            Mobile = @Mobile,
                            ClassId = @ClassId,
                            Username = @Username,
                            Password = @Password
                        WHERE StudentId = @StudentId";
                }


                using (SqlCommand command =
                       new SqlCommand(
                           updateQuery,
                           connection))
                {
                    command.Parameters.AddWithValue(
                        "@RollNo",
                        rollNo);

                    command.Parameters.AddWithValue(
                        "@StudentName",
                        studentName);

                    command.Parameters.AddWithValue(
                        "@Email",
                        email);

                    command.Parameters.AddWithValue(
                        "@Mobile",
                        mobile);

                    command.Parameters.AddWithValue(
                        "@ClassId",
                        classId);

                    command.Parameters.AddWithValue(
                        "@Username",
                        username);

                    command.Parameters.AddWithValue(
                        "@StudentId",
                        studentId);


                    if (password != "")
                    {
                        command.Parameters.AddWithValue(
                            "@Password",
                            password);
                    }


                    command.ExecuteNonQuery();
                }
            }


            lblMessage.Text =
                "Student updated successfully.";

            ClearForm();

            LoadStudents();
        }

        protected void gvStudents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int studentId =
                Convert.ToInt32(
                    e.CommandArgument);


            if (e.CommandName ==
                "EditStudent")
            {
                EditStudent(studentId);
            }


            if (e.CommandName ==
                "DeleteStudent")
            {
                DeleteStudent(studentId);
            }
        }
        private void EditStudent(int studentId)
        {
            using (SqlConnection connection =
                   new SqlConnection(conn))
            {
                string query = @"
                    SELECT
                        StudentId,
                        RollNo,
                        StudentName,
                        Email,
                        Mobile,
                        ClassId,
                        Username
                    FROM Students
                    WHERE StudentId = @StudentId";


                using (SqlCommand command =
                       new SqlCommand(
                           query,
                           connection))
                {
                    command.Parameters.AddWithValue(
                        "@StudentId",
                        studentId);


                    connection.Open();


                    SqlDataReader reader =
                        command.ExecuteReader();


                    if (reader.Read())
                    {
                        hfStudentId.Value =
                            reader["StudentId"].ToString();

                        txtRollNo.Text =
                            reader["RollNo"].ToString();

                        txtStudentName.Text =
                            reader["StudentName"].ToString();

                        txtEmail.Text =
                            reader["Email"].ToString();

                        txtMobile.Text =
                            reader["Mobile"].ToString();

                        ddlClass.SelectedValue =
                            reader["ClassId"].ToString();

                        txtUsername.Text =
                            reader["Username"].ToString();

                        // Don't load old password
                        txtPassword.Text = "";

                        btnSave.Text =
                            "Update Student";

                        btnCancel.Visible =
                            true;

                        lblMessage.Text = "";
                    }


                    reader.Close();
                }
            }
        }


        // Deactivate student
        private void DeleteStudent(int studentId)
        {
            using (SqlConnection connection =
                   new SqlConnection(conn))
            {
                string query = @"
                    UPDATE Students
                    SET IsActive = 0
                    WHERE StudentId = @StudentId";


                using (SqlCommand command =
                       new SqlCommand(
                           query,
                           connection))
                {
                    command.Parameters.AddWithValue(
                        "@StudentId",
                        studentId);


                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }


            lblMessage.Text =
                "Student deactivated successfully.";

            LoadStudents();
        }


        // Clear form
        private void ClearForm()
        {
            hfStudentId.Value = "";

            txtRollNo.Text = "";

            txtStudentName.Text = "";

            txtEmail.Text = "";

            txtMobile.Text = "";

            txtUsername.Text = "";

            txtPassword.Text = "";


            if (ddlClass.Items.Count > 0)
            {
                ddlClass.SelectedIndex = 0;
            }


            btnSave.Text =
                "Add Student";

            btnCancel.Visible =
                false;
        }
    }
}