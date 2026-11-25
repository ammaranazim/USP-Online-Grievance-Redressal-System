
<%@ Page Title="Dashboard"
    Language="C#"
    MasterPageFile="~/DepartmentHead/HODMaster.master"
    AutoEventWireup="true"
    CodeFile="HODDashboard.aspx.cs"
    Inherits="DepartmentHead_HODDashboard" %>

<asp:Content ID="DashboardHeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

    <link rel="stylesheet"
        href="<%= ResolveUrl("~/DepartmentHead/css/HODDashboard.css") %>?v=2" />

</asp:Content>

<asp:Content ID="DashboardMainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="hod-dashboard">

        <!-- Page Heading -->
        <div class="hod-page-heading">
            <div>
                <h2>Dashboard Overview</h2>
                <p>Monitor grievances and activity within your department.</p>
            </div>

            <div class="hod-department-badge">
                <span class="department-indicator"></span>
                <asp:Label ID="lblDepartmentName"
                    runat="server"
                    Text="Department" />
            </div>
        </div>

        <!-- KPI Cards -->
        <div class="hod-kpi-grid">

            <div class="hod-kpi-card">
                <div class="hod-kpi-top">
                    <span class="hod-kpi-label">Total Grievances</span>
                    <span class="hod-kpi-icon total-icon">&#9776;</span>
                </div>

                <asp:Label ID="lblTotalGrievances"
                    runat="server"
                    CssClass="hod-kpi-value"
                    Text="0" />
            </div>

            <div class="hod-kpi-card">
                <div class="hod-kpi-top">
                    <span class="hod-kpi-label">Pending</span>
                    <span class="hod-kpi-icon pending-icon">&#9203;</span>
                </div>

                <asp:Label ID="lblPendingGrievances"
                    runat="server"
                    CssClass="hod-kpi-value"
                    Text="0" />
            </div>

            <div class="hod-kpi-card">
                <div class="hod-kpi-top">
                    <span class="hod-kpi-label">In Process</span>
                    <span class="hod-kpi-icon progress-icon">&#8635;</span>
                </div>

                <asp:Label ID="lblInProcessGrievances"
                    runat="server"
                    CssClass="hod-kpi-value"
                    Text="0" />
            </div>

            <div class="hod-kpi-card">
                <div class="hod-kpi-top">
                    <span class="hod-kpi-label">Resolved</span>
                    <span class="hod-kpi-icon resolved-icon">&#10003;</span>
                </div>

                <asp:Label ID="lblResolvedGrievances"
                    runat="server"
                    CssClass="hod-kpi-value"
                    Text="0" />
            </div>

        </div>

        <!-- Dashboard Insights -->
        <div class="hod-dashboard-grid">

            <!-- Requires Attention -->
            <section class="hod-dashboard-panel hod-attention-panel">

                <div class="hod-panel-header">
                    <div>
                        <h3>Requires Attention</h3>
                        <p>Departmental work requiring action</p>
                    </div>
                </div>

                <div class="hod-attention-content">

                    <!-- Clickable Overdue Grievances Item -->
                    <asp:HyperLink
                        ID="lnkOverdueGrievances"
                        runat="server"
                        NavigateUrl="~/DepartmentHead/HODProgressResolutionReview.aspx?dueDate=Overdue"
                        CssClass="hod-attention-item hod-attention-clickable">

                        <span class="hod-attention-icon overdue-icon">&#9200;</span>

                        <span class="hod-attention-text">
                            <strong>Overdue Grievances</strong>
                            <span>Grievances that have passed their deadline.</span>
                        </span>

                        <asp:Label ID="lblOverdueCount"
                            runat="server"
                            CssClass="hod-attention-count"
                            Text="—" />

                        <span class="hod-attention-link" aria-hidden="true">&#8250;</span>

                    </asp:HyperLink>

                    <!-- Clickable Unassigned Grievances Item -->
                    <asp:HyperLink
                        ID="lnkUnassignedGrievances"
                        runat="server"
                        NavigateUrl="~/DepartmentHead/HODStaffAllAssignment.aspx?assignment=Unassigned"
                        CssClass="hod-attention-item hod-attention-clickable">

                        <span class="hod-attention-icon unassigned-icon">&#128100;</span>

                        <span class="hod-attention-text">
                            <strong>Unassigned Grievances</strong>
                            <span>Grievances awaiting staff assignment.</span>
                        </span>

                        <asp:Label ID="lblUnassignedCount"
                            runat="server"
                            CssClass="hod-attention-count"
                            Text="—" />

                        <span class="hod-attention-link" aria-hidden="true">&#8250;</span>

                    </asp:HyperLink>

                   
                   
<!-- Clickable Pending Resolution Reviews -->
<asp:HyperLink
    ID="lnkResolutionReview"
    runat="server"
    NavigateUrl="~/DepartmentHead/HODProgressResolutionReview.aspx?stage=Resolution%20Under%20Review"
    CssClass="hod-attention-item hod-attention-clickable">

    <span class="hod-attention-icon review-icon">&#10003;</span>

    <span class="hod-attention-text">
        <strong>Pending Resolution Reviews</strong>
        <span>Resolutions awaiting departmental review.</span>
    </span>

    <asp:Label ID="lblResolutionReviewCount"
        runat="server"
        CssClass="hod-attention-count"
        Text="—" />

    <span class="hod-attention-link" aria-hidden="true">&#8250;</span>

</asp:HyperLink>


                </div>
            </section>

            <!-- Grievance Trend -->
            <section class="hod-dashboard-panel hod-trend-panel">

                <div class="hod-panel-header">
                    <div>
                        <h3>Grievance Trend</h3>
                        <p>Monthly submissions for your department</p>
                    </div>

                    <span class="hod-trend-period">Last 6 Months</span>
                </div>

                <div class="hod-trend-chart">
                    <canvas id="hodGrievanceTrendChart"
                        role="img"
                        aria-label="Bar chart showing monthly grievance submissions">
                    </canvas>

                    <asp:Label ID="lblTrendMessage"
                        runat="server"
                        CssClass="hod-trend-message"
                        Text=""
                        Visible="false" />
                </div>

            </section>

        </div>

        <!-- Recent Grievances -->
        <section class="hod-dashboard-panel hod-recent-panel hod-recent-full">

            <div class="hod-panel-header">
                <div>
                    <h3>Recent Grievances</h3>
                    <p>Latest grievances from your department</p>
                </div>

                <asp:HyperLink ID="lnkViewGrievances"
                    runat="server"
                    NavigateUrl="~/DepartmentHead/HODGrievances.aspx"
                    CssClass="hod-view-link">
                    View All
                </asp:HyperLink>
            </div>

            <div class="hod-table-responsive">

                <asp:GridView ID="gvRecentGrievances"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="hod-dashboard-table"
                    GridLines="None"
                    EmptyDataText="No grievances found for your department."
                    UseAccessibleHeader="true">

                    <Columns>

                        <asp:TemplateField HeaderText="Grievance ID">
                            <ItemTemplate>
                                <span class="hod-grievance-id">
                                    GRV-<%# Eval("grievanceID") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="studentName"
                            HeaderText="Student Name" />

                        <asp:BoundField DataField="grievanceTitle"
                            HeaderText="Title" />

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class='<%# GetStatusBadgeClass(Eval("grievanceStatus")) %>'>
                                    <%# Eval("grievanceStatus") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="submissionDate"
                            HeaderText="Submitted"
                            DataFormatString="{0:dd MMM yyyy}" />

                    </Columns>

                </asp:GridView>

            </div>
        </section>

        <!-- Chart.js -->
        <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

        <script type="text/javascript">
            function loadHODGrievanceTrend(labels, values) {
                var canvas = document.getElementById("hodGrievanceTrendChart");

                if (!canvas || typeof Chart === "undefined") {
                    return;
                }

                if (window.hodGrievanceTrendInstance) {
                    window.hodGrievanceTrendInstance.destroy();
                }

                window.hodGrievanceTrendInstance = new Chart(canvas, {
                    type: "bar",
                    data: {
                        labels: labels,
                        datasets: [{
                            label: "Grievances",
                            data: values,
                            backgroundColor: "#2f6fed",
                            borderRadius: 5,
                            maxBarThickness: 38
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            legend: { display: false }
                        },
                        scales: {
                            x: {
                                grid: { display: false },
                                ticks: {
                                    color: "#667085",
                                    font: { family: "Inter", size: 11 }
                                }
                            },
                            y: {
                                beginAtZero: true,
                                ticks: {
                                    precision: 0,
                                    color: "#667085",
                                    font: { family: "Inter", size: 11 }
                                },
                                grid: { color: "#f1f3f6" }
                            }
                        }
                    }
                });
            }
        </script>

    </div>

</asp:Content>
