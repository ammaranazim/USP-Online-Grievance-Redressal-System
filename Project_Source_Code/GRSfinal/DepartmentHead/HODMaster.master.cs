
using System;
using System.IO;
using System.Web.UI;

public partial class DepartmentHead_HODMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Only authenticated Department Heads may access protected pages.
        if (Session["DeptHeadID"] == null)
        {
            RedirectToLogin();
            return;
        }

        string currentPage = Path.GetFileName(
            Request.Url.AbsolutePath
        );

        bool isChangePasswordPage = string.Equals(
            currentPage,
            "HODChangePassword.aspx",
            StringComparison.OrdinalIgnoreCase
        );

        // Preserve mandatory first-login password change.
        if (!isChangePasswordPage &&
            Session["MustChangePassword"] != null &&
            Convert.ToBoolean(Session["MustChangePassword"]))
        {
            Response.Redirect(
                "~/DepartmentHead/HODChangePassword.aspx",
                false
            );

            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (!IsPostBack)
        {
            string hodName = Convert.ToString(
                Session["DeptHeadName"]
            );

            if (string.IsNullOrWhiteSpace(hodName))
            {
                hodName = "Department Head";
            }

            hodName = hodName.Trim();

            lblHodName.Text = hodName;
            lblUserInitials.Text =
                hodName.Substring(0, 1).ToUpperInvariant();
        }

        // Refresh unread notification count on every request.
        LoadHODUnreadNotificationCount();
    }

    // Returns active class only for the current page.
    protected string GetNavActiveClass(string pageName)
    {
        string currentPage = Path.GetFileName(
            Request.Url.AbsolutePath
        );

        return string.Equals(
            currentPage,
            pageName,
            StringComparison.OrdinalIgnoreCase
        )
            ? "active"
            : string.Empty;
    }

    private void LoadHODUnreadNotificationCount()
    {
        int hodID = Convert.ToInt32(Session["DeptHeadID"]);

        int unreadCount = DALSystemNotification.GetUnreadCount(
            "HOD",
            hodID
        );

        string displayCount = unreadCount > 99
            ? "99+"
            : unreadCount.ToString();

        lblHODUnreadCount.Visible = unreadCount > 0;
        lblHODSidebarUnreadCount.Visible = unreadCount > 0;

        if (unreadCount > 0)
        {
            lblHODUnreadCount.Text = displayCount;
            lblHODSidebarUnreadCount.Text = displayCount;
        }
        else
        {
            lblHODUnreadCount.Text = string.Empty;
            lblHODSidebarUnreadCount.Text = string.Empty;
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();

        RedirectToLogin();
    }

    private void RedirectToLogin()
    {
        Response.Redirect(
            "~/DepartmentHead/HODLogin.aspx",
            false
        );

        Context.ApplicationInstance.CompleteRequest();
    }
}
