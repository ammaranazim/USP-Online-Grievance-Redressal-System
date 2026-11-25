using System;
using System.Web.UI;

public partial class Admin_AdminMaster : System.Web.UI.MasterPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        // Check admin login
        if (Session["AdminID"] == null)
        {
            Response.Redirect("~/Admin/AdminLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (!IsPostBack)
        {
            LoadAdminInformation();
        }

        LoadAdminUnreadCount();
    }


    public void RefreshNotificationBadge()
    {
        LoadAdminUnreadCount();
    }


    private void LoadAdminInformation()
    {
        // Display admin name
        if (Session["AdminName"] != null)
        {
            string adminName = Session["AdminName"].ToString().Trim();

            lblAdminName.Text = adminName;

            if (!string.IsNullOrEmpty(adminName))
            {
                lblAdminInitial.Text = adminName.Substring(0, 1).ToUpper();
            }
        }
    }


    protected void btnLogout_Click(object sender, EventArgs e)
    {
        // Clear admin session
        Session.Remove("AdminID");
        Session.Remove("AdminName");
        Session.Remove("AdminEmail");

        // Redirect to admin login
        Response.Redirect(
            "~/Admin/AdminLogin.aspx",
            false);

        Context.ApplicationInstance.CompleteRequest();
    }

    private void LoadAdminUnreadCount()
    {
        if (Session["AdminID"] == null)
        {
            lblAdminUnreadCount.Visible = false;
            return;
        }

        int adminID = Convert.ToInt32(Session["AdminID"]);

        int unreadCount = DALSystemNotification.GetUnreadCount(
            "Admin", adminID);

        lblAdminUnreadCount.Visible = unreadCount > 0;
        lblAdminUnreadCount.Text = unreadCount > 99
            ? "99+"
            : unreadCount.ToString();
    }
   
}