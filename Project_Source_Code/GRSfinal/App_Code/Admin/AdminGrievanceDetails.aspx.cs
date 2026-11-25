using System;
using System.Data;
using System.Web.UI;

public partial class Admin_AdminGrievanceDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Check Admin login
        if (Session["AdminID"] == null)
        {
            Response.Redirect("~/Admin/AdminLogin.aspx", false);
            return;
        }

        if (!IsPostBack)
        {
            LoadGrievanceDetails();
        }
    }


    private void LoadGrievanceDetails()
    {
        // Get grievance ID from Query String
        string grievanceIDValue = Request.QueryString["id"];

        if (string.IsNullOrEmpty(grievanceIDValue))
        {
            Response.Redirect("~/Admin/AllGrievances.aspx", false);
            return;
        }


        int grievanceID;

        if (!int.TryParse(grievanceIDValue, out grievanceID) ||
            grievanceID <= 0)
        {
            Response.Redirect("~/Admin/AllGrievances.aspx", false);
            return;
        }


        // Get grievance details from database
        DataTable dt =
            DALGrievance.GetAdminGrievanceDetailsByID(grievanceID);


        if (dt == null || dt.Rows.Count == 0)
        {
            Response.Redirect("~/Admin/AllGrievances.aspx", false);
            return;
        }


        DataRow row = dt.Rows[0];


        // Grievance ID
        lblGrievanceID.Text =
            "GRV-" + row["grievanceID"].ToString();


        // Student Information
        lblStudentName.Text =
            Server.HtmlEncode(row["StudentName"].ToString());

        lblStudentRegNo.Text =
            Server.HtmlEncode(row["stdRegNo"].ToString());


        // Category and Department
        lblCategory.Text =
            Server.HtmlEncode(row["categoryName"].ToString());

        lblDepartment.Text =
            Server.HtmlEncode(row["deptName"].ToString());

        lblAssignedDepartment.Text =
            Server.HtmlEncode(Convert.ToString(row["deptName"]));

        lblDepartmentHead.Text = "Not Assigned Yet";
        lblAssignedStaff.Text = "Not Assigned Yet";
        lblAssignedOn.Text = "Not Assigned Yet";

        DataTable assignmentDetails =
            DALGrievance.GetAdminAssignmentDetails(grievanceID);

        if (assignmentDetails != null &&
            assignmentDetails.Rows.Count > 0)
        {
            DataRow assignment = assignmentDetails.Rows[0];

            if (assignment["deptHeadName"] != DBNull.Value &&
                !string.IsNullOrWhiteSpace(
                    Convert.ToString(assignment["deptHeadName"])))
            {
                lblDepartmentHead.Text =
                    Server.HtmlEncode(
                        Convert.ToString(assignment["deptHeadName"]));
            }

            if (assignment["staffName"] != DBNull.Value &&
                !string.IsNullOrWhiteSpace(
                    Convert.ToString(assignment["staffName"])))
            {
                lblAssignedStaff.Text =
                    Server.HtmlEncode(
                        Convert.ToString(assignment["staffName"]));
            }

            if (assignment["assignedDate"] != DBNull.Value)
            {
                lblAssignedOn.Text =
                    Convert.ToDateTime(assignment["assignedDate"])
                        .ToString("dd MMM yyyy, hh:mm tt");
            }
        }



        // Grievance Information
        lblTitle.Text =
            Server.HtmlEncode(row["grievanceTitle"].ToString());

        lblDescription.Text =
            Server.HtmlEncode(row["grievanceDescription"].ToString());

        lblPriority.Text =
            Server.HtmlEncode(row["grievancePriority"].ToString());


        // Status
        string status =
            row["grievanceStatus"].ToString().Trim();

        lblStatus.Text =
            Server.HtmlEncode(status);
        lblResolutionStatus.Text =
    Server.HtmlEncode(status);

        statusBadge.Attributes["class"] =
            "grievance-status " + GetStatusClass(status);


        // Submission Date
        if (row["grievanceSubmissionDate"] != DBNull.Value)
        {
            DateTime submissionDate =
                Convert.ToDateTime(row["grievanceSubmissionDate"]);

            lblSubmissionDate.Text =
                submissionDate.ToString("dd MMM yyyy");
        }
        else
        {
            lblSubmissionDate.Text = "Not Available";
        }


        // Resolution Date
        if (row["grievanceResolutionDate"] == DBNull.Value)
        {
            lblResolutionDate.Text =
                "Not Resolved Yet";
        }
        else
        {
            DateTime resolutionDate =
                Convert.ToDateTime(row["grievanceResolutionDate"]);

            if (resolutionDate == DateTime.MinValue)
            {
                lblResolutionDate.Text =
                    "Not Resolved Yet";
            }
            else
            {
                lblResolutionDate.Text =
                    resolutionDate.ToString("dd MMM yyyy");
            }
        }


        // Priority CSS class
        string priority =
            row["grievancePriority"].ToString().Trim();

        lblPriority.CssClass =
            "priority-badge " + GetPriorityClass(priority);


        // Load evidence
        LoadEvidence(grievanceID);
        LoadTimeline(grievanceID);
      
    }


    private void LoadEvidence(int grievanceID)
    {
        var evidenceList =
            DALEvidence.GetEvidenceByGrievanceID(grievanceID);

        if (evidenceList != null && evidenceList.Count > 0)
        {
            rptEvidence.DataSource = evidenceList;
            rptEvidence.DataBind();

            pnlEvidence.Visible = true;
            pnlNoEvidence.Visible = false;
        }
        else
        {
            rptEvidence.DataSource = null;
            rptEvidence.DataBind();

            pnlEvidence.Visible = false;
            pnlNoEvidence.Visible = true;
        }
    }


   
    private string GetStatusClass(string status)
    {
        switch ((status ?? string.Empty).Trim().ToLowerInvariant())
        {
            case "pending":
                return "status-pending";

            case "in process":
                return "status-in-process";

            case "resolved":
                return "status-resolved";

            default:
                return "status-pending";
        }
    }

    private string GetPriorityClass(string priority)
    {
        switch ((priority ?? string.Empty).Trim().ToLowerInvariant())
        {
            case "low":
                return "priority-low";

            case "medium":
                return "priority-medium";

            case "high":
                return "priority-high";

            case "urgent":
                return "priority-urgent";

            default:
                return "priority-low";
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(
            "~/Admin/AllGrievances.aspx",
            false);
    }


    private void LoadTimeline(int grievanceID)
    {
        DataTable dt =
            DALGrievance.GetAdminGrievanceTimeline(grievanceID);

        if (dt == null || dt.Rows.Count == 0)
            return;

        DataRow row = dt.Rows[0];

        string status =
            Convert.ToString(row["grievanceStatus"]).Trim();

        bool isResolved = status.Equals(
            "Resolved", StringComparison.OrdinalIgnoreCase);

        bool isRejected = status.Equals(
            "Rejected", StringComparison.OrdinalIgnoreCase);

        DateTime submissionDate =
            Convert.ToDateTime(row["grievanceSubmissionDate"]);

        bool departmentReviewed =
            row["DepartmentReviewDate"] != DBNull.Value;

        bool staffAssigned =
            row["StaffAssignedDate"] != DBNull.Value;

        bool resolutionUnderReview =
            row["ResolutionReviewDate"] != DBNull.Value;

        // Reset all stages.
        stepSubmitted.Attributes["class"] = "tracking-step";
        stepDepartment.Attributes["class"] = "tracking-step";
        stepHodReview.Attributes["class"] = "tracking-step";
        stepAssigned.Attributes["class"] = "tracking-step";
        stepResolved.Attributes["class"] = "tracking-step";

        // 1. Grievance Submitted
        stepSubmitted.Attributes["class"] = "tracking-step completed";
        lblStepSubmitted.Text = "✓";
        lblTimelineSubmitted.Text =
            submissionDate.ToString("dd MMM yyyy");

        // 2. Department Review
        if (departmentReviewed || staffAssigned ||
            resolutionUnderReview || isResolved)
        {
            stepDepartment.Attributes["class"] =
                "tracking-step completed";
            lblStepDepartment.Text = "✓";

            lblTimelineDepartment.Text = departmentReviewed
                ? Convert.ToDateTime(row["DepartmentReviewDate"])
                    .ToString("dd MMM yyyy")
                : "Department review completed";
        }
        else
        {
            lblTimelineDepartment.Text =
                "Awaiting department review";
        }

        // 3. Assigned to Staff
        if (staffAssigned || resolutionUnderReview || isResolved)
        {
            stepHodReview.Attributes["class"] =
                "tracking-step completed";
            lblStepHodReview.Text = "✓";

            lblTimelineHodReview.Text =
                staffAssigned
                    ? Convert.ToDateTime(row["StaffAssignedDate"])
                        .ToString("dd MMM yyyy")
                    : "Staff assignment date not recorded";
        }
        else
        {
            lblTimelineHodReview.Text =
                "Awaiting staff assignment";
        }

        // 4. Resolution Under Review
        if (resolutionUnderReview || isResolved)
        {
            stepAssigned.Attributes["class"] =
                "tracking-step completed";
            lblStepAssigned.Text = "✓";

            lblTimelineAssigned.Text = resolutionUnderReview
                ? Convert.ToDateTime(row["ResolutionReviewDate"])
                    .ToString("dd MMM yyyy")
                : "Resolution review date not recorded";
        }
        else
        {
            // Assignment alone does not activate Stage 4.
            lblTimelineAssigned.Text = staffAssigned
                ? "Awaiting resolution submission and review"
                : "Awaiting staff assignment";
        }

        // 5. Final outcome
        if (isResolved)
        {
            stepResolved.Attributes["class"] =
                "tracking-step completed";
            lblStepResolved.Text = "✓";

            lblTimelineResolved.Text =
                row["grievanceResolutionDate"] != DBNull.Value
                    ? Convert.ToDateTime(row["grievanceResolutionDate"])
                        .ToString("dd MMM yyyy")
                    : "Grievance resolved";

            lblHandlingStage.Text = "Resolved";
        }
        else if (isRejected)
        {
            stepResolved.Attributes["class"] =
                "tracking-step rejected-step";
            lblStepResolved.Text = "✕";

            lblTimelineResolved.Text = "Grievance rejected";
            lblHandlingStage.Text = "Rejected";
        }
        else
        {
            lblTimelineResolved.Text = "Awaiting resolution";

            if (resolutionUnderReview)
                lblHandlingStage.Text = "Resolution Under Review";
            else if (staffAssigned)
                lblHandlingStage.Text = "Staff Handling";
            else
                lblHandlingStage.Text = "Department Review";
        }
    }

}