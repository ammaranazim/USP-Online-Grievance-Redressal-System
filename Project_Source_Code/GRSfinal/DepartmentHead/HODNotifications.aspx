
<%@ Page Title="HOD Notifications" Language="C#"
    MasterPageFile="~/DepartmentHead/HODMaster.master"
    AutoEventWireup="true"
    CodeFile="HODNotifications.aspx.cs"
    Inherits="DepartmentHead_HODNotifications" %>

<asp:Content ID="TitleContent1"
    ContentPlaceHolderID="TitleContent"
    runat="server">
    Notifications
</asp:Content>

<asp:Content ID="HeadContent1"
    ContentPlaceHolderID="HeadContent"
    runat="server">
    <link href="css/HODNotifications.css?v=1" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="hod-notifications-page">

        <div class="hod-notifications-header">
            <div>
                <h2>Notifications</h2>
                <p>Stay informed about updates to your department's grievances.</p>
            </div>

            <asp:Label ID="lblNotificationSummary"
                runat="server"
                CssClass="hod-notification-summary" />
        </div>

        <asp:Label ID="lblNotificationMessage"
            runat="server"
            CssClass="hod-notification-feedback"
            EnableViewState="false" />

        <asp:Panel ID="pnlNoNotifications"
            runat="server"
            CssClass="hod-notifications-empty"
            Visible="false">
            <div class="hod-empty-icon">
                <i class="fa-regular fa-bell"></i>
            </div>
            <h3>No notifications yet</h3>
            <p>New notifications about your grievances will appear here.</p>
        </asp:Panel>

      
<asp:Repeater ID="rptNotifications"
    runat="server"
    OnItemCommand="rptNotifications_ItemCommand">

    <ItemTemplate>
        <div class='<%# Convert.ToBoolean(Eval("isRead"))
            ? "hod-notification-card"
            : "hod-notification-card unread" %>'>

            <div class="hod-notification-card-icon">
                <i class="fa-regular fa-bell"></i>
            </div>

            <div class="hod-notification-card-content">

                <div class="hod-notification-card-heading">

                    <asp:Label ID="lblReadStatus"
                        runat="server"
                        CssClass="hod-read-status"
                        Visible='<%# !Convert.ToBoolean(Eval("isRead")) %>'
                        Text="Unread" />

                </div>

                <p class="hod-notification-message">
                    <%#: Eval("message") %>
                </p>

                <div class="hod-notification-meta">

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

            <div class="hod-notification-card-action">

                <asp:LinkButton ID="btnMarkAsRead"
                    runat="server"
                    CssClass="hod-mark-read-btn"
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
