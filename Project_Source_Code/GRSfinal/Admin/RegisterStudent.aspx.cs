
using System;
using System.Security.Cryptography;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_RegisterStudent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDepartments();
        }
    }

    private void BindDepartments()
    {
        ddlDepartment.DataSource = DALDepartment.GetAllDepartments();
        ddlDepartment.DataTextField = "deptName";
        ddlDepartment.DataValueField = "deptID";
        ddlDepartment.DataBind();

        ddlDepartment.Items.Insert(
            0, new ListItem("-- Select Department --", "0"));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;

        if (ddlDepartment.SelectedValue == "0")
        {
            ShowMessage("Please select a department.", false);
            return;
        }

        if (string.IsNullOrWhiteSpace(ddlGender.SelectedValue))
        {
            ShowMessage("Please select gender.", false);
            return;
        }

        if (string.IsNullOrWhiteSpace(txtRegNo.Text) ||
            string.IsNullOrWhiteSpace(txtFirstName.Text) ||
            string.IsNullOrWhiteSpace(txtEmail.Text))
        {
            ShowMessage(
                "Registration number, first name and email are required.",
                false);
            return;
        }

        if (string.IsNullOrWhiteSpace(ddlStatus.SelectedValue))
        {
            ShowMessage("Please select student status.", false);
            return;
        }

        DateTime dob;
        DateTime enrollmentDate;

        if (!DateTime.TryParse(txtDOB.Text, out dob))
        {
            ShowMessage("Please enter a valid date of birth.", false);
            return;
        }

        if (!DateTime.TryParse(txtEnrollmentDate.Text, out enrollmentDate))
        {
            ShowMessage("Please enter a valid enrollment date.", false);
            return;
        }

        try
        {
            MStudent student = new MStudent(
                0,
                txtRegNo.Text.Trim(),
                txtFirstName.Text.Trim(),
                txtLastName.Text.Trim(),
                txtFatherName.Text.Trim(),
                txtCNIC.Text.Trim(),
                txtPhoneNo.Text.Trim(),
                txtSemester.Text.Trim(),
                txtProgram.Text.Trim(),
                txtSession.Text.Trim(),
                txtDescription.Text.Trim(),
                ddlGender.SelectedValue,
                txtAddress.Text.Trim(),
                dob,
                enrollmentDate,
                txtCity.Text.Trim(),
                ddlStatus.SelectedValue,
                Convert.ToInt32(ddlDepartment.SelectedValue),
                txtEmail.Text.Trim()
            );

            string temporaryPassword = GenerateTemporaryPassword();

            int result = DALStudent.RegisterStudentWithLogin(
                student, temporaryPassword);

            if (result > 0)
            {
                ClearForm();

                lblMessage.Text =
                    "Student registered successfully. " +
                    "Temporary password: <strong>" +
                    Server.HtmlEncode(temporaryPassword) +
                    "</strong>. Give this password to the student privately. " +
                    "The student must change it after the first login.";

                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                ShowMessage("Student registration failed.", false);
            }
        }
        catch (InvalidOperationException ex)
        {
            ShowMessage(ex.Message, false);
        }
        catch (Exception)
        {
            ShowMessage(
                "Registration could not be completed. Please check the " +
                "entered information and try again.", false);
        }
    }

    private string GenerateTemporaryPassword()
    {
        byte[] bytes = new byte[12];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }

        return Convert.ToBase64String(bytes)
            .Replace("+", "A")
            .Replace("/", "B")
            .Replace("=", "C");
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
        ShowMessage("Form cleared.", true);
    }

    private void ClearForm()
    {
        txtRegNo.Text = string.Empty;
        txtFirstName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtFatherName.Text = string.Empty;
        txtCNIC.Text = string.Empty;
        txtPhoneNo.Text = string.Empty;
        txtSemester.Text = string.Empty;
        txtProgram.Text = string.Empty;
        txtSession.Text = string.Empty;
        txtDescription.Text = string.Empty;
        txtAddress.Text = string.Empty;
        txtDOB.Text = string.Empty;
        txtEnrollmentDate.Text = string.Empty;
        txtCity.Text = string.Empty;
        txtEmail.Text = string.Empty;

        if (ddlGender.Items.Count > 0)
            ddlGender.SelectedIndex = 0;

        if (ddlStatus.Items.Count > 0)
            ddlStatus.SelectedIndex = 0;

        if (ddlDepartment.Items.Count > 0)
            ddlDepartment.SelectedIndex = 0;
    }

    private void ShowMessage(string message, bool isSuccess)
    {
        lblMessage.Text = Server.HtmlEncode(message);
        lblMessage.ForeColor = isSuccess
            ? System.Drawing.Color.Green
            : System.Drawing.Color.Red;
    }
}
