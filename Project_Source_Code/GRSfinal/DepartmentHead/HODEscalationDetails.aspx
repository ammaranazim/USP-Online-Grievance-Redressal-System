
<%@ Page Title="Escalation Details"
    Language="C#"
    MasterPageFile="~/DepartmentHead/HODMaster.master"
    AutoEventWireup="true"
    CodeFile="HODEscalationDetails.aspx.cs"
    Inherits="DepartmentHead_HODEscalationDetails" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    Escalation Details
</asp:Content>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="css/HODEscalationDetails.css?v=1" rel="stylesheet" />
    
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="hod-escalation-details">

        <div class="page-header">
            <a href="HODEscalations.aspx" class="back-link">
                &larr; Back to Escalations
            </a>

            <h2 class="page-title">Escalation Details</h2>
            <p class="page-subtitle">
                Review the escalation request and record your decision.
            </p>
        </div>

        <asp:Panel ID="pnlMessage" runat="server" Visible="false">
            <asp:Label ID="lblMessage" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlDetails" runat="server" Visible="false">

            <div class="details-card">
                <div class="card-heading">
                    <h3>Request Information</h3>

                   <asp:Label ID="lblRequestStatus" runat="server"
    CssClass="escalation-status escalation-pending" />
                </div>

                <div class="details-grid">

                    <div class="detail-item">
                        <span class="detail-label">Escalation ID</span>
                        <asp:Label ID="lblEscalationID" runat="server"
                            CssClass="detail-value" />
                    </div>

                    <div class="detail-item">
                        <span class="detail-label">Grievance ID</span>
                        <asp:Label ID="lblGrievanceID" runat="server"
                            CssClass="detail-value" />
                    </div>

                    <div class="detail-item">
                        <span class="detail-label">Requested Date</span>
                        <asp:Label ID="lblRequestedDate" runat="server"
                            CssClass="detail-value" />
                    </div>

                    <div class="detail-item">
                        <span class="detail-label">Student Name</span>
                        <asp:Label ID="lblStudentName" runat="server"
                            CssClass="detail-value" />
                    </div>

                    <div class="detail-item">
                        <span class="detail-label">Registration Number</span>
                        <asp:Label ID="lblRegistrationNo" runat="server"
                            CssClass="detail-value" />
                    </div>

                    <div class="detail-item">
                        <span class="detail-label">Category</span>
                        <asp:Label ID="lblCategory" runat="server"
                            CssClass="detail-value" />
                    </div>

                    <div class="detail-item">
                        <span class="detail-label">Department</span>
                        <asp:Label ID="lblDepartment" runat="server"
                            CssClass="detail-value" />
                    </div>

                    <div class="detail-item">
                        <span class="detail-label">Grievance Priority</span>
                        <asp:Label ID="lblPriority" runat="server"
                            CssClass="detail-value" />
                    </div>

                    <div class="detail-item detail-full">
                        <span class="detail-label">Grievance Title</span>
                        <asp:Label ID="lblGrievanceTitle" runat="server"
                            CssClass="detail-value" />
                    </div>

                    <div class="detail-item detail-full">
                        <span class="detail-label">Grievance Description</span>
                        <asp:Label ID="lblGrievanceDescription" runat="server"
                            CssClass="detail-value detail-description" />
                    </div>

                    <div class="detail-item detail-full">
                        <span class="detail-label">Escalation Reason</span>
                        <asp:Label ID="lblEscalationReason" runat="server"
                            CssClass="detail-value detail-description" />
                    </div>

                    <div class="detail-item detail-full">
                        <span class="detail-label">Escalation Description</span>
                        <asp:Label ID="lblEscalationDescription" runat="server"
                            CssClass="detail-value detail-description" />
                    </div>

                </div>
            </div>

            <div class="details-card">
              

                <asp:Panel ID="pnlHODDecision" runat="server">

                    <div class="form-group">
                        <label for="<%= txtHODRemarks.ClientID %>">
                            HOD Remarks <span class="required-mark">*</span>
                        </label>

                        <asp:TextBox ID="txtHODRemarks" runat="server"
                            TextMode="MultiLine"
                            Rows="4"
                            MaxLength="2000"
                            CssClass="remarks-input"
                            placeholder="Enter your review remarks..." />

                        <small class="field-help">
                            Remarks are required for both forwarding and rejection.
                        </small>
                    </div>

                    <div class="decision-actions">
                        <asp:Button ID="btnForwardToAdmin" runat="server"
                            Text="Forward to Admin"
                            CssClass="btn-forward"
                            OnClick="btnForwardToAdmin_Click"
                            OnClientClick="return confirm('Forward this escalation to Admin for review?');" />

                        <asp:Button ID="btnReject" runat="server"
                            Text="Reject Escalation"
                            CssClass="btn-reject"
                            OnClick="btnReject_Click"
                            OnClientClick="return confirm('Are you sure you want to reject this escalation?');" />
                    </div>

                </asp:Panel>

                <asp:Panel ID="pnlDecisionLocked" runat="server"
                    Visible="false" CssClass="decision-locked">
                    This escalation is no longer awaiting HOD review.
                </asp:Panel>
            </div>
            <!-- Admin Feedback -->
<asp:Panel ID="pnlAdminFeedback" runat="server"
   Visible="false" CssClass="details-card admin-feedback-card">

  <div class="card-heading admin-feedback-heading">
    <h3>Admin Feedback</h3>

    <div class="admin-feedback-badges">
       <asp:Label ID="lblAdminFeedbackStatus" runat="server"
    CssClass="admin-feedback-badge escalation-pending" />

        <asp:Label ID="lblAdminActionType" runat="server"
            CssClass="admin-feedback-badge admin-action-badge" />
    </div>
</div>

    <div class="details-grid">
        <div class="detail-item detail-full">
           <asp:Label ID="lblAdminRemarksLabel" runat="server"
    CssClass="detail-label"
    Text="Admin Remarks" />
            <asp:Label ID="lblAdminRemarks" runat="server"
                CssClass="detail-value detail-description" />
        </div>

        <div class="detail-item">
            <span class="detail-label">Admin Decision Date</span>
            <asp:Label ID="lblAdminDecisionDate" runat="server"
                CssClass="detail-value" />
        </div>
    </div>

    <asp:Panel ID="pnlHODResponse" runat="server"
    Visible="false" CssClass="hod-admin-response">
        <div class="form-group">
            <label for="<%= txtHODAdminResponse.ClientID %>">
                HOD Response <span class="required-mark">*</span>
            </label>
<asp:TextBox ID="txtHODAdminResponse" runat="server"
    TextMode="MultiLine"
    Rows="4"
    MaxLength="400"
    CssClass="remarks-input"
    placeholder="Enter your response or follow-up update..." />

            <small class="field-help">
                Submit your response to Admin. Your response will be added to the escalation activity history.
            </small>
        </div>

        <div class="decision-actions">
            <asp:Button ID="btnSubmitHODResponse" runat="server"
                Text="Submit Response to Admin"
                CssClass="btn-forward"
                OnClick="btnSubmitHODResponse_Click"
                OnClientClick="return confirm('Submit this response to Admin?');" />
        </div>

    </asp:Panel>

    <asp:Panel ID="pnlHODResponseLocked" runat="server"
        Visible="false" CssClass="decision-locked">
        A response cannot be submitted for this escalation in its current status.
    </asp:Panel>

</asp:Panel>

            <asp:Panel ID="pnlHODStaffInstruction" runat="server"
    Visible="false" CssClass="details-card">

    <div class="card-heading">
        <h3>Instructions to Staff</h3>
    </div>

    <div class="form-group">
        <label for="<%= txtHODStaffInstruction.ClientID %>">
            HOD Instructions <span class="required-mark">*</span>
        </label>

        <asp:TextBox ID="txtHODStaffInstruction" runat="server"
            TextMode="MultiLine"
            Rows="4"
            MaxLength="400"
            CssClass="remarks-input"
            placeholder="Translate Admin's decision into clear instructions for Staff..." />

        <small class="field-help">
            Enter the actions Staff must take in response to the Admin decision.
        </small>
    </div>

    <div class="decision-actions">
        <asp:Button ID="btnSaveHODStaffInstruction" runat="server"
            Text="Send Instructions to Staff"
            CssClass="btn-forward"
            OnClick="btnSaveHODStaffInstruction_Click"
            OnClientClick="return confirm('Send these instructions to Staff?');" />
    </div>

</asp:Panel>
            <div class="details-card">
                <div class="card-heading">
                    <h3>Escalation Activity</h3>
                </div>

                <asp:Repeater ID="rptEscalationTimeline" runat="server">
                    <HeaderTemplate>
                        <div class="timeline-list">
                    </HeaderTemplate>

                    <ItemTemplate>
                        <div class="timeline-item">
                            <div class="timeline-marker"></div>

                            <div class="timeline-content">
                                <div class="timeline-description">
                                    <%#: Eval("trackingactivityDescription") %>
                                </div>

                                <div class="timeline-date">
                                    <%# Eval("trackingactivitydatetime", "{0:dd MMM yyyy, hh:mm tt}") %>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>

                    <FooterTemplate>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>

                <asp:Panel ID="pnlNoTimeline" runat="server"
                    Visible="false" CssClass="empty-timeline">
                    No escalation activity has been recorded yet.
                </asp:Panel>
            </div>

        </asp:Panel>

    </div>

</asp:Content>
