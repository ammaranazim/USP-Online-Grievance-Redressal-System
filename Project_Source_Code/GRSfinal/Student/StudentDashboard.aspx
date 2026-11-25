<%@ Page Title="" Language="C#" MasterPageFile="~/Student/StudentMaster.master" AutoEventWireup="true" CodeFile="StudentDashboard.aspx.cs" Inherits="Student_StudentDashboard" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="HeadContent"
    Runat="Server">

    <link href="css/StudentDashboard.css" rel="stylesheet" />

</asp:Content>


<asp:Content ID="Content2"
    ContentPlaceHolderID="MainContent"
    Runat="Server">

    <asp:MultiView ID="mvStudentDashboard"
        runat="server"
        ActiveViewIndex="0">


        <!-- ================================================= -->
        <!-- VIEW 0 : OVERVIEW -->
        <!-- ================================================= -->

        <asp:View ID="viewOverview"
            runat="server">

            <div class="dashboard-container">

                <!-- Page Heading -->

               <div class="dashboard-welcome d-flex align-items-center justify-content-between">

                    <div>
                        <h1>Student Dashboard</h1>

                        <p>
                            View and manage your grievances
                        </p>
                    </div>

                   <a href="SubmitGrievance.aspx"
   class="dashboard-submit-btn btn btn-primary">

    <span>+</span>
    Submit Grievance

</a>

                </div>


                <!-- Statistics -->

           
                <div class="stats-grid">

    <!-- Total Grievances -->
    <div class="stat-card">

       <div class="stat-icon total-icon">
    <i class="fas fa-clipboard-list"></i>
</div>

        <div class="stat-info">

            <span class="stat-label">
                Total Grievances
            </span>

            <span class="stat-number">
                <asp:Label ID="lblTotal"
                    runat="server"
                    Text="0">
                </asp:Label>
            </span>

        </div>

    </div>


    <!-- Pending -->
    <div class="stat-card">

        <div class="stat-icon pending-icon">
    <i class="fas fa-hourglass-half"></i>
</div>

        <div class="stat-info">

            <span class="stat-label">
                Pending
            </span>

            <span class="stat-number">
                <asp:Label ID="lblPending"
                    runat="server"
                    Text="0">
                </asp:Label>
            </span>

        </div>

    </div>


    <!-- In Process -->
    <div class="stat-card">

        <div class="stat-icon process-icon">
    <i class="fas fa-sync-alt"></i>
</div>

        <div class="stat-info">

            <span class="stat-label">
                In Process
            </span>

            <span class="stat-number">
                <asp:Label ID="lblInProcess"
                    runat="server"
                    Text="0">
                </asp:Label>
            </span>

        </div>

    </div>


    <!-- Resolved -->
    <div class="stat-card">

       <div class="stat-icon resolved-icon">
    <i class="fas fa-check"></i>
</div>

        <div class="stat-info">

            <span class="stat-label">
                Resolved
            </span>

            <span class="stat-number">
                <asp:Label ID="lblResolved"
                    runat="server"
                    Text="0">
                </asp:Label>
            </span>

        </div>

    </div>

</div>



                <!-- Recent Grievances -->

                <div class="recent-section">

                    <div class="section-header">

                        <div>
                            <h2>Recent Grievances</h2>

                            <p>
                                Your recently submitted grievances
                            </p>
                        </div>

                        <a href="StudentDashboard.aspx?view=TrackGrievance"
                           class="view-all-link">
                            View All
                        </a>

                    </div>


                    <div class="table-wrapper">

                        <asp:GridView
                            ID="gvRecentGrievances"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="grievance-table table mb-0"
                            GridLines="None">

                            <Columns>

                                <asp:TemplateField HeaderText="Grievance ID">
        <ItemTemplate>
            <span class="grievance-id">
                <%# Eval("grievanceID", "GRV-{0}") %>
            </span>
        </ItemTemplate>
    </asp:TemplateField>

                                <asp:BoundField
                                    DataField="grievanceTitle"
                                    HeaderText="Title" />

                              <asp:TemplateField HeaderText="Status">
    <ItemTemplate>
        <span class='<%# "badge grievance-status " + 
            (Eval("grievanceStatus").ToString() == "Pending"
                ? "status-pending"
                : Eval("grievanceStatus").ToString() == "In Process"
                    ? "status-in-process"
                    : Eval("grievanceStatus").ToString() == "Resolved"
                        ? "status-resolved"
                        : "status-rejected") %>'>
            <%#: Eval("grievanceStatus") %>
        </span>
    </ItemTemplate>
</asp:TemplateField>

                                <asp:BoundField
                                    DataField="grievanceSubmissionDate"
                                    HeaderText="Submitted Date"
                                    DataFormatString="{0:dd MMM yyyy}" />

                            </Columns>

                            <EmptyDataTemplate>

                                <div class="empty-state">

                                    <div class="empty-state-icon">
                                        📋
                                    </div>

                                    <h3>
                                        No grievances yet
                                    </h3>

                                    <p>
                                        You have not submitted any grievance.
                                    </p>

                                </div>

                            </EmptyDataTemplate>

                        </asp:GridView>

                    </div>

                </div>


                <!-- Help -->

                <div class="dashboard-help">

                    <div class="help-icon">
                        ?
                    </div>

                    <div class="help-content">

                        <h3>
                            Need Help?
                        </h3>

                        <p>
                            If you have any issue regarding your grievance,
                            please contact the relevant department.
                        </p>

                    </div>

                </div>

            </div>

        </asp:View>



        <!-- ================================================= -->
        <!-- VIEW 1 : TRACK GRIEVANCE -->
        <!-- ================================================= -->

        <asp:View ID="viewTrack"
            runat="server">

            <div class="dashboard-container">

                <div class="page-heading">

                    <div>
                        <h1>Track Grievance</h1>

                        <p>
                            View the status and progress of your grievances.
                        </p>
                    </div>

                </div>

                
<div class="content-card track-grievance-card">

    <div class="card-header track-card-header">
        <div>
            <h2>My Grievances</h2>
            <p>Track your submitted grievances</p>
        </div>

        <asp:Label ID="lblGrievanceCount"
            runat="server"
            CssClass="record-count-badge"
            Text="0 Records" />
    </div>

    <div class="grievance-toolbar">

        <div class="grievance-search-row">
            <div class="grievance-search-box">
                <i class="fas fa-search" aria-hidden="true"></i>

                <asp:TextBox ID="txtGrievanceSearch"
                    runat="server"
                    CssClass="grievance-search-input"
                    placeholder="Search Grievance ID or title..."
                    MaxLength="100" />
            </div>

            <asp:Button ID="btnGrievanceSearch"
                runat="server"
                Text="Search"
                CssClass="grievance-search-btn"
                OnClick="btnGrievanceSearch_Click"
                CausesValidation="false" />

            <asp:Button ID="btnGrievanceClear"
                runat="server"
                Text="Clear"
                CssClass="grievance-clear-btn"
                OnClick="btnGrievanceClear_Click"
                CausesValidation="false" />

            <asp:Button ID="btnGrievanceFilters"
                runat="server"
                Text="Filters"
                CssClass="grievance-filter-toggle"
                OnClick="btnGrievanceFilters_Click"
                CausesValidation="false" />
        </div>

        <asp:Panel ID="pnlGrievanceFilters"
            runat="server"
            CssClass="grievance-filter-panel"
            Visible="false">

            <div class="grievance-filter-field">
                <asp:Label ID="lblStatusFilter"
                    runat="server"
                    AssociatedControlID="ddlGrievanceStatus"
                    Text="Grievance Status" />

                <asp:DropDownList ID="ddlGrievanceStatus"
                    runat="server"
                    CssClass="grievance-status-filter">
                    <asp:ListItem Text="All Statuses" Value="" />
                    <asp:ListItem Text="Pending" Value="Pending" />
                    <asp:ListItem Text="In Process" Value="In Process" />
                    <asp:ListItem Text="Resolved" Value="Resolved" />
                    <asp:ListItem Text="Rejected" Value="Rejected" />
                </asp:DropDownList>
            </div>

            <p class="grievance-filter-help">
                Select a status and click Search to apply the filter.
            </p>
        </asp:Panel>

    </div>

    <div class="table-wrapper">

        <asp:GridView ID="gvAllGrievances"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="grievance-table table mb-0"
            GridLines="None"
            DataKeyNames="grievanceID"
            AllowPaging="true"
            PageSize="10"
            PagerSettings-Mode="NumericFirstLast"
            PagerSettings-FirstPageText="First"
            PagerSettings-LastPageText="Last"
            PagerSettings-PageButtonCount="5"
            PagerStyle-CssClass="grievance-pager"
            OnPageIndexChanging="gvAllGrievances_PageIndexChanging">

            <Columns>

                <asp:TemplateField HeaderText="Grievance ID">
                    <ItemTemplate>
                        <span class="grievance-id">
                            <%# Eval("grievanceID", "GRV-{0}") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField
                    DataField="grievanceTitle"
                    HeaderText="Title" />

                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span class='<%# "badge grievance-status " +
                            (Convert.ToString(Eval("grievanceStatus")) == "Pending"
                                ? "status-pending"
                                : Convert.ToString(Eval("grievanceStatus")) == "In Process"
                                    ? "status-in-process"
                                    : Convert.ToString(Eval("grievanceStatus")) == "Resolved"
                                        ? "status-resolved"
                                        : "status-rejected") %>'>
                            <%#: Eval("grievanceStatus") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField
                    DataField="grievanceSubmissionDate"
                    HeaderText="Date"
                    DataFormatString="{0:dd MMM yyyy}" />

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:HyperLink
                            ID="lnkViewDetails"
                            runat="server"
                            Text="View Details"
                            NavigateUrl='<%# "~/Student/GrievanceDetails.aspx?id=" + Eval("grievanceID") %>'
                            CssClass="btn btn-primary btn-sm grievance-view-btn">
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>

            <EmptyDataTemplate>
                <div class="grievance-empty-state">
                    <i class="fas fa-folder-open" aria-hidden="true"></i>
                    <h3>No grievances found</h3>
                    <p>Try changing your search or filter, or submit a new grievance.</p>
                </div>
            </EmptyDataTemplate>

        </asp:GridView>

    </div>

</div>


            </div>

        </asp:View>



  <asp:View ID="viewNotifications" runat="server">

    <div class="dashboard-container student-notifications-page">

        <div class="student-notifications-header">
            <div>
                <h1>Notifications</h1>
                <p>Stay informed about updates to your grievances.</p>
            </div>

            <asp:Label ID="lblUnreadCount"
                runat="server"
                CssClass="student-notification-summary" />
        </div>

        <asp:Repeater
            ID="rptNotifications"
            runat="server"
            OnItemCommand="rptNotifications_ItemCommand">

            <ItemTemplate>

                <div class='<%# Convert.ToBoolean(Eval("isRead"))
                    ? "student-notification-card"
                    : "student-notification-card unread" %>'>

                    <div class="student-notification-card-icon">
                        <i class="fas fa-bell"></i>
                    </div>

                    <div class="student-notification-card-content">

                        <div class="student-notification-card-heading">
                            <h3><%#: Eval("notificationType") %></h3>

                            <asp:Label ID="lblReadStatus"
                                runat="server"
                                CssClass="student-read-status"
                                Text="Unread"
                                Visible='<%# !Convert.ToBoolean(Eval("isRead")) %>' />
                        </div>

                        <p class="student-notification-message">
                            <%#: Eval("message") %>
                        </p>

                        <div class="student-notification-meta">

                            <asp:PlaceHolder
                                ID="phGrievanceID"
                                runat="server"
                                Visible='<%# Eval("grievanceID") != DBNull.Value %>'>

                                <span>
                                    Grievance #<%#: Eval("grievanceID") %>
                                </span>

                            </asp:PlaceHolder>

                            <span>
                                <%#: Eval("createdDate", "{0:dd MMM yyyy, hh:mm tt}") %>
                            </span>

                        </div>

                        <asp:LinkButton
                            ID="lnkMarkAsRead"
                            runat="server"
                            Text="Mark as Read"
                            CommandName="MarkAsRead"
                            CommandArgument='<%# Eval("notificationID") %>'
                            CssClass="student-mark-read-btn"
                            CausesValidation="false"
                            Visible='<%# !Convert.ToBoolean(Eval("isRead")) %>'>
                        </asp:LinkButton>

                    </div>

                </div>

            </ItemTemplate>

        </asp:Repeater>

        <asp:Panel
            ID="pnlNoNotifications"
            runat="server"
            CssClass="student-notifications-empty"
            Visible="false">

            <div class="student-empty-icon">
                <i class="fas fa-bell"></i>
            </div>

            <h3>No notifications yet</h3>

            <p>Updates about your grievances will appear here.</p>

        </asp:Panel>

    </div>

</asp:View>


        <!-- ================================================= -->
        <!-- VIEW 3 : PROFILE -->
        <!-- ================================================= -->

                <asp:View ID="viewProfile" runat="server">
    <div class="dashboard-container">

        <!-- Page Heading -->
        <div class="page-heading">
            <div>
                <h1>My Profile</h1>
                <p>View your student information.</p>
            </div>
        </div>

        <!-- Profile Header -->
        <div class="content-card profile-header-card">
            <div class="profile-header">
                <div class="large-avatar">
                    <asp:Label ID="lblProfileInitials" runat="server" Text="ST"></asp:Label>
                </div>

                <div class="profile-header-info">
                    <h2>
                        <asp:Label ID="lblProfileName" runat="server"></asp:Label>
                    </h2>
                    <span>Student</span>
                </div>
            </div>
        </div>

        <!-- Profile Information -->
        <div class="profile-grid">

            <div class="profile-field">
                <label>Registration Number</label>
                <div class="profile-value">
                    <asp:Label ID="lblProfileRegNo" runat="server"></asp:Label>
                </div>
            </div>

            <div class="profile-field">
                <label>Email</label>
                <div class="profile-value">
                    <asp:Label ID="lblProfileEmail" runat="server"></asp:Label>
                </div>
            </div>

            <div class="profile-field">
                <label>Department</label>
                <div class="profile-value">
                    <asp:Label ID="lblProfileDepartment" runat="server"></asp:Label>
                </div>
            </div>

            <div class="profile-field">
                <label>Program</label>
                <div class="profile-value">
                    <asp:Label ID="lblProfileProgram" runat="server"></asp:Label>
                </div>
            </div>

            <div class="profile-field">
                <label>Semester</label>
                <div class="profile-value">
                    <asp:Label ID="lblProfileSemester" runat="server"></asp:Label>
                </div>
            </div>

        </div>

    </div>
</asp:View>

    </asp:MultiView>

</asp:Content>