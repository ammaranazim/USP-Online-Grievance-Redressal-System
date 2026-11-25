
<%@ Page Title="Grievances"
    Language="C#"
    MasterPageFile="~/DepartmentHead/HODMaster.master"
    AutoEventWireup="true"
    CodeFile="HODGrievances.aspx.cs"
    Inherits="DepartmentHead_HODGrievances" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="TitleContent"
    runat="server">
    Grievances
</asp:Content>

<asp:Content ID="Content2"
    ContentPlaceHolderID="HeadContent"
    runat="server">

    <link rel="stylesheet"
        href="<%= ResolveUrl("~/DepartmentHead/css/HODGrievances.css") %>?v=1" />

</asp:Content>

<asp:Content ID="Content3"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="hod-grievances-page">

        <!-- Page Heading -->
        <div class="hod-grievances-heading">
            <div>
                <h2>Grievances</h2>
                <p>View and manage grievances submitted to your department.</p>
            </div>
        </div>

        <!-- Search and Filter Toolbar -->
        <section class="hod-grievances-panel">

            <div class="hod-grievances-toolbar">

                <div class="hod-search-group">
                    <asp:TextBox ID="txtSearch"
                        runat="server"
                        CssClass="hod-search-input"
                        placeholder="Search by grievance ID, student or title..." />

                    <asp:Button ID="btnSearch"
                        runat="server"
                        Text="Search"
                        CssClass="hod-btn hod-btn-primary"
                        CausesValidation="false"
                        OnClick="btnSearch_Click" />

                    <asp:Button ID="btnClear"
                        runat="server"
                        Text="Clear"
                        CssClass="hod-btn hod-btn-secondary"
                        CausesValidation="false"
                        OnClick="btnClear_Click" />
                </div>

                <asp:Button ID="btnToggleFilters"
                    runat="server"
                    Text="Filters"
                    CssClass="hod-btn hod-btn-filter"
                    CausesValidation="false"
                    OnClick="btnToggleFilters_Click" />

            </div>

            
<!-- Advanced Filters -->
<asp:Panel ID="pnlAdvancedFilters"
    runat="server"
    CssClass="hod-advanced-filters"
    Visible="false">

    <div class="hod-filter-field">
        <asp:Label ID="lblStatusFilter"
            runat="server"
            AssociatedControlID="ddlStatus"
            Text="Status" />

        <asp:DropDownList ID="ddlStatus"
            runat="server"
            CssClass="hod-filter-select">
            <asp:ListItem Text="All Statuses" Value="" />
            <asp:ListItem Text="Pending" Value="Pending" />
            <asp:ListItem Text="In Process" Value="In Process" />
            <asp:ListItem Text="Resolved" Value="Resolved" />
            <asp:ListItem Text="Rejected" Value="Rejected" />
        </asp:DropDownList>
    </div>

    <div class="hod-filter-field">
        <asp:Label ID="lblPriorityFilter"
            runat="server"
            AssociatedControlID="ddlPriority"
            Text="Priority" />

        <asp:DropDownList ID="ddlPriority"
            runat="server"
            CssClass="hod-filter-select">
            <asp:ListItem Text="All Priorities" Value="" />
            <asp:ListItem Text="Low" Value="Low" />
            <asp:ListItem Text="Medium" Value="Medium" />
            <asp:ListItem Text="High" Value="High" />
            <asp:ListItem Text="Urgent" Value="Urgent" />
        </asp:DropDownList>
    </div>

    <div class="hod-filter-field">
        <asp:Label ID="lblDateFrom"
            runat="server"
            AssociatedControlID="txtDateFrom"
            Text="Submitted From" />

        <asp:TextBox ID="txtDateFrom"
            runat="server"
            CssClass="hod-filter-select"
            TextMode="Date" />
    </div>

    <div class="hod-filter-field">
        <asp:Label ID="lblDateTo"
            runat="server"
            AssociatedControlID="txtDateTo"
            Text="Submitted To" />

        <asp:TextBox ID="txtDateTo"
            runat="server"
            CssClass="hod-filter-select"
            TextMode="Date" />
    </div>

</asp:Panel>


            <!-- Record Count -->
            <div class="hod-grievances-table-heading">
                <div>
                    <h3>All Grievances</h3>
                    <p>Grievances assigned to your department</p>
                </div>

                <asp:Label ID="lblRecordCount"
                    runat="server"
                    CssClass="hod-record-count"
                    Text="0 Records" />
            </div>

            <!-- Grievances Table -->
            <div class="hod-grievances-table-responsive">

                <asp:GridView ID="gvGrievances"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="hod-grievances-table"
                    GridLines="None"
                    EmptyDataText="No grievances found."
                    AllowPaging="True"
                    PageSize="10"
                    OnPageIndexChanging="gvGrievances_PageIndexChanging"
                    OnRowCommand="gvGrievances_RowCommand"
                    UseAccessibleHeader="true">

                    <Columns>

                        <asp:TemplateField HeaderText="Grievance ID">
                            <ItemTemplate>
                                <span class="hod-grievance-id">
                                    GRV-<%# Eval("grievanceID") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                       
    <asp:TemplateField HeaderText="Student">
    <ItemTemplate>
        <div class="student-info">
            <span class="student-name">
                <%# Eval("studentName") %>
            </span>
            <span class="student-reg">
                <%# Eval("stdRegNo") %>
            </span>
        </div>
    </ItemTemplate>
</asp:TemplateField>

                        <asp:BoundField DataField="grievanceTitle"
                            HeaderText="Title" />

                        <asp:TemplateField HeaderText="Priority">
                            <ItemTemplate>
                                <span class='<%# GetPriorityBadgeClass(Eval("grievancePriority")) %>'>
                                    <%# Eval("grievancePriority") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

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

                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnViewDetails"
                                    runat="server"
                                    Text="View Details"
                                    CssClass="hod-view-details"
                                    CommandName="ViewDetails"
                                    CommandArgument='<%# Eval("grievanceID") %>'
                                    CausesValidation="false" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                    <PagerStyle CssClass="hod-grievances-pager" />

                </asp:GridView>

            </div>

        </section>

    </div>

</asp:Content>
