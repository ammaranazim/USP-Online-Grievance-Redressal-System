
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DepartmentHead_HODEscalations : System.Web.UI.Page
{
    private int DepartmentHeadID
    {
        get
        {
            return Session["DeptHeadID"] == null
                ? 0
                : Convert.ToInt32(Session["DeptHeadID"]);
        }
    }

    private int DepartmentID
    {
        get
        {
            return Session["DeptID"] == null
                ? 0
                : Convert.ToInt32(Session["DeptID"]);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["DeptHeadID"] == null || Session["DeptID"] == null)
        {
            Response.Redirect("~/DepartmentHead/HODLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (!IsPostBack)
        {
            LoadEscalations();
        }
    }

    private void LoadEscalations()
    {
        try
        {
            // This DAL method will be added in the DAL file step.
            DataTable dt = DALEscalatedGrievance.GetHODEscalations(
                DepartmentID,
                txtSearch.Text.Trim(),
                ddlStatus.SelectedValue
            );

            gvEscalations.DataSource = dt;
            gvEscalations.DataBind();

            lblRecordCount.Text = dt.Rows.Count.ToString();

            pnlMessage.Visible = false;
        }
        catch (Exception)
        {
            gvEscalations.DataSource = null;
            gvEscalations.DataBind();

            lblRecordCount.Text = "0";
            ShowMessage(
                "Unable to load Grievance escalations right now. Please try again.",
                false
            );
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gvEscalations.PageIndex = 0;
        LoadEscalations();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        ddlStatus.SelectedValue = string.Empty;
        gvEscalations.PageIndex = 0;

        LoadEscalations();
    }

    protected void gvEscalations_RowCommand(
        object sender,
        GridViewCommandEventArgs e)
    {
        if (e.CommandName != "ViewDetails")
        {
            return;
        }

        int escalationID;

        if (!int.TryParse(Convert.ToString(e.CommandArgument), out escalationID)
            || escalationID <= 0)
        {
            ShowMessage("Invalid escalation reference.", false);
            return;
        }

        // The details page must verify department ownership again.
        string url = ResolveUrl(
            "~/DepartmentHead/HODEscalationDetails.aspx?id=" +
            escalationID
        );

        Response.Redirect(url, false);
        Context.ApplicationInstance.CompleteRequest();
    }


    protected string GetEscalationStatusClass(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return "escalation-status escalation-pending";
        }

        switch (status.Trim().ToLowerInvariant())
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

    private void ShowMessage(string message, bool isSuccess)
    {
        lblMessage.Text = HttpUtility.HtmlEncode(message);
        pnlMessage.Visible = true;

        lblMessage.CssClass = isSuccess
            ? "alert-message alert-success"
            : "alert-message alert-danger";
    }
}
