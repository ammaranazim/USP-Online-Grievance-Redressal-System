
<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="HODLogin.aspx.cs"
    Inherits="DepartmentHead_DepartmentHeadLogin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Department Head Login | USP</title>
     <!-- Inter Font -->
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap"
        rel="stylesheet" />

    <link href="css/HODLogin.css?v=2" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <main class="hod-login-page">
            <div class="background-overlay" aria-hidden="true"></div>

            <section class="hod-login-card" aria-label="Department Head Login">

                <div class="login-brand">
                    <img src="../Images/usp-logo.png"
                         alt="University of Southern Punjab Logo"
                         class="usp-logo" />

                    <p class="university-name">
                        University of Southern Punjab
                    </p>
                </div>

                <div class="login-heading">
                    <span class="portal-label">DEPARTMENT HEAD PORTAL</span>
                    <h1>Welcome Back</h1>
                    <p>Sign in to manage your department Grievances.</p>
                </div>

                <asp:Label ID="lblMessage" runat="server"
                    CssClass="login-message"
                    EnableViewState="false"></asp:Label>

                <div class="form-group">
                    <asp:Label ID="lblUsername" runat="server"
                        AssociatedControlID="txtUsername"
                        CssClass="form-label"
                        Text="Username">
                    </asp:Label>

                    <asp:TextBox ID="txtUsername" runat="server"
                        CssClass="form-control"
                        MaxLength="50"
                        autocomplete="username"
                        placeholder="Enter your username">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server"
                        ControlToValidate="txtUsername"
                        ErrorMessage="Username is required."
                        CssClass="field-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <asp:Label ID="lblPassword" runat="server"
                        AssociatedControlID="txtPassword"
                        CssClass="form-label"
                        Text="Password">
                    </asp:Label>

                    <div class="password-wrapper">
                        <asp:TextBox ID="txtPassword" runat="server"
                            TextMode="Password"
                            CssClass="form-control password-input"
                            MaxLength="100"
                            autocomplete="current-password"
                            placeholder="Enter your password">
                        </asp:TextBox>

                        <button type="button"
                            class="password-toggle"
                            id="togglePassword"
                            aria-label="Show password"
                            aria-pressed="false">
                            Show
                        </button>
                    </div>

                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                        ControlToValidate="txtPassword"
                        ErrorMessage="Password is required."
                        CssClass="field-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <div class="login-options">
                    <a href="../ForgotPassword.aspx"
   class="forgot-link">
    Forgot Password?
</a>
                </div>

                <asp:Button ID="btnLogin" runat="server"
                    Text="Sign In"
                    CssClass="login-button"
                    OnClick="btnLogin_Click" />

                <div class="security-note">
                    <span class="security-dot"></span>
                    Authorized Department Heads Only
                </div>

            </section>

            <footer class="page-footer">
                © University of Southern Punjab, Multan
            </footer>
        </main>

        <script type="text/javascript">
            (function () {
                var passwordField = document.getElementById(
                    '<%= txtPassword.ClientID %>');
                var toggleButton = document.getElementById('togglePassword');

                if (!passwordField || !toggleButton) {
                    return;
                }

                toggleButton.addEventListener('click', function () {
                    var hidden = passwordField.type === 'password';

                    passwordField.type = hidden ? 'text' : 'password';
                    toggleButton.textContent = hidden ? 'Hide' : 'Show';

                    toggleButton.setAttribute(
                        'aria-label',
                        hidden ? 'Hide password' : 'Show password'
                    );

                    toggleButton.setAttribute(
                        'aria-pressed',
                        hidden ? 'true' : 'false'
                    );
                });
            })();
        </script>

    </form>
</body>
</html>
