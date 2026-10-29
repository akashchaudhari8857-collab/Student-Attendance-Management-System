<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttendanceReport.aspx.cs" Inherits="StudentAttendanceSystem.Admin.AttendanceReport" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .page-title {
            margin-bottom: 20px;
        }

        .page-title h2 {
            margin: 0;
            color: #333;
        }

        .filter-container {
            background: white;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            margin-bottom: 25px;
        }

        .filter-row {
            display: flex;
            align-items: center;
            gap: 15px;
            flex-wrap: wrap;
            margin-bottom: 15px;
        }

        .filter-row:last-child {
            margin-bottom: 0;
        }

        .filter-row label {
            font-weight: bold;
            min-width: 100px;
        }

        .form-control {
            padding: 9px 12px;
            border: 1px solid #ccc;
            border-radius: 4px;
            width: 220px;
            font-size: 14px;
        }

        .date-control {
            padding: 9px 12px;
            border: 1px solid #ccc;
            border-radius: 4px;
            width: 170px;
            font-size: 14px;
        }

        .btn {
            padding: 9px 18px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
        }

        .btn-primary {
            background-color: #3498db;
            color: white;
        }

        .message {
            display: block;
            margin-top: 15px;
            font-weight: bold;
        }

        .grid-container {
            background: white;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            overflow-x: auto;
        }

        .grid-container h3 {
            margin-top: 0;
            color: #333;
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
    </style>

</asp:Content>


<asp:Content
    ID="Content2"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="page-title">
        <h2>Attendance Report</h2>
    </div>


    <div class="filter-container">

        <div class="filter-row">

            <label for="ddlClass">
                Class:
            </label>

            <asp:DropDownList
                ID="ddlClass"
                runat="server"
                CssClass="form-control"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlClass_SelectedIndexChanged">
            </asp:DropDownList>

        </div>


        <div class="filter-row">

            <label for="ddlSubject">
                Subject:
            </label>

            <asp:DropDownList
                ID="ddlSubject"
                runat="server"
                CssClass="form-control">
            </asp:DropDownList>

        </div>


        <div class="filter-row">

            <label for="txtFromDate">
                From Date:
            </label>

            <asp:TextBox
                ID="txtFromDate"
                runat="server"
                TextMode="Date"
                CssClass="date-control">
            </asp:TextBox>


            <label for="txtToDate">
                To Date:
            </label>

            <asp:TextBox
                ID="txtToDate"
                runat="server"
                TextMode="Date"
                CssClass="date-control">
            </asp:TextBox>

        </div>


        <div class="filter-row">

            <asp:Button
                ID="btnGenerate"
                runat="server"
                Text="Generate Report"
                CssClass="btn btn-primary"
                OnClick="btnGenerateReport_Click" />

        </div>


        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="message">
        </asp:Label>

    </div>


    <div class="grid-container">

        <h3>Attendance Details</h3>

        <asp:GridView
            ID="gvReport"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="grid">

            <Columns>

                <asp:BoundField
                    DataField="RollNo"
                    HeaderText="Roll No" />


                <asp:BoundField
                    DataField="StudentName"
                    HeaderText="Student Name" />


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