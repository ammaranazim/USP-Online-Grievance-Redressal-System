using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AdminEditDepartment : System.Web.UI.Page
{
    private int DepartmentID
    {
        get
        {
            int id;
            return int.TryParse(Request.QueryString["id"], out id) ? id : 0;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (DepartmentID <= 0)
            {
                RedirectToDepartments();
                return;
            }

            LoadCategories();
            LoadDepartmentDetails();
        }
    }

    private void LoadCategories()
    {
        try
        {
            DataTable categories = DALGrievanceCategory.GetAllCategories();

            ddlCategory.DataSource = categories;
            ddlCategory.DataTextField = "categoryName";
            ddlCategory.DataValueField = "categoryID";
            ddlCategory.DataBind();

            ddlCategory.Items.Insert(
                0,
                new ListItem("Select Category", "")
            );
        }
        catch (Exception)
        {
            ShowMessage(
                "Unable to load department categories. Please try again.",
                false
            );

            btnUpdateDepartment.Enabled = false;
        }
    }

    private void LoadDepartmentDetails()
    {
        try
        {
            MDepartment department = DALDepartment.GetDepartmentByID(DepartmentID);

            if (department == null)
            {
                ShowMessage("Department was not found.", false);
                btnUpdateDepartment.Enabled = false;
                return;
            }

            hfDepartmentID.Value = department.DeptID.ToString();

            txtDepartmentName.Text = department.DeptName;
            txtDepartmentCode.Text = department.DeptCode;
            txtDescription.Text = department.DeptDescription;
            txtEmail.Text = department.DeptEmail;
            txtContact.Text = department.DeptContactNo;
            txtWorkingHours.Text = department.DeptWH;
            txtLocation.Text = department.DeptLocation;

            ListItem categoryItem =
                ddlCategory.Items.FindByValue(department.CategoryID.ToString());

            if (categoryItem != null)
            {
                ddlCategory.SelectedValue = department.CategoryID.ToString();
            }

            ListItem statusItem =
                ddlStatus.Items.FindByValue(department.DeptStatus);

            if (statusItem != null)
            {
                ddlStatus.SelectedValue = department.DeptStatus;
            }
            else
            {
                ShowMessage(
                    "The department has an unrecognized status. Please verify its database value.",
                    false
                );

                btnUpdateDepartment.Enabled = false;
            }
        }
        catch (Exception)
        {
            ShowMessage(
                "Unable to load department details. Please try again.",
                false
            );

            btnUpdateDepartment.Enabled = false;
        }
    }

    protected void btnUpdateDepartment_Click(object sender, EventArgs e)
    {
        Page.Validate("EditDepartment");

        if (!Page.IsValid)
            return;

        int departmentID;
        int categoryID;
        int workingHours;

        if (!int.TryParse(hfDepartmentID.Value, out departmentID) ||
            departmentID <= 0 ||
            departmentID != DepartmentID)
        {
            ShowMessage("Invalid department reference. Please reopen the edit page.", false);
            return;
        }

        if (!int.TryParse(ddlCategory.SelectedValue, out categoryID) ||
            categoryID <= 0)
        {
            ShowMessage("Please select a valid category.", false);
            return;
        }

        if (!int.TryParse(txtWorkingHours.Text.Trim(), out workingHours) ||
            workingHours < 1 || workingHours > 24)
        {
            ShowMessage("Working hours must be between 1 and 24.", false);
            return;
        }

        string departmentName = txtDepartmentName.Text.Trim();
        string departmentCode = txtDepartmentCode.Text.Trim();

        try
        {
            if (DepartmentAlreadyExists(
                departmentName,
                departmentCode,
                departmentID))
            {
                ShowMessage(
                    "Another department already uses this name or code.",
                    false
                );

                return;
            }

            MDepartment department = new MDepartment(
                departmentID,
                departmentName,
                departmentCode,
                ddlStatus.SelectedValue,
                txtDescription.Text.Trim(),
                txtEmail.Text.Trim(),
                txtContact.Text.Trim(),
                workingHours.ToString(),
                txtLocation.Text.Trim(),
                categoryID
            );

            int result = DALDepartment.UpdateDepartment(department);

            if (result > 0)
            {
                Session["DepartmentSuccessMessage"] =
                    "Department updated successfully.";

                Response.Redirect(
                    "~/Admin/AdminDepartments.aspx",
                    false
                );

                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            ShowMessage(
                "Department could not be updated. It may have been removed.",
                false
            );
        }
        catch (Exception)
        {
            ShowMessage(
                "An error occurred while updating the department. Please try again.",
                false
            );
        }
    }

    private bool DepartmentAlreadyExists(
        string departmentName,
        string departmentCode,
        int departmentID)
    {
        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("Database connection is not configured.");

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(@"
            SELECT COUNT(*)
            FROM Department
            WHERE deptID <> @deptID
              AND
              (
                  LOWER(LTRIM(RTRIM(deptName))) =
                      LOWER(LTRIM(RTRIM(@deptName)))
                  OR
                  LOWER(LTRIM(RTRIM(deptCode))) =
                      LOWER(LTRIM(RTRIM(@deptCode)))
              );", con))
        {
            cmd.Parameters.Add("@deptID", SqlDbType.Int).Value = departmentID;

            cmd.Parameters.Add("@deptName", SqlDbType.NVarChar, 50)
                .Value = departmentName;

            cmd.Parameters.Add("@deptCode", SqlDbType.NVarChar, 50)
                .Value = departmentCode;

            con.Open();

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
    }

    private void ShowMessage(string message, bool success)
    {
        lblMessage.Text = message;

        lblMessage.CssClass = success
            ? "edit-department-message success"
            : "edit-department-message error";
    }

    private void RedirectToDepartments()
    {
        Response.Redirect("~/Admin/AdminDepartments.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }
}