using System;
using System.Web.UI;

public partial class Admin_AdminLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string login = txtLogin.Text.Trim();
        string password = txtPassword.Text.Trim();

        // Check empty fields
        if (string.IsNullOrEmpty(login))
        {
            lblMessage.Text = "Please enter your admin name or email.";
            txtLogin.Focus();
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            lblMessage.Text = "Please enter your password.";
            txtPassword.Focus();
            return;
        }

        // Check admin credentials
        MLogin admin =
            DALAdminLogin.AdminLogin(login, password);

        if (admin != null)
        {
            // Store admin information in session
            Session["AdminID"] = admin.AdminLoginID;
            Session["AdminName"] = admin.AdminLoginName;
            Session["AdminEmail"] = admin.AdminLoginEmail;

            // Redirect to Admin Dashboard
            Response.Redirect(
                "~/Admin/AdminDashboard.aspx",
                false);

            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            lblMessage.Text =
                "Invalid admin name/email or password.";
        }
    }
}