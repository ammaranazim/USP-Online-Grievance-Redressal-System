
<%@ Page Title="Staff Notifications" Language="C#"
    MasterPageFile="~/Staff/StaffMaster.master"
    AutoEventWireup="true"
    CodeFile="StaffNotifications.aspx.cs"
    Inherits="Staff_StaffNotifications" %>

<asp:Content ID="TitleContent1"
    ContentPlaceHolderID="HeadContent"
    runat="server">
    <link href="css/StaffNotifications.css?v=1" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="staff-notifications-page">

        <div class="staff-notifications-header">
            <div>
                <h2>Notifications</h2>
                <p>Stay informed about updates to your assigned grievances.</p>
            </div>

            <asp:Label ID="lblNotificationSummary"
                runat="server"
                CssClass="staff-notification-summary" />
        </div>

        <asp:Label ID="lblNotificationMessage"
            runat="server"
            CssClass="staff-notification-feedback"
            EnableViewState="false" />

        <asp:Panel ID="pnlNoNotifications"
            runat="server"
            CssClass="staff-notifications-empty"
            Visible="false">

            <div class="staff-empty-icon">
                <i class="fa-solid fa-bell"></i>
            </div>

            <h3>No notifications yet</h3>
            <p>Updates about your assigned grievances will appear here.</p>
        </asp:Panel>

        <asp:Repeater ID="rptNotifications"
            runat="server"
            OnItemCommand="rptNotifications_ItemCommand">

            <ItemTemplate>

                <div class='<%# Convert.ToBoolean(Eval("isRead"))
                    ? "staff-notification-card"
                    : "staff-notification-card unread" %>'>

                    <div class="staff-notification-card-icon">
                        <i class="fa-solid fa-bell"></i>
                    </div>

                    <div class="staff-notification-card-content">

                        <div class="staff-notification-card-heading">
                            <h3><%#: Eval("notificationType") %></h3>

                            <asp:Label ID="lblReadStatus"
                                runat="server"
                                CssClass="staff-read-status"
                                Visible='<%# !Convert.ToBoolean(Eval("isRead")) %>'
                                Text="Unread" />
                        </div>

                        <p class="staff-notification-message">
                            <%#: Eval("message") %>
                        </p>

                        <div class="staff-notification-meta">

                            <asp:PlaceHolder ID="PlaceHolder1"
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

                    </div>

                    <div class="staff-notification-card-action">

                        <asp:LinkButton ID="btnMarkAsRead"
                            runat="server"
                            CssClass="staff-mark-read-btn"
                            CommandName="MarkAsRead"
                            CommandArgument='<%# Eval("notificationID") %>'
                            CausesValidation="false"
                            Visible='<%# !Convert.ToBoolean(Eval("isRead")) %>'>
                            Mark as Read
                        </asp:LinkButton>

                    </div>

                </div>

            </ItemTemplate>

        </asp:Repeater>

    </div>

</asp:Content>
