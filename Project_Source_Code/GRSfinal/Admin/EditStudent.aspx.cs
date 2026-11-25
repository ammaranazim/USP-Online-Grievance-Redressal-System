using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlDepartment.DataSource = DALDepartment.GetAllDepartments();
            ddlDepartment.DataTextField = "deptName";
            ddlDepartment.DataValueField = "deptID";
            ddlDepartment.DataBind();

            ddlDepartment.Items.Insert(0, new ListItem("-- Select Department --", "0"));

            if (Request.QueryString["StdID"] != null)
            {
                int stdID = Convert.ToInt32(Request.QueryString["StdID"]);

                LoadStudent(stdID);
            }
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int stdID = Convert.ToInt32(Request.QueryString["StdID"]);

        MStudent std = new MStudent(
            stdID,
            txtRegNo.Text,
            txtFirstName.Text,
            txtLastName.Text,
            txtFatherName.Text,
            txtCNIC.Text,
            txtPhoneNo.Text,
            txtSemester.Text,
            txtProgram.Text,
            txtSession.Text,
            txtDescription.Text,
            ddlGender.SelectedValue,
            txtAddress.Text,
            Convert.ToDateTime(txtDOB.Text),
            Convert.ToDateTime(txtEnrollmentDate.Text),
            txtCity.Text,
            ddlStatus.SelectedValue,
            Convert.ToInt32(ddlDepartment.SelectedValue),
            txtEmail.Text
        );

        int result = DALStudent.UpdateStudent(std);
   
        if (result > 0)
        {

            Response.Write("<script>alert('Student Updated Successfully'); window.location='AdminStudents.aspx';</script>");

        }
        else
        {
            Response.Write("<script>alert('Student Update Failed');</script>");
        }
    }
    private void LoadStudent(int stdID)
    {
        MStudent std = DALStudent.GetStudentByID(stdID);

        if (std != null)
        {
            txtRegNo.Text = std.StdRegNo;
            txtFirstName.Text = std.StdFirstName;
            txtLastName.Text = std.StdLastName;
            txtFatherName.Text = std.StdFName;
            txtCNIC.Text = std.StdCnic;
            txtPhoneNo.Text = std.StdPhoneNo;
            txtSemester.Text = std.StdSemester;
            txtProgram.Text = std.StdProgram;
            txtSession.Text = std.StdSession;
            txtDescription.Text = std.StdDescription;

            ddlGender.SelectedValue = std.StdGender;

            txtAddress.Text = std.StdAddress;

            txtDOB.Text = std.StdDateOfBirth.ToString("yyyy-MM-dd");
            txtEnrollmentDate.Text = std.StdEnrollmentDate.ToString("yyyy-MM-dd");

            txtCity.Text = std.StdCity;

            ddlStatus.SelectedValue = std.StdStatus;
            ddlDepartment.SelectedValue = std.DeptID.ToString();

            txtEmail.Text = std.StdEmail;
           

        }
    }
}