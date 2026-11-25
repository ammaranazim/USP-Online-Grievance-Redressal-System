<%@ Page Title="" Language="C#" MasterPageFile="~/Student/StudentMaster.master" AutoEventWireup="true" CodeFile="StudentChangePassword.aspx.cs" Inherits="Student_StudentChangePassword" %>

<asp:Content ID="HeadSection"
    ContentPlaceHolderID="HeadContent"
    runat="server">

    <link href="<%= ResolveUrl("~/Student/css/StudentChangePassword.css") %>"
        rel="stylesheet" />

</asp:Content>

<asp:Content ID="MainSection"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="change-password-page">

        <div class="change-password-card">

            <div class="change-password-header">
                <h2>Change Password</h2>
                <p>
                    Create a new password to secure your student account.
                </p>
            </div>

            <asp:Label ID="lblMessage"
                runat="server"
                CssClass="password-message"
                EnableViewState="false">
            </asp:Label>

            <div class="password-form-group">
                <asp:Label ID="lblCurrentPassword"
                    runat="server"
                    AssociatedControlID="txtCurrentPassword"
                    Text="Current Password">
                </asp:Label>

                <asp:TextBox ID="txtCurrentPassword"
                    runat="server"
                    TextMode="Password"
                    CssClass="password-input"
                    MaxLength="128"
                    autocomplete="current-password">
                </asp:TextBox>

                <asp:RequiredFieldValidator
                    ID="rfvCurrentPassword"
                    runat="server"
                    ControlToValidate="txtCurrentPassword"
                    ErrorMessage="Enter your current password."
                    CssClass="password-validation"
                    Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>

            <div class="password-form-group">
                <asp:Label ID="lblNewPassword"
                    runat="server"
                    AssociatedControlID="txtNewPassword"
                    Text="New Password">
                </asp:Label>

                <asp:TextBox ID="txtNewPassword"
                    runat="server"
                    TextMode="Password"
                    CssClass="password-input"
                    MaxLength="128"
                    autocomplete="new-password">
                </asp:TextBox>

                <asp:RequiredFieldValidator
                    ID="rfvNewPassword"
                    runat="server"
                    ControlToValidate="txtNewPassword"
                    ErrorMessage="Enter a new password."
                    CssClass="password-validation"
                    Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>

            <div class="password-form-group">
                <asp:Label ID="lblConfirmPassword"
                    runat="server"
                    AssociatedControlID="txtConfirmPassword"
                    Text="Confirm New Password">
                </asp:Label>

                <asp:TextBox ID="txtConfirmPassword"
                    runat="server"
                    TextMode="Password"
                    CssClass="password-input"
                    MaxLength="128"
                    autocomplete="new-password">
                </asp:TextBox>

                <asp:RequiredFieldValidator
                    ID="rfvConfirmPassword"
                    runat="server"
                    ControlToValidate="txtConfirmPassword"
                    ErrorMessage="Confirm your new password."
                    CssClass="password-validation"
                    Display="Dynamic">
                </asp:RequiredFieldValidator>

                <asp:CompareValidator
                    ID="cvConfirmPassword"
                    runat="server"
                    ControlToValidate="txtConfirmPassword"
                    ControlToCompare="txtNewPassword"
                    ErrorMessage="Passwords do not match."
                    CssClass="password-validation"
                    Display="Dynamic">
                </asp:CompareValidator>
            </div>

            <p class="password-hint">
                Use a strong password of at least 8 characters.
            </p>

            <div class="password-actions">
                <asp:Button ID="btnUpdatePassword"
                    runat="server"
                    Text="Update Password"
                    CssClass="password-submit-button"
                    OnClick="btnUpdatePassword_Click" />
            </div>

        </div>

    </section>

</asp:Content>
