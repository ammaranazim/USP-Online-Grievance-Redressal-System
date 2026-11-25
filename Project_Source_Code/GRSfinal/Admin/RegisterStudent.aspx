
<%@ Page Title="Register Student"
    Language="C#"
    MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true"
    CodeFile="RegisterStudent.aspx.cs"
    Inherits="Admin_RegisterStudent" %>

<asp:Content ID="RegisterStudentHead"
    ContentPlaceHolderID="HeadContent"
    runat="server">

    <link href="<%= ResolveUrl("~/Admin/css/RegisterStudent.css?v=2") %>"
        rel="stylesheet" />

</asp:Content>

<asp:Content ID="RegisterStudentContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container register-student-page">

        <div class="card register-student-card">

            <div class="card-header register-student-header">
                <h3>Student Registration</h3>
                <p>Enter the student's information to create an account.</p>
            </div>

            <div class="card-body">

                <asp:Label ID="lblMessage"
                    runat="server"
                    CssClass="register-student-message"
                    EnableViewState="false">
                </asp:Label>

                <asp:ValidationSummary ID="vsRegistration"
                    runat="server"
                    ValidationGroup="StudentRegistration"
                    CssClass="registration-validation-summary"
                    HeaderText="Please correct the following:"
                    DisplayMode="BulletList"
                    EnableClientScript="true" />

                                    <div class="registration-section-heading">
                        <span class="registration-section-number">01</span>
                        <div class="registration-section-copy">
                            <h4>Account Information</h4>
                            <p>Login identity and account status.</p>
                        </div>
                    </div>
                    <div class="row registration-fields registration-account-fields">

                        <div class="col-md-6 mb-3">
                                                <label for="<%= txtRegNo.ClientID %>">Registration No <span class="required-mark">*</span></label>
                                                <asp:TextBox ID="txtRegNo"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    MaxLength="100"
                                                    autocomplete="off">
                                                </asp:TextBox>
                        
                                                <asp:RequiredFieldValidator ID="rfvRegNo"
                                                    runat="server"
                                                    ControlToValidate="txtRegNo"
                                                    ValidationGroup="StudentRegistration"
                                                    ErrorMessage="Registration number is required."
                                                    CssClass="field-validation"
                                                    Display="Dynamic">
                                                </asp:RequiredFieldValidator>
                                            </div>

                        <div class="col-md-6 mb-3">
                                                <label for="<%= txtEmail.ClientID %>">Email <span class="required-mark">*</span></label>
                                                <asp:TextBox ID="txtEmail"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    TextMode="Email"
                                                    MaxLength="254">
                                                </asp:TextBox>
                        
                                                <asp:RequiredFieldValidator ID="rfvEmail"
                                                    runat="server"
                                                    ControlToValidate="txtEmail"
                                                    ValidationGroup="StudentRegistration"
                                                    ErrorMessage="Email is required."
                                                    CssClass="field-validation"
                                                    Display="Dynamic">
                                                </asp:RequiredFieldValidator>
                                            </div>

                        <div class="col-md-6 mb-3">
                                                <label for="<%= ddlStatus.ClientID %>">Status <span class="required-mark">*</span></label>
                                                <asp:DropDownList ID="ddlStatus"
                                                    runat="server"
                                                    CssClass="form-control">
                                                    <asp:ListItem Text="Select Status" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                                                    <asp:ListItem Text="Inactive" Value="Inactive"></asp:ListItem>
                                                </asp:DropDownList>
                        
                                                <asp:RequiredFieldValidator ID="rfvStatus"
                                                    runat="server"
                                                    ControlToValidate="ddlStatus"
                                                    InitialValue=""
                                                    ValidationGroup="StudentRegistration"
                                                    ErrorMessage="Please select student status."
                                                    CssClass="field-validation"
                                                    Display="Dynamic">
                                                </asp:RequiredFieldValidator>
                                            </div>

                    </div>

                    <div class="registration-section-heading">
                        <span class="registration-section-number">02</span>
                        <div class="registration-section-copy">
                            <h4>Personal Information</h4>
                            <p>Student identity and contact details.</p>
                        </div>
                    </div>
                    <div class="row registration-fields">

                        <div class="col-md-6 mb-3">
                                                <label for="<%= txtFirstName.ClientID %>">First Name <span class="required-mark">*</span></label>
                                                <asp:TextBox ID="txtFirstName"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    MaxLength="100">
                                                </asp:TextBox>
                        
                                                <asp:RequiredFieldValidator ID="rfvFirstName"
                                                    runat="server"
                                                    ControlToValidate="txtFirstName"
                                                    ValidationGroup="StudentRegistration"
                                                    ErrorMessage="First name is required."
                                                    CssClass="field-validation"
                                                    Display="Dynamic">
                                                </asp:RequiredFieldValidator>
                                            </div>

                        <div class="col-md-6 mb-3">
                                                <label for="<%= txtLastName.ClientID %>">Last Name</label>
                                                <asp:TextBox ID="txtLastName"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    MaxLength="100">
                                                </asp:TextBox>
                                            </div>

                        <div class="col-md-6 mb-3">
                                                <label for="<%= txtFatherName.ClientID %>">Father Name</label>
                                                <asp:TextBox ID="txtFatherName"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    MaxLength="100">
                                                </asp:TextBox>
                                            </div>

                        <div class="col-md-6 mb-3">
                                                <label for="<%= txtCNIC.ClientID %>">CNIC</label>
                                                <asp:TextBox ID="txtCNIC"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    MaxLength="15"
                                                    placeholder="XXXXX-XXXXXXX-X">
                                                </asp:TextBox>
                                            </div>

                        <div class="col-md-6 mb-3">
                                                <label for="<%= txtPhoneNo.ClientID %>">Phone No</label>
                                                <asp:TextBox ID="txtPhoneNo"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    MaxLength="20"
                                                    TextMode="Phone">
                                                </asp:TextBox>
                                            </div>

                        <div class="col-md-6 mb-3">
                                                <label for="<%= ddlGender.ClientID %>">Gender <span class="required-mark">*</span></label>
                                                <asp:DropDownList ID="ddlGender"
                                                    runat="server"
                                                    CssClass="form-control">
                                                    <asp:ListItem Text="Select Gender" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                                    <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                                </asp:DropDownList>
                        
                                                <asp:RequiredFieldValidator ID="rfvGender"
                                                    runat="server"
                                                    ControlToValidate="ddlGender"
                                                    InitialValue=""
                                                    ValidationGroup="StudentRegistration"
                                                    ErrorMessage="Please select gender."
                                                    CssClass="field-validation"
                                                    Display="Dynamic">
                                                </asp:RequiredFieldValidator>
                                            </div>

                        <div class="col-md-6 mb-3">
                                                <label for="<%= txtDOB.ClientID %>">Date of Birth <span class="required-mark">*</span></label>
                                                <asp:TextBox ID="txtDOB"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    TextMode="Date">
                                                </asp:TextBox>
                        
                                                <asp:RequiredFieldValidator ID="rfvDOB"
                                                    runat="server"
                                                    ControlToValidate="txtDOB"
                                                    ValidationGroup="StudentRegistration"
                                                    ErrorMessage="Date of birth is required."
                                                    CssClass="field-validation"
                                                    Display="Dynamic">
                                                </asp:RequiredFieldValidator>
                                            </div>

                        <div class="col-md-6 mb-3">
                                                <label for="<%= txtCity.ClientID %>">City</label>
                                                <asp:TextBox ID="txtCity"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    MaxLength="100">
                                                </asp:TextBox>
                                            </div>

                        <div class="col-md-12 mb-3">
                                                <label for="<%= txtAddress.ClientID %>">Address</label>
                                                <asp:TextBox ID="txtAddress"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    TextMode="MultiLine"
                                                    Rows="3">
                                                </asp:TextBox>
                                            </div>

                    </div>

                    <div class="registration-section-heading">
                        <span class="registration-section-number">03</span>
                        <div class="registration-section-copy">
                            <h4>Academic Information</h4>
                            <p>University enrollment and academic details.</p>
                        </div>
                    </div>
                    <div class="row registration-fields">

                        <div class="col-md-6 mb-3">
                                                <label for="<%= ddlDepartment.ClientID %>">Department <span class="required-mark">*</span></label>
                                                <asp:DropDownList ID="ddlDepartment"
                                                    runat="server"
                                                    CssClass="form-control">
                                                </asp:DropDownList>
                        
                                                <asp:RequiredFieldValidator ID="rfvDepartment"
                                                    runat="server"
                                                    ControlToValidate="ddlDepartment"
                                                    InitialValue="0"
                                                    ValidationGroup="StudentRegistration"
                                                    ErrorMessage="Please select a department."
                                                    CssClass="field-validation"
                                                    Display="Dynamic">
                                                </asp:RequiredFieldValidator>
                                            </div>

                        <div class="col-md-6 mb-3">
                                                <label for="<%= txtProgram.ClientID %>">Program</label>
                                                <asp:TextBox ID="txtProgram"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    MaxLength="100">
                                                </asp:TextBox>
                                            </div>

                        <div class="col-md-6 mb-3">
                                                <label for="<%= txtSemester.ClientID %>">Semester</label>
                                                <asp:TextBox ID="txtSemester"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    MaxLength="30">
                                                </asp:TextBox>
                                            </div>

                        <div class="col-md-6 mb-3">
                                                <label for="<%= txtSession.ClientID %>">Session</label>
                                                <asp:TextBox ID="txtSession"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    MaxLength="50">
                                                </asp:TextBox>
                                            </div>

                        <div class="col-md-6 mb-3">
                                                <label for="<%= txtEnrollmentDate.ClientID %>">Enrollment Date <span class="required-mark">*</span></label>
                                                <asp:TextBox ID="txtEnrollmentDate"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    TextMode="Date">
                                                </asp:TextBox>
                        
                                                <asp:RequiredFieldValidator ID="rfvEnrollmentDate"
                                                    runat="server"
                                                    ControlToValidate="txtEnrollmentDate"
                                                    ValidationGroup="StudentRegistration"
                                                    ErrorMessage="Enrollment date is required."
                                                    CssClass="field-validation"
                                                    Display="Dynamic">
                                                </asp:RequiredFieldValidator>
                                            </div>

                    </div>

                    <div class="registration-section-heading">
                        <span class="registration-section-number">04</span>
                        <div class="registration-section-copy">
                            <h4>Additional Information</h4>
                            <p>Optional notes about the student.</p>
                        </div>
                    </div>
                    <div class="row registration-fields">

                        <div class="col-md-12 mb-3">
                                                <label for="<%= txtDescription.ClientID %>">Description</label>
                                                <asp:TextBox ID="txtDescription"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    TextMode="MultiLine"
                                                    Rows="3">
                                                </asp:TextBox>
                                            </div>

                    </div>

                <div class="register-student-actions">

                    <asp:Button ID="btnSave"
                        runat="server"
                        Text="Register Student"
                        CssClass="btn btn-primary register-save-button"
                        ValidationGroup="StudentRegistration"
                        OnClick="btnSave_Click" />

                    <asp:Button ID="btnClear"
                        runat="server"
                        Text="Clear"
                        CssClass="btn btn-secondary register-clear-button"
                        CausesValidation="false"
                        OnClick="btnClear_Click" />

                </div>

            </div>
        </div>
    </div>

</asp:Content>
