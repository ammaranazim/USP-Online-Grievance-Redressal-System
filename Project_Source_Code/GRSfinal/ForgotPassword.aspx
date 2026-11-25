<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="ForgotPassword.aspx.cs"
    Inherits="ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <meta charset="utf-8" />

    <meta name="viewport"
        content="width=device-width, initial-scale=1" />

    <title>Forgot Password - USP Grievance Redressal System</title>

    <!-- Bootstrap 4.4.1 -->
    <link rel="stylesheet"
        href="https://cdn.jsdelivr.net/npm/bootstrap@4.4.1/dist/css/bootstrap.min.css" />

    <!-- Project Font -->
    <link rel="stylesheet"
        href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" />

    <!-- Shared Forgot Password CSS -->
    <link rel="stylesheet"
        href="CSS/ForgotPassword.css" />

</head>

<body>

    <form id="form1" runat="server">

        <div class="forgot-page">

            <div class="forgot-card">

                <!-- Logo / Header -->
                <div class="forgot-header">

                    <div class="logo-circle">
                        <span>USP</span>
                    </div>

                    <h1>Forgot Password?</h1>

                    <p>
                        Enter your registered email address and select
                        your account role to receive a password reset link.
                    </p>

                </div>

                <!-- Form -->
                <div class="forgot-form">

                    <!-- Role -->
                    <div class="form-group">

                        <label for="ddlRole">
                            Account Role
                        </label>

                        <asp:DropDownList
                            ID="ddlRole"
                            runat="server"
                            CssClass="form-control">

                            <asp:ListItem
                                Text="-- Select Role --"
                                Value="">
                            </asp:ListItem>

                            <asp:ListItem
                                Text="Student"
                                Value="Student">
                            </asp:ListItem>

                            <asp:ListItem
                                Text="Admin"
                                Value="Admin">
                            </asp:ListItem>

                            <asp:ListItem
                                Text="Department Head (HOD)"
                                Value="HOD">
                            </asp:ListItem>

                            <asp:ListItem
                                Text="Staff"
                                Value="Staff">
                            </asp:ListItem>

                        </asp:DropDownList>

                    </div>

                    <!-- Email -->
                    <div class="form-group">

                        <label for="txtEmail">
                            Registered Email
                        </label>

                        <asp:TextBox
                            ID="txtEmail"
                            runat="server"
                            CssClass="form-control"
                            TextMode="Email"
                            placeholder="Enter your registered email">
                        </asp:TextBox>

                    </div>

                    <!-- Message -->
                    <asp:Label
                        ID="lblMessage"
                        runat="server"
                        CssClass="message-label">
                    </asp:Label>

                    <!-- Send Button -->
                    <asp:Button
                        ID="btnSendResetLink"
                        runat="server"
                        Text="Send Reset Link"
                        CssClass="btn btn-primary btn-send"
                        OnClick="btnSendResetLink_Click" />

                    <!-- Back -->
                    <div class="back-login">

                        <a href="javascript:history.back();">
                            Back to Login
                        </a>

                    </div>

                </div>

                <!-- Footer -->
                <div class="forgot-footer">

                    <span>
                        Enhanced USP Online Grievance Redressal System
                    </span>

                </div>

            </div>

        </div>

    </form>

</body>
</html>