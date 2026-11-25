
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminLogin.aspx.cs" Inherits="Admin_AdminLogin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Admin Login | USP Grievance System</title>
     <!-- Inter Font -->
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap"
        rel="stylesheet" />
    <link href="css/AdminLogin.css?v=3" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">

        <main class="admin-login-page">
            <div class="background-overlay" aria-hidden="true"></div>

            <section class="admin-login-card" aria-label="Admin Login">

                <div class="login-brand">
                    <img src="../Images/usp-logo.png"
                         alt="University of Southern Punjab Logo"
                         class="usp-logo" />

                    <p class="university-name">
                        University of Southern Punjab
                    </p>
                </div>

                <div class="login-heading">
                    <span class="portal-label">ADMIN PORTAL</span>
                    <h1>Welcome Back</h1>
                    <p>Sign in to manage university Grievances.</p>
                </div>

                <asp:Label ID="lblMessage"
                    runat="server"
                    CssClass="login-message"
                    EnableViewState="false">
                </asp:Label>

                <div class="form-group">
                    <asp:Label ID="lblLogin"
                        runat="server"
                        AssociatedControlID="txtLogin"
                        CssClass="form-label">
                        Admin Name or Email
                    </asp:Label>

                    <asp:TextBox ID="txtLogin"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="100"
                        autocomplete="username"
                        placeholder="Enter your admin name or email">
                    </asp:TextBox>
                </div>

                <div class="form-group">
                    <asp:Label ID="lblPassword"
                        runat="server"
                        AssociatedControlID="txtPassword"
                        CssClass="form-label">
                        Password
                    </asp:Label>

                    <div class="password-wrapper">
                        <asp:TextBox ID="txtPassword"
                            runat="server"
                            TextMode="Password"
                            CssClass="form-control password-input"
                            autocomplete="current-password"
                            placeholder="Enter your password">
                        </asp:TextBox>

                        <button type="button"
                            class="password-toggle"
                            id="btnPasswordToggle"
                            aria-label="Show password"
                            aria-pressed="false">
                            Show
                        </button>
                    </div>
                </div>

                <div class="login-options">
                    <label class="remember-option" for="chkRemember">
                        <input type="checkbox"
                            id="chkRemember"
                            name="chkRemember" />
                        <span>Remember Me</span>
                    </label>
<a href="../ForgotPassword.aspx" class="forgot-link">
    Forgot Password?
</a>
                </div>

                <asp:Button ID="btnLogin"
                    runat="server"
                    Text="Sign In"
                    CssClass="login-button"
                    OnClick="btnLogin_Click" />

                <div class="security-note">
                    <span class="security-dot"></span>
                    Authorized Administrators Only
                </div>

            </section>

            <footer class="page-footer">
                © University of Southern Punjab, Multan
            </footer>
        </main>

        <script type="text/javascript">
            (function () {
                var toggle = document.getElementById("btnPasswordToggle");
                var password = document.getElementById(
                    "<%= txtPassword.ClientID %>");

                if (!toggle || !password) {
                    return;
                }

                toggle.addEventListener("click", function () {
                    var hidden = password.type === "password";

                    password.type = hidden ? "text" : "password";
                    toggle.textContent = hidden ? "Hide" : "Show";

                    toggle.setAttribute(
                        "aria-label",
                        hidden ? "Hide password" : "Show password"
                    );

                    toggle.setAttribute(
                        "aria-pressed",
                        hidden ? "true" : "false"
                    );
                });
            })();
        </script>

    </form>
</body>
</html>
