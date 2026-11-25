
<%@ Page Title="Edit Student" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="EditStudent.aspx.cs" Inherits="Admin_Default" %>

<asp:Content ID="EditStudentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Admin/css/EditStudent.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="EditStudentContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="edit-student-page container-fluid">

        <div class="edit-student-page-header">
            <div>
                <h2>Edit Student</h2>
                <p>Manage student profile, academic details and account status.</p>
            </div>

            <asp:HyperLink ID="lnkBackToStudents" runat="server"
                NavigateUrl="~/Admin/AdminStudents.aspx"
                CssClass="edit-student-secondary-btn">
                Back to Students
            </asp:HyperLink>
        </div>

        <asp:Label ID="lblMessage" runat="server"
            CssClass="edit-student-message"
            EnableViewState="false" />

        <div class="card edit-student-card">

            <div class="card-header edit-student-card-header">
                <div>
                    <h3>Student Information</h3>
                    <p>Review the information below and save any necessary changes.</p>
                </div>
            </div>

            <div class="card-body edit-student-card-body">

                <!-- Personal Information -->
                <section class="edit-student-section">
                    <div class="edit-student-section-heading">
                        <h4>Personal Information</h4>
                        <p>Identity and contact details</p>
                    </div>

                    <div class="row">

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblRegNo" runat="server"
                                AssociatedControlID="txtRegNo"
                                CssClass="edit-student-label">Registration No.</asp:Label>
                            <asp:TextBox ID="txtRegNo" runat="server"
                                CssClass="form-control edit-student-control" />
                        </div>

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblEmail" runat="server"
                                AssociatedControlID="txtEmail"
                                CssClass="edit-student-label">Email Address</asp:Label>
                            <asp:TextBox ID="txtEmail" runat="server"
                                CssClass="form-control edit-student-control"
                                TextMode="Email" />
                        </div>

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblFirstName" runat="server"
                                AssociatedControlID="txtFirstName"
                                CssClass="edit-student-label">First Name</asp:Label>
                            <asp:TextBox ID="txtFirstName" runat="server"
                                CssClass="form-control edit-student-control" />
                        </div>

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblLastName" runat="server"
                                AssociatedControlID="txtLastName"
                                CssClass="edit-student-label">Last Name</asp:Label>
                            <asp:TextBox ID="txtLastName" runat="server"
                                CssClass="form-control edit-student-control" />
                        </div>

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblFatherName" runat="server"
                                AssociatedControlID="txtFatherName"
                                CssClass="edit-student-label">Father's Name</asp:Label>
                            <asp:TextBox ID="txtFatherName" runat="server"
                                CssClass="form-control edit-student-control" />
                        </div>

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblCNIC" runat="server"
                                AssociatedControlID="txtCNIC"
                                CssClass="edit-student-label">CNIC</asp:Label>
                            <asp:TextBox ID="txtCNIC" runat="server"
                                CssClass="form-control edit-student-control" />
                        </div>

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblPhoneNo" runat="server"
                                AssociatedControlID="txtPhoneNo"
                                CssClass="edit-student-label">Phone Number</asp:Label>
                            <asp:TextBox ID="txtPhoneNo" runat="server"
                                CssClass="form-control edit-student-control" />
                        </div>

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblGender" runat="server"
                                AssociatedControlID="ddlGender"
                                CssClass="edit-student-label">Gender</asp:Label>
                            <asp:DropDownList ID="ddlGender" runat="server"
                                CssClass="form-control edit-student-control">
                                <asp:ListItem Text="Select Gender" Value="" />
                                <asp:ListItem Text="Male" Value="Male" />
                                <asp:ListItem Text="Female" Value="Female" />
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblDOB" runat="server"
                                AssociatedControlID="txtDOB"
                                CssClass="edit-student-label">Date of Birth</asp:Label>
                            <asp:TextBox ID="txtDOB" runat="server"
                                CssClass="form-control edit-student-control"
                                TextMode="Date" />
                        </div>

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblCity" runat="server"
                                AssociatedControlID="txtCity"
                                CssClass="edit-student-label">City</asp:Label>
                            <asp:TextBox ID="txtCity" runat="server"
                                CssClass="form-control edit-student-control" />
                        </div>

                    </div>
                </section>

                <!-- Academic Information -->
                <section class="edit-student-section">
                    <div class="edit-student-section-heading">
                        <h4>Academic Information</h4>
                        <p>Department, program and enrollment details</p>
                    </div>

                    <div class="row">

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblDepartment" runat="server"
                                AssociatedControlID="ddlDepartment"
                                CssClass="edit-student-label">Department</asp:Label>
                            <asp:DropDownList ID="ddlDepartment" runat="server"
                                CssClass="form-control edit-student-control" />
                        </div>

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblProgram" runat="server"
                                AssociatedControlID="txtProgram"
                                CssClass="edit-student-label">Program</asp:Label>
                            <asp:TextBox ID="txtProgram" runat="server"
                                CssClass="form-control edit-student-control" />
                        </div>

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblSemester" runat="server"
                                AssociatedControlID="txtSemester"
                                CssClass="edit-student-label">Semester</asp:Label>
                            <asp:TextBox ID="txtSemester" runat="server"
                                CssClass="form-control edit-student-control" />
                        </div>

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblSession" runat="server"
                                AssociatedControlID="txtSession"
                                CssClass="edit-student-label">Session</asp:Label>
                            <asp:TextBox ID="txtSession" runat="server"
                                CssClass="form-control edit-student-control" />
                        </div>

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblEnrollmentDate" runat="server"
                                AssociatedControlID="txtEnrollmentDate"
                                CssClass="edit-student-label">Enrollment Date</asp:Label>
                            <asp:TextBox ID="txtEnrollmentDate" runat="server"
                                CssClass="form-control edit-student-control"
                                TextMode="Date" />
                        </div>

                        <div class="col-md-6 form-group">
                            <asp:Label ID="lblStatus" runat="server"
                                AssociatedControlID="ddlStatus"
                                CssClass="edit-student-label">Account Status</asp:Label>
                            <asp:DropDownList ID="ddlStatus" runat="server"
                                CssClass="form-control edit-student-control">
                                <asp:ListItem Text="Select Status" Value="" />
                                <asp:ListItem Text="Active" Value="Active" />
                                <asp:ListItem Text="Inactive" Value="Inactive" />
                            </asp:DropDownList>
                        </div>

                    </div>
                </section>

                <!-- Additional Information -->
                <section class="edit-student-section edit-student-section-last">
                    <div class="edit-student-section-heading">
                        <h4>Additional Information</h4>
                        <p>Address and other recorded details</p>
                    </div>

                    <div class="form-group">
                        <asp:Label ID="lblAddress" runat="server"
                            AssociatedControlID="txtAddress"
                            CssClass="edit-student-label">Address</asp:Label>
                        <asp:TextBox ID="txtAddress" runat="server"
                            CssClass="form-control edit-student-control edit-student-textarea"
                            TextMode="MultiLine" Rows="3" />
                    </div>

                    <div class="form-group mb-0">
                        <asp:Label ID="lblDescription" runat="server"
                            AssociatedControlID="txtDescription"
                            CssClass="edit-student-label">Description</asp:Label>
                        <asp:TextBox ID="txtDescription" runat="server"
                            CssClass="form-control edit-student-control edit-student-textarea"
                            TextMode="MultiLine" Rows="3" />
                    </div>
                </section>

                <!-- Actions -->
                <div class="edit-student-actions">
                    <asp:HyperLink ID="lnkCancel" runat="server"
                        NavigateUrl="~/Admin/AdminStudents.aspx"
                        CssClass="edit-student-secondary-btn">
                        Cancel
                    </asp:HyperLink>

                    <asp:Button ID="btnUpdate" runat="server"
                        Text="Update Student"
                        CssClass="edit-student-primary-btn"
                        OnClick="btnUpdate_Click" />
                </div>

            </div>
        </div>
    </div>

</asp:Content>
