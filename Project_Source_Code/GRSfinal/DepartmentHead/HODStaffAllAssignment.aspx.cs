
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DepartmentHead_HODStaffAllAssignment : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["DeptHeadID"] == null ||
            Session["DeptID"] == null)
        {
            Response.Redirect("~/DepartmentHead/HODLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (!IsPostBack)
        {
            BindAssignmentCounts();

            string assignmentFilter =
                Convert.ToString(Request.QueryString["assignment"]);

            if (string.Equals(
                assignmentFilter,
                "Unassigned",
                StringComparison.OrdinalIgnoreCase))
            {
                ListItem unassignedItem =
                    ddlAssignmentStatus.Items.FindByValue("Unassigned");

                if (unassignedItem != null)
                {
                    ddlAssignmentStatus.ClearSelection();
                    unassignedItem.Selected = true;
                }
            }

            BindAssignments();
        }
    }

    private void BindAssignmentCounts()
    {
        DataTable dt =
            DALGrievance.GetHODStaffAssignmentCounts(DepartmentID);

        if (dt.Rows.Count == 0)
        {
            lblTotalGrievances.Text = "0";
            lblAssignedGrievances.Text = "0";
            lblUnassignedGrievances.Text = "0";
            return;
        }

        DataRow row = dt.Rows[0];

        lblTotalGrievances.Text =
            Convert.ToString(row["TotalGrievances"]);

        lblAssignedGrievances.Text =
            Convert.ToString(row["AssignedGrievances"]);

        lblUnassignedGrievances.Text =
            Convert.ToString(row["UnassignedGrievances"]);
    }

    private void BindAssignments()
    {
        DataTable dt = DALGrievance.GetHODStaffAssignmentOverview(
            DepartmentID,
            txtSearch.Text.Trim(),
            ddlAssignmentStatus.SelectedValue
        );

        gvStaffAssignments.DataSource = dt;
        gvStaffAssignments.DataBind();

        lblRecordCount.Text = dt.Rows.Count.ToString();

        bool hasRecords = dt.Rows.Count > 0;

        gvStaffAssignments.Visible = hasRecords;
        pnlNoAssignments.Visible = !hasRecords;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        pnlMessage.Visible = false;
        BindAssignments();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        ddlAssignmentStatus.SelectedIndex = 0;
        pnlMessage.Visible = false;

        BindAssignments();
    }



    protected void gvStaffAssignments_RowDataBound(
        object sender,
        GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
        {
            return;
        }

        string assignmentStatus = Convert.ToString(
            DataBinder.Eval(e.Row.DataItem, "assignmentStatus"));

        bool isAssigned = string.Equals(
            assignmentStatus,
            "Assigned",
            StringComparison.OrdinalIgnoreCase);

        // Hide Assign Staff when an active assignment already exists.
        HyperLink lnkAssignStaff =
            e.Row.FindControl("lnkAssignStaff") as HyperLink;

        if (lnkAssignStaff != null)
        {
            lnkAssignStaff.Visible = !isAssigned;
        }

        // Assignment status badge.
        Label lblAssignmentStatus =
            e.Row.FindControl("lblAssignmentStatus") as Label;

        if (lblAssignmentStatus != null)
        {
            lblAssignmentStatus.CssClass = isAssigned
                ? "hod-assignment-status assigned"
                : "hod-assignment-status unassigned";
        }

        // Assigned staff badge.
        Label lblAssignedStaff =
            e.Row.FindControl("lblAssignedStaff") as Label;

        if (lblAssignedStaff != null)
        {
            string staffName = Convert.ToString(
                DataBinder.Eval(e.Row.DataItem, "staffName"));

            bool hasStaff = !string.IsNullOrWhiteSpace(staffName)
                            && isAssigned;

            lblAssignedStaff.Text = hasStaff
                ? staffName
                : "Not Assigned";

        }

        // Grievance status badge.
        Label lblGrievanceStatus =
            e.Row.FindControl("lblGrievanceStatus") as Label;

        if (lblGrievanceStatus != null)
        {
            string status = Convert.ToString(
                DataBinder.Eval(e.Row.DataItem, "grievanceStatus"));

            string statusClass = "hod-grievance-status";

            if (string.Equals(status, "Pending",
                StringComparison.OrdinalIgnoreCase))
            {
                statusClass += " status-pending";
            }
            else if (string.Equals(status, "In Process",
                StringComparison.OrdinalIgnoreCase))
            {
                statusClass += " status-in-process";
            }
            else if (string.Equals(status, "Resolved",
                StringComparison.OrdinalIgnoreCase))
            {
                statusClass += " status-resolved";
            }
            else if (string.Equals(status, "Rejected",
                StringComparison.OrdinalIgnoreCase))
            {
                statusClass += " status-rejected";
            }

            lblGrievanceStatus.CssClass = statusClass;
        }
    }


}
