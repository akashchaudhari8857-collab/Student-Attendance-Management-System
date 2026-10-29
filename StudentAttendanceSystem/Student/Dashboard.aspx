<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="StudentAttendanceSystem.Student.Dashboard" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .page-title {
            margin-bottom: 20px;
        }

        .page-title h2 {
            margin: 0;
            color: #333;
        }

        .profile-container {
            background: white;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            margin-bottom: 25px;
        }

        .profile-container h3 {
            margin-top: 0;
            color: #333;
        }

        .profile-row {
            display: flex;
            gap: 40px;
            flex-wrap: wrap;
        }

        .profile-item {
            min-width: 180px;
        }

        .profile-label {
            display: block;
            color: #777;
            font-size: 13px;
            margin-bottom: 5px;
        }

        .profile-value {
            font-size: 17px;
            font-weight: bold;
            color: #333;
        }

        .section-container {
            background: white;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            margin-bottom: 25px;
        }

        .section-container h3 {
            margin-top: 0;
            color: #333;
        }

        .attendance-cards {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 20px;
        }

        .attendance-card {
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 6px;
            text-align: center;
        }

        .attendance-card h4 {
            margin: 0 0 10px 0;
            color: #555;
        }

        .attendance-number {
            font-size: 26px;
            font-weight: bold;
            color: #2c3e50;
        }

        .grid {
            width: 100%;
            border-collapse: collapse;
            margin-top: 15px;
        }

        .grid th {
            background-color: #34495e;
            color: white;
            padding: 12px;
            text-align: left;
        }

        .grid td {
            padding: 10px;
            border-bottom: 1px solid #ddd;
        }

        .grid tr:hover {
            background-color: #f5f5f5;
        }

        @media (max-width: 768px) {
            .attendance-cards {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 500px) {
            .attendance-cards {
                grid-template-columns: 1fr;
            }
        }
    </style>

</asp:Content>


<asp:Content
    ID="Content2"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="page-title">
        <h2>Student Dashboard</h2>
    </div>


    <!-- Student Profile -->

    <div class="profile-container">

        <h3>My Profile</h3>

        <div class="profile-row">

            <div class="profile-item">

                <span class="profile-label">
                    Student Name
                </span>

                <asp:Label
                    ID="lblStudentName"
                    runat="server"
                    CssClass="profile-value">
                </asp:Label>

            </div>


            <div class="profile-item">

                <span class="profile-label">
                    Roll No
                </span>

                <asp:Label
                    ID="lblRollNo"
                    runat="server"
                    CssClass="profile-value">
                </asp:Label>

            </div>


            <div class="profile-item">

                <span class="profile-label">
                    Class
                </span>

                <asp:Label
                    ID="lblClassName"
                    runat="server"
                    CssClass="profile-value">
                </asp:Label>

            </div>

        </div>

    </div>


    <!-- Overall Attendance -->

    <div class="section-container">

        <h3>Overall Attendance</h3>

        <div class="attendance-cards">

            <div class="attendance-card">

                <h4>Total Classes</h4>

                <div class="attendance-number">

                    <asp:Label
                        ID="lblTotalClasses"
                        runat="server"
                        Text="0">
                    </asp:Label>

                </div>

            </div>


            <div class="attendance-card">

                <h4>Present</h4>

                <div class="attendance-number">

                    <asp:Label
                        ID="lblPresent"
                        runat="server"
                        Text="0">
                    </asp:Label>

                </div>

            </div>


            <div class="attendance-card">

                <h4>Absent</h4>

                <div class="attendance-number">

                    <asp:Label
                        ID="lblAbsent"
                        runat="server"
                        Text="0">
                    </asp:Label>

                </div>

            </div>


            <div class="attendance-card">

                <h4>Attendance %</h4>

                <div class="attendance-number">

                    <asp:Label
                        ID="lblPercentage"
                        runat="server"
                        Text="0%">
                    </asp:Label>

                </div>

            </div>

        </div>

    </div>


    <!-- Subject-wise Attendance -->

    <div class="section-container">

        <h3>Subject-wise Attendance</h3>

        <asp:GridView
            ID="gvAttendance"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="grid">

            <Columns>

                <asp:BoundField
                    DataField="SubjectName"
                    HeaderText="Subject" />


                <asp:BoundField
                    DataField="TotalClasses"
                    HeaderText="Total Classes" />


                <asp:BoundField
                    DataField="Present"
                    HeaderText="Present" />


                <asp:BoundField
                    DataField="Absent"
                    HeaderText="Absent" />


                <asp:BoundField
                    DataField="AttendancePercentage"
                    HeaderText="Attendance %" />

            </Columns>

        </asp:GridView>

    </div>

</asp:Content>