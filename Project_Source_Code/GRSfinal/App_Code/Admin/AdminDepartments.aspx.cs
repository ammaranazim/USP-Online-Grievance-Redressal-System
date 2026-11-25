using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AdminDepartments : System.Web.UI.Page
{
    private const string AllDepartments = "all";
    private const string ActiveDepartments = "active";
    private const string InactiveDepartments = "inactive";

    private string KpiStatusFilter
    {
        get
        {
            return ViewState["KpiStatusFilter"] as string ?? AllDepartments;
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
            KpiStatusFilter = AllDepartments;

            LoadCategories();
            LoadDepartmentCounts();
            LoadDepartments();
            ShowRegistrationMessage();
        }
    }

    private void LoadCategories()
    {
        DataTable categories = DALGrievanceCategory.GetAllCategories();

        ddlCategoryFilter.DataSource = categories;
        ddlCategoryFilter.DataTextField = "categoryName";
        ddlCategoryFilter.DataValueField = "categoryID";
        ddlCategoryFilter.DataBind();

        ddlCategoryFilter.Items.Insert(
            0,
            new ListItem("All Categories", "0")
        );
    }

    private void LoadDepartmentCounts()
    {
        DataTable counts = DALDepartment.GetDepartmentStatusCounts();

        if (counts.Rows.Count == 0)
        {
            lblTotalDepartments.Text = "0";
            lblActiveDepartments.Text = "0";
            lblInactiveDepartments.Text = "0";
            return;
        }

        DataRow row = counts.Rows[0];

        lblTotalDepartments.Text =
            row["TotalDepartments"].ToString();

        lblActiveDepartments.Text =
            row["ActiveDepartments"].ToString();

        lblInactiveDepartments.Text =
            row["InactiveDepartments"].ToString();
    }

    private void LoadDepartments()
    {
        int categoryID = 0;

        if (!string.IsNullOrEmpty(ddlCategoryFilter.SelectedValue))
        {
            int.TryParse(ddlCategoryFilter.SelectedValue, out categoryID);
        }

        string statusFilter = ddlStatusFilter.SelectedValue;

        // KPI selection and status dropdown must work together.
        if (KpiStatusFilter == ActiveDepartments)
        {
            statusFilter = "Active";
        }
        else if (KpiStatusFilter == InactiveDepartments)
        {
            statusFilter = "Inactive";
        }

        DataTable departments = DALDepartment.GetAdminDepartments(
            txtDepartmentSearch.Text.Trim(),
            categoryID,
            statusFilter
        );

        gvDepartments.DataSource = departments;
        gvDepartments.DataBind();

        lblDepartmentCount.Text =
            departments.Rows.Count == 1
                ? "1 record"
                : departments.Rows.Count + " records";
    }

    protected void btnDepartmentSearch_Click(object sender, EventArgs e)
    {
        LoadDepartments();
    }

    protected void btnDepartmentClear_Click(object sender, EventArgs e)
    {
        txtDepartmentSearch.Text = "";
        ddlCategoryFilter.SelectedIndex = 0;
        ddlStatusFilter.SelectedIndex = 0;

        KpiStatusFilter = AllDepartments;

        lblDepartmentMessage.Text = "";
        LoadDepartmentCounts();
        LoadDepartments();
    }

    protected void DepartmentFilter_Changed(object sender, EventArgs e)
    {
        KpiStatusFilter = AllDepartments;
        LoadDepartments();
    }

    protected void KpiCard_Click(object sender, CommandEventArgs e)
    {
        string selectedFilter = Convert.ToString(e.CommandArgument);

        if (selectedFilter != AllDepartments &&
            selectedFilter != ActiveDepartments &&
            selectedFilter != InactiveDepartments)
        {
            return;
        }

        KpiStatusFilter = selectedFilter;

        // Reflect KPI selection in the status dropdown.
        ddlStatusFilter.SelectedValue =
            selectedFilter == ActiveDepartments ? "Active" :
            selectedFilter == InactiveDepartments ? "Inactive" : "";

        LoadDepartments();
    }

    protected void gvDepartments_RowDataBound(
        object sender,
        GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
        {
            return;
        }

        string status = Convert.ToString(
            DataBinder.Eval(e.Row.DataItem, "deptStatus")
        );

        Label statusLabel = e.Row.FindControl(
            "lblDepartmentStatus"
        ) as Label;

        LinkButton toggleButton = e.Row.FindControl(
            "btnToggleDepartment"
        ) as LinkButton;

        if (statusLabel != null)
        {
            if (string.Equals(
                status, "Active",
                StringComparison.OrdinalIgnoreCase))
            {
                statusLabel.Text = "Active";
                statusLabel.CssClass =
                    "departments-status-badge departments-status-active";
            }
            else
            {
                statusLabel.Text = "Inactive";
                statusLabel.CssClass =
                    "departments-status-badge departments-status-inactive";
            }
        }

        if (toggleButton != null)
        {
            bool isActive = string.Equals(
                status, "Active",
                StringComparison.OrdinalIgnoreCase);

            toggleButton.Text = isActive ? "Deactivate" : "Activate";

            toggleButton.CssClass = isActive
                ? "departments-action-btn departments-deactivate-btn"
                : "departments-action-btn departments-activate-btn";

            toggleButton.OnClientClick = isActive
                ? "return confirm('Deactivate this department?');"
                : "return confirm('Activate this department?');";
        }
    }

    protected void gvDepartments_RowCommand(
        object sender, GridViewCommandEventArgs e)
    {
        int departmentID;

        if (!int.TryParse(
            Convert.ToString(e.CommandArgument),
            out departmentID))
        {
            return;
        }

        if (e.CommandName == "EditDepartment")
        {
            Response.Redirect(
                "~/Admin/AdminEditDepartment.aspx?id=" + departmentID,
                false
            );

            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (e.CommandName == "ToggleStatus")
        {
            try
            {
                MDepartment department =
                    DALDepartment.GetDepartmentByID(departmentID);

                if (department == null)
                {
                    ShowMessage(
                        "Department was not found.",
                        false
                    );
                    return;
                }

                bool isActive = string.Equals(
                    department.DeptStatus,
                    "Active",
                    StringComparison.OrdinalIgnoreCase
                );

                string newStatus = isActive ? "Inactive" : "Active";

                int result = DALDepartment.UpdateDepartmentStatus(
                    departmentID,
                    newStatus
                );

                if (result > 0)
                {
                    ShowMessage(
                        "Department status updated successfully.",
                        true
                    );

                    LoadDepartmentCounts();
                    LoadDepartments();
                }
                else
                {
                    ShowMessage(
                        "Department status could not be updated.",
                        false
                    );
                }
            }
            catch (Exception)
            {
                ShowMessage(
                    "An error occurred while updating department status.",
                    false
                );
            }
        }
    }

    private void ShowRegistrationMessage()
    {
        if (Session["DepartmentSuccessMessage"] != null)
        {
            ShowMessage(
                Convert.ToString(Session["DepartmentSuccessMessage"]),
                true
            );

            Session.Remove("DepartmentSuccessMessage");
        }
    }

    private void ShowMessage(string message, bool success)
    {
        lblDepartmentMessage.Text = message;
        lblDepartmentMessage.CssClass = success
            ? "departments-message success"
            : "departments-message error";
    }
}