using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_GrievanceSubmitted : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        int sessionStudentID;

        if (Session["StdID"] == null ||
            !int.TryParse(Session["StdID"].ToString(), out sessionStudentID) ||
            sessionStudentID <= 0)
        {
            Session.Clear();
            Response.Redirect("~/Student/StudentLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (!IsPostBack)
        {
            int studentID;
            int grievanceID;

            if (!int.TryParse(Session["StdID"].ToString(), out studentID) ||
                !int.TryParse(Request.QueryString["ref"], out grievanceID) ||
                grievanceID <= 0)
            {
                lblReferenceNo.Text = "N/A";
                return;
            }

            try
            {
                MGrievance grievance =
                    DALGrievance.GetGrievanceByID(grievanceID);

                if (grievance != null && grievance.StdID == studentID)
                {
                    lblReferenceNo.Text = "GRV-" + grievanceID;
                }
                else
                {
                    lblReferenceNo.Text = "N/A";
                }
            }
            catch
            {
                lblReferenceNo.Text = "N/A";
            }
        }
    }

    protected void btnGoToGrievances_Click(object sender, EventArgs e)
    {
        Response.Redirect(
            "~/Student/StudentDashboard.aspx?view=TrackGrievance");
    }

    protected void btnBackToDashboard_Click(object sender, EventArgs e)
    {
        Response.Redirect(
            "~/Student/StudentDashboard.aspx?view=Overview");
    }
}