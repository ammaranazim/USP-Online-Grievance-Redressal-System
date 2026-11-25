using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Admin_AdminEscalatedGrievances : System.Web.UI.Page
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
            ViewState["EscalationFilter"] = "Total";
            LoadEscalationData(true);
        }
    }

    private void BindEscalatedGrievances(DataTable dt)
    {
        gvEscalatedGrievances.DataSource = dt;
        gvEscalatedGrievances.DataBind();
    }

    private void LoadEscalationSummary(DataTable dt)
    {
        int totalEscalated = 0;
        int pendingReview = 0;
        int actionRequired = 0;

        if (dt != null)
        {
            totalEscalated = dt.Rows.Count;

            foreach (DataRow row in dt.Rows)
            {
                string status = Convert.ToString(row["requestStatus"]).Trim();

                if (string.Equals(
                    status,
                    "Pending Admin Review",
                    StringComparison.OrdinalIgnoreCase))
                {
                    pendingReview++;
                }
                else if (string.Equals(
                    status,
                    "Action Required",
                    StringComparison.OrdinalIgnoreCase))
                {
                    actionRequired++;
                }
            }
        }

        lblTotalEscalated.Text = totalEscalated.ToString();
        lblPendingReview.Text = pendingReview.ToString();
        lblActionRequired.Text = actionRequired.ToString();
    }

    private void LoadEscalationData(bool updateSummary)
    {
        DataTable dt =
            DALEscalatedGrievance.GetAdminEscalatedGrievances();

        if (updateSummary)
        {
            LoadEscalationSummary(dt);
        }

        if (dt == null)
        {
            BindEscalatedGrievances(null);
            return;
        }

        string kpiFilter =
            Convert.ToString(ViewState["EscalationFilter"]);

        if (string.IsNullOrEmpty(kpiFilter))
        {
            kpiFilter = "Total";
        }

        string searchKeyword = txtEscalationSearch.Text.Trim();
        string selectedStatus = ddlEscalationStatusFilter.SelectedValue;

        // Create a separate result table without modifying the original data.
        DataTable filteredTable = dt.Clone();

        foreach (DataRow row in dt.Rows)
        {
            string requestStatus =
                Convert.ToString(row["requestStatus"]).Trim();

            // Apply the selected KPI card filter.
            if (kpiFilter == "Pending" &&
                !string.Equals(
                    requestStatus,
                    "Pending Admin Review",
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (kpiFilter == "ActionRequired" &&
                !string.Equals(
                    requestStatus,
                    "Action Required",
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            // Apply the advanced status filter.
            if (!string.IsNullOrEmpty(selectedStatus) &&
                !string.Equals(
                    requestStatus,
                    selectedStatus,
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            // Apply the search keyword across relevant columns.
            if (!string.IsNullOrWhiteSpace(searchKeyword) &&
                !MatchesEscalationSearch(row, searchKeyword))
            {
                continue;
            }

            filteredTable.ImportRow(row);
        }

        BindEscalatedGrievances(filteredTable);
    }

    private bool MatchesEscalationSearch(
        DataRow row,
        string searchKeyword)
    {
        string[] searchableColumns =
        {
            "escalationID",
            "grievanceID",
            "StudentName",
            "stdRegNo",
            "escalationReason",
            "requestStatus"
        };

        foreach (string columnName in searchableColumns)
        {
            // Check that the column exists before accessing it.
            if (!row.Table.Columns.Contains(columnName))
            {
                continue;
            }

            string columnValue =
                Convert.ToString(row[columnName]);

            if (columnValue.IndexOf(
                searchKeyword,
                StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
        }

        return false;
    }

    protected void btnEscalationSearch_Click(
        object sender,
        EventArgs e)
    {
        gvEscalatedGrievances.PageIndex = 0;
        LoadEscalationData(false);
    }

    protected void btnApplyEscalationFilters_Click(
        object sender,
        EventArgs e)
    {
        gvEscalatedGrievances.PageIndex = 0;
        LoadEscalationData(false);
    }

    protected void btnEscalationClear_Click(
        object sender,
        EventArgs e)
    {
        txtEscalationSearch.Text = string.Empty;
        ddlEscalationStatusFilter.SelectedIndex = 0;

        // Reset the KPI filter as well.
        ViewState["EscalationFilter"] = "Total";

        gvEscalatedGrievances.PageIndex = 0;

        LoadEscalationData(false);
    }

    protected void gvEscalatedGrievances_PageIndexChanging(
        object sender,
        GridViewPageEventArgs e)
    {
        gvEscalatedGrievances.PageIndex = e.NewPageIndex;
        LoadEscalationData(false);
    }

    protected void gvEscalatedGrievances_RowCommand(
        object sender,
        GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ViewEscalation")
        {
            int escalationID;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument),
                out escalationID) ||
                escalationID <= 0)
            {
                return;
            }

            Response.Redirect(
                "~/Admin/AdminEscalationDetails.aspx?id=" + escalationID,
                false);

            Context.ApplicationInstance.CompleteRequest();
        }
    }

    protected string GetEscalationStatusClass(object statusObject)
    {
        string status = Convert.ToString(statusObject).Trim();

        switch (status.ToLowerInvariant())
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
    protected void KpiCard_Command(
        object sender,
        CommandEventArgs e)
    {
        string filter = Convert.ToString(e.CommandArgument);

        if (filter != "Total" &&
            filter != "Pending" &&
            filter != "ActionRequired")
        {
            return;
        }

        ViewState["EscalationFilter"] = filter;
        gvEscalatedGrievances.PageIndex = 0;

        LoadEscalationData(true);
    }
}