
using System;
using System.Data;
using System.Web.UI;

public partial class Staff_StaffAssignedGrievances : System.Web.UI.Page
{
    private DataTable GetAssignedGrievances()
    {
        int staffID = Convert.ToInt32(Session["StaffID"]);

        return DALGrievance.GetStaffAssignedGrievances(staffID);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["StaffID"] == null)
        {
            Response.Redirect("~/Staff/StaffLogin.aspx");
            return;
        }

        if (!IsPostBack)
        {
            BindAssignedGrievances();
        }
    }

    private void BindAssignedGrievances()
    {
        DataTable dt = GetAssignedGrievances();

        string keyword = (txtSearch.Text ?? "").Trim();
        string selectedStatus = ddlStatus.SelectedValue;

        DataView view = new DataView(dt);
        string rowFilter = "";

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            string safeKeyword = keyword
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("*", "[*]");

            rowFilter =
                "Convert(grievanceID, 'System.String') LIKE '%" + safeKeyword + "%'" +
                " OR grievanceTitle LIKE '%" + safeKeyword + "%'" +
                " OR categoryName LIKE '%" + safeKeyword + "%'" +
                " OR grievancePriority LIKE '%" + safeKeyword + "%'";
        }

        if (!string.IsNullOrEmpty(selectedStatus))
        {
            string safeStatus = selectedStatus.Replace("'", "''");

            if (!string.IsNullOrEmpty(rowFilter))
            {
                rowFilter = "(" + rowFilter + ")";
                rowFilter += " AND grievanceStatus = '" + safeStatus + "'";
            }
            else
            {
                rowFilter = "grievanceStatus = '" + safeStatus + "'";
            }
        }

        view.RowFilter = rowFilter;
        view.Sort = "assignedDate DESC, assignmentID DESC";

        gvAssignedGrievances.DataSource = view;
        gvAssignedGrievances.DataBind();

        lblRecordCount.Text = view.Count == 1
            ? "1 record"
            : view.Count + " records";
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindAssignedGrievances();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlStatus.SelectedIndex = 0;

        BindAssignedGrievances();
    }
}
