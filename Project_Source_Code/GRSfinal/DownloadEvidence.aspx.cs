
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

public partial class Shared_DownloadEvidence : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int evidenceID;

        if (!int.TryParse(Request.QueryString["id"], out evidenceID)
            || evidenceID <= 0)
        {
            DenyAccess(400);
            return;
        }

        // Require exactly one recognized authenticated role.
        int roleCount = 0;
        string role = "";

        int adminID = 0;
        int stdID = 0;
        int deptHeadID = 0;
        int deptID = 0;
        int staffID = 0;

        bool isAdmin =
            int.TryParse(Convert.ToString(Session["AdminID"]), out adminID)
            && adminID > 0;

        bool isStudent =
            int.TryParse(Convert.ToString(Session["StdID"]), out stdID)
            && stdID > 0;

        bool isHOD =
            int.TryParse(Convert.ToString(Session["DeptHeadID"]), out deptHeadID)
            && deptHeadID > 0
            && int.TryParse(Convert.ToString(Session["DeptID"]), out deptID)
            && deptID > 0;
        bool isStaff =
    int.TryParse(Convert.ToString(Session["StaffID"]), out staffID)
    && staffID > 0;

        if (isAdmin) { roleCount++; role = "Admin"; }
        if (isStudent) { roleCount++; role = "Student"; }
        if (isHOD) { roleCount++; role = "HOD"; }
        if (isStaff) { roleCount++; role = "Staff"; }

        if (roleCount != 1)
        {
            DenyAccess(403);
            return;
        }

        string evidenceName = null;
        string storedPath = null;
        string uploadedBy = null;
        int evidenceGrievanceID = 0;

        try
        {
            string cs = ConfigurationManager
                .ConnectionStrings["GRS"].ConnectionString;

            string query = @"
           SELECT
               e.evidenceName,
    e.evidenceFilepath,
    e.evidenceUploadedby,
    e.grievanceID
                FROM Grievanceevidence e
                INNER JOIN Grievance g
                    ON g.grievanceID = e.grievanceID
                WHERE e.evidenceID = @evidenceID";

            if (role == "Student")
                query += " AND g.stdID = @stdID";

            if (role == "HOD")
                query += " AND g.deptID = @deptID";

            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@evidenceID", SqlDbType.Int).Value =
                    evidenceID;

                if (role == "Student")
                    cmd.Parameters.Add("@stdID", SqlDbType.Int).Value = stdID;

                if (role == "HOD")
                    cmd.Parameters.Add("@deptID", SqlDbType.Int).Value = deptID;

                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        evidenceName = Convert.ToString(dr["evidenceName"]);
                        storedPath = Convert.ToString(dr["evidenceFilepath"]);
                        uploadedBy = Convert.ToString(dr["evidenceUploadedby"]);
                        evidenceGrievanceID = Convert.ToInt32(dr["grievanceID"]);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());

            DenyAccess(500);
            return;
        }
        if (role == "Staff")
        {
            string expectedUploader = "Staff:" + staffID;

            if (!string.Equals(
                uploadedBy,
                expectedUploader,
                StringComparison.Ordinal))
            {
                DenyAccess(403);
                return;
            }

            DataTable assignedGrievance =
                DALGrievance.GetStaffGrievanceDetailsByID(
                    evidenceGrievanceID,
                    staffID);

            if (assignedGrievance == null ||
                assignedGrievance.Rows.Count == 0)
            {
                DenyAccess(403);
                return;
            }
        }
        if (string.IsNullOrWhiteSpace(storedPath))
        {
            DenyAccess(404);
            return;
        }

        string fullPath;

        try
        {
            string relativePath = storedPath
                .Replace('/', '\\')
                .TrimStart('\\');

            if (!relativePath.StartsWith(
                "UploadedFiles\\", StringComparison.OrdinalIgnoreCase))
            {
                DenyAccess(403);
                return;
            }

            string uploadRoot = Path.GetFullPath(
                Server.MapPath("~/UploadedFiles/"));

            if (!uploadRoot.EndsWith("\\"))
                uploadRoot += "\\";

            fullPath = Path.GetFullPath(
                Path.Combine(Server.MapPath("~/"), relativePath));

            if (!fullPath.StartsWith(
                    uploadRoot, StringComparison.OrdinalIgnoreCase))
            {
                DenyAccess(403);
                return;
            }

            if (!File.Exists(fullPath))
            {
                DenyAccess(404);
                return;
            }
        }
        catch
        {
            DenyAccess(404);
            return;
        }

        string extension = Path.GetExtension(fullPath).ToLowerInvariant();
        string contentType;

        switch (extension)
        {
            case ".pdf":
                contentType = "application/pdf";
                break;

            case ".doc":
                contentType = "application/msword";
                break;

            case ".docx":
                contentType =
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                break;

            case ".jpg":
            case ".jpeg":
                contentType = "image/jpeg";
                break;

            case ".png":
                contentType = "image/png";
                break;

            default:
                DenyAccess(403);
                return;
        }

        string safeName = Path.GetFileName(evidenceName);

        safeName = safeName
            .Replace("\"", "")
            .Replace("\r", "")
            .Replace("\n", "");

        if (string.IsNullOrWhiteSpace(safeName))
            safeName = "Evidence" + extension;

        Response.Clear();
        Response.ContentType = contentType;
        Response.AddHeader("X-Content-Type-Options", "nosniff");

        string mode = Convert.ToString(Request.QueryString["mode"]);
        mode = mode.Trim().ToLowerInvariant();

        if (mode != "open" && mode != "download")
        {
            DenyAccess(400);
            return;
        }

        bool canOpenInBrowser =
            extension == ".pdf" ||
            extension == ".jpg" ||
            extension == ".jpeg" ||
            extension == ".png";

        string disposition =
            mode == "open" && canOpenInBrowser
                ? "inline"
                : "attachment";

        Response.Clear();
        Response.ContentType = contentType;
        Response.AddHeader("X-Content-Type-Options", "nosniff");
        Response.AddHeader(
            "Content-Disposition",
            disposition + "; filename=\"" + safeName + "\""
        );

        Response.TransmitFile(fullPath);
        Context.ApplicationInstance.CompleteRequest();
    }

    private void DenyAccess(int statusCode)
    {
        Response.Clear();
        Response.StatusCode = statusCode;
        Response.TrySkipIisCustomErrors = true;
        Response.SuppressContent = true;
        Context.ApplicationInstance.CompleteRequest();
    }
}
