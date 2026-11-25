
<%@ Page Title="Edit Staff"
    Language="C#"
    MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true"
    CodeFile="AdminEditStaff.aspx.cs"
    Inherits="Admin_AdminEditStaff" %>

<asp:Content ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">
    <link href="css/AdminEditStaff.css?v=1" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="admin-edit-staff">

        <div class="edit-staff-header">
            <div>
                <h1>Edit Staff</h1>
                <p>Update staff information, department and account status.</p>
            </div>

            <asp:HyperLink ID="lnkBack" runat="server"
                NavigateUrl="~/Admin/AdminStaff.aspx"
                CssClass="edit-staff-back">
                &larr; Back to Staff
            </asp:HyperLink>
        </div>

        <div class="edit-staff-card">

            <div class="edit-staff-card-heading">
                <h2>Staff Information</h2>
                <p>Update the details of the selected staff member.</p>
            </div>

            <asp:Label ID="lblMessage" runat="server"
                CssClass="edit-staff-message"
                EnableViewState="false" />

            <asp:ValidationSummary ID="vsEditStaff" runat="server"
                ValidationGroup="EditStaff"
                CssClass="edit-staff-validation-summary"
                HeaderText="Please correct the following:"
                DisplayMode="BulletList" />

            <asp:HiddenField ID="hfStaffID" runat="server" />

            <div class="edit-staff-grid">

                <div class="edit-staff-field">
                    <asp:Label ID="lblStaffID" runat="server"
                        AssociatedControlID="txtStaffID"
                        Text="Staff ID" />
                    <asp:TextBox ID="txtStaffID" runat="server"
                        CssClass="edit-staff-input"
                        ReadOnly="true" />
                </div>

                <div class="edit-staff-field">
                    <asp:Label ID="lblStaffName" runat="server"
                        AssociatedControlID="txtStaffName"
                        Text="Full Name *" />
                    <asp:TextBox ID="txtStaffName" runat="server"
                        CssClass="edit-staff-input"
                        MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvStaffName" runat="server"
                        ControlToValidate="txtStaffName"
                        ValidationGroup="EditStaff"
                        ErrorMessage="Full name is required."
                        CssClass="edit-staff-field-error"
                        Display="Dynamic" />
                </div>

                <div class="edit-staff-field">
                    <asp:Label ID="lblUsername" runat="server"
                        AssociatedControlID="txtUsername"
                        Text="Username *" />
                    <asp:TextBox ID="txtUsername" runat="server"
                        CssClass="edit-staff-input"
                        MaxLength="50" />
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server"
                        ControlToValidate="txtUsername"
                        ValidationGroup="EditStaff"
                        ErrorMessage="Username is required."
                        CssClass="edit-staff-field-error"
                        Display="Dynamic" />
                </div>

                <div class="edit-staff-field">
                    <asp:Label ID="lblEmail" runat="server"
                        AssociatedControlID="txtEmail"
                        Text="Email Address *" />
                    <asp:TextBox ID="txtEmail" runat="server"
                        CssClass="edit-staff-input"
                        TextMode="Email"
                        MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                        ControlToValidate="txtEmail"
                        ValidationGroup="EditStaff"
                        ErrorMessage="Email address is required."
                        CssClass="edit-staff-field-error"
                        Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server"
                        ControlToValidate="txtEmail"
                        ValidationGroup="EditStaff"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                        ErrorMessage="Enter a valid email address."
                        CssClass="edit-staff-field-error"
                        Display="Dynamic" />
                </div>

                <div class="edit-staff-field">
                    <asp:Label ID="lblPhone" runat="server"
                        AssociatedControlID="txtPhone"
                        Text="Phone Number *" />
                    <asp:TextBox ID="txtPhone" runat="server"
                        CssClass="edit-staff-input"
                        MaxLength="50" />
                    <asp:RequiredFieldValidator ID="rfvPhone" runat="server"
                        ControlToValidate="txtPhone"
                        ValidationGroup="EditStaff"
                        ErrorMessage="Phone number is required."
                        CssClass="edit-staff-field-error"
                        Display="Dynamic" />
                </div>

                <div class="edit-staff-field">
                    <asp:Label ID="lblDesignation" runat="server"
                        AssociatedControlID="txtDesignation"
                        Text="Designation *" />
                    <asp:TextBox ID="txtDesignation" runat="server"
                        CssClass="edit-staff-input"
                        MaxLength="50" />
                    <asp:RequiredFieldValidator ID="rfvDesignation" runat="server"
                        ControlToValidate="txtDesignation"
                        ValidationGroup="EditStaff"
                        ErrorMessage="Designation is required."
                        CssClass="edit-staff-field-error"
                        Display="Dynamic" />
                </div>

                <div class="edit-staff-field">
                    <asp:Label ID="lblDepartment" runat="server"
                        AssociatedControlID="ddlDepartment"
                        Text="Department *" />
                    <asp:DropDownList ID="ddlDepartment" runat="server"
                        CssClass="edit-staff-select">
                        <asp:ListItem Text="Select Department" Value="" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvDepartment" runat="server"
                        ControlToValidate="ddlDepartment"
                        InitialValue=""
                        ValidationGroup="EditStaff"
                        ErrorMessage="Please select a department."
                        CssClass="edit-staff-field-error"
                        Display="Dynamic" />
                </div>

                <div class="edit-staff-field">
                    <asp:Label ID="lblStatus" runat="server"
                        AssociatedControlID="ddlStatus"
                        Text="Account Status *" />
                    <asp:DropDownList ID="ddlStatus" runat="server"
                        CssClass="edit-staff-select">
                        <asp:ListItem Text="Active" Value="Active" />
                        <asp:ListItem Text="Inactive" Value="Inactive" />
                    </asp:DropDownList>
                </div>

            </div>

            <div class="edit-staff-info">
                Password information is not editable here. Existing password
                and first-login password-change settings will be preserved.
            </div>

            <div class="edit-staff-actions">
                <asp:HyperLink ID="lnkCancel" runat="server"
                    NavigateUrl="~/Admin/AdminStaff.aspx"
                    CssClass="edit-staff-cancel">
                    Cancel
                </asp:HyperLink>

                <asp:Button ID="btnSaveChanges" runat="server"
                    Text="Save Changes"
                    CssClass="edit-staff-submit"
                    ValidationGroup="EditStaff"
                    OnClick="btnSaveChanges_Click" />
            </div>

        </div>
    </div>

</asp:Content>
