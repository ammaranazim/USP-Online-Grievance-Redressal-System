<%@ Page Title="Department Heads"
    Language="C#"
    MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true"
    CodeFile="AdminDepartmentHeads.aspx.cs"
    Inherits="Admin_AdminDepartmentHeads" %>

<asp:Content ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

    <link href="css/AdminDepartmentHeads.css"
        rel="stylesheet" />

</asp:Content>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="admin-department-heads-page">

        <!-- Page Header -->
        <div class="department-heads-header">
            <div>
                <h1>Department Heads</h1>
                <p>Manage department heads and their account access.</p>
            </div>

            <asp:HyperLink
                ID="lnkRegisterDepartmentHead"
                runat="server"
                NavigateUrl="~/Admin/AdminRegisterDepartmentHead.aspx"
                CssClass="btn-register-department-head">
                <span aria-hidden="true">+</span>
                Register Department Head
            </asp:HyperLink>
        </div>
<!-- KPI Cards - Consistent with Escalated Grievances -->
<section class="row dashboard-kpi-grid department-heads-kpi-grid">

    <!-- TOTAL DEPARTMENT HEADS -->
    <div class="col-12 col-sm-6 col-xl-4 mb-3">
        <asp:LinkButton
            ID="btnTotalDepartmentHeads"
            runat="server"
            CssClass="kpi-card kpi-card-clickable"
            CommandArgument="all"
            OnClick="KpiCard_Click"
            CausesValidation="false"
            aria-label="Show all department heads">

            <div class="kpi-icon kpi-icon-blue" aria-hidden="true">
                &#8593;
            </div>

            <div class="kpi-content">
                <span class="kpi-label">Total Department Heads</span>

                <asp:Label
                    ID="lblTotalDepartmentHeads"
                    runat="server"
                    CssClass="kpi-value"
                    Text="0" />
            </div>

        </asp:LinkButton>
    </div>

    <!-- ACTIVE DEPARTMENT HEADS -->
    <div class="col-12 col-sm-6 col-xl-4 mb-3">
        <asp:LinkButton
            ID="btnActiveDepartmentHeads"
            runat="server"
            CssClass="kpi-card kpi-card-clickable"
            CommandArgument="active"
            OnClick="KpiCard_Click"
            CausesValidation="false"
            aria-label="Show active department heads">

            <div class="kpi-icon kpi-icon-blue" aria-hidden="true">
                &#10003;
            </div>

            <div class="kpi-content">
                <span class="kpi-label">Active</span>

                <asp:Label
                    ID="lblActiveDepartmentHeads"
                    runat="server"
                    CssClass="kpi-value"
                    Text="0" />
            </div>

        </asp:LinkButton>
    </div>

    <!-- INACTIVE DEPARTMENT HEADS -->
    <div class="col-12 col-sm-6 col-xl-4 mb-3">
        <asp:LinkButton
            ID="btnInactiveDepartmentHeads"
            runat="server"
            CssClass="kpi-card kpi-card-clickable"
            CommandArgument="inactive"
            OnClick="KpiCard_Click"
            CausesValidation="false"
            aria-label="Show inactive department heads">

            <div class="kpi-icon kpi-icon-orange" aria-hidden="true">
                !
            </div>

            <div class="kpi-content">
                <span class="kpi-label">Inactive</span>

                <asp:Label
                    ID="lblInactiveDepartmentHeads"
                    runat="server"
                    CssClass="kpi-value"
                    Text="0" />
            </div>

        </asp:LinkButton>
    </div>

</section>

        <!-- Message -->
        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="department-heads-message"
            EnableViewState="false" />

        <!-- Search and Filters -->
        <div class="department-heads-filter-card">

            <div class="department-heads-toolbar">

                <div class="department-heads-search">
                    <asp:TextBox
                        ID="txtSearch"
                        runat="server"
                        CssClass="department-heads-search-input"
                        placeholder="Search name, username, email..." />

                    <asp:Button
                        ID="btnSearch"
                        runat="server"
                        Text="Search"
                        CssClass="department-heads-search-btn"
                        OnClick="btnSearch_Click" />
                </div>

                <div class="department-heads-toolbar-actions">

                    <asp:Button
                        ID="btnClear"
                        runat="server"
                        Text="Clear"
                        CssClass="department-heads-clear-btn"
                        CausesValidation="false"
                        OnClick="btnClear_Click" />

                    <button
                        type="button"
                        id="btnToggleDepartmentHeadFilters"
                        class="department-heads-filter-toggle"
                        aria-expanded="false"
                        aria-controls="departmentHeadAdvancedFilters"
                        onclick="toggleDepartmentHeadFilters()">
                        Filters
                        <span aria-hidden="true">&#9662;</span>
                    </button>

                </div>

            </div>

            <!-- Advanced Filters -->
            <div
                id="departmentHeadAdvancedFilters"
                class="department-heads-advanced-filters">

                <div class="department-heads-filter-field">
                    <label for="<%= ddlDepartment.ClientID %>">
                        Department
                    </label>

                    <asp:DropDownList
                        ID="ddlDepartment"
                        runat="server"
                        CssClass="department-heads-filter-select"
                        AppendDataBoundItems="true">
                        <asp:ListItem
                            Text="All Departments"
                            Value="0" />
                    </asp:DropDownList>
                </div>

                <div class="department-heads-filter-field">
                    <label for="<%= ddlStatus.ClientID %>">
                        Account Status
                    </label>

                    <asp:DropDownList
                        ID="ddlStatus"
                        runat="server"
                        CssClass="department-heads-filter-select">
                        <asp:ListItem
                            Text="All Statuses"
                            Value="" />
                        <asp:ListItem
                            Text="Active"
                            Value="active" />
                        <asp:ListItem
                            Text="Inactive"
                            Value="inactive" />
                    </asp:DropDownList>
                </div>

            </div>

        </div>

        <!-- Department Heads Records -->
        <div class="department-heads-table-card">

            <div class="department-heads-table-header">
                <div>
                    <h2>Department Head Records</h2>
                    <p>View and manage registered department heads.</p>
                </div>

                <div class="department-heads-record-count">
                    <asp:Label
                        ID="lblRecordCount"
                        runat="server"
                        Text="0 records" />
                </div>
            </div>

            <div class="department-heads-table-responsive">

                <asp:GridView
                    ID="gvDepartmentHeads"
                    runat="server"
                    AutoGenerateColumns="false"
                    CssClass="department-heads-table"
                    GridLines="None"
                    DataKeyNames="deptHeadID,deptHeadStatus"
                    EmptyDataText="No department heads found."
                    OnRowCommand="gvDepartmentHeads_RowCommand">

                    <Columns>

                        <asp:BoundField
                            DataField="deptHeadName"
                            HeaderText="HOD Name" />

                       <asp:TemplateField HeaderText="Username">
    <ItemTemplate>
        <span class="department-heads-username">
            <%# Eval("deptHeadUserName") %>
        </span>
    </ItemTemplate>
</asp:TemplateField>

                        <asp:BoundField
                            DataField="deptHeadEmail"
                            HeaderText="Email" />

                        <asp:BoundField
                            DataField="deptName"
                            HeaderText="Department" />

                        <asp:BoundField
                            DataField="deptHeadDesignation"
                            HeaderText="Designation" />

                      <asp:TemplateField HeaderText="Status">
    <ItemTemplate>
        <span class='<%# "department-heads-status " +
            (String.Equals(
                Convert.ToString(Eval("deptHeadStatus")).Trim(),
                "active",
                StringComparison.OrdinalIgnoreCase)
                ? "department-heads-status-active"
                : "department-heads-status-inactive") %>'>
            <%#: Eval("deptHeadStatus") %>
        </span>
    </ItemTemplate>
</asp:TemplateField>

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>

                                <div class="department-heads-row-actions">

                                    <asp:LinkButton
                                        ID="btnEditDepartmentHead"
                                        runat="server"
                                        Text="Edit"
                                        CssClass="department-heads-edit-btn"
                                        CommandName="EditDepartmentHead"
                                        CommandArgument='<%# Eval("deptHeadID") %>'
                                        CausesValidation="false" />

                                    <asp:LinkButton
                                        ID="btnToggleDepartmentHeadStatus"
                                        runat="server"
                                        Text='<%# GetDepartmentHeadActionText(Eval("deptHeadStatus")) %>'
                                        CssClass="department-heads-status-btn"
                                        CommandName="ToggleStatus"
                                        CommandArgument='<%# Eval("deptHeadID") %>'
                                        CausesValidation="false"
                                        OnClientClick="return confirm('Are you sure you want to change this account status?');" />

                                </div>

                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

        </div>

    </div>

    <script type="text/javascript">
        function toggleDepartmentHeadFilters() {
            var panel = document.getElementById(
                "departmentHeadAdvancedFilters"
            );

            var button = document.getElementById(
                "btnToggleDepartmentHeadFilters"
            );

            var isOpen = panel.classList.toggle("is-open");

            button.classList.toggle("is-active", isOpen);
            button.setAttribute("aria-expanded", isOpen);
        }
    </script>

</asp:Content>