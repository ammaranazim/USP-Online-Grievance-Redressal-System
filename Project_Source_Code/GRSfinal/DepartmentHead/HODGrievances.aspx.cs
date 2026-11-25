
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DepartmentHead_HODGrievances : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int deptID;

        if (!int.TryParse(
            Convert.ToString(Session["DeptID"]), out deptID))
        {
            Response.Redirect(
                "~/DepartmentHead/HODLogin.aspx", true);
            return;
        }

        if (!IsPostBack)
        {
            BindGrievances();
        }
    }

    private void BindGrievances()
    {
        int deptID;

        if (!int.TryParse(
            Convert.ToString(Session["DeptID"]), out deptID))
        {
            return;
        }

        DateTime parsedDate;
        DateTime? dateFrom = null;
        DateTime? dateTo = null;

        if (!string.IsNullOrWhiteSpace(txtDateFrom.Text))
        {
            if (DateTime.TryParse(txtDateFrom.Text, out parsedDate))
                dateFrom = parsedDate.Date;
        }

        if (!string.IsNullOrWhiteSpace(txtDateTo.Text))
        {
            if (DateTime.TryParse(txtDateTo.Text, out parsedDate))
                dateTo = parsedDate.Date;
        }

        DataTable dt = DALGrievance.GetHODGrievances(
            deptID,
            txtSearch.Text.Trim(),
            ddlStatus.SelectedValue,
            ddlPriority.SelectedValue,
            dateFrom,
            dateTo);

        gvGrievances.DataSource = dt;
        gvGrievances.DataBind();

        lblRecordCount.Text = dt.Rows.Count == 1
            ? "1 Record"
            : dt.Rows.Count + " Records";
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gvGrievances.PageIndex = 0;
        BindGrievances();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlStatus.SelectedIndex = 0;
        pnlAdvancedFilters.Visible = false;
        gvGrievances.PageIndex = 0;
        ddlPriority.SelectedIndex = 0;
        txtDateFrom.Text = "";
        txtDateTo.Text = "";

        BindGrievances();
    }

    protected void btnToggleFilters_Click(
        object sender, EventArgs e)
    {
        pnlAdvancedFilters.Visible =
            !pnlAdvancedFilters.Visible;
    }

    protected void gvGrievances_PageIndexChanging(
        object sender, GridViewPageEventArgs e)
    {
        gvGrievances.PageIndex = e.NewPageIndex;
        BindGrievances();
    }

    protected string GetStatusBadgeClass(object statusValue)
    {
        string status = Convert.ToString(statusValue);

        switch (status)
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

    protected string GetPriorityBadgeClass(object priorityValue)
    {
        string priority = Convert.ToString(priorityValue)
            .Trim()
            .ToLowerInvariant();

        switch (priority)
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

    protected void gvGrievances_RowCommand(
        object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "ViewDetails")
        {
            return;
        }

        int grievanceID;

        if (!int.TryParse(
            Convert.ToString(e.CommandArgument),
            out grievanceID) || grievanceID <= 0)
        {
            return;
        }

        Response.Redirect(
            "~/DepartmentHead/HODGrievanceDetails.aspx?id="
            + grievanceID,
            false);

        Context.ApplicationInstance.CompleteRequest();
    }

}
