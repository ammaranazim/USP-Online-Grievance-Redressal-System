<%@ Page Title="Grievance Progress &amp; Resolution" Language="C#" MasterPageFile="~/DepartmentHead/HODMaster.master" AutoEventWireup="true" CodeFile="HODProgressResolutionDetails.aspx.cs" Inherits="DepartmentHead_HODProgressResolutionDetails" %>

<asp:Content ID="TitleContent1" ContentPlaceHolderID="TitleContent" Runat="Server">
    Grievance Progress &amp; Resolution
</asp:Content>

<asp:Content ID="HeadContent1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="css/HODProgressResolutionDetails.css?v=2" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" Runat="Server">

<div class="hod-prd-page">

    <div class="hod-prd-header">
        <div>
            <h1>Grievance Progress &amp; Resolution</h1>
            <p>Monitor the grievance progress and review its resolution status.</p>
        </div>

        <asp:HyperLink ID="lnkBack" runat="server"
            NavigateUrl="~/DepartmentHead/HODProgressResolutionReview.aspx"
            CssClass="hod-prd-button hod-prd-button-secondary">
            &larr; Back to Progress &amp; Resolution Review
        </asp:HyperLink>
    </div>

    <!-- Compact Grievance Overview -->
    <section class="hod-prd-card">

        <div class="hod-prd-card-heading">
            <div>
                <h2>Grievance Overview</h2>
                <p>Essential information about the selected grievance.</p>
            </div>

            <div class="hod-prd-badges">
                <asp:Label ID="lblStatus" runat="server"
                    CssClass="status-badge" />

                <asp:Label ID="lblPriority" runat="server"
                    CssClass="priority-badge" />
            </div>
        </div>

        <div class="hod-prd-info-grid hod-prd-compact-grid">

            <div class="hod-prd-info-item">
                <span>Grievance ID</span>
                <strong>
                    <asp:Label ID="lblGrievanceID" runat="server" />
                </strong>
            </div>

            <div class="hod-prd-info-item">
                <span>Grievance Title</span>
                <strong>
                    <asp:Label ID="lblGrievanceTitle" runat="server" />
                </strong>
            </div>

            <div class="hod-prd-info-item">
                <span>Student Name</span>
                <strong>
                    <asp:Label ID="lblStudentName" runat="server" />
                </strong>
            </div>

            <div class="hod-prd-info-item">
                <span>Registration No.</span>
                <strong>
                    <asp:Label ID="lblRegistrationNo" runat="server" />
                </strong>
            </div>

            <div class="hod-prd-info-item">
                <span>Due Date</span>
                <strong>
                    <asp:Label ID="lblDueDate" runat="server" />
                </strong>
            </div>

        </div>
    </section>
    <!-- Original Grievance Evidence -->
    <section class="hod-prd-card">

        <div class="hod-prd-card-heading">
            <div>
                <h2>Supporting Evidence</h2>
                <p>Files attached to the original grievance submission.</p>
            </div>
        </div>

        <asp:Panel ID="pnlNoEvidence" runat="server"
            CssClass="hod-prd-empty"
            Visible="false">
            No supporting evidence was submitted with this grievance.
        </asp:Panel>

        <asp:Repeater ID="rptEvidence" runat="server">
            <ItemTemplate>

                <div class="hod-prd-evidence-row">

                    <span class="hod-prd-evidence-name">
                        <i class="fa fa-file-o" aria-hidden="true"></i>
                        <asp:Label ID="lblEvidenceName" runat="server"
                            Text='<%# Eval("EvidenceName") %>' />
                    </span>

                    <span class="hod-prd-evidence-actions">

                        <asp:HyperLink ID="lnkOpenEvidence" runat="server"
                            NavigateUrl='<%# GetEvidenceUrl(Eval("EvidenceID"), "open") %>'
                            Target="_blank"
                            CssClass="hod-prd-evidence-button">
                            Open
                        </asp:HyperLink>

                        <asp:HyperLink ID="lnkDownloadEvidence" runat="server"
                            NavigateUrl='<%# GetEvidenceUrl(Eval("EvidenceID"), "download") %>'
                            CssClass="hod-prd-evidence-button hod-prd-evidence-button-primary">
                            Download
                        </asp:HyperLink>

                    </span>
                </div>

            </ItemTemplate>
        </asp:Repeater>

    </section>

    <!-- Grievance Progress Timeline -->
    <section class="hod-prd-card">

        <div class="hod-prd-card-heading">
            <div>
                <h2>Grievance Progress</h2>
                <p>Track the grievance through the agreed workflow stages.</p>
            </div>
        </div>

        <div class="hod-prd-timeline">

            <div id="stepSubmitted" runat="server" class="hod-prd-step">
                <div class="hod-prd-step-circle">
                    <asp:Label ID="lblStepSubmitted" runat="server" Text="1" />
                </div>
                <div class="hod-prd-step-content">
                    <strong>Grievance Submitted</strong>
                    <asp:Label ID="lblTimelineSubmitted" runat="server" />
                </div>
            </div>

            <div id="stepDepartmentReview" runat="server" class="hod-prd-step">
                <div class="hod-prd-step-circle">
                    <asp:Label ID="lblStepDepartmentReview" runat="server" Text="2" />
                </div>
                <div class="hod-prd-step-content">
                    <strong>Department Review</strong>
                    <asp:Label ID="lblTimelineDepartmentReview" runat="server" />
                </div>
            </div>

            <div id="stepAssigned" runat="server" class="hod-prd-step">
                <div class="hod-prd-step-circle">
                    <asp:Label ID="lblStepAssigned" runat="server" Text="3" />
                </div>
                <div class="hod-prd-step-content">
                    <strong>Assigned to Staff</strong>
                    <asp:Label ID="lblTimelineAssigned" runat="server" />
                </div>
            </div>

            <div id="stepResolutionReview" runat="server" class="hod-prd-step">
                <div class="hod-prd-step-circle">
                    <asp:Label ID="lblStepResolutionReview" runat="server" Text="4" />
                </div>
                <div class="hod-prd-step-content">
                    <strong>Resolution Under Review</strong>
                    <asp:Label ID="lblTimelineResolutionReview" runat="server" />
                </div>
            </div>

            <div id="stepResolved" runat="server"
                class="hod-prd-step hod-prd-step-last">
                <div class="hod-prd-step-circle">
                    <asp:Label ID="lblStepResolved" runat="server" Text="5" />
                </div>
                <div class="hod-prd-step-content">
                    <strong>Resolved</strong>
                    <asp:Label ID="lblTimelineResolved" runat="server" />
                </div>
            </div>

        </div>
    </section>

    
   
<!-- Resolution Review -->
<section class="hod-prd-card">

    <div class="hod-prd-card-heading">
        <div>
            <h2>Resolution Review</h2>
            <p>Review the proposed resolution submitted by Staff.</p>
        </div>

        <asp:Label ID="lblResolutionState" runat="server"
            CssClass="hod-prd-resolution-state"
            Text="Resolution Pending" />
    </div>
    <asp:Panel ID="pnlReviewMessage" runat="server"
    Visible="false" CssClass="hod-prd-review-message">
    <asp:Label ID="lblReviewMessage" runat="server" />
</asp:Panel>

    <asp:Panel ID="pnlResolutionPending" runat="server"
        CssClass="hod-prd-pending-panel">

        <span class="hod-prd-pending-icon" aria-hidden="true">
            <i class="fa-solid fa-clock"></i>
        </span>

        <div>
            <h3>Resolution Pending</h3>
            <p>
                The proposed resolution will appear here after Staff
                submits it for HOD review.
            </p>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlResolutionSubmitted" runat="server"
        CssClass="hod-prd-resolution-placeholder"
        Visible="false">

        <div class="hod-prd-resolution-field">
            <label>Proposed Resolution</label>
            <asp:Label ID="lblProposedResolution" runat="server"
                CssClass="hod-prd-resolution-text" />
        </div>

        <div class="hod-prd-resolution-field">
            <label>Staff Remarks</label>
            <asp:Label ID="lblStaffRemarks" runat="server"
                CssClass="hod-prd-resolution-text" />
        </div>

        <div class="hod-prd-resolution-field">
            <label>Resolution Supporting Documents</label>

            <asp:Panel ID="pnlNoResolutionEvidence" runat="server"
                CssClass="hod-prd-empty"
                Visible="false">
                No supporting documents were uploaded with the resolution.
            </asp:Panel>

            <asp:Repeater ID="rptResolutionEvidence" runat="server">
                <ItemTemplate>
                    <div class="hod-prd-evidence-row">

                        <span class="hod-prd-evidence-name">
                            <i class="fa fa-file-o" aria-hidden="true"></i>
                            <asp:Label ID="lblResolutionEvidenceName"
                                runat="server"
                                Text='<%# Eval("EvidenceName") %>' />
                        </span>

                        <span class="hod-prd-evidence-actions">
                            <asp:HyperLink ID="lnkOpenResolutionEvidence"
                                runat="server"
                                NavigateUrl='<%# GetEvidenceUrl(Eval("EvidenceID"), "open") %>'
                                Target="_blank"
                                CssClass="hod-prd-evidence-button">
                                Open
                            </asp:HyperLink>

                            <asp:HyperLink ID="lnkDownloadResolutionEvidence"
                                runat="server"
                                NavigateUrl='<%# GetEvidenceUrl(Eval("EvidenceID"), "download") %>'
                                CssClass="hod-prd-evidence-button hod-prd-evidence-button-primary">
                                Download
                            </asp:HyperLink>
                        </span>

                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <asp:Panel ID="pnlHODReviewActions" runat="server"
            Visible="false">

           <asp:Label ID="lblHODRemarksCaption" runat="server"
    AssociatedControlID="txtHODRemarks"
    Text="HOD Remarks"
    CssClass="hod-prd-review-label" />

                <asp:TextBox ID="txtHODRemarks" runat="server"
                    TextMode="MultiLine"
                    Rows="4"
                    MaxLength="200"
                    CssClass="hod-prd-review-textarea"
                    placeholder="Enter your review remarks..." />
            </div>

            <div class="hod-prd-review-actions">
                <asp:Button ID="btnApproveResolution" runat="server"
    Text="Approve Resolution"
    OnClick="btnApproveResolution_Click"
    CssClass="hod-prd-button hod-prd-button-primary"
    CausesValidation="false" />

                <asp:Button ID="btnReturnResolution" runat="server"
    Text="Return to Staff"
    OnClick="btnReturnResolution_Click"
    CssClass="hod-prd-button hod-prd-button-secondary hod-prd-button-danger"
    CausesValidation="false" />
            </div>
        </asp:Panel>

    </asp:Panel>

</section>
</div>

</asp:Content>