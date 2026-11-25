
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Notifications : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AdminID"] == null)
        {
            Response.Redirect("~/Admin/AdminLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (!IsPostBack)
        {
            LoadNotifications();
        }
    }

    private void LoadNotifications()
    {
        int adminID = Convert.ToInt32(Session["AdminID"]);

        DataTable notifications =
            DALSystemNotification.GetNotifications("Admin", adminID);

        if (notifications == null)
        {
            notifications = new DataTable();
        }

        rptNotifications.DataSource = notifications;
        rptNotifications.DataBind();

        pnlNoNotifications.Visible = notifications.Rows.Count == 0;

        int unreadCount = 0;

        foreach (DataRow row in notifications.Rows)
        {
            if (!Convert.IsDBNull(row["isRead"]) &&
                !Convert.ToBoolean(row["isRead"]))
            {
                unreadCount++;
            }
        }

        lblNotificationCount.Text =
            notifications.Rows.Count + " total · " +
            unreadCount + " unread";
    }

    protected void rptNotifications_ItemCommand(
        object source, RepeaterCommandEventArgs e)
    {
        if (Session["AdminID"] == null)
        {
            Response.Redirect("~/Admin/AdminLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        int notificationID;

        if (!int.TryParse(
            Convert.ToString(e.CommandArgument),
            out notificationID))
        {
            return;
        }

        int adminID = Convert.ToInt32(Session["AdminID"]);

        if (e.CommandName == "MarkRead")
        {
            int updated = DALSystemNotification.MarkAsRead(
                notificationID, "Admin", adminID);

            ShowMessage(
                updated > 0
                    ? "Notification marked as read."
                    : "Notification was already read or could not be found.",
                updated > 0);

            LoadNotifications();
            RefreshAdminNotificationBadge();
            return;
        }

        if (e.CommandName == "ViewDetails")
        {
            DataTable notifications =
                DALSystemNotification.GetNotifications("Admin", adminID);

            if (notifications == null ||
                !notifications.Columns.Contains("notificationID"))
            {
                ShowMessage("Notification was not found.", false);
                return;
            }

            DataRow[] matches = notifications.Select(
                "notificationID = " + notificationID);

            if (matches.Length == 0)
            {
                ShowMessage("Notification was not found.", false);
                LoadNotifications();
                return;
            }

            string type = Convert.ToString(
                matches[0]["notificationType"]);

            string destination = GetNotificationDestination(type);

            if (destination == null)
            {
                ShowMessage(
                    "No destination is configured for this notification.",
                    false);
                return;
            }

            Response.Redirect(destination, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }

    private string GetNotificationDestination(string notificationType)
    {
        switch ((notificationType ?? "").Trim())
        {
            case "EscalationReview":
                return ResolveUrl(
                    "~/Admin/AdminEscalatedGrievances.aspx");

            default:
                return null;
        }
    }

    private void RefreshAdminNotificationBadge()
    {
        Admin_AdminMaster adminMaster =
            Master as Admin_AdminMaster;

        if (adminMaster != null)
        {
            adminMaster.RefreshNotificationBadge();
        }
    }

    private void ShowMessage(string message, bool success)
    {
        lblMessage.Text = message;

        lblMessage.CssClass = success
            ? "admin-notifications-message success"
            : "admin-notifications-message error";
    }
}
