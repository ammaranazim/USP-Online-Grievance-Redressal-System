
using System;

public partial class Staff_StaffChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["StaffID"] == null)
        {
            Response.Redirect("~/Staff/StaffLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (Session["MustChangePassword"] == null ||
            !Convert.ToBoolean(Session["MustChangePassword"]))
        {
            Response.Redirect("~/Staff/StaffDashboard.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }
    }

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        Page.Validate();

        if (!Page.IsValid)
            return;

        string currentPassword = txtCurrentPassword.Text;
        string newPassword = txtNewPassword.Text;

        if (string.Equals(currentPassword, newPassword,
            StringComparison.Ordinal))
        {
            lblMessage.Text =
                "Your new password must be different from your current password.";
            return;
        }

        try
        {
            int staffID = Convert.ToInt32(Session["StaffID"]);

            bool changed = DALStaff.ChangeStaffPassword(
                staffID, currentPassword, newPassword);

            if (changed)
            {
                Session["MustChangePassword"] = false;

                Response.Redirect(
                    "~/Staff/StaffDashboard.aspx", false);

                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            lblMessage.Text =
                "Current password is incorrect, or your account is inactive.";
        }
        catch (Exception)
        {
            lblMessage.Text =
                "Unable to change your password right now. Please try again.";
        }
    }
}
