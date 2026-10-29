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
    public partial class AttendanceReport : System.Web.UI.Page
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

                // Set default date range
                txtFromDate.Text =
                    DateTime.Today.AddDays(-30)
                    .ToString("yyyy-MM-dd");

                txtToDate.Text =
                    DateTime.Today
                    .ToString("yyyy-MM-dd");


                ddlSubject.Items.Clear();

                ddlSubject.Items.Add(
                    new System.Web.UI.WebControls.ListItem(
                        "-- Select Subject --",
                        ""
                    )
                );
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

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";


            // Validate class

            if (string.IsNullOrEmpty(
                ddlClass.SelectedValue))
            {
                lblMessage.Text =
                    "Please select a class.";

                return;
            }


            // Validate subject

            if (string.IsNullOrEmpty(
                ddlSubject.SelectedValue))
            {
                lblMessage.Text =
                    "Please select a subject.";

                return;
            }


            // Validate dates

            if (string.IsNullOrEmpty(
                txtFromDate.Text))
            {
                lblMessage.Text =
                    "Please select From Date.";

                return;
            }


            if (string.IsNullOrEmpty(
                txtToDate.Text))
            {
                lblMessage.Text =
                    "Please select To Date.";

                return;
            }


            DateTime fromDate;

            DateTime toDate;


            if (!DateTime.TryParse(
                txtFromDate.Text,
                out fromDate))
            {
                lblMessage.Text =
                    "Invalid From Date.";

                return;
            }


            if (!DateTime.TryParse(
                txtToDate.Text,
                out toDate))
            {
                lblMessage.Text =
                    "Invalid To Date.";

                return;
            }


            // Check date range

            if (fromDate > toDate)
            {
                lblMessage.Text =
                    "From Date cannot be greater than To Date.";

                return;
            }


            int classId =
                Convert.ToInt32(
                    ddlClass.SelectedValue
                );


            int subjectId =
                Convert.ToInt32(
                    ddlSubject.SelectedValue
                );


            // Check subject belongs to class

            if (!IsSubjectBelongsToClass(
                subjectId,
                classId))
            {
                lblMessage.Text =
                    "Selected subject does not belong to selected class.";

                return;
            }


            LoadAttendanceReport(
                classId,
                subjectId,
                fromDate,
                toDate
            );
        }

        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            gvReport.DataSource = null;
            gvReport.DataBind();


            if (string.IsNullOrEmpty(
                ddlClass.SelectedValue))
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
                Convert.ToInt32(
                    ddlClass.SelectedValue
                );


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
                        Convert.ToInt32(
                            cmd.ExecuteScalar()
                        );


                    return count > 0;
                }
            }
        }

        private void LoadAttendanceReport(int classId, int subjectId, DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                string query = @"SELECT S.RollNo, S.StudentName, COUNT(AD.AttendanceDetailId) AS TotalClasses, SUM(CASE WHEN AD.Status = 'Present' THEN 1 ELSE 0 END ) 
                                AS Present, SUM(CASE WHEN AD.Status = 'Absent' THEN 1 ELSE 0 END ) AS Absent, CAST((SUM(CASE WHEN AD.Status = 'Present' THEN 1 ELSE 0 END)
                                * 100.0 / NULLIF(COUNT(AD.AttendanceDetailId),0 )) AS DECIMAL(5,2) ) AS AttendancePercentage FROM Students S
                                LEFT JOIN AttendanceDetails AD ON S.StudentId = AD.StudentId
                                LEFT JOIN Attendance A ON AD.AttendanceId = A.AttendanceId AND A.SubjectId = @SubjectId AND A.AttendanceDate BETWEEN @FromDate AND @ToDate
                                WHERE S.ClassId = @ClassId AND S.IsActive = 1
                                GROUP BY S.StudentId, S.RollNo, S.StudentName ORDER BY S.RollNo";


                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@ClassId",
                        classId
                    );

                    cmd.Parameters.AddWithValue(
                        "@SubjectId",
                        subjectId
                    );

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        fromDate.Date
                    );

                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        toDate.Date
                    );


                    using (SqlDataAdapter da =
                        new SqlDataAdapter(cmd))
                    {
                        DataTable dt =
                            new DataTable();

                        da.Fill(dt);


                        gvReport.DataSource = dt;

                        gvReport.DataBind();

                        if (dt.Rows.Count == 0)
                        {
                            lblMessage.Text =
                                "No attendance records found for the selected criteria.";

                            lblMessage.ForeColor =
                                System.Drawing.Color.Red;
                        }
                        else
                        {
                            lblMessage.Text =
                                "Attendance report generated successfully.";

                            lblMessage.ForeColor =
                                System.Drawing.Color.Green;
                        }
                    }
                }
            }
        }
    }
}