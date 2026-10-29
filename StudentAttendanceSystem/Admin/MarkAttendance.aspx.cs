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
    public partial class MarkAttendance : System.Web.UI.Page
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

                // Set today's date
                txtAttendanceDate.Text =
                    DateTime.Today.ToString("yyyy-MM-dd");

                // Initially disable subject until class is selected
                ddlSubject.Items.Clear();

                ddlSubject.Items.Add(
                    new System.Web.UI.WebControls.ListItem(
                        "-- Select Subject --",
                        ""
                    )
                );

                gvStudents.DataSource = null;
                gvStudents.DataBind();
                btnSaveAttendance.Enabled = false;
            }
        }
        private void LoadClasses()
        {
            ddlClass.Items.Clear();

            ddlClass.Items.Add(
                new System.Web.UI.WebControls.ListItem(
                    "-- Select Class --",
                    ""
                )
            );


            using (SqlConnection con =
                new SqlConnection(conn))
            {
                string query = @"
                    SELECT ClassId, ClassName
                    FROM Classes
                    WHERE IsActive = 1
                    ORDER BY ClassName
                ";


                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    con.Open();


                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ddlClass.Items.Add(
                                new System.Web.UI.WebControls.ListItem(
                                    reader["ClassName"].ToString(),
                                    reader["ClassId"].ToString()
                                )
                            );
                        }
                    }
                }
            }
        }

        protected void btnSaveAttendance_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";


            // Validate class
            if (string.IsNullOrEmpty(ddlClass.SelectedValue))
            {
                lblMessage.Text =
                    "Please select a class.";

                return;
            }


            // Validate subject
            if (string.IsNullOrEmpty(ddlSubject.SelectedValue))
            {
                lblMessage.Text =
                    "Please select a subject.";

                return;
            }


            // Validate date
            if (string.IsNullOrEmpty(txtAttendanceDate.Text))
            {
                lblMessage.Text =
                    "Please select attendance date.";

                return;
            }


            if (gvStudents.Rows.Count == 0)
            {
                lblMessage.Text =
                    "Please load students first.";

                return;
            }


            int classId =
                Convert.ToInt32(ddlClass.SelectedValue);


            int subjectId =
                Convert.ToInt32(ddlSubject.SelectedValue);


            DateTime attendanceDate;

            if (!DateTime.TryParse(
                txtAttendanceDate.Text,
                out attendanceDate))
            {
                lblMessage.Text =
                    "Invalid attendance date.";

                return;
            }


            // Check subject belongs to class
            if (!IsSubjectBelongsToClass(
                subjectId,
                classId))
            {
                lblMessage.Text =
                    "Selected subject does not belong to selected class.";

                return;
            }


            // Check duplicate attendance
            if (AttendanceAlreadyExists(subjectId,attendanceDate))
            {
                lblMessage.Text =
                    "Attendance for this subject and date already exists.";

                return;
            }


            using (SqlConnection con =
                new SqlConnection(conn))
            {
                con.Open();


                // Start transaction
                SqlTransaction transaction =
                    con.BeginTransaction();


                try
                {
                    // ==========================================
                    // INSERT ATTENDANCE HEADER
                    // ==========================================

                    string attendanceQuery = @"
                        INSERT INTO Attendance
                        (
                            SubjectId,
                            AttendanceDate
                        )
                        VALUES
                        (
                            @SubjectId,
                            @AttendanceDate
                        );

                        SELECT SCOPE_IDENTITY();
                    ";


                    int attendanceId;


                    using (SqlCommand cmd =
                        new SqlCommand(
                            attendanceQuery,
                            con,
                            transaction))
                    {
                        cmd.Parameters.AddWithValue(
                            "@SubjectId",
                            subjectId
                        );

                        cmd.Parameters.AddWithValue(
                            "@AttendanceDate",
                            attendanceDate
                        );


                        attendanceId =
                            Convert.ToInt32(
                                cmd.ExecuteScalar()
                            );
                    }


                    // ==========================================
                    // INSERT EACH STUDENT ATTENDANCE
                    // ==========================================

                    foreach (System.Web.UI.WebControls.GridViewRow row
                        in gvStudents.Rows)
                    {
                        int studentId =
                            Convert.ToInt32(
                                gvStudents.DataKeys[
                                    row.RowIndex
                                ].Value
                            );


                        System.Web.UI.WebControls.DropDownList ddlStatus =
                            (System.Web.UI.WebControls.DropDownList)
                            row.FindControl("ddlStatus");


                        string status =
                            ddlStatus.SelectedValue;


                        string detailQuery = @"
                            INSERT INTO AttendanceDetails
                            (
                                AttendanceId,
                                StudentId,
                                Status
                            )
                            VALUES
                            (
                                @AttendanceId,
                                @StudentId,
                                @Status
                            )
                        ";


                        using (SqlCommand cmd =
                            new SqlCommand(
                                detailQuery,
                                con,
                                transaction))
                        {
                            cmd.Parameters.AddWithValue(
                                "@AttendanceId",
                                attendanceId
                            );

                            cmd.Parameters.AddWithValue(
                                "@StudentId",
                                studentId
                            );

                            cmd.Parameters.AddWithValue(
                                "@Status",
                                status
                            );


                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();


                    lblMessage.Text =
                        "Attendance saved successfully.";

                    lblMessage.ForeColor =
                        System.Drawing.Color.Green;


                    gvStudents.DataSource = null;
                    gvStudents.DataBind();
                    btnSaveAttendance.Enabled = false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();


                    lblMessage.Text =
                        "Error while saving attendance: "
                        + ex.Message;

                    lblMessage.ForeColor =
                        System.Drawing.Color.Red;
                }
            }
        }

        protected void btnLoadStudents_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";


            // Validate class
            if (string.IsNullOrEmpty(ddlClass.SelectedValue))
            {
                lblMessage.Text = "Please select a class.";
                return;
            }


            // Validate subject
            if (string.IsNullOrEmpty(ddlSubject.SelectedValue))
            {
                lblMessage.Text = "Please select a subject.";
                return;
            }


            // Validate date
            if (string.IsNullOrEmpty(txtAttendanceDate.Text))
            {
                lblMessage.Text = "Please select attendance date.";
                return;
            }


            int classId =
                Convert.ToInt32(ddlClass.SelectedValue);


            int subjectId =
                Convert.ToInt32(ddlSubject.SelectedValue);


            // Check that subject belongs to selected class
            if (!IsSubjectBelongsToClass(
                subjectId,
                classId))
            {
                lblMessage.Text =
                    "Selected subject does not belong to selected class.";

                return;
            }


            LoadStudents(classId);


            if (gvStudents.Rows.Count == 0)
            {
                lblMessage.Text =
                    "No active students found in this class.";

                btnSaveAttendance.Enabled = false;
                return;
            }


            btnSaveAttendance.Enabled = true;
        }

        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            gvStudents.DataSource = null;
            gvStudents.DataBind();


            if (string.IsNullOrEmpty(ddlClass.SelectedValue))
            {
                ddlSubject.Items.Clear();

                ddlSubject.Items.Add(
                    new System.Web.UI.WebControls.ListItem(
                        "-- Select Subject --",
                        ""
                    )
                );

                return;
            }


            LoadSubjects();
        }
        private void LoadSubjects()
        {
            ddlSubject.Items.Clear();

            ddlSubject.Items.Add(
                new System.Web.UI.WebControls.ListItem(
                    "-- Select Subject --",
                    ""
                )
            );


            int classId =
                Convert.ToInt32(ddlClass.SelectedValue);


            using (SqlConnection con =
                new SqlConnection(conn))
            {
                string query = @"
                    SELECT SubjectId, SubjectName
                    FROM Subjects
                    WHERE ClassId = @ClassId
                    AND IsActive = 1
                    ORDER BY SubjectName
                ";


                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@ClassId",
                        classId
                    );


                    con.Open();


                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ddlSubject.Items.Add(
                                new System.Web.UI.WebControls.ListItem(
                                    reader["SubjectName"].ToString(),
                                    reader["SubjectId"].ToString()
                                )
                            );
                        }
                    }
                }
            }
        }
        private bool IsSubjectBelongsToClass(
            int subjectId,
            int classId)
        {
            using (SqlConnection con =
                new SqlConnection(conn))
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM Subjects
                    WHERE SubjectId = @SubjectId
                    AND ClassId = @ClassId
                    AND IsActive = 1
                ";


                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@SubjectId",
                        subjectId
                    );

                    cmd.Parameters.AddWithValue(
                        "@ClassId",
                        classId
                    );


                    con.Open();


                    int count =
                        Convert.ToInt32(cmd.ExecuteScalar());


                    return count > 0;
                }
            }
        }


        // ==========================================
        // LOAD STUDENTS FOR SELECTED CLASS
        // ==========================================

        private void LoadStudents(int classId)
        {
            using (SqlConnection con =
                new SqlConnection(conn))
            {
                string query = @"
                    SELECT
                        StudentId,
                        RollNo,
                        StudentName
                    FROM Students
                    WHERE ClassId = @ClassId
                    AND IsActive = 1
                    ORDER BY RollNo
                ";


                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@ClassId",
                        classId
                    );


                    using (SqlDataAdapter da =
                        new SqlDataAdapter(cmd))
                    {
                        DataTable dt =
                            new DataTable();

                        da.Fill(dt);


                        gvStudents.DataSource = dt;

                        gvStudents.DataBind();
                    }
                }
            }
        }
        private bool AttendanceAlreadyExists(
            int subjectId,
            DateTime attendanceDate)
        {
            using (SqlConnection con =
                new SqlConnection(conn))
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM Attendance
                    WHERE SubjectId = @SubjectId
                    AND AttendanceDate = @AttendanceDate
                ";


                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@SubjectId",
                        subjectId
                    );

                    cmd.Parameters.AddWithValue(
                        "@AttendanceDate",
                        attendanceDate.Date
                    );


                    con.Open();


                    int count =
                        Convert.ToInt32(
                            cmd.ExecuteScalar()
                        );


                    return count > 0;
                }
            }
        }
    }
}