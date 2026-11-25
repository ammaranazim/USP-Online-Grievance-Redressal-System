<%@ Page Title="Manage Departments" Language="C#" MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true" CodeFile="AdminDepartments.aspx.cs"
    Inherits="Admin_AdminDepartments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="css/AdminDepartments.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <div class="admin-departments-page">

        <!-- Page Header -->
        <div class="departments-page-header">
            <div class="departments-header-content">
                <h1>Departments</h1>
                <p>Manage university departments and their assigned categories.</p>
            </div>

            <asp:HyperLink
                ID="lnkAddDepartment"
                runat="server"
                NavigateUrl="~/Admin/AdminRegisterDepartment.aspx"
                CssClass="departments-add-btn">
                <span class="departments-add-icon">+</span>
                Add Department
            </asp:HyperLink>
        </div>

        <!-- KPI Cards -->
        <div class="departments-kpi-grid">

            <asp:LinkButton
                ID="lnkTotalDepartments"
                runat="server"
                CssClass="departments-kpi-card"
                CommandArgument="all"
                OnCommand="KpiCard_Click"
                CausesValidation="false">
                <span class="departments-kpi-label">Total Departments</span>
                <asp:Label ID="lblTotalDepartments" runat="server"
                    CssClass="departments-kpi-value" Text="0" />
                <span class="departments-kpi-note">All departments</span>
            </asp:LinkButton>

            <asp:LinkButton
                ID="lnkActiveDepartments"
                runat="server"
                CssClass="departments-kpi-card"
                CommandArgument="active"
                OnCommand="KpiCard_Click"
                CausesValidation="false">
                <span class="departments-kpi-label">Active Departments</span>
                <asp:Label ID="lblActiveDepartments" runat="server"
                    CssClass="departments-kpi-value" Text="0" />
                <span class="departments-kpi-note">Currently active</span>
            </asp:LinkButton>

            <asp:LinkButton
                ID="lnkInactiveDepartments"
                runat="server"
                CssClass="departments-kpi-card"
                CommandArgument="inactive"
                OnCommand="KpiCard_Click"
                CausesValidation="false">
                <span class="departments-kpi-label">Inactive Departments</span>
                <asp:Label ID="lblInactiveDepartments" runat="server"
                    CssClass="departments-kpi-value" Text="0" />
                <span class="departments-kpi-note">Currently inactive</span>
            </asp:LinkButton>

        </div>

        <!-- Departments Table Card -->
        <div class="departments-table-card">

            <div class="departments-table-heading">
                <div>
                    <h2>All Departments</h2>
                    <p>View and manage department information.</p>
                </div>

                <asp:Label ID="lblDepartmentCount" runat="server"
                    CssClass="departments-record-count" Text="0 records" />
            </div>

            <!-- Search Toolbar -->
            <div class="departments-toolbar">

                <div class="departments-search-group">
                    <asp:TextBox
                        ID="txtDepartmentSearch"
                        runat="server"
                        CssClass="departments-search-input"
                        placeholder="Search by department name or code..."
                        MaxLength="100" />

                    <asp:Button
                        ID="btnDepartmentSearch"
                        runat="server"
                        Text="Search"
                        CssClass="departments-search-btn"
                        OnClick="btnDepartmentSearch_Click"
                        CausesValidation="false" />
                </div>

                <div class="departments-toolbar-actions">

                    <asp:Button
                        ID="btnDepartmentClear"
                        runat="server"
                        Text="Clear"
                        CssClass="departments-clear-btn"
                        OnClick="btnDepartmentClear_Click"
                        CausesValidation="false" />

                    <button type="button"
                        id="btnDepartmentFilters"
                        class="departments-filter-toggle"
                        aria-expanded="false"
                        aria-controls="departmentFiltersPanel">
                        Filters
                        <span class="departments-filter-chevron">&#9662;</span>
                    </button>

                </div>
            </div>

            <!-- Collapsible Filters -->
            <div id="departmentFiltersPanel" class="departments-filter-panel">

                <div class="departments-filter-field">
                    <label for="<%= ddlCategoryFilter.ClientID %>">Category</label>

                    <asp:DropDownList
                        ID="ddlCategoryFilter"
                        runat="server"
                        CssClass="departments-filter-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="DepartmentFilter_Changed">
                    </asp:DropDownList>
                </div>

                <div class="departments-filter-field">
                    <label for="<%= ddlStatusFilter.ClientID %>">Status</label>

                    <asp:DropDownList
                        ID="ddlStatusFilter"
                        runat="server"
                        CssClass="departments-filter-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="DepartmentFilter_Changed">
                        <asp:ListItem Text="All Statuses" Value="" />
                        <asp:ListItem Text="Active" Value="Active" />
                        <asp:ListItem Text="Inactive" Value="Inactive" />
                    </asp:DropDownList>
                </div>

            </div>

            <!-- Feedback Message -->
            <asp:Label ID="lblDepartmentMessage" runat="server"
                CssClass="departments-message"
                EnableViewState="false" />

            <!-- Departments Grid -->
            <div class="departments-table-responsive">

                <asp:GridView
                    ID="gvDepartments"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="departments-table"
                    GridLines="None"
                    DataKeyNames="deptID,deptStatus"
                    EmptyDataText="No departments found."
                    OnRowCommand="gvDepartments_RowCommand"
                    OnRowDataBound="gvDepartments_RowDataBound">

                    <Columns>

                        <asp:BoundField
                            DataField="deptID"
                            HeaderText="ID" />

                        <asp:BoundField
                            DataField="deptName"
                            HeaderText="Department Name" />

                        <asp:BoundField
                            DataField="deptCode"
                            HeaderText="Department Code" />

                        <asp:BoundField
                            DataField="categoryName"
                            HeaderText="Category" />

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblDepartmentStatus"
                                    runat="server"
                                    Text='<%# Eval("deptStatus") %>'
                                    CssClass="departments-status-badge" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>

                                <div class="departments-row-actions">

                                    <asp:LinkButton
                                        ID="btnEditDepartment"
                                        runat="server"
                                        Text="Edit"
                                        CssClass="departments-action-btn departments-edit-btn"
                                        CommandName="EditDepartment"
                                        CommandArgument='<%# Eval("deptID") %>'
                                        CausesValidation="false" />

                                    <asp:LinkButton
                                        ID="btnToggleDepartment"
                                        runat="server"
                                        CssClass="departments-action-btn departments-toggle-btn"
                                        CommandName="ToggleStatus"
                                        CommandArgument='<%# Eval("deptID") %>'
                                        CausesValidation="false"
                                        OnClientClick="return confirm('Are you sure you want to change this department status?');" />

                                </div>

                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                    <EmptyDataRowStyle CssClass="departments-empty-row" />

                </asp:GridView>

            </div>

        </div>

    </div>

    <!-- Filters Toggle Behavior -->
    <script type="text/javascript">
        (function () {
            var toggle = document.getElementById("btnDepartmentFilters");
            var panel = document.getElementById("departmentFiltersPanel");

            if (!toggle || !panel) {
                return;
            }

            toggle.addEventListener("click", function () {
                var isOpen = panel.classList.toggle("is-open");

                toggle.classList.toggle("is-active", isOpen);
                toggle.setAttribute("aria-expanded", isOpen ? "true" : "false");
            });
        })();
    </script>

</asp:Content>