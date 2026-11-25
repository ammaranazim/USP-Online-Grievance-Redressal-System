using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AdminStaff : System.Web.UI.Page
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
            LoadStaffStatusCounts();
            LoadStaff();

            if (Request.QueryString["registered"] == "1")
            {
                lblMessage.Text = Convert.ToString(
                    Session["StaffRegistrationMessage"]);

                if (string.IsNullOrWhiteSpace(lblMessage.Text))
                {
                    lblMessage.Text = "Staff account registered successfully.";
                }

                Session.Remove("StaffRegistrationMessage");
            }
        }
    }


    private void LoadDepartments()
    {
        ddlDepartment.Items.Clear();
        ddlDepartment.Items.Add(new ListItem("All Departments", "0"));

        DataTable departments = DALDepartment.GetAllDepartments();

        if (departments != null)
        {
            foreach (DataRow row in departments.Rows)
            {
                ddlDepartment.Items.Add(new ListItem(
                    Convert.ToString(row["deptName"]),
                    Convert.ToString(row["deptID"])));
            }
        }
    }

    private void LoadStaffStatusCounts()
    {
        DataTable counts = DALStaff.GetStaffStatusCounts();

        if (counts != null && counts.Rows.Count > 0)
        {
            DataRow row = counts.Rows[0];

            lblTotalStaff.Text = Convert.ToString(row["TotalCount"]);
            lblActiveStaff.Text = Convert.ToString(row["ActiveCount"]);
            lblInactiveStaff.Text = Convert.ToString(row["InactiveCount"]);
        }
        else
        {
            lblTotalStaff.Text = "0";
            lblActiveStaff.Text = "0";
            lblInactiveStaff.Text = "0";
        }
    }

    private void LoadStaff()
    {
        string statusFilter = !string.IsNullOrEmpty(KpiStatusFilter)
            ? KpiStatusFilter
            : ddlStatus.SelectedValue;

        DataTable staff = DALStaff.GetAdminStaff(
            txtSearch.Text.Trim(),
            ddlDepartment.SelectedValue,
            statusFilter);

        gvStaff.DataSource = staff;
        gvStaff.DataBind();

        int count = staff == null ? 0 : staff.Rows.Count;

        lblRecordCount.Text = count == 1
            ? "1 record"
            : count + " records";
    }

    protected void KpiCard_Click(object sender, EventArgs e)
    {
        LinkButton card = sender as LinkButton;

        if (card == null)
            return;

        string filter = card.CommandArgument;

        if (filter == "all")
        {
            KpiStatusFilter = string.Empty;
            ddlStatus.SelectedValue = string.Empty;
        }
        else if (filter == "active" || filter == "inactive")
        {
            KpiStatusFilter = filter;
            ddlStatus.SelectedValue = filter;
        }
        else
        {
            return;
        }

        LoadStaff();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadStaff();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        ddlDepartment.SelectedValue = "0";
        ddlStatus.SelectedValue = string.Empty;
        KpiStatusFilter = string.Empty;

        LoadStaff();
    }

    protected string GetStaffStatusCss(object statusValue)
    {
        string status = Convert.ToString(statusValue).Trim();

        if (string.Equals(status, "Active",
            StringComparison.OrdinalIgnoreCase))
            return "staff-status-badge staff-status-active";

        if (string.Equals(status, "Inactive",
            StringComparison.OrdinalIgnoreCase))
            return "staff-status-badge staff-status-inactive";

        return "staff-status-badge";
    }

    protected string GetStaffActionText(object statusValue)
    {
        return string.Equals(Convert.ToString(statusValue).Trim(),
            "Active", StringComparison.OrdinalIgnoreCase)
            ? "Deactivate"
            : "Activate";
    }

    protected void gvStaff_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int staffID;

        if (!int.TryParse(Convert.ToString(e.CommandArgument), out staffID))
        {
            lblMessage.Text = "Invalid Staff ID.";
            return;
        }

        if (e.CommandName == "EditStaff")
        {
            Response.Redirect("~/Admin/AdminEditStaff.aspx?id=" + staffID);
            return;
        }

        if (e.CommandName == "ToggleStatus")
        {
            GridViewRow row = (GridViewRow)
                ((Control)e.CommandSource).NamingContainer;

            string currentStatus = Convert.ToString(
                gvStaff.DataKeys[row.RowIndex].Values["staffStatus"]);

            string newStatus = string.Equals(currentStatus, "Active",
                StringComparison.OrdinalIgnoreCase)
                ? "Inactive"
                : "Active";

            bool updated = DALStaff.UpdateStaffStatus(staffID, newStatus);

            if (updated)
            {
                lblMessage.Text = newStatus == "Active"
                    ? "Staff account activated successfully."
                    : "Staff account deactivated successfully.";

                LoadStaffStatusCounts();
                LoadStaff();
            }
            else
            {
                lblMessage.Text =
                    "Unable to update staff account status. Please try again.";
            }
        }
    }
}