<%@ Page Title="Register Department Head"
    Language="C#"
    MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true"
    CodeFile="AdminRegisterDepartmentHead.aspx.cs"
    Inherits="Admin_AdminRegisterDepartmentHead" %>

<asp:Content ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

    <link href="css/AdminRegisterDepartmentHead.css"
        rel="stylesheet" />

</asp:Content>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="register-hod-page">

        <!-- Page Header -->
        <div class="register-hod-header">
            <div>
                <h1>Register Department Head</h1>
                <p>Create a department head account and assign a department.</p>
            </div>

            <asp:HyperLink
                ID="lnkBackToDepartmentHeads"
                runat="server"
                NavigateUrl="~/Admin/AdminDepartmentHeads.aspx"
                CssClass="register-hod-back-btn">
                Back to Department Heads
            </asp:HyperLink>
        </div>

        <!-- Message -->
        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="register-hod-message"
            EnableViewState="false" />

        <!-- Registration Form -->
        <div class="register-hod-card">

            <div class="register-hod-card-header">
                <h2>Department Head Information</h2>
                <p>Enter the details below to create the account.</p>
            </div>

            <div class="register-hod-form">

                <!-- Personal Information -->
                <div class="register-hod-section">
                    <h3>Personal Information</h3>

                    <div class="register-hod-grid">

                        <div class="register-hod-field">
                            <asp:Label
                                ID="lblHodName"
                                runat="server"
                                AssociatedControlID="txtHodName"
                                Text="Full Name *" />

                            <asp:TextBox
                                ID="txtHodName"
                                runat="server"
                                CssClass="register-hod-input"
                                MaxLength="50"
                                placeholder="Enter full name" />

                            <asp:RequiredFieldValidator
                                ID="rfvHodName"
                                runat="server"
                                ControlToValidate="txtHodName"
                                ErrorMessage="Full name is required."
                                CssClass="register-hod-validation"
                                Display="Dynamic" />
                        </div>

                        <div class="register-hod-field">
                            <asp:Label
                                ID="lblDesignation"
                                runat="server"
                                AssociatedControlID="txtDesignation"
                                Text="Designation *" />

                            <asp:TextBox
                                ID="txtDesignation"
                                runat="server"
                                CssClass="register-hod-input"
                                MaxLength="50"
                                placeholder="e.g. Department Head" />

                            <asp:RequiredFieldValidator
                                ID="rfvDesignation"
                                runat="server"
                                ControlToValidate="txtDesignation"
                                ErrorMessage="Designation is required."
                                CssClass="register-hod-validation"
                                Display="Dynamic" />
                        </div>

                    </div>
                </div>

                <!-- Contact Information -->
                <div class="register-hod-section">
                    <h3>Contact Information</h3>

                    <div class="register-hod-grid">

                        <div class="register-hod-field">
                            <asp:Label
                                ID="lblEmail"
                                runat="server"
                                AssociatedControlID="txtEmail"
                                Text="Email Address *" />

                            <asp:TextBox
                                ID="txtEmail"
                                runat="server"
                                CssClass="register-hod-input"
                                TextMode="Email"
                                MaxLength="50"
                                placeholder="Enter email address" />

                            <asp:RequiredFieldValidator
                                ID="rfvEmail"
                                runat="server"
                                ControlToValidate="txtEmail"
                                ErrorMessage="Email address is required."
                                CssClass="register-hod-validation"
                                Display="Dynamic" />

                            <asp:RegularExpressionValidator
                                ID="revEmail"
                                runat="server"
                                ControlToValidate="txtEmail"
                                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                                ErrorMessage="Enter a valid email address."
                                CssClass="register-hod-validation"
                                Display="Dynamic" />
                        </div>

                        <div class="register-hod-field">
                            <asp:Label
                                ID="lblPhone"
                                runat="server"
                                AssociatedControlID="txtPhone"
                                Text="Phone Number *" />

                            <asp:TextBox
                                ID="txtPhone"
                                runat="server"
                                CssClass="register-hod-input"
                                MaxLength="50"
                                placeholder="Enter phone number" />

                            <asp:RequiredFieldValidator
                                ID="rfvPhone"
                                runat="server"
                                ControlToValidate="txtPhone"
                                ErrorMessage="Phone number is required."
                                CssClass="register-hod-validation"
                                Display="Dynamic" />
                        </div>

                    </div>
                </div>

                <!-- Account Information -->
                <div class="register-hod-section">
                    <h3>Account Information</h3>

                    <div class="register-hod-grid">

                        <div class="register-hod-field">
                            <asp:Label
                                ID="lblUsername"
                                runat="server"
                                AssociatedControlID="txtUsername"
                                Text="Username *" />

                            <asp:TextBox
                                ID="txtUsername"
                                runat="server"
                                CssClass="register-hod-input"
                                MaxLength="50"
                                placeholder="Choose a username" />

                            <asp:RequiredFieldValidator
                                ID="rfvUsername"
                                runat="server"
                                ControlToValidate="txtUsername"
                                ErrorMessage="Username is required."
                                CssClass="register-hod-validation"
                                Display="Dynamic" />
                        </div>

                        <div class="register-hod-field">
                            <asp:Label
                                ID="lblDepartment"
                                runat="server"
                                AssociatedControlID="ddlDepartment"
                                Text="Department *" />

                            <asp:DropDownList
                                ID="ddlDepartment"
                                runat="server"
                                CssClass="register-hod-select"
                                AppendDataBoundItems="true">
                                <asp:ListItem
                                    Text="Select Department"
                                    Value="" />
                            </asp:DropDownList>

                            <asp:RequiredFieldValidator
                                ID="rfvDepartment"
                                runat="server"
                                ControlToValidate="ddlDepartment"
                                InitialValue=""
                                ErrorMessage="Please select a department."
                                CssClass="register-hod-validation"
                                Display="Dynamic" />
                        </div>

                        <div class="register-hod-field">
                            <asp:Label
                                ID="lblStatus"
                                runat="server"
                                AssociatedControlID="ddlStatus"
                                Text="Account Status *" />

                            <asp:DropDownList
                                ID="ddlStatus"
                                runat="server"
                                CssClass="register-hod-select">
                                <asp:ListItem
                                    Text="Active"
                                    Value="active"
                                    Selected="True" />
                                <asp:ListItem
                                    Text="Inactive"
                                    Value="inactive" />
                            </asp:DropDownList>
                        </div>

                    </div>

                    <div class="register-hod-info">
                        A temporary password will be generated after registration.
                        Share it privately with the department head.
                    </div>
                </div>

                <!-- Form Actions -->
                <div class="register-hod-actions">

                    <asp:HyperLink
                        ID="lnkCancel"
                        runat="server"
                        NavigateUrl="~/Admin/AdminDepartmentHeads.aspx"
                        CssClass="register-hod-cancel-btn">
                        Cancel
                    </asp:HyperLink>

                    <asp:Button
                        ID="btnRegister"
                        runat="server"
                        Text="Register Department Head"
                        CssClass="register-hod-submit-btn"
                        OnClick="btnRegister_Click" />

                </div>

            </div>
        </div>

    </div>

</asp:Content>