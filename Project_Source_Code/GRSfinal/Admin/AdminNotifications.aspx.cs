
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AdminNotifications : System.Web.UI.Page

{
    private int AdminID
    {
        get
        {
            if (Session["AdminID"] == null)
            {
                return 0;
            }

            return Convert.ToInt32(Session["AdminID"]);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (AdminID <= 0)
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
        DataTable notifications =
            DALSystemNotification.GetNotifications("Admin", AdminID);

        rptNotifications.DataSource = notifications;
        rptNotifications.DataBind();

        pnlNoNotifications.Visible = notifications.Rows.Count == 0;

        int unreadCount =
            DALSystemNotification.GetUnreadCount("Admin", AdminID);

        lblNotificationSummary.Text =
            notifications.Rows.Count + " total · " +
            unreadCount + " unread";
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


    protected void rptNotifications_ItemCommand(
        object source,
        RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "MarkAsRead")
        {
            return;
        }

        int notificationID;

        if (!int.TryParse(
            Convert.ToString(e.CommandArgument),
            out notificationID))
        {
            lblNotificationMessage.Text =
                "Invalid notification selection.";

            LoadNotifications();
            return;
        }

        int result = DALSystemNotification.MarkAsRead(
            notificationID,
            "Admin",
            AdminID
        );

        lblNotificationMessage.Text = result > 0
            ? "Notification marked as read."
            : "Notification could not be updated. It may already be read.";

        LoadNotifications();
        RefreshAdminNotificationBadge();
    }
}
