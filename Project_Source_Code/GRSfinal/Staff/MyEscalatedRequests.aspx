
<%@ Page Title="My Escalated Requests"
    Language="C#"
    MasterPageFile="~/Staff/StaffMaster.master"
    AutoEventWireup="true"
    CodeFile="MyEscalatedRequests.aspx.cs"
    Inherits="Staff_MyEscalatedRequests" %>

<asp:Content ID="HeadContent1"
    ContentPlaceHolderID="HeadContent"
    runat="server">

    <link href="<%= ResolveUrl("~/Staff/css/MyEscalatedRequests.css") %>"
        rel="stylesheet" />

</asp:Content>

<asp:Content ID="MainContent1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="escalated-page">

        <!-- Page Heading -->
        <div class="escalated-page-header">
            <div>
                <h1>My Escalated Requests</h1>
                <p>Review escalation requests related to your assigned Grievances.</p>
            </div>
        </div>

        <!-- Search and Filters -->
        <section class="escalated-toolbar">

            <div class="escalated-toolbar-main">

                <div class="escalated-search">
                    <i class="fa-solid fa-magnifying-glass"
                        aria-hidden="true"></i>

                    <asp:TextBox ID="txtSearch"
                        runat="server"
                        CssClass="escalated-search-input"
                        placeholder="Search by ID, title, category or reason"
                        MaxLength="200" />
                </div>

                <asp:Button ID="btnSearch"
                    runat="server"
                    Text="Search"
                    CssClass="escalated-btn escalated-btn-primary"
                    OnClick="btnSearch_Click" />

                <asp:Button ID="btnClear"
                    runat="server"
                    Text="Clear"
                    CssClass="escalated-btn escalated-btn-clear"
                    CausesValidation="false"
                    OnClick="btnClear_Click" />

                <button type="button"
                    id="btnToggleFilters"
                    class="escalated-btn escalated-btn-filter"
                    aria-expanded="false"
                    aria-controls="advancedFilters"
                    onclick="toggleEscalatedFilters()">

                    <i class="fa-solid fa-sliders"
                        aria-hidden="true"></i>
                    Filters
                </button>

            </div>

            <div id="advancedFilters"
                class="escalated-advanced-filters"
                hidden>

                <div class="escalated-filter-field">
                    <label for="<%= ddlRequestStatus.ClientID %>">
                        Request Status
                    </label>

                    <asp:DropDownList ID="ddlRequestStatus"
                        runat="server"
                        CssClass="escalated-filter-select">

                        <asp:ListItem Text="All Statuses" Value="" />
                        <asp:ListItem Text="Pending HOD Review"
                            Value="Pending HOD Review" />
                        <asp:ListItem Text="Pending Admin Review"
                            Value="Pending Admin Review" />
                        <asp:ListItem Text="Action Required"
                            Value="Action Required" />
                        <asp:ListItem Text="Action Taken"
                            Value="Action Taken" />
                        <asp:ListItem Text="Closed"
                            Value="Closed" />

                    </asp:DropDownList>
                </div>

            </div>

        </section>

        <!-- Escalated Requests Table -->
        <section class="escalated-table-panel">

            <div class="escalated-table-heading">
                <div>
                    <h2>Escalation Requests</h2>
                    <p>Requests associated with your current assignments.</p>
                </div>

                <span class="escalated-record-count">
                    <asp:Label ID="lblRecordCount"
                        runat="server"
                        Text="0 records" />
                </span>
            </div>

            <div class="table-responsive escalated-table-wrap">

                <asp:GridView ID="gvEscalatedRequests"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table escalated-requests-table"
                    GridLines="None"
                    Width="100%"
                    EmptyDataText="No escalated requests match your search."
                    UseAccessibleHeader="true">

                    <Columns>

                        <asp:TemplateField HeaderText="Escalation ID">
                            <ItemTemplate>
                                <span class="escalated-id">
                                    <%# Eval("escalationID", "ESC-{0}") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Grievance ID">
                            <ItemTemplate>
                                <span class="grievance-id">
                                    <%# Eval("grievanceID", "GRV-{0}") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField
                            DataField="grievanceTitle"
                            HeaderText="Grievance Title"
                            NullDisplayText="—" />

                        <asp:BoundField
                            DataField="categoryName"
                            HeaderText="Category"
                            NullDisplayText="—" />

                        <asp:BoundField
                            DataField="escalationReason"
                            HeaderText="Reason"
                            NullDisplayText="—" />

                       <asp:TemplateField HeaderText="Request Status">
    <ItemTemplate>
        <span class='escalation-status <%# GetEscalationStatusClass(Convert.ToString(Eval("requestStatus"))) %>'>
            <%# Server.HtmlEncode(Convert.ToString(Eval("requestStatus"))) %>
        </span>
    </ItemTemplate>
</asp:TemplateField>

                        <asp:BoundField
                            DataField="requestedDate"
                            HeaderText="Requested Date"
                            DataFormatString="{0:dd MMM yyyy}"
                            HtmlEncode="false" />

                     <asp:TemplateField HeaderText="Action">
    <ItemTemplate>
        <asp:HyperLink
            ID="lnkViewDetails"
            runat="server"
            NavigateUrl='<%# Eval("grievanceID", "~/Staff/StaffGrievanceDetails.aspx?grievanceID={0}") %>'
            CssClass="view-details-btn"
            Text="View Details" />
    </ItemTemplate>
</asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

        </section>

    </div>

    <script type="text/javascript">
        function toggleEscalatedFilters() {
            var panel = document.getElementById("advancedFilters");
            var button = document.getElementById("btnToggleFilters");

            if (!panel || !button) {
                return;
            }

            var isHidden = panel.hidden;
            panel.hidden = !isHidden;

            button.setAttribute(
                "aria-expanded",
                isHidden ? "true" : "false"
            );

            button.classList.toggle("filters-active", isHidden);
        }
    </script>

</asp:Content>
