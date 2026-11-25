<%@ Page Title="" Language="C#" MasterPageFile="~/Student/StudentMaster.master"
    AutoEventWireup="true"
    CodeFile="GrievanceDetails.aspx.cs"
    Inherits="Student_GrievanceDetails" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="HeadContent"
    Runat="Server">

    <link href="css/GrievanceDetails.css?v=5" rel="stylesheet" />

</asp:Content>


<asp:Content ID="Content2"
    ContentPlaceHolderID="MainContent"
    Runat="Server">

    <div class="container-fluid grievance-details-page">

        <!-- =====================================================
             PAGE HEADER
             ===================================================== -->

        <div class="details-page-header">

            <div>
                <h1>
                    Grievance Details
                </h1>

                <p class="page-subtitle">
                    View complete information about your grievance.
                </p>
            </div>


            <asp:LinkButton
                ID="btnBack"
                runat="server"
                CssClass="btn btn-light border back-btn"
                OnClick="btnBack_Click">

                ← Back to My Grievances

            </asp:LinkButton>

        </div>


        <!-- =====================================================
             MAIN DETAILS CARD
             ===================================================== -->

        <div class="grievance-details-card">


            <!-- =================================================
                 LEFT + RIGHT CONTENT
                 ================================================= -->

            <div class="details-main-layout">


                <!-- =============================================
                     LEFT — GRIEVANCE INFORMATION
                     ============================================= -->

                <div class="details-info-panel">


                    <div class="details-panel-heading">

                        <div>
                            <h2>
                                Grievance Information
                            </h2>

                            <p>
                                Details of your submitted grievance
                            </p>
                        </div>


                        <!-- STATUS -->

                        <span id="statusBadge"
                            runat="server"
                            class="grievance-status">

                            <asp:Label
                                ID="lblStatus"
                                runat="server">
                            </asp:Label>

                        </span>

                    </div>


                    <!-- =========================================
                         INFORMATION
                         ========================================= -->

                    <div class="details-fields">


                        <!-- Grievance ID -->

                        <div class="detail-field">

                            <span class="detail-label">
                                Grievance ID
                            </span>

                            <span class="detail-separator">
                                :
                            </span>

                            <span class="detail-value">

                                <asp:Label
                                    ID="lblGrievanceID"
                                    runat="server">
                                </asp:Label>

                            </span>

                        </div>


                        <!-- Submission Date -->

                        <div class="detail-field">

                            <span class="detail-label">
                                Submission Date
                            </span>

                            <span class="detail-separator">
                                :
                            </span>

                            <span class="detail-value">

                                <asp:Label
                                    ID="lblSubmissionDate"
                                    runat="server">
                                </asp:Label>

                            </span>

                        </div>
                        <!-- Category -->

<div class="detail-field">

    <span class="detail-label">
        Category
    </span>

    <span class="detail-separator">
        :
    </span>

    <span class="detail-value">

        <asp:Label
            ID="lblCategory"
            runat="server">
        </asp:Label>

    </span>

</div>
                        <!-- Department -->

<div class="detail-field">

    <span class="detail-label">
        Department
    </span>

    <span class="detail-separator">
        :
    </span>

    <span class="detail-value">

        <asp:Label
            ID="lblDepartment"
            runat="server">
        </asp:Label>

    </span>

</div>


                        <!-- Grievance Title -->

                        <div class="detail-field">

                            <span class="detail-label">
                                Grievance Title
                            </span>

                            <span class="detail-separator">
                                :
                            </span>

                            <span class="detail-value">

                                <asp:Label
                                    ID="lblTitle"
                                    runat="server">
                                </asp:Label>

                            </span>

                        </div>


                        <!-- Priority -->

                        <div class="detail-field">

                            <span class="detail-label">
                                Priority
                            </span>

                            <span class="detail-separator">
                                :
                            </span>

                            <span class="detail-value">

                                <asp:Label
                                    ID="lblPriority"
                                    runat="server"
                                    CssClass="priority-badge">
                                </asp:Label>

                            </span>

                        </div>


                        <!-- Resolution Date -->

                        <div class="detail-field">

                            <span class="detail-label">
                                Resolution Date
                            </span>

                            <span class="detail-separator">
                                :
                            </span>

                            <span class="detail-value">

                                <asp:Label
                                    ID="lblResolutionDate"
                                    runat="server">
                                </asp:Label>

                            </span>

                        </div>


                    </div>


                    <!-- =========================================
                         EVIDENCE
                         ========================================= -->

                    <div class="details-subsection">

                        <h3>
                            Evidence
                        </h3>


                        <asp:Panel
                            ID="pnlEvidence"
                            runat="server">

                            <asp:Repeater
                                ID="rptEvidence"
                                runat="server">

                                <ItemTemplate>

                                    <div class="evidence-item">

                                        <span class="evidence-icon">
                                            📎
                                        </span>


                                       
<asp:HyperLink
    ID="lnkEvidence"
    runat="server"
    Text='<%# Eval("EvidenceName") %>'
    NavigateUrl='<%# ResolveUrl("~/DownloadEvidence.aspx?id=" + Eval("EvidenceID") + "&mode=download") %>'
    CssClass="evidence-link">
</asp:HyperLink>


                                    </div>

                                </ItemTemplate>

                            </asp:Repeater>

                        </asp:Panel>


                        <asp:Panel
                            ID="pnlNoEvidence"
                            runat="server"
                            CssClass="pnl-no-evidence"
                            Visible="false">

                            <span>
                                No evidence attached.
                            </span>

                        </asp:Panel>

                    </div>


                    <!-- =========================================
                         DESCRIPTION
                         ========================================= -->

                    <div class="details-subsection description-section">

                        <h3>
                            Grievance Description
                        </h3>


                        <div class="description-box">

                            <asp:Label
                                ID="lblDescription"
                                runat="server">
                            </asp:Label>

                        </div>

                    </div>


                </div>

 
                <!-- =============================================
                     RIGHT — GRIEVANCE PROGRESS
                     ============================================= -->

                <div class="details-progress-panel">


                    <div class="details-panel-heading progress-panel-heading">

                        <div>
                            <h2>
                                Grievance Progress
                            </h2>

                            <p>
                                Track the current status of your grievance
                            </p>
                        </div>

                    </div>

<div id="stepSubmitted" runat="server" class="tracking-step">
   <span id="circleSubmitted" runat="server" class="step-circle">1</span>
    <div class="step-content">
        <strong>Grievance Submitted</strong>
        <span>Your Grievance has been submitted successfully.</span>
    </div>
</div>



<div id="stepDepartmentReview" runat="server" class="tracking-step">
    <span id="circleDepartmentReview" runat="server" class="step-circle">2</span>
    <div class="step-content">
        <strong>Department Review</strong>
         <span>The department is reviewing your Grievance.</span>
         </div>
</div>

<div id="stepAssigned" runat="server" class="tracking-step">
   <span id="circleAssigned" runat="server" class="step-circle">3</span>
    <div class="step-content">
        <strong>Assigned to Staff</strong>
         <span> Your Grievance has been assigned to a staff member.</span>
    </div>
</div>

<div id="stepResolutionReview" runat="server" class="tracking-step">
     <span id="circleResolutionReview" runat="server" class="step-circle">4</span>
    <div class="step-content">
        <strong>Resolution Under Review</strong>
         <span> The proposed resolution is awaiting department review.</span>
          </div>
</div>

<div id="stepResolved" runat="server" class="tracking-step">
   <span id="circleResolved" runat="server" class="step-circle">5</span>
    <div class="step-content">
        <strong>Resolved</strong>
         <span>  Your Grievance has been resolved.</span>
       </div>
</div>

<div id="stepRejected" runat="server"
     class="tracking-step rejected-step" visible="false">

    <span class="step-circle">!</span>

    <div class="step-content">
        <strong>Rejected</strong>
        <span>Your Grievance has been rejected.</span>
    </div>
</div>


                </div>


            </div>


        </div>

    </div>
                   
<!-- STUDENT FEEDBACK -->
<div class="feedback-card">

    <h3>Grievance Feedback</h3>

    <asp:Panel ID="pnlFeedbackForm" runat="server" Visible="false">

        <p class="feedback-description">
            Please rate your experience with the resolution of this Grievance.
        </p>

        <div class="feedback-field">
            <label for="<%= rblRating.ClientID %>">
                Satisfaction Rating
            </label>

            <asp:RadioButtonList
                ID="rblRating"
                runat="server"
                RepeatDirection="Horizontal"
                RepeatLayout="Flow"
                CssClass="feedback-rating">
                <asp:ListItem Value="1">1 ★</asp:ListItem>
                <asp:ListItem Value="2">2 ★</asp:ListItem>
                <asp:ListItem Value="3">3 ★</asp:ListItem>
                <asp:ListItem Value="4">4 ★</asp:ListItem>
                <asp:ListItem Value="5">5 ★</asp:ListItem>
            </asp:RadioButtonList>

            <asp:RequiredFieldValidator
                ID="rfvRating"
                runat="server"
                ControlToValidate="rblRating"
                InitialValue=""
                ErrorMessage="Please select a rating."
                CssClass="feedback-validation"
                Display="Dynamic"
                ValidationGroup="StudentFeedback" />
        </div>

        <div class="feedback-field">
            <label for="<%= txtFeedbackComments.ClientID %>">
                Comments (Optional)
            </label>

            <asp:TextBox
                ID="txtFeedbackComments"
                runat="server"
                TextMode="MultiLine"
                Rows="4"
                MaxLength="1000"
                CssClass="form-control"
                placeholder="Share your experience with the resolution..." />
        </div>

        <asp:Label
            ID="lblFeedbackMessage"
            runat="server"
            CssClass="feedback-message"
            EnableViewState="false" />

        <asp:Button
            ID="btnSubmitFeedback"
            runat="server"
            Text="Submit Feedback"
            CssClass="btn btn-primary"
            ValidationGroup="StudentFeedback"
            CausesValidation="true"
            OnClick="btnSubmitFeedback_Click" />

    </asp:Panel>

    <asp:Panel ID="pnlFeedbackSubmitted" runat="server" Visible="false">

        <p class="feedback-success">
            You have already submitted feedback for this Grievance.
        </p>

        <div class="feedback-field">
            <label>Your Rating</label>
            <asp:Label ID="lblSubmittedRating" runat="server" />
        </div>

        <div class="feedback-field">
            <label>Your Comments</label>
            <asp:Label ID="lblSubmittedComments" runat="server" />
        </div>

        <div class="feedback-field">
            <label>Submitted Date</label>
            <asp:Label ID="lblFeedbackDate" runat="server" />
        </div>

    </asp:Panel>

    <asp:Panel ID="pnlFeedbackUnavailable" runat="server" Visible="false">
        <p class="feedback-description">
            Feedback will be available after your Grievance has been resolved.
        </p>
    </asp:Panel>

</div>

</asp:Content>