
<%@ Page Title="Grievance Details" Language="C#" MasterPageFile="~/Staff/StaffMaster.master" AutoEventWireup="true" CodeFile="StaffGrievanceDetails.aspx.cs" Inherits="Staff_StaffGrievanceDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="<%= ResolveUrl("~/Staff/css/StaffGrievanceDetails.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <!-- Page Header -->
    <div class="staff-page-header">
        <div>
            <h2>Grievance Details</h2>
            <p>View complete information about the selected grievance.</p>
        </div>

        <asp:HyperLink
            ID="lnkBackToAssigned"
            runat="server"
            NavigateUrl="~/Staff/StaffAssignedGrievances.aspx"
            CssClass="btn btn-outline-secondary">
            <i class="fas fa-arrow-left"></i> Back to Assigned Grievances
        </asp:HyperLink>
    </div>

    <!-- Feedback Message -->
    <asp:Panel ID="pnlMessage" runat="server" Visible="false"
        CssClass="alert" role="alert">
        <asp:Label ID="lblMessage" runat="server" />
    </asp:Panel>

    <asp:Panel ID="pnlGrievanceDetails" runat="server" Visible="false">

        <!-- Grievance Overview -->
        <div class="staff-detail-card grievance-overview-card">

            <div class="grievance-overview-header">
                <div>
                    <h5>Grievance Overview</h5>
                    <p>Basic information and current status of the grievance.</p>
                </div>

                <div class="grievance-overview-badges">
                    <asp:Label
                        ID="lblGrievanceStatus"
                        runat="server"
                        CssClass="grievance-status-badge" />

                    <asp:Label
                        ID="lblPriority"
                        runat="server"
                        CssClass="grievance-priority-badge" />
                </div>
            </div>

            <div class="grievance-overview-rows">

                <div class="grievance-overview-row">
                    <span class="grievance-overview-label">Grievance ID</span>
                    <span class="grievance-overview-separator">:</span>
                    <asp:Label
                        ID="lblGrievanceID"
                        runat="server"
                        CssClass="grievance-overview-value" />
                </div>

                <div class="grievance-overview-row">
                    <span class="grievance-overview-label">Submission Date</span>
                    <span class="grievance-overview-separator">:</span>
                    <asp:Label
                        ID="lblSubmissionDate"
                        runat="server"
                        CssClass="grievance-overview-value" />
                </div>

                <div class="grievance-overview-row">
                    <span class="grievance-overview-label">Category</span>
                    <span class="grievance-overview-separator">:</span>
                    <asp:Label
                        ID="lblCategory"
                        runat="server"
                        CssClass="grievance-overview-value" />
                </div>

                <div class="grievance-overview-row">
                    <span class="grievance-overview-label">Department</span>
                    <span class="grievance-overview-separator">:</span>
                    <asp:Label
                        ID="lblDepartment"
                        runat="server"
                        CssClass="grievance-overview-value" />
                </div>

                <div class="grievance-overview-row">
                    <span class="grievance-overview-label">Grievance Title</span>
                    <span class="grievance-overview-separator">:</span>
                    <asp:Label
                        ID="lblGrievanceTitle"
                        runat="server"
                        CssClass="grievance-overview-value" />
                </div>

                <div class="grievance-overview-row grievance-description-row">
                    <span class="grievance-overview-label">Description</span>
                    <span class="grievance-overview-separator">:</span>
                    <asp:Label
                        ID="lblDescription"
                        runat="server"
                        CssClass="grievance-overview-value grievance-description-value" />
                </div>

                <div class="grievance-overview-row">
                    <span class="grievance-overview-label">Due Date</span>
                    <span class="grievance-overview-separator">:</span>
                    <asp:Label
                        ID="lblDueDate"
                        runat="server"
                        CssClass="grievance-overview-value" />
                </div>

            </div>
        </div>


        <!-- Work Progress -->
        <div class="staff-detail-card">
            <div class="staff-detail-card-header">
                <h5>Work Progress</h5>
            </div>

            <p>
                Start working on this grievance to record your progress.
            </p>

            <asp:Button
                ID="btnStartWorking"
                runat="server"
                Text="Start Working"
                CssClass="btn btn-primary"
                OnClick="btnStartWorking_Click"
                CausesValidation="false" />
        </div>

    </asp:Panel>
    
<!-- Staff Escalation Request -->
<asp:Panel ID="pnlStaffEscalation" runat="server"
    CssClass="staff-detail-card staff-escalation-card"
    Visible="false">

    <div class="staff-detail-card-header">
        <div>
            <h5>Request Escalation</h5>
            <p>
                If this Grievance requires higher-level intervention,
                submit a request to your Head of Department.
            </p>
        </div>

     <button type="button"
    id="btnToggleEscalation"
    runat="server"
    ClientIDMode="Static"
    class="btn btn-outline-primary"
    onclick="toggleEscalationForm();">
    Request Escalation
</button>
    </div>

    <asp:Panel ID="pnlEscalationStatus" runat="server"
        Visible="false" CssClass="staff-escalation-status">
        <asp:Label ID="lblEscalationStatus" runat="server" />
    </asp:Panel>
<asp:Panel ID="pnlEscalationForm"
    runat="server"
    CssClass="escalation-form-hidden">

        <div class="staff-resolution-field">
            <label for="<%= ddlEscalationReason.ClientID %>">
                Reason for Escalation <span class="staff-required">*</span>
            </label>

            <asp:DropDownList ID="ddlEscalationReason" runat="server"
                CssClass="form-control">
                <asp:ListItem Text="Select a reason" Value="" />
                <asp:ListItem Text="Outside my authority"
                    Value="Outside my authority" />
                <asp:ListItem Text="Requires additional resources"
                    Value="Requires additional resources" />
                <asp:ListItem Text="Complex issue requiring higher-level review"
                    Value="Complex issue requiring higher-level review" />
                <asp:ListItem Text="Other"
                    Value="Other" />
            </asp:DropDownList>
        </div>

        <div class="staff-resolution-field">
            <label for="<%= txtEscalationDescription.ClientID %>">
                Justification <span class="staff-required">*</span>
            </label>

            <asp:TextBox ID="txtEscalationDescription" runat="server"
                TextMode="MultiLine" Rows="4" MaxLength="2000"
                CssClass="form-control staff-resolution-textarea"
                placeholder="Explain why this Grievance requires escalation..." />

        </div>

        <div class="staff-resolution-actions">
           <button type="button"
    class="btn btn-outline-secondary"
    onclick="cancelEscalationForm()">
    Cancel
</button>

            <asp:Button ID="btnSubmitEscalation" runat="server"
                Text="Submit Escalation Request"
                CssClass="btn btn-primary"
                CausesValidation="false"
                OnClick="btnSubmitEscalation_Click" />
        </div>

    </asp:Panel>
</asp:Panel>

    
<!-- HOD Feedback for Returned Resolution -->
<asp:Panel
    ID="pnlHODFeedback"
    runat="server"
    CssClass="staff-detail-card hod-feedback-card">

    <div class="hod-feedback-header">
        <div>
            <h5 class="hod-feedback-title">HOD Review Feedback</h5>
            <p class="hod-feedback-subtitle">
                Review the feedback provided by your Head of Department.
            </p>
        </div>

        <span class="hod-returned-badge">Returned for Revision</span>
    </div>

    <div class="hod-feedback-message">
        <strong>Action Required</strong>
        <p>
            Your proposed resolution has been returned.
            Review the remarks below, revise your response, and submit it again.
        </p>
    </div>

    <div class="hod-remarks-section">
        <div class="hod-remarks-label">HOD Remarks</div>

        <asp:Label
            ID="lblHODRemarks"
            runat="server"
            CssClass="hod-remarks-text" />

        <asp:Panel
            ID="pnlNoHODRemarks"
            runat="server"
            CssClass="hod-no-remarks"
            Visible="false">
            No additional remarks were provided by the HOD.
        </asp:Panel>
    </div>

</asp:Panel>
    


<!-- HOD Instruction, Staff Escalation History, and Progress Update -->
<asp:Panel ID="pnlStaffEscalationTimeline" runat="server" Visible="false">

    <!-- Latest HOD Instruction: separate card -->
    <asp:Panel ID="pnlHODStaffInstruction" runat="server"
        CssClass="staff-detail-card staff-hod-instruction-card" Visible="false">
        <div class="staff-detail-card-header">
            <div>
                <h5>HOD Instructions</h5>
                <p>Review the latest instruction provided by your Head of Department.</p>
            </div>
        </div>
        <div class="staff-hod-instruction-content">
            <div class="staff-hod-instruction-label">Latest HOD Instruction</div>
            <asp:Label ID="lblLatestHODStaffInstruction" runat="server"
                CssClass="staff-hod-instruction-text" />
        </div>
    </asp:Panel>
       <!-- Progress form: separate card -->
    <asp:Panel ID="pnlStaffProgress" runat="server"
        CssClass="staff-detail-card staff-progress-card" Visible="false">
        <div class="staff-detail-card-header">
            <div>
                <h5>Submit Progress Update</h5>
                <p>Record the actions you have taken in response to the HOD instruction.</p>
            </div>
        </div>
        <div class="staff-progress-form">
            <div class="staff-resolution-field">
                <label for="<%= txtStaffEscalationProgress.ClientID %>">
                    Progress Update <span class="staff-required">*</span>
                </label>
                <asp:TextBox ID="txtStaffEscalationProgress" runat="server"
                    TextMode="MultiLine" Rows="4" MaxLength="2000"
                    CssClass="form-control staff-resolution-textarea"
                    placeholder="Describe the actions you have taken in response to the HOD instruction..." />
                <small>Maximum 2,000 characters.</small>
            </div>
            <div class="staff-resolution-actions">
                <asp:Button ID="btnSubmitStaffEscalationProgress" runat="server"
                    Text="Submit Progress Update"
                    CssClass="btn btn-primary"
                    CausesValidation="false"
                    OnClick="btnSubmitStaffEscalationProgress_Click" />
            </div>
        </div>
    </asp:Panel>
    <!-- Separate history card: only HOD-to-Staff instructions and Staff updates -->
    <div class="staff-detail-card staff-escalation-history-card">
        <div class="staff-detail-card-header">
            <div>
                <h5>Staff Escalation History</h5>
                <p>Chronological record of HOD instructions to Staff and Staff progress updates.</p>
            </div>
        </div>
        <div class="staff-escalation-timeline">
            <asp:Repeater ID="rptStaffEscalationTimeline" runat="server">
                <HeaderTemplate>
                    <div class="staff-timeline-list">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="staff-timeline-item">
                        <div class="staff-timeline-activity"><%#: Eval("trackingactivityDescription") %></div>
                        <div class="staff-timeline-date"><%# Eval("trackingactivitydatetime", "{0:dd MMM yyyy, hh:mm tt}") %></div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Panel ID="pnlNoStaffTimeline" runat="server" Visible="false"
                CssClass="staff-timeline-empty">
                No HOD instructions or Staff progress updates are available yet.
            </asp:Panel>
        </div>
    </div>

 
</asp:Panel>

<!-- Proposed Resolution -->
<div class="staff-detail-card staff-resolution-card">

    <div class="staff-detail-card-header">
        <div>
            <h5>Proposed Resolution</h5>
            <p class="staff-resolution-subtitle">
                Provide your proposed solution for review by the Head of Department.
            </p>
        </div>
    </div>

    <div class="staff-resolution-form">

        <div class="staff-resolution-field">
            <label for="<%= txtProposedResolution.ClientID %>">
                Proposed Resolution <span class="staff-required">*</span>
            </label>

            <asp:TextBox
                ID="txtProposedResolution"
                runat="server"
                TextMode="MultiLine"
                Rows="5"
                CssClass="form-control staff-resolution-textarea"
                MaxLength="10000"
                placeholder="Describe the solution proposed to resolve this Grievance..." />

            <small>
                Explain the action taken or the solution you recommend.
            </small>
        </div>

        <div class="staff-resolution-field">
            <label for="<%= txtStaffRemarks.ClientID %>">
                Staff Remarks
            </label>

            <asp:TextBox
                ID="txtStaffRemarks"
                runat="server"
                TextMode="MultiLine"
                Rows="3"
                CssClass="form-control staff-resolution-textarea"
                MaxLength="2000"
                placeholder="Add any relevant remarks for the HOD..." />

            <small>
                Add any additional information that may help the HOD review.
            </small>
        </div>

        <div class="staff-resolution-field">
            <label for="<%= fuResolutionDocuments.ClientID %>">
                Supporting Documents
            </label>

            <asp:FileUpload
                ID="fuResolutionDocuments"
                runat="server"
                AllowMultiple="true"
                CssClass="form-control-file staff-resolution-file" />

            <small>
                Attach relevant evidence supporting your proposed resolution.
            </small>
        </div>
        <!-- Previously Submitted Resolution Supporting Documents -->
<asp:Panel ID="pnlResolutionEvidence" runat="server"
    CssClass="staff-detail-card staff-resolution-evidence-card" Visible="false">

    <div class="staff-detail-card-header">
        <h5>Submitted Resolution Documents</h5>
    </div>

    <asp:Repeater ID="rptResolutionEvidence" runat="server">
        <HeaderTemplate>
            <div class="staff-evidence-list">
        </HeaderTemplate>
        <ItemTemplate>
            <div class="staff-evidence-item">
                <div>
                    <strong><%#: Eval("EvidenceName") %></strong>
                    <p><%#: Eval("EvidenceDescription") %></p>
                </div>
                <div class="staff-evidence-actions">
                    <asp:HyperLink
                        ID="lnkOpenResolutionEvidence"
                        runat="server"
                        CssClass="btn btn-sm btn-outline-primary"
                        NavigateUrl='<%# ResolveUrl("~/DownloadEvidence.aspx?id=" + Eval("EvidenceID") + "&mode=open") %>'
                        Target="_blank">
                        <i class="fas fa-eye"></i> View
                    </asp:HyperLink>
                    <asp:HyperLink
                        ID="lnkDownloadResolutionEvidence"
                        runat="server"
                        CssClass="btn btn-sm btn-outline-primary"
                        NavigateUrl='<%# ResolveUrl("~/DownloadEvidence.aspx?id=" + Eval("EvidenceID") + "&mode=download") %>'>
                        <i class="fas fa-download"></i> Download
                    </asp:HyperLink>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>
</asp:Panel>

        <div class="staff-resolution-actions">
            <asp:Button
    ID="btnSubmitResolution"
    runat="server"
    Text="Submit Proposed Resolution"
    CssClass="btn btn-primary"
    CausesValidation="false"
    OnClick="btnSubmitResolution_Click" />
        </div>

    </div>

</div>
    <script type="text/javascript">
        function toggleEscalationForm() {
            var form = document.getElementById(
                '<%= pnlEscalationForm.ClientID %>');
            var button = document.getElementById('btnToggleEscalation');

            if (!form || !button) {
                console.error("Escalation form or button not found.");
                return;
            }

            form.classList.remove('escalation-form-hidden');
            button.style.display = 'none';
        }

        function cancelEscalationForm() {
            var form = document.getElementById(
                '<%= pnlEscalationForm.ClientID %>');
            var button = document.getElementById('btnToggleEscalation');

            if (!form || !button) {
                return;
            }

            form.classList.add('escalation-form-hidden');
            button.style.display = '';
        }
    </script>
</asp:Content>
