using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AdminEditDepartmentHead : System.Web.UI.Page
{
    private int DeptHeadID
    {
        get
        {
            int id;
            return int.TryParse(Request.QueryString["id"], out id)
                ? id
                : 0;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (DeptHeadID <= 0)
            {
                Response.Redirect("~/Admin/AdminDepartmentHeads.aspx");
                return;
            }

            BindDepartments();
            LoadDepartmentHead();
        }
    }

    private void BindDepartments()
    {
        DataTable departments = DALDepartmentHead.GetDepartmentHeadDepartments();

        ddlDepartment.DataSource = departments;
        ddlDepartment.DataTextField = "deptName";
        ddlDepartment.DataValueField = "deptID";
        ddlDepartment.DataBind();

        ddlDepartment.Items.Insert(
            0,
            new ListItem("Select Department", "")
        );
    }

    private void LoadDepartmentHead()
    {
        MDepartmentHead hod =
            DALDepartmentHead.GetDepartmentHeadByID(DeptHeadID);

        if (hod == null)
        {
            Response.Redirect("~/Admin/AdminDepartmentHeads.aspx");
            return;
        }

        hfDeptHeadID.Value = hod.DeptHeadID.ToString();

        txtHodName.Text = hod.DeptHeadName;
        txtUsername.Text = hod.DeptHeadUserName;
        txtEmail.Text = hod.DeptHeadEmail;
        txtPhone.Text = hod.DeptHeadPhoneNo;
        txtDesignation.Text = hod.DeptHeadDesignation;

        ListItem departmentItem =
            ddlDepartment.Items.FindByValue(hod.DeptID.ToString());

        if (departmentItem != null)
        {
            ddlDepartment.SelectedValue = hod.DeptID.ToString();
        }

        string status = (hod.DeptHeadStatus ?? "").Trim();

        ListItem statusItem =
            ddlStatus.Items.FindByValue(status);

        if (statusItem == null)
        {
            statusItem = ddlStatus.Items.FindByValue(
                status.ToLowerInvariant()
            );
        }

        if (statusItem != null)
        {
            ddlStatus.SelectedValue = statusItem.Value;
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            return;
        }

        int id;

        if (!int.TryParse(hfDeptHeadID.Value, out id) ||
            id != DeptHeadID ||
            id <= 0)
        {
            ShowMessage("Invalid department head record.", false);
            return;
        }

        try
        {
            // Verify that the HOD record still exists.
            MDepartmentHead existingHod =
                DALDepartmentHead.GetDepartmentHeadByID(id);

            if (existingHod == null)
            {
                ShowMessage(
                    "Department head record was not found.",
                    false
                );
                return;
            }

            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();

            // Prevent duplicate usernames belonging to another HOD.
            if (DALDepartmentHead.DepartmentHeadUsernameExists(username))
            {
                if (!string.Equals(
                    existingHod.DeptHeadUserName,
                    username,
                    StringComparison.OrdinalIgnoreCase))
                {
                    ShowMessage(
                        "This username is already in use.",
                        false
                    );
                    return;
                }
            }

            // Prevent duplicate email addresses belonging to another HOD.
            if (DALDepartmentHead.DepartmentHeadEmailExists(email))
            {
                if (!string.Equals(
                    existingHod.DeptHeadEmail,
                    email,
                    StringComparison.OrdinalIgnoreCase))
                {
                    ShowMessage(
                        "This email address is already in use.",
                        false
                    );
                    return;
                }
            }

            int departmentID;

            if (!int.TryParse(ddlDepartment.SelectedValue, out departmentID))
            {
                ShowMessage("Please select a valid department.", false);
                return;
            }

            MDepartmentHead hod = new MDepartmentHead(
     id,
     txtHodName.Text.Trim(),
     username,
     existingHod.DeptHeadPassword,
     email,
     txtPhone.Text.Trim(),
     txtDesignation.Text.Trim(),
     ddlStatus.SelectedValue,
     departmentID,
     existingHod.MustChangePassword
 );

            bool updated =
                DALDepartmentHead.UpdateDepartmentHeadProfile(hod);

            if (updated)
            {
                Response.Redirect(
                    "~/Admin/AdminDepartmentHeads.aspx",
                    false
                );

                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            ShowMessage(
                "No changes were saved. Please try again.",
                false
            );
        }
        catch (Exception)
        {
            ShowMessage(
                "Unable to update department head. Please try again.",
                false
            );
        }
    }

    private void ShowMessage(string message, bool success)
    {
        lblMessage.Text = message;
        lblMessage.CssClass = success
            ? "form-message success"
            : "form-message error";
    }
}