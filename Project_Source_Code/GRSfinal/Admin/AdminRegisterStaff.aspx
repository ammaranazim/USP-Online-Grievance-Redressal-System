<%@ Page Title="Register Staff"
    Language="C#"
    MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true"
    CodeFile="AdminRegisterStaff.aspx.cs"
    Inherits="Admin_AdminRegisterStaff" %>

<asp:Content ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">
    <link href="css/AdminRegisterStaff.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="admin-register-staff">

        <div class="register-page-header">
            <div>
                <h1>Register New Staff</h1>
                <p>Create a staff account and assign it to a department.</p>
            </div>

            <asp:HyperLink ID="lnkBackToStaff" runat="server"
                NavigateUrl="~/Admin/AdminStaff.aspx"
                CssClass="register-back-btn">
                &larr; Back to Staff
            </asp:HyperLink>
        </div>

        <div class="register-staff-card">

            <div class="register-card-heading">
                <h2>Staff Information</h2>
                <p>Enter the staff member's details below.</p>
            </div>

            <asp:Label ID="lblMessage" runat="server"
                CssClass="register-message"
                EnableViewState="false" />

            <asp:ValidationSummary ID="vsStaff" runat="server"
                ValidationGroup="StaffRegistration"
                CssClass="register-validation-summary"
                HeaderText="Please correct the following:"
                DisplayMode="BulletList" />

            <div class="register-form-grid">

                <div class="register-field">
                    <asp:Label ID="lblStaffName" runat="server"
                        AssociatedControlID="txtStaffName"
                        Text="Full Name *" />

                    <asp:TextBox ID="txtStaffName" runat="server"
                        CssClass="register-input"
                        MaxLength="100"
                        placeholder="Enter full name" />

                    <asp:RequiredFieldValidator ID="rfvStaffName"
                        runat="server"
                        ControlToValidate="txtStaffName"
                        ValidationGroup="StaffRegistration"
                        ErrorMessage="Full name is required."
                        CssClass="register-field-error"
                        Display="Dynamic" />
                </div>

                <div class="register-field">
                    <asp:Label ID="lblUsername" runat="server"
                        AssociatedControlID="txtUsername"
                        Text="Username *" />

                    <asp:TextBox ID="txtUsername" runat="server"
                        CssClass="register-input"
                        MaxLength="50"
                        placeholder="Create a unique username" />

                    <asp:RequiredFieldValidator ID="rfvUsername"
                        runat="server"
                        ControlToValidate="txtUsername"
                        ValidationGroup="StaffRegistration"
                        ErrorMessage="Username is required."
                        CssClass="register-field-error"
                        Display="Dynamic" />
                </div>

                <div class="register-field">
                    <asp:Label ID="lblEmail" runat="server"
                        AssociatedControlID="txtEmail"
                        Text="Email Address *" />

                    <asp:TextBox ID="txtEmail" runat="server"
                        CssClass="register-input"
                        TextMode="Email"
                        MaxLength="100"
                        placeholder="name@example.com" />

                    <asp:RequiredFieldValidator ID="rfvEmail"
                        runat="server"
                        ControlToValidate="txtEmail"
                        ValidationGroup="StaffRegistration"
                        ErrorMessage="Email address is required."
                        CssClass="register-field-error"
                        Display="Dynamic" />

                    <asp:RegularExpressionValidator ID="revEmail"
                        runat="server"
                        ControlToValidate="txtEmail"
                        ValidationGroup="StaffRegistration"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                        ErrorMessage="Enter a valid email address."
                        CssClass="register-field-error"
                        Display="Dynamic" />
                </div>

                <div class="register-field">
                    <asp:Label ID="lblPhone" runat="server"
                        AssociatedControlID="txtPhone"
                        Text="Phone Number *" />

                    <asp:TextBox ID="txtPhone" runat="server"
                        CssClass="register-input"
                        MaxLength="50"
                        placeholder="Enter phone number" />

                    <asp:RequiredFieldValidator ID="rfvPhone"
                        runat="server"
                        ControlToValidate="txtPhone"
                        ValidationGroup="StaffRegistration"
                        ErrorMessage="Phone number is required."
                        CssClass="register-field-error"
                        Display="Dynamic" />
                </div>

                <div class="register-field">
                    <asp:Label ID="lblDesignation" runat="server"
                        AssociatedControlID="txtDesignation"
                        Text="Designation *" />

                    <asp:TextBox ID="txtDesignation" runat="server"
                        CssClass="register-input"
                        MaxLength="50"
                        placeholder="e.g. Lecturer, Lab Assistant" />

                    <asp:RequiredFieldValidator ID="rfvDesignation"
                        runat="server"
                        ControlToValidate="txtDesignation"
                        ValidationGroup="StaffRegistration"
                        ErrorMessage="Designation is required."
                        CssClass="register-field-error"
                        Display="Dynamic" />
                </div>

                <div class="register-field">
                    <asp:Label ID="lblDepartment" runat="server"
                        AssociatedControlID="ddlDepartment"
                        Text="Department *" />

                    <asp:DropDownList ID="ddlDepartment" runat="server"
                        CssClass="register-select"
                        AppendDataBoundItems="false">
                        <asp:ListItem Text="Select Department"
                            Value="" />
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="rfvDepartment"
                        runat="server"
                        ControlToValidate="ddlDepartment"
                        InitialValue=""
                        ValidationGroup="StaffRegistration"
                        ErrorMessage="Please select a department."
                        CssClass="register-field-error"
                        Display="Dynamic" />
                </div>

              

                

            </div>

            <div class="register-form-footer">
                <asp:HyperLink ID="lnkCancel" runat="server"
                    NavigateUrl="~/Admin/AdminStaff.aspx"
                    CssClass="register-cancel-btn">
                    Cancel
                </asp:HyperLink>

                <asp:Button ID="btnRegisterStaff" runat="server"
                    Text="Register Staff"
                    CssClass="register-submit-btn"
                    ValidationGroup="StaffRegistration"
                    OnClick="btnRegisterStaff_Click" />
            </div>

        </div>
    </div>

</asp:Content>