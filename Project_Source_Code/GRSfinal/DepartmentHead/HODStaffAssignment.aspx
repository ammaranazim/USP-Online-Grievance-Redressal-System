<%@ Page Title="Staff Assignment" Language="C#" MasterPageFile="~/DepartmentHead/HODMaster.master" AutoEventWireup="true" CodeFile="HODStaffAssignment.aspx.cs" Inherits="DepartmentHead_HODStaffAssignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" Runat="Server">
    Staff Assignment
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="<%= ResolveUrl("~/DepartmentHead/css/HODStaffAssignment.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">

    <div class="hod-assignment-page">

        <div class="hod-assignment-heading">
            <div>
                <h2>Assign Grievance to Staff</h2>
                <p>Review the grievance details and assign it to an active staff member in your department.</p>
            </div>

            <asp:HyperLink ID="lnkBackToDetails" runat="server"
                CssClass="hod-assignment-back"
                Text="Back to Grievance Details" />
        </div>

        <asp:Panel ID="pnlMessage" runat="server"
            CssClass="hod-assignment-message"
            Visible="false">
            <asp:Label ID="lblMessage" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlInvalidGrievance" runat="server"
            CssClass="hod-assignment-card"
            Visible="false">
            <h3>Grievance Not Available</h3>
            <p>This Grievance could not be found in your department, or the requested Grievance ID is invalid.</p>
        </asp:Panel>

        <asp:Panel ID="pnlAssignmentContent" runat="server" Visible="false">

            <div class="hod-assignment-card">
                <div class="hod-assignment-card-heading">
                    <div>
                        <h3>Grievance Information</h3>
                        <p>Confirm that you are assigning the correct Grievance.</p>
                    </div>
                    <asp:Label ID="lblGrievanceStatus" runat="server"
                        CssClass="hod-assignment-status" />
                </div>

                <div class="hod-assignment-details-grid">
                    <div class="hod-assignment-detail">
                        <span>Grievance ID</span>
                        <asp:Label ID="lblGrievanceID" runat="server" />
                    </div>

                    <div class="hod-assignment-detail">
                        <span>Title</span>
                        <asp:Label ID="lblGrievanceTitle" runat="server" />
                    </div>

                    <div class="hod-assignment-detail">
                        <span>Student</span>
                        <asp:Label ID="lblStudentName" runat="server" />
                    </div>

                    <div class="hod-assignment-detail">
                        <span>Registration Number</span>
                        <asp:Label ID="lblRegistrationNo" runat="server" />
                    </div>

                    <div class="hod-assignment-detail">
                        <span>Category</span>
                        <asp:Label ID="lblCategory" runat="server" />
                    </div>

                    <div class="hod-assignment-detail">
                        <span>Department</span>
                        <asp:Label ID="lblDepartment" runat="server" />
                    </div>

                    <div class="hod-assignment-detail">
                        <span>Priority</span>
                        <asp:Label ID="lblPriority" runat="server" />
                    </div>

                    <div class="hod-assignment-detail">
                        <span>Submission Date</span>
                        <asp:Label ID="lblSubmissionDate" runat="server" />
                    </div>
                </div>

                <div class="hod-assignment-description">
                    <span>Grievance Description</span>
                    <asp:Label ID="lblDescription" runat="server" />
                </div>
            </div>

            <div class="hod-assignment-card">
                <div class="hod-assignment-card-heading">
                    <div>
                        <h3>Staff Assignment</h3>
                        <p>Select an active staff member from your own department.</p>
                    </div>
                </div>

                <asp:Panel ID="pnlExistingAssignment" runat="server"
                    CssClass="hod-existing-assignment"
                    Visible="false">
                    <span class="hod-existing-assignment-label">Current Assignment</span>
                    <strong>
                        <asp:Label ID="lblAssignedStaffName" runat="server" />
                    </strong>
                    <asp:Label ID="lblAssignedStaffDesignation" runat="server"
                        CssClass="hod-assigned-designation" />
                    <span>
                        Status:
                        <asp:Label ID="lblAssignmentStatus" runat="server" />
                    </span>
                    <span>
                        Assigned on:
                        <asp:Label ID="lblAssignedDate" runat="server" />
                    </span>
                </asp:Panel>

                <asp:Panel ID="pnlNoStaff" runat="server"
                    CssClass="hod-assignment-empty"
                    Visible="false">
                    <div class="hod-assignment-empty-icon">
    <svg xmlns="http://www.w3.org/2000/svg"
         width="24" height="24" viewBox="0 0 24 24"
         fill="none" stroke="currentColor" stroke-width="1.8"
         stroke-linecap="round" stroke-linejoin="round"
         aria-hidden="true">
        <path d="M16 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
        <circle cx="10" cy="7" r="4" />
        <path d="M20 21v-2a4 4 0 0 0-3-3.87" />
        <path d="M16 3.13a4 4 0 0 1 0 7.75" />
    </svg>
</div>
                    <h4>No Active Staff Available</h4>
                    <p>No active staff members are currently available in your department. Please add or activate a staff member before assigning this Grievance.</p>
                </asp:Panel>

                <asp:Panel ID="pnlStaffForm" runat="server" Visible="false">
                    <div class="hod-assignment-form-group">
                        <asp:Label ID="lblStaffSelect" runat="server"
                            AssociatedControlID="ddlStaff"
                            CssClass="hod-assignment-label"
                            Text="Select Staff Member" />

                        <asp:DropDownList ID="ddlStaff" runat="server"
                            CssClass="hod-assignment-select" />

                        <asp:RequiredFieldValidator ID="rfvStaff" runat="server"
                            ControlToValidate="ddlStaff"
                            InitialValue=""
                            ValidationGroup="StaffAssignment"
                            CssClass="hod-assignment-validation"
                            ErrorMessage="Please select a staff member."
                            Display="Dynamic" />
                    </div>

                    <div class="hod-assignment-actions">
                        <asp:HyperLink ID="lnkCancelAssignment" runat="server"
                            CssClass="hod-assignment-cancel"
                            Text="Cancel" />

                        
<asp:Button ID="btnAssignStaff" runat="server"
    Text="Assign to Staff"
    CssClass="hod-assignment-submit"
    ValidationGroup="StaffAssignment"
    OnClick="btnAssignStaff_Click"
    OnClientClick="if (!Page_ClientValidate('StaffAssignment')) return false; this.value='Assigning...'; this.classList.add('assignment-processing');" />

                    </div>
                </asp:Panel>
            </div>

        </asp:Panel>
    </div>

</asp:Content>