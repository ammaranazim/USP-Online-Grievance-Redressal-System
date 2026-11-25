using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Admin_Default : System.Web.UI.Page
{
    private void BindGrid()

    {
        string keyword = txtSearch.Text.Trim();

        GridView1.DataSource = DALStudent.SearchStudent(keyword);
        GridView1.DataBind();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
        }
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        int stdID = Convert.ToInt32(GridView1.DataKeys[e.NewEditIndex].Value);

        Response.Redirect("EditStudent.aspx?StdID=" + stdID);
    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = GridView1.Rows[e.RowIndex];

        int stdID = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

        string regNo = ((TextBox)row.FindControl("txtRegNo")).Text;
        string firstName = ((TextBox)row.FindControl("txtFirstName")).Text;
        string lastName = ((TextBox)row.FindControl("txtLastName")).Text;
        string fatherName = ((TextBox)row.FindControl("txtFatherName")).Text;
        string cnic = ((TextBox)row.FindControl("txtCNIC")).Text;
        string phone = ((TextBox)row.FindControl("txtPhoneNo")).Text;
        string semester = ((TextBox)row.FindControl("txtSemester")).Text;
        string program = ((TextBox)row.FindControl("txtProgram")).Text;
        string session = ((TextBox)row.FindControl("txtSession")).Text;
        string description = ((TextBox)row.FindControl("txtDescription")).Text;


        string gender = ((DropDownList)row.FindControl("ddlGender")).SelectedValue;
        string status = ((DropDownList)row.FindControl("ddlStatus")).SelectedValue;

        string address = ((TextBox)row.FindControl("txtAddress")).Text;

        DateTime dob = Convert.ToDateTime(((TextBox)row.FindControl("txtDOB")).Text);
        DateTime enrollment = Convert.ToDateTime(((TextBox)row.FindControl("txtEnrollmentDate")).Text);

        string city = ((TextBox)row.FindControl("txtCity")).Text;
        string email = ((TextBox)row.FindControl("txtEmail")).Text;

        int deptID = Convert.ToInt32(row.Cells[18].Text);
        

        MStudent std = new MStudent(
            stdID,
            regNo,
            firstName,
            lastName,
            fatherName,
            cnic,
            phone,
            semester,
            program,
            session,
            description,
            gender,
            address,
            dob,
            enrollment,
            city,
            status,
            deptID,
            email
        );

        int result = DALStudent.UpdateStudent(std);

        if (result > 0)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int stdID = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

        int result = DALStudent.DeleteStudent(stdID);

        if (result > 0)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        BindGrid();
    }
}
     