<%@ Page Title="Edit Department Head"
    Language="C#"
    MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true"
    CodeFile="AdminEditDepartmentHead.aspx.cs"
    Inherits="Admin_AdminEditDepartmentHead" %>

<asp:Content ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

    <link href="css/AdminEditDepartmentHead.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="edit-hod-page">

        <!-- Page Header -->
        <div class="page-header">
            <div>
                <h1>Edit Department Head</h1>
                <p>Update department head information and account status.</p>
            </div>

            <a href="AdminDepartmentHeads.aspx" class="btn-back">
                Back to Department Heads
            </a>
        </div>

        <!-- Edit Form Card -->
        <div class="edit-hod-card">

            <div class="card-heading">
                <div>
                    <h2>Department Head Details</h2>
                    <p>Update the information in the fields below.</p>
                </div>
            </div>

            <asp:Label
                ID="lblMessage"
                runat="server"
                CssClass="form-message"
                EnableViewState="false">
            </asp:Label>

            <asp:HiddenField
                ID="hfDeptHeadID"
                runat="server" />

            <div class="form-grid">

                <!-- Full Name -->
                <div class="form-group">
                    <asp:Label
                        ID="lblHodName"
                        runat="server"
                        AssociatedControlID="txtHodName"
                        CssClass="form-label"
                        Text="Full Name">
                    </asp:Label>

                    <asp:TextBox
                        ID="txtHodName"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="50"
                        placeholder="Enter full name">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvHodName"
                        runat="server"
                        ControlToValidate="txtHodName"
                        ErrorMessage="Name is required."
                        CssClass="field-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <!-- Username -->
                <div class="form-group">
                    <asp:Label
                        ID="lblUsername"
                        runat="server"
                        AssociatedControlID="txtUsername"
                        CssClass="form-label"
                        Text="Username">
                    </asp:Label>

                    <asp:TextBox
                        ID="txtUsername"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="50"
                        placeholder="Enter username">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvUsername"
                        runat="server"
                        ControlToValidate="txtUsername"
                        ErrorMessage="Username is required."
                        CssClass="field-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <!-- Email -->
                <div class="form-group">
                    <asp:Label
                        ID="lblEmail"
                        runat="server"
                        AssociatedControlID="txtEmail"
                        CssClass="form-label"
                        Text="Email Address">
                    </asp:Label>

                    <asp:TextBox
                        ID="txtEmail"
                        runat="server"
                        CssClass="form-control"
                        TextMode="Email"
                        MaxLength="50"
                        placeholder="Enter email address">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvEmail"
                        runat="server"
                        ControlToValidate="txtEmail"
                        ErrorMessage="Email is required."
                        CssClass="field-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                    <asp:RegularExpressionValidator
                        ID="revEmail"
                        runat="server"
                        ControlToValidate="txtEmail"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                        ErrorMessage="Enter a valid email address."
                        CssClass="field-error"
                        Display="Dynamic">
                    </asp:RegularExpressionValidator>
                </div>

                <!-- Phone -->
                <div class="form-group">
                    <asp:Label
                        ID="lblPhone"
                        runat="server"
                        AssociatedControlID="txtPhone"
                        CssClass="form-label"
                        Text="Phone Number">
                    </asp:Label>

                    <asp:TextBox
                        ID="txtPhone"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="50"
                        placeholder="Enter phone number">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvPhone"
                        runat="server"
                        ControlToValidate="txtPhone"
                        ErrorMessage="Phone number is required."
                        CssClass="field-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <!-- Designation -->
                <div class="form-group">
                    <asp:Label
                        ID="lblDesignation"
                        runat="server"
                        AssociatedControlID="txtDesignation"
                        CssClass="form-label"
                        Text="Designation">
                    </asp:Label>

                    <asp:TextBox
                        ID="txtDesignation"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="50"
                        placeholder="Enter designation">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvDesignation"
                        runat="server"
                        ControlToValidate="txtDesignation"
                        ErrorMessage="Designation is required."
                        CssClass="field-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <!-- Department -->
                <div class="form-group">
                    <asp:Label
                        ID="lblDepartment"
                        runat="server"
                        AssociatedControlID="ddlDepartment"
                        CssClass="form-label"
                        Text="Department">
                    </asp:Label>

                    <asp:DropDownList
                        ID="ddlDepartment"
                        runat="server"
                        CssClass="form-control">
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator
                        ID="rfvDepartment"
                        runat="server"
                        ControlToValidate="ddlDepartment"
                        InitialValue=""
                        ErrorMessage="Please select a department."
                        CssClass="field-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <!-- Account Status -->
                <div class="form-group">
                    <asp:Label
                        ID="lblStatus"
                        runat="server"
                        AssociatedControlID="ddlStatus"
                        CssClass="form-label"
                        Text="Account Status">
                    </asp:Label>

                    <asp:DropDownList
                        ID="ddlStatus"
                        runat="server"
                        CssClass="form-control">
                        <asp:ListItem Text="Select Status" Value="" />
                        <asp:ListItem Text="Active" Value="active" />
                        <asp:ListItem Text="Inactive" Value="inactive" />
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator
                        ID="rfvStatus"
                        runat="server"
                        ControlToValidate="ddlStatus"
                        InitialValue=""
                        ErrorMessage="Please select account status."
                        CssClass="field-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

            </div>


            <!-- Actions -->
            <div class="form-actions">

                <a href="AdminDepartmentHeads.aspx" class="btn-cancel">
                    Cancel
                </a>

                <asp:Button
                    ID="btnUpdate"
                    runat="server"
                    Text="Save Changes"
                    CssClass="btn-save"
                    OnClick="btnUpdate_Click" />

            </div>

        </div>

    </div>

</asp:Content>