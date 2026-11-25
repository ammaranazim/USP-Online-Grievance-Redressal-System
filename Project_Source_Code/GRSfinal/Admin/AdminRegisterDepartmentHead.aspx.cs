using System;
using System.Data;
using System.Security.Cryptography;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AdminRegisterDepartmentHead : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDepartments();
        }
    }

    private void LoadDepartments()
    {
        DataTable departments = DALDepartment.GetAllDepartments();

        ddlDepartment.Items.Clear();
        ddlDepartment.Items.Add(
            new ListItem("Select Department", ""));

        if (departments != null)
        {
            ddlDepartment.DataSource = departments;
            ddlDepartment.DataTextField = "deptName";
            ddlDepartment.DataValueField = "deptID";
            ddlDepartment.DataBind();

            ddlDepartment.Items.Insert(
                0, new ListItem("Select Department", ""));
        }
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        Page.Validate();

        if (!Page.IsValid)
        {
            return;
        }

        string name = txtHodName.Text.Trim();
        string username = txtUsername.Text.Trim();
        string email = txtEmail.Text.Trim();
        string phone = txtPhone.Text.Trim();
        string designation = txtDesignation.Text.Trim();
        string status = ddlStatus.SelectedValue;
        int departmentID;

        if (!int.TryParse(ddlDepartment.SelectedValue, out departmentID))
        {
            ShowMessage("Please select a valid department.");
            return;
        }

        if (DALDepartmentHead.DepartmentHeadUsernameExists(username))
        {
            ShowMessage("This username is already registered.");
            return;
        }

        if (DALDepartmentHead.DepartmentHeadEmailExists(email))
        {
            ShowMessage("This email address is already registered.");
            return;
        }

        string temporaryPassword = GenerateTemporaryPassword();
        string passwordHash = PasswordHelper.HashPassword(temporaryPassword);

        MDepartmentHead hod = new MDepartmentHead(
            0,
            name,
            username,
            passwordHash,
            email,
            phone,
            designation,
            status,
            departmentID
        );

        try
        {
            int result = DALDepartmentHead.RegisterDepartmentHead(hod);

            if (result == 1)
            {
                lblMessage.Text =
                    "Department Head registered successfully. " +
                    "Temporary password: " + Server.HtmlEncode(temporaryPassword) +
                    ". Share it privately with the account holder.";

                lblMessage.CssClass =
                    "register-hod-message register-hod-message-success";

                ClearForm();
            }
            else
            {
                ShowMessage(
                    "Registration failed. Please verify the information and try again.");
            }
        }
        catch (Exception)
        {
            ShowMessage(
                "An unexpected error occurred during registration. Please try again.");
        }
    }

    private string GenerateTemporaryPassword()
    {
        const string chars =
            "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789@#$!";

        char[] password = new char[12];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            byte[] randomBytes = new byte[password.Length];
            rng.GetBytes(randomBytes);

            for (int i = 0; i < password.Length; i++)
            {
                password[i] = chars[randomBytes[i] % chars.Length];
            }
        }

        return new string(password);
    }

    private void ClearForm()
    {
        txtHodName.Text = string.Empty;
        txtUsername.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtPhone.Text = string.Empty;
        txtDesignation.Text = string.Empty;

        ddlDepartment.SelectedIndex = 0;
        ddlStatus.SelectedValue = "active";
    }

    private void ShowMessage(string message)
    {
        lblMessage.Text = Server.HtmlEncode(message);
        lblMessage.CssClass =
            "register-hod-message register-hod-message-error";
    }
}