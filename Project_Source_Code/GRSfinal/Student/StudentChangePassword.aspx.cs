using System;
using System.Web.UI;

public partial class Student_StudentChangePassword : System.Web.UI.Page
{
protected void Page_Load(object sender, EventArgs e)
{
    if (Session["StdID"] == null ||
        Session["StdLoginID"] == null)
    {
        Response.Redirect("~/Student/StudentLogin.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
        return;
    }

    try
    {
        int loginID = Convert.ToInt32(Session["StdLoginID"]);
        int studentID = Convert.ToInt32(Session["StdID"]);

        bool? mustChangePassword =
            DALStudentLogin.GetMustChangePassword(loginID, studentID);

        if (!mustChangePassword.HasValue)
        {
            Session.Clear();
            Response.Redirect("~/Student/StudentLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        Session["MustChangePassword"] = mustChangePassword.Value;

        if (!mustChangePassword.Value)
        {
            Response.Redirect("~/Student/StudentDashboard.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }
    }
    catch (Exception)
    {
        // Do not allow password changes when account status
        // cannot be verified.
        Response.Redirect("~/Student/StudentLogin.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
        return;
    }
}
    protected void btnUpdatePassword_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;

        if (!Page.IsValid)
            return;

        string currentPassword = txtCurrentPassword.Text;
        string newPassword = txtNewPassword.Text;
        string confirmPassword = txtConfirmPassword.Text;

        if (newPassword.Length < 8 || newPassword.Length > 128)
        {
            ShowMessage(
                "Your new password must be between 8 and 128 characters.",
                false);
            return;
        }

        if (newPassword != confirmPassword)
        {
            ShowMessage("The new passwords do not match.", false);
            return;
        }

        if (currentPassword == newPassword)
        {
            ShowMessage(
                "Your new password must be different from your current password.",
                false);
            return;
        }

        try
        {
            int loginID = Convert.ToInt32(Session["StdLoginID"]);
            int studentID = Convert.ToInt32(Session["StdID"]);

            int result = DALStudentLogin.ChangeStudentPassword(
                loginID,
                studentID,
                currentPassword,
                newPassword);

            if (result == 1)
            {
                Session["MustChangePassword"] = false;

                txtCurrentPassword.Text = string.Empty;
                txtNewPassword.Text = string.Empty;
                txtConfirmPassword.Text = string.Empty;

                Response.Redirect("StudentDashboard.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            ShowMessage(
                "Password change failed. Check your current password and try again.",
                false);
        }
        catch (Exception)
        {
            ShowMessage(
                "Your password could not be changed. Please try again later.",
                false);
        }
    }

    private void ShowMessage(string message, bool isSuccess)
    {
        lblMessage.Text = Server.HtmlEncode(message);
        lblMessage.ForeColor = isSuccess
            ? System.Drawing.Color.Green
            : System.Drawing.Color.Red;
    }
}