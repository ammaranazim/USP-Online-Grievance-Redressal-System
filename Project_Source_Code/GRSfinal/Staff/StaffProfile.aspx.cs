using System;
using System.Web.UI;

public partial class Staff_StaffProfile : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["StaffID"] == null)
        {
            Response.Redirect("~/Staff/StaffLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (!IsPostBack)
        {
            LoadStaffProfile();
        }
    }

    private void LoadStaffProfile()
    {
        string staffName = GetSessionValue("StaffName", "Staff Member");

        lblProfileName.Text = Server.HtmlEncode(staffName);
        lblStaffFullName.Text = Server.HtmlEncode(staffName);

        lblStaffID.Text = Server.HtmlEncode(
            Convert.ToString(Session["StaffID"]));

        lblStaffUsername.Text = Server.HtmlEncode(
            GetSessionValue("StaffUsername", "Not available"));

        lblStaffEmail.Text = Server.HtmlEncode(
            GetSessionValue("StaffEmail", "Not available"));

        lblStaffPhone.Text = Server.HtmlEncode(
            GetSessionValue("StaffPhoneNo", "Not available"));

        string designation = GetSessionValue(
            "StaffDesignation", "Staff Member");

        lblProfileDesignation.Text = Server.HtmlEncode(designation);
        lblStaffDesignation.Text = Server.HtmlEncode(designation);

        lblDepartmentName.Text = Server.HtmlEncode(
            GetSessionValue("DeptName", "Not assigned"));

        lblProfileStatus.Text = Server.HtmlEncode(
            GetSessionValue("StaffStatus", "Unknown"));

        lblProfileInitials.Text = Server.HtmlEncode(
            GetInitials(staffName));
    }

    private string GetSessionValue(string key, string fallback)
    {
        string value = Convert.ToString(Session[key]);

        return string.IsNullOrWhiteSpace(value)
            ? fallback
            : value.Trim();
    }

    private string GetInitials(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "S";
        }

        string[] parts = name.Trim().Split(
            new char[] { ' ' },
            StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 1)
        {
            return parts[0].Substring(0, 1).ToUpperInvariant();
        }

        return (
            parts[0].Substring(0, 1) +
            parts[parts.Length - 1].Substring(0, 1)
        ).ToUpperInvariant();
    }
}