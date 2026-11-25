<%@ Page Title="All Grievances" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="AllGrievances.aspx.cs" Inherits="Admin_AllGrievances" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

    <link href="<%= ResolveUrl("~/Admin/css/AllGrievances.css") %>" rel="stylesheet" />

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <div class="all-grievances-page">

        <!-- Page Header -->
        <div class="page-header">
            <div>
                <h1 class="page-title">All Grievances</h1>
                <p class="page-subtitle">
                    View and manage all submitted grievances
                </p>
            </div>
        </div>


      
       <!-- Compact Search & Filter -->
<div class="filter-card">

    <!-- Main Toolbar -->
    <div class="filter-toolbar">

        <div class="filter-search">

            <asp:TextBox
                ID="txtSearch"
                runat="server"
                CssClass="form-control"
                placeholder="Search by Grievance ID, Student Reg No. or Student Name">
            </asp:TextBox>

        </div>

        <div class="filter-action">

            <asp:Button
                ID="btnSearch"
                runat="server"
                Text="Search"
                CssClass="btn btn-search"
                OnClick="btnSearch_Click" />

        </div>

        <div class="filter-action">
    <asp:Button
        ID="btnClearSearch"
        runat="server"
        Text="Clear"
        CssClass="btn btn-clear-search"
        CausesValidation="false"
        OnClick="btnClear_Click" />
</div>

        <button
            type="button"
            id="btnToggleFilters"
            class="btn btn-filter-toggle"
            onclick="toggleGrievanceFilters()"
            aria-expanded="false">

            <span class="filter-toggle-icon">⚙</span>
            <span>Filters</span>

        </button>

    </div>


    <!-- Collapsible Advanced Filters -->
    <div
        id="advancedFilters"
        class="advanced-filters">

        <div class="advanced-filter-inner">

            <!-- Status -->
            <div class="filter-field">

                <label for="<%= ddlStatus.ClientID %>">
                    Status
                </label>

                <asp:DropDownList
                    ID="ddlStatus"
                    runat="server"
                    CssClass="form-control">

                    <asp:ListItem
                        Text="All Statuses"
                        Value="">
                    </asp:ListItem>

                    <asp:ListItem
                        Text="Pending"
                        Value="Pending">
                    </asp:ListItem>

                    <asp:ListItem
                        Text="In Process"
                        Value="In Process">
                    </asp:ListItem>

                    <asp:ListItem
                        Text="Resolved"
                        Value="Resolved">
                    </asp:ListItem>

                </asp:DropDownList>

            </div>


            <!-- Category -->
            <div class="filter-field">

                <label for="<%= ddlCategory.ClientID %>">
                    Category
                </label>

                <asp:DropDownList
                    ID="ddlCategory"
                    runat="server"
                    CssClass="form-control">

                    <asp:ListItem
                        Text="All Categories"
                        Value="0">
                    </asp:ListItem>

                </asp:DropDownList>

            </div>


            <!-- Department -->
            <div class="filter-field">

                <label for="<%= ddlDepartment.ClientID %>">
                    Department
                </label>

                <asp:DropDownList
                    ID="ddlDepartment"
                    runat="server"
                    CssClass="form-control">

                    <asp:ListItem
                        Text="All Departments"
                        Value="0">
                    </asp:ListItem>

                </asp:DropDownList>

            </div>


            <!-- From Date -->
            <div class="filter-date">

                <label for="<%= txtFromDate.ClientID %>">
                    From Date
                </label>

                <asp:TextBox
                    ID="txtFromDate"
                    runat="server"
                    CssClass="form-control"
                    TextMode="Date">
                </asp:TextBox>

            </div>


            <!-- To Date -->
            <div class="filter-date">

                <label for="<%= txtToDate.ClientID %>">
                    To Date
                </label>

                <asp:TextBox
                    ID="txtToDate"
                    runat="server"
                    CssClass="form-control"
                    TextMode="Date">
                </asp:TextBox>

            </div>
    </div>

    </div>
     <!-- Validation Message: ADD HERE -->
    <asp:Label
        ID="lblFilterMessage"
        runat="server"
        CssClass="filter-validation-message"
        EnableViewState="false"
        Visible="false">
    </asp:Label>


</div>

        <!-- Grievances Table Card -->
        <div class="grievances-card">

            <!-- Table Header -->
            <div class="table-card-header">

                <div>
                    <h2>All Grievances</h2>
                    <p>Submitted grievances across the university.</p>
                </div>

                <div class="record-count">

                    <asp:Label
                        ID="lblRecordCount"
                        runat="server"
                        Text="0 Records">
                    </asp:Label>

                </div>

            </div>


            <!-- Table -->
            <div class="table-responsive">

                <asp:GridView
                    ID="gvGrievances"
                    runat="server"
                    AutoGenerateColumns="False"
                    DataKeyNames="grievanceID"
                    CssClass="grievances-table"
                    GridLines="None"
                    AllowPaging="True"
                    PageSize="10"
                    EmptyDataText="No grievances found."
                    OnPageIndexChanging="gvGrievances_PageIndexChanging"
                    OnRowCommand="gvGrievances_RowCommand">

                    <Columns>

                       
                        <asp:TemplateField HeaderText="Grievance ID">

                            <ItemTemplate>

                                <span class="grievance-id">
                                    <%# "GRV-" + Eval("grievanceID") %>
                                </span>

                            </ItemTemplate>

                        </asp:TemplateField>


                        
                        <asp:TemplateField HeaderText="Student">

                            <ItemTemplate>

                                <div class="student-info">

                                    <span class="student-name">
                                        <%# Eval("StudentName") %>
                                    </span>

                                    <span class="student-reg">
                                        <%# Eval("stdRegNo") %>
                                    </span>

                                </div>

                            </ItemTemplate>

                        </asp:TemplateField>


                         
                        <asp:BoundField
                            DataField="categoryName"
                            HeaderText="Category" />


                       
                        <asp:BoundField
                            DataField="deptName"
                            HeaderText="Department" />


                      
                        <asp:TemplateField HeaderText="Priority">

                            <ItemTemplate>

                               <span class='<%# "priority-badge priority-" + Eval("grievancePriority").ToString().ToLower().Replace(" ", "-") %>'>
    <%# Eval("grievancePriority") %>
</span>

                            </ItemTemplate>

                        </asp:TemplateField>


                       
                        <asp:TemplateField HeaderText="Status">

                            <ItemTemplate>

                               <span class='<%# "status-badge status-" + Eval("grievanceStatus").ToString().ToLower().Replace(" ", "-") %>'>
    <%# Eval("grievanceStatus") %>
</span>

                            </ItemTemplate>

                        </asp:TemplateField>


                     
                        <asp:TemplateField HeaderText="Submitted">

                            <ItemTemplate>

                                <span class="submission-date">
                                    <%# Eval("grievanceSubmissionDate", "{0:dd MMM yyyy}") %>
                                </span>

                            </ItemTemplate>

                        </asp:TemplateField>


                     
                        <asp:TemplateField HeaderText="Action">

                            <ItemTemplate>

                                <asp:LinkButton
                                    ID="btnViewDetails"
                                    runat="server"
                                    CssClass="view-details-btn"
                                    Text="View Details"
                                    CommandName="ViewDetails"
                                    CommandArgument='<%# Eval("grievanceID") %>'>
                                </asp:LinkButton>

                            </ItemTemplate>

                        </asp:TemplateField>

                    </Columns>

                    <PagerStyle CssClass="grievances-pager" />

                </asp:GridView>

            </div>

        </div>

    </div>
    <script type="text/javascript">


        function toggleGrievanceFilters() {
            var filters = document.getElementById("advancedFilters");
            var button = document.getElementById("btnToggleFilters");

            if (!filters || !button) {
                return false;
            }

            var isOpen = filters.classList.contains("show");

            filters.classList.toggle("show", !isOpen);
            button.classList.toggle("active", !isOpen);
            button.setAttribute("aria-expanded", String(!isOpen));

            return false;
        }


</script>
</asp:Content>
