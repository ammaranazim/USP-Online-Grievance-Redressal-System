
using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Admin_AdminEditStaff : System.Web.UI.Page
{
    private int StaffID
    {
        get
        {
            int id;
            return int.TryParse(hfStaffID.Value, out id) ? id : 0;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AdminID"] == null)
        {
            Response.Redirect("~/Admin/AdminLogin.aspx");
            return;
        }

        if (!IsPostBack)
        {
            int staffID;

            if (!int.TryParse(Request.QueryString["id"], out staffID) ||
                staffID <= 0)
            {
                ReturnToStaffList("Invalid staff record selected.");
                return;
            }

            hfStaffID.Value = staffID.ToString();

            if (!LoadDepartments())
            {
                ReturnToStaffList("Unable to load departments.");
                return;
            }

            if (!LoadStaff(staffID))
            {
                ReturnToStaffList("Staff record was not found.");
            }
        }
    }

    private bool LoadDepartments()
    {
        try
        {
            var departments = DALDepartment.GetAllDepartments();

            ddlDepartment.Items.Clear();
            ddlDepartment.DataSource = departments;
            ddlDepartment.DataTextField = "deptName";
            ddlDepartment.DataValueField = "deptID";
            ddlDepartment.DataBind();

            ddlDepartment.Items.Insert(
                0, new ListItem("Select Department", ""));

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private bool LoadStaff(int staffID)
    {
        try
        {
            MStaff staff = DALStaff.GetStaffByID(staffID);

            if (staff == null)
                return false;

            txtStaffID.Text = staff.StaffID.ToString();
            txtStaffName.Text = staff.StaffName;
            txtUsername.Text = staff.StaffUserName;
            txtEmail.Text = staff.StaffEmail;
            txtPhone.Text = staff.StaffPhoneNo;
            txtDesignation.Text = staff.StaffDesignation;

            if (ddlDepartment.Items.FindByValue(
                staff.DeptID.ToString()) != null)
            {
                ddlDepartment.SelectedValue = staff.DeptID.ToString();
            }
            else
            {
                ShowMessage(
                    "The staff member's department is unavailable. Please select a valid department.",
                    false);
            }

            if (ddlStatus.Items.FindByValue(staff.StaffStatus) != null)
            {
                ddlStatus.SelectedValue = staff.StaffStatus;
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    protected void btnSaveChanges_Click(object sender, EventArgs e)
    {
        Page.Validate("EditStaff");

        if (!Page.IsValid)
            return;

        int staffID = StaffID;

        if (staffID <= 0)
        {
            ShowMessage("Invalid staff record.", false);
            return;
        }

        int deptID;

        if (!int.TryParse(ddlDepartment.SelectedValue, out deptID) ||
            deptID <= 0)
        {
            ShowMessage("Please select a valid department.", false);
            return;
        }

        MStaff staff = new MStaff(
            staffID,
            txtStaffName.Text.Trim(),
            txtUsername.Text.Trim(),
            txtEmail.Text.Trim(),
            txtPhone.Text.Trim(),
            txtDesignation.Text.Trim(),
            ddlStatus.SelectedValue,
            deptID,
            false,
            string.Empty
        );

        try
        {
            if (DALStaff.IsStaffUsernameExists(
                staff.StaffUserName, staffID))
            {
                ShowMessage(
                    "This username is already used by another staff member.",
                    false);
                return;
            }

            bool updated = DALStaff.UpdateStaff(staff);

            if (updated)
            {
                Response.Redirect(
                    "~/Admin/AdminStaff.aspx?updated=1",
                    false);

                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            ShowMessage(
                "Staff details could not be updated. Please check the information.",
                false);
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2601 || ex.Number == 2627)
            {
                ShowMessage(
                    "This username is already in use.",
                    false);
            }
            else
            {
                ShowMessage(
                    "A database error occurred. Please try again.",
                    false);
            }
        }
        catch (Exception)
        {
            ShowMessage(
                "An unexpected error occurred. Please try again.",
                false);
        }
    }

    private void ShowMessage(string message, bool success)
    {
        lblMessage.Text = Server.HtmlEncode(message);
        lblMessage.CssClass = success
            ? "edit-staff-message edit-staff-message-success"
            : "edit-staff-message edit-staff-message-error";
    }

    private void ReturnToStaffList(string message)
    {
        Session["StaffEditMessage"] = message;

        Response.Redirect(
            "~/Admin/AdminStaff.aspx",
            false);

        Context.ApplicationInstance.CompleteRequest();
    }
}
