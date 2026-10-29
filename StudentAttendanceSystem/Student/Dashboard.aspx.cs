using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace StudentAttendanceSystem.Student
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string conn = ConfigurationManager.ConnectionStrings["StudentAttendanceConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check Student login
            if (Session["StudentId"] == null || Session["Role"] == null || Session["Role"].ToString() != "Student")
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }


            if (!IsPostBack)
            {
                LoadStudentProfile();

                LoadOverallAttendance();

                LoadSubjectWiseAttendance();
            }
        }
        private void LoadStudentProfile()
        {
            int studentId = Convert.ToInt32(Session["StudentId"]);
            using (SqlConnection con = new SqlConnection(conn))
            {
                string query = @"
                    SELECT
                        S.StudentName,
                        S.RollNo,
                        C.ClassName
                    FROM Students S
                    INNER JOIN Classes C
                        ON S.ClassId = C.ClassId
                    WHERE S.StudentId = @StudentId
                    AND S.IsActive = 1
                ";


                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@StudentId",
                        studentId
                    );


                    con.Open();


                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblStudentName.Text =
                                reader["StudentName"].ToString();

                            lblRollNo.Text =
                                reader["RollNo"].ToString();

                            lblClassName.Text =
                                reader["ClassName"].ToString();
                        }
                    }
                }
            }
        }


        // ==========================================
        // LOAD OVERALL ATTENDANCE
        // ==========================================

        private void LoadOverallAttendance()
        {
            int studentId =
                Convert.ToInt32(
                    Session["StudentId"]
                );


            using (SqlConnection con =
                new SqlConnection(conn))
            {
                string query = @"
                    SELECT

                        COUNT(AD.AttendanceDetailId)
                            AS TotalClasses,

                        SUM(
                            CASE
                                WHEN AD.Status = 'Present'
                                THEN 1
                                ELSE 0
                            END
                        ) AS Present,

                        SUM(
                            CASE
                                WHEN AD.Status = 'Absent'
                                THEN 1
                                ELSE 0
                            END
                        ) AS Absent

                    FROM AttendanceDetails AD

                    WHERE AD.StudentId = @StudentId
                ";


                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@StudentId",
                        studentId
                    );


                    con.Open();


                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int totalClasses =
                                reader["TotalClasses"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(
                                        reader["TotalClasses"]
                                    );


                            int present =
                                reader["Present"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(
                                        reader["Present"]
                                    );


                            int absent =
                                reader["Absent"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(
                                        reader["Absent"]
                                    );


                            decimal percentage = 0;


                            if (totalClasses > 0)
                            {
                                percentage =
                                    (present * 100m)
                                    / totalClasses;
                            }


                            lblTotalClasses.Text =
                                totalClasses.ToString();


                            lblPresent.Text =
                                present.ToString();


                            lblAbsent.Text =
                                absent.ToString();


                            lblPercentage.Text =
                                percentage.ToString("0.00")
                                + "%";
                        }
                    }
                }
            }
        }


        // ==========================================
        // LOAD SUBJECT-WISE ATTENDANCE
        // ==========================================

        private void LoadSubjectWiseAttendance()
        {
            int studentId =
                Convert.ToInt32(
                    Session["StudentId"]
                );


            using (SqlConnection con =
                new SqlConnection(conn))
            {
                string query = @"

                    SELECT

                        SU.SubjectName,

                        COUNT(
                            AD.AttendanceDetailId
                        ) AS TotalClasses,

                        SUM(
                            CASE
                                WHEN AD.Status = 'Present'
                                THEN 1
                                ELSE 0
                            END
                        ) AS Present,

                        SUM(
                            CASE
                                WHEN AD.Status = 'Absent'
                                THEN 1
                                ELSE 0
                            END
                        ) AS Absent,

                        CAST(

                            (
                                SUM(
                                    CASE
                                        WHEN AD.Status = 'Present'
                                        THEN 1
                                        ELSE 0
                                    END
                                ) * 100.0

                                /

                                NULLIF(
                                    COUNT(
                                        AD.AttendanceDetailId
                                    ),
                                    0
                                )
                            )

                            AS DECIMAL(5,2)

                        ) AS AttendancePercentage

                    FROM AttendanceDetails AD

                    INNER JOIN Attendance A
                        ON AD.AttendanceId =
                           A.AttendanceId

                    INNER JOIN Subjects SU
                        ON A.SubjectId =
                           SU.SubjectId

                    WHERE AD.StudentId = @StudentId

                    GROUP BY
                        SU.SubjectId,
                        SU.SubjectName

                    ORDER BY
                        SU.SubjectName

                ";


                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@StudentId",
                        studentId
                    );


                    using (SqlDataAdapter da =
                        new SqlDataAdapter(cmd))
                    {
                        DataTable dt =
                            new DataTable();


                        da.Fill(dt);


                        gvAttendance.DataSource =
                            dt;

                        gvAttendance.DataBind();
                    }
                }
            }
        }
    }
}