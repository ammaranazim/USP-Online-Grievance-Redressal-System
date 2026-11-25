
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;

public partial class Admin_AdminReports : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AdminID"] == null)
        {
            Response.Redirect("~/Admin/AdminLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        if (!IsPostBack)
        {
            LoadReportFilters();
            ResetReportResults();
        }
    }

    private void LoadReportFilters()
    {
        try
        {
            ddlCategory.DataSource =
                DALGrievanceCategory.GetAllCategories();

            ddlCategory.DataTextField = "categoryName";
            ddlCategory.DataValueField = "categoryID";
            ddlCategory.DataBind();

            ddlDepartment.DataSource =
                DALDepartment.GetAllDepartments();

            ddlDepartment.DataTextField = "deptName";
            ddlDepartment.DataValueField = "deptID";
            ddlDepartment.DataBind();
        }
        catch (Exception)
        {
            ShowMessage(
                "Unable to load report filters. Please try again.");
        }
    }

    private bool ValidateReportDates(
        out DateTime fromDate,
        out DateTime toDate)
    {
        fromDate = DateTime.MinValue;
        toDate = DateTime.MinValue;

        if (!DateTime.TryParseExact(
                txtFromDate.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out fromDate)
            ||
            !DateTime.TryParseExact(
                txtToDate.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out toDate))
        {
            ShowMessage(
                "Please select valid From Date and To Date.");
            return false;
        }

        if (fromDate.Date > toDate.Date)
        {
            ShowMessage(
                "From Date cannot be later than To Date.");
            return false;
        }

        return true;
    }

    protected void btnGenerateReport_Click(
        object sender, EventArgs e)
    {
        ResetReportResults();

        DateTime fromDate;
        DateTime toDate;

        if (!ValidateReportDates(out fromDate, out toDate))
        {
            return;
        }

        int categoryID = 0;
        int deptID = 0;

        if (!string.IsNullOrEmpty(ddlCategory.SelectedValue) &&
            !int.TryParse(
                ddlCategory.SelectedValue, out categoryID))
        {
            ShowMessage("Please select a valid category.");
            return;
        }

        if (!string.IsNullOrEmpty(ddlDepartment.SelectedValue) &&
            !int.TryParse(
                ddlDepartment.SelectedValue, out deptID))
        {
            ShowMessage("Please select a valid department.");
            return;
        }

        try
        {
            DataTable reportData =
                DALGrievance.GetAdminDetailedGrievanceReport(
                    fromDate,
                    toDate,
                    ddlStatus.SelectedValue,
                    categoryID,
                    deptID);

            if (reportData == null || reportData.Rows.Count == 0)
            {
                pnlReportEmpty.Visible = true;
                gvReportResults.Visible = false;

                ShowMessage(
                    "No Grievance records found for the selected filters.");

                return;
            }

            Session["AdminReportData"] = reportData;

            gvReportResults.AutoGenerateColumns = true;
            gvReportResults.DataSource = reportData;
            gvReportResults.DataBind();

            if (gvReportResults.HeaderRow != null)
            {
                gvReportResults.HeaderRow.Cells[0].Text = "Grievance ID";
                gvReportResults.HeaderRow.Cells[1].Text = "Title";
                gvReportResults.HeaderRow.Cells[2].Text = "Status";
                gvReportResults.HeaderRow.Cells[3].Text = "Priority";
                gvReportResults.HeaderRow.Cells[4].Text = "Submission Date";
                gvReportResults.HeaderRow.Cells[5].Text = "Resolution Date";
                gvReportResults.HeaderRow.Cells[6].Text = "Student Name";
                gvReportResults.HeaderRow.Cells[7].Text = "Registration No.";
                gvReportResults.HeaderRow.Cells[8].Text = "Category";
                gvReportResults.HeaderRow.Cells[9].Text = "Department";
            }

            gvReportResults.Visible = true;
            pnlReportEmpty.Visible = false;

            btnExportExcel.Enabled = true;
            

            lblReportMessage.CssClass =
                "admin-report-message admin-report-message-success";

            ShowMessage(
                "Report generated successfully. Records found: " +
                reportData.Rows.Count + ".");
        }
        catch (Exception)
        {
            ResetReportResults();

            ShowMessage(
                "Unable to generate the report. Please try again.");
        }
    }

    protected void btnResetFilters_Click(
        object sender, EventArgs e)
    {
        txtFromDate.Text = "";
        txtToDate.Text = "";

        ddlStatus.SelectedIndex = 0;
        ddlCategory.SelectedIndex = 0;
        ddlDepartment.SelectedIndex = 0;

        ResetReportResults();
    }

    private void ResetReportResults()
    {
        gvReportResults.DataSource = null;
        gvReportResults.DataBind();
        gvReportResults.Visible = false;

        pnlReportEmpty.Visible = true;

        btnExportExcel.Enabled = false;
       

        Session.Remove("AdminReportData");

        lblReportMessage.Text = "";
        lblReportMessage.CssClass = "admin-report-message";
    }

    private void ShowMessage(string message)
    {
        lblReportMessage.Text = message;
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        if (Session["AdminID"] == null)
        {
            Response.Redirect("~/Admin/AdminLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        DataTable reportData =
            Session["AdminReportData"] as DataTable;

        if (reportData == null || reportData.Rows.Count == 0)
        {
            ShowMessage(
                "Please generate a report before exporting.");
            return;
        }

        string[] headers =
        {
            "Grievance ID",
            "Title",
            "Status",
            "Priority",
            "Submission Date",
            "Resolution Date",
            "Student Name",
            "Registration No.",
            "Category",
            "Department"
        };

        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-excel";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.Charset = "";

        Response.AddHeader(
            "Content-Disposition",
            "attachment;filename=Grievance_Detailed_Report.xls");

        Response.Write(
            "<html><head><meta charset='utf-8' /></head><body>");

        Response.Write("<table border='1'>");
        Response.Write("<tr>");

        foreach (string header in headers)
        {
            Response.Write("<th>" +
                Server.HtmlEncode(header) + "</th>");
        }

        Response.Write("</tr>");

        foreach (DataRow row in reportData.Rows)
        {
            Response.Write("<tr>");

            foreach (DataColumn column in reportData.Columns)
            {
                string value = Convert.ToString(row[column]);

                if (column.ColumnName == "grievanceSubmissionDate" ||
                    column.ColumnName == "grievanceResolutionDate")
                {
                    if (row[column] != DBNull.Value)
                    {
                        value = Convert.ToDateTime(row[column])
                            .ToString("dd MMM yyyy hh:mm tt");
                    }
                    else
                    {
                        value = "";
                    }
                }

                Response.Write("<td>" +
                    Server.HtmlEncode(value) + "</td>");
            }

            Response.Write("</tr>");
        }

        Response.Write("</table></body></html>");
        Response.Flush();

        Context.ApplicationInstance.CompleteRequest();
    }
}
