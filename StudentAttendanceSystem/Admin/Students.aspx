<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Students.aspx.cs" Inherits="StudentAttendanceSystem.Admin.Students" MasterPageFile="~/Site.Master" %>

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
            width: 120px;
        }

        .form-control {
            padding: 9px 12px;
            border: 1px solid #ccc;
            border-radius: 4px;
            width: 250px;
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

        .btn-secondary {
            background-color: #95a5a6;
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
        <h2>Student Management</h2>
    </div>


    <div class="form-container">

        <asp:HiddenField
            ID="hfStudentId"
            runat="server" />


        <div class="form-row">

            <label for="txtRollNo">
                Roll No:
            </label>

            <asp:TextBox
                ID="txtRollNo"
                runat="server"
                CssClass="form-control">
            </asp:TextBox>

        </div>


        <div class="form-row">

            <label for="txtStudentName">
                Student Name:
            </label>

            <asp:TextBox
                ID="txtStudentName"
                runat="server"
                CssClass="form-control">
            </asp:TextBox>

        </div>


        <div class="form-row">

            <label for="txtEmail">
                Email:
            </label>

            <asp:TextBox
                ID="txtEmail"
                runat="server"
                CssClass="form-control">
            </asp:TextBox>

        </div>


        <div class="form-row">

            <label for="txtMobile">
                Mobile:
            </label>

            <asp:TextBox
                ID="txtMobile"
                runat="server"
                CssClass="form-control">
            </asp:TextBox>

        </div>


        <div class="form-row">

            <label for="ddlClass">
                Class:
            </label>

            <asp:DropDownList
                ID="ddlClass"
                runat="server"
                CssClass="form-control">
            </asp:DropDownList>

        </div>


        <div class="form-row">

            <label for="txtUsername">
                Username:
            </label>

            <asp:TextBox
                ID="txtUsername"
                runat="server"
                CssClass="form-control">
            </asp:TextBox>

        </div>


        <div class="form-row">

            <label for="txtPassword">
                Password:
            </label>

            <asp:TextBox
                ID="txtPassword"
                runat="server"
                TextMode="Password"
                CssClass="form-control">
            </asp:TextBox>

        </div>


        <div class="form-row">

            <span style="width: 120px;"></span>

            <asp:Button
                ID="btnSave"
                runat="server"
                Text="Save"
                CssClass="btn btn-primary"
                OnClick="btnSave_Click" />

            <asp:Button
                ID="btnCancel"
                runat="server"
                Text="Cancel"
                CssClass="btn btn-secondary"
                OnClick="btnCancel_Click"
                CausesValidation="false" />

        </div>


        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="message">
        </asp:Label>

    </div>


    <div class="grid-container">

        <h3>Student List</h3>

        <asp:GridView
            ID="gvStudents"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="grid"
            DataKeyNames="StudentId"
            OnRowCommand="gvStudents_RowCommand">

            <Columns>

                <asp:BoundField
                    DataField="StudentId"
                    HeaderText="ID" />

                <asp:BoundField
                    DataField="RollNo"
                    HeaderText="Roll No" />

                <asp:BoundField
                    DataField="StudentName"
                    HeaderText="Student Name" />

                <asp:BoundField
                    DataField="Email"
                    HeaderText="Email" />

                <asp:BoundField
                    DataField="Mobile"
                    HeaderText="Mobile" />

                <asp:BoundField
                    DataField="ClassName"
                    HeaderText="Class" />

                <asp:BoundField
                    DataField="Username"
                    HeaderText="Username" />

                <asp:TemplateField
                    HeaderText="Status">

                    <ItemTemplate>

                        <%# Convert.ToBoolean(Eval("IsActive"))
                            ? "Active"
                            : "Inactive" %>

                    </ItemTemplate>

                </asp:TemplateField>


                <asp:TemplateField
                    HeaderText="Action">

                    <ItemTemplate>

                        <asp:LinkButton
                            ID="btnEdit"
                            runat="server"
                            Text="Edit"
                            CommandName="EditStudent"
                            CommandArgument='<%# Eval("StudentId") %>'
                            CausesValidation="false">
                        </asp:LinkButton>

                        &nbsp;|&nbsp;

                        <asp:LinkButton
                            ID="btnDeactivate"
                            runat="server"
                            Text="Deactivate"
                            CommandName="DeleteStudent"
                            CommandArgument='<%# Eval("StudentId") %>'
                            CausesValidation="false"
                            OnClientClick="return confirm('Are you sure you want to deactivate this student?');">
                        </asp:LinkButton>

                    </ItemTemplate>

                </asp:TemplateField>

            </Columns>

        </asp:GridView>

    </div>

</asp:Content>