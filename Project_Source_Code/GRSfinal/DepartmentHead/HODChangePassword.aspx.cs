using System;

public partial class DepartmentHead_HODChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["DeptHeadID"] == null)
        {
            Response.Redirect("~/DepartmentHead/DepartmentHeadLogin.aspx");
            return;
        }

        if (Session["MustChangePassword"] == null ||
            !Convert.ToBoolean(Session["MustChangePassword"]))
        {
            Response.Redirect("~/DepartmentHead/DepartmentHeadDashboard.aspx");
            return;
        }
    }

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        Page.Validate();

        if (!Page.IsValid)
            return;

        int deptHeadID = Convert.ToInt32(Session["DeptHeadID"]);
        string currentPassword = txtCurrentPassword.Text;
        string newPassword = txtNewPassword.Text;

        try
        {
            bool changed = DALDepartmentHead.ChangeDepartmentHeadPassword(
                deptHeadID,
                currentPassword,
                newPassword
            );

            if (changed)
            {
                Session["MustChangePassword"] = false;

                Response.Redirect(
                    "~/DepartmentHead/DepartmentHeadDashboard.aspx",
                    false
                );

                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                lblMessage.Text =
                    "Current password is incorrect, or your account is inactive.";
            }
        }
        catch (Exception)
        {
            lblMessage.Text =
                "Unable to change your password right now. Please try again.";
        }
    }
}