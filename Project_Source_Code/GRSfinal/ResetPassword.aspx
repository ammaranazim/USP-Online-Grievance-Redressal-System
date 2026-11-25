<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="ResetPassword.aspx.cs"
    Inherits="ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <meta charset="utf-8" />

    <meta name="viewport"
        content="width=device-width, initial-scale=1" />

    <title>Reset Password - USP Grievance Redressal System</title>

    <!-- Bootstrap 4.4.1 -->
    <link rel="stylesheet"
        href="https://cdn.jsdelivr.net/npm/bootstrap@4.4.1/dist/css/bootstrap.min.css" />

    <!-- Inter -->
    <link rel="stylesheet"
        href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" />

    <!-- External CSS -->
    <link rel="stylesheet"
        href="CSS/ResetPassword.css" />

</head>

<body>

    <form id="form1" runat="server">

        <div class="reset-page">

            <div class="reset-card">

                <!-- Header -->
                <div class="reset-header">

                    <div class="logo-circle">
                        <span>USP</span>
                    </div>

                    <h1>Reset Password</h1>

                    <p>
                        Create a new password for your account.
                    </p>

                </div>

                <!-- Form -->
                <div class="reset-form">

                    <!-- New Password -->
                    <div class="form-group">

                        <label for="txtNewPassword">
                            New Password
                        </label>

                        <asp:TextBox
                            ID="txtNewPassword"
                            runat="server"
                            CssClass="form-control"
                            TextMode="Password"
                            placeholder="Enter your new password">
                        </asp:TextBox>

                    </div>

                    <!-- Confirm Password -->
                    <div class="form-group">

                        <label for="txtConfirmPassword">
                            Confirm New Password
                        </label>

                        <asp:TextBox
                            ID="txtConfirmPassword"
                            runat="server"
                            CssClass="form-control"
                            TextMode="Password"
                            placeholder="Re-enter your new password">
                        </asp:TextBox>

                    </div>

                    <!-- Password Requirements -->
                    <div class="password-info">

                        Password must be at least 8 characters long.

                    </div>

                    <!-- Message -->
                    <asp:Label
                        ID="lblMessage"
                        runat="server"
                        CssClass="message-label">
                    </asp:Label>

                    <!-- Reset Button -->
                    <asp:Button
                        ID="btnResetPassword"
                        runat="server"
                        Text="Reset Password"
                        CssClass="btn btn-primary btn-reset"
                        OnClick="btnResetPassword_Click" />

                    <!-- Login Link -->
                    <div class="login-link">

                        <a href="ForgotPassword.aspx">
                            Back to Forgot Password
                        </a>

                    </div>

                </div>

                <!-- Footer -->
                <div class="reset-footer">

                    <span>
                        Enhanced USP Online Grievance Redressal System
                    </span>

                </div>

            </div>

        </div>

    </form>

</body>

</html>