<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarkAttendance.aspx.cs" Inherits="StudentAttendanceSystem.Admin.MarkAttendance" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .page-title {
            margin-bottom: 20px;
        }

        .page-title h2 {
            margin: 0;
            color: #333;
        }

        .form-container {
            background: white;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            margin-bottom: 25px;
        }

        .form-row {
            display: flex;
            align-items: center;
            gap: 15px;
            flex-wrap: wrap;
            margin-bottom: 15px;
        }

        .form-row label {
            font-weight: bold;
            min-width: 100px;
        }

        .form-control {
            padding: 9px 12px;
            border: 1px solid #ccc;
            border-radius: 4px;
            width: 250px;
            font-size: 14px;
        }

        .date-control {
            padding: 9px 12px;
            border: 1px solid #ccc;
            border-radius: 4px;
            width: 180px;
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

        .btn-success {
            background-color: #27ae60;
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

        .attendance-dropdown {
            padding: 7px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }
    </style>

</asp:Content>


<asp:Content
    ID="Content2"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="page-title">
        <h2>Mark Attendance</h2>
    </div>


    <div class="form-container">

        <div class="form-row">

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


        <div class="form-row">

            <label for="ddlSubject">
                Subject:
            </label>

            <asp:DropDownList
                ID="ddlSubject"
                runat="server"
                CssClass="form-control">
            </asp:DropDownList>

        </div>


        <div class="form-row">

            <label for="txtAttendanceDate">
                Date:
            </label>

            <asp:TextBox
                ID="txtAttendanceDate"
                runat="server"
                TextMode="Date"
                CssClass="date-control">
            </asp:TextBox>

        </div>


        <div class="form-row">

            <asp:Button
                ID="btnLoadStudents"
                runat="server"
                Text="Load Students"
                CssClass="btn btn-primary"
                OnClick="btnLoadStudents_Click" />

        </div>


        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="message">
        </asp:Label>

    </div>


    <div class="grid-container">

        <h3>Student Attendance</h3>

        <asp:GridView
            ID="gvStudents"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="grid"
            DataKeyNames="StudentId">

            <Columns>

                <asp:BoundField
                    DataField="RollNo"
                    HeaderText="Roll No" />


                <asp:BoundField
                    DataField="StudentName"
                    HeaderText="Student Name" />


                <asp:TemplateField
                    HeaderText="Attendance Status">

                    <ItemTemplate>

                        <asp:DropDownList
                            ID="ddlStatus"
                            runat="server"
                            CssClass="attendance-dropdown">

                            <asp:ListItem
                                Text="Present"
                                Value="Present">
                            </asp:ListItem>

                            <asp:ListItem
                                Text="Absent"
                                Value="Absent">
                            </asp:ListItem>

                        </asp:DropDownList>

                    </ItemTemplate>

                </asp:TemplateField>

            </Columns>

        </asp:GridView>


        <br />

        <asp:Button
            ID="btnSaveAttendance"
            runat="server"
            Text="Save Attendance"
            CssClass="btn btn-success"
            OnClick="btnSaveAttendance_Click" />

    </div>

</asp:Content>