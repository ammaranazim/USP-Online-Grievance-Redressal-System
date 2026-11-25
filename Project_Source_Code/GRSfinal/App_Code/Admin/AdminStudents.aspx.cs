using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Students : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadFilterOptions();
            BindStudents();
        }
    }

    private void LoadFilterOptions()
    {
        // Department dropdown
        ddlDepartment.Items.Clear();
        ddlDepartment.Items.Add(
            new ListItem("All Departments", "0"));

        DataTable departmentTable = DALDepartment.GetAllDepartments();

        foreach (DataRow row in departmentTable.Rows)
        {
            ddlDepartment.Items.Add(
                new ListItem(
                    Convert.ToString(row["deptName"]),
                    Convert.ToString(row["deptID"])));
        }

        // Program dropdown: load distinct programs from StudentInfo
        ddlProgram.Items.Clear();
        ddlProgram.Items.Add(
            new ListItem("All Programs", ""));

        DataTable studentTable = DALStudent.SearchStudent("");

        DataView programView = new DataView(studentTable);
        DataTable programs = programView.ToTable(true, "stdProgram");

        foreach (DataRow row in programs.Rows)
        {
            string program = Convert.ToString(row["stdProgram"]).Trim();

            if (!String.IsNullOrWhiteSpace(program))
            {
                ddlProgram.Items.Add(
                    new ListItem(program, program));
            }
        }

        // Semester dropdown: always populate semesters 1 to 8
        ddlSemester.Items.Clear();

        ddlSemester.Items.Add(new ListItem("All Semesters", ""));

        for (int semester = 1; semester <= 8; semester++)
        {
            ddlSemester.Items.Add(
                new ListItem(
                    semester.ToString(),
                    semester.ToString()
                )
            );
        }

        ddlSemester.DataBind();
    }

    private void BindStudents()
    {
        int departmentID;

        if (!Int32.TryParse(
                ddlDepartment.SelectedValue,
                out departmentID))
        {
            departmentID = 0;
        }

        DataTable students = DALStudent.GetStudentsForAdmin(
            txtSearch.Text.Trim(),
            departmentID,
            ddlStatus.SelectedValue,
            ddlProgram.SelectedValue,
            ddlSemester.SelectedValue);

        gvStudents.DataSource = students;
        gvStudents.DataBind();

        lblRecordCount.Text =
            students.Rows.Count == 1
                ? "1 student"
                : students.Rows.Count + " students";

        lblMessage.Text = String.Empty;
    }

    protected void btnApplyFilters_Click(object sender, EventArgs e)
    {
        BindStudents();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = String.Empty;
        ddlDepartment.SelectedValue = "0";
        ddlStatus.SelectedValue = String.Empty;
        ddlProgram.SelectedValue = String.Empty;
        ddlSemester.SelectedValue = String.Empty;

        BindStudents();
    }


    protected void gvStudents_RowCommand(
        object sender,
        GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditStudent")
        {
            int studentID;

            if (Int32.TryParse(
                    Convert.ToString(e.CommandArgument),
                    out studentID)
                && studentID > 0)
            {
                Response.Redirect(
                    "~/Admin/EditStudent.aspx?StdID=" + studentID,
                    false);

                Context.ApplicationInstance.CompleteRequest();
            }

            return;
        }

if (e.CommandName == "ChangeStudentStatus")
{
    int studentID;

    if (!Int32.TryParse(
            Convert.ToString(e.CommandArgument),
            out studentID)
        || studentID <= 0)
    {
        lblMessage.Text = "Invalid student record.";
        return;
    }

    GridViewRow row =
        (GridViewRow)((Control)e.CommandSource).NamingContainer;

    string currentStatus = Convert.ToString(
        gvStudents.DataKeys[row.RowIndex].Values["stdStatus"]);

    string newStatus;

    if (String.Equals(
            currentStatus, "Active",
            StringComparison.OrdinalIgnoreCase))
    {
        newStatus = "Inactive";
    }
    else if (String.Equals(
                 currentStatus, "Inactive",
                 StringComparison.OrdinalIgnoreCase))
    {
        newStatus = "Active";
    }
    else
    {
        lblMessage.Text = "Unknown student status.";
        return;
    }

    try
    {
        MStudent student = DALStudent.GetStudentByID(studentID);

        if (student == null)
        {
            lblMessage.Text = "Student record not found.";
            return;
        }

        student.StdStatus = newStatus;

        int result = DALStudent.UpdateStudent(student);

        if (result > 0)
        {
            BindStudents();
            lblMessage.Text = "Student status updated successfully.";
        }
        else
        {
            lblMessage.Text = "Student status could not be updated.";
        }
    }
    catch (Exception)
    {
        lblMessage.Text =
            "Unable to update student status. Please try again.";
    }

    return;
}

    }

}