<%@ Page Title="Edit Department" Language="C#"
    MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true"
    CodeFile="AdminEditDepartment.aspx.cs"
    Inherits="Admin_AdminEditDepartment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="css/AdminEditDepartment.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="admin-edit-department-page">

        <!-- Page Header -->
        <div class="edit-department-header">
            <div>
                <h1>Edit Department</h1>
                <p>Update department information and assigned category.</p>
            </div>

            <asp:HyperLink
                ID="lnkBackToDepartments"
                runat="server"
                NavigateUrl="~/Admin/AdminDepartments.aspx"
                CssClass="edit-department-back-btn">
                Back to Departments
            </asp:HyperLink>
        </div>

        <!-- Edit Department Form -->
        <div class="edit-department-card">

            <div class="edit-department-card-heading">
                <h2>Department Information</h2>
                <p>Update the details below and save your changes.</p>
            </div>

            <div class="edit-department-form">

                <asp:HiddenField ID="hfDepartmentID" runat="server" />

                <asp:Label
                    ID="lblMessage"
                    runat="server"
                    CssClass="edit-department-message"
                    EnableViewState="false" />

                <asp:ValidationSummary
                    ID="vsDepartment"
                    runat="server"
                    ValidationGroup="EditDepartment"
                    CssClass="edit-department-validation-summary"
                    HeaderText="Please correct the following:"
                    DisplayMode="BulletList" />

                <!-- Department Name -->
                <div class="edit-department-field">
                    <asp:Label
                        ID="lblDepartmentName"
                        runat="server"
                        AssociatedControlID="txtDepartmentName"
                        Text="Department Name *" />

                    <asp:TextBox
                        ID="txtDepartmentName"
                        runat="server"
                        CssClass="edit-department-input"
                        MaxLength="50" />

                    <asp:RequiredFieldValidator
                        ID="rfvDepartmentName"
                        runat="server"
                        ControlToValidate="txtDepartmentName"
                        ValidationGroup="EditDepartment"
                        ErrorMessage="Department name is required."
                        CssClass="edit-department-field-error"
                        Display="Dynamic" />
                </div>

                <!-- Department Code -->
                <div class="edit-department-field">
                    <asp:Label
                        ID="lblDepartmentCode"
                        runat="server"
                        AssociatedControlID="txtDepartmentCode"
                        Text="Department Code *" />

                    <asp:TextBox
                        ID="txtDepartmentCode"
                        runat="server"
                        CssClass="edit-department-input"
                        MaxLength="50" />

                    <asp:RequiredFieldValidator
                        ID="rfvDepartmentCode"
                        runat="server"
                        ControlToValidate="txtDepartmentCode"
                        ValidationGroup="EditDepartment"
                        ErrorMessage="Department code is required."
                        CssClass="edit-department-field-error"
                        Display="Dynamic" />
                </div>

                <!-- Category -->
                <div class="edit-department-field">
                    <asp:Label
                        ID="lblCategory"
                        runat="server"
                        AssociatedControlID="ddlCategory"
                        Text="Category *" />

                    <asp:DropDownList
                        ID="ddlCategory"
                        runat="server"
                        CssClass="edit-department-select">
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator
                        ID="rfvCategory"
                        runat="server"
                        ControlToValidate="ddlCategory"
                        InitialValue=""
                        ValidationGroup="EditDepartment"
                        ErrorMessage="Please select a category."
                        CssClass="edit-department-field-error"
                        Display="Dynamic" />
                </div>

                <!-- Status -->
                <div class="edit-department-field">
                    <asp:Label
                        ID="lblStatus"
                        runat="server"
                        AssociatedControlID="ddlStatus"
                        Text="Status *" />

                    <asp:DropDownList
                        ID="ddlStatus"
                        runat="server"
                        CssClass="edit-department-select">
                        <asp:ListItem Text="Select Status" Value="" />
                        <asp:ListItem Text="Active" Value="Active" />
                        <asp:ListItem Text="Inactive" Value="Inactive" />
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator
                        ID="rfvStatus"
                        runat="server"
                        ControlToValidate="ddlStatus"
                        InitialValue=""
                        ValidationGroup="EditDepartment"
                        ErrorMessage="Please select a status."
                        CssClass="edit-department-field-error"
                        Display="Dynamic" />
                </div>

                <!-- Description -->
                <div class="edit-department-field edit-department-field-full">
                    <asp:Label
                        ID="lblDescription"
                        runat="server"
                        AssociatedControlID="txtDescription"
                        Text="Description" />

                    <asp:TextBox
                        ID="txtDescription"
                        runat="server"
                        CssClass="edit-department-input edit-department-textarea"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="50" />
                </div>

                <!-- Email -->
                <div class="edit-department-field">
                    <asp:Label
                        ID="lblEmail"
                        runat="server"
                        AssociatedControlID="txtEmail"
                        Text="Department Email" />

                    <asp:TextBox
                        ID="txtEmail"
                        runat="server"
                        CssClass="edit-department-input"
                        TextMode="Email"
                        MaxLength="50" />
                </div>

                <!-- Contact Number -->
                <div class="edit-department-field">
                    <asp:Label
                        ID="lblContact"
                        runat="server"
                        AssociatedControlID="txtContact"
                        Text="Contact Number" />

                    <asp:TextBox
                        ID="txtContact"
                        runat="server"
                        CssClass="edit-department-input"
                        MaxLength="50" />
                </div>

                <!-- Working Hours -->
                <div class="edit-department-field">
                    <asp:Label
                        ID="lblWorkingHours"
                        runat="server"
                        AssociatedControlID="txtWorkingHours"
                        Text="Working Hours (1–24) *" />

                    <asp:TextBox
                        ID="txtWorkingHours"
                        runat="server"
                        CssClass="edit-department-input"
                        TextMode="Number" />

                    <asp:RequiredFieldValidator
                        ID="rfvWorkingHours"
                        runat="server"
                        ControlToValidate="txtWorkingHours"
                        ValidationGroup="EditDepartment"
                        ErrorMessage="Working hours are required."
                        CssClass="edit-department-field-error"
                        Display="Dynamic" />

                    <asp:RangeValidator
                        ID="rvWorkingHours"
                        runat="server"
                        ControlToValidate="txtWorkingHours"
                        MinimumValue="1"
                        MaximumValue="24"
                        Type="Integer"
                        ValidationGroup="EditDepartment"
                        ErrorMessage="Working hours must be between 1 and 24."
                        CssClass="edit-department-field-error"
                        Display="Dynamic" />
                </div>

                <!-- Location -->
                <div class="edit-department-field">
                    <asp:Label
                        ID="lblLocation"
                        runat="server"
                        AssociatedControlID="txtLocation"
                        Text="Location" />

                    <asp:TextBox
                        ID="txtLocation"
                        runat="server"
                        CssClass="edit-department-input"
                        MaxLength="50" />
                </div>

            </div>

            <!-- Form Actions -->
            <div class="edit-department-actions">

                <asp:HyperLink
                    ID="lnkCancel"
                    runat="server"
                    NavigateUrl="~/Admin/AdminDepartments.aspx"
                    CssClass="edit-department-cancel-btn">
                    Cancel
                </asp:HyperLink>

                <asp:Button
                    ID="btnUpdateDepartment"
                    runat="server"
                    Text="Update Department"
                    CssClass="edit-department-update-btn"
                    ValidationGroup="EditDepartment"
                    OnClick="btnUpdateDepartment_Click" />

            </div>

        </div>

    </div>

</asp:Content>