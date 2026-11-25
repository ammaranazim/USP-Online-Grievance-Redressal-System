using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

public partial class Admin_AdminRegisterDepartment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCategories();
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
                new System.Web.UI.WebControls.ListItem(
                    "Select Category", ""
                )
            );
        }
        catch (Exception)
        {
            lblMessage.Text = "Unable to load categories. Please try again.";
            lblMessage.CssClass = "department-form-message error";
        }
    }

    protected void btnRegisterDepartment_Click(object sender, EventArgs e)
    {
        Page.Validate("DepartmentForm");

        if (!Page.IsValid)
        {
            return;
        }

        try
        {
            int categoryID;
            int workingHours;

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

            // Check whether the department name or code already exists.
            if (DepartmentAlreadyExists(
                txtDepartmentName.Text.Trim(),
                txtDepartmentCode.Text.Trim()))
            {
                ShowMessage(
                    "A department with this name or code already exists.",
                    false
                );
                return;
            }

            MDepartment department = new MDepartment(
                0,
                txtDepartmentName.Text.Trim(),
                txtDepartmentCode.Text.Trim(),
                ddlStatus.SelectedValue,
                txtDescription.Text.Trim(),
                txtEmail.Text.Trim(),
                txtContact.Text.Trim(),
                workingHours.ToString(),
                txtLocation.Text.Trim(),
                categoryID
            );

            int result = DALDepartment.RegisterDept(department);

            if (result > 0)
            {
                Session["DepartmentSuccessMessage"] =
                    "Department registered successfully.";

                Response.Redirect(
                    "~/Admin/AdminDepartments.aspx",
                    false
                );

                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            ShowMessage(
                "Department could not be registered. Please try again.",
                false
            );
        }
        catch (Exception)
        {
            ShowMessage(
                "An error occurred while registering the department. " +
                "Please verify the information and try again.",
                false
            );
        }
    }

    private bool DepartmentAlreadyExists(string departmentName, string departmentCode)
    {
        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Database connection is not configured.");
        }

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(@"
            SELECT COUNT(*)
            FROM Department
            WHERE LOWER(LTRIM(RTRIM(deptName))) =
                      LOWER(LTRIM(RTRIM(@deptName)))
               OR LOWER(LTRIM(RTRIM(deptCode))) =
                      LOWER(LTRIM(RTRIM(@deptCode)))", con))
        {
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
            ? "department-form-message success"
            : "department-form-message error";
    }
}