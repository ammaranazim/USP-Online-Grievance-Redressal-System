using System;
using System.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;

public partial class DepartmentHead_HODDashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int deptID;

            if (Session["DeptID"] == null ||
                !int.TryParse(Session["DeptID"].ToString(), out deptID))

            {
                Response.Redirect("~/DepartmentHead/HODLogin.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            LoadDashboardStatistics(deptID);
            LoadOverdueCount(deptID);
            LoadUnassignedCount(deptID);
            LoadResolutionReviewCount(deptID);
            LoadRecentGrievances(deptID);
            LoadGrievanceTrend(deptID);
        }
    }

    private void LoadDepartmentName(int deptID)
    {
        try
        {
            DataTable dt = DALGrievance.GetHODDepartmentName(deptID);

            lblDepartmentName.Text = dt.Rows.Count > 0
                ? dt.Rows[0]["deptName"].ToString()
                : "Department";
        }
        catch (Exception)
        {
            lblDepartmentName.Text = "Department";
        }
    }

    private void LoadDashboardStatistics(int deptID)
    {
        lblTotalGrievances.Text = "0";
        lblPendingGrievances.Text = "0";
        lblInProcessGrievances.Text = "0";
        lblResolvedGrievances.Text = "0";

        try
        {
            DataTable dt = DALGrievance.GetHODDashboardStatistics(deptID);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                lblTotalGrievances.Text =
                    row["TotalGrievances"].ToString();

                lblPendingGrievances.Text =
                    row["PendingGrievances"].ToString();

                lblInProcessGrievances.Text =
                    row["InProcessGrievances"].ToString();

                lblResolvedGrievances.Text =
                    row["ResolvedGrievances"].ToString();
            }
        }
        catch (Exception)
        {
            // Keep the dashboard available if statistics fail.
        }
    }

    private void LoadOverdueCount(int deptID)
    {
        lblOverdueCount.Text = "—";

        try
        {
            int overdueCount =
                DALGrievance.GetHODOverdueGrievanceCount(deptID);

            lblOverdueCount.Text = overdueCount.ToString();
        }
        catch (Exception)
        {
            // Keep the dashboard available if the count cannot be loaded.
            lblOverdueCount.Text = "—";
        }
    }
    private void LoadUnassignedCount(int deptID)
    {
        lblUnassignedCount.Text = "—";

        try
        {
            DataTable dt =
                DALGrievance.GetHODStaffAssignmentCounts(deptID);

            if (dt != null && dt.Rows.Count > 0)
            {
                lblUnassignedCount.Text =
                    Convert.ToString(dt.Rows[0]["UnassignedGrievances"]);
            }
        }
        catch (Exception)
        {
            // Keep the dashboard available if the count cannot be loaded.
            lblUnassignedCount.Text = "—";
        }
    }

    private void LoadRecentGrievances(int deptID)
    {
        try
        {
            DataTable dt =
                DALGrievance.GetHODDashboardRecentGrievances(deptID);

            gvRecentGrievances.DataSource = dt;
            gvRecentGrievances.DataBind();
        }
        catch (Exception)
        {
            gvRecentGrievances.DataSource = null;
            gvRecentGrievances.DataBind();
        }
    }

    private void LoadGrievanceTrend(int deptID)
    {
        lblTrendMessage.Visible = false;

        try
        {
            DataTable dt =
                DALGrievance.GetHODDashboardGrievanceTrend(deptID);

            List<string> labels = new List<string>();
            List<int> values = new List<int>();

            foreach (DataRow row in dt.Rows)
            {
                int year = Convert.ToInt32(row["GrievanceYear"]);
                int month = Convert.ToInt32(row["GrievanceMonth"]);
                int count = Convert.ToInt32(row["GrievanceCount"]);

                DateTime monthDate = new DateTime(year, month, 1);

                labels.Add(
                    monthDate.ToString("MMM yyyy", CultureInfo.InvariantCulture));

                values.Add(count);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string script =
                "loadHODGrievanceTrend(" +
                serializer.Serialize(labels) + "," +
                serializer.Serialize(values) +
                ");";

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "HODGrievanceTrendChart",
                script,
                true);
        }
        catch (Exception)
        {
            lblTrendMessage.Text =
                "The grievance trend is temporarily unavailable.";
            lblTrendMessage.Visible = true;
        }
    }


    protected string GetStatusBadgeClass(object statusValue)
    {
        string status = Convert.ToString(statusValue);

        switch (status)
        {
            case "Pending":
                return "status-badge status-pending";

            case "In Process":
                return "status-badge status-in-process";

            case "Resolved":
                return "status-badge status-resolved";

            case "Rejected":
                return "status-badge status-rejected";

            default:
                return "status-badge";
        }
    }
    private void LoadResolutionReviewCount(int deptID)
    {
        lblResolutionReviewCount.Text = "—";

        try
        {
            DataTable dt =
                DALGrievance.GetHODProgressResolutionStatistics(deptID);

            if (dt != null && dt.Rows.Count > 0)
            {
                lblResolutionReviewCount.Text =
                    Convert.ToString(dt.Rows[0]["AwaitingReview"]);
            }
            else
            {
                lblResolutionReviewCount.Text = "0";
            }
        }
        catch (Exception)
        {
            // Keep the dashboard available if the count cannot be loaded.
            lblResolutionReviewCount.Text = "—";
        }
    }

}