
<%@ Page Title="Staff Dashboard" Language="C#" MasterPageFile="~/Staff/StaffMaster.master" AutoEventWireup="true" CodeFile="StaffDashboard.aspx.cs" Inherits="Staff_StaffDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="<%= ResolveUrl("~/Staff/css/StaffDashboard.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <div class="staff-dashboard">

        <!-- Dashboard Heading -->
        <div class="dashboard-header">
            <div>
                <h1>Staff Dashboard</h1>
                <p>View your assigned Grievances and monitor their progress.</p>
            </div>
        </div>

        <!-- KPI Cards -->
        <div class="dashboard-kpi-grid">

            <div class="dashboard-kpi-card kpi-total">
                <div class="kpi-icon">
                    <i class="fa-solid fa-folder-open"></i>
                </div>
                <div class="kpi-content">
                    <span class="kpi-label">Total Assigned</span>
                    <asp:Label ID="lblTotalAssigned" runat="server"
                        CssClass="kpi-value" Text="0" />
                </div>
            </div>

            <div class="dashboard-kpi-card kpi-pending">
                <div class="kpi-icon">
                    <i class="fa-regular fa-clock"></i>
                </div>
                <div class="kpi-content">
                    <span class="kpi-label">Pending</span>
                    <asp:Label ID="lblPending" runat="server"
                        CssClass="kpi-value" Text="0" />
                </div>
            </div>

            <div class="dashboard-kpi-card kpi-progress">
                <div class="kpi-icon">
                    <i class="fa-solid fa-spinner"></i>
                </div>
                <div class="kpi-content">
                    <span class="kpi-label">In Process</span>
                    <asp:Label ID="lblInProcess" runat="server"
                        CssClass="kpi-value" Text="0" />
                </div>
            </div>

            <div class="dashboard-kpi-card kpi-resolved">
                <div class="kpi-icon">
                    <i class="fa-solid fa-circle-check"></i>
                </div>
                <div class="kpi-content">
                    <span class="kpi-label">Resolved</span>
                    <asp:Label ID="lblResolved" runat="server"
                        CssClass="kpi-value" Text="0" />
                </div>
            </div>

        </div>

        <!-- Assigned Grievances -->
        <section class="dashboard-panel">

            <div class="panel-header">
                <div>
                    <h2>My Assigned Grievances</h2>
                    <p>Grievances currently assigned to you.</p>
                </div>

                <asp:HyperLink ID="lnkViewAll"
                    runat="server"
                    NavigateUrl="~/Staff/StaffAssignedGrievances.aspx"
                    CssClass="view-all-link">
                    View All <i class="fa-solid fa-arrow-right"></i>
                </asp:HyperLink>
            </div>

            <div class="table-responsive dashboard-table-wrap">

                <asp:GridView ID="gvAssignedGrievances"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table staff-dashboard-table"
                    GridLines="None"
                    Width="100%"
                    EmptyDataText="No Grievances are currently assigned to you."
                    UseAccessibleHeader="true">

                    <Columns>

                       <asp:TemplateField HeaderText="Grievance ID">
                            <ItemTemplate>
                                <span class="grievance-id">
                                    <%# Eval("grievanceID", "GRV-{0}") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField
                            DataField="grievanceTitle"
                            HeaderText="Grievance Title" />

                        <asp:BoundField
                            DataField="categoryName"
                            HeaderText="Category" />

                        <asp:TemplateField HeaderText="Priority">
    <ItemTemplate>
        <span class='<%# "priority-badge priority-" + Convert.ToString(Eval("grievancePriority")).Trim().ToLowerInvariant() %>'>
            <%#: Eval("grievancePriority") %>
        </span>
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Status">
    <ItemTemplate>
        <span class='<%# "status-badge status-" + Convert.ToString(Eval("grievanceStatus")).Trim().ToLowerInvariant().Replace(" ", "-") %>'>
            <%#: Eval("grievanceStatus") %>
        </span>
    </ItemTemplate>
</asp:TemplateField>

                        <asp:BoundField
                            DataField="assignedDate"
                            HeaderText="Assigned Date"
                            DataFormatString="{0:dd MMM yyyy}"
                            HtmlEncode="false" />

                        <asp:BoundField
                            DataField="grievanceDueDate"
                            HeaderText="Due Date"
                            DataFormatString="{0:dd MMM yyyy}"
                            HtmlEncode="false"
                            NullDisplayText="—" />

                    </Columns>

                </asp:GridView>

            </div>
        </section>

    </div>

</asp:Content>
