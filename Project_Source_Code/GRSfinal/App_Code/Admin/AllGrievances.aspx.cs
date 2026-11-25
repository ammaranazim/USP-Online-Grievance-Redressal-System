using System;
using System.Data;

public partial class Admin_AllGrievances : System.Web.UI.Page
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
            try
            {
                LoadCategories();
                LoadDepartments();
                LoadGrievances();
            }
            catch (Exception)
            {
                ShowFilterValidationMessage(
                    "Unable to load grievance data. Please refresh the page and try again.");
            }
        }
    }


private void LoadCategories()
    {
        DataTable dt = DALGrievanceCategory.GetAllCategories();

        ddlCategory.DataSource = dt;
        ddlCategory.DataTextField = "categoryName";
        ddlCategory.DataValueField = "categoryID";
        ddlCategory.DataBind();

        ddlCategory.Items.Insert(
            0,
            new System.Web.UI.WebControls.ListItem(
                "All Categories",
                "0"));
    }

private void LoadDepartments()
{
    DataTable dt = DALDepartment.GetAllDepartments();

    ddlDepartment.DataSource = dt;
    ddlDepartment.DataTextField = "deptName";
    ddlDepartment.DataValueField = "deptID";
    ddlDepartment.DataBind();

    ddlDepartment.Items.Insert(
        0,
        new System.Web.UI.WebControls.ListItem(
            "All Departments",
            "0"));
}


    private void LoadGrievances()
    {
        // Hide any previous validation message.
        lblFilterMessage.Text = "";
        lblFilterMessage.Visible = false;

        string searchKeyword = txtSearch.Text.Trim();
        string status = ddlStatus.SelectedValue;

        int categoryID = 0;
        int deptID = 0;

        int.TryParse(ddlCategory.SelectedValue, out categoryID);
        int.TryParse(ddlDepartment.SelectedValue, out deptID);

        DateTime? fromDate = null;
        DateTime? toDate = null;

        // Validate From Date.
        if (!string.IsNullOrWhiteSpace(txtFromDate.Text))
        {
            DateTime parsedFromDate;

            if (!DateTime.TryParse(
                txtFromDate.Text, out parsedFromDate))
            {
                ShowFilterValidationMessage(
                    "Please enter a valid From Date.");
                return;
            }

            fromDate = parsedFromDate.Date;
        }

        // Validate To Date.
        if (!string.IsNullOrWhiteSpace(txtToDate.Text))
        {
            DateTime parsedToDate;

            if (!DateTime.TryParse(
                txtToDate.Text, out parsedToDate))
            {
                ShowFilterValidationMessage(
                    "Please enter a valid To Date.");
                return;
            }

            toDate = parsedToDate.Date;
        }

        // Ensure the date range is valid.
        if (fromDate.HasValue &&
            toDate.HasValue &&
            fromDate.Value > toDate.Value)
        {
            ShowFilterValidationMessage(
                "From Date cannot be later than To Date.");
            return;
        }

        // Load grievances only after validation succeeds.
        DataTable dt = DALGrievance.GetAdminAllGrievances(
            searchKeyword,
            status,
            categoryID,
            deptID,
            fromDate,
            toDate);

        gvGrievances.DataSource = dt;
        gvGrievances.DataBind();

        lblRecordCount.Text =
            dt.Rows.Count.ToString() +
            (dt.Rows.Count == 1 ? " Record" : " Records");
    }
    private void ShowFilterValidationMessage(string message)
    {
        lblFilterMessage.Text = message;
        lblFilterMessage.Visible = true;
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gvGrievances.PageIndex = 0;

        LoadGrievances();
    }


    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";

        ddlStatus.SelectedIndex = 0;
        ddlCategory.SelectedIndex = 0;
        ddlDepartment.SelectedIndex = 0;

        txtFromDate.Text = "";
        txtToDate.Text = "";

        gvGrievances.PageIndex = 0;

        LoadGrievances();
    }


    protected void gvGrievances_PageIndexChanging(
        object sender,
        System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        gvGrievances.PageIndex = e.NewPageIndex;

        LoadGrievances();
    }


    protected void gvGrievances_RowCommand(
     object sender,
     System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ViewDetails")
        {
            string grievanceID = e.CommandArgument.ToString();

            Response.Redirect(
                "~/Admin/AdminGrievanceDetails.aspx?id=" + grievanceID,
                false);

            return;
        }
    }
}