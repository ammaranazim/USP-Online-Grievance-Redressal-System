
<%@ Page Title="Grievance Details" Language="C#" MasterPageFile="~/DepartmentHead/HODMaster.master" AutoEventWireup="true" CodeFile="HODGrievanceDetails.aspx.cs" Inherits="DepartmentHead_HODGrievanceDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" Runat="Server">
    Grievance Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="css/HODGrievanceDetails.css?v=4" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">

    <div class="hod-grievance-details-page">

        <!-- Page Header -->
        <div class="hod-details-page-header">
            <div>
                <h1>Grievance Details</h1>
                <p>View complete information about the selected grievance.</p>
            </div>

            <asp:HyperLink ID="lnkBackToGrievances"
                runat="server"
                NavigateUrl="~/DepartmentHead/HODGrievances.aspx"
                CssClass="hod-btn hod-btn-secondary">
                &larr; Back to Grievances
            </asp:HyperLink>
        </div>

        <!-- Overview + Progress -->
        <div class="hod-overview-progress-card">

            <!-- Left: Overview -->
            <div class="hod-overview-panel">

                <div class="hod-overview-heading">
                    <div>
                        <h2>Grievance Overview</h2>
                        <p>Basic information and current status of the grievance.</p>
                    </div>

                    <div class="hod-overview-badges">
                        <asp:Label ID="lblStatus" runat="server"
                            CssClass="status-badge" />

                        <asp:Label ID="lblPriority" runat="server"
                            CssClass="priority-badge" />
                    </div>
                </div>

                <div class="hod-information-rows">

                    <div class="hod-information-row">
                        <span class="hod-information-label">Grievance ID</span>
                        <span class="hod-information-separator">:</span>
                        <asp:Label ID="lblGrievanceID" runat="server"
                            CssClass="hod-information-value" />
                    </div>

                    <div class="hod-information-row">
                        <span class="hod-information-label">Submission Date</span>
                        <span class="hod-information-separator">:</span>
                        <asp:Label ID="lblSubmissionDate" runat="server"
                            CssClass="hod-information-value" />
                    </div>

                    <div class="hod-information-row">
                        <span class="hod-information-label">Student Name</span>
                        <span class="hod-information-separator">:</span>
                        <asp:Label ID="lblStudentName" runat="server"
                            CssClass="hod-information-value" />
                    </div>

                    <div class="hod-information-row">
                        <span class="hod-information-label">Registration No.</span>
                        <span class="hod-information-separator">:</span>
                        <asp:Label ID="lblRegistrationNo" runat="server"
                            CssClass="hod-information-value" />
                    </div>

                    <div class="hod-information-row">
                        <span class="hod-information-label">Category</span>
                        <span class="hod-information-separator">:</span>
                        <asp:Label ID="lblCategory" runat="server"
                            CssClass="hod-information-value" />
                    </div>

                    <div class="hod-information-row">
                        <span class="hod-information-label">Department</span>
                        <span class="hod-information-separator">:</span>
                        <asp:Label ID="lblDepartment" runat="server"
                            CssClass="hod-information-value" />
                    </div>

                    <div class="hod-information-row hod-information-row-last">
                        <span class="hod-information-label">Grievance Title</span>
                        <span class="hod-information-separator">:</span>
                        <asp:Label ID="lblGrievanceTitle" runat="server"
                            CssClass="hod-information-value" />
                    </div>

                </div>
            </div>

            <!-- Right: Progress -->
            <aside class="hod-progress-panel">

                <h2>Grievance Progress</h2>
                <p class="hod-progress-subtitle">
                    Current processing stage of the grievance.
                </p>

               <div class="tracking-timeline">

    <div id="stepSubmitted" runat="server" class="tracking-step">
        <div class="step-circle">
            <asp:Label ID="lblStepSubmitted" runat="server" Text="1" />
        </div>
        <div class="step-content">
            <strong>Grievance Submitted</strong>
            <asp:Label ID="lblTimelineSubmitted" runat="server" />
        </div>
    </div>

    <div id="stepDepartmentReview" runat="server" class="tracking-step">
        <div class="step-circle">
            <asp:Label ID="lblStepDepartmentReview" runat="server" Text="2" />
        </div>
        <div class="step-content">
            <strong>Department Review</strong>
            <asp:Label ID="lblTimelineDepartmentReview" runat="server" />
        </div>
    </div>

    <div id="stepAssigned" runat="server" class="tracking-step">
        <div class="step-circle">
            <asp:Label ID="lblStepAssigned" runat="server" Text="3" />
        </div>
        <div class="step-content">
            <strong>Assigned to Staff</strong>
            <asp:Label ID="lblTimelineAssigned" runat="server" />
        </div>
    </div>

    <div id="stepResolutionReview" runat="server" class="tracking-step">
        <div class="step-circle">
            <asp:Label ID="lblStepResolutionReview" runat="server" Text="4" />
        </div>
        <div class="step-content">
            <strong>Resolution Under Review</strong>
            <asp:Label ID="lblTimelineResolutionReview" runat="server" />
        </div>
    </div>

    <div id="stepResolved" runat="server" class="tracking-step">
        <div class="step-circle">
            <asp:Label ID="lblStepResolved" runat="server" Text="5" />
        </div>
        <div class="step-content">
            <strong>Resolved</strong>
            <asp:Label ID="lblTimelineResolved" runat="server" />
        </div>
    </div>

</div>

            </aside>
        </div>
        <!-- Priority and Resolution Deadline -->
<div class="hod-details-card hod-priority-deadline-card">

    <div class="hod-details-card-heading">
        <div>
            <h2>Priority &amp; Resolution Deadline</h2>
            <p>Set the grievance priority and its target resolution date.</p>
        </div>
    </div>

    <div class="hod-priority-deadline-content">

        <div class="hod-deadline-field">
            <label for="<%= ddlGrievancePriority.ClientID %>">
                Grievance Priority
            </label>

            <asp:DropDownList ID="ddlGrievancePriority"
                runat="server"
                CssClass="hod-priority-select">
                <asp:ListItem Text="Urgent" Value="Urgent" />
                <asp:ListItem Text="High" Value="High" />
                <asp:ListItem Text="Medium" Value="Medium" />
                <asp:ListItem Text="Low" Value="Low" />
            </asp:DropDownList>
        </div>

        <div class="hod-deadline-field">
            <span class="hod-deadline-label">Allowed Resolution Time</span>
            <asp:Label ID="lblResolutionDays"
                runat="server"
                CssClass="hod-deadline-value" />
        </div>

        <div class="hod-deadline-field">
            <span class="hod-deadline-label">Resolution Due Date</span>
            <asp:Label ID="lblDueDate"
                runat="server"
                CssClass="hod-deadline-value" />
        </div>

        <div class="hod-deadline-field">
            <span class="hod-deadline-label">Deadline Status</span>
            <asp:Label ID="lblDeadlineStatus"
                runat="server"
                CssClass="hod-deadline-value" />
        </div>

    </div>

    <asp:Panel ID="pnlPriorityMessage"
        runat="server"
        CssClass="hod-priority-message"
        Visible="false">
        <asp:Label ID="lblPriorityMessage" runat="server" />
    </asp:Panel>

    <asp:Button ID="btnSavePriority"
        runat="server"
        Text="Save Priority &amp; Due Date"
        CssClass="hod-btn hod-btn-primary"
        OnClick="btnSavePriority_Click" />

</div>

        <!-- Description and Evidence -->
        <div class="hod-details-card hod-description-card">

            <div class="hod-details-card-heading">
                <div>
                    <h2>Description &amp; Evidence</h2>
                    <p>Submitted grievance description and supporting evidence.</p>
                </div>
            </div>

            <div class="hod-description-section">
                <span class="hod-detail-label">Grievance Description</span>

                <div class="hod-description-box">
                    <asp:Label ID="lblDescription" runat="server"
                        CssClass="hod-description-text" />
                </div>
            </div>

            <div class="hod-evidence-section">
                <span class="hod-detail-label">Supporting Evidence</span>

                <asp:Panel ID="pnlNoEvidence" runat="server"
                    CssClass="hod-empty-message" Visible="false">
                    No supporting evidence was submitted.
                </asp:Panel>

          
<asp:Repeater ID="rptEvidence" runat="server">
    <ItemTemplate>
        <div class="hod-evidence-item">
            <div class="hod-evidence-file">
                <asp:Label
                    ID="lblEvidenceName"
                    runat="server"
                    Text='<%# Eval("EvidenceName") %>'
                    CssClass="hod-evidence-name" />
            </div>

            <div class="hod-evidence-actions">
                <asp:HyperLink
                    ID="lnkOpenEvidence"
                    runat="server"
                    NavigateUrl='<%# GetEvidenceUrl(Eval("EvidenceID"), "open") %>'
                    Target="_blank"
                    CssClass="hod-evidence-btn hod-evidence-open">
                    Open
                </asp:HyperLink>

                <asp:HyperLink
                    ID="lnkDownloadEvidence"
                    runat="server"
                    NavigateUrl='<%# GetEvidenceUrl(Eval("EvidenceID"), "download") %>'
                    CssClass="hod-evidence-btn hod-evidence-download">
                    Download
                </asp:HyperLink>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>

            </div>

        </div>
        
<!-- Reject Grievance -->
<asp:Panel ID="pnlRejectGrievance"
    runat="server"
    CssClass="hod-details-card hod-rejection-card">

    <div class="hod-details-card-heading">
        <div>
            <h2>Reject Grievance</h2>
            <p>Provide a clear reason if the grievance does not qualify for further processing.</p>
        </div>
    </div>

    <asp:Panel ID="pnlRejectionMessage"
        runat="server"
        Visible="false"
        CssClass="hod-priority-message">
        <asp:Label ID="lblRejectionMessage" runat="server" />
    </asp:Panel>

    <div class="hod-rejection-field">
        <label for="<%= txtRejectionReason.ClientID %>">
            Rejection Reason <span class="required-mark">*</span>
        </label>

        <asp:TextBox ID="txtRejectionReason"
            runat="server"
            TextMode="MultiLine"
            Rows="4"
            MaxLength="255"
            CssClass="hod-rejection-textarea"
            placeholder="Explain why this grievance is being rejected." />

        <asp:RequiredFieldValidator
            ID="rfvRejectionReason"
            runat="server"
            ControlToValidate="txtRejectionReason"
            ErrorMessage="Please enter a rejection reason."
            CssClass="hod-validation-error"
            Display="Dynamic"
            ValidationGroup="RejectGrievance" />
    </div>

    <asp:Button ID="btnRejectGrievance"
        runat="server"
        Text="Reject Grievance"
        CssClass="hod-btn hod-btn-danger"
        OnClick="btnRejectGrievance_Click"
        ValidationGroup="RejectGrievance"
        OnClientClick="return confirm('Are you sure you want to reject this grievance? This action cannot be undone through this page.');" />

</asp:Panel>

        <div class="hod-assignment-action">
    <asp:HyperLink ID="lnkAssignToStaff"
        runat="server"
        CssClass="hod-btn hod-btn-primary">
        <i class="fa fa-user-plus" aria-hidden="true"></i>
        Assign to Staff
    </asp:HyperLink>
</div>

    </div>

</asp:Content>
