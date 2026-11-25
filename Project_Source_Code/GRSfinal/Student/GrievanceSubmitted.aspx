<%@ Page Title="" Language="C#" MasterPageFile="~/Student/StudentMaster.master" AutoEventWireup="true" CodeFile="GrievanceSubmitted.aspx.cs" Inherits="Student_GrievanceSubmitted" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="<%= ResolveUrl("~/Student/css/GrievanceSubmitted.css") %>"
        rel="stylesheet"
        type="text/css" />
</asp:Content>
<asp:Content ID="ContentMain"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="submission-page">

        <div class="success-card">

            <div class="success-icon">
                ✓
            </div>

            <h2>Grievance Submitted Successfully!</h2>

            <p class="success-message">
                Your grievance has been submitted successfully
                and is being reviewed by the concerned authority.
            </p>

            <div class="reference-box">

                <span>Reference Number</span>

                <strong>
                    <asp:Label
                        ID="lblReferenceNo"
                        runat="server">
                    </asp:Label>
                </strong>

            </div>

            <p class="tracking-info">
                Please keep this reference number for future tracking.
            </p>

            <div class="success-actions">

                <asp:Button
                    ID="btnGoToGrievances"
                    runat="server"
                    Text="Go to My Grievances"
                    CssClass="btn btn-primary"
                    CausesValidation="false"
                    OnClick="btnGoToGrievances_Click" />

                <asp:Button
                    ID="btnBackToDashboard"
                    runat="server"
                    Text="Back to Dashboard"
                    CssClass="btn btn-light"
                    CausesValidation="false"
                    OnClick="btnBackToDashboard_Click" />

            </div>

        </div>

    </div>

</asp:Content>


