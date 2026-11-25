<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminEscalationDetails.aspx.cs" Inherits="Admin_AdminEscalationDetails" %>
<asp:Content ID="Content1"
    ContentPlaceHolderID="HeadContent"
    runat="server">

    <link href="<%= ResolveUrl("~/Admin/css/AdminEscalationDetails.css") %>"
        rel="stylesheet" />

</asp:Content>


<asp:Content ID="Content2"
    ContentPlaceHolderID="MainContent"
    runat="server">


  <!-- PAGE HEADER -->
<div class="escalation-details-header">

    <div class="escalation-details-heading">
        <h1>Escalation Details</h1>
        <p>Review and manage the selected grievance escalation.</p>
    </div>

    <asp:Button
        ID="btnBack"
        runat="server"
        Text="Back to Escalated Grievances"
        CssClass="btn btn-back"
        OnClick="btnBack_Click" />

</div>


    <!-- ESCALATION OVERVIEW -->

    <section class="detail-panel">

        <div class="panel-header">

            <div>
                <h2>Escalation Overview</h2>

                <p>
                    Summary of the escalation request.
                </p>
            </div>

        </div>


        <div class="detail-grid">

            <div class="detail-item">

                <span class="detail-label">
                    Escalation ID
                </span>

                <asp:Label
                    ID="lblEscalationID"
                    runat="server"
                    CssClass="detail-value">
                </asp:Label>

            </div>


            <div class="detail-item">

                <span class="detail-label">
                    Grievance ID
                </span>

                <asp:Label
                    ID="lblGrievanceID"
                    runat="server"
                    CssClass="detail-value">
                </asp:Label>

            </div>


            <div class="detail-item">
    <span class="detail-label">Status</span>

    <asp:Label
        ID="lblStatus"
        runat="server"
        CssClass="escalation-status">
    </asp:Label>
</div>
            <div class="detail-item">

                <span class="detail-label">
                    Escalated Date
                </span>

                <asp:Label
                    ID="lblRequestedDate"
                    runat="server"
                    CssClass="detail-value">
                </asp:Label>

            </div>

        </div>

    </section>


    <!-- RELATED GRIEVANCE -->

    <section class="detail-panel">

        <div class="panel-header">

            <div>
                <h2>Related Grievance</h2>

                <p>
                    Grievance associated with this escalation.
                </p>
            </div>

        </div>


        <div class="detail-grid">

            <div class="detail-item">

                <span class="detail-label">
                    Student
                </span>

                <asp:Label
                    ID="lblStudentName"
                    runat="server"
                    CssClass="detail-value">
                </asp:Label>

            </div>


            <div class="detail-item">

                <span class="detail-label">
                    Registration No.
                </span>

                <asp:Label
                    ID="lblStudentRegNo"
                    runat="server"
                    CssClass="detail-value">
                </asp:Label>

            </div>


            <div class="detail-item">

                <span class="detail-label">
                    Category
                </span>

                <asp:Label
                    ID="lblCategory"
                    runat="server"
                    CssClass="detail-value">
                </asp:Label>

            </div>


            <div class="detail-item">

                <span class="detail-label">
                    Department
                </span>

                <asp:Label
                    ID="lblDepartment"
                    runat="server"
                    CssClass="detail-value">
                </asp:Label>

            </div>


            <div class="detail-item detail-item-full">

                <span class="detail-label">
                    Grievance Title
                </span>

                <asp:Label
                    ID="lblGrievanceTitle"
                    runat="server"
                    CssClass="detail-value">
                </asp:Label>

            </div>

        </div>

    </section>


    <!-- ESCALATION REASON -->

    <section class="detail-panel">

        <div class="panel-header">

            <div>
                <h2>Escalation Reason</h2>

                <p>
                    Reason and supporting explanation provided for escalation.
                </p>
            </div>

        </div>


        <div class="text-section">

            <span class="detail-label">
                Reason
            </span>

            <asp:Label
                ID="lblEscalationReason"
                runat="server"
                CssClass="description-text">
            </asp:Label>


            <div class="description-block">

                <span class="detail-label">
                    Description
                </span>

                <asp:Label
                    ID="lblEscalationDescription"
                    runat="server"
                    CssClass="description-text">
                </asp:Label>

            </div>

            <div class="description-block">

                <span class="detail-label">
                    HOD Remarks
                </span>

                <asp:Label
                    ID="lblHODRemarks"
                    runat="server"
                    CssClass="description-text">
                </asp:Label>

            </div>

        </div>

    </section>
    
<!-- HOD RESPONSE TO ADMIN -->

<section class="detail-panel">

    <div class="panel-header">
        <div>
            <h2>HOD Response to Admin</h2>
            <p>Latest response submitted by the Head of Department.</p>
        </div>
    </div>

    <div class="text-section">

        <span class="detail-label">HOD Response</span>

        <asp:Label
            ID="lblLatestHODResponse"
            runat="server"
            CssClass="description-text">
        </asp:Label>

        <div class="description-block">

            <span class="detail-label">Response Date</span>

            <asp:Label
                ID="lblLatestHODResponseDate"
                runat="server"
                CssClass="description-text">
            </asp:Label>

        </div>

    </div>

</section>

     <!-- ADMINISTRATIVE REVIEW -->
    <asp:Panel ID="pnlAdministrativeReview" runat="server">
    <section class="detail-panel">

        <div class="panel-header">

            <div>
                <h2>Administrative Review</h2>

                <p>
                    Record the administrative decision or required action.
                </p>
            </div>

        </div>


        <div class="review-form">

            <div class="form-group">

                <label for="<%= txtAdminRemarks.ClientID %>">
                    Admin Remarks
                </label>

               <asp:TextBox ID="txtAdminRemarks" runat="server"
    TextMode="MultiLine"
    Rows="4"
    MaxLength="500"
    CssClass="form-control"
    placeholder="Enter remarks or instructions...">
</asp:TextBox>

            </div>


            <div class="action-row">

                <asp:Button
                    ID="btnRequestInfo"
                    runat="server"
                    Text="Request Additional Information"
                    CssClass="btn btn-action btn-warning-action"
                    OnClick="btnRequestInfo_Click" />

                <asp:Button
                    ID="btnGiveDirection"
                    runat="server"
                    Text="Give Direction"
                    CssClass="btn btn-action btn-blue-action"
                    OnClick="btnGiveDirection_Click" />

                <asp:Button
                    ID="btnTakeAction"
                    runat="server"
                    Text="Take Administrative Action"
                    CssClass="btn btn-action btn-blue-action"
                    OnClick="btnTakeAction_Click" />


                <asp:Button
                    ID="btnCloseEscalation"
                    runat="server"
                    Text="Close Escalation"
                    CssClass="btn btn-action btn-close-action"
                    OnClick="btnCloseEscalation_Click"
                    OnClientClick="return confirm('Are you sure you want to close this escalation?');" />

            </div>

        </div>

    </section>
    </asp:Panel>

    <!-- CLOSED ESCALATION MESSAGE -->
    <asp:Panel
        ID="pnlClosedMessage"
        runat="server"
        CssClass="closed-escalation-message"
        Visible="false">
        This escalation has been closed. No further administrative action is available.
    </asp:Panel>

    <asp:Label
        ID="lblMessage"
        runat="server"
        CssClass="action-message">
    </asp:Label>

    <!-- ESCALATION HISTORY -->

    <section class="detail-panel">

        <div class="panel-header">

            <div>
                <h2>Escalation History</h2>

                <p>
                    Activities recorded during escalation review.
                </p>
            </div>

        </div>


        <div class="timeline-wrapper">

            <asp:Repeater
                ID="rptEscalationTimeline"
                runat="server">

                <ItemTemplate>

                    <div class="history-item">

                        <div class="history-marker">
                            <span></span>
                        </div>

                        <div class="history-content">

                            <strong>
                               <%#: Eval("trackingactivityDescription") %>
                            </strong>

                            <span>
                                <%# Convert.ToDateTime(
                                    Eval("trackingactivitydatetime")
                                ).ToString("dd MMM yyyy, hh:mm tt") %>
                            </span>

                        </div>

                    </div>

                </ItemTemplate>

            </asp:Repeater>


            <asp:Panel
                ID="pnlNoHistory"
                runat="server"
                CssClass="empty-history"
                Visible="false">

                No escalation activity recorded yet.

            </asp:Panel>

        </div>

    </section>


   

</asp:Content>

