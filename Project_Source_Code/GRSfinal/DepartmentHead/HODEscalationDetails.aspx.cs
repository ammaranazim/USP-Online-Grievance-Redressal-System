
using System;
using System.Data;
using System.Web;
using System.Web.UI;

public partial class DepartmentHead_HODEscalationDetails : System.Web.UI.Page
{
    private int DepartmentID
    {
        get
        {
            return Session["DeptID"] == null
                ? 0
                : Convert.ToInt32(Session["DeptID"]);
        }
    }

    private int DepartmentHeadID
    {
        get
        {
            return Session["DeptHeadID"] == null
                ? 0
                : Convert.ToInt32(Session["DeptHeadID"]);
        }
    }

    private int EscalationID
    {
        get
        {
            int id;
            return int.TryParse(Request.QueryString["id"], out id)
                ? id
                : 0;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (DepartmentHeadID <= 0 || DepartmentID <= 0)
        {
            Response.Redirect("~/DepartmentHead/HODLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (!IsPostBack)
        {
            if (EscalationID <= 0)
            {
                ShowMessage("Invalid escalation reference.", false);
                pnlDetails.Visible = false;
                return;
            }

            LoadEscalationDetails();
        }
    }

    private void LoadEscalationDetails()
    {
        try
        {
            DataTable dt =
                DALEscalatedGrievance.GetHODEscalationByID(
                    EscalationID,
                    DepartmentID
                );

            if (dt == null || dt.Rows.Count == 0)
            {
                pnlDetails.Visible = false;
                ShowMessage(
                    "This escalation was not found in your department.",
                    false
                );
                return;
            }

            DataRow row = dt.Rows[0];

            pnlDetails.Visible = true;

            lblEscalationID.Text = SafeText(row["escalationID"]);
            lblGrievanceID.Text = SafeText(row["grievanceID"]);
            lblRequestedDate.Text = SafeDate(
                row["requestedDate"], "dd MMM yyyy, hh:mm tt");

            lblStudentName.Text = SafeText(row["StudentName"]);
            lblRegistrationNo.Text = SafeText(row["stdRegNo"]);
            lblCategory.Text = SafeText(row["categoryName"]);
            lblDepartment.Text = SafeText(row["deptName"]);
            lblPriority.Text = SafeText(row["grievancePriority"]);
            lblGrievanceTitle.Text = SafeText(row["grievanceTitle"]);
            lblGrievanceDescription.Text =
                SafeText(row["grievanceDescription"]);

            lblEscalationReason.Text =
                SafeText(row["escalationReason"]);

            lblEscalationDescription.Text =
                SafeText(row["escalationDescription"]);

            string status = Convert.ToString(row["requestStatus"]);
            // Show Admin feedback when remarks are available.
            string adminRemarks = Convert.ToString(row["adminRemarks"]);

            bool hasAdminFeedback =
                !string.IsNullOrWhiteSpace(adminRemarks);

            pnlAdminFeedback.Visible = hasAdminFeedback;

            lblAdminRemarks.Text = SafeText(row["adminRemarks"]);

            lblAdminDecisionDate.Text =
                SafeDate(row["adminDecisionDate"], "dd MMM yyyy, hh:mm tt");

            lblAdminFeedbackStatus.Text = HttpUtility.HtmlEncode(status);
            lblAdminFeedbackStatus.CssClass =
     "admin-feedback-badge " + GetAdminFeedbackStatusClass(status);
            DataTable adminActivity =
    DALEscalatedGrievance.GetLatestAdminEscalationActivity(
        EscalationID
    );

            string adminActionType = "Admin Feedback";

            if (adminActivity != null && adminActivity.Rows.Count > 0)
            {
                string activityDescription = Convert.ToString(
                    adminActivity.Rows[0]["trackingactivityDescription"]
                );

                if (activityDescription.StartsWith(
                    "Admin requested additional information",
                    StringComparison.OrdinalIgnoreCase))
                {
                    adminActionType = "Additional Information Requested";
                }
                else if (activityDescription.StartsWith(
                    "Admin issued directions",
                    StringComparison.OrdinalIgnoreCase))
                {
                    adminActionType = "Administrative Direction";
                }
                else if (activityDescription.StartsWith(
                    "Admin took administrative action",
                    StringComparison.OrdinalIgnoreCase))
                {
                    adminActionType = "Administrative Action Taken";
                }
                else if (activityDescription.StartsWith(
                    "Admin closed the escalation",
                    StringComparison.OrdinalIgnoreCase))
                {
                    adminActionType = "Escalation Closed";
                }
            }

            lblAdminActionType.Text = HttpUtility.HtmlEncode(adminActionType);
            switch (adminActionType)
            {
                case "Additional Information Requested":
                    lblAdminRemarksLabel.Text =
                        "Admin Remarks / Additional Information Required";
                    break;

                case "Administrative Direction":
                    lblAdminRemarksLabel.Text =
                        "Administrative Direction / Instructions";
                    break;

                case "Administrative Action Taken":
                    lblAdminRemarksLabel.Text =
                        "Administrative Action / Remarks";
                    break;

                case "Escalation Closed":
                    lblAdminRemarksLabel.Text =
                        "Escalation Closure Remarks";
                    break;

                default:
                    lblAdminRemarksLabel.Text = "Admin Remarks";
                    break;
            }

            // HOD can respond to Admin's request or follow up on action taken.
            bool canRespondToAdmin =
                hasAdminFeedback &&
                (
                    string.Equals(
                        status,
                        "Action Required",
                        StringComparison.OrdinalIgnoreCase
                    )
                    ||
                    string.Equals(
                        status,
                        "Action Taken",
                        StringComparison.OrdinalIgnoreCase
                    )
                );

            pnlHODResponse.Visible = canRespondToAdmin;
            pnlHODResponseLocked.Visible =
                hasAdminFeedback && !canRespondToAdmin;

            btnSubmitHODResponse.Enabled = canRespondToAdmin;
            pnlHODResponse.Visible = canRespondToAdmin;
            pnlHODResponseLocked.Visible =
                hasAdminFeedback && !canRespondToAdmin;

            btnSubmitHODResponse.Enabled = canRespondToAdmin;

            // HOD instructions to Staff visibility
            bool canSendStaffInstruction =
                hasAdminFeedback &&
                (
                    string.Equals(status, "Action Required",
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    string.Equals(status, "Action Taken",
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    string.Equals(status, "Closed",
                        StringComparison.OrdinalIgnoreCase)
                );

            pnlHODStaffInstruction.Visible = canSendStaffInstruction;
            btnSaveHODStaffInstruction.Enabled = canSendStaffInstruction;

            lblRequestStatus.Text = HttpUtility.HtmlEncode(status);
            lblRequestStatus.CssClass = GetEscalationStatusClass(status);

            lblRequestStatus.Text = HttpUtility.HtmlEncode(status);
            lblRequestStatus.CssClass = GetEscalationStatusClass(status);

            bool canReview = string.Equals(
                status,
                "Pending HOD Review",
                StringComparison.OrdinalIgnoreCase
            );

            pnlHODDecision.Visible = canReview;
            pnlDecisionLocked.Visible = !canReview;

            btnForwardToAdmin.Enabled = canReview;
            btnReject.Enabled = canReview;

            LoadTimeline();
            pnlMessage.Visible = false;
        }
        catch (Exception)
        {
            pnlDetails.Visible = false;
            ShowMessage(
                "Unable to load escalation details. Please try again.",
                false
            );
        }
    }

    private void LoadTimeline()
    {
        DataTable timeline =
          DALEscalatedGrievance.GetHODEscalationTimeline(EscalationID);
        rptEscalationTimeline.DataSource = timeline;
        rptEscalationTimeline.DataBind();

        pnlNoTimeline.Visible =
            timeline == null || timeline.Rows.Count == 0;
    }

    protected void btnForwardToAdmin_Click(object sender, EventArgs e)
    {
        ProcessHODDecision(
            "Pending Admin Review",
            "Escalation forwarded to Admin by HOD."
        );
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        ProcessHODDecision(
            "Rejected by HOD",
            "Escalation rejected by HOD."
        );
    }
    protected void btnSubmitHODResponse_Click(object sender, EventArgs e)
    {
        if (DepartmentHeadID <= 0 || DepartmentID <= 0)
        {
            Response.Redirect("~/DepartmentHead/HODLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (EscalationID <= 0)
        {
            ShowMessage("Invalid escalation reference.", false);
            return;
        }

        string response = txtHODAdminResponse.Text.Trim();

        if (string.IsNullOrWhiteSpace(response))
        {
            ShowMessage("Please enter your response to Admin.", false);
            return;
        }

        if (response.Length > 400)
        {
            ShowMessage("Your response cannot exceed 400 characters.", false);
            return;
        }

        try
        {
            int result =
                DALEscalatedGrievance.RecordHODAdditionalInfoResponse(
                    EscalationID,
                    DepartmentID,
                    DepartmentHeadID,
                    response
                );

            if (result == 1)
            {
                try
                {
                    System.Data.DataTable escalation =
                        DALEscalatedGrievance.GetHODEscalationByID(
                            EscalationID,
                            DepartmentID);

                    if (escalation != null && escalation.Rows.Count > 0)
                    {
                        int grievanceID = Convert.ToInt32(
                            escalation.Rows[0]["grievanceID"]);

                        NotifyAdmin(
                            grievanceID,
                            "HOD Response Submitted",
                            "HOD has submitted a response for Grievance ID: "
                            + grievanceID + ".");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(
                        "HOD response notification failed: " + ex.Message);
                }
                txtHODAdminResponse.Text = string.Empty;

                ShowMessage(
                    "Your response has been submitted to Admin successfully.",
                    true
                );

                LoadEscalationDetails();
            }
            else
            {
                ShowMessage(
                    "The response could not be submitted. The escalation may no longer require additional information.",
                    false
                );

                LoadEscalationDetails();
            }
        }
        catch (Exception)
        {
            ShowMessage(
                "An error occurred while submitting your response. Please try again.",
                false
            );
        }
    }
    protected void btnSaveHODStaffInstruction_Click(
    object sender, EventArgs e)
    {
        if (DepartmentHeadID <= 0 || DepartmentID <= 0)
        {
            Response.Redirect("~/DepartmentHead/HODLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (EscalationID <= 0)
        {
            ShowMessage("Invalid escalation reference.", false);
            return;
        }

        string instruction = txtHODStaffInstruction.Text.Trim();

        if (string.IsNullOrWhiteSpace(instruction))
        {
            ShowMessage("Please enter instructions for Staff.", false);
            return;
        }

        if (instruction.Length > 400)
        {
            ShowMessage(
                "Instructions cannot exceed 400 characters.", false);
            return;
        }

        try
        {
            int result =
                DALEscalatedGrievance.AddHODInstructionToStaff(
                    EscalationID,
                    DepartmentID,
                    DepartmentHeadID,
                    instruction
                );

            if (result == 1)
            {
                txtHODStaffInstruction.Text = string.Empty;

                ShowMessage(
                    "Instructions have been saved successfully.", true);

                LoadEscalationDetails();
            }
            else
            {
                ShowMessage(
                    "Instructions could not be saved. Please verify the escalation status and Admin feedback.",
                    false);

                LoadEscalationDetails();
            }
        }
        catch (Exception)
        {
            ShowMessage(
                "An error occurred while saving instructions. Please try again.",
                false);
        }
    }
    private void NotifyAdmin(
    int grievanceID,
    string notificationType,
    string message)
    {
        try
        {
            System.Collections.Generic.List<MLogin> admins =
                DALAdminLogin.SearchAdminLogin();

            if (admins == null || admins.Count == 0)
                return;

            int adminID = admins[0].AdminLoginID;

            DALSystemNotification.InsertNotification(
                "Admin",
                adminID,
                grievanceID,
                notificationType,
                message);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError(
                "Admin notification failed for Grievance ID "
                + grievanceID + ": " + ex.Message);
        }
    }
    private void ProcessHODDecision(
        string newStatus,
        string activityDescription)
    {
        if (DepartmentHeadID <= 0 || DepartmentID <= 0)
        {
            Response.Redirect("~/DepartmentHead/HODLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (EscalationID <= 0)
        {
            ShowMessage("Invalid escalation reference.", false);
            return;
        }

        string remarks = txtHODRemarks.Text.Trim();

        if (string.IsNullOrWhiteSpace(remarks))
        {
            ShowMessage("Please enter HOD remarks before submitting your decision.", false);
            return;
        }

        if (remarks.Length > 2000)
        {
            ShowMessage("HOD remarks cannot exceed 2000 characters.", false);
            return;
        }

        try
        {
            // This DAL method will be added in the next DAL step.
            // It must verify department ownership and Pending HOD Review
            // inside a database transaction before updating the request.
            int result =
                DALEscalatedGrievance.UpdateHODEscalationDecision(
                    EscalationID,
                    DepartmentID,
                    DepartmentHeadID,
                    newStatus,
                    remarks,
                    activityDescription
                );

            if (result == 1)
            {
                if (newStatus == "Pending Admin Review")
                {
                    try
                    {
                        System.Data.DataTable escalation =
                            DALEscalatedGrievance.GetHODEscalationByID(
                                EscalationID,
                                DepartmentID);

                        if (escalation != null && escalation.Rows.Count > 0)
                        {
                            int grievanceID = Convert.ToInt32(
                                escalation.Rows[0]["grievanceID"]);

                            NotifyAdmin(
                                grievanceID,
                                "Grievance Escalated",
                                "Grievance ID: " + grievanceID
                                + " has been escalated by the HOD.");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError(
                            "Escalation notification failed: " + ex.Message);
                    }
                }
                txtHODRemarks.Text = string.Empty;

                ShowMessage(
                    newStatus == "Pending Admin Review"
                        ? "Escalation forwarded to Admin successfully."
                        : "Escalation rejected successfully.",
                    true
                );

                LoadEscalationDetails();
            }
            else
            {
                ShowMessage(
                    "The decision could not be saved. The request may already have been reviewed.",
                    false
                );

                LoadEscalationDetails();
            }
        }
        catch (Exception)
        {
            ShowMessage(
                "An error occurred while saving the HOD decision. Please try again.",
                false
            );
        }
    }

    private string SafeText(object value)
    {
        if (value == null || value == DBNull.Value)
            return "—";

        string text = Convert.ToString(value);

        if (string.IsNullOrWhiteSpace(text))
            return "—";

        return HttpUtility.HtmlEncode(text);
    }

    private string SafeDate(object value, string format)
    {
        if (value == null || value == DBNull.Value)
            return "—";

        DateTime date;

        if (!DateTime.TryParse(Convert.ToString(value), out date))
            return "—";

        return HttpUtility.HtmlEncode(date.ToString(format));
    }


    protected string GetEscalationStatusClass(string status)
    {
        switch ((status ?? string.Empty).Trim().ToLowerInvariant())
        {
            case "pending hod review":
            case "pending admin review":
                return "escalation-status escalation-pending";

            case "rejected by hod":
                return "escalation-status escalation-rejected";

            case "action required":
                return "escalation-status escalation-action";

            case "action taken":
                return "escalation-status escalation-taken";

            case "closed":
                return "escalation-status escalation-closed";

            default:
                return "escalation-status escalation-pending";
        }
    }

    private string GetAdminFeedbackStatusClass(string status)
    {
        switch ((status ?? string.Empty).Trim().ToLowerInvariant())
        {
            case "action required":
                return "escalation-action";

            case "action taken":
                return "escalation-taken";

            case "closed":
                return "escalation-closed";

            default:
                return "escalation-pending";
        }
    }


    private void ShowMessage(string message, bool isSuccess)
    {
        lblMessage.Text = HttpUtility.HtmlEncode(message);
        pnlMessage.Visible = true;

        lblMessage.CssClass = isSuccess
            ? "alert-message alert-success"
            : "alert-message alert-danger";
    }
}
