
<%@ Page Title="Staff Assignment Overview" Language="C#" MasterPageFile="~/DepartmentHead/HODMaster.master" AutoEventWireup="true" CodeFile="HODStaffAllAssignment.aspx.cs" Inherits="DepartmentHead_HODStaffAllAssignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" Runat="Server">
    Staff Assignment Overview
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="css/HODStaffAllAssignment.css?v=1" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">

    <div class="hod-assignment-page">

        <div class="hod-page-header">
            <div>
                <h2 class="hod-page-title">Staff Assignment Overview</h2>
                <p class="hod-page-subtitle">
                    View department grievances, check their staff assignments, and assign unassigned grievances.
                </p>
            </div>
        </div>

        <!-- Assignment Statistics -->
        <div class="row hod-assignment-stats">

            <div class="col-lg-4 col-md-6">
                <div class="hod-stat-card">
                    <div class="hod-stat-label">Total Grievances</div>
                    <asp:Label ID="lblTotalGrievances" runat="server"
                        CssClass="hod-stat-value" Text="0" />
                </div>
            </div>

            <div class="col-lg-4 col-md-6">
                <div class="hod-stat-card">
                    <div class="hod-stat-label">Assigned</div>
                    <asp:Label ID="lblAssignedGrievances" runat="server"
                        CssClass="hod-stat-value" Text="0" />
                </div>
            </div>

            <div class="col-lg-4 col-md-6">
                <div class="hod-stat-card">
                    <div class="hod-stat-label">Unassigned</div>
                    <asp:Label ID="lblUnassignedGrievances" runat="server"
                        CssClass="hod-stat-value" Text="0" />
                </div>
            </div>

        </div>

        <!-- Messages -->
        <asp:Panel ID="pnlMessage" runat="server"
            CssClass="hod-assignment-message" Visible="false">
            <asp:Label ID="lblMessage" runat="server" />
        </asp:Panel>

        <!-- Search and Filters Toolbar -->
        <div class="hod-assignment-toolbar">

            <div class="hod-assignment-search">
                <asp:TextBox ID="txtSearch" runat="server"
                    CssClass="form-control hod-search-input"
                    placeholder="Search by Grievance ID, title, student, or registration number"
                    MaxLength="250" />

                <asp:Button ID="btnSearch" runat="server"
                    Text="Search"
                    CssClass="btn hod-btn-search"
                    OnClick="btnSearch_Click" />

                <asp:Button ID="btnClear" runat="server"
                    Text="Clear"
                    CssClass="btn hod-btn-clear"
                    CausesValidation="false"
                    OnClick="btnClear_Click" />

              <button type="button"
    class="btn hod-btn-filter"
    onclick="toggleHODAssignmentFilters(this)"
    aria-expanded="false"
    aria-controls="hodAssignmentFilters">
    <span class="filter-icon">&#9776;</span>
    Filters
</button>
          
            </div>

            <div class="collapse hod-filter-collapse" id="hodAssignmentFilters">
                <div class="hod-filter-panel">
                    <div class="form-group hod-filter-field">
                        <label for="<%= ddlAssignmentStatus.ClientID %>">
                            Assignment Status
                        </label>

                        <asp:DropDownList ID="ddlAssignmentStatus" runat="server"
                            CssClass="form-control hod-filter-select">
                            <asp:ListItem Text="All Assignments" Value="" />
                            <asp:ListItem Text="Assigned" Value="Assigned" />
                            <asp:ListItem Text="Unassigned" Value="Unassigned" />
                        </asp:DropDownList>
                    </div>

                    <p class="hod-filter-help">
                        Select an assignment status and click Search to apply the filter.
                    </p>
                </div>
            </div>

        </div>

        <!-- Assignment Overview Table -->
        <div class="hod-assignment-table-card">

         <div class="hod-table-heading hod-grievances-table-heading">
    <div>
        <h3 class="hod-section-title">Department Grievances</h3>
        <p class="hod-section-subtitle">
            Current assignment details for grievances in your department.
        </p>
    </div>

    <div class="hod-record-count">
        Records:
        <asp:Label ID="lblRecordCount" runat="server" Text="0" />
    </div>
</div>
            <div class="table-responsive hod-table-responsive">

                <asp:GridView ID="gvStaffAssignments" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table hod-grievances-table"
                    GridLines="None"
                    DataKeyNames="grievanceID"
                    EmptyDataText="No grievances found."
                    OnRowDataBound="gvStaffAssignments_RowDataBound">

                   
<Columns>
    <asp:TemplateField HeaderText="Grievance ID">
        <ItemTemplate>
            <span class="hod-grievance-id">
                <%# Eval("grievanceID", "GRV-{0}") %>
            </span>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Student">
        <ItemTemplate>
            <div class="hod-student-name">
                <%# Eval("studentName") %>
            </div>
            <div class="hod-student-regno">
                <%# Eval("stdRegNo") %>
            </div>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:BoundField DataField="grievanceTitle"
        HeaderText="Title" />

    <asp:TemplateField HeaderText="Priority">
        <ItemTemplate>
            <asp:Label ID="lblPriority" runat="server"
                Text='<%# Eval("grievancePriority") %>'
                CssClass='<%# "hod-priority-badge priority-" + Convert.ToString(Eval("grievancePriority")).ToLower() %>' />
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Status">
        <ItemTemplate>
            <asp:Label ID="lblGrievanceStatus" runat="server"
                Text='<%# Eval("grievanceStatus") %>'
                CssClass="hod-grievance-status" />
        </ItemTemplate>
    </asp:TemplateField>

   <asp:TemplateField HeaderText="Assigned Staff">
    <ItemTemplate>
        <asp:Label ID="lblAssignedStaff" runat="server"
            Text='<%# String.IsNullOrWhiteSpace(Convert.ToString(Eval("staffName"))) ? "Not Assigned" : Eval("staffName") %>'
            CssClass="hod-staff-name" />
    </ItemTemplate>
</asp:TemplateField>

    <asp:TemplateField HeaderText="Assignment">
        <ItemTemplate>
            <asp:Label ID="lblAssignmentStatus" runat="server"
                Text='<%# Convert.ToString(Eval("assignmentStatus")) == "Assigned" ? "Assigned" : "Unassigned" %>'
                CssClass="hod-assignment-status" />
        </ItemTemplate>
    </asp:TemplateField>

   
<asp:TemplateField HeaderText="Action">
    <ItemTemplate>
        <div class="hod-assignment-actions">

            <asp:HyperLink ID="lnkViewGrievance" runat="server"
                Text="View Details"
                CssClass="hod-view-details-btn"
                NavigateUrl='<%# Eval("grievanceID", "~/DepartmentHead/HODGrievanceDetails.aspx?id={0}") %>' />

            <asp:HyperLink ID="lnkAssignStaff" runat="server"
                Text="Assign Staff"
                CssClass="hod-assign-link"
                NavigateUrl='<%# Eval("grievanceID", "~/DepartmentHead/HODStaffAssignment.aspx?id={0}") %>' />

        </div>
    </ItemTemplate>
</asp:TemplateField>

</Columns>


                    <EmptyDataRowStyle CssClass="hod-empty-row" />

                </asp:GridView>

                <asp:Panel ID="pnlNoAssignments" runat="server"
                    CssClass="hod-no-assignments" Visible="false">
                    <h4>No Grievances Found</h4>
                    <p>There are no grievances matching the selected search and filter criteria.</p>
                </asp:Panel>

            </div>

        </div>

    </div>
    
<script type="text/javascript">
    function toggleHODAssignmentFilters(button) {
        var panel = document.getElementById("hodAssignmentFilters");

        if (!panel) {
            return;
        }

        var isOpen = panel.classList.contains("show");

        panel.classList.toggle("show", !isOpen);
        button.setAttribute("aria-expanded", (!isOpen).toString());
    }
</script>


</asp:Content>
