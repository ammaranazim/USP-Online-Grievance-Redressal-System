using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;

public partial class DepartmentHead_HODStaffAssignment : System.Web.UI.Page
{
    private int GrievanceID
    {
        get
        {
            int id;
            return int.TryParse(Request.QueryString["id"], out id) ? id : 0;
        }
    }

    private int DepartmentID
    {
        get
        {
            return Convert.ToInt32(Session["DeptID"]);
        }
    }

    private int DepartmentHeadID
    {
        get
        {
            return Convert.ToInt32(Session["DeptHeadID"]);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["DeptHeadID"] == null ||
            Session["DeptID"] == null)
        {
            Response.Redirect("~/DepartmentHead/HODLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }


        if (!IsPostBack)
        {
            if (GrievanceID <= 0)
            {
                ShowInvalidGrievance();
                return;
            }

            LoadGrievanceDetails();
        }
    }

    private void LoadGrievanceDetails()
    {
        DataTable dt = DALGrievance.GetHODGrievanceDetailsByID(
            GrievanceID, DepartmentID);

        if (dt == null || dt.Rows.Count == 0)
        {
            ShowInvalidGrievance();
            return;
        }

        DataRow row = dt.Rows[0];

        pnlInvalidGrievance.Visible = false;
        pnlAssignmentContent.Visible = true;

        lblGrievanceID.Text = "GRV-" + Convert.ToString(row["grievanceID"]);
        lblGrievanceTitle.Text = SafeText(row["grievanceTitle"]);
        lblStudentName.Text = SafeText(row["StudentName"]);
        lblRegistrationNo.Text = SafeText(row["stdRegNo"]);
        lblCategory.Text = SafeText(row["categoryName"]);
        lblDepartment.Text = SafeText(row["deptName"]);
        lblDescription.Text = SafeText(row["grievanceDescription"]);
        lblPriority.Text = SafeText(row["grievancePriority"]);
        string status = Convert.ToString(row["grievanceStatus"]);

        lblGrievanceStatus.Text = SafeText(status);

        switch (status.Trim().ToLowerInvariant())
        {
            case "pending":
                lblGrievanceStatus.CssClass =
                    "hod-assignment-status status-pending";
                break;

            case "in process":
                lblGrievanceStatus.CssClass =
                    "hod-assignment-status status-in-process";
                break;

            case "resolved":
                lblGrievanceStatus.CssClass =
                    "hod-assignment-status status-resolved";
                break;

            case "rejected":
                lblGrievanceStatus.CssClass =
                    "hod-assignment-status status-rejected";
                break;

            default:
                lblGrievanceStatus.CssClass = "hod-assignment-status";
                break;
        }

        if (row["grievanceSubmissionDate"] != DBNull.Value)
        {
            lblSubmissionDate.Text = Convert.ToDateTime(
                row["grievanceSubmissionDate"]).ToString("dd MMM yyyy, hh:mm tt");
        }
        else
        {
            lblSubmissionDate.Text = "—";
        }
        
        lnkBackToDetails.NavigateUrl =
            ResolveUrl("~/DepartmentHead/HODGrievanceDetails.aspx?id=" + GrievanceID);

        lnkCancelAssignment.NavigateUrl =
            ResolveUrl("~/DepartmentHead/HODGrievanceDetails.aspx?id=" + GrievanceID);

        LoadExistingAssignment();
        LoadStaffDropdown();
    }

    private void LoadExistingAssignment()
    {
        DataTable dt = DALGrievance.GetHODAssignmentDetails(
            GrievanceID, DepartmentID);

        pnlExistingAssignment.Visible = false;

        if (dt == null || dt.Rows.Count == 0)
        {
            return;
        }

        DataRow row = dt.Rows[0];

        pnlExistingAssignment.Visible = true;
        lblAssignedStaffName.Text = SafeText(row["staffName"]);
        lblAssignedStaffDesignation.Text = SafeText(row["staffDesignation"]);
        lblAssignmentStatus.Text = SafeText(row["assignmentStatus"]);

        if (row["assignedDate"] != DBNull.Value)
        {
            lblAssignedDate.Text = Convert.ToDateTime(
                row["assignedDate"]).ToString("dd MMM yyyy, hh:mm tt");
        }
        else
        {
            lblAssignedDate.Text = "—";
        }

        // A currently assigned Grievance must not be assigned a second time.
        if (string.Equals(
            Convert.ToString(row["assignmentStatus"]),
            "Assigned",
            StringComparison.OrdinalIgnoreCase))
        {
            pnlStaffForm.Visible = false;
        }
    }

    private void LoadStaffDropdown()
    {
        DataTable staff = DALGrievance.GetActiveStaffByDepartment(DepartmentID);

        ddlStaff.Items.Clear();
        ddlStaff.Items.Add(new ListItem("-- Select Staff Member --", ""));

        if (staff == null || staff.Rows.Count == 0)
        {
            pnlNoStaff.Visible = true;
            pnlStaffForm.Visible = false;
            return;
        }

        pnlNoStaff.Visible = false;

        foreach (DataRow row in staff.Rows)
        {
            string name = SafeText(row["staffName"]);
            string designation = SafeText(row["staffDesignation"]);

            string displayText = string.IsNullOrWhiteSpace(designation)
                ? name
                : name + " — " + designation;

            ddlStaff.Items.Add(new ListItem(
                displayText,
                Convert.ToString(row["staffID"])));
        }

        // Don't show the assignment form if there is already an active assignment.
        DataTable current = DALGrievance.GetHODAssignmentDetails(
            GrievanceID, DepartmentID);

        bool alreadyAssigned = current != null &&
            current.Rows.Count > 0 &&
            string.Equals(
                Convert.ToString(current.Rows[0]["assignmentStatus"]),
                "Assigned",
                StringComparison.OrdinalIgnoreCase);

        pnlStaffForm.Visible = !alreadyAssigned;
    }

    protected void btnAssignStaff_Click(object sender, EventArgs e)
    {
        pnlMessage.Visible = false;

        if (!Page.IsValid)
        {
            return;
        }

        int staffID;

        if (!int.TryParse(ddlStaff.SelectedValue, out staffID) || staffID <= 0)
        {
            ShowMessage("Please select a valid staff member.", false);
            return;
        }

        // Re-check department ownership on the server before inserting.
        DataTable grievance = DALGrievance.GetHODGrievanceDetailsByID(
            GrievanceID, DepartmentID);

        if (grievance == null || grievance.Rows.Count == 0)
        {
            ShowInvalidGrievance();
            return;
        }

        try
        {
            int result = DALGrievance.AssignHODGrievanceToStaff(
                GrievanceID,
                staffID,
                DepartmentHeadID,
                DepartmentID);

            if (result == 1)
            {
                string grievanceTitle = Convert.ToString(
                    grievance.Rows[0]["grievanceTitle"]);

                int stdID = Convert.ToInt32(
                    grievance.Rows[0]["stdID"]);

                // =========================================================
                // 1. EXISTING STAFF SYSTEM NOTIFICATION
                // =========================================================
                try
                {
                    DALSystemNotification.InsertNotification(
                        "Staff",
                        staffID,
                        GrievanceID,
                        "Grievance Assigned",
                        "Grievance (ID: " + GrievanceID +
                        ") - " + grievanceTitle +
                        " has been assigned to you by your Department Head.");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(
                        "Failed to create Staff notification for Grievance ID " +
                        GrievanceID + ": " + ex);
                }

                // =========================================================
                // 2. EXISTING STUDENT SYSTEM NOTIFICATION
                // =========================================================
                try
                {
                    DALSystemNotification.InsertNotification(
                        "Student",
                        stdID,
                        GrievanceID,
                        "Grievance In Process",
                        "Your Grievance #" + GrievanceID +
                        " is now In Process.");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(
                        "Failed to create Student status notification for Grievance ID " +
                        GrievanceID + ": " + ex);
                }

                // =========================================================
                // 3. EMAIL NOTIFICATION TO STAFF
                // =========================================================
                try
                {
                    string staffEmail = GetStaffEmail(staffID);
                    string staffName = GetStaffName(staffID);

                    if (!string.IsNullOrWhiteSpace(staffEmail))
                    {
                        string subject =
                            "New Grievance Assigned - GRV-" + GrievanceID;

                        string body =
                            "<html>" +
                            "<body style='font-family:Arial,sans-serif;'>" +

                            "<h2 style='color:#123a7a;'>USP Grievance Portal</h2>" +

                            "<p>Dear " + Server.HtmlEncode(staffName) + ",</p>" +

                            "<p>A new Grievance has been assigned to you by your Department Head.</p>" +

                            "<table cellpadding='6' cellspacing='0' border='0'>" +

                            "<tr>" +
                            "<td><strong>Grievance ID</strong></td>" +
                            "<td>GRV-" + GrievanceID + "</td>" +
                            "</tr>" +

                            "<tr>" +
                            "<td><strong>Title</strong></td>" +
                            "<td>" + Server.HtmlEncode(grievanceTitle) + "</td>" +
                            "</tr>" +

                            "<tr>" +
                            "<td><strong>Status</strong></td>" +
                            "<td>In Process</td>" +
                            "</tr>" +

                            "</table>" +

                            "<p>Please log in to the Staff Portal to review and process the Grievance.</p>" +

                            "<p>Regards,<br/>" +
                            "USP Online Grievance Redressal System</p>" +

                            "</body>" +
                            "</html>";

                        EmailService.SendEmail(
                            staffEmail,
                            subject,
                            body);
                    }
                }
                catch (Exception ex)
                {
                    // Email failure must NOT undo a successful assignment.
                    System.Diagnostics.Trace.TraceError(
                        "Failed to send Staff assignment email for Grievance ID " +
                        GrievanceID + ": " + ex);
                }

                // =========================================================
                // 4. EMAIL NOTIFICATION TO STUDENT
                // =========================================================
                try
                {
                    string studentEmail = GetStudentEmail(stdID);
                    string studentName = GetStudentName(stdID);

                    if (!string.IsNullOrWhiteSpace(studentEmail))
                    {
                        string subject =
                            "Grievance Status Updated - GRV-" + GrievanceID;

                        string body =
                            "<html>" +
                            "<body style='font-family:Arial,sans-serif;'>" +

                            "<h2 style='color:#123a7a;'>USP Grievance Portal</h2>" +

                            "<p>Dear " + Server.HtmlEncode(studentName) + ",</p>" +

                            "<p>Your Grievance has been reviewed by the Department Head and assigned to a staff member for further processing.</p>" +

                            "<table cellpadding='6' cellspacing='0' border='0'>" +

                            "<tr>" +
                            "<td><strong>Grievance ID</strong></td>" +
                            "<td>GRV-" + GrievanceID + "</td>" +
                            "</tr>" +

                            "<tr>" +
                            "<td><strong>Title</strong></td>" +
                            "<td>" + Server.HtmlEncode(grievanceTitle) + "</td>" +
                            "</tr>" +

                            "<tr>" +
                            "<td><strong>Current Status</strong></td>" +
                            "<td>In Process</td>" +
                            "</tr>" +

                            "</table>" +

                            "<p>You can log in to the Student Portal to track the progress of your Grievance.</p>" +

                            "<p>Regards,<br/>" +
                            "USP Online Grievance Redressal System</p>" +

                            "</body>" +
                            "</html>";

                        EmailService.SendEmail(
                            studentEmail,
                            subject,
                            body);
                    }
                }
                catch (Exception ex)
                {
                    // Email failure must NOT undo a successful assignment.
                    System.Diagnostics.Trace.TraceError(
                        "Failed to send Student status email for Grievance ID " +
                        GrievanceID + ": " + ex);
                }

                // =========================================================
                // 5. SUCCESS MESSAGE
                // =========================================================
                ShowMessage(
                    "Grievance has been assigned to the selected staff member successfully.",
                    true);

                LoadGrievanceDetails();
            }
            else if (result == 2)
            {
                ShowMessage(
                    "This Grievance already has an active staff assignment. It cannot be assigned again from this page.",
                    false);

                LoadGrievanceDetails();
            }
            else
            {
                ShowMessage(
                    "Assignment could not be completed. Please verify the Grievance and staff member.",
                    false);

                LoadGrievanceDetails();
            }
        }
        catch (Exception)
        {
            ShowMessage(
                "An unexpected error occurred while saving the assignment. Please try again.",
                false);
        }
    }
    private string GetStaffEmail(int staffID)
    {
        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(@"
        SELECT staffEmail
        FROM dbo.Staff
        WHERE staffID = @staffID;", con))
        {
            cmd.Parameters.Add("@staffID", SqlDbType.Int).Value = staffID;

            con.Open();

            object result = cmd.ExecuteScalar();

            return result == null || result == DBNull.Value
                ? ""
                : Convert.ToString(result).Trim();
        }
    }


    private string GetStaffName(int staffID)
    {
        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(@"
        SELECT staffName
        FROM dbo.Staff
        WHERE staffID = @staffID;", con))
        {
            cmd.Parameters.Add("@staffID", SqlDbType.Int).Value = staffID;

            con.Open();

            object result = cmd.ExecuteScalar();

            return result == null || result == DBNull.Value
                ? "Staff Member"
                : Convert.ToString(result).Trim();
        }
    }


    private string GetStudentEmail(int stdID)
    {
        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(@"
        SELECT stdEmail
        FROM dbo.StudentInfo
        WHERE stdID = @stdID;", con))
        {
            cmd.Parameters.Add("@stdID", SqlDbType.Int).Value = stdID;

            con.Open();

            object result = cmd.ExecuteScalar();

            return result == null || result == DBNull.Value
                ? ""
                : Convert.ToString(result).Trim();
        }
    }


    private string GetStudentName(int stdID)
    {
        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(@"
        SELECT LTRIM(RTRIM(
                   ISNULL(stdFirstName, '') + ' ' +
                   ISNULL(stdLastName, '')
               ))
        FROM dbo.StudentInfo
        WHERE stdID = @stdID;", con))
        {
            cmd.Parameters.Add("@stdID", SqlDbType.Int).Value = stdID;

            con.Open();

            object result = cmd.ExecuteScalar();

            return result == null || result == DBNull.Value ||
                   string.IsNullOrWhiteSpace(Convert.ToString(result))
                ? "Student"
                : Convert.ToString(result).Trim();
        }
    }

    private void ShowInvalidGrievance()
    {
        pnlAssignmentContent.Visible = false;
        pnlInvalidGrievance.Visible = true;
    }

    private void ShowMessage(string message, bool success)
    {
        pnlMessage.Visible = true;
        lblMessage.Text = message;

        pnlMessage.CssClass = success
            ? "hod-assignment-message success"
            : "hod-assignment-message error";
    }

    private string SafeText(object value)
    {
        if (value == null || value == DBNull.Value)
        {
            return "—";
        }

        string text = Convert.ToString(value);

        return string.IsNullOrWhiteSpace(text) ? "—" : text;
    }
}