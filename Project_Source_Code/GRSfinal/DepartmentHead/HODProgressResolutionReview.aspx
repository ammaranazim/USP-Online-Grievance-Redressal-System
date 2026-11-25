
<%@ Page Title="Progress &amp; Resolution Review"
    Language="C#"
    MasterPageFile="~/DepartmentHead/HODMaster.master"
    AutoEventWireup="true"
    CodeFile="HODProgressResolutionReview.aspx.cs"
    Inherits="DepartmentHead_HODProgressResolutionReview" %>

<asp:Content ID="TitleContent1" ContentPlaceHolderID="TitleContent" Runat="Server">
    Progress &amp; Resolution Review
</asp:Content>

<asp:Content ID="HeadContent1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="css/HODProgressResolutionReview.css?v=2" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" Runat="Server">

<div class="hod-pr-page">

    <div class="hod-pr-header">
        <div>
            <h1>Progress &amp; Resolution Review</h1>
            <p>Monitor departmental grievances, track progress and review submitted resolutions.</p>
        </div>
    </div>

    <!-- SEARCH AND FILTERS -->
    <div class="hod-pr-filter-card">

        <div class="hod-pr-toolbar">

            <asp:TextBox ID="txtSearch" runat="server"
                CssClass="hod-pr-search"
                placeholder="Search by Grievance ID, title, student or registration number" />

            <asp:Button ID="btnSearch" runat="server"
                Text="Search"
                CssClass="hod-pr-btn hod-pr-btn-primary"
                OnClick="btnSearch_Click" />

            <asp:Button ID="btnClear" runat="server"
                Text="Clear"
                CssClass="hod-pr-btn hod-pr-btn-secondary"
                CausesValidation="false"
                OnClick="btnClear_Click" />

            <button type="button"
                id="btnToggleFilters"
                class="hod-pr-btn hod-pr-btn-filter"
                aria-expanded="false"
                aria-controls="advancedFilters"
                onclick="toggleHODProgressFilters()">
                <i class="fa fa-sliders"></i> Filters
            </button>

        </div>

        <div id="advancedFilters" class="hod-pr-advanced-filters" style="display:none;">

            <div class="hod-pr-filter-field">
                <label for="<%= ddlStatus.ClientID %>">Status</label>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="hod-pr-select">
                    <asp:ListItem Text="All Statuses" Value="" />
                    <asp:ListItem Text="Pending" Value="Pending" />
                    <asp:ListItem Text="In Process" Value="In Process" />
                    <asp:ListItem Text="Resolved" Value="Resolved" />
                    <asp:ListItem Text="Rejected" Value="Rejected" />
                </asp:DropDownList>
            </div>

            <div class="hod-pr-filter-field">
                <label for="<%= ddlPriority.ClientID %>">Priority</label>
                <asp:DropDownList ID="ddlPriority" runat="server" CssClass="hod-pr-select">
                    <asp:ListItem Text="All Priorities" Value="" />
                    <asp:ListItem Text="Urgent" Value="Urgent" />
                    <asp:ListItem Text="High" Value="High" />
                    <asp:ListItem Text="Medium" Value="Medium" />
                    <asp:ListItem Text="Low" Value="Low" />
                </asp:DropDownList>
            </div>

            <div class="hod-pr-filter-field">
                <label for="<%= ddlStage.ClientID %>">Progress Stage</label>
                <asp:DropDownList ID="ddlStage" runat="server" CssClass="hod-pr-select">
                    <asp:ListItem Text="All Stages" Value="" />
                    <asp:ListItem Text="Department Review" Value="Department Review" />
                    <asp:ListItem Text="Assigned to Staff" Value="Assigned to Staff" />
                    <asp:ListItem Text="Resolution Under Review" Value="Resolution Under Review" />
                    <asp:ListItem Text="Resolved" Value="Resolved" />
                    <asp:ListItem Text="Rejected" Value="Rejected" />
                </asp:DropDownList>
            </div>

            <div class="hod-pr-filter-field">
                <label for="<%= ddlDueDate.ClientID %>">Due Date</label>
                <asp:DropDownList ID="ddlDueDate" runat="server" CssClass="hod-pr-select">
                    <asp:ListItem Text="All Due Dates" Value="" />
                    <asp:ListItem Text="Overdue" Value="Overdue" />
                    <asp:ListItem Text="Due Today" Value="Today" />
                    <asp:ListItem Text="Upcoming" Value="Upcoming" />
                    <asp:ListItem Text="No Due Date" Value="None" />
                </asp:DropDownList>
            </div>

        </div>
    </div>

    <!-- PROGRESS TABLE -->
    <div class="hod-pr-table-card">

        <div class="hod-pr-table-header">
            <div>
                <h2>Grievance Progress</h2>
                <p>Track workflow stages, assigned staff, priority and deadlines.</p>
            </div>

            <asp:Label ID="lblRecordCount" runat="server"
                CssClass="hod-pr-record-count" Text="Records: 0" />
        </div>

        <div class="hod-pr-table-wrap">

            <asp:GridView ID="gvProgress" runat="server"
                AutoGenerateColumns="False"
                CssClass="hod-pr-table"
                GridLines="None"
                DataKeyNames="grievanceID"
                EmptyDataText="No grievances match the selected search and filters."
                OnRowDataBound="gvProgress_RowDataBound">

                <Columns>

                    <asp:TemplateField HeaderText="Grievance ID">
                        <ItemTemplate>
                            <span class="hod-pr-grievance-id">
                                <%# "GRV-" + Eval("grievanceID") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Student">
                        <ItemTemplate>
                            <div class="hod-pr-student">
                                <strong><%# Server.HtmlEncode(Convert.ToString(Eval("studentName"))) %></strong>
                                <small><%# Server.HtmlEncode(Convert.ToString(Eval("stdRegNo"))) %></small>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="staffName"
                        HeaderText="Assigned Staff"
                        NullDisplayText="Not Assigned" />

                    <asp:TemplateField HeaderText="Current Stage">
                        <ItemTemplate>
                            <asp:Label ID="lblRowStage" runat="server"
                                Text='<%# Eval("progressStage") %>'
                                CssClass="hod-pr-stage-badge" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Priority">
                        <ItemTemplate>
                            <asp:Label ID="lblRowPriority" runat="server"
                                Text='<%# Eval("grievancePriority") %>'
                                CssClass="hod-pr-priority-badge" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblRowStatus" runat="server"
                                Text='<%# Eval("grievanceStatus") %>'
                                CssClass="status-badge" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Due Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDueDate" runat="server"
                                Text='<%# Eval("grievanceDueDate", "{0:dd MMM yyyy}") %>'
                                CssClass="hod-pr-due-date" />
                            <asp:Label ID="lblDueState" runat="server"
                                CssClass="hod-pr-due-state" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkViewProgress" runat="server"
                               NavigateUrl='<%# "~/DepartmentHead/HODProgressResolutionDetails.aspx?id=" + Eval("grievanceID") %>'
                                CssClass="hod-pr-view-btn">
                                <i class="fa fa-eye"></i> View Progress
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

        </div>
    </div>

</div>

<script type="text/javascript">
    function toggleHODProgressFilters() {
        var panel = document.getElementById("advancedFilters");
        var button = document.getElementById("btnToggleFilters");
        var isHidden = panel.style.display === "none";

        panel.style.display = isHidden ? "grid" : "none";
        button.setAttribute("aria-expanded", isHidden ? "true" : "false");
    }
</script>

</asp:Content>
