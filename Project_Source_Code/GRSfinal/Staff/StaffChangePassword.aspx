
<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="StaffChangePassword.aspx.cs"
    Inherits="Staff_StaffChangePassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Change Password | USP Staff</title>
    <link href="css/StaffChangePassword.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <main class="change-password-page">
            <section class="change-password-card">

                <div class="brand-section">
                    <img src="../Images/usp-logo.png"
                         alt="USP Logo" class="usp-logo" />
                    <h1>University of Southern Punjab</h1>
                    <p class="university-location">Multan, Pakistan</p>
                </div>

                <div class="page-heading">
                    <span class="page-tag">ACCOUNT SECURITY</span>
                    <h2>Change Your Password</h2>
                    <p>Create a new password before accessing your Staff Dashboard.</p>
                </div>

                <div class="security-notice">
                    <strong>First login password update required</strong>
                    <p>Please replace your temporary password with a new one.</p>
                </div>

                <asp:Label ID="lblMessage" runat="server"
                    EnableViewState="false" CssClass="form-message"></asp:Label>

<div class="form-group">
    <asp:Label ID="lblCurrentPassword" runat="server"
        AssociatedControlID="txtCurrentPassword"
        CssClass="form-label" Text="Current Password" />

    <div class="password-wrapper">
        <asp:TextBox ID="txtCurrentPassword" runat="server"
            TextMode="Password" CssClass="form-control password-input"
            MaxLength="100" autocomplete="current-password"
            placeholder="Enter temporary password">
        </asp:TextBox>

        <button type="button" class="password-toggle"
            data-target="<%= txtCurrentPassword.ClientID %>"
            aria-label="Show current password"
            aria-pressed="false">Show</button>
    </div>

    <asp:RequiredFieldValidator ID="rfvCurrentPassword"
        runat="server" ControlToValidate="txtCurrentPassword"
        ErrorMessage="Current password is required."
        CssClass="field-error" Display="Dynamic" />
</div>

<div class="form-group">
    <asp:Label ID="lblNewPassword" runat="server"
        AssociatedControlID="txtNewPassword"
        CssClass="form-label" Text="New Password" />

    <div class="password-wrapper">
        <asp:TextBox ID="txtNewPassword" runat="server"
            TextMode="Password" CssClass="form-control password-input"
            MaxLength="12" autocomplete="new-password"
            placeholder="Create a new password">
        </asp:TextBox>

        <button type="button" class="password-toggle"
            data-target="<%= txtNewPassword.ClientID %>"
            aria-label="Show new password"
            aria-pressed="false">Show</button>
    </div>

    <asp:RequiredFieldValidator ID="rfvNewPassword"
        runat="server" ControlToValidate="txtNewPassword"
        ErrorMessage="New password is required."
        CssClass="field-error" Display="Dynamic" />

    <asp:RegularExpressionValidator ID="revNewPassword"
        runat="server" ControlToValidate="txtNewPassword"
        ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,12}$"
        ErrorMessage="Use 8–12 characters with uppercase, lowercase, a number and a special character."
        CssClass="field-error" Display="Dynamic" />
</div>

<div class="form-group">
    <asp:Label ID="lblConfirmPassword" runat="server"
        AssociatedControlID="txtConfirmPassword"
        CssClass="form-label" Text="Confirm New Password" />

    <div class="password-wrapper">
        <asp:TextBox ID="txtConfirmPassword" runat="server"
            TextMode="Password" CssClass="form-control password-input"
            MaxLength="12" autocomplete="new-password"
            placeholder="Re-enter new password">
        </asp:TextBox>

        <button type="button" class="password-toggle"
            data-target="<%= txtConfirmPassword.ClientID %>"
            aria-label="Show confirmed password"
            aria-pressed="false">Show</button>
    </div>

    <asp:RequiredFieldValidator ID="rfvConfirmPassword"
        runat="server" ControlToValidate="txtConfirmPassword"
        ErrorMessage="Please confirm your new password."
        CssClass="field-error" Display="Dynamic" />

    <asp:CompareValidator ID="cvConfirmPassword"
        runat="server" ControlToValidate="txtConfirmPassword"
        ControlToCompare="txtNewPassword"
        ErrorMessage="Passwords do not match."
        CssClass="field-error" Display="Dynamic" />
</div>

<asp:Button ID="btnChangePassword" runat="server"
    Text="Update Password" CssClass="primary-button"
    OnClick="btnChangePassword_Click" />


                <p class="security-footer">
                    Your new password will be stored securely.
                </p>
            </section>

            <footer class="page-footer">
                Enhanced USP Online Grievance Redressal System
            </footer>
        </main>
        
<script>
    (function () {
        var buttons = document.querySelectorAll('.password-toggle');

        buttons.forEach(function (button) {
            button.addEventListener('click', function () {
                var field = document.getElementById(
                    button.getAttribute('data-target'));

                if (!field) return;

                var showPassword = field.type === 'password';

                field.type = showPassword ? 'text' : 'password';
                button.textContent = showPassword ? 'Hide' : 'Show';
                button.setAttribute(
                    'aria-pressed',
                    showPassword ? 'true' : 'false');

                var label = button.getAttribute('aria-label') || '';
                button.setAttribute(
                    'aria-label',
                    (showPassword ? 'Hide ' : 'Show ') +
                    label.replace(/^(Show|Hide) /, ''));
            });
        });
    })();
</script>

    </form>
</body>
</html>
