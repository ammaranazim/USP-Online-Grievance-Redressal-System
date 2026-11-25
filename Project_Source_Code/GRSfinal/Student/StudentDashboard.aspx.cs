using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Student_StudentDashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int stdID;
        int stdLoginID;

        if (!int.TryParse(Convert.ToString(Session["StdID"]), out stdID) ||
            stdID <= 0 ||
            !int.TryParse(Convert.ToString(Session["StdLoginID"]), out stdLoginID) ||
            stdLoginID <= 0)
        {
            Session.Clear();
            Response.Redirect("~/Student/StudentLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        // Prevent access to the dashboard before the mandatory
        // first-login password change is completed.
        bool? mustChangePassword =
         DALStudentLogin.GetMustChangePassword(stdLoginID, stdID);

        if (!mustChangePassword.HasValue)
        {
            Session.Clear();
            Response.Redirect("~/Student/StudentLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (mustChangePassword.Value)
        {
            Session["MustChangePassword"] = true;

            Response.Redirect("~/Student/StudentChangePassword.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        Session["MustChangePassword"] = false;

        if (!IsPostBack)
        {
            LoadStudentInfo();
            SetActiveView();

            if (mvStudentDashboard.ActiveViewIndex == 0)
            {
                LoadGrievanceStatistics();
                LoadRecentGrievances();
            }

            if (mvStudentDashboard.ActiveViewIndex == 1)
            {
                LoadStudentGrievances();
            }
        }

        if (mvStudentDashboard.ActiveViewIndex == 2)
        {
            LoadStudentNotifications();
        }
    }



    private void LoadStudentInfo()
    {
        int stdID = Convert.ToInt32(Session["StdID"]);

        MStudent student = DALStudent.GetStudentByID(stdID);

        if (student != null)
        {
            // Student Name
            string fullName =
                student.StdFirstName + " " + student.StdLastName;

            lblProfileName.Text = Server.HtmlEncode(fullName.Trim());

            // Registration Number
            lblProfileRegNo.Text = Server.HtmlEncode(student.StdRegNo ?? "");

            // Initials
            string firstInitial = "";
            string lastInitial = "";

            if (!string.IsNullOrWhiteSpace(student.StdFirstName))
            {
                firstInitial =
                    student.StdFirstName.Substring(0, 1).ToUpper();
            }

            if (!string.IsNullOrWhiteSpace(student.StdLastName))
            {
                lastInitial =
                    student.StdLastName.Substring(0, 1).ToUpper();
            }

            lblProfileInitials.Text = Server.HtmlEncode(firstInitial + lastInitial);

            // Email
            lblProfileEmail.Text = Server.HtmlEncode(student.StdEmail ?? "");

            // Program
            lblProfileProgram.Text = Server.HtmlEncode(student.StdProgram ?? "");
            // Semester
            lblProfileSemester.Text = Server.HtmlEncode(student.StdSemester ?? "");

            // Department
            MDepartment department =
                DALDepartment.GetDepartmentByID(student.DeptID);

            if (department != null)
            {
                lblProfileDepartment.Text = Server.HtmlEncode(department.DeptName ?? "");
            }
            else
            {
                lblProfileDepartment.Text = "N/A";
            }
        }
    }
    private void SetActiveView()
    {
        string view = Request.QueryString["view"];

        if (string.IsNullOrEmpty(view))
        {
            mvStudentDashboard.ActiveViewIndex = 0;
            return;
        }


        switch (view)
        {
            case "Overview":
                mvStudentDashboard.ActiveViewIndex = 0;
                break;

            case "TrackGrievance":
                mvStudentDashboard.ActiveViewIndex = 1;
                break;

            case "Notifications":
                mvStudentDashboard.ActiveViewIndex = 2;
                break;

            case "Profile":
                mvStudentDashboard.ActiveViewIndex = 3;
                break;

            default:
                mvStudentDashboard.ActiveViewIndex = 0;
                break;
        }
    }
    private void LoadStudentGrievances()
    {
        BindStudentGrievances();
    }

    private void LoadStudentNotifications()
    {
        int stdID = Convert.ToInt32(Session["StdID"]);

        DataTable notifications =
            DALSystemNotification.GetNotifications("Student", stdID);

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

        lblUnreadCount.Text =
            notifications.Rows.Count + " total · " +
            unreadCount + " unread";
    }
    protected void rptNotifications_ItemCommand(
    object source,
    RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "MarkAsRead")
            return;

        int notificationID;

        if (!int.TryParse(
            Convert.ToString(e.CommandArgument),
            out notificationID))
        {
            return;
        }

        int stdID = Convert.ToInt32(Session["StdID"]);

        int result = DALSystemNotification.MarkAsRead(
            notificationID,
            "Student",
            stdID);

        if (result > 0)
        {
            LoadStudentNotifications();

            // Refresh the notification badges in the master page.
            Student_StudentMaster studentMaster =
                Master as Student_StudentMaster;

            if (studentMaster != null)
            {
                studentMaster.RefreshNotificationBadge();
            }
        }
    }

    private void LoadGrievanceStatistics()
    {
        int stdID = Convert.ToInt32(Session["StdID"]);

        DataTable dt =
            DALGrievance.GetGrievanceStatisticsByStudentID(stdID);

        if (dt != null && dt.Rows.Count > 0)
        {
            lblTotal.Text =
                dt.Rows[0]["TotalGrievances"].ToString();

            lblPending.Text =
                dt.Rows[0]["PendingGrievances"].ToString();

            lblInProcess.Text =
                dt.Rows[0]["InProcessGrievances"].ToString();

            lblResolved.Text =
                dt.Rows[0]["ResolvedGrievances"].ToString();
        }
    }
    private void LoadRecentGrievances()
    {
        int stdID = Convert.ToInt32(Session["StdID"]);

        DataTable dt = DALGrievance.GetGrievancesByStudentID(stdID);

        if (dt.Rows.Count > 5)
        {
            dt = dt.AsEnumerable()
                     .Take(5)
                     .CopyToDataTable();
        }

        gvRecentGrievances.DataSource = dt;
        gvRecentGrievances.DataBind();
    }

    private const int GrievancesPageSize = 10;

    private string GrievanceSearch
    {
        get
        {
            return Convert.ToString(
                ViewState["GrievanceSearch"]) ?? string.Empty;
        }
        set
        {
            ViewState["GrievanceSearch"] = value;
        }
    }

    private string GrievanceStatusFilter
    {
        get
        {
            return Convert.ToString(
                ViewState["GrievanceStatusFilter"]) ?? string.Empty;
        }
        set
        {
            ViewState["GrievanceStatusFilter"] = value;
        }
    }

    private void BindStudentGrievances()
    {
        int stdID = Convert.ToInt32(Session["StdID"]);

        DataTable dt = DALGrievance.GetGrievancesByStudentID(stdID);

        if (dt == null)
        {
            dt = new DataTable();
        }

        // Search only within the logged-in student's grievances.
        string search = GrievanceSearch.Trim();
        string status = GrievanceStatusFilter.Trim();

        DataTable filtered = dt.Clone();

        foreach (DataRow row in dt.Rows)
        {
            string grievanceID =
                Convert.ToString(row["grievanceID"]);

            string title =
                Convert.ToString(row["grievanceTitle"]);

            string grievanceStatus =
                Convert.ToString(row["grievanceStatus"]);

            bool matchesSearch =
                string.IsNullOrWhiteSpace(search) ||
                grievanceID.IndexOf(
                    search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf(
                    search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                ("GRV-" + grievanceID).IndexOf(
                    search, StringComparison.OrdinalIgnoreCase) >= 0;

            bool matchesStatus =
                string.IsNullOrWhiteSpace(status) ||
                string.Equals(
                    grievanceStatus,
                    status,
                    StringComparison.OrdinalIgnoreCase);

            if (matchesSearch && matchesStatus)
            {
                filtered.ImportRow(row);
            }
        }

        lblGrievanceCount.Text =
            filtered.Rows.Count + " Records";

        gvAllGrievances.PageSize = GrievancesPageSize;

        int totalPages = Math.Max(
            1,
            (int)Math.Ceiling(
                filtered.Rows.Count / (double)GrievancesPageSize));

        if (gvAllGrievances.PageIndex >= totalPages)
        {
            gvAllGrievances.PageIndex = totalPages - 1;
        }

        gvAllGrievances.DataSource = filtered;
        gvAllGrievances.DataBind();
    }

    protected void btnGrievanceSearch_Click(
        object sender, EventArgs e)
    {
        GrievanceSearch = txtGrievanceSearch.Text.Trim();
        GrievanceStatusFilter = ddlGrievanceStatus.SelectedValue;

        gvAllGrievances.PageIndex = 0;

        BindStudentGrievances();
    }

    protected void btnGrievanceClear_Click(
        object sender, EventArgs e)
    {
        txtGrievanceSearch.Text = string.Empty;
        ddlGrievanceStatus.SelectedIndex = 0;

        GrievanceSearch = string.Empty;
        GrievanceStatusFilter = string.Empty;

        gvAllGrievances.PageIndex = 0;
        pnlGrievanceFilters.Visible = false;

        BindStudentGrievances();
    }

    protected void btnGrievanceFilters_Click(
        object sender, EventArgs e)
    {
        pnlGrievanceFilters.Visible =
            !pnlGrievanceFilters.Visible;

        // Preserve the current search and status selections.
        txtGrievanceSearch.Text = GrievanceSearch;

        string savedStatus = GrievanceStatusFilter;

        if (ddlGrievanceStatus.Items.FindByValue(savedStatus) != null)
        {
            ddlGrievanceStatus.SelectedValue = savedStatus;
        }

        BindStudentGrievances();
    }

    protected void gvAllGrievances_PageIndexChanging(
        object sender, GridViewPageEventArgs e)
    {
        gvAllGrievances.PageIndex = e.NewPageIndex;

        BindStudentGrievances();
    }

}