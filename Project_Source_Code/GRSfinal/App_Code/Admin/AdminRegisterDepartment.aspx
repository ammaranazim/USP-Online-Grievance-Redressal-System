<%@ Page Title="Register Department" Language="C#" MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true" CodeFile="AdminRegisterDepartment.aspx.cs"
    Inherits="Admin_AdminRegisterDepartment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="css/AdminRegisterDepartment.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <div class="admin-register-department-page">

        <div class="department-form-header">
            <div>
                <h1>Register Department</h1>
                <p>Add a new department and assign it to a university category.</p>
            </div>

            <asp:HyperLink
                ID="lnkBackToDepartments"
                runat="server"
                NavigateUrl="~/Admin/AdminDepartments.aspx"
                CssClass="department-back-btn">
                Back to Departments
            </asp:HyperLink>
        </div>

        <asp:Label ID="lblMessage" runat="server"
            CssClass="department-form-message"
            EnableViewState="false" />

        <asp:ValidationSummary
            ID="vsDepartment"
            runat="server"
            ValidationGroup="DepartmentForm"
            CssClass="department-validation-summary"
            HeaderText="Please correct the following:" />

        <div class="department-form-card">

            <div class="department-form-section">
                <h2>Department Information</h2>
                <p>Enter the department's basic information and category.</p>

                <div class="department-form-grid">

                    <div class="department-form-group">
                        <asp:Label ID="lblDepartmentName" runat="server"
                            AssociatedControlID="txtDepartmentName"
                            Text="Department Name *" />

                        <asp:TextBox ID="txtDepartmentName" runat="server"
                            CssClass="department-form-control"
                            MaxLength="50"
                            placeholder="Enter department name" />

                        <asp:RequiredFieldValidator ID="rfvDepartmentName"
                            runat="server"
                            ControlToValidate="txtDepartmentName"
                            ErrorMessage="Department name is required."
                            ValidationGroup="DepartmentForm"
                            CssClass="department-field-error"
                            Display="Dynamic" />
                    </div>

                    <div class="department-form-group">
                        <asp:Label ID="lblDepartmentCode" runat="server"
                            AssociatedControlID="txtDepartmentCode"
                            Text="Department Code *" />

                        <asp:TextBox ID="txtDepartmentCode" runat="server"
                            CssClass="department-form-control"
                            MaxLength="50"
                            placeholder="Enter department code" />

                        <asp:RequiredFieldValidator ID="rfvDepartmentCode"
                            runat="server"
                            ControlToValidate="txtDepartmentCode"
                            ErrorMessage="Department code is required."
                            ValidationGroup="DepartmentForm"
                            CssClass="department-field-error"
                            Display="Dynamic" />
                    </div>

                    <div class="department-form-group">
                        <asp:Label ID="lblCategory" runat="server"
                            AssociatedControlID="ddlCategory"
                            Text="Category *" />

                        <asp:DropDownList ID="ddlCategory" runat="server"
                            CssClass="department-form-control">
                            <asp:ListItem Text="Select Category" Value="" />
                        </asp:DropDownList>

                        <asp:RequiredFieldValidator ID="rfvCategory"
                            runat="server"
                            ControlToValidate="ddlCategory"
                            InitialValue=""
                            ErrorMessage="Please select a category."
                            ValidationGroup="DepartmentForm"
                            CssClass="department-field-error"
                            Display="Dynamic" />
                    </div>

                    <div class="department-form-group">
                        <asp:Label ID="lblStatus" runat="server"
                            AssociatedControlID="ddlStatus"
                            Text="Status *" />

                        <asp:DropDownList ID="ddlStatus" runat="server"
                            CssClass="department-form-control">
                            <asp:ListItem Text="Active" Value="Active" />
                            <asp:ListItem Text="Inactive" Value="Inactive" />
                        </asp:DropDownList>
                    </div>

                    <div class="department-form-group department-form-group-full">
                        <asp:Label ID="lblDescription" runat="server"
                            AssociatedControlID="txtDescription"
                            Text="Description" />

                        <asp:TextBox ID="txtDescription" runat="server"
                            CssClass="department-form-control department-textarea"
                            TextMode="MultiLine"
                            Rows="3"
                            MaxLength="50"
                            placeholder="Enter a brief department description" />
                    </div>

                </div>
            </div>

            <div class="department-form-section">
                <h2>Contact & Location</h2>
                <p>Provide the department's contact and operational details.</p>

                <div class="department-form-grid">

                    <div class="department-form-group">
                        <asp:Label ID="lblEmail" runat="server"
                            AssociatedControlID="txtEmail"
                            Text="Department Email *" />

                        <asp:TextBox ID="txtEmail" runat="server"
                            CssClass="department-form-control"
                            MaxLength="50"
                            TextMode="Email"
                            placeholder="department@university.edu.pk" />

                        <asp:RequiredFieldValidator ID="rfvEmail"
                            runat="server"
                            ControlToValidate="txtEmail"
                            ErrorMessage="Department email is required."
                            ValidationGroup="DepartmentForm"
                            CssClass="department-field-error"
                            Display="Dynamic" />

                        <asp:RegularExpressionValidator ID="revEmail"
                            runat="server"
                            ControlToValidate="txtEmail"
                            ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                            ErrorMessage="Enter a valid email address."
                            ValidationGroup="DepartmentForm"
                            CssClass="department-field-error"
                            Display="Dynamic" />
                    </div>

                    <div class="department-form-group">
                        <asp:Label ID="lblContact" runat="server"
                            AssociatedControlID="txtContact"
                            Text="Contact Number *" />

                        <asp:TextBox ID="txtContact" runat="server"
                            CssClass="department-form-control"
                            MaxLength="50"
                            placeholder="Enter contact number" />

                        <asp:RequiredFieldValidator ID="rfvContact"
                            runat="server"
                            ControlToValidate="txtContact"
                            ErrorMessage="Contact number is required."
                            ValidationGroup="DepartmentForm"
                            CssClass="department-field-error"
                            Display="Dynamic" />
                    </div>

                    <div class="department-form-group">
                        <asp:Label ID="lblWorkingHours" runat="server"
                            AssociatedControlID="txtWorkingHours"
                            Text="Working Hours *" />

                        <asp:TextBox ID="txtWorkingHours" runat="server"
                            CssClass="department-form-control"
                            TextMode="Number"
                            placeholder="e.g. 8" />

                        <asp:RequiredFieldValidator ID="rfvWorkingHours"
                            runat="server"
                            ControlToValidate="txtWorkingHours"
                            ErrorMessage="Working hours are required."
                            ValidationGroup="DepartmentForm"
                            CssClass="department-field-error"
                            Display="Dynamic" />

                        <asp:RangeValidator ID="rvWorkingHours"
                            runat="server"
                            ControlToValidate="txtWorkingHours"
                            MinimumValue="1"
                            MaximumValue="24"
                            Type="Integer"
                            ErrorMessage="Working hours must be between 1 and 24."
                            ValidationGroup="DepartmentForm"
                            CssClass="department-field-error"
                            Display="Dynamic" />
                    </div>

                    <div class="department-form-group">
                        <asp:Label ID="lblLocation" runat="server"
                            AssociatedControlID="txtLocation"
                            Text="Location *" />

                        <asp:TextBox ID="txtLocation" runat="server"
                            CssClass="department-form-control"
                            MaxLength="50"
                            placeholder="Enter department location" />

                        <asp:RequiredFieldValidator ID="rfvLocation"
                            runat="server"
                            ControlToValidate="txtLocation"
                            ErrorMessage="Department location is required."
                            ValidationGroup="DepartmentForm"
                            CssClass="department-field-error"
                            Display="Dynamic" />
                    </div>

                </div>
            </div>

            <div class="department-form-actions">

                <asp:HyperLink ID="lnkCancel" runat="server"
                    NavigateUrl="~/Admin/AdminDepartments.aspx"
                    CssClass="department-cancel-btn">
                    Cancel
                </asp:HyperLink>

                <asp:Button ID="btnRegisterDepartment" runat="server"
                    Text="Register Department"
                    CssClass="department-submit-btn"
                    ValidationGroup="DepartmentForm"
                    OnClick="btnRegisterDepartment_Click" />

            </div>

        </div>

    </div>

</asp:Content>