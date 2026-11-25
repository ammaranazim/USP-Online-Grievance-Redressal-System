
using System;
using System.Data;
using System.Security.Cryptography;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AdminRegisterStaff : System.Web.UI.Page
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
            LoadDepartments();
        }
    }

    private void LoadDepartments()
    {
        DataTable departments = DALDepartment.GetAllDepartments();

        ddlDepartment.Items.Clear();

        if (departments != null)
        {
            ddlDepartment.DataSource = departments;
            ddlDepartment.DataTextField = "deptName";
            ddlDepartment.DataValueField = "deptID";
            ddlDepartment.DataBind();
        }

        ddlDepartment.Items.Insert(
            0,
            new ListItem("Select Department", "")
        );
    }

    protected void btnRegisterStaff_Click(object sender, EventArgs e)
    {
        Page.Validate("StaffRegistration");

        if (!Page.IsValid)
            return;

        string name = txtStaffName.Text.Trim();
        string username = txtUsername.Text.Trim();
        string email = txtEmail.Text.Trim();
        string phone = txtPhone.Text.Trim();
        string designation = txtDesignation.Text.Trim();

        int deptID;

        if (!int.TryParse(ddlDepartment.SelectedValue, out deptID) ||
            deptID <= 0)
        {
            ShowMessage(
                "Please select a valid department.",
                false
            );
            return;
        }

        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(phone) ||
            string.IsNullOrWhiteSpace(designation))
        {
            ShowMessage(
                "Please complete all required fields.",
                false
            );
            return;
        }

        try
        {
            if (DALStaff.IsStaffUsernameExists(username))
            {
                ShowMessage(
                    "This username is already registered. Please choose another.",
                    false
                );
                return;
            }

            // Generate a temporary password automatically.
            string temporaryPassword = GenerateTemporaryPassword();

            MStaff staff = new MStaff(
                0,
                name,
                username,
                email,
                phone,
                designation,
                "Active",
                deptID,
                true,
                string.Empty
            );

            // DALStaff.RegisterStaff hashes the password internally.
            int result = DALStaff.RegisterStaff(
                staff,
                temporaryPassword
            );

            if (result > 0)
            {
                lblMessage.Text =
                    "Staff account registered successfully. " +
                    "Temporary password: " +
                    Server.HtmlEncode(temporaryPassword) +
                    ". Share it privately with the staff member.";

                lblMessage.CssClass =
                    "register-message register-message-success";

                ClearForm();
            }
            else
            {
                ShowMessage(
                    "Registration could not be completed. Please check the information and try again.",
                    false
                );
            }
        }
        catch (Exception)
        {
            ShowMessage(
                "An unexpected error occurred during registration. Please try again.",
                false
            );
        }
    }

    private string GenerateTemporaryPassword()
    {
        const string chars =
            "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789@#$!";

        char[] password = new char[12];
        byte[] randomBytes = new byte[password.Length];

        using (RandomNumberGenerator rng =
            RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        for (int i = 0; i < password.Length; i++)
        {
            password[i] = chars[randomBytes[i] % chars.Length];
        }

        return new string(password);
    }

    private void ClearForm()
    {
        txtStaffName.Text = string.Empty;
        txtUsername.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtPhone.Text = string.Empty;
        txtDesignation.Text = string.Empty;

        if (ddlDepartment.Items.Count > 0)
        {
            ddlDepartment.SelectedIndex = 0;
        }
    }

    private void ShowMessage(string message, bool success)
    {
        lblMessage.Text = Server.HtmlEncode(message);

        lblMessage.CssClass = success
            ? "register-message register-message-success"
            : "register-message register-message-error";
    }
}
