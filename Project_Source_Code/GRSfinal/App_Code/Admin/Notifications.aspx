
<%@ Page Title="Notifications" Language="C#" MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true" CodeFile="Notifications.aspx.cs"
    Inherits="Admin_Notifications" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
   <link href="css/AdminNotifications.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="admin-notifications-page">

        <div class="admin-notifications-header">
            <div>
                <h1>Notifications</h1>
                <p>Latest updates that require your attention.</p>
            </div>

            <asp:Label ID="lblNotificationCount" runat="server"
                CssClass="admin-notification-summary" />
        </div>

        <asp:Label ID="lblMessage" runat="server"
            CssClass="admin-notifications-message"
            EnableViewState="false" />

        <section class="admin-notification-list">

            <asp:Repeater ID="rptNotifications" runat="server"
                OnItemCommand="rptNotifications_ItemCommand">

                <ItemTemplate>

                    <div class='<%# Convert.ToBoolean(Eval("isRead"))
                        ? "admin-notification-card read"
                        : "admin-notification-card unread" %>'>

                        <div class="admin-notification-card-icon">
                            <i class="fas fa-bell" aria-hidden="true"></i>
                        </div>

                        <div class="admin-notification-card-content">

                            <div class="admin-notification-card-heading">
                                <div class="admin-notification-message">
                                    <%#: Eval("message") %>
                                </div>

                                <span class='<%# Convert.ToBoolean(Eval("isRead"))
                                    ? "admin-notification-status read"
                                    : "admin-notification-status unread" %>'>
                                    <%# Convert.ToBoolean(Eval("isRead"))
                                        ? "Read" : "Unread" %>
                                </span>
                            </div>

                            <div class="admin-notification-type">
                                <%#: Eval("notificationType") %>
                            </div>

                            <div class="admin-notification-date">
                                <%# Eval("createdDate", "{0:dd MMM yyyy, hh:mm tt}") %>
                            </div>

                            <div class="admin-notification-actions">

                                <asp:LinkButton ID="btnViewDetails" runat="server"
                                    Text="View Details"
                                    CssClass="admin-notification-view-btn"
                                    CommandName="ViewDetails"
                                    CommandArgument='<%# Eval("notificationID") %>'
                                    CausesValidation="false"
                                    Visible='<%# Convert.ToString(Eval("notificationType")) == "EscalationReview" %>' />

                                <asp:LinkButton ID="btnMarkRead" runat="server"
                                    Text="Mark as Read"
                                    CssClass="admin-notification-read-btn"
                                    CommandName="MarkRead"
                                    CommandArgument='<%# Eval("notificationID") %>'
                                    CausesValidation="false"
                                    Visible='<%# !Convert.ToBoolean(Eval("isRead")) %>' />

                            </div>

                        </div>
                    </div>

                </ItemTemplate>

            </asp:Repeater>

            <asp:Panel ID="pnlNoNotifications" runat="server"
                CssClass="admin-notification-empty" Visible="false">

                <div class="admin-notification-empty-icon">
                    <i class="fas fa-bell" aria-hidden="true"></i>
                </div>

                <h3>No Notifications Yet</h3>
                <p>New updates will appear here when they become available.</p>

            </asp:Panel>

        </section>

    </div>

</asp:Content>
