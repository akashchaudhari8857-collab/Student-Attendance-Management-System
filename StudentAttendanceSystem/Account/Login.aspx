<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="StudentAttendanceSystem.Account.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Attendance Management System - Login</title>
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            background-color: #f2f4f7;
        }

        .login-container {
            width: 380px;
            margin: 100px auto;
            background-color: white;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0px 0px 10px #cccccc;
        }
        .title {
            text-align: center;
            margin-bottom: 25px;
        }

        .title h2 {
            margin-bottom: 5px;
        }

        .title p {
            color: #777777;
        }

        .form-group {
            margin-bottom: 18px;
        }

        .form-group label {
            display: block;
            margin-bottom: 6px;
            font-weight: bold;
        }
        .textbox {
            width: 100%;
            padding: 10px;
            box-sizing: border-box;
            border: 1px solid #cccccc;
            border-radius: 4px;
        }

        .login-button {
            width: 100%;
            padding: 11px;
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
        }
        .login-button:hover {
            background-color: #0056b3;
        }

        .message {
            display: block;
            text-align: center;
            margin-top: 15px;
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="title">
                <h2>Student Attendance</h2>
                <p>Management System</p>
            </div>
            <div class="form-group">
                <label>Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="textbox"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Password</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="textbox"></asp:TextBox>
            </div>
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="login-button" OnClick="btnLogin_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>

        </div>
    </form>
</body>
</html>
