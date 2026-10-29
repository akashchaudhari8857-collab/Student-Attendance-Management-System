<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Classes.aspx.cs" Inherits="StudentAttendanceSystem.Admin.Classes" MasterPageFile="~/Site.Master" %>

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
        }

        .form-row label {
            font-weight: bold;
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
        <h2>Class Management</h2>
    </div>


    <div class="form-container">

        <asp:HiddenField
            ID="hfClassId"
            runat="server" />

        <div class="form-row">

            <label for="txtClassName">
                Class Name:
            </label>

            <asp:TextBox
                ID="txtClassName"
                runat="server"
                CssClass="form-control">
            </asp:TextBox>


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

        <h3>Class List</h3>

        <asp:GridView
            ID="gvClasses"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="grid"
            DataKeyNames="ClassId"
            OnRowCommand="gvClasses_RowCommand">

            <Columns>

                <asp:BoundField
                    DataField="ClassId"
                    HeaderText="ID" />

                <asp:BoundField
                    DataField="ClassName"
                    HeaderText="Class Name" />

                <asp:TemplateField
                    HeaderText="Status">

                    <ItemTemplate>

                        <%# Convert.ToBoolean(Eval("IsActive"))
                            ? "Active"
                            : "Inactive" %>

                    </ItemTemplate>

                </asp:TemplateField>


                <asp:TemplateField HeaderText="Action">
    <ItemTemplate>

        <%-- Show Edit and Deactivate only for active classes --%>
        <asp:LinkButton
            ID="btnEdit"
            runat="server"
            Text="Edit"
            CommandName="EditClass"
            CommandArgument='<%# Eval("ClassId") %>'
            CausesValidation="false"
            Visible='<%# Convert.ToBoolean(Eval("IsActive")) %>'>
        </asp:LinkButton>

        <asp:PlaceHolder
            ID="phSeparator"
            runat="server"
            Visible='<%# Convert.ToBoolean(Eval("IsActive")) %>'>
            &nbsp;|&nbsp;
        </asp:PlaceHolder>

        <asp:LinkButton
            ID="btnDeactivate"
            runat="server"
            Text="Deactivate"
            CommandName="DeleteClass"
            CommandArgument='<%# Eval("ClassId") %>'
            CausesValidation="false"
            Visible='<%# Convert.ToBoolean(Eval("IsActive")) %>'
            OnClientClick="return confirm('Are you sure you want to deactivate this class?');">
        </asp:LinkButton>

        <%-- Show Activate only for inactive classes --%>
        <asp:LinkButton
            ID="btnActivate"
            runat="server"
            Text="Activate"
            CommandName="ActivateClass"
            CommandArgument='<%# Eval("ClassId") %>'
            CausesValidation="false"
            Visible='<%# !Convert.ToBoolean(Eval("IsActive")) %>'
            OnClientClick="return confirm('Are you sure you want to activate this class?');">
        </asp:LinkButton>

    </ItemTemplate>
</asp:TemplateField>
            </Columns>

        </asp:GridView>

    </div>

</asp:Content>