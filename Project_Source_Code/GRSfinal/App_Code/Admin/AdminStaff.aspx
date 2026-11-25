<%@ Page Title="Staff Management"
    Language="C#"
    MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true"
    CodeFile="AdminStaff.aspx.cs"
    Inherits="Admin_AdminStaff" %>

<asp:Content ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">
    <link href="css/AdminStaff.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="admin-staff-page">

        <div class="staff-page-header">
            <div>
                <h1>Staff Management</h1>
                <p>Manage staff accounts, department association and access status.</p>
            </div>

            <asp:HyperLink ID="lnkAddStaff" runat="server"
                NavigateUrl="~/Admin/AdminRegisterStaff.aspx"
                CssClass="staff-primary-btn">
                <span aria-hidden="true">+</span> Add New Staff
            </asp:HyperLink>
        </div>

        <section class="row staff-kpi-grid">

            <div class="col-12 col-sm-6 col-xl-4 mb-3">
                <asp:LinkButton ID="btnTotalStaff" runat="server"
                    CssClass="staff-kpi-card"
                    CommandArgument="all"
                    OnClick="KpiCard_Click"
                    CausesValidation="false">
                    <span class="staff-kpi-icon staff-kpi-blue">&#9775;</span>
                    <span class="staff-kpi-content">
                        <span class="staff-kpi-label">Total Staff</span>
                        <asp:Label ID="lblTotalStaff" runat="server"
                            CssClass="staff-kpi-value" Text="0" />
                    </span>
                </asp:LinkButton>
            </div>

            <div class="col-12 col-sm-6 col-xl-4 mb-3">
                <asp:LinkButton ID="btnActiveStaff" runat="server"
                    CssClass="staff-kpi-card"
                    CommandArgument="active"
                    OnClick="KpiCard_Click"
                    CausesValidation="false">
                    <span class="staff-kpi-icon staff-kpi-green">&#10003;</span>
                    <span class="staff-kpi-content">
                        <span class="staff-kpi-label">Active Staff</span>
                        <asp:Label ID="lblActiveStaff" runat="server"
                            CssClass="staff-kpi-value" Text="0" />
                    </span>
                </asp:LinkButton>
            </div>

            <div class="col-12 col-sm-6 col-xl-4 mb-3">
                <asp:LinkButton ID="btnInactiveStaff" runat="server"
                    CssClass="staff-kpi-card"
                    CommandArgument="inactive"
                    OnClick="KpiCard_Click"
                    CausesValidation="false">
                    <span class="staff-kpi-icon staff-kpi-red">&#33;</span>
                    <span class="staff-kpi-content">
                        <span class="staff-kpi-label">Inactive Staff</span>
                        <asp:Label ID="lblInactiveStaff" runat="server"
                            CssClass="staff-kpi-value" Text="0" />
                    </span>
                </asp:LinkButton>
            </div>

        </section>

        <asp:Label ID="lblMessage" runat="server"
            CssClass="staff-message" EnableViewState="false" />

        <div class="staff-filter-card">

            <div class="staff-toolbar">
                <div class="staff-search">
                    <asp:TextBox ID="txtSearch" runat="server"
                        CssClass="staff-search-input"
                        placeholder="Search by name, username, email or designation..." />

                    <asp:Button ID="btnSearch" runat="server"
                        Text="Search" CssClass="staff-search-btn"
                        OnClick="btnSearch_Click" />
                </div>

                <div class="staff-toolbar-actions">
                    <asp:Button ID="btnClear" runat="server"
                        Text="Clear" CssClass="staff-clear-btn"
                        CausesValidation="false"
                        OnClick="btnClear_Click" />

                    <button type="button" id="btnToggleStaffFilters"
                        class="staff-filter-toggle"
                        aria-expanded="false"
                        aria-controls="staffAdvancedFilters"
                        onclick="toggleStaffFilters()">
                        Filters <span aria-hidden="true">&#9662;</span>
                    </button>
                </div>
            </div>

            <div id="staffAdvancedFilters" class="staff-advanced-filters">

                <div class="staff-filter-field">
                    <label for="<%= ddlDepartment.ClientID %>">Department</label>
                    <asp:DropDownList ID="ddlDepartment" runat="server"
                        CssClass="staff-filter-select" />
                </div>

                <div class="staff-filter-field">
                    <label for="<%= ddlStatus.ClientID %>">Account Status</label>
                    <asp:DropDownList ID="ddlStatus" runat="server"
                        CssClass="staff-filter-select">
                        <asp:ListItem Text="All Statuses" Value="" />
                        <asp:ListItem Text="Active" Value="active" />
                        <asp:ListItem Text="Inactive" Value="inactive" />
                    </asp:DropDownList>
                </div>

            </div>
        </div>

        <div class="staff-table-card">

            <div class="staff-table-heading">
                <div>
                    <h2>Staff Records</h2>
                    <p>View and manage registered staff accounts.</p>
                </div>
                <span class="staff-record-count">
                    <asp:Label ID="lblRecordCount" runat="server"
                        Text="0 records" />
                </span>
            </div>

            <div class="staff-table-responsive">
                <asp:GridView ID="gvStaff" runat="server"
                    AutoGenerateColumns="false"
                    CssClass="staff-table"
                    GridLines="None"
                    DataKeyNames="staffID,staffStatus"
                    EmptyDataText="No staff records found."
                    OnRowCommand="gvStaff_RowCommand">

                   <Columns>
    <asp:BoundField
        DataField="staffID"
        HeaderText="Staff ID">
        <ItemStyle Width="8%" />
        <HeaderStyle Width="8%" />
    </asp:BoundField>

    <asp:BoundField
        DataField="staffName"
        HeaderText="Staff Name">
        <ItemStyle Width="16%" />
        <HeaderStyle Width="16%" />
    </asp:BoundField>

    <asp:BoundField
        DataField="deptName"
        HeaderText="Department">
        <ItemStyle Width="18%" />
        <HeaderStyle Width="18%" />
    </asp:BoundField>

    <asp:BoundField
        DataField="staffEmail"
        HeaderText="Email">
        <ItemStyle Width="23%" />
        <HeaderStyle Width="23%" />
    </asp:BoundField>

    <asp:TemplateField HeaderText="Status">
        <ItemStyle Width="12%" />
        <HeaderStyle Width="12%" />
        <ItemTemplate>
            <span class='<%# GetStaffStatusCss(Eval("staffStatus")) %>'>
                <%#: Eval("staffStatus") %>
            </span>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Actions">
        <ItemStyle Width="23%" />
        <HeaderStyle Width="23%" />
        <ItemTemplate>
            <div class="staff-row-actions">
                <asp:LinkButton ID="btnEditStaff" runat="server"
                    Text="Edit"
                    CssClass="staff-edit-btn"
                    CommandName="EditStaff"
                    CommandArgument='<%# Eval("staffID") %>'
                    CausesValidation="false" />

                <asp:LinkButton ID="btnToggleStaffStatus" runat="server"
                    Text='<%# GetStaffActionText(Eval("staffStatus")) %>'
                    CssClass="staff-status-btn"
                    CommandName="ToggleStatus"
                    CommandArgument='<%# Eval("staffID") %>'
                    CausesValidation="false"
                    OnClientClick="return confirm('Are you sure you want to change this staff account status?');" />
            </div>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>

                </asp:GridView>
            </div>
        </div>

    </div>

    <script type="text/javascript">
        function toggleStaffFilters() {
            var panel = document.getElementById("staffAdvancedFilters");
            var button = document.getElementById("btnToggleStaffFilters");
            var isOpen = panel.classList.toggle("is-open");

            button.classList.toggle("is-active", isOpen);
            button.setAttribute("aria-expanded", isOpen ? "true" : "false");
        }
    </script>

</asp:Content>