<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Subjects.aspx.cs" Inherits="StudentAttendanceSystem.Admin.Subjects" MasterPageFile="~/Site.Master" %>

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

        .form-row:last-child {
            margin-bottom: 0;
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
        <h2>Subject Management</h2>
    </div>


    <div class="form-container">

        <asp:HiddenField
            ID="hfSubjectId"
            runat="server" />


        <div class="form-row">

            <label for="txtSubjectName">
                Subject Name:
            </label>

            <asp:TextBox
                ID="txtSubjectName"
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

        <h3>Subject List</h3>

        <asp:GridView
            ID="gvSubjects"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="grid"
            DataKeyNames="SubjectId"
            OnRowCommand="gvSubjects_RowCommand">

            <Columns>

                <asp:BoundField
                    DataField="SubjectId"
                    HeaderText="ID" />


                <asp:BoundField
                    DataField="SubjectName"
                    HeaderText="Subject Name" />


                <asp:BoundField
                    DataField="ClassName"
                    HeaderText="Class" />


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
                            CommandName="EditSubject"
                            CommandArgument='<%# Eval("SubjectId") %>'
                            CausesValidation="false">
                        </asp:LinkButton>

                        &nbsp;|&nbsp;

                        <asp:LinkButton
                            ID="btnDeactivate"
                            runat="server"
                            Text="Deactivate"
                            CommandName="DeactivateSubject"
                            CommandArgument='<%# Eval("SubjectId") %>'
                            CausesValidation="false"
                            OnClientClick="return confirm('Are you sure you want to deactivate this subject?');">
                        </asp:LinkButton>

                    </ItemTemplate>

                </asp:TemplateField>

            </Columns>

        </asp:GridView>

    </div>

</asp:Content>