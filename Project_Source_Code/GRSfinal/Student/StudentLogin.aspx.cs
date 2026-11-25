
using System;
using System.Web.UI;

public partial class Student_StudentLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int stdID;
            int stdLoginID;

            if (int.TryParse(Convert.ToString(Session["StdID"]), out stdID) &&
                stdID > 0 &&
                int.TryParse(Convert.ToString(Session["StdLoginID"]), out stdLoginID) &&
                stdLoginID > 0)
            {
                Response.Redirect("StudentDashboard.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            Session.Clear();
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;

        string regNo = txtRegNo.Text.Trim();
        string password = txtPassword.Text;

        // Registration number validation
        if (string.IsNullOrWhiteSpace(regNo))
        {
            lblMessage.Text = "Please enter registration number.";
            return;
        }

        // Password validation
        if (string.IsNullOrEmpty(password))
        {
            lblMessage.Text = "Please enter password.";
            return;
        }

        try
        {
            MstudentLogin login =
                DALStudentLogin.ValidateLogin(regNo, password);

            if (login == null)
            {
                lblMessage.Text =
                    "Invalid Registration Number or Password, or your account is inactive.";
                return;
            }

            // Set existing Student Module session keys
            Session["StdLoginID"] = login.StdLoginID;
            Session["StdID"] = login.StdID;
            MStudent student = DALStudent.GetStudentByID(login.StdID);

            if (student == null)
            {
                Session.Clear();
                lblMessage.Text = "Student record not found. Please contact the administrator.";
                return;
            }

            Session["StdRegNo"] = student.StdRegNo;

            // Retrieve student name
            string studentName =
                DALStudent.GetStudentNameByID(login.StdID);

            if (string.IsNullOrWhiteSpace(studentName))
            {
                Session.Clear();
                lblMessage.Text =
                    "Unable to load your student profile. Please contact Admin.";
                return;
            }

            Session["StdName"] = studentName;
            Session["MustChangePassword"] = login.MustChangePassword;

            if (login.MustChangePassword)
            {
                Response.Redirect("StudentChangePassword.aspx", false);
            }
            else
            {
                Response.Redirect("StudentDashboard.aspx", false);
            }

            Context.ApplicationInstance.CompleteRequest();
        }
        catch (Exception)
        {
            lblMessage.Text =
                "Login could not be completed. Please try again later.";
        }
    }
}
