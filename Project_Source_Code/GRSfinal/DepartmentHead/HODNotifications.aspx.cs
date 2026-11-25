
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DepartmentHead_HODNotifications : System.Web.UI.Page
{
    private int HODID
    {
        get
        {
            if (Session["DeptHeadID"] == null)
            {
                return 0;
            }

            return Convert.ToInt32(Session["DeptHeadID"]);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (HODID <= 0)
        {
            Response.Redirect(
                "~/DepartmentHead/HODLogin.aspx",
                false
            );

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
            DALSystemNotification.GetNotifications("HOD", HODID);

        rptNotifications.DataSource = notifications;
        rptNotifications.DataBind();

        pnlNoNotifications.Visible = notifications.Rows.Count == 0;

        int unreadCount =
            DALSystemNotification.GetUnreadCount("HOD", HODID);

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
            "HOD",
            HODID
        );

        if (result > 0)
        {
            lblNotificationMessage.Text =
                "Notification marked as read.";
        }
        else
        {
            lblNotificationMessage.Text =
                "Notification could not be updated. It may already be read.";
        }

        LoadNotifications();
    }
}
