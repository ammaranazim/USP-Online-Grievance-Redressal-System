
using System;
using System.Data;
using System.Web;
using System.Web.UI;

public partial class Admin_AdminEscalationDetails : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AdminID"] == null)
        {
            Response.Redirect("~/Admin/AdminLogin.aspx");
            return;
        }

        if (!IsPostBack)
        {
            BindEscalationDetails();

            if (Request.QueryString["saved"] == "1")
            {
                ShowMessage("Admin action saved successfully.");
            }
        }
    }


    private int GetEscalationID()
    {
        int escalationID;

        if (int.TryParse(Request.QueryString["id"], out escalationID)
            && escalationID > 0)
        {
            return escalationID;
        }

        return 0;
    }

    private void BindEscalationDetails()
    {
        int escalationID = GetEscalationID();

        if (escalationID == 0)
        {
            ShowMessage("Invalid escalation ID.");
            SetActionButtonsEnabled(false);
            return;
        }

        try
        {
            DataTable dt =
                DALEscalatedGrievance.GetEscalatedGrievanceByID(
                    escalationID);

            if (dt == null || dt.Rows.Count == 0)
            {
                ShowMessage("Escalation record was not found.");
                SetActionButtonsEnabled(false);
                return;
            }

            DataRow row = dt.Rows[0];

            lblEscalationID.Text =
                "ESC-" + GetFieldValue(row, "escalationID");

            lblGrievanceID.Text =
                "GRV-" + GetFieldValue(row, "grievanceID");

            lblStudentName.Text =
                EncodeValue(GetFieldValue(row, "StudentName"));

            lblStudentRegNo.Text =
                EncodeValue(GetFieldValue(row, "stdRegNo"));

            lblCategory.Text =
                EncodeValue(GetFieldValue(row, "categoryName"));

            lblDepartment.Text =
                EncodeValue(GetFieldValue(row, "deptName"));

            lblGrievanceTitle.Text =
                EncodeValue(GetFieldValue(row, "grievanceTitle"));

            lblEscalationReason.Text =
                EncodeValue(GetFieldValue(row, "escalationReason"));

            lblEscalationDescription.Text =
                EncodeValue(GetFieldValue(row, "escalationDescription"));
            string hodRemarks = GetFieldValue(row, "hodRemarks");

            lblHODRemarks.Text = string.IsNullOrWhiteSpace(hodRemarks)
                ? "No HOD remarks available yet."
                : EncodeValue(hodRemarks);
            string status = GetFieldValue(row, "requestStatus");

            bool isClosed = string.Equals(
                status,
                "Closed",
                StringComparison.OrdinalIgnoreCase
            );

            pnlAdministrativeReview.Visible = !isClosed;
            pnlClosedMessage.Visible = isClosed;

            lblStatus.Text = EncodeValue(status);
            lblStatus.CssClass = GetEscalationStatusClass(status);

            if (row["requestedDate"] != DBNull.Value)
            {
                lblRequestedDate.Text =
                    Convert.ToDateTime(row["requestedDate"])
                        .ToString("dd MMM yyyy, hh:mm tt");
            }
            else
            {
                lblRequestedDate.Text = "Not available";
            }

            BindEscalationTimeline();
            BindLatestHODResponse();

            bool canAdminReview = IsAdminActionAllowed(status);

            bool isPendingAdminReview = string.Equals(
                status,
                "Pending Admin Review",
                StringComparison.OrdinalIgnoreCase
            );

            bool isAwaitingHODResponse =
                string.Equals(status, "Action Required",
                    StringComparison.OrdinalIgnoreCase)
                ||
                string.Equals(status, "Action Taken",
                    StringComparison.OrdinalIgnoreCase);

            bool hasLatestHODResponse =
                HasHODRespondedAfterLatestAdminAction(
                    GetEscalationID()
                );

            // First Admin review: all actions are available.
            // After an Admin action: wait for HOD's response.
            bool canTakeNextDecision =
                isPendingAdminReview ||
                (isAwaitingHODResponse && hasLatestHODResponse);

            txtAdminRemarks.Enabled =
                canAdminReview && canTakeNextDecision;

            // Keep all four actions visible while the review panel is open.
            // Their enabled state follows the escalation workflow.
            btnRequestInfo.Visible = true;
            btnGiveDirection.Visible = true;
            btnTakeAction.Visible = true;
            btnCloseEscalation.Visible = true;

            SetActionButtonsEnabled(
                canAdminReview && canTakeNextDecision
            );

            if (isAwaitingHODResponse && !hasLatestHODResponse)
            {
                ShowMessage(
                    "Waiting for the HOD response. You can take the next administrative decision after the response is submitted."
                );
            }
        }
        catch (Exception)
        {
            ShowMessage(
                "Unable to load escalation details. Please try again.");

            SetActionButtonsEnabled(false);
        }
    }


    private void BindEscalationTimeline()
    {
        int escalationID = GetEscalationID();

        DataTable dt =
            DALEscalatedGrievance.GetAdminEscalationTimeline(
                escalationID);

        if (dt == null || dt.Rows.Count == 0)
        {
            rptEscalationTimeline.DataSource = null;
            rptEscalationTimeline.DataBind();

            pnlNoHistory.Visible = true;
        }
        else
        {
            rptEscalationTimeline.DataSource = dt;
            rptEscalationTimeline.DataBind();

            pnlNoHistory.Visible = false;
        }
    }

    private void BindLatestHODResponse()
    {
        int escalationID = GetEscalationID();

        DataTable dt =
            DALEscalatedGrievance.GetLatestHODAdminResponse(
                escalationID);

        if (dt == null || dt.Rows.Count == 0)
        {
            lblLatestHODResponse.Text =
                "No HOD response has been submitted yet.";

            lblLatestHODResponseDate.Text = "Not available";
            return;
        }

        DataRow row = dt.Rows[0];

        string activity = GetFieldValue(
            row, "trackingactivityDescription");

        string response = activity;

        string[] prefixes =
    {
        "HOD response to Admin's additional information request: ",
        "HOD response to Admin's escalation action: "
    };

        foreach (string prefix in prefixes)
        {
            if (response.StartsWith(
                prefix, StringComparison.OrdinalIgnoreCase))
            {
                response = response.Substring(prefix.Length);
                break;
            }
        }

        lblLatestHODResponse.Text = EncodeValue(response);

        if (row["trackingactivitydatetime"] != DBNull.Value)
        {
            lblLatestHODResponseDate.Text =
                Convert.ToDateTime(row["trackingactivitydatetime"])
                    .ToString("dd MMM yyyy, hh:mm tt");
        }
        else
        {
            lblLatestHODResponseDate.Text = "Not available";
        }
    }

    private string GetFieldValue(DataRow row, string columnName)
    {
        if (row == null ||
            !row.Table.Columns.Contains(columnName) ||
            row[columnName] == DBNull.Value)
        {
            return string.Empty;
        }

        return row[columnName].ToString();
    }

    private string EncodeValue(string value)
    {
        return HttpUtility.HtmlEncode(value ?? string.Empty);
    }

    private bool IsAdminActionAllowed(string status)
    {
        return string.Equals(status, "Pending Admin Review", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(status, "Action Required", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(status, "Action Taken", StringComparison.OrdinalIgnoreCase);
    }
    private bool HasHODRespondedAfterLatestAdminAction(int escalationID)
    {
        if (escalationID <= 0)
            return false;

        DataTable adminActivity =
            DALEscalatedGrievance.GetLatestAdminEscalationActivity(escalationID);

        DataTable hodResponse =
            DALEscalatedGrievance.GetLatestHODAdminResponse(escalationID);

        if (adminActivity == null || adminActivity.Rows.Count == 0 ||
            hodResponse == null || hodResponse.Rows.Count == 0 ||
            !adminActivity.Columns.Contains("trackID") ||
            !hodResponse.Columns.Contains("trackID") ||
            adminActivity.Rows[0]["trackID"] == DBNull.Value ||
            hodResponse.Rows[0]["trackID"] == DBNull.Value)
        {
            return false;
        }

        // trackID is ordered by insertion, avoiding equal-timestamp edge cases.
        long adminTrackID = Convert.ToInt64(adminActivity.Rows[0]["trackID"]);
        long hodTrackID = Convert.ToInt64(hodResponse.Rows[0]["trackID"]);

        return hodTrackID > adminTrackID;
    }
    private void SetActionButtonsEnabled(bool enabled)
    {
        btnRequestInfo.Enabled = enabled;
        btnGiveDirection.Enabled = enabled;
        btnTakeAction.Enabled = enabled;
        btnCloseEscalation.Enabled = enabled;
    }

    private void ShowMessage(string message)
    {
        lblMessage.Text = EncodeValue(message);
    }
    private void NotifyHODForAdminAction(
    int grievanceID,
    int departmentID,
    string notificationType,
    string message)
    {
        try
        {
            if (grievanceID <= 0 || departmentID <= 0)
                return;

            System.Data.DataTable dt = null;

            string connectionString =
                System.Configuration.ConfigurationManager
                    .ConnectionStrings["GRS"].ConnectionString;

            using (System.Data.SqlClient.SqlConnection con =
                new System.Data.SqlClient.SqlConnection(connectionString))
            {
                string query = @"
                SELECT TOP 1 deptHeadID
                FROM DepartmentHead
                WHERE deptID = @deptID
                  AND deptHeadStatus = 'Active'
                ORDER BY deptHeadID;";

                using (System.Data.SqlClient.SqlCommand cmd =
                    new System.Data.SqlClient.SqlCommand(query, con))
                {
                    cmd.Parameters.Add(
                        "@deptID", System.Data.SqlDbType.Int).Value =
                        departmentID;

                    con.Open();

                    object value = cmd.ExecuteScalar();

                    if (value == null || value == DBNull.Value)
                        return;

                    int hodID = Convert.ToInt32(value);

                    DALSystemNotification.InsertNotification(
                        "HOD",
                        hodID,
                        grievanceID,
                        notificationType,
                        message);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError(
                "Failed to send Admin-to-HOD notification for Grievance ID "
                + grievanceID + ": " + ex.Message);
        }
    }
    private void SaveAdminAction(
        string newStatus,
        string activityDescription)
    {
        lblMessage.Text = string.Empty;

        if (Session["AdminID"] == null)
        {
            Response.Redirect("~/Admin/AdminLogin.aspx");
            return;
        }

        int escalationID = GetEscalationID();
        int adminID;

        if (escalationID == 0)
        {
            ShowMessage("Invalid escalation ID.");
            SetActionButtonsEnabled(false);
            return;
        }

        if (!int.TryParse(
            Convert.ToString(Session["AdminID"]), out adminID)
            || adminID <= 0)
        {
            ShowMessage("Invalid Admin session. Please log in again.");
            SetActionButtonsEnabled(false);
            return;
        }

        string adminRemarks = txtAdminRemarks.Text.Trim();

        if (string.IsNullOrWhiteSpace(adminRemarks))
        {
            ShowMessage("Please enter Admin remarks before taking action.");
            txtAdminRemarks.Focus();
            return;
        }

        try
        {
            // Recheck the current status before saving an Admin action.
            DataTable dt =
                DALEscalatedGrievance.GetEscalatedGrievanceByID(
                    escalationID);

            if (dt == null || dt.Rows.Count == 0)
            {
                ShowMessage("Escalation record was not found.");
                SetActionButtonsEnabled(false);
                return;
            }

            string currentStatus =
                GetFieldValue(dt.Rows[0], "requestStatus");

            if (!IsAdminActionAllowed(currentStatus))
            {
                ShowMessage(
                    "This escalation is not available for Admin action.");

                BindEscalationDetails();
                return;
            }

            // The initial Admin review needs no prior HOD response.
            // Every subsequent Admin action must wait for a newer HOD response.
            bool isInitialAdminReview = string.Equals(
                currentStatus,
                "Pending Admin Review",
                StringComparison.OrdinalIgnoreCase);

            if (!isInitialAdminReview &&
                !HasHODRespondedAfterLatestAdminAction(escalationID))
            {
                ShowMessage(
                    "Please wait for the HOD response before taking another action.");

                BindEscalationDetails();
                return;
            }

            string activity =
                activityDescription + " Remarks: " + adminRemarks;

            int result =
                DALEscalatedGrievance.UpdateAdminEscalation(
                    escalationID,
                    newStatus,
                    adminRemarks,
                    adminID,
                    activity);
            if (result == 1)
            {
                try
                {
                    int grievanceID = Convert.ToInt32(
                        dt.Rows[0]["grievanceID"]);

                    int departmentID = Convert.ToInt32(
                        dt.Rows[0]["deptID"]);

                    string notificationType;
                    string notificationMessage;

                    switch (newStatus)
                    {
                        case "Action Required":
                            notificationType = "Admin Action Required";
                            notificationMessage =
                                "Admin has requested action for Grievance ID: "
                                + grievanceID + ".";
                            break;

                        case "Action Taken":
                            notificationType = "Admin Action Taken";
                            notificationMessage =
                                "Admin has taken administrative action on Grievance ID: "
                                + grievanceID + ".";
                            break;

                        case "Closed":
                            notificationType = "Escalation Closed";
                            notificationMessage =
                                "Escalation for Grievance ID: "
                                + grievanceID + " has been closed.";
                            break;

                        default:
                            notificationType = "Admin Escalation Update";
                            notificationMessage =
                                "Admin has updated the escalation for Grievance ID: "
                                + grievanceID + ".";
                            break;
                    }

                    NotifyHODForAdminAction(
                        grievanceID,
                        departmentID,
                        notificationType,
                        notificationMessage);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(
                        "Admin action notification failed: " + ex.Message);
                }

                string redirectUrl =
                    "~/Admin/AdminEscalationDetails.aspx?id="
                    + escalationID
                    + "&saved=1";

                Response.Redirect(redirectUrl, false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
            else
            {
                ShowMessage(
                    "The action could not be saved. Please verify the escalation status and try again.");

                BindEscalationDetails();
            }
        }
        catch (Exception)
        {
            ShowMessage(
                "An error occurred while saving the Admin action. Please try again.");
        }
    }

    protected void btnRequestInfo_Click(object sender, EventArgs e)
    {
        SaveAdminAction(
            "Action Required",
            "Admin requested additional information for the escalated grievance.");
    }

    protected void btnGiveDirection_Click(object sender, EventArgs e)
    {
        SaveAdminAction(
            "Action Required",
            "Admin issued directions regarding the escalated grievance.");
    }

    protected void btnTakeAction_Click(object sender, EventArgs e)
    {
        SaveAdminAction(
            "Action Taken",
            "Admin took administrative action on the escalated grievance.");
    }

    protected void btnCloseEscalation_Click(object sender, EventArgs e)
    {
        SaveAdminAction(
            "Closed",
            "Admin closed the escalation.");
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Admin/AdminEscalatedGrievances.aspx");
    }

    private string GetEscalationStatusClass(string status)
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

}
