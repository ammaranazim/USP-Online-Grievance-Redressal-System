using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;

public partial class Staff_StaffGrievanceDetails : System.Web.UI.Page
{
    private int GrievanceID
    {
        get
        {
            int id;
            return int.TryParse(Request.QueryString["grievanceID"], out id)
                ? id
                : 0;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["StaffID"] == null)
        {
            Response.Redirect("~/Staff/StaffLogin.aspx", false);
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
        if (GrievanceID <= 0)
        {
            ShowMessage("Invalid Grievance ID.", "alert-danger");
            pnlGrievanceDetails.Visible = false;
            return;
        }

        int staffID = Convert.ToInt32(Session["StaffID"]);

        DataTable dt = DALGrievance.GetStaffGrievanceDetailsByID(
            GrievanceID, staffID);

        if (dt == null || dt.Rows.Count == 0)
        {
            pnlGrievanceDetails.Visible = false;
            ShowMessage(
                "This Grievance is not currently assigned to your account, or you do not have access to it.",
                "alert-warning");
            return;
        }

        DataRow row = dt.Rows[0];

        lblGrievanceID.Text = Convert.ToString(row["grievanceID"]);
        lblGrievanceTitle.Text = Convert.ToString(row["grievanceTitle"]);
        lblCategory.Text = Convert.ToString(row["categoryName"]);
        lblDepartment.Text = Convert.ToString(row["deptName"]);
        lblPriority.Text = Convert.ToString(row["grievancePriority"]);
        lblGrievanceStatus.Text = Convert.ToString(row["grievanceStatus"]);
        lblDescription.Text = Convert.ToString(row["grievanceDescription"]);

        lblSubmissionDate.Text = FormatDate(row["grievanceSubmissionDate"]);
        lblDueDate.Text = FormatDate(row["grievanceDueDate"]);

        pnlGrievanceDetails.Visible = true;

        // Load only the escalation workflow associated with this Staff member's assigned Grievance.
        LoadStaffEscalationWorkflow(staffID);



        string currentStatus = Convert.ToString(row["grievanceStatus"]);

        bool isInProcess = string.Equals(
            currentStatus,
            "In Process",
            StringComparison.OrdinalIgnoreCase
        );
        // Show escalation only while the Grievance is In Process.
        pnlStaffEscalation.Visible = isInProcess;

        // Set escalation button/status based on the latest escalation activity.
        LoadStaffEscalationRequestState(staffID);

        DataTable resolution = DALGrievance.GetStaffResolutionByGrievanceID(
            GrievanceID,
            staffID
        );

        LoadResolutionEvidence(staffID);

        string reviewStatus = string.Empty;
        string hodRemarks = string.Empty;

        bool hasResolution = resolution != null && resolution.Rows.Count > 0;

        if (hasResolution)
        {
            DataRow resolutionRow = resolution.Rows[0];

            reviewStatus = Convert.ToString(resolutionRow["reviewStatus"]);

            txtProposedResolution.Text =
                Convert.ToString(resolutionRow["proposedResolution"]);

            txtStaffRemarks.Text =
                resolutionRow["staffRemarks"] == DBNull.Value
                    ? string.Empty
                    : Convert.ToString(resolutionRow["staffRemarks"]);

            hodRemarks =
                resolutionRow["hodRemarks"] == DBNull.Value
                    ? string.Empty
                    : Convert.ToString(resolutionRow["hodRemarks"]);
        }
        else
        {
            txtProposedResolution.Text = string.Empty;
            txtStaffRemarks.Text = string.Empty;
        }

        bool isReturned = string.Equals(
            reviewStatus,
            "Returned",
            StringComparison.OrdinalIgnoreCase
        );

        bool isPending = string.Equals(
            reviewStatus,
            "Pending",
            StringComparison.OrdinalIgnoreCase
        );

        bool isApproved = string.Equals(
            reviewStatus,
            "Approved",
            StringComparison.OrdinalIgnoreCase
        );

        // First submission or revision after HOD returns the resolution.
        bool canSubmitResolution =
            isInProcess &&
            (!hasResolution || isReturned);

        // Never allow another submission while a resolution is pending
        // or after the latest resolution has been approved.
        if (isPending || isApproved)
        {
            canSubmitResolution = false;
        }

        bool hasStartedWorking =
     DALGrievance.HasStaffStartedWorking(GrievanceID, staffID);

        btnStartWorking.Visible = isInProcess;
        btnStartWorking.Enabled = isInProcess && !hasStartedWorking;

        btnSubmitResolution.Enabled = canSubmitResolution;
        txtProposedResolution.ReadOnly = !canSubmitResolution;
        txtStaffRemarks.ReadOnly = !canSubmitResolution;
        fuResolutionDocuments.Enabled = canSubmitResolution;

        // HOD feedback controls are shown only for a returned resolution.
        pnlHODFeedback.Visible = isInProcess && isReturned;

        lblHODRemarks.Text = string.IsNullOrWhiteSpace(hodRemarks)
            ? string.Empty
            : Server.HtmlEncode(hodRemarks);

        pnlNoHODRemarks.Visible =
            pnlHODFeedback.Visible &&
            string.IsNullOrWhiteSpace(hodRemarks);

    }


    private void LoadStaffEscalationRequestState(int staffID)
    {
        // Default: allow a new request and keep the form collapsed.
        btnToggleEscalation.Visible = true;
        pnlEscalationStatus.Visible = false;
        pnlEscalationForm.CssClass = "escalation-form-hidden";

        int escalationID =
            DALEscalatedGrievance.GetStaffEscalationIDForGrievance(
                GrievanceID, staffID);

        if (escalationID <= 0)
            return;

        // Read the full timeline internally to detect the HOD decision.
        // This data is used only for button logic and is not bound to the Staff UI.
        DataTable timeline =
            DALEscalatedGrievance.GetEscalationTimeline(escalationID);

        if (timeline == null || timeline.Rows.Count == 0)
            return;

        string latestActivity = string.Empty;

        // Timeline is returned oldest-to-newest, so the final row is latest.
        for (int i = timeline.Rows.Count - 1; i >= 0; i--)
        {
            string activity = Convert.ToString(
                timeline.Rows[i]["trackingactivityDescription"]);

            if (!string.IsNullOrWhiteSpace(activity))
            {
                latestActivity = activity.Trim();
                break;
            }
        }

        bool rejectedByHOD = string.Equals(
            latestActivity,
            "Escalation rejected by HOD.",
            StringComparison.OrdinalIgnoreCase);

        // Only a rejected escalation allows the Staff member to submit again.
        // For all other existing escalation states, hide the request button.
        if (rejectedByHOD)
        {
            btnToggleEscalation.Visible = true;
            pnlEscalationStatus.Visible = true;
            lblEscalationStatus.Text =
                "Your previous escalation was rejected by the HOD. You may submit a new request.";
        }
        else
        {
            btnToggleEscalation.Visible = false;
            pnlEscalationStatus.Visible = true;
            lblEscalationStatus.Text =
                "An escalation request already exists for this Grievance.";
        }
    }

    private void LoadStaffEscalationWorkflow(int staffID)
    {
        pnlStaffEscalationTimeline.Visible = false;
        pnlHODStaffInstruction.Visible = false;
        pnlStaffProgress.Visible = false;
        pnlNoStaffTimeline.Visible = false;
        rptStaffEscalationTimeline.DataSource = null;
        rptStaffEscalationTimeline.DataBind();

        int escalationID = DALEscalatedGrievance.GetStaffEscalationIDForGrievance(
            GrievanceID, staffID);

        if (escalationID <= 0)
            return;

        DataTable timeline = DALEscalatedGrievance.GetStaffEscalationTimeline(
            escalationID);

        pnlStaffEscalationTimeline.Visible = true;

        bool hasHODInstruction = false;

        if (timeline != null && timeline.Rows.Count > 0)
        {
            rptStaffEscalationTimeline.DataSource = timeline;
            rptStaffEscalationTimeline.DataBind();

            // Display the latest HOD instruction separately for clarity.
            for (int i = timeline.Rows.Count - 1; i >= 0; i--)
            {
                string activity = Convert.ToString(
                    timeline.Rows[i]["trackingactivityDescription"]);

                const string prefix = "HOD instruction to Staff:";
                if (activity.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    lblLatestHODStaffInstruction.Text =
                        Server.HtmlEncode(activity.Substring(prefix.Length).Trim());
                    pnlHODStaffInstruction.Visible = true;
                    hasHODInstruction = true;
                    break;
                }
            }
        }
        else
        {
            pnlNoStaffTimeline.Visible = true;
        }

        // Only enable progress submission after an HOD instruction is available.
        // The DAL performs the authoritative assignment/status/instruction validation.
        pnlStaffProgress.Visible = hasHODInstruction;
    }

    protected void btnSubmitStaffEscalationProgress_Click(object sender, EventArgs e)
    {
        if (Session["StaffID"] == null)
        {
            Response.Redirect("~/Staff/StaffLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (GrievanceID <= 0)
        {
            ShowMessage("Invalid Grievance ID.", "alert-danger");
            return;
        }

        string progress = txtStaffEscalationProgress.Text.Trim();

        if (string.IsNullOrWhiteSpace(progress))
        {
            ShowMessage("Please enter your progress update.", "alert-danger");
            return;
        }

        if (progress.Length > 2000)
        {
            ShowMessage("The progress update cannot exceed 2,000 characters.", "alert-danger");
            return;
        }

        try
        {
            int staffID = Convert.ToInt32(Session["StaffID"]);

            // Revalidate that this Grievance is still assigned to the logged-in Staff member.
            DataTable assignedGrievance =
                DALGrievance.GetStaffGrievanceDetailsByID(GrievanceID, staffID);

            if (assignedGrievance == null || assignedGrievance.Rows.Count == 0)
            {
                ShowMessage("This Grievance is not currently assigned to your account.", "alert-warning");
                return;
            }

            int escalationID =
                DALEscalatedGrievance.GetStaffEscalationIDForGrievance(
                    GrievanceID, staffID);

            if (escalationID <= 0)
            {
                ShowMessage("No escalation is available for this assigned Grievance.", "alert-warning");
                LoadGrievanceDetails();
                return;
            }

            int result = DALEscalatedGrievance.AddStaffEscalationProgress(
                escalationID, staffID, progress);

            if (result == 1)
            {
                NotifyHOD(
                    GrievanceID,
                    "Staff Progress Update",
                    "Staff has submitted a progress update for Grievance (ID: "
                    + GrievanceID + "). Please review the latest progress.");

                txtStaffEscalationProgress.Text = string.Empty;

                ShowMessage(
                    "Your progress update has been recorded successfully.",
                    "alert-success");
            }
            else
            {
                ShowMessage(
                    "Progress could not be saved. Verify that the HOD has sent an instruction and that this escalation is still eligible for updates.",
                    "alert-warning");
            }

            LoadGrievanceDetails();
        }
        catch (Exception)
        {
            ShowMessage("An error occurred while saving your progress update. Please try again.", "alert-danger");
        }
    }

    private void LoadResolutionEvidence(int staffID)
    {
        List<MEvidence> evidence =
            DALEvidence.GetResolutionEvidenceByGrievanceID(
                GrievanceID, staffID);

        rptResolutionEvidence.DataSource = evidence;
        rptResolutionEvidence.DataBind();

        pnlResolutionEvidence.Visible =
            evidence != null && evidence.Count > 0;
    }

    private string FormatDate(object value)
    {
        if (value == null || value == DBNull.Value)
            return "Not specified";

        DateTime date;

        if (DateTime.TryParse(Convert.ToString(value), out date))
            return date.ToString("dd MMM yyyy");

        return "Not specified";
    }

    protected void btnStartWorking_Click(object sender, EventArgs e)
    {
        if (Session["StaffID"] == null)
        {
            Response.Redirect("~/Staff/StaffLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (GrievanceID <= 0)
        {
            ShowMessage("Invalid Grievance ID.", "alert-danger");
            return;
        }

        try
        {
            int staffID = Convert.ToInt32(Session["StaffID"]);

            int result = DALGrievance.StartStaffGrievanceWork(
                GrievanceID, staffID);

            if (result == 1)
            {
                btnStartWorking.Enabled = false;

                ShowMessage(
                    "Work started successfully. Your action has been recorded.",
                    "alert-success");
            }
            else if (result == 2)
            {
                ShowMessage(
                    "You have already recorded the Start Working action for this assignment.",
                    "alert-info");
            }
            else
            {
                ShowMessage(
                    "The Grievance could not be started. Please verify its assignment and status.",
                    "alert-warning");
            }

            LoadGrievanceDetails();
        }
        catch (Exception)
        {
            ShowMessage(
                "An error occurred while recording your action. Please try again.",
                "alert-danger");
        }
    }

    protected void btnSubmitResolution_Click(object sender, EventArgs e)
    {
        if (Session["StaffID"] == null)
        {
            Response.Redirect("~/Staff/StaffLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (GrievanceID <= 0)
        {
            ShowMessage("Invalid Grievance ID.", "alert-danger");
            return;
        }

        string proposedResolution = txtProposedResolution.Text.Trim();
        string staffRemarks = txtStaffRemarks.Text.Trim();

        if (string.IsNullOrWhiteSpace(proposedResolution))
        {
            ShowMessage(
                "Please enter the proposed resolution before submitting.",
                "alert-danger");
            return;
        }

        if (proposedResolution.Length > 10000)
        {
            ShowMessage(
                "The proposed resolution cannot exceed 10,000 characters.",
                "alert-danger");
            return;
        }

        if (staffRemarks.Length > 2000)
        {
            ShowMessage(
                "Staff remarks cannot exceed 2,000 characters.",
                "alert-danger");
            return;
        }

        string[] allowedExtensions =
        {
            ".doc", ".docx", ".pdf", ".png", ".jpg"
        };

        const int maxFileSizeBytes = 2 * 1024 * 1024;
        const int maxFileCount = 2;

        List<HttpPostedFile> selectedFiles = new List<HttpPostedFile>();

        if (fuResolutionDocuments.HasFiles)
        {
            if (fuResolutionDocuments.PostedFiles.Count > maxFileCount)
            {
                ShowMessage(
                    "You can upload a maximum of 2 supporting documents.",
                    "alert-danger");
                return;
            }

            foreach (HttpPostedFile postedFile in
                fuResolutionDocuments.PostedFiles)
            {
                if (postedFile == null || postedFile.ContentLength <= 0)
                    continue;

                if (postedFile.ContentLength > maxFileSizeBytes)
                {
                    ShowMessage(
                        "Each supporting document must be 2 MB or smaller.",
                        "alert-danger");
                    return;
                }

                string originalName = Path.GetFileName(postedFile.FileName);
                string extension =
                    Path.GetExtension(originalName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ShowMessage(
                        "Only DOC, DOCX, PDF, PNG, and JPG files are allowed.",
                        "alert-danger");
                    return;
                }

                selectedFiles.Add(postedFile);
            }
        }

        int staffID = Convert.ToInt32(Session["StaffID"]);
        string uploadedBy = "Staff:" + staffID;

        string uploadFolder = Server.MapPath(
            "~/UploadedFiles/Grievance/" + GrievanceID + "/");

        List<string> savedFilePaths = new List<string>();
        List<MEvidence> evidenceList = new List<MEvidence>();

        try
        {
            if (selectedFiles.Count > 0)
                Directory.CreateDirectory(uploadFolder);

            foreach (HttpPostedFile postedFile in selectedFiles)
            {
                string originalName = Path.GetFileName(postedFile.FileName);
                string extension =
                    Path.GetExtension(originalName).ToLowerInvariant();

                string storedFileName = originalName;
                string savePath = Path.Combine(uploadFolder, storedFileName);

                if (File.Exists(savePath))
                {
                    storedFileName =
                        Path.GetFileNameWithoutExtension(originalName) +
                        "_" + Guid.NewGuid().ToString("N") + extension;

                    savePath = Path.Combine(uploadFolder, storedFileName);
                }

                postedFile.SaveAs(savePath);
                savedFilePaths.Add(savePath);

                string relativePath =
                    "UploadedFiles/Grievance/" + GrievanceID +
                    "/" + storedFileName;

                evidenceList.Add(new MEvidence(
                    0,
                    storedFileName,
                    "Resolution supporting document",
                    DateTime.Now,
                    uploadedBy,
                    relativePath,
                    GrievanceID));
            }

            int result = DALGrievance.SubmitStaffResolution(
                GrievanceID,
                staffID,
                proposedResolution,
                staffRemarks,
                uploadedBy,
                evidenceList);

            if (result == 1)
            {
                NotifyHOD(
                    GrievanceID,
                    "Resolution Submitted",
                    "Staff has submitted a proposed resolution for Grievance (ID: "
                    + GrievanceID + "). Your review is required.");

                txtProposedResolution.Text = "";
                txtStaffRemarks.Text = "";

                ShowMessage(
                    "Proposed resolution submitted successfully for HOD review.",
                    "alert-success");

                LoadGrievanceDetails();
                return;
            }

            DeleteSavedFiles(savedFilePaths);

            if (result == 2)
            {
                ShowMessage(
                    "A resolution is already pending review for this Grievance.",
                    "alert-warning");
            }
            else
            {
                ShowMessage(
                    "The resolution could not be submitted. Please verify your assignment and the Grievance status.",
                    "alert-warning");
            }
        }
        catch (Exception)
        {
            DeleteSavedFiles(savedFilePaths);

            ShowMessage(
                "An error occurred while submitting the proposed resolution. Please try again.",
                "alert-danger");
        }
    }
    protected void btnSubmitEscalation_Click(object sender, EventArgs e)
    {
        if (Session["StaffID"] == null)
        {
            Response.Redirect("~/Staff/StaffLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (GrievanceID <= 0)
        {
            ShowMessage("Invalid Grievance ID.", "alert-danger");
            return;
        }

        string reason = ddlEscalationReason.SelectedValue;
        string description = txtEscalationDescription.Text.Trim();

        // Validate the reason against the permitted dropdown options.
        string[] allowedReasons =
    {
        "Outside my authority",
        "Requires additional resources",
        "Complex issue requiring higher-level review",
        "Other"
    };

        if (!allowedReasons.Contains(reason))
        {
            ShowMessage(
                "Please select a valid reason for escalation.",
                "alert-danger");
            return;
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            ShowMessage(
                "Please provide a justification for the escalation request.",
                "alert-danger");
            return;
        }

        if (description.Length > 2000)
        {
            ShowMessage(
                "The justification cannot exceed 2,000 characters.",
                "alert-danger");
            return;
        }

        try
        {
            int staffID = Convert.ToInt32(Session["StaffID"]);

            int result =
                DALEscalatedGrievance.CreateStaffEscalationRequest(
                    GrievanceID,
                    staffID,
                    reason,
                    description);

            if (result == 1)
            {
                pnlEscalationStatus.Visible = true;
                pnlEscalationForm.Visible = true;
                pnlEscalationForm.CssClass = "escalation-form-hidden";
                NotifyHOD(
    GrievanceID,
    "Escalation Request Submitted",
    "Staff has submitted an escalation request for Grievance (ID: "
    + GrievanceID + "). Your review is required.");

                lblEscalationStatus.Text =
                    "Your escalation request has been submitted successfully " +
                    "and is awaiting HOD review.";

                txtEscalationDescription.Text = string.Empty;

                ShowMessage(
                    "Escalation request submitted successfully.",
                    "alert-success");

                LoadGrievanceDetails();
            }
            else if (result == 2)
            {
                pnlEscalationStatus.Visible = true;
                pnlEscalationForm.Visible = true;
                pnlEscalationForm.CssClass = "escalation-form-hidden";


                lblEscalationStatus.Text =
                    "An escalation request for this Grievance is already " +
                    "pending HOD review.";

                ShowMessage(
                    "A pending escalation request already exists for this Grievance.",
                    "alert-warning");

                LoadGrievanceDetails();
            }
            else
            {
                ShowMessage(
                    "The escalation request could not be submitted. " +
                    "Please verify your current assignment and try again.",
                    "alert-warning");
            }
        }
        catch (Exception)
        {
            ShowMessage(
                "An error occurred while submitting the escalation request. " +
                "Please try again.",
                "alert-danger");
        }
    }
    private void NotifyHOD(
    int grievanceID,
    string notificationType,
    string message)
    {
        try
        {
            int hodID = 0;

            string connectionString =
                ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT TOP 1 dh.deptHeadID
                FROM DepartmentHead dh
                INNER JOIN Grievance g
                    ON g.deptID = dh.deptID
                WHERE g.grievanceID = @grievanceID
                  AND dh.deptHeadStatus = 'Active'
                ORDER BY dh.deptHeadID;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                        .Value = grievanceID;

                    con.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        hodID = Convert.ToInt32(result);
                    }
                }
            }

            if (hodID > 0)
            {
                DALSystemNotification.InsertNotification(
                    "HOD",
                    hodID,
                    grievanceID,
                    notificationType,
                    message);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError(
                "Failed to create HOD notification for Grievance ID " +
                grievanceID + ": " + ex);
        }
    }
    private void DeleteSavedFiles(List<string> savedFilePaths)
    {
        foreach (string savedPath in savedFilePaths)
        {
            try
            {
                if (File.Exists(savedPath))
                    File.Delete(savedPath);
            }
            catch (Exception)
            {
                // Preserve the original submission result if cleanup fails.
            }
        }
    }

    private void ShowMessage(string message, string alertClass)
    {
        pnlMessage.Visible = true;
        pnlMessage.CssClass = "alert " + alertClass;
        lblMessage.Text = message;
    }
}