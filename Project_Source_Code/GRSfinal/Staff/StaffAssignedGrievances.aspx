
<%@ Page Title="My Assigned Grievances" Language="C#" MasterPageFile="~/Staff/StaffMaster.master" AutoEventWireup="true" CodeFile="StaffAssignedGrievances.aspx.cs" Inherits="Staff_StaffAssignedGrievances" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="<%= ResolveUrl("~/Staff/css/StaffAssignedGrievances.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <div class="assigned-page">

        <div class="assigned-page-header">
            <div>
                <h1>My Assigned Grievances</h1>
                <p>Search, review and manage the Grievances assigned to you.</p>
            </div>
        </div>

        <!-- Search and Filter Toolbar -->
        <section class="assigned-toolbar">

            <div class="assigned-toolbar-main">

                <div class="assigned-search">
                    <i class="fa-solid fa-magnifying-glass" aria-hidden="true"></i>

                    <asp:TextBox ID="txtSearch"
                        runat="server"
                        CssClass="assigned-search-input"
                        placeholder="Search by ID, title, category or priority"
                        MaxLength="200" />
                </div>

                <asp:Button ID="btnSearch"
                    runat="server"
                    Text="Search"
                    CssClass="assigned-btn assigned-btn-primary"
                    OnClick="btnSearch_Click" />

                <asp:Button ID="btnClear"
                    runat="server"
                    Text="Clear"
                    CssClass="assigned-btn assigned-btn-clear"
                    CausesValidation="false"
                    OnClick="btnClear_Click" />

                <button type="button"
                    id="btnToggleFilters"
                    class="assigned-btn assigned-btn-filter"
                    aria-expanded="false"
                    aria-controls="advancedFilters"
                    onclick="toggleAssignedFilters()">
                    <i class="fa-solid fa-sliders" aria-hidden="true"></i>
                    Filters
                </button>

            </div>

            <div id="advancedFilters" class="assigned-advanced-filters" hidden>
                <div class="assigned-filter-field">
                    <label for="<%= ddlStatus.ClientID %>">Grievance Status</label>

                    <asp:DropDownList ID="ddlStatus"
                        runat="server"
                        CssClass="assigned-filter-select">
                        <asp:ListItem Text="All Statuses" Value="" />
                        <asp:ListItem Text="Pending" Value="Pending" />
                        <asp:ListItem Text="In Process" Value="In Process" />
                        <asp:ListItem Text="Resolution Under Review" Value="Resolution Under Review" />
                        <asp:ListItem Text="Resolved" Value="Resolved" />
                        <asp:ListItem Text="Rejected" Value="Rejected" />
                    </asp:DropDownList>
                </div>

             
            </div>

        </section>

        <!-- Assigned Grievances Table -->
        <section class="assigned-table-panel">

            <div class="assigned-table-heading">
                <div>
                    <h2>Assigned Grievances</h2>
                    <p>Only Grievances assigned to your account are displayed.</p>
                </div>

                <span class="assigned-record-count">
                    <asp:Label ID="lblRecordCount" runat="server" Text="0 records" />
                </span>
            </div>

            <div class="table-responsive assigned-table-wrap">

                <asp:GridView ID="gvAssignedGrievances"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table assigned-grievances-table"
                    GridLines="None"
                    Width="100%"
                    EmptyDataText="No assigned Grievances match your search."
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
                            DataField="categoryName"
                            HeaderText="Category"
                            NullDisplayText="—" />

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

                        <asp:HyperLinkField
                            HeaderText="Action"
                            Text="View Details"
                            DataNavigateUrlFields="grievanceID"
                            DataNavigateUrlFormatString="~/Staff/StaffGrievanceDetails.aspx?grievanceID={0}"
                            ControlStyle-CssClass="assigned-view-link" />

                    </Columns>

                </asp:GridView>

            </div>
        </section>

    </div>

    <script type="text/javascript">
        function toggleAssignedFilters() {
            var panel = document.getElementById("advancedFilters");
            var button = document.getElementById("btnToggleFilters");

            if (!panel || !button) {
                return;
            }

            var isHidden = panel.hidden;
            panel.hidden = !isHidden;
            button.setAttribute("aria-expanded", isHidden ? "true" : "false");
            button.classList.toggle("filters-active", isHidden);
        }
    </script>

</asp:Content>
