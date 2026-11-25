
using System;
using System.Data;
using System.Web.UI;

public partial class Staff_StaffDashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["StaffID"] == null)
        {
            Response.Redirect("~/Staff/StaffLogin.aspx");
            return;
        }

        if (!IsPostBack)
        {
            LoadDashboard();
        }
    }

    private void LoadDashboard()
    {
        int staffID = Convert.ToInt32(Session["StaffID"]);

        DataTable statistics =
            DALGrievance.GetStaffDashboardStatistics(staffID);

        if (statistics.Rows.Count > 0)
        {
            DataRow row = statistics.Rows[0];

            lblTotalAssigned.Text =
                Convert.ToString(row["TotalAssignedGrievances"]);

            lblPending.Text =
                Convert.ToString(row["PendingGrievances"]);

            lblInProcess.Text =
                Convert.ToString(row["InProcessGrievances"]);

            lblResolved.Text =
                Convert.ToString(row["ResolvedGrievances"]);
        }

        DataTable grievances =
            DALGrievance.GetStaffAssignedGrievances(staffID);

        gvAssignedGrievances.DataSource = grievances;
        gvAssignedGrievances.DataBind();
    }
}
