
using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Data.SqlClient;

public partial class DepartmentHead_HODGrievanceDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int deptID;
        int grievanceID;

        // Validate HOD session
        if (!int.TryParse(Convert.ToString(Session["DeptID"]), out deptID))
        {
            Response.Redirect("~/DepartmentHead/HODLogin.aspx", true);
            return;
        }

        // Validate grievance ID in URL
        if (!int.TryParse(Request.QueryString["id"], out grievanceID)
            || grievanceID <= 0)
        {
            Response.Redirect("~/DepartmentHead/HODGrievances.aspx", true);
            return;
        }
        if (!IsPostBack)
        {
            LoadGrievanceDetails(grievanceID, deptID);
        }
    }

    private void LoadGrievanceDetails(int grievanceID, int deptID)
    {
        // Fetch only a grievance belonging to this HOD's department
        DataTable dt =
            DALGrievance.GetHODGrievanceDetailsByID(grievanceID, deptID);

        // Also handles an ID belonging to another department
        if (dt == null || dt.Rows.Count == 0)
        {
            Response.Redirect("~/DepartmentHead/HODGrievances.aspx", true);
            return;
        }

        DataRow row = dt.Rows[0];

        lblGrievanceID.Text =
            "GRV-" + Convert.ToString(row["grievanceID"]);

        lblStudentName.Text =
            Server.HtmlEncode(Convert.ToString(row["StudentName"]));

        lblRegistrationNo.Text =
            Server.HtmlEncode(Convert.ToString(row["stdRegNo"]));

        lblCategory.Text =
            Server.HtmlEncode(Convert.ToString(row["categoryName"]));

        lblDepartment.Text =
            Server.HtmlEncode(Convert.ToString(row["deptName"]));

        lblGrievanceTitle.Text =
            Server.HtmlEncode(Convert.ToString(row["grievanceTitle"]));

        lblDescription.Text =
            Server.HtmlEncode(Convert.ToString(row["grievanceDescription"]));

        // Submission date
        DateTime submissionDate = DateTime.MinValue;

        bool hasSubmissionDate =
            row["grievanceSubmissionDate"] != DBNull.Value &&
            DateTime.TryParse(
                Convert.ToString(row["grievanceSubmissionDate"]),
                out submissionDate);

        if (hasSubmissionDate)
        {
            lblSubmissionDate.Text =
                submissionDate.ToString("dd MMM yyyy");
        }
        else
        {
            lblSubmissionDate.Text = "Not available";
        }


        string status =
            Convert.ToString(row["grievanceStatus"]).Trim();

        string priority =
            Convert.ToString(row["grievancePriority"]).Trim();
        // Control available actions according to grievance status.
        bool isPending = status.Equals(
            "Pending", StringComparison.OrdinalIgnoreCase);

        bool isClosed =
            status.Equals("Resolved", StringComparison.OrdinalIgnoreCase) ||
            status.Equals("Rejected", StringComparison.OrdinalIgnoreCase);
        pnlRejectGrievance.Visible = isPending;

        // Rejection is available only for Pending grievances.
        pnlRejectionMessage.Visible = false;
        txtRejectionReason.Visible = isPending;
        btnRejectGrievance.Visible = isPending;

        // Priority can only be changed while the grievance is active.
        ddlGrievancePriority.Enabled = !isClosed;
        btnSavePriority.Enabled = !isClosed;

        // Existing status and priority badges
        lblStatus.Text = Server.HtmlEncode(status);
        lblStatus.CssClass = GetStatusBadgeClass(status);

        lblPriority.Text = Server.HtmlEncode(priority);
        lblPriority.CssClass = GetPriorityBadgeClass(priority);

        // Populate the priority dropdown
        if (ddlGrievancePriority.Items.FindByValue(priority) != null)
        {
            ddlGrievancePriority.SelectedValue = priority;
        }

        // Resolution SLA according to priority
        int resolutionDays;

        switch (priority.ToLowerInvariant())
        {
            case "urgent":
                resolutionDays = 1;
                break;

            case "high":
                resolutionDays = 3;
                break;

            case "medium":
                resolutionDays = 5;
                break;

            case "low":
                resolutionDays = 7;
                break;

            default:
                resolutionDays = 5;
                break;
        }

        lblResolutionDays.Text =
            resolutionDays + (resolutionDays == 1 ? " day" : " days");

        // Display the saved due date and deadline status.
        // This requires grievanceDueDate to be included in the DAL SELECT query.
        if (row.Table.Columns.Contains("grievanceDueDate") &&
            row["grievanceDueDate"] != DBNull.Value)
        {
            DateTime dueDate =
                Convert.ToDateTime(row["grievanceDueDate"]);

            lblDueDate.Text = dueDate.ToString("dd MMM yyyy");

            if (isClosed)
            {
                lblDeadlineStatus.Text = "Closed";
                lblDeadlineStatus.CssClass = "hod-deadline-value";
            }
            else if (dueDate.Date < DateTime.Today)
            {
                lblDeadlineStatus.Text = "Overdue";
                lblDeadlineStatus.CssClass =
                    "hod-deadline-value hod-deadline-overdue";
            }
            else
            {
                lblDeadlineStatus.Text = "Within Deadline";
                lblDeadlineStatus.CssClass = "hod-deadline-value";
            }
        }
        else
        {
            lblDueDate.Text = "Not set";
            lblDeadlineStatus.Text = "Not set";
            lblDeadlineStatus.CssClass = "hod-deadline-value";
        }

        // Load evidence only after department ownership is verified
        List<MEvidence> evidence =
            DALEvidence.GetEvidenceByGrievanceID(grievanceID);

        rptEvidence.DataSource = evidence;
        rptEvidence.DataBind();

        pnlNoEvidence.Visible =
            evidence == null || evidence.Count == 0;

        // Check whether this Grievance already has an active assignment
        DataTable assignment =
            DALGrievance.GetHODAssignmentDetails(grievanceID, deptID);

        bool isAssigned =
            assignment != null &&
            assignment.Rows.Count > 0 &&
            string.Equals(
                Convert.ToString(assignment.Rows[0]["assignmentStatus"]),
                "Assigned",
                StringComparison.OrdinalIgnoreCase);


        if (isClosed)
        {
            lnkAssignToStaff.Visible = false;
        }
        else if (isAssigned)
        {
            lnkAssignToStaff.Visible = true;
            lnkAssignToStaff.Text = "Already Assigned";
            lnkAssignToStaff.NavigateUrl = "#";
            lnkAssignToStaff.Attributes["onclick"] = "return false;";

            if (!lnkAssignToStaff.CssClass.Contains(
                "hod-assignment-disabled"))
            {
                lnkAssignToStaff.CssClass +=
                    " hod-assignment-disabled";
            }
        }
        else
        {
            lnkAssignToStaff.Visible = true;
            lnkAssignToStaff.Text = "Assign to Staff";
            lnkAssignToStaff.NavigateUrl =
                ResolveUrl(
                    "~/DepartmentHead/HODStaffAssignment.aspx?id=" +
                    grievanceID);

            lnkAssignToStaff.Attributes.Remove("onclick");

            lnkAssignToStaff.CssClass =
                lnkAssignToStaff.CssClass.Replace(
                    " hod-assignment-disabled", "");
        }


        // Load the database-driven progress timeline after ownership is verified.
        LoadHODTimeline(grievanceID, deptID);
    }


    private void LoadHODTimeline(int grievanceID, int deptID)
    {
        DataTable dt = DALGrievance.GetHODGrievanceTimeline(grievanceID, deptID);
        if (dt == null || dt.Rows.Count == 0)
            return;

        DataRow row = dt.Rows[0];
        string status = Convert.ToString(row["grievanceStatus"]).Trim();
        bool isResolved = status.Equals("Resolved", StringComparison.OrdinalIgnoreCase);
        bool isRejected = status.Equals("Rejected", StringComparison.OrdinalIgnoreCase);
        bool hasAssignment = row["StaffAssignedDate"] != DBNull.Value;
        bool hasResolutionReview = row["ResolutionReviewDate"] != DBNull.Value;

        // Reset the stage classes and number/check indicators.
        ResetTimelineStep(stepSubmitted, lblStepSubmitted, "1");
        ResetTimelineStep(stepDepartmentReview, lblStepDepartmentReview, "2");
        ResetTimelineStep(stepAssigned, lblStepAssigned, "3");
        ResetTimelineStep(stepResolutionReview, lblStepResolutionReview, "4");
        ResetTimelineStep(stepResolved, lblStepResolved, "5");

        // Submission is always a completed event for an existing grievance.
        stepSubmitted.Attributes["class"] = AddCssClass(
            stepSubmitted.Attributes["class"], "completed");
        lblStepSubmitted.Text = "✓";

        DateTime submissionDate;
        if (row["grievanceSubmissionDate"] != DBNull.Value &&
            DateTime.TryParse(Convert.ToString(row["grievanceSubmissionDate"]), out submissionDate))
        {
            lblTimelineSubmitted.Text = "Submitted on " + submissionDate.ToString("dd MMM yyyy, hh:mm tt");
        }
        else
        {
            lblTimelineSubmitted.Text = "Grievance submitted";
        }

        if (hasAssignment || hasResolutionReview || isResolved)
        {
            stepDepartmentReview.Attributes["class"] = AddCssClass(
                stepDepartmentReview.Attributes["class"], "completed");
            lblStepDepartmentReview.Text = "✓";
            lblTimelineDepartmentReview.Text = "Department review completed";
        }
        else
        {
            stepDepartmentReview.Attributes["class"] = AddCssClass(
                stepDepartmentReview.Attributes["class"], "active");
            lblTimelineDepartmentReview.Text = "Awaiting department review";
        }

        if (hasAssignment || hasResolutionReview || isResolved)
        {
            stepAssigned.Attributes["class"] = AddCssClass(
                stepAssigned.Attributes["class"], "completed");
            lblStepAssigned.Text = "✓";
            DateTime assignedDate;
            if (DateTime.TryParse(Convert.ToString(row["StaffAssignedDate"]), out assignedDate))
                lblTimelineAssigned.Text = "Assigned on " + assignedDate.ToString("dd MMM yyyy, hh:mm tt");
            else
                lblTimelineAssigned.Text = "Assigned to staff";
        }
        else
        {
            lblTimelineAssigned.Text = "Awaiting staff assignment";
        }

        if (hasResolutionReview || isResolved)
        {
            stepResolutionReview.Attributes["class"] = AddCssClass(
                stepResolutionReview.Attributes["class"], "completed");
            lblStepResolutionReview.Text = "✓";
            if (hasResolutionReview)
            {
                DateTime reviewDate;
                if (DateTime.TryParse(Convert.ToString(row["ResolutionReviewDate"]), out reviewDate))
                    lblTimelineResolutionReview.Text = "Submitted for review on " + reviewDate.ToString("dd MMM yyyy, hh:mm tt");
                else
                    lblTimelineResolutionReview.Text = "Resolution submitted for review";
            }
            else
            {
                lblTimelineResolutionReview.Text = "Completed; review date not recorded";
            }
        }
        else
        {
            // Resolution review starts only when its tracking event exists.
            // Staff assignment alone does not activate Stage 4.
            lblTimelineResolutionReview.Text = hasAssignment
                ? "Awaiting resolution submission and review"
                : "Awaiting staff assignment";
        }

        if (isResolved)
        {
            stepResolved.Attributes["class"] = AddCssClass(
                stepResolved.Attributes["class"], "completed");
            lblStepResolved.Text = "✓";
            DateTime resolutionDate;
            if (row["grievanceResolutionDate"] != DBNull.Value &&
                DateTime.TryParse(Convert.ToString(row["grievanceResolutionDate"]), out resolutionDate))
                lblTimelineResolved.Text = "Resolved on " + resolutionDate.ToString("dd MMM yyyy, hh:mm tt");
            else
                lblTimelineResolved.Text = "Grievance resolved";
        }
        else if (isRejected)
        {
            stepResolved.Attributes["class"] = AddCssClass(
                stepResolved.Attributes["class"], "rejected-step");
            lblStepResolved.Text = "✕";
            lblTimelineResolved.Text = "Grievance rejected";
        }
        else
        {
            lblTimelineResolved.Text = "Awaiting resolution";
        }
    }

    private void ResetTimelineStep(
        System.Web.UI.HtmlControls.HtmlGenericControl step,
        System.Web.UI.WebControls.Label indicator,
        string number)
    {
        string css = step.Attributes["class"] ?? "";
        css = css.Replace(" completed", "")
                 .Replace(" active", "")
                 .Replace(" rejected-step", "");
        step.Attributes["class"] = css.Trim();
        indicator.Text = number;
    }

    private string AddCssClass(string existingClasses, string newClass)
    {
        existingClasses = existingClasses ?? "";
        if ((" " + existingClasses + " ").IndexOf(
            " " + newClass + " ", StringComparison.OrdinalIgnoreCase) < 0)
            return (existingClasses + " " + newClass).Trim();
        return existingClasses;
    }

    protected string GetStatusBadgeClass(string status)
    {
        switch ((status ?? "").Trim())
        {
            case "Pending":
                return "status-badge status-pending";

            case "In Process":
                return "status-badge status-in-process";

            case "Resolved":
                return "status-badge status-resolved";

            case "Rejected":
                return "status-badge status-rejected";

            default:
                return "status-badge";
        }
    }

    protected string GetPriorityBadgeClass(string priority)
    {
        switch ((priority ?? "").Trim().ToLowerInvariant())
        {
            case "low":
                return "priority-badge priority-low";

            case "medium":
                return "priority-badge priority-medium";

            case "high":
                return "priority-badge priority-high";

            case "urgent":
                return "priority-badge priority-urgent";

            default:
                return "priority-badge";
        }
    }



    protected string GetEvidenceUrl(object evidenceID, string mode)
    {
        int id;

        if (!int.TryParse(Convert.ToString(evidenceID), out id) || id <= 0)
            return "#";

        if (mode != "open" && mode != "download")
            return "#";

        return ResolveUrl(
            "~/DownloadEvidence.aspx?id=" + id +
            "&mode=" + mode
        );
    }

    private int GetResolutionDays(string priority)
    {
        switch ((priority ?? "").Trim().ToLowerInvariant())
        {
            case "urgent":
                return 1;

            case "high":
                return 3;

            case "medium":
                return 5;

            case "low":
                return 7;

            default:
                throw new ArgumentException("Invalid grievance priority.");
        }
    }

    private void ShowPriorityMessage(string message)
    {
        pnlPriorityMessage.Visible = true;
        lblPriorityMessage.Text = Server.HtmlEncode(message);
    }

    protected void btnSavePriority_Click(object sender, EventArgs e)
    {
        int deptID;
        int grievanceID;

        if (!int.TryParse(Convert.ToString(Session["DeptID"]), out deptID) ||
            !int.TryParse(Request.QueryString["id"], out grievanceID) ||
            grievanceID <= 0)
        {
            Response.Redirect("~/DepartmentHead/HODLogin.aspx", true);
            return;
        }

        string priority = ddlGrievancePriority.SelectedValue;

        int days;

        try
        {
            days = GetResolutionDays(priority);
        }
        catch (ArgumentException)
        {
            ShowPriorityMessage("Please select a valid priority.");
            return;
        }

        // Re-fetch the grievance to verify department ownership
        // and obtain its original submission date.
        DataTable dt =
            DALGrievance.GetHODGrievanceDetailsByID(grievanceID, deptID);

        if (dt == null || dt.Rows.Count == 0)
        {
            Response.Redirect("~/DepartmentHead/HODGrievances.aspx", true);
            return;
        }

        DataRow row = dt.Rows[0];

        string currentStatus =
            Convert.ToString(row["grievanceStatus"]).Trim();

        if (currentStatus.Equals("Resolved",
                StringComparison.OrdinalIgnoreCase) ||
            currentStatus.Equals("Rejected",
                StringComparison.OrdinalIgnoreCase))
        {
            ShowPriorityMessage(
                "Priority and deadline cannot be changed for a resolved or rejected grievance.");
            return;
        }

        if (row["grievanceSubmissionDate"] == DBNull.Value)
        {
            ShowPriorityMessage(
                "The submission date is missing. The deadline cannot be calculated.");
            return;
        }

        DateTime submissionDate =
            Convert.ToDateTime(row["grievanceSubmissionDate"]);

        DateTime dueDate = submissionDate.Date.AddDays(days);

        int result =
            DALGrievance.UpdateHODGrievancePriorityAndDueDate(
                grievanceID, deptID, priority, dueDate);

        if (result == 1)
        {
            LoadGrievanceDetails(grievanceID, deptID);
            ShowPriorityMessage(
                "Priority and resolution due date saved successfully.");
        }
        else
        {
            ShowPriorityMessage(
                "The update could not be saved. Please refresh and try again.");
        }
    }

    protected void btnRejectGrievance_Click(object sender, EventArgs e)
    {
        pnlRejectionMessage.Visible = false;
        lblRejectionMessage.Text = "";

        string reason = (txtRejectionReason.Text ?? "").Trim();

        if (string.IsNullOrWhiteSpace(reason))
        {
            ShowRejectionMessage("Please enter a rejection reason.");
            return;
        }

        if (reason.Length > 255)
        {
            ShowRejectionMessage(
                "The rejection reason cannot exceed 255 characters.");
            return;
        }

        int grievanceID;
        int deptID;
        int deptHeadID;

        if (Session["DeptID"] == null ||
            Session["DeptHeadID"] == null ||
            !int.TryParse(Request.QueryString["id"], out grievanceID) ||
            !int.TryParse(Session["DeptID"].ToString(), out deptID) ||
            !int.TryParse(Session["DeptHeadID"].ToString(), out deptHeadID))
        {
            ShowRejectionMessage(
                "Your session or grievance information is invalid. Please sign in again.");
            return;
        }

        try
        {
            int result = DALGrievance.RejectHODGrievance(
                grievanceID,
                deptID,
                deptHeadID,
                reason);

            if (result == 1)
            {
                // =========================================================
                // EMAIL NOTIFICATION TO STUDENT - REJECTED
                // The database rejection has already succeeded.
                // Email failure must NOT undo the rejection.
                // =========================================================
                try
                {
                    DataTable grievance =
                        DALGrievance.GetHODGrievanceDetailsByID(
                            grievanceID, deptID);

                    if (grievance != null && grievance.Rows.Count > 0)
                    {
                        int stdID = Convert.ToInt32(
                            grievance.Rows[0]["stdID"]);

                        string grievanceTitle = Convert.ToString(
                            grievance.Rows[0]["grievanceTitle"]);

                        string studentEmail = GetStudentEmail(stdID);
                        string studentName = GetStudentName(stdID);

                        if (!string.IsNullOrWhiteSpace(studentEmail))
                        {
                            string subject =
                                "Grievance Rejected - GRV-" + grievanceID;

                            string body =
                                "<html>" +
                                "<body style='font-family:Arial,sans-serif;color:#172033;'>" +

                                "<h2 style='color:#123a7a;'>" +
                                "Grievance Rejected" +
                                "</h2>" +

                                "<p>Dear " +
                                Server.HtmlEncode(studentName) +
                                ",</p>" +

                                "<p>" +
                                "Your Grievance has been reviewed by the " +
                                "Department Head and has been rejected." +
                                "</p>" +

                                "<table cellpadding='6' cellspacing='0' border='0'>" +

                                "<tr>" +
                                "<td><strong>Grievance ID</strong></td>" +
                                "<td>GRV-" + grievanceID + "</td>" +
                                "</tr>" +

                                "<tr>" +
                                "<td><strong>Title</strong></td>" +
                                "<td>" +
                                Server.HtmlEncode(grievanceTitle) +
                                "</td>" +
                                "</tr>" +

                                "<tr>" +
                                "<td><strong>Current Status</strong></td>" +
                                "<td>Rejected</td>" +
                                "</tr>" +

                                "<tr>" +
                                "<td><strong>Rejection Reason</strong></td>" +
                                "<td>" +
                                Server.HtmlEncode(reason) +
                                "</td>" +
                                "</tr>" +

                                "</table>" +

                                "<p>" +
                                "Please log in to the Student Portal to view " +
                                "the details of your Grievance." +
                                "</p>" +

                                "<p>" +
                                "Regards,<br/>" +
                                "<strong>USP Online Grievance Redressal System</strong><br/>" +
                                "University of Southern Punjab, Multan" +
                                "</p>" +

                                "</body>" +
                                "</html>";

                            EmailService.SendEmail(
                                studentEmail,
                                subject,
                                body);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Rejection has already succeeded.
                    // Email failure must not turn it into a rejection failure.
                    System.Diagnostics.Trace.TraceError(
                        "Failed to send Rejected email for Grievance ID " +
                        grievanceID + ": " + ex);
                }

                txtRejectionReason.Text = "";

                LoadGrievanceDetails(grievanceID, deptID);

                ShowRejectionMessage(
                    "The grievance was rejected successfully. The student has been notified.");
            }
            else if (result == 0)
            {
                LoadGrievanceDetails(grievanceID, deptID);

                ShowRejectionMessage(
                    "The grievance could not be rejected. It may no longer be Pending, or it may not belong to your department.");
            }
            else
            {
                ShowRejectionMessage(
                    "Please provide a valid rejection reason of up to 255 characters.");
            }
        }
        catch (Exception)
        {
            ShowRejectionMessage(
                "The grievance could not be rejected because a database operation failed. Please try again.");
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

            return result == null ||
                   result == DBNull.Value ||
                   string.IsNullOrWhiteSpace(Convert.ToString(result))
                ? "Student"
                : Convert.ToString(result).Trim();
        }
    }


    private void ShowRejectionMessage(string message)
    {
        lblRejectionMessage.Text = message;
        pnlRejectionMessage.Visible = true;
    }

}
