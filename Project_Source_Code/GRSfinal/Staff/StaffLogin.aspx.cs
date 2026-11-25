
using System;

public partial class Staff_StaffLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Already logged-in staff can continue to the dashboard.
            if (Session["StaffID"] != null)
            {
                Response.Redirect(
                    "~/Staff/StaffDashboard.aspx", false);

                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            txtUsername.Focus();
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Page.Validate();

        if (!Page.IsValid)
            return;

        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text;

        try
        {
            MStaff staff = DALStaff.ValidateLogin(username, password);

            if (staff == null)
            {
                lblMessage.Text =
                    "Invalid username or password, or your account is inactive.";
                return;
            }

            // Start a fresh authenticated session.
            Session.Clear();

            Session["StaffID"] = staff.StaffID;
            Session["StaffName"] = staff.StaffName;
            Session["StaffUsername"] = staff.StaffUserName;
            Session["StaffEmail"] = staff.StaffEmail;
            Session["StaffPhoneNo"] = staff.StaffPhoneNo;
            Session["StaffDesignation"] = staff.StaffDesignation;
            Session["StaffStatus"] = staff.StaffStatus;
            Session["DeptID"] = staff.DeptID;
            Session["DeptName"] = staff.DeptName;
            Session["MustChangePassword"] = staff.MustChangePassword;

            if (staff.MustChangePassword)
            {
                Response.Redirect(
                    "~/Staff/StaffChangePassword.aspx", false);

                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            Response.Redirect(
                "~/Staff/StaffDashboard.aspx", false);

            Context.ApplicationInstance.CompleteRequest();
        }
        catch (Exception)
        {
            lblMessage.Text =
                "Unable to sign in right now. Please try again later.";
        }
    }
}
