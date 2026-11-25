using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;

public partial class DepartmentHead_HODProgressResolutionDetails : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int deptID;
        int grievanceID;

        if (Session["DeptHeadID"] == null ||
            !int.TryParse(Convert.ToString(Session["DeptID"]), out deptID) ||
            deptID <= 0)
        {
            Session.Clear();
            Response.Redirect("~/DepartmentHead/HODLogin.aspx", true);
            return;
        }

        if (!int.TryParse(Request.QueryString["id"], out grievanceID) ||
            grievanceID <= 0)
        {
            Response.Redirect(
                "~/DepartmentHead/HODProgressResolutionReview.aspx", true);
            return;
        }

        if (!IsPostBack)
        {
            LoadPage(grievanceID, deptID);
        }
    }

    private void LoadPage(int grievanceID, int deptID)
    {
        // Retrieve the grievance only if it belongs to this HOD's department.
        DataTable details =
            DALGrievance.GetHODGrievanceDetailsByID(grievanceID, deptID);

        if (details == null || details.Rows.Count == 0)
        {
            Response.Redirect(
                "~/DepartmentHead/HODProgressResolutionReview.aspx", true);
            return;
        }

        DataRow row = details.Rows[0];

        lblGrievanceID.Text =
            "GRV-" + Convert.ToString(row["grievanceID"]);

        lblStudentName.Text =
            Server.HtmlEncode(Convert.ToString(row["StudentName"]));

        lblRegistrationNo.Text =
            Server.HtmlEncode(Convert.ToString(row["stdRegNo"]));

        lblGrievanceTitle.Text =
            Server.HtmlEncode(Convert.ToString(row["grievanceTitle"]));

        // Display the due date when one has been assigned.
        if (details.Columns.Contains("grievanceDueDate") &&
            row["grievanceDueDate"] != DBNull.Value)
        {
            DateTime dueDate =
                Convert.ToDateTime(row["grievanceDueDate"]);

            lblDueDate.Text = dueDate.ToString("dd MMM yyyy");
        }
        else
        {
            lblDueDate.Text = "Not set";
        }

        string status =
            Convert.ToString(row["grievanceStatus"]).Trim();

        string priority =
            Convert.ToString(row["grievancePriority"]).Trim();

        lblStatus.Text = Server.HtmlEncode(status);
        lblStatus.CssClass = GetStatusBadgeClass(status);

        lblPriority.Text = Server.HtmlEncode(priority);
        lblPriority.CssClass = GetPriorityBadgeClass(priority);

        // Load evidence only after department ownership is verified.
        List<MEvidence> evidence =
            DALEvidence.GetEvidenceByGrievanceID(grievanceID);

        rptEvidence.DataSource = evidence;
        rptEvidence.DataBind();

        pnlNoEvidence.Visible =
     evidence == null || evidence.Count == 0;

        // Load the grievance workflow timeline.
        LoadTimeline(grievanceID, deptID, status);

        // Load the proposed resolution and its supporting evidence.
        LoadResolution(grievanceID, deptID, status);
    }

    private void LoadTimeline(
        int grievanceID, int deptID, string status)
    {
        DataTable dt =
            DALGrievance.GetHODGrievanceTimeline(grievanceID, deptID);

        if (dt == null || dt.Rows.Count == 0)
        {
            pnlResolutionPending.Visible = true;
            pnlResolutionSubmitted.Visible = false;
            lblResolutionState.Text = "Resolution Pending";
            lblResolutionState.CssClass = "hod-prd-resolution-state";
            return;
        }

        DataRow row = dt.Rows[0];

        bool isResolved = status.Equals(
            "Resolved", StringComparison.OrdinalIgnoreCase);

        bool isRejected = status.Equals(
            "Rejected", StringComparison.OrdinalIgnoreCase);

        bool hasAssignment =
            row["StaffAssignedDate"] != DBNull.Value;

        bool hasResolutionReview =
            row["ResolutionReviewDate"] != DBNull.Value;

        ResetStep(stepSubmitted, lblStepSubmitted, "1");
        ResetStep(stepDepartmentReview, lblStepDepartmentReview, "2");
        ResetStep(stepAssigned, lblStepAssigned, "3");
        ResetStep(stepResolutionReview, lblStepResolutionReview, "4");
        ResetStep(stepResolved, lblStepResolved, "5");

        // Stage 1: Grievance Submitted
        SetStepState(
            stepSubmitted, lblStepSubmitted, "completed", "✓");

        DateTime submitted;

        if (row["grievanceSubmissionDate"] != DBNull.Value &&
            DateTime.TryParse(
                Convert.ToString(row["grievanceSubmissionDate"]),
                out submitted))
        {
            lblTimelineSubmitted.Text =
                "Submitted on " +
                submitted.ToString("dd MMM yyyy, hh:mm tt");
        }
        else
        {
            lblTimelineSubmitted.Text = "Grievance submitted";
        }

        // Stage 2: Department Review
        if (hasAssignment || hasResolutionReview ||
            isResolved || isRejected)
        {
            SetStepState(
                stepDepartmentReview,
                lblStepDepartmentReview, "completed", "✓");

            lblTimelineDepartmentReview.Text =
                "Department review completed";
        }
        else
        {
            SetStepState(
                stepDepartmentReview,
                lblStepDepartmentReview, "active", "2");

            lblTimelineDepartmentReview.Text =
                "Awaiting department review";
        }

        // Stage 3: Assigned to Staff
        if (hasAssignment || hasResolutionReview || isResolved)
        {
            SetStepState(
                stepAssigned, lblStepAssigned, "completed", "✓");

            DateTime assigned;

            if (row["StaffAssignedDate"] != DBNull.Value &&
                DateTime.TryParse(
                    Convert.ToString(row["StaffAssignedDate"]),
                    out assigned))
            {
                lblTimelineAssigned.Text =
                    "Assigned on " +
                    assigned.ToString("dd MMM yyyy, hh:mm tt");
            }
            else
            {
                lblTimelineAssigned.Text = "Assigned to staff";
            }
        }
        else
        {
            lblTimelineAssigned.Text = "Awaiting staff assignment";
        }

        // Stage 4: Resolution Under Review
        if (hasResolutionReview || isResolved)
        {
            SetStepState(
                stepResolutionReview,
                lblStepResolutionReview, "completed", "✓");

            DateTime reviewDate;

            if (row["ResolutionReviewDate"] != DBNull.Value &&
                DateTime.TryParse(
                    Convert.ToString(row["ResolutionReviewDate"]),
                    out reviewDate))
            {
                lblTimelineResolutionReview.Text =
                    "Submitted for review on " +
                    reviewDate.ToString("dd MMM yyyy, hh:mm tt");
            }
            else
            {
                lblTimelineResolutionReview.Text =
                    "Resolution submitted for review";
            }
        }
        else
        {
            lblTimelineResolutionReview.Text = hasAssignment
                ? "Awaiting resolution submission and review"
                : "Awaiting staff assignment";
        }

        // Stage 5: Final outcome
        if (isResolved)
        {
            SetStepState(
                stepResolved, lblStepResolved, "completed", "✓");

            DateTime resolved;

            if (row["grievanceResolutionDate"] != DBNull.Value &&
                DateTime.TryParse(
                    Convert.ToString(row["grievanceResolutionDate"]),
                    out resolved))
            {
                lblTimelineResolved.Text =
                    "Resolved on " +
                    resolved.ToString("dd MMM yyyy, hh:mm tt");
            }
            else
            {
                lblTimelineResolved.Text = "Grievance resolved";
            }
        }
        else if (isRejected)
        {
            SetStepState(
                stepResolved, lblStepResolved, "rejected-step", "✕");

            lblTimelineResolved.Text = "Grievance rejected";
        }
        else
        {
            lblTimelineResolved.Text = "Awaiting resolution";
        }

        // Resolution details are not yet available from the current workflow.
        pnlResolutionPending.Visible =
            !hasResolutionReview && !isResolved && !isRejected;

        pnlResolutionSubmitted.Visible =
            hasResolutionReview && !isResolved && !isRejected;

        if (isResolved)
        {
            lblResolutionState.Text = "Resolved";
            lblResolutionState.CssClass =
                "hod-prd-resolution-state resolution-complete";
        }
        else if (isRejected)
        {
            lblResolutionState.Text = "Rejected";
            lblResolutionState.CssClass =
                "hod-prd-resolution-state resolution-rejected";
        }
        else if (hasResolutionReview)
        {
            lblResolutionState.Text = "Submitted for Review";
            lblResolutionState.CssClass =
                "hod-prd-resolution-state resolution-submitted";
        }
        else
        {
            lblResolutionState.Text = "Resolution Pending";
            lblResolutionState.CssClass =
                "hod-prd-resolution-state";
        }
    }

    private void LoadResolution(
        int grievanceID,
        int deptID,
        string grievanceStatus)
    {
        DataTable resolution =
            DALGrievance.GetHODResolutionByGrievanceID(
                grievanceID, deptID);

        if (resolution == null || resolution.Rows.Count == 0)
        {
            pnlResolutionPending.Visible = true;
            pnlResolutionSubmitted.Visible = false;
            pnlHODReviewActions.Visible = false;
            lblResolutionState.Text = "Resolution Pending";
            lblProposedResolution.Text = "";
            lblStaffRemarks.Text = "";
            rptResolutionEvidence.DataSource = null;
            rptResolutionEvidence.DataBind();
            pnlNoResolutionEvidence.Visible = false;
            return;
        }

        DataRow row = resolution.Rows[0];

        string reviewStatus =
            Convert.ToString(row["reviewStatus"]).Trim();

        lblProposedResolution.Text =
            Server.HtmlEncode(
                Convert.ToString(row["proposedResolution"]));

        lblStaffRemarks.Text =
            row["staffRemarks"] == DBNull.Value
                ? "No remarks provided."
                : Server.HtmlEncode(
                    Convert.ToString(row["staffRemarks"]));

        pnlResolutionPending.Visible = false;
        pnlResolutionSubmitted.Visible = true;

        bool awaitingReview =
            reviewStatus.Equals(
                "Pending", StringComparison.OrdinalIgnoreCase)
            &&
            grievanceStatus.Equals(
                "In Process", StringComparison.OrdinalIgnoreCase);

        pnlHODReviewActions.Visible = awaitingReview;

        switch (reviewStatus)
        {
            case "Approved":
                lblResolutionState.Text = "Approved";
                lblResolutionState.CssClass =
                    "hod-prd-resolution-state resolution-complete";
                break;

            case "Returned":
                lblResolutionState.Text = "Returned to Staff";
                lblResolutionState.CssClass =
                    "hod-prd-resolution-state resolution-submitted";
                break;

            default:
                lblResolutionState.Text = "Submitted for Review";
                lblResolutionState.CssClass =
                    "hod-prd-resolution-state resolution-submitted";
                break;
        }

        int staffID = Convert.ToInt32(row["staffID"]);

        List<MEvidence> evidence =
            DALEvidence.GetResolutionEvidenceByGrievanceID(
                grievanceID, staffID);

        rptResolutionEvidence.DataSource = evidence;
        rptResolutionEvidence.DataBind();

        pnlNoResolutionEvidence.Visible =
            evidence == null || evidence.Count == 0;
    }

    private void ResetStep(
        System.Web.UI.HtmlControls.HtmlGenericControl step,
        Label indicator,
        string number)
    {
        string css = step.Attributes["class"] ?? "";

        css = css.Replace(" completed", "")
                 .Replace(" active", "")
                 .Replace(" rejected-step", "");

        step.Attributes["class"] = css.Trim();
        indicator.Text = number;
    }

    private void SetStepState(
        System.Web.UI.HtmlControls.HtmlGenericControl step,
        Label indicator,
        string state,
        string text)
    {
        string css = step.Attributes["class"] ?? "";

        if ((" " + css + " ").IndexOf(
            " " + state + " ",
            StringComparison.OrdinalIgnoreCase) < 0)
        {
            css = (css + " " + state).Trim();
        }

        step.Attributes["class"] = css;
        indicator.Text = text;
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

    protected void btnApproveResolution_Click(
        object sender, EventArgs e)
    {
        ProcessResolutionReview("Approve");
    }

    protected void btnReturnResolution_Click(
        object sender, EventArgs e)
    {
        ProcessResolutionReview("Return");
    }

    private void ProcessResolutionReview(string decision)
    {
        int deptHeadID;
        int deptID;
        int grievanceID;

        if (!int.TryParse(
                Convert.ToString(Session["DeptHeadID"]),
                out deptHeadID)
            || deptHeadID <= 0
            || !int.TryParse(
                Convert.ToString(Session["DeptID"]),
                out deptID)
            || deptID <= 0)
        {
            Session.Clear();
            Response.Redirect(
                "~/DepartmentHead/HODLogin.aspx", true);
            return;
        }

        if (!int.TryParse(Request.QueryString["id"], out grievanceID)
            || grievanceID <= 0)
        {
            Response.Redirect(
                "~/DepartmentHead/HODProgressResolutionReview.aspx",
                true);
            return;
        }

        string remarks = (txtHODRemarks.Text ?? "").Trim();

        if (remarks.Length > 2000)
        {
            ShowReviewMessage(
                "HOD remarks cannot exceed 2,000 characters.", false);
            return;
        }

        if (decision == "Return" &&
            string.IsNullOrWhiteSpace(remarks))
        {
            ShowReviewMessage(
                "Please enter remarks before returning the resolution.",
                false);
            return;
        }

        int result = DALGrievance.ReviewStaffResolution(
            grievanceID,
            deptID,
            deptHeadID,
            decision,
            remarks);

        if (result == 1)
        {
            // Send notifications only after the HOD review succeeds.
            try
            {
                DataTable resolution =
                    DALGrievance.GetHODResolutionByGrievanceID(
                        grievanceID, deptID);

                DataTable grievance =
                    DALGrievance.GetHODGrievanceDetailsByID(
                        grievanceID, deptID);

                if (resolution.Rows.Count > 0 &&
                    grievance.Rows.Count > 0)
                {
                    int staffID = Convert.ToInt32(
                        resolution.Rows[0]["staffID"]);

                    int stdID = Convert.ToInt32(
                        grievance.Rows[0]["stdID"]);

                    if (decision == "Approve")
                    {
                        DALSystemNotification.InsertNotification(
                            "Staff",
                            staffID,
                            grievanceID,
                            "Resolution Approved",
                            "Your proposed resolution for Grievance #" +
                            grievanceID +
                            " has been approved by the HOD.");

                        DALSystemNotification.InsertNotification(
                            "Student",
                            stdID,
                            grievanceID,
                            "Grievance Resolved",
                            "Your Grievance #" + grievanceID +
                            " has been resolved.");

                        // =====================================================
                        // EMAIL NOTIFICATION TO STUDENT - RESOLVED
                        // =====================================================
                        try
                        {
                            string studentEmail = GetStudentEmail(stdID);
                            string studentName = GetStudentName(stdID);
                            string grievanceTitle = Convert.ToString(
                                grievance.Rows[0]["grievanceTitle"]);

                            if (!string.IsNullOrWhiteSpace(studentEmail))
                            {
                                string subject =
                                    "Grievance Resolved - GRV-" + grievanceID;

                                string body =
                                    "<html>" +
                                    "<body style='font-family:Arial,sans-serif;color:#172033;'>" +

                                    "<h2 style='color:#123a7a;'>" +
                                    "Grievance Resolved" +
                                    "</h2>" +

                                    "<p>Dear " +
                                    Server.HtmlEncode(studentName) +
                                    ",</p>" +

                                    "<p>" +
                                    "Your Grievance has been reviewed and the " +
                                    "proposed resolution has been approved by " +
                                    "the Department Head." +
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
                                    "<td>Resolved</td>" +
                                    "</tr>" +

                                    "</table>" +

                                    "<p>" +
                                    "You can log in to the Student Portal to " +
                                    "view the details and resolution of your " +
                                    "Grievance." +
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
                        catch (Exception ex)
                        {
                            // The resolution has already succeeded.
                            // Email failure must NOT turn it into a review failure.
                            System.Diagnostics.Trace.TraceError(
                                "Failed to send Resolved email for Grievance ID " +
                                grievanceID + ": " + ex);
                        }
                    }
                    else if (decision == "Return")
                    {
                        DALSystemNotification.InsertNotification(
                            "Staff",
                            staffID,
                            grievanceID,
                            "Resolution Returned",
                            "Your proposed resolution for Grievance #" +
                            grievanceID +
                            " has been returned for revision. HOD remarks: " +
                            remarks);
                    }
                }
            }
            catch (Exception ex)
            {
                // The review has already succeeded.
                // Do not treat a notification error as a review failure.
                System.Diagnostics.Trace.TraceError(
                    "Grievance notification error: " + ex);
            }
            LoadPage(grievanceID, deptID);
            txtHODRemarks.Text = "";

            ShowReviewMessage(
                decision == "Approve"
                    ? "Resolution approved successfully. The grievance is now Resolved."
                    : "Resolution returned to Staff for revision. The grievance remains In Process.",
                true);
        }
        else if (result == 2)
        {
            ShowReviewMessage(
                "Remarks are required when returning a resolution.",
                false);
        }
        else
        {
            LoadPage(grievanceID, deptID);

            ShowReviewMessage(
                "The resolution could not be reviewed. It may already have been reviewed, or it may not belong to your department.",
                false);
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


    private void ShowReviewMessage(string message, bool success)
    {
        lblReviewMessage.Text = Server.HtmlEncode(message);
        pnlReviewMessage.Visible = true;

        pnlReviewMessage.CssClass = success
            ? "hod-prd-review-message success"
            : "hod-prd-review-message error";
    }

    protected string GetEvidenceUrl(object evidenceID, string mode)
    {
        int id;

        if (!int.TryParse(Convert.ToString(evidenceID), out id) ||
            id <= 0)
        {
            return "#";
        }

        if (mode != "open" && mode != "download")
        {
            return "#";
        }

        return ResolveUrl(
            "~/DownloadEvidence.aspx?id=" + id + "&mode=" + mode);
    }
}