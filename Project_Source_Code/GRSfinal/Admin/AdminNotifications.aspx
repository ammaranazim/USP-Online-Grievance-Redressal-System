<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminNotifications.aspx.cs" Inherits="Admin_AdminNotifications" %>
<asp:Content ID="HeadContent1"
    ContentPlaceHolderID="HeadContent"
    runat="server">
   <link href="css/AdminNotifications.css?v=1" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="admin-notifications-page">

        <div class="admin-notifications-header">
            <div>
                <h2>Notifications</h2>
                <p>Stay updated on important Grievance activities.</p>
            </div>

            <asp:Label ID="lblNotificationSummary"
                runat="server"
                CssClass="admin-notification-summary" />
        </div>

        <asp:Label ID="lblNotificationMessage"
            runat="server"
            CssClass="admin-notification-feedback"
            EnableViewState="false" />

        <asp:Panel ID="pnlNoNotifications"
            runat="server"
            CssClass="admin-notifications-empty"
            Visible="false">

            <div class="admin-empty-icon">
                <i class="fa-solid fa-bell"></i>
            </div>

            <h3>No notifications yet</h3>
            <p>New system notifications will appear here.</p>
        </asp:Panel>

        <asp:Repeater ID="rptNotifications"
            runat="server"
            OnItemCommand="rptNotifications_ItemCommand">

            <ItemTemplate>
         
<div class='<%# Convert.ToBoolean(Eval("isRead")) ? "admin-notification-card read" : "admin-notification-card unread" %>'>

    <div class="admin-notification-card-icon" aria-hidden="true">
        <i class="fa-regular fa-bell"></i>
    </div>

    <div class="admin-notification-card-content">

        <p class="admin-notification-message">
            <%#: Eval("message") %>
        </p>

        <div class="admin-notification-meta">

            <asp:PlaceHolder ID="phGrievanceID"
                runat="server"
                Visible='<%# Eval("grievanceID") != DBNull.Value %>'>
                <span>Grievance #<%#: Eval("grievanceID") %></span>
            </asp:PlaceHolder>

            <span>
                <%#: Eval("createdDate", "{0:dd MMM yyyy, hh:mm tt}") %>
            </span>

        </div>
    </div>

    <div class="admin-notification-card-action">
        <asp:LinkButton ID="btnMarkAsRead"
            runat="server"
            CssClass="admin-mark-read-btn"
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


