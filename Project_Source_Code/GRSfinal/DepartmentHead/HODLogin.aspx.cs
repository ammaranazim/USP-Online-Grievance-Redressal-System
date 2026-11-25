using System;
using System.Web.UI;

public partial class DepartmentHead_DepartmentHeadLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtUsername.Focus();
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Page.Validate();

        if (!Page.IsValid)
        {
            return;
        }

        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text;

        try
        {
            MDepartmentHead hod =
                DALDepartmentHead.ValidateLogin(username, password);

            if (hod == null)
            {
                lblMessage.Text =
                    "Invalid username or password, or your account is inactive.";

                return;
            }

            // Clear any previous session.
            Session.Clear();

            // Store authenticated HOD information.
            Session["DeptHeadID"] = hod.DeptHeadID;
            Session["DeptHeadName"] = hod.DeptHeadName;
            Session["DeptHeadUsername"] = hod.DeptHeadUserName;
            Session["DeptID"] = hod.DeptID;
            Session["MustChangePassword"] = hod.MustChangePassword;

            // Mandatory password change for first login.
            if (hod.MustChangePassword)
            {
                Response.Redirect(
                    "~/DepartmentHead/DepartmentHeadChangePassword.aspx",
                    false);

                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // Destination will be implemented in the next step.
            Response.Redirect(
                "~/DepartmentHead/HODDashboard.aspx",
                false);

            Context.ApplicationInstance.CompleteRequest();
        }
        catch (Exception)
        {
            lblMessage.Text =
                "Unable to sign in right now. Please try again later.";
        }
    }
}