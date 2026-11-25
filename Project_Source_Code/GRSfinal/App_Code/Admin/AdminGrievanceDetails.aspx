<%@ Page Title="Grievance Details"
    Language="C#"
    MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true"
    CodeFile="AdminGrievanceDetails.aspx.cs"
    Inherits="Admin_AdminGrievanceDetails" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="HeadContent"
    Runat="Server">

    <link href="css/AdminGrievanceDetails.css?v=6" rel="stylesheet" />

</asp:Content>


<asp:Content ID="Content2"
    ContentPlaceHolderID="MainContent"
    Runat="Server">

    <div class="admin-grievance-details-page">

        <!-- PAGE HEADER -->
        <div class="details-page-header">

            <div>
                <h1>Grievance Details</h1>

                <p class="page-subtitle">
                    View complete information about the selected grievance.
                </p>
            </div>

            <asp:LinkButton
                ID="btnBack"
                runat="server"
                CssClass="back-btn"
                OnClick="btnBack_Click">

                ← Back to All Grievances

            </asp:LinkButton>

        </div>


        <!-- =====================================================
             OVERVIEW + PROGRESS
             ===================================================== -->

        <div class="main-details-card">

            <!-- LEFT: OVERVIEW -->

            <div class="overview-panel">

                <div class="panel-heading">

                    <div>
                        <h2>Grievance Overview</h2>

                        <p>
                            Basic information and current status of the grievance.
                        </p>
                    </div>

                    <div class="overview-badges">

                        <span
                            id="statusBadge"
                            runat="server"
                            class="grievance-status">

                            <asp:Label
                                ID="lblStatus"
                                runat="server">
                            </asp:Label>

                        </span>

                        <asp:Label
                            ID="lblPriority"
                            runat="server"
                            CssClass="priority-badge">
                        </asp:Label>

                    </div>

                </div>


                <div class="details-fields">

                    <div class="detail-field">

                        <span class="detail-label">
                            Grievance ID
                        </span>

                        <span class="detail-separator">:</span>

                        <asp:Label
                            ID="lblGrievanceID"
                            runat="server"
                            CssClass="detail-value">
                        </asp:Label>

                    </div>


                    <div class="detail-field">

                        <span class="detail-label">
                            Submission Date
                        </span>

                        <span class="detail-separator">:</span>

                        <asp:Label
                            ID="lblSubmissionDate"
                            runat="server"
                            CssClass="detail-value">
                        </asp:Label>

                    </div>


                    <div class="detail-field">

                        <span class="detail-label">
                            Student Name
                        </span>

                        <span class="detail-separator">:</span>

                        <asp:Label
                            ID="lblStudentName"
                            runat="server"
                            CssClass="detail-value">
                        </asp:Label>

                    </div>


                    <div class="detail-field">

                        <span class="detail-label">
                            Registration No.
                        </span>

                        <span class="detail-separator">:</span>

                        <asp:Label
                            ID="lblStudentRegNo"
                            runat="server"
                            CssClass="detail-value">
                        </asp:Label>

                    </div>


                    <div class="detail-field">

                        <span class="detail-label">
                            Category
                        </span>

                        <span class="detail-separator">:</span>

                        <asp:Label
                            ID="lblCategory"
                            runat="server"
                            CssClass="detail-value">
                        </asp:Label>

                    </div>


                    <div class="detail-field">

                        <span class="detail-label">
                            Department
                        </span>

                        <span class="detail-separator">:</span>

                        <asp:Label
                            ID="lblDepartment"
                            runat="server"
                            CssClass="detail-value">
                        </asp:Label>

                    </div>


                    <div class="detail-field">

                        <span class="detail-label">
                            Grievance Title
                        </span>

                        <span class="detail-separator">:</span>

                        <asp:Label
                            ID="lblTitle"
                            runat="server"
                            CssClass="detail-value">
                        </asp:Label>

                    </div>

                </div>

            </div>


            <!-- RIGHT: PROGRESS -->

            <div class="progress-panel">

                <div class="progress-heading">

                    <h2>Grievance Progress</h2>

                    <p>
                        Current processing stage of the grievance.
                    </p>

                </div>

               <div class="tracking-timeline">

   

<!-- 1. GRIEVANCE SUBMITTED -->
<div id="stepSubmitted"
    runat="server"
    class="tracking-step">

    <div class="step-circle">
        <asp:Label
            ID="lblStepSubmitted"
            runat="server"
            Text="1">
        </asp:Label>
    </div>

    <div class="step-content">
        <strong>Grievance Submitted</strong>

        <asp:Label
            ID="lblTimelineSubmitted"
            runat="server">
        </asp:Label>
    </div>
</div>


<!-- 2. DEPARTMENT REVIEW -->
<div id="stepDepartment"
    runat="server"
    class="tracking-step">

    <div class="step-circle">
        <asp:Label
            ID="lblStepDepartment"
            runat="server"
            Text="2">
        </asp:Label>
    </div>

    <div class="step-content">
        <strong>Department Review</strong>

        <asp:Label
            ID="lblTimelineDepartment"
            runat="server">
        </asp:Label>
    </div>
</div>


<!-- 3. ASSIGNED TO STAFF -->
<div id="stepHodReview"
    runat="server"
    class="tracking-step">

    <div class="step-circle">
        <asp:Label
            ID="lblStepHodReview"
            runat="server"
            Text="3">
        </asp:Label>
    </div>

    <div class="step-content">
        <strong>Assigned to Staff</strong>

        <asp:Label
            ID="lblTimelineHodReview"
            runat="server">
        </asp:Label>
    </div>
</div>


<!-- 4. RESOLUTION UNDER REVIEW -->
<div id="stepAssigned"
    runat="server"
    class="tracking-step">

    <div class="step-circle">
        <asp:Label
            ID="lblStepAssigned"
            runat="server"
            Text="4">
        </asp:Label>
    </div>

    <div class="step-content">
        <strong>Resolution Under Review</strong>

        <asp:Label
            ID="lblTimelineAssigned"
            runat="server">
        </asp:Label>
    </div>
</div>


<!-- 5. RESOLVED -->
<div id="stepResolved"
    runat="server"
    class="tracking-step">

    <div class="step-circle">
        <asp:Label
            ID="lblStepResolved"
            runat="server"
            Text="5">
        </asp:Label>
    </div>

    <div class="step-content">
        <strong>Resolved</strong>

        <asp:Label
            ID="lblTimelineResolved"
            runat="server">
        </asp:Label>
    </div>
</div>
</div>


</div>
               

            </div>

        </div>


        <!-- =====================================================
             DESCRIPTION & EVIDENCE
             ===================================================== -->

        <div class="details-card">

            <div class="section-heading">

                <div>
                    <h2>Description &amp; Evidence</h2>

                    <p>
                        Submitted grievance description and supporting evidence.
                    </p>
                </div>

            </div>


            <div class="description-section">

                <span class="detail-label">
                    Grievance Description
                </span>

                <div class="description-box">

                    <asp:Label
                        ID="lblDescription"
                        runat="server">
                    </asp:Label>

                </div>

            </div>


            <div class="evidence-section">

                <span class="detail-label">
                    Supporting Evidence
                </span>


                <asp:Panel
                    ID="pnlEvidence"
                    runat="server"
                    CssClass="evidence-list">

                    <asp:Repeater
                        ID="rptEvidence"
                        runat="server">

                   
<ItemTemplate>
    <a
        href='<%# ResolveUrl(
            "~/DownloadEvidence.aspx?id=" +
            Eval("EvidenceID") +
            "&mode=open"
        ) %>'
        target="_blank"
        rel="noopener noreferrer"
        class="evidence-item">

        <span class="evidence-icon">📎</span>

        <span class="evidence-name">
            <%# Server.HtmlEncode(
                Convert.ToString(Eval("EvidenceName"))
            ) %>
        </span>

        <span class="evidence-view">View</span>
    </a>
</ItemTemplate>


                    </asp:Repeater>

                </asp:Panel>


                <asp:Panel
                    ID="pnlNoEvidence"
                    runat="server"
                    CssClass="no-evidence">

                    No supporting evidence was submitted with this grievance.

                </asp:Panel>

            </div>

        </div>


        <!-- =====================================================
             ASSIGNMENT & HANDLING
             ===================================================== -->

        <div class="details-card">

            <div class="section-heading">

                <div>
                    <h2>Assignment &amp; Handling</h2>

                    <p>
                        Current department and responsibility information.
                    </p>
                </div>

            </div>


            <div class="assignment-grid">

                <div class="detail-field">

                    <span class="detail-label">
                        Assigned Department
                    </span>

                    <span class="detail-separator">:</span>

                    <asp:Label
                        ID="lblAssignedDepartment"
                        runat="server"
                        CssClass="detail-value">
                    </asp:Label>

                </div>


                <div class="detail-field">

                    <span class="detail-label">
                        Department Head
                    </span>

                    <span class="detail-separator">:</span>

                    <asp:Label
                        ID="lblDepartmentHead"
                        runat="server"
                        CssClass="detail-value">
                    </asp:Label>

                </div>


                <div class="detail-field">

                    <span class="detail-label">
                        Assigned Staff
                    </span>

                    <span class="detail-separator">:</span>

                    <asp:Label
                        ID="lblAssignedStaff"
                        runat="server"
                        CssClass="detail-value">
                    </asp:Label>

                </div>


                <div class="detail-field">

                    <span class="detail-label">
                        Assigned On
                    </span>

                    <span class="detail-separator">:</span>

                    <asp:Label
                        ID="lblAssignedOn"
                        runat="server"
                        CssClass="detail-value">
                    </asp:Label>

                </div>


                <div class="detail-field detail-field-full">

                    <span class="detail-label">
                        Handling Stage
                    </span>

                    <span class="detail-separator">:</span>

                    <asp:Label
                        ID="lblHandlingStage"
                        runat="server"
                        CssClass="handling-stage">
                    </asp:Label>

                </div>

            </div>

        </div>


        <!-- =====================================================
             RESOLUTION / REVIEW
             ===================================================== -->

        <div class="details-card">

            <div class="section-heading">

                <div>
                    <h2>Resolution / Review</h2>

                    <p>
                        Resolution information recorded for the grievance.
                    </p>
                </div>

            </div>
        
<div class="resolution-grid">

    <div class="detail-field">
        <span class="detail-label">Resolution Date</span>
        <span class="detail-separator">:</span>
        <asp:Label
            ID="lblResolutionDate"
            runat="server"
            CssClass="detail-value" />
    </div>

    <div class="detail-field">
        <span class="detail-label">Current Status</span>
        <span class="detail-separator">:</span>
        <asp:Label
    ID="lblResolutionStatus"
    runat="server"
    CssClass="resolution-status-badge" />
    </div>

</div>
        </div>


</asp:Content>