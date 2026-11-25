using System;

public partial class Staff_StaffMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Ensure the Staff member is logged in.
        if (Session["StaffID"] == null)
        {
            Response.Redirect("~/Staff/StaffLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        // Enforce password change when required.
        if (Session["MustChangePassword"] != null &&
            Convert.ToBoolean(Session["MustChangePassword"]))
        {
            string currentPage = System.IO.Path.GetFileName(
                Request.AppRelativeCurrentExecutionFilePath);

            if (!string.Equals(
                currentPage,
                "StaffChangePassword.aspx",
                StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("~/Staff/StaffChangePassword.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
        }

        if (!IsPostBack)
        {
            LoadStaffHeader();
        }
        LoadStaffUnreadNotificationCount();
    }

    private void LoadStaffUnreadNotificationCount()
    {
        int staffID = Convert.ToInt32(Session["StaffID"]);

        int unreadCount = DALSystemNotification.GetUnreadCount(
            "Staff",
            staffID
        );

        string displayCount = unreadCount > 99
            ? "99+"
            : unreadCount.ToString();

        lblStaffUnreadCount.Visible = unreadCount > 0;
        lblStaffSidebarUnreadCount.Visible = unreadCount > 0;

        lblStaffUnreadCount.Text = unreadCount > 0
            ? displayCount
            : string.Empty;

        lblStaffSidebarUnreadCount.Text = unreadCount > 0
            ? displayCount
            : string.Empty;
    }

    private void LoadStaffHeader()
    {
        string staffName = Convert.ToString(Session["StaffName"]);

        if (string.IsNullOrWhiteSpace(staffName))
        {
            staffName = "Staff Member";
        }

        lblStaffName.Text = Server.HtmlEncode(staffName);

        string initials = GetInitials(staffName);
        lblStaffInitial.Text = Server.HtmlEncode(initials);
    }

    private string GetInitials(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "S";
        }

        string[] parts = name.Trim().Split(
            new char[] { ' ' },
            StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 1)
        {
            return parts[0].Substring(0, 1).ToUpperInvariant();
        }

        return (
            parts[0].Substring(0, 1) +
            parts[parts.Length - 1].Substring(0, 1)
        ).ToUpperInvariant();
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();

        Response.Redirect("~/Staff/StaffLogin.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }
}