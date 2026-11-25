using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;



public partial class Student_GrievanceDetails : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        int studentID;

        if (Session["StdID"] == null ||
            !int.TryParse(Session["StdID"].ToString(), out studentID) ||
            studentID <= 0)
        {
            Session.Remove("StdID");
            Response.Redirect("~/Student/StudentLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
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
            Response.Redirect(
                "~/Student/StudentDashboard.aspx?view=TrackGrievance",
                false);

            return;
        }


        int grievanceID;

        if (!int.TryParse(grievanceIDValue, out grievanceID))
        {
            Response.Redirect(
                "~/Student/StudentDashboard.aspx?view=TrackGrievance",
                false);

            return;
        }


        // Get grievance from database
        MGrievance grievance =
            DALGrievance.GetGrievanceByID(grievanceID);


        if (grievance == null)
        {
            Response.Redirect(
                "~/Student/StudentDashboard.aspx?view=TrackGrievance",
                false);

            return;
        }


        // Security check
        // Make sure this grievance belongs to logged-in student

        int loggedInStudentID;

        if (!int.TryParse(
                Convert.ToString(Session["StdID"]),
                out loggedInStudentID) ||
            loggedInStudentID <= 0)
        {
            Response.Redirect("~/Student/StudentLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }


        if (grievance.StdID != loggedInStudentID)
        {
            Response.Redirect(
                "~/Student/StudentDashboard.aspx?view=TrackGrievance",
                false);

            Context.ApplicationInstance.CompleteRequest();
            return;
        }
        LoadEvidence(grievanceID);


        // Display grievance information

        lblGrievanceID.Text =
            grievance.GrievanceID.ToString();
        lblCategory.Text =
    Server.HtmlEncode(grievance.CategoryName);

        lblDepartment.Text =
            Server.HtmlEncode(grievance.DepartmentName);

        lblTitle.Text =
            Server.HtmlEncode(grievance.GrievanceTitle);

        lblDescription.Text =
            Server.HtmlEncode(grievance.GrievanceDescription);

        lblPriority.Text =
            Server.HtmlEncode(grievance.GrievancePriority);

        lblStatus.Text =
            Server.HtmlEncode(grievance.GrievanceStatus);
        LoadFeedbackSection(
    grievance.GrievanceID,
    loggedInStudentID,
    grievance.GrievanceStatus);
        string status = grievance.GrievanceStatus.Trim();

        statusBadge.Attributes["class"] =
            "badge grievance-status " + GetStatusClass(status);

        lblSubmissionDate.Text =
            grievance.GrievanceSubmissionDate.ToString("dd MMM yyyy");


        // Handle empty/default resolution date

        if (grievance.GrievanceResolutionDate ==
            DateTime.MinValue)
        {
            lblResolutionDate.Text = "Not Resolved Yet";
        }
        else
        {
            lblResolutionDate.Text =
                grievance.GrievanceResolutionDate.ToString("dd MMM yyyy");
        }
        DataTable timelineData =
     DALGrievance.GetStudentGrievanceTimeline(
         grievanceID,
         loggedInStudentID);

        if (timelineData.Rows.Count > 0)
        {
            DataRow timeline = timelineData.Rows[0];

            SetProgressStatus(
                Convert.ToString(timeline["grievanceStatus"]),
                Convert.ToInt32(timeline["HasAssignment"]) == 1,
                Convert.ToInt32(timeline["HasResolutionReview"]) == 1);
        }
    }
    


    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(
            "~/Student/StudentDashboard.aspx?view=TrackGrievance",
            false);
    }
    private void LoadEvidence(int grievanceID)
    {
        List<MEvidence> evidenceList =
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

            case "rejected":
                return "status-rejected";

            default:
                return "status-pending";
        }
    }


    private void SetProgressStatus(
     string status,
     bool hasAssignment,
     bool hasResolutionReview)
    {
        status = (status ?? string.Empty)
            .Trim()
            .ToLowerInvariant();

        bool isPending = status == "pending";
        bool isInProcess = status == "in process";
        bool isResolved = status == "resolved";
        bool isRejected = status == "rejected";

        // Reset all stages.
        stepSubmitted.Attributes["class"] = "tracking-step";
        stepDepartmentReview.Attributes["class"] = "tracking-step";
        stepAssigned.Attributes["class"] = "tracking-step";
        stepResolutionReview.Attributes["class"] = "tracking-step";
        stepResolved.Attributes["class"] = "tracking-step";

        stepRejected.Attributes["class"] = "tracking-step rejected-step";
        stepRejected.Visible = false;

        // Stage 1: Grievance Submitted.
        stepSubmitted.Attributes["class"] = "tracking-step completed";

        // Stage 2: Department Review.
        if (isPending)
        {
            stepSubmitted.Attributes["class"] = "tracking-step active";
        }
        else
        {
            stepDepartmentReview.Attributes["class"] =
                "tracking-step completed";
        }

        // Stage 3: Assigned to Staff.
        if (hasAssignment)
        {
            stepAssigned.Attributes["class"] =
                "tracking-step completed";
        }
        else if (isInProcess)
        {
            stepAssigned.Attributes["class"] =
                "tracking-step active";
        }

        // Stage 4: Resolution Under Review.
        if (hasResolutionReview)
        {
            stepResolutionReview.Attributes["class"] =
                "tracking-step completed";
        }

        // Stage 5: Resolved.
        if (isResolved)
        {
            stepSubmitted.Attributes["class"] =
                "tracking-step completed";

            stepDepartmentReview.Attributes["class"] =
                "tracking-step completed";

            stepAssigned.Attributes["class"] =
                "tracking-step completed";

            stepResolutionReview.Attributes["class"] =
                "tracking-step completed";

            stepResolved.Attributes["class"] =
                "tracking-step active";

        }

        // Rejected is a separate final outcome.
        if (isRejected)
        {
            stepRejected.Attributes["class"] =
                "tracking-step active rejected-step";

            stepRejected.Visible = true;
        }
        SetCircleIndicator(circleSubmitted, stepSubmitted);
        SetCircleIndicator(circleDepartmentReview, stepDepartmentReview);
        SetCircleIndicator(circleAssigned, stepAssigned);
        SetCircleIndicator(circleResolutionReview, stepResolutionReview);
        SetCircleIndicator(circleResolved, stepResolved);
    }
    private void SetCircleIndicator(
    System.Web.UI.HtmlControls.HtmlGenericControl circle,
    System.Web.UI.HtmlControls.HtmlGenericControl step)
    {
        string cssClass = step.Attributes["class"] ?? "";

        if (step.ID == "stepSubmitted" ||
            cssClass.Contains("completed"))
        {
            circle.InnerHtml = "&#10003;";
            circle.Attributes["aria-label"] = "Completed";
        }
       
    }

    private void LoadFeedbackSection(
        int grievanceID,
        int studentID,
        string grievanceStatus)
    {
        pnlFeedbackForm.Visible = false;
        pnlFeedbackSubmitted.Visible = false;
        pnlFeedbackUnavailable.Visible = false;

        DataTable feedback =
            DALGrievanceFeedback.GetFeedback(grievanceID, studentID);

        if (feedback != null && feedback.Rows.Count > 0)
        {
            DataRow row = feedback.Rows[0];

            pnlFeedbackSubmitted.Visible = true;

            lblSubmittedRating.Text =
                Convert.ToInt32(row["rating"]) + " / 5";

            lblSubmittedComments.Text =
                string.IsNullOrWhiteSpace(
                    Convert.ToString(row["feedbackComments"]))
                ? "No comments provided."
                : Server.HtmlEncode(
                    Convert.ToString(row["feedbackComments"]));

            lblFeedbackDate.Text =
                Convert.ToDateTime(row["feedbackDate"])
                    .ToString("dd MMM yyyy, hh:mm tt");

            return;
        }

        if (string.Equals(
            grievanceStatus,
            "Resolved",
            StringComparison.OrdinalIgnoreCase))
        {
            pnlFeedbackForm.Visible = true;
        }
        else
        {
            pnlFeedbackUnavailable.Visible = true;
        }
    }

    protected void btnSubmitFeedback_Click(object sender, EventArgs e)
    {
        lblFeedbackMessage.Text = "";

        int studentID;
        int grievanceID;

        if (!int.TryParse(
            Convert.ToString(Session["StdID"]),
            out studentID) ||
            studentID <= 0 ||
            !int.TryParse(
                Request.QueryString["id"],
                out grievanceID) ||
            grievanceID <= 0)
        {
            Response.Redirect(
                "~/Student/StudentLogin.aspx",
                false);

            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        // Re-read the Grievance to verify ownership and current status.
        MGrievance grievance =
            DALGrievance.GetGrievanceByID(grievanceID);

        if (grievance == null || grievance.StdID != studentID)
        {
            Response.Redirect(
                "~/Student/StudentDashboard.aspx?view=TrackGrievance",
                false);

            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (!string.Equals(
            grievance.GrievanceStatus,
            "Resolved",
            StringComparison.OrdinalIgnoreCase))
        {
            LoadFeedbackSection(
                grievanceID,
                studentID,
                grievance.GrievanceStatus);

            lblFeedbackMessage.Text =
                "Feedback is available only for resolved Grievances.";

            return;
        }

        int rating;

        if (!int.TryParse(rblRating.SelectedValue, out rating) ||
            rating < 1 ||
            rating > 5)
        {
            pnlFeedbackForm.Visible = true;
            lblFeedbackMessage.Text =
                "Please select a valid rating from 1 to 5.";
            return;
        }

        try
        {
            int result = DALGrievanceFeedback.SubmitFeedback(
                grievanceID,
                studentID,
                rating,
                txtFeedbackComments.Text);

            if (result > 0)
            {
                LoadFeedbackSection(
                    grievanceID,
                    studentID,
                    grievance.GrievanceStatus);
            }
            else
            {
                LoadFeedbackSection(
                    grievanceID,
                    studentID,
                    grievance.GrievanceStatus);

                if (!pnlFeedbackSubmitted.Visible)
                {
                    pnlFeedbackForm.Visible = true;
                    lblFeedbackMessage.Text =
                        "Feedback could not be submitted. Please verify the Grievance status and try again.";
                }
            }
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            System.Diagnostics.Trace.TraceError(
                "Failed to submit feedback for Grievance ID " +
                grievanceID + ": " + ex);

            LoadFeedbackSection(
                grievanceID,
                studentID,
                grievance.GrievanceStatus);

            if (!pnlFeedbackSubmitted.Visible)
            {
                pnlFeedbackForm.Visible = true;
                lblFeedbackMessage.Text =
                    "Feedback could not be submitted. Please try again.";
            }
        }
    }

}