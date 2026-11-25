using System;
using System.Data;
using System.Web.UI;

public partial class Staff_MyEscalatedRequests : System.Web.UI.Page
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
            BindEscalatedRequests();
        }
    }

    private void BindEscalatedRequests()
    {
        int staffID = Convert.ToInt32(Session["StaffID"]);

        DataTable dt = DALEscalatedGrievance.GetMyEscalatedRequests(staffID);

        string keyword = (txtSearch.Text ?? string.Empty).Trim();
        string selectedStatus = ddlRequestStatus.SelectedValue;

        DataView view = new DataView(dt);
        string rowFilter = string.Empty;

        // Search filter
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            string safeKeyword = keyword
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("*", "[*]");

            rowFilter =
                "Convert(escalationID, 'System.String') LIKE '%" + safeKeyword + "%'" +
                " OR Convert(grievanceID, 'System.String') LIKE '%" + safeKeyword + "%'" +
                " OR grievanceTitle LIKE '%" + safeKeyword + "%'" +
                " OR categoryName LIKE '%" + safeKeyword + "%'" +
                " OR escalationReason LIKE '%" + safeKeyword + "%'";
        }

        // Request status filter
        if (!string.IsNullOrWhiteSpace(selectedStatus))
        {
            string safeStatus = selectedStatus.Replace("'", "''");

            if (!string.IsNullOrWhiteSpace(rowFilter))
            {
                rowFilter = "(" + rowFilter + ")";
                rowFilter += " AND requestStatus = '" + safeStatus + "'";
            }
            else
            {
                rowFilter = "requestStatus = '" + safeStatus + "'";
            }
        }

        view.RowFilter = rowFilter;
        view.Sort = "requestedDate DESC, escalationID DESC";

        gvEscalatedRequests.DataSource = view;
        gvEscalatedRequests.DataBind();

        lblRecordCount.Text = view.Count == 1
            ? "1 record"
            : view.Count + " records";
    }
    protected string GetEscalationStatusClass(string status)
    {
        switch ((status ?? string.Empty).Trim().ToLowerInvariant())
        {
            case "pending hod review":
            case "pending admin review":
                return "escalation-pending";

            case "rejected by hod":
                return "escalation-rejected";

            case "action required":
                return "escalation-action";

            case "action taken":
                return "escalation-taken";

            case "closed":
                return "escalation-closed";

            default:
                return string.Empty;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindEscalatedRequests();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        ddlRequestStatus.SelectedIndex = 0;

        BindEscalatedRequests();
    }
}