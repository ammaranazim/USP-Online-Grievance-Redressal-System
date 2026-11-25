
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Staff_StaffNotifications : System.Web.UI.Page
{
    private int StaffID
    {
        get
        {
            if (Session["StaffID"] == null)
            {
                return 0;
            }

            return Convert.ToInt32(Session["StaffID"]);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (StaffID <= 0)
        {
            Response.Redirect("~/Staff/StaffLogin.aspx", false);
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
            DALSystemNotification.GetNotifications("Staff", StaffID);

        rptNotifications.DataSource = notifications;
        rptNotifications.DataBind();

        pnlNoNotifications.Visible = notifications.Rows.Count == 0;

        int unreadCount =
            DALSystemNotification.GetUnreadCount("Staff", StaffID);

        lblNotificationSummary.Text =
            notifications.Rows.Count + " total · " +
            unreadCount + " unread";
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
            "Staff",
            StaffID
        );

        lblNotificationMessage.Text = result > 0
            ? "Notification marked as read."
            : "Notification could not be updated. It may already be read.";

        LoadNotifications();
    }
}
