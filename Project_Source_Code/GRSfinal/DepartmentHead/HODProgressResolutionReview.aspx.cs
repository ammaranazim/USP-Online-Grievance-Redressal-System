
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DepartmentHead_HODProgressResolutionReview : Page
{
    private int DeptID
    {
        get
        {
            if (Session["DeptID"] == null)
                return 0;

            int deptID;
            return int.TryParse(Convert.ToString(Session["DeptID"]), out deptID)
                ? deptID
                : 0;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["DeptHeadID"] == null || DeptID <= 0)
        {
            Session.Clear();
            Response.Redirect("~/DepartmentHead/HODLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (!IsPostBack)
        {
            // Automatically apply the Overdue filter when coming
            // from the HOD Dashboard.
            string dueDateFilter =
                Convert.ToString(Request.QueryString["dueDate"]);

            if (string.Equals(
                dueDateFilter,
                "Overdue",
                StringComparison.OrdinalIgnoreCase))
            {
                ListItem overdueItem =
                    ddlDueDate.Items.FindByValue("Overdue");

                if (overdueItem != null)
                {
                    ddlDueDate.ClearSelection();
                    overdueItem.Selected = true;
                }
            }

            string stageFilter =
                Convert.ToString(Request.QueryString["stage"]);

            if (string.Equals(
                stageFilter,
                "Resolution Under Review",
                StringComparison.OrdinalIgnoreCase))
            {
                ListItem reviewItem =
                    ddlStage.Items.FindByValue("Resolution Under Review");

                if (reviewItem != null)
                {
                    ddlStage.ClearSelection();
                    reviewItem.Selected = true;
                }
            }

            BindProgressGrid();
        }
    }

    private void BindProgressGrid()
    {
        DataTable dt = DALGrievance.GetHODProgressResolutionOverview(
            DeptID,
            txtSearch.Text.Trim(),
            ddlStatus.SelectedValue,
            ddlPriority.SelectedValue,
            ddlStage.SelectedValue,
            ddlDueDate.SelectedValue
        );

        gvProgress.DataSource = dt;
        gvProgress.DataBind();

        lblRecordCount.Text = "Records: " + dt.Rows.Count;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindProgressGrid();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlStatus.SelectedIndex = 0;
        ddlPriority.SelectedIndex = 0;
        ddlStage.SelectedIndex = 0;
        ddlDueDate.SelectedIndex = 0;

        BindProgressGrid();
    }

    protected void gvProgress_RowDataBound(
        object sender,
        GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        DataRowView row = (DataRowView)e.Row.DataItem;

        string status =
            Convert.ToString(row["grievanceStatus"]).Trim();

        string priority =
            Convert.ToString(row["grievancePriority"]).Trim();

        Label lblStatus =
            e.Row.FindControl("lblRowStatus") as Label;

        Label lblPriority =
            e.Row.FindControl("lblRowPriority") as Label;

        Label lblDueDate =
            e.Row.FindControl("lblDueDate") as Label;

        Label lblDueState =
            e.Row.FindControl("lblDueState") as Label;

        // Shared project status classes remain responsible for status colors.
        if (lblStatus != null)
        {
            string normalizedStatus = status.ToLowerInvariant()
                .Replace(" ", "-");

            lblStatus.CssClass =
                "status-badge status-" + normalizedStatus;
        }

        if (lblPriority != null)
        {
            string normalizedPriority =
                priority.ToLowerInvariant();

            lblPriority.CssClass =
                "hod-pr-priority-badge priority-" +
                normalizedPriority;
        }

        if (lblDueDate != null && lblDueState != null)
        {
            if (row["grievanceDueDate"] == DBNull.Value)
            {
                lblDueDate.Text = "Not set";
                lblDueState.Text = "";
            }
            else
            {
                DateTime dueDate =
                    Convert.ToDateTime(row["grievanceDueDate"]);

                lblDueDate.Text =
                    dueDate.ToString("dd MMM yyyy");

                if (status.Equals(
                        "Resolved",
                        StringComparison.OrdinalIgnoreCase) ||
                    status.Equals(
                        "Rejected",
                        StringComparison.OrdinalIgnoreCase))
                {
                    lblDueState.Text = "";
                }
                else if (dueDate.Date < DateTime.Today)
                {
                    lblDueState.Text = "Overdue";
                    lblDueState.CssClass =
                        "hod-pr-due-state due-overdue";
                }
                else if (dueDate.Date == DateTime.Today)
                {
                    lblDueState.Text = "Due today";
                    lblDueState.CssClass =
                        "hod-pr-due-state due-today";
                }
                else
                {
                    lblDueState.Text = "Upcoming";
                    lblDueState.CssClass =
                        "hod-pr-due-state due-upcoming";
                }
            }
        }
    }
}
