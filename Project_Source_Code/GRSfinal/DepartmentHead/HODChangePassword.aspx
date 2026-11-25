<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="HODChangePassword.aspx.cs"
    Inherits="DepartmentHead_HODChangePassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Change Password | USP</title>

    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap"
        rel="stylesheet" />

    <link href="css/DepartmentHeadChangePassword.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <main class="change-password-page">
            <section class="change-password-card">

                <div class="brand-section">
                    <div class="brand-logo">USP</div>
                    <div>
                        <h1>University of Southern Punjab</h1>
                        <p>Multan, Pakistan</p>
                    </div>
                </div>

                <div class="page-heading">
                    <span class="page-tag">ACCOUNT SECURITY</span>
                    <h2>Change Your Password</h2>
                    <p>
                        For your account security, create a new password
                        before accessing your department dashboard.
                    </p>
                </div>

                <div class="security-notice">
                    <strong>First login password update required</strong>
                    <p>
                        You are using a temporary password. Please replace it
                        with a new password that only you know.
                    </p>
                </div>

                <asp:Label
                    ID="lblMessage"
                    runat="server"
                    EnableViewState="false"
                    CssClass="form-message">
                </asp:Label>

                <div class="form-group">
                    <asp:Label
                        ID="lblCurrentPassword"
                        runat="server"
                        AssociatedControlID="txtCurrentPassword"
                        CssClass="form-label"
                        Text="Current Password">
                    </asp:Label>

                    <asp:TextBox
                        ID="txtCurrentPassword"
                        runat="server"
                        TextMode="Password"
                        CssClass="form-control"
                        MaxLength="100"
                        placeholder="Enter temporary password">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvCurrentPassword"
                        runat="server"
                        ControlToValidate="txtCurrentPassword"
                        ErrorMessage="Current password is required."
                        CssClass="field-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <asp:Label
                        ID="lblNewPassword"
                        runat="server"
                        AssociatedControlID="txtNewPassword"
                        CssClass="form-label"
                        Text="New Password">
                    </asp:Label>

                    <asp:TextBox
                        ID="txtNewPassword"
                        runat="server"
                        TextMode="Password"
                        CssClass="form-control"
                        MaxLength="100"
                        placeholder="Create a new password">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvNewPassword"
                        runat="server"
                        ControlToValidate="txtNewPassword"
                        ErrorMessage="New password is required."
                        CssClass="field-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                    <asp:RegularExpressionValidator
                        ID="revNewPassword"
                        runat="server"
                        ControlToValidate="txtNewPassword"
                        ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,100}$"
                        ErrorMessage="Use at least 8 characters with uppercase, lowercase, a number and a special character."
                        CssClass="field-error"
                        Display="Dynamic">
                    </asp:RegularExpressionValidator>
                </div>

                <div class="form-group">
                    <asp:Label
                        ID="lblConfirmPassword"
                        runat="server"
                        AssociatedControlID="txtConfirmPassword"
                        CssClass="form-label"
                        Text="Confirm New Password">
                    </asp:Label>

                    <asp:TextBox
                        ID="txtConfirmPassword"
                        runat="server"
                        TextMode="Password"
                        CssClass="form-control"
                        MaxLength="100"
                        placeholder="Re-enter new password">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvConfirmPassword"
                        runat="server"
                        ControlToValidate="txtConfirmPassword"
                        ErrorMessage="Please confirm your new password."
                        CssClass="field-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                    <asp:CompareValidator
                        ID="cvConfirmPassword"
                        runat="server"
                        ControlToValidate="txtConfirmPassword"
                        ControlToCompare="txtNewPassword"
                        ErrorMessage="Passwords do not match."
                        CssClass="field-error"
                        Display="Dynamic">
                    </asp:CompareValidator>
                </div>

                <asp:Button
                    ID="btnChangePassword"
                    runat="server"
                    Text="Update Password"
                    CssClass="primary-button"
                    OnClick="btnChangePassword_Click" />

                <p class="security-footer">
                    Your new password will be stored securely.
                </p>

            </section>

            <footer class="page-footer">
                Enhanced USP Online Grievance Redressal System
            </footer>
        </main>

    </form>
</body>
</html>