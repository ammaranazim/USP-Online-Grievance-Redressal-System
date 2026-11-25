using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Admin_AdminDashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDashboardStatistics();
            LoadAttentionStatistics();
            LoadCategoryStatistics();
            LoadDepartmentStatistics();
            LoadRecentGrievances();
            LoadGrievanceTrend();
        }
    }


    private void LoadDashboardStatistics()
    {
        try
        {
            DataTable dt =
                DALGrievance.GetAdminDashboardStatistics();

            if (dt.Rows.Count > 0)
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
        catch (Exception ex)
        {
            // Keep dashboard usable if database
            // statistics cannot be loaded.
            lblTotalGrievances.Text = "0";
            lblPendingGrievances.Text = "0";
            lblInProcessGrievances.Text = "0";
            lblResolvedGrievances.Text = "0";
        }
    }
    private void LoadCategoryStatistics()
    {
        try
        {
            DataTable dt =
                DALGrievance.GetAdminDashboardCategoryStatistics();

            lblAcademiaCount.Text = "0";
            lblAccountsCount.Text = "0";
            lblExamDegreeCount.Text = "0";

            foreach (DataRow row in dt.Rows)
            {
                string categoryName =
                    row["categoryName"].ToString();

                string count =
                    row["GrievanceCount"].ToString();

                if (categoryName.Equals(
                    "Academia",
                    StringComparison.OrdinalIgnoreCase))
                {
                    lblAcademiaCount.Text = count;
                }
                else if (categoryName.Equals(
                    "Accounts",
                    StringComparison.OrdinalIgnoreCase))
                {
                    lblAccountsCount.Text = count;
                }
                else if (categoryName.Equals(
                    "Exam & Degree",
                    StringComparison.OrdinalIgnoreCase))
                {
                    lblExamDegreeCount.Text = count;
                }
            }
        }
        catch (Exception)
        {
            lblAcademiaCount.Text = "0";
            lblAccountsCount.Text = "0";
            lblExamDegreeCount.Text = "0";
        }
        ScriptManager.RegisterStartupScript(
    this,
    GetType(),
    "CategoryDonutChart",
    "loadCategoryDonutChart(" +
        lblAcademiaCount.Text + "," +
        lblAccountsCount.Text + "," +
        lblExamDegreeCount.Text +
    ");",
    true
);
    }
    private void LoadDepartmentStatistics()
    {
        try
        {
            DataTable dt =
                DALGrievance.GetAdminDashboardDepartmentStatistics();

            rptDepartmentOverview.DataSource = dt;
            rptDepartmentOverview.DataBind();
        }
        catch (Exception)
        {
            rptDepartmentOverview.DataSource = null;
            rptDepartmentOverview.DataBind();
        }
    }
    private void LoadRecentGrievances()
    {
        try
        {
            DataTable dt =
                DALGrievance.GetRecentAdminGrievances();

            rptRecentGrievances.DataSource = dt;
            rptRecentGrievances.DataBind();
        }
        catch (Exception)
        {
            rptRecentGrievances.DataSource = null;
            rptRecentGrievances.DataBind();
        }
    }

    protected string GetStatusCssClass(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return string.Empty;

        if (status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            return "status-pending";

        if (status.Equals("In Process", StringComparison.OrdinalIgnoreCase))
            return "status-in-process";

        if (status.Equals("Resolved", StringComparison.OrdinalIgnoreCase))
            return "status-resolved";

        return string.Empty;
    }

    private void LoadGrievanceTrend()
    {
        try
        {
            DataTable dt =
                DALGrievance.GetAdminDashboardGrievanceTrend();

            string labels = "[";
            string values = "[";

            foreach (DataRow row in dt.Rows)
            {
                int year =
                    Convert.ToInt32(row["GrievanceYear"]);

                int month =
                    Convert.ToInt32(row["GrievanceMonth"]);

                int count =
                    Convert.ToInt32(row["GrievanceCount"]);

                DateTime date =
                    new DateTime(year, month, 1);

                labels += "'" +
                          date.ToString("MMM yyyy") +
                          "',";

                values += count + ",";
            }

            labels += "]";
            values += "]";

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "GrievanceTrendChart",
                "loadGrievanceTrendChart(" +
                    labels + "," +
                    values +
                ");",
                true
            );
        }
        catch (Exception)
        {
            // Keep dashboard usable
        }
    }


    private void LoadAttentionStatistics()
    {
        // Default value in case the query returns no rows.
        lblEscalatedGrievances.Text = "0";

        try
        {
            DataTable dt =
                DALGrievance.GetAdminDashboardAttentionStatistics();

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                lblEscalatedGrievances.Text =
                    row["EscalatedGrievances"].ToString();
            }
        }
        catch (Exception)
        {
           
        }
    }

}