using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_StudentMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int stdID;

        if (Session["StdID"] == null ||
            !int.TryParse(Session["StdID"].ToString(), out stdID) ||
            stdID <= 0)
        {
            Session.Clear();
            Response.Redirect("~/Student/StudentLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (!IsPostBack)
        {
            LoadStudentInfo();
            HighlightActiveNav();
           
        }
       RefreshNotificationBadge();
    }


    private void LoadStudentInfo()
    {
        // Student Name
        if (Session["StdName"] != null)
        {
            string studentName = Session["StdName"].ToString();

            lblStdName.Text = Server.HtmlEncode(studentName);

            // First two letters for avatar
            string[] nameParts = studentName.Trim().Split(' ');

            if (nameParts.Length >= 2)
            {
                lblAvatarInitials.Text =
                    nameParts[0].Substring(0, 1).ToUpper() +
                    nameParts[1].Substring(0, 1).ToUpper();
            }
            else if (studentName.Length >= 2)
            {
                lblAvatarInitials.Text =
                    studentName.Substring(0, 2).ToUpper();
            }
            else
            {
                lblAvatarInitials.Text = studentName.ToUpper();
            }
        }
    }


    private void HighlightActiveNav()
    {
        string currentPage =
            System.IO.Path.GetFileName(
                Request.Url.AbsolutePath);

        string view =
            Request.QueryString["view"];

        // Reset all navigation links
        lnkOverview.CssClass = "nav-link";
        lnkSubmit.CssClass = "nav-link";
        lnkTrack.CssClass = "nav-link";
        lnkNotifications.CssClass = "nav-link";
        lnkProfile.CssClass = "nav-link";
        // Change Password page: do not highlight Dashboard navigation
        if (currentPage.Equals(
            "StudentChangePassword.aspx",
            StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        // Grievance Details belongs to Track Grievance
        if (currentPage.Equals(
            "GrievanceDetails.aspx",
            StringComparison.OrdinalIgnoreCase))
        {
            lnkTrack.CssClass = "nav-link active";
            return;
        }


        // Other pages use the dashboard view
        if (string.IsNullOrEmpty(view))
        {
            view = "Overview";
        }


        switch (view)
        {
            case "SubmitGrievance":
                lnkSubmit.CssClass = "nav-link active";
                break;

            case "TrackGrievance":
                lnkTrack.CssClass = "nav-link active";
                break;

            case "Notifications":
                lnkNotifications.CssClass = "nav-link active";
                break;

            case "Profile":
                lnkProfile.CssClass = "nav-link active";
                break;

            default:
                lnkOverview.CssClass = "nav-link active";
                break;
        }
    }

    public void RefreshNotificationBadge()
    {
        try
        {
            if (Session["StdID"] == null)
            {
                lblNavBadge.Visible = false;
                lblBellDot.Visible = false;
                return;
            }

            int stdID;

            if (!int.TryParse(
                Convert.ToString(Session["StdID"]),
                out stdID) || stdID <= 0)
            {
                lblNavBadge.Visible = false;
                lblBellDot.Visible = false;
                return;
            }

            int unreadCount =
                DALSystemNotification.GetUnreadCount("Student", stdID);

            if (unreadCount > 0)
            {
                string displayCount = unreadCount > 99
                    ? "99+"
                    : unreadCount.ToString();

                lblNavBadge.Text = displayCount;
                lblNavBadge.Visible = true;

                lblBellDot.Text = displayCount;
                lblBellDot.Visible = true;
            }
            else
            {
                lblNavBadge.Text = string.Empty;
                lblBellDot.Text = string.Empty;

                lblNavBadge.Visible = false;
                lblBellDot.Visible = false;
            }
        }
        catch
        {
            lblNavBadge.Visible = false;
            lblBellDot.Visible = false;
        }
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();

        Response.Redirect("~/Student/StudentLogin.aspx", false);
    }
}