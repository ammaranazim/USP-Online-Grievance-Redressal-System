<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true"
    CodeFile="AdminDashboard.aspx.cs"
    Inherits="Admin_AdminDashboard" %>

<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <link href="css/AdminDashboard.css" rel="stylesheet" />
</asp:Content>


<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">


    <!-- =========================================
         DASHBOARD HEADER
         ========================================= -->

    <section class="dashboard-header">

        <div class="dashboard-heading">

            <h1>
                Admin Dashboard
            </h1>

            <p>
                Overview of grievances and system activity
            </p>

        </div>

    </section>


    <!-- =========================================
         KPI CARDS
         Bootstrap Responsive Grid
         ========================================= -->

    <section class="row dashboard-kpi-grid">

        <!-- TOTAL -->

        <div class="col-12 col-sm-6 col-xl-3 mb-3">

            <div class="kpi-card">

                <div class="kpi-icon kpi-icon-blue">
                    ▤
                </div>

                <div class="kpi-content">

                    <span class="kpi-label">
                        Total Grievances
                    </span>

                    <asp:Label
                        ID="lblTotalGrievances"
                        runat="server"
                        CssClass="kpi-value">
                        —
                    </asp:Label>

                </div>

            </div>

        </div>


        <!-- PENDING -->

     <div class="col-12 col-sm-6 col-xl-3 mb-3">

            <div class="kpi-card">

                <div class="kpi-icon kpi-icon-orange">
                    ◷
                </div>

                <div class="kpi-content">

                    <span class="kpi-label">
                        Pending
                    </span>

                    <asp:Label
                        ID="lblPendingGrievances"
                        runat="server"
                        CssClass="kpi-value">
                        —
                    </asp:Label>

                </div>

            </div>

        </div>


        <!-- IN PROCESS -->

        <div class="col-12 col-sm-6 col-xl-3 mb-3">

            <div class="kpi-card">

                <div class="kpi-icon kpi-icon-purple">
                    ⇄
                </div>

                <div class="kpi-content">

                    <span class="kpi-label">
                        In Process
                    </span>

                    <asp:Label
                        ID="lblInProcessGrievances"
                        runat="server"
                        CssClass="kpi-value">
                        —
                    </asp:Label>

                </div>

            </div>

        </div>


        <!-- RESOLVED -->

       <div class="col-12 col-sm-6 col-xl-3 mb-3">

            <div class="kpi-card">

                <div class="kpi-icon kpi-icon-green">
                    ✓
                </div>

                <div class="kpi-content">

                    <span class="kpi-label">
                        Resolved
                    </span>

                    <asp:Label
                        ID="lblResolvedGrievances"
                        runat="server"
                        CssClass="kpi-value">
                        —
                    </asp:Label>

                </div>

            </div>

        </div>

    </section>


    <!-- =========================================
         REQUIRES ATTENTION
         ========================================= -->

    <section class="dashboard-section">

        <div class="section-heading">

            <div>

                <h2>
                    Requires Attention
                </h2>

                <p>
                    Items that may require administrative action
                </p>

            </div>

        </div>


        <!-- Bootstrap Responsive Grid -->

        <div class="row attention-grid">

            
<!-- ESCALATED -->

<div class="col-12 col-md-4 mb-3">

    <asp:HyperLink
        ID="lnkEscalatedGrievances"
        runat="server"
        NavigateUrl="~/Admin/AdminEscalatedGrievances.aspx"
        CssClass="attention-card attention-card-link">

        <div class="attention-icon attention-red">
            ↑
        </div>

        <div class="attention-content">

            <span>
                Escalated Grievances
            </span>

            <asp:Label
                ID="lblEscalatedGrievances"
                runat="server"
                CssClass="attention-value">
                —
            </asp:Label>

        </div>

        <span class="attention-arrow">
            →
        </span>

    </asp:HyperLink>

</div>

</div>

    </section>


    <!-- =========================================
         CHARTS
         Bootstrap Responsive Grid
         ========================================= -->

   <section class="row dashboard-chart-grid">


        <!-- GRIEVANCE TREND -->

       <div class="col-12 col-lg-7 mb-3">

            <div class="dashboard-panel">

                <div class="panel-header">

                    <div>

                        <h2>
                            Grievance Trend
                        </h2>

                        <p>
                            Grievance submission overview
                        </p>

                    </div>

                    <span class="panel-filter">
                        Monthly
                    </span>

                </div>


                <div class="chart-placeholder">

                    <canvas id="grievanceTrendChart"></canvas>

                </div>

            </div>

        </div>


        <!-- CATEGORY -->

       <div class="col-12 col-lg-5 mb-3">

            <div class="dashboard-panel">

                <div class="panel-header">

                    <div>

                        <h2>
                            Grievances by Category
                        </h2>

                        <p>
                            Distribution across categories
                        </p>

                    </div>

                </div>


                <div class="category-placeholder">

                    <div class="category-circle">

                        <canvas id="categoryDonutChart"></canvas>

                    </div>


                    <div class="category-list">

                        <div class="category-item">

                            <span class="category-dot academia">
                            </span>

                            <span>
                                Academia
                            </span>

                            <strong>

                                <asp:Label
                                    ID="lblAcademiaCount"
                                    runat="server">
                                    0
                                </asp:Label>

                            </strong>

                        </div>


                        <div class="category-item">

                            <span class="category-dot accounts">
                            </span>

                            <span>
                                Accounts
                            </span>

                            <strong>

                                <asp:Label
                                    ID="lblAccountsCount"
                                    runat="server">
                                    0
                                </asp:Label>

                            </strong>

                        </div>


                        <div class="category-item">

                            <span class="category-dot exam">
                            </span>

                            <span>
                                Exam &amp; Degree
                            </span>

                            <strong>

                                <asp:Label
                                    ID="lblExamDegreeCount"
                                    runat="server">
                                    0
                                </asp:Label>

                            </strong>

                        </div>

                    </div>

                </div>

            </div>

        </div>

    </section>


    <!-- =========================================
         DEPARTMENT OVERVIEW
         ========================================= -->

    <section class="dashboard-section">

        <div class="section-heading">

            <div>

                <h2>
                    Department Overview
                </h2>

                <p>
                    Grievance status across departments
                </p>

            </div>

            <a href="AdminDepartments.aspx"
               class="section-action">

                View Departments

            </a>

        </div>


        <div class="table-responsive dashboard-table-wrapper">

            <table class="dashboard-table">

                <thead>

                    <tr>

                        <th>
                            Department
                        </th>

                        <th>
                            Total
                        </th>

                        <th>
                            Pending
                        </th>

                        <th>
                            In Process
                        </th>

                        <th>
                            Resolved
                        </th>

                    </tr>

                </thead>


                <tbody>

                    <asp:Repeater
                        ID="rptDepartmentOverview"
                        runat="server">

                        <ItemTemplate>

                            <tr>

                                <td>
                                    <%# Eval("deptName") %>
                                </td>

                                <td>
                                    <%# Eval("TotalGrievances") %>
                                </td>

                                <td>
                                    <%# Eval("PendingGrievances") %>
                                </td>

                                <td>
                                    <%# Eval("InProcessGrievances") %>
                                </td>

                                <td>
                                    <%# Eval("ResolvedGrievances") %>
                                </td>

                            </tr>

                        </ItemTemplate>

                    </asp:Repeater>

                </tbody>

            </table>

        </div>

    </section>


    <!-- =========================================
         RECENT GRIEVANCES
         ========================================= -->

    <section class="dashboard-section">

        <div class="section-heading">

            <div>

                <h2>
                    Recent Grievances
                </h2>

                <p>
                    Latest submitted grievances
                </p>

            </div>

            <a href="AllGrievances.aspx"
               class="section-action">

                View All Grievances

            </a>

        </div>


        <div class="table-responsive dashboard-table-wrapper">

            <table class="dashboard-table recent-table">

                <thead>

                    <tr>

                        <th>
                            Grievance ID
                        </th>

                        <th>
                            Student
                        </th>

                        <th>
                            Title
                        </th>

                        <th>
                            Category
                        </th>

                        <th>
                            Department
                        </th>

                        <th>
                            Status
                        </th>

                    </tr>

                </thead>


                <tbody>

                    <asp:Repeater
                        ID="rptRecentGrievances"
                        runat="server">

                        <ItemTemplate>

                            <tr>

                                <td class="grievance-id">
                                    <%# "GRV-" + Eval("grievanceID") %>
                                </td>

                                <td>
                                    <%# Eval("StudentName") %>
                                </td>

                                <td>
                                    <%# Eval("grievanceTitle") %>
                                </td>

                                <td>
                                    <%# Eval("categoryName") %>
                                </td>

                                <td>
                                    <%# Eval("deptName") %>
                                </td>

                               <td>
    <asp:Label
        ID="lblGrievanceStatus"
        runat="server"
        CssClass='<%# "status-badge " + GetStatusCssClass(Eval("grievanceStatus").ToString()) %>'
        Text='<%# Eval("grievanceStatus") %>'>
    </asp:Label>
</td>

                            </tr>

                        </ItemTemplate>

                    </asp:Repeater>

                </tbody>

            </table>

        </div>

    </section>


    <!-- =========================================
         CATEGORY DONUT CHART
         ========================================= -->

    <script type="text/javascript">

        function loadCategoryDonutChart(academia, accounts, examDegree) {

            var canvas =
                document.getElementById("categoryDonutChart");

            if (!canvas) {
                return;
            }

            new Chart(canvas, {

                type: "doughnut",

                data: {

                    labels: [
                        "Academia",
                        "Accounts",
                        "Exam & Degree"
                    ],

                    datasets: [{

                        data: [
                            academia,
                            accounts,
                            examDegree
                        ],

                        backgroundColor: [
                            "#2f6fed",
                            "#f59e0b",
                            "#22c55e"
                        ],

                        borderWidth: 0

                    }]

                },

                options: {

                    responsive: true,
                    maintainAspectRatio: false,

                    cutout: "68%",

                    plugins: {

                        legend: {
                            display: false
                        },

                        tooltip: {
                            enabled: true
                        }

                    }

                }

            });

        }

    </script>


    <!-- =========================================
         GRIEVANCE TREND CHART
         ========================================= -->

    <script type="text/javascript">

        function loadGrievanceTrendChart(labels, values) {

            var canvas =
                document.getElementById("grievanceTrendChart");

            if (!canvas) {
                return;
            }

            new Chart(canvas, {

                type: "bar",

                data: {

                    labels: labels,

                    datasets: [{

                        label: "Grievances",

                        data: values,

                        backgroundColor: "#2f6fed",

                        borderRadius: 5,

                        barThickness: 32

                    }]

                },

                options: {

                    responsive: true,

                    maintainAspectRatio: false,

                    plugins: {

                        legend: {
                            display: false
                        },

                        tooltip: {
                            enabled: true
                        }

                    },

                    scales: {

                        x: {

                            grid: {
                                display: false
                            },

                            ticks: {

                                color: "#667085",

                                font: {
                                    family: "Inter",
                                    size: 11
                                }

                            }

                        },

                        y: {

                            beginAtZero: true,

                            ticks: {

                                precision: 0,

                                color: "#667085",

                                font: {
                                    family: "Inter",
                                    size: 11
                                }

                            },

                            grid: {

                                color: "#f1f3f6",

                                lineWidth: 1

                            }

                        }

                    }

                }

            });

        }

    </script>


</asp:Content>