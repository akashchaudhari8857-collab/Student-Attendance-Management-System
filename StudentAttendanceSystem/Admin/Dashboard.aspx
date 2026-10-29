<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="StudentAttendanceSystem.Admin.Dashboard" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .dashboard-title {
            margin-bottom: 20px;
        }

            .dashboard-title h2 {
                margin: 0;
                color: #333;
            }

            .dashboard-title p {
                color: #777;
                margin-top: 5px;
            }

        .stats-container {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 20px;
            margin-bottom: 25px;
        }

        .stat-card h3 {
            margin: 0 0 10px 0;
            color: #555;
            font-size: 16px;
        }

        .stat-number {
            font-size: 30px;
            font-weight: bold;
            color: #2c3e50;
        }

        .attendance-section {
            background: white;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
        }

            .attendance-section h3 {
                margin-top: 0;
                color: #333;
            }

        .attendance-stats {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 20px;
            margin-top: 20px;
        }

        .attendance-box {
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 6px;
            text-align: center;
        }

            .attendance-box h4 {
                margin: 0 0 10px 0;
                color: #555;
            }

        .attendance-value {
            font-size: 25px;
            font-weight: bold;
        }

        .stat-card {
            display: block;
            text-decoration: none;
            color: inherit;
            cursor: pointer;
            text-align: center;
            border: none;
            font-family: inherit;
            font-size: inherit;
            width: 100%;
            box-sizing: border-box;
        }

            .stat-card:hover {
                transform: translateY(-3px);
                box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
            }

        .details-section {
            margin-top: 30px;
            padding: 20px;
            background: #ffffff;
            border-radius: 8px;
            overflow-x: auto;
        }

            .details-section h2 {
                margin-bottom: 15px;
            }

            .details-section .grid {
                border-collapse: collapse;
            }

                .details-section .grid th,
                .details-section .grid td {
                    padding: 10px;
                    border: 1px solid #dddddd;
                    text-align: left;
                }

        @media (max-width: 768px) {
            .stats-container,
            .attendance-stats {
                grid-template-columns: 1fr;
            }
        }
    </style>

</asp:Content>


<asp:Content
    ID="Content2"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="dashboard-title">
        <h2>Admin Dashboard</h2>
        <p>Overview of the Student Attendance Management System</p>
    </div>

    ```aspx
    <div class="stats-container">

        <asp:LinkButton ID="btnStudents"
            runat="server"
            CssClass="stat-card"
            OnClick="btnStudents_Click"
            CausesValidation="false">

            <h3>Total Active Students</h3>
            <asp:Label ID="lblTotalStudents" runat="server" Text="0" />

        </asp:LinkButton>

        <asp:LinkButton ID="btnClasses"
            runat="server"
            CssClass="stat-card"
            OnClick="btnClasses_Click"
            CausesValidation="false">

            <h3>Total Active Classes</h3>
            <asp:Label ID="lblTotalClasses" runat="server" Text="0" />

        </asp:LinkButton>

        <asp:LinkButton ID="btnSubjects"
            runat="server"
            CssClass="stat-card"
            OnClick="btnSubjects_Click"
            CausesValidation="false">

            <h3>Total Active Subjects</h3>
            <asp:Label ID="lblTotalSubjects" runat="server" Text="0" />

        </asp:LinkButton>

        <asp:LinkButton ID="btnAttendance"
            runat="server"
            CssClass="stat-card"
            OnClick="btnAttendance_Click"
            CausesValidation="false">

            <h3>Total Attendance Records</h3>
            <asp:Label ID="lblTotalAttendance" runat="server" Text="0" />

        </asp:LinkButton>

    </div>
    <div class="today-attendance">

    <h2>Today's Attendance</h2>

    <div class="stats-container">

        <div class="stat-card">
            <h3>Present Students</h3>
            <asp:Label ID="lblTodayPresent"
                runat="server"
                Text="0" />
        </div>

        <div class="stat-card">
            <h3>Absent Students</h3>
            <asp:Label ID="lblTodayAbsent"
                runat="server"
                Text="0" />
        </div>

        <div class="stat-card">
            <h3>Attendance Percentage</h3>
            <asp:Label ID="lblTodayPercentage"
                runat="server"
                Text="0.00%" />
        </div>

    </div>

</div>

    <!-- Selected records appear below the cards -->

    <asp:Panel ID="pnlDetails" runat="server"
        Visible="false"
        CssClass="details-section">

        <h2>
            <asp:Label ID="lblDetailsTitle"
                runat="server" />
        </h2>

        <asp:Button ID="btnCloseDetails"
            runat="server"
            Text="Close Details"
            OnClick="btnCloseDetails_Click"
            CausesValidation="false" />

        <br />
        <br />

        <asp:GridView ID="gvDetails"
            runat="server"
            AutoGenerateColumns="true"
            CssClass="grid"
            GridLines="Both"
            EmptyDataText="No records found."
            Width="100%">
        </asp:GridView>

    </asp:Panel>

</asp:Content>
