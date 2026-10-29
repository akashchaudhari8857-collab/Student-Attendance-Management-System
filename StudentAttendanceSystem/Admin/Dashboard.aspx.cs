
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace StudentAttendanceSystem.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string conn = ConfigurationManager
            .ConnectionStrings["StudentAttendanceConnection"]
            .ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check Admin login
            if (Session["UserId"] == null ||
                Session["Role"] == null ||
                Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadDashboardStatistics();
                LoadTodayAttendance();
            }
        }

        // ==========================================
        // LOAD DASHBOARD STATISTICS
        // ==========================================

        private void LoadDashboardStatistics()
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                string query = @"
                    SELECT
                        (SELECT COUNT(*)
                         FROM Students
                         WHERE IsActive = 1) AS TotalStudents,

                        (SELECT COUNT(*)
                         FROM Classes
                         WHERE IsActive = 1) AS TotalClasses,

                        (SELECT COUNT(*)
                         FROM Subjects
                         WHERE IsActive = 1) AS TotalSubjects,

                        (SELECT COUNT(*)
                         FROM Attendance) AS TotalAttendance";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblTotalStudents.Text =
                                reader["TotalStudents"].ToString();

                            lblTotalClasses.Text =
                                reader["TotalClasses"].ToString();

                            lblTotalSubjects.Text =
                                reader["TotalSubjects"].ToString();

                            lblTotalAttendance.Text =
                                reader["TotalAttendance"].ToString();
                        }
                    }
                }
            }
        }

        // ==========================================
        // LOAD TODAY'S ATTENDANCE
        // ==========================================

        private void LoadTodayAttendance()
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                string query = @"
                    SELECT
                        ISNULL(SUM(
                            CASE WHEN AD.Status = 'Present'
                                 THEN 1 ELSE 0 END), 0) AS Present,

                        ISNULL(SUM(
                            CASE WHEN AD.Status = 'Absent'
                                 THEN 1 ELSE 0 END), 0) AS Absent

                    FROM Attendance A
                    INNER JOIN AttendanceDetails AD
                        ON A.AttendanceId = AD.AttendanceId
                    WHERE A.AttendanceDate = @Today";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@Today", DateTime.Today);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int present = Convert.ToInt32(
                                reader["Present"]);

                            int absent = Convert.ToInt32(
                                reader["Absent"]);

                            int total = present + absent;

                            decimal percentage = 0;

                            if (total > 0)
                            {
                                percentage =
                                    (present * 100m) / total;
                            }

                            lblTodayPresent.Text =
                                present.ToString();

                            lblTodayAbsent.Text =
                                absent.ToString();

                            lblTodayPercentage.Text =
                                percentage.ToString("0.00") + "%";
                        }
                    }
                }
            }
        }

        // ==========================================
        // COMMON METHOD TO DISPLAY RECORDS
        // ==========================================

        private void ShowDetails(string title, string query)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();

                        adapter.Fill(dt);

                        lblDetailsTitle.Text =
                            title + " (" + dt.Rows.Count + ")";

                        gvDetails.DataSource = dt;
                        gvDetails.DataBind();

                        pnlDetails.Visible = true;
                    }
                }
            }
        }

        // ==========================================
        // CLICK: ACTIVE STUDENTS
        // ==========================================

        protected void btnStudents_Click(object sender, EventArgs e)
        {
            string query = @"
                SELECT *
                FROM Students
                WHERE IsActive = 1";

            ShowDetails("Active Students", query);
        }

        // ==========================================
        // CLICK: ACTIVE CLASSES
        // ==========================================

        protected void btnClasses_Click(object sender, EventArgs e)
        {
            string query = @"
                SELECT *
                FROM Classes
                WHERE IsActive = 1";

            ShowDetails("Active Classes", query);
        }

        // ==========================================
        // CLICK: ACTIVE SUBJECTS
        // ==========================================

        protected void btnSubjects_Click(object sender, EventArgs e)
        {
            string query = @"
                SELECT *
                FROM Subjects
                WHERE IsActive = 1";

            ShowDetails("Active Subjects", query);
        }

        // ==========================================
        // CLICK: ATTENDANCE RECORDS
        // ==========================================

        protected void btnAttendance_Click(object sender, EventArgs e)
        {
            string query = @"
                SELECT
                    A.AttendanceId,
                    A.AttendanceDate,
                    AD.StudentId,
                    AD.Status
                FROM Attendance A
                INNER JOIN AttendanceDetails AD
                    ON A.AttendanceId = AD.AttendanceId
                ORDER BY A.AttendanceDate DESC,
                         A.AttendanceId DESC";

            ShowDetails("Attendance Records", query);
        }

        // ==========================================
        // CLOSE DETAILS
        // ==========================================

        protected void btnCloseDetails_Click(object sender, EventArgs e)
        {
            pnlDetails.Visible = false;
            gvDetails.DataSource = null;
            gvDetails.DataBind();
        }
    }
}
