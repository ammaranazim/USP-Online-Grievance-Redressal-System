using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AdminDepartmentHeads : System.Web.UI.Page
{
    private string KpiStatusFilter
    {
        get
        {
            return ViewState["KpiStatusFilter"] == null
                ? string.Empty
                : ViewState["KpiStatusFilter"].ToString();
        }
        set
        {
            ViewState["KpiStatusFilter"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KpiStatusFilter = string.Empty;

            LoadDepartments();
            LoadDepartmentHeadStatusCounts();
            LoadDepartmentHeads();
        }
    }

    private void LoadDepartments()
    {
        ddlDepartment.Items.Clear();

        ddlDepartment.Items.Add(
            new ListItem("All Departments", "0"));

        DataTable departments = DALDepartment.GetAllDepartments();

        if (departments != null)
        {
            ddlDepartment.DataSource = departments;
            ddlDepartment.DataTextField = "deptName";
            ddlDepartment.DataValueField = "deptID";
            ddlDepartment.DataBind();

            ddlDepartment.Items.Insert(
                0,
                new ListItem("All Departments", "0"));
        }
    }

    private void LoadDepartmentHeadStatusCounts()
    {
        DataTable counts =
            DALDepartmentHead.GetDepartmentHeadStatusCounts();

        if (counts != null && counts.Rows.Count > 0)
        {
            DataRow row = counts.Rows[0];

            lblTotalDepartmentHeads.Text =
                Convert.ToString(row["TotalCount"]);

            lblActiveDepartmentHeads.Text =
                Convert.ToString(row["ActiveCount"]);

            lblInactiveDepartmentHeads.Text =
                Convert.ToString(row["InactiveCount"]);
        }
        else
        {
            lblTotalDepartmentHeads.Text = "0";
            lblActiveDepartmentHeads.Text = "0";
            lblInactiveDepartmentHeads.Text = "0";
        }
    }

    private void LoadDepartmentHeads()
    {
        string keyword = txtSearch.Text.Trim();

        string departmentFilter =
            ddlDepartment.SelectedValue;

        // Use the selected KPI status when a KPI card is active.
        // Otherwise, use the status dropdown.
        string statusFilter = !string.IsNullOrEmpty(KpiStatusFilter)
            ? KpiStatusFilter
            : ddlStatus.SelectedValue;

        DataTable departmentHeads =
            DALDepartmentHead.GetAdminDepartmentHeads(
                keyword,
                departmentFilter,
                statusFilter);

        gvDepartmentHeads.DataSource = departmentHeads;
        gvDepartmentHeads.DataBind();

        int recordCount = departmentHeads == null
            ? 0
            : departmentHeads.Rows.Count;

        lblRecordCount.Text = recordCount == 1
            ? "1 record"
            : recordCount + " records";
    }

    protected void KpiCard_Click(object sender, EventArgs e)
    {
        LinkButton clickedCard = sender as LinkButton;

        if (clickedCard == null)
        {
            return;
        }

        string selectedFilter = clickedCard.CommandArgument;

        if (selectedFilter == "all")
        {
            // Show all statuses.
            KpiStatusFilter = string.Empty;
            ddlStatus.SelectedValue = string.Empty;
        }
        else if (
            selectedFilter == "active" ||
            selectedFilter == "inactive")
        {
            // Apply the selected status.
            KpiStatusFilter = selectedFilter;
            ddlStatus.SelectedValue = selectedFilter;
        }
        else
        {
            return;
        }

        LoadDepartmentHeads();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadDepartmentHeads();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        ddlDepartment.SelectedValue = "0";
        ddlStatus.SelectedValue = string.Empty;

        KpiStatusFilter = string.Empty;

        LoadDepartmentHeads();
    }

    protected string GetDepartmentHeadStatusCss(object statusValue)
    {
        string status = Convert.ToString(statusValue).Trim();

        if (string.Equals(
            status, "active", StringComparison.OrdinalIgnoreCase))
        {
            return "status-badge status-active";
        }

        if (string.Equals(
            status, "inactive", StringComparison.OrdinalIgnoreCase))
        {
            return "status-badge status-inactive";
        }

        return "status-badge";
    }
    protected string GetDepartmentHeadStatusText(object statusValue)
    {
        string status = Convert.ToString(statusValue).Trim();

        if (string.Equals(
            status, "active", StringComparison.OrdinalIgnoreCase))
        {
            return "Active";
        }

        if (string.Equals(
            status, "inactive", StringComparison.OrdinalIgnoreCase))
        {
            return "Inactive";
        }

        return status;
    }

    protected string GetDepartmentHeadActionText(object statusValue)
    {
        string status = Convert.ToString(statusValue);

        if (string.Equals(
            status,
            "active",
            StringComparison.OrdinalIgnoreCase))
        {
            return "Deactivate";
        }

        return "Activate";
    }

    protected void gvDepartmentHeads_RowCommand(
        object sender,
        GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditDepartmentHead")
        {
            int departmentHeadID;

            if (int.TryParse(
                Convert.ToString(e.CommandArgument),
                out departmentHeadID))
            {
                Response.Redirect(
                    "~/Admin/AdminEditDepartmentHead.aspx?id="
                    + departmentHeadID);
            }

            return;
        }

        if (e.CommandName == "ToggleStatus")
        {
            int departmentHeadID;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument),
                out departmentHeadID))
            {
                lblMessage.Text = "Invalid Department Head ID.";
                return;
            }

            GridViewRow row = (GridViewRow)
                ((Control)e.CommandSource).NamingContainer;

            string currentStatus = Convert.ToString(
                gvDepartmentHeads.DataKeys[row.RowIndex]
                    .Values["deptHeadStatus"]);

            string newStatus =
                string.Equals(
                    currentStatus,
                    "active",
                    StringComparison.OrdinalIgnoreCase)
                ? "inactive"
                : "active";

            bool updated =
                DALDepartmentHead.UpdateDepartmentHeadStatus(
                    departmentHeadID,
                    newStatus);

            if (updated)
            {
                lblMessage.Text =
                    newStatus == "active"
                    ? "Department Head account activated successfully."
                    : "Department Head account deactivated successfully.";

                LoadDepartmentHeadStatusCounts();
                LoadDepartmentHeads();
            }
            else
            {
                lblMessage.Text =
                    "Unable to update account status. Please try again.";
            }
        }
    }
}