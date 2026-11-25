
<%@ Page Title="Admin Reports" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminReports.aspx.cs" Inherits="Admin_AdminReports" %>

<asp:Content ID="HeadContent1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Admin/css/AdminReports.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="admin-reports-page">

        <!-- Page Header -->
        <div class="admin-reports-header">
            <div>
                <h2>Reports</h2>
                <p>Generate and download Grievance reports.</p>
            </div>
        </div>

        <!-- Report Configuration -->
        <section class="admin-report-panel">

            <!-- Available Report -->
            <div class="admin-report-section">
                <h3 class="admin-report-section-title">Report Type</h3>

                <div class="admin-report-types">
                    <div class="admin-report-type-option">
                        <span class="admin-report-type-content">
                            <span class="admin-report-type-title">Detailed Report</span>
                            <span class="admin-report-type-description">
                                View individual Grievance records with student,
                                category, department, status and date details.
                            </span>
                        </span>
                    </div>
                </div>
            </div>

            <!-- Filters -->
            <div class="admin-report-section admin-report-filters-section">

                <h3 class="admin-report-section-title">Filters</h3>

                <div class="row">

                    <div class="col-lg-6 col-md-6 form-group">
                        <asp:Label ID="lblFromDate" runat="server"
                            AssociatedControlID="txtFromDate"
                            CssClass="admin-report-label"
                            Text="From Date" />

                        <asp:TextBox ID="txtFromDate" runat="server"
                            CssClass="form-control admin-report-input"
                            TextMode="Date" />
                    </div>

                    <div class="col-lg-6 col-md-6 form-group">
                        <asp:Label ID="lblToDate" runat="server"
                            AssociatedControlID="txtToDate"
                            CssClass="admin-report-label"
                            Text="To Date" />

                        <asp:TextBox ID="txtToDate" runat="server"
                            CssClass="form-control admin-report-input"
                            TextMode="Date" />
                    </div>

                    <div class="col-lg-6 col-md-6 form-group">
                        <asp:Label ID="lblStatus" runat="server"
                            AssociatedControlID="ddlStatus"
                            CssClass="admin-report-label"
                            Text="Status" />

                        <asp:DropDownList ID="ddlStatus" runat="server"
                            CssClass="form-control admin-report-input">
                            <asp:ListItem Text="All Statuses" Value="" />
                            <asp:ListItem Text="Pending" Value="Pending" />
                            <asp:ListItem Text="In Process" Value="In Process" />
                            <asp:ListItem Text="Resolved" Value="Resolved" />
                            <asp:ListItem Text="Rejected" Value="Rejected" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-lg-6 col-md-6 form-group">
                        <asp:Label ID="lblCategory" runat="server"
                            AssociatedControlID="ddlCategory"
                            CssClass="admin-report-label"
                            Text="Category" />

                        <asp:DropDownList ID="ddlCategory" runat="server"
                            CssClass="form-control admin-report-input"
                            AppendDataBoundItems="true">
                            <asp:ListItem Text="All Categories" Value="" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-lg-6 col-md-6 form-group admin-report-department-filter">
                        <asp:Label ID="lblDepartment" runat="server"
                            AssociatedControlID="ddlDepartment"
                            CssClass="admin-report-label"
                            Text="Department" />

                        <asp:DropDownList ID="ddlDepartment" runat="server"
                            CssClass="form-control admin-report-input"
                            AppendDataBoundItems="true">
                            <asp:ListItem Text="All Departments" Value="" />
                        </asp:DropDownList>
                    </div>

                </div>

                <div class="admin-report-actions">
                    <asp:Button ID="btnGenerateReport" runat="server"
                        Text="Generate Report"
                        CssClass="btn admin-report-generate-btn"
                        CausesValidation="false"
                        OnClick="btnGenerateReport_Click" />

                    <asp:Button ID="btnResetFilters" runat="server"
                        Text="Reset"
                        CssClass="btn admin-report-reset-btn"
                        CausesValidation="false"
                        OnClick="btnResetFilters_Click" />
                </div>

            </div>

        </section>

        <!-- Report Results -->
        <section class="admin-report-panel admin-report-results-panel">

            <div class="admin-report-results-header">
                <div>
                    <h3 class="admin-report-section-title">Report Results</h3>
                    <p class="admin-report-results-description">
                        Generated report data will appear here.
                    </p>
                </div>

                <div class="admin-report-export-actions">
                    <asp:Button ID="btnExportExcel" runat="server"
                        Text="Export to Excel"
                        CssClass="btn admin-report-export-btn admin-report-excel-btn"
                        CausesValidation="false"
                        Enabled="false"
                        OnClick="btnExportExcel_Click" />

                  
                </div>
            </div>

            <asp:Label ID="lblReportMessage" runat="server"
                CssClass="admin-report-message"
                EnableViewState="false"
                role="alert" />

            <asp:Panel ID="pnlReportEmpty" runat="server"
                CssClass="admin-report-empty">

                <div class="admin-report-empty-icon">
                    <i class="fa-regular fa-file-lines" aria-hidden="true"></i>
                </div>

                <h4>No report generated yet</h4>

                <p>
                    Set your filters and click Generate Report to view
                    the Detailed Grievance Report.
                </p>

            </asp:Panel>

            <div class="admin-report-results-table-wrapper">
                <asp:GridView ID="gvReportResults" runat="server"
                    CssClass="table admin-report-table"
                    AutoGenerateColumns="true"
                    GridLines="None"
                    EmptyDataText="No records found for the selected filters."
                    Visible="false">
                </asp:GridView>
            </div>

        </section>

    </div>

</asp:Content>
