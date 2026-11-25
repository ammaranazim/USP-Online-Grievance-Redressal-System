<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminEscalatedGrievances.aspx.cs" Inherits="Admin_AdminEscalatedGrievances" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Admin/css/AdminEscalatedGrievances.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Page Header -->
    <div class="page-header">
        <div>
            <h1>Escalated Grievances</h1>
            <p>Review and take action on grievances escalated to the administration.</p>
        </div>
    </div>

    <!-- Escalation KPI Cards -->
    <section class="row dashboard-kpi-grid">

        <!-- Total Escalated -->
        <div class="col-12 col-sm-6 col-xl-4 mb-3">
            <asp:LinkButton
                ID="lnkTotalEscalated"
                runat="server"
                CssClass="kpi-card kpi-card-link"
                CommandName="FilterKPI"
                CommandArgument="Total"
                OnCommand="KpiCard_Command">

                <div class="kpi-icon kpi-icon-blue">↑</div>

                <div class="kpi-content">
                    <span class="kpi-label">Total Escalated</span>

                    <asp:Label
                        ID="lblTotalEscalated"
                        runat="server"
                        CssClass="kpi-value">0</asp:Label>
                </div>

            </asp:LinkButton>
        </div>

        <!-- Pending Review -->
        <div class="col-12 col-sm-6 col-xl-4 mb-3">
            <asp:LinkButton
                ID="lnkPendingReview"
                runat="server"
                CssClass="kpi-card kpi-card-link"
                CommandName="FilterKPI"
                CommandArgument="Pending"
                OnCommand="KpiCard_Command">

                <div class="kpi-icon kpi-icon-orange">◷</div>

                <div class="kpi-content">
                    <span class="kpi-label">Pending Review</span>

                    <asp:Label
                        ID="lblPendingReview"
                        runat="server"
                        CssClass="kpi-value">0</asp:Label>
                </div>

            </asp:LinkButton>
        </div>

        <!-- Action Required -->
        <div class="col-12 col-sm-6 col-xl-4 mb-3">
            <asp:LinkButton
                ID="lnkActionRequired"
                runat="server"
                CssClass="kpi-card kpi-card-link"
                CommandName="FilterKPI"
                CommandArgument="ActionRequired"
                OnCommand="KpiCard_Command">

                <div class="kpi-icon kpi-icon-red">!</div>

                <div class="kpi-content">
                    <span class="kpi-label">Action Required</span>

                    <asp:Label
                        ID="lblActionRequired"
                        runat="server"
                        CssClass="kpi-value">0</asp:Label>
                </div>

            </asp:LinkButton>
        </div>

    </section>

    <!-- Escalation Requests -->
    <section class="escalation-panel">

        <div class="panel-header">
            <div>
                <h2>Escalation Requests</h2>
                <p>Grievances requiring higher-level review or administrative action.</p>
            </div>
        </div>

        <!-- Search and Filter Toolbar -->
        <div class="escalation-toolbar">

            <div class="escalation-search-row">

                <div class="escalation-search-field">
                    <asp:TextBox
                        ID="txtEscalationSearch"
                        runat="server"
                        CssClass="escalation-search-input"
                        MaxLength="100"
                        placeholder="Search escalation ID, grievance ID, student name or registration no." />
                </div>

                <asp:Button
                    ID="btnEscalationSearch"
                    runat="server"
                    Text="Search"
                    CssClass="escalation-search-btn"
                    CausesValidation="false"
                    OnClick="btnEscalationSearch_Click" />

                <asp:Button
                    ID="btnEscalationClear"
                    runat="server"
                    Text="Clear"
                    CssClass="escalation-clear-btn"
                    CausesValidation="false"
                    OnClick="btnEscalationClear_Click" />

                <asp:Button
                    ID="btnToggleEscalationFilters"
                    runat="server"
                    Text="Filters"
                    CssClass="escalation-filter-toggle"
                    CausesValidation="false"
                    UseSubmitBehavior="false"
                    OnClientClick="toggleEscalationFilters(); return false;" />

            </div>

            <!-- Advanced Filters -->
            <div id="escalationAdvancedFilters" class="escalation-advanced-filters">

                <div class="escalation-filter-field">

                    <asp:Label
                        ID="lblEscalationStatusFilter"
                        runat="server"
                        AssociatedControlID="ddlEscalationStatusFilter">
                        Status
                    </asp:Label>

                    <asp:DropDownList
                        ID="ddlEscalationStatusFilter"
                        runat="server"
                        CssClass="escalation-filter-control">

                        <asp:ListItem Text="All Statuses" Value="" />
                        <asp:ListItem Text="Pending Admin Review" Value="Pending Admin Review" />
                        <asp:ListItem Text="Action Required" Value="Action Required" />
                        <asp:ListItem Text="Action Taken" Value="Action Taken" />
                        <asp:ListItem Text="Closed" Value="Closed" />

                    </asp:DropDownList>

                </div>

            </div>

        </div>

        <!-- Escalated Grievances Table -->
        <div class="table-responsive">

            <asp:GridView
                ID="gvEscalatedGrievances"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="admin-data-table"
                GridLines="None"
                AllowPaging="true"
                PageSize="10"
                EmptyDataText="No escalated grievances found."
                OnPageIndexChanging="gvEscalatedGrievances_PageIndexChanging"
                OnRowCommand="gvEscalatedGrievances_RowCommand">

                <Columns>

                    <asp:TemplateField HeaderText="Escalation ID">
                        <ItemTemplate>
                            <span class="record-id">
                                <%# "ESC-" + Eval("escalationID") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Grievance ID">
                        <ItemTemplate>
                            <span class="record-id">
                                <%# "GRV-" + Eval("grievanceID") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Student">
                        <ItemTemplate>
                            <div class="student-cell">
                                <strong><%# Eval("StudentName") %></strong>
                                <span><%# Eval("stdRegNo") %></span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Reason">
                        <ItemTemplate>
                            <span
                                class="reason-text"
                                title='<%# Eval("escalationReason") %>'>
                                <%# Eval("escalationReason") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='<%# GetEscalationStatusClass(Eval("requestStatus")) %>'>
                                <%# Eval("requestStatus") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton
                                ID="btnViewEscalation"
                                runat="server"
                                CommandName="ViewEscalation"
                                CommandArgument='<%# Eval("escalationID") %>'
                                CssClass="view-action">
                                View
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

        </div>

    </section>

    <!-- Filter Toggle Script -->
    <script type="text/javascript">
        window.toggleEscalationFilters = function () {
            var panel = document.getElementById("escalationAdvancedFilters");

            if (!panel) {
                console.error("Escalation filter panel was not found.");
                return;
            }

            panel.classList.toggle("is-open");
        };
    </script>

</asp:Content>