using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Collections.Generic;

public partial class Student_SubmitGrievance : System.Web.UI.Page
{
    private string ConnStr
    {
        get { return ConfigurationManager.ConnectionStrings["GRS"].ConnectionString; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        int stdID;

        if (Session["StdID"] == null ||
            !int.TryParse(Session["StdID"].ToString(), out stdID) ||
            stdID <= 0)
        {
            Session.Remove("StdID");
            Response.Redirect("~/Student/StudentLogin.aspx");
            return;
        }

        if (!IsPostBack)
        {
            LoadStudentInfo();
            LoadCategories();
            LoadDepartments(0, stdID);
            LoadGrievanceTypes(0);
        }
    }

    private void LoadStudentInfo()
    {
        int stdID = Convert.ToInt32(Session["StdID"]);

        using (SqlConnection con = new SqlConnection(ConnStr))
        {
            string query = @"SELECT si.stdFirstName, si.stdLastName, si.stdRegNo, si.stdProgram,
                                     si.stdSemester, si.stdSession, si.stdEmail, si.stdPhoneNo,
                                     d.deptName
                              FROM StudentInfo si
                              LEFT JOIN Department d ON si.deptID = d.deptID
                              WHERE si.stdID = @stdID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@stdID", stdID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtFullName.Text = reader["stdFirstName"].ToString() + " " + reader["stdLastName"].ToString();
                    txtRegNo.Text = reader["stdRegNo"].ToString();
                    txtProgram.Text = reader["stdProgram"].ToString();
                    txtStudentDept.Text = reader["deptName"] != DBNull.Value ? reader["deptName"].ToString() : "";


                }
            }
        }
    }

    private void LoadCategories()
    {
        using (SqlConnection con = new SqlConnection(ConnStr))
        {
            string query = "SELECT categoryID, categoryName FROM GrievanceCategory WHERE categoryStatus = 'Active'";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "categoryName";
                ddlCategory.DataValueField = "categoryID";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Category --", "0"));
            }
        }
    }

    private void LoadDepartments(int categoryID, int stdID)
    {
        ddlDepartment.Items.Clear();
        ddlDepartment.Items.Add(new System.Web.UI.WebControls.ListItem(
            categoryID > 0 ? "-- Select Department --" : "-- Select Category First --", "0"));

        if (categoryID <= 0)
        {
            return;
        }

        using (SqlConnection con = new SqlConnection(ConnStr))
        {
            string query = @"
                SELECT d.deptID, d.deptName
                FROM Department d
                INNER JOIN GrievanceCategory c ON c.categoryID = d.categoryID
                WHERE d.deptStatus = 'Active'
                  AND c.categoryStatus = 'Active'
                  AND c.categoryID = @categoryID
                  AND
                  (
                      -- Academia: only the student's registered department.
                      (LOWER(LTRIM(RTRIM(c.categoryName))) = 'academia'
                       AND d.deptID =
                       (
                           SELECT si.deptID
                           FROM StudentInfo si
                           WHERE si.stdID = @stdID
                       ))
                      OR
                      -- Accounts: only the Accounts department.
                      (LOWER(LTRIM(RTRIM(c.categoryName))) = 'accounts'
                       AND LOWER(LTRIM(RTRIM(d.deptName))) = 'accounts')
                      OR
                      -- Exam/Degree: only the Exam & Degree department.
                      (LOWER(LTRIM(RTRIM(c.categoryName))) IN ('exam/degree', 'exam & degree', 'exam and degree')
                       AND LOWER(LTRIM(RTRIM(d.deptName))) IN ('exam/degree', 'exam & degree', 'exam and degree'))
                  )
                ORDER BY d.deptName";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@categoryID", SqlDbType.Int).Value = categoryID;
                cmd.Parameters.Add("@stdID", SqlDbType.Int).Value = stdID;
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ddlDepartment.Items.Add(new System.Web.UI.WebControls.ListItem(
                            reader["deptName"].ToString(), reader["deptID"].ToString()));
                    }
                }
            }
        }

        // Academia should have only the student's registered department available.
        // With a single option, the student cannot choose another department.
        if (ddlDepartment.Items.Count == 2)
        {
            ddlDepartment.SelectedIndex = 1;
        }
    }

    private void LoadGrievanceTypes(int categoryID)
    {
        ddlGrievanceType.Items.Clear();
        ddlGrievanceType.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Grievance Type --", "0"));

        if (categoryID <= 0)
        {
            return;
        }

        using (SqlConnection con = new SqlConnection(ConnStr))
        {
            string query = "SELECT gvTypeID, gvTypeName FROM GrievanceType WHERE categoryID = @categoryID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@categoryID", categoryID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    ddlGrievanceType.Items.Add(new System.Web.UI.WebControls.ListItem(
                        row["gvTypeName"].ToString(), row["gvTypeID"].ToString()));
                }
            }
        }
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        int categoryID;
        if (!int.TryParse(ddlCategory.SelectedValue, out categoryID))
        {
            categoryID = 0;
        }

        int stdID = Convert.ToInt32(Session["StdID"]);
        LoadDepartments(categoryID, stdID);
        LoadGrievanceTypes(categoryID);
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;

        if (!chkDeclaration.Checked)
        {
            ShowMessage("Please check the declaration box before submitting.", false);
            return;
        }


        int stdID;
        int stdLoginID;

        if (Session["StdID"] == null ||
            !int.TryParse(Session["StdID"].ToString(), out stdID) ||
            stdID <= 0 ||
            Session["StdLoginID"] == null ||
            !int.TryParse(Session["StdLoginID"].ToString(), out stdLoginID) ||
            stdLoginID <= 0)
        {
            Session.Clear();
            Response.Redirect("~/Student/StudentLogin.aspx");
            return;
        }


        int categoryID, deptID, gvTypeID;

        if (!int.TryParse(ddlCategory.SelectedValue, out categoryID) || categoryID <= 0 ||
            !int.TryParse(ddlDepartment.SelectedValue, out deptID) || deptID <= 0 ||
            !int.TryParse(ddlGrievanceType.SelectedValue, out gvTypeID) || gvTypeID <= 0)
        {
            ShowMessage(
                "Please select a valid category, department, and grievance type.",
                false);
            return;
        }

        if (!IsValidGrievanceRouting(stdID, categoryID, deptID, gvTypeID))
        {
            ShowMessage(
                "The selected department or grievance type does not match the selected category. Please review your selections.",
                false);
            return;
        }

        string title = txtTitle.Text.Trim();
        string description = txtDescription.Text.Trim();

        string[] allowedExtensions =
    {
        ".doc", ".docx", ".pdf", ".png", ".jpg"
    };

        const int maxFileSizeBytes = 2 * 1024 * 1024;
        const int maxFileCount = 2;

        // Validate all selected files before inserting the grievance.
        if (fileUpload.HasFiles)
        {
            if (fileUpload.PostedFiles.Count > maxFileCount)
            {
                ShowMessage(
                    "You can upload a maximum of 2 supporting documents.",
                    false);
                return;
            }

            foreach (HttpPostedFile postedFile in fileUpload.PostedFiles)
            {
                if (postedFile.ContentLength <= 0)
                    continue;

                if (postedFile.ContentLength > maxFileSizeBytes)
                {
                    ShowMessage(
                        "Each supporting document must be 2 MB or smaller.",
                        false);
                    return;
                }

                string fileName = Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(fileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ShowMessage(
                        "Only DOC, DOCX, PDF, PNG, and JPG files are allowed.",
                        false);
                    return;
                }
            }
        }

        int newGrievanceID = 0;
        string uploadFolder = null;
        List<string> savedFilePaths = new List<string>();

        try
        {
            using (SqlConnection con = new SqlConnection(ConnStr))
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                    INSERT INTO Grievance
                    (
                        stdID, categoryID, deptID, gvTypeID,
                        grievanceTitle, grievanceDescription,
                        grievancePriority, grievanceStatus,
                        grievanceSubmissionDate
                    )
                    VALUES
                    (
                        @stdID, @categoryID, @deptID, @gvTypeID,
                        @Title, @Description, 'Medium', 'Pending', GETDATE()
                    );

                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@stdID", stdID);
                            cmd.Parameters.AddWithValue("@categoryID", categoryID);
                            cmd.Parameters.AddWithValue("@deptID", deptID);
                            cmd.Parameters.AddWithValue("@gvTypeID", gvTypeID);
                            cmd.Parameters.AddWithValue("@Title", title);
                            cmd.Parameters.AddWithValue("@Description", description);

                            newGrievanceID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        if (fileUpload.HasFiles)
                        {
                            uploadFolder = Server.MapPath(
                                "~/UploadedFiles/Grievance/" + newGrievanceID + "/");

                            Directory.CreateDirectory(uploadFolder);

                            foreach (HttpPostedFile postedFile in fileUpload.PostedFiles)
                            {
                                if (postedFile.ContentLength <= 0)
                                    continue;

                                string originalName = Path.GetFileName(postedFile.FileName);
                                string extension =
                                    Path.GetExtension(originalName).ToLowerInvariant();

                                // Preserve the original name where possible.
                                // Use a unique name if another selected file has the same name.
                                string storedFileName = originalName;
                                string savePath = Path.Combine(uploadFolder, storedFileName);

                                if (File.Exists(savePath))
                                {
                                    string nameWithoutExtension =
                                        Path.GetFileNameWithoutExtension(originalName);

                                    storedFileName = nameWithoutExtension + "_" +
                                        Guid.NewGuid().ToString("N") + extension;

                                    savePath = Path.Combine(uploadFolder, storedFileName);
                                }

                                postedFile.SaveAs(savePath);
                                savedFilePaths.Add(savePath);

                                string relativePath =
                                    "UploadedFiles/Grievance/" + newGrievanceID +
                                    "/" + storedFileName;

                                SaveEvidenceRecord(
                                    con,
                                    transaction,
                                    newGrievanceID,
                                    storedFileName,
                                    relativePath,
                                    txtFullName.Text.Trim());
                            }
                        }


                        string trackingQuery = @"
    INSERT INTO dbo.TrackGrievance
    (
        grievanceID,
        escalationID,
        loginID,
        trackingactivityDescription,
        trackingactivitydatetime
    )
    VALUES
    (
        @grievanceID,
        NULL,
        @loginID,
        @description,
        GETDATE()
    );";

                        using (SqlCommand trackingCmd =
                               new SqlCommand(trackingQuery, con, transaction))
                        {
                            trackingCmd.Parameters.Add(
                                "@grievanceID", SqlDbType.Int).Value = newGrievanceID;

                            trackingCmd.Parameters.Add(
                                "@loginID", SqlDbType.Int).Value = stdLoginID;

                            trackingCmd.Parameters.Add(
                                "@description", SqlDbType.NVarChar, 500).Value =
                                "Grievance Submitted";

                            trackingCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                    }
                    catch (Exception)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception)
                        {
                            // Rollback may fail if the transaction has already ended.
                        }

                        // Remove files saved during this submission.
                        foreach (string savedPath in savedFilePaths)
                        {
                            try
                            {
                                if (File.Exists(savedPath))
                                    File.Delete(savedPath);
                            }
                            catch (Exception)
                            {
                                // Cleanup failure should not hide the original save failure.
                            }
                        }

                        // Remove the submission folder only if it is empty.
                        if (!string.IsNullOrEmpty(uploadFolder))
                        {
                            try
                            {
                                if (Directory.Exists(uploadFolder) &&
                                    Directory.GetFiles(uploadFolder).Length == 0 &&
                                    Directory.GetDirectories(uploadFolder).Length == 0)
                                {
                                    Directory.Delete(uploadFolder);
                                }
                            }
                            catch (Exception)
                            {
                                // Leave the folder if cleanup is not possible.
                            }
                        }

                        ShowMessage(
                            "We couldn't submit your grievance at this time. Please try again later.",
                            false);
                        return;
                    }
                }
            }
        }
        catch (Exception)
        {
            // Handles connection-opening and transaction-start failures.
            ShowMessage(
                "We couldn't connect to the database or start the submission process. Please try again later.",
                false);
            return;
        }
        // ============================================================
        // POST-SUBMISSION NOTIFICATIONS
        // The database transaction has already been committed.
        // Email failures must NOT affect the submitted Grievance.
        // ============================================================

        int hodID = 0;
        string hodEmail = "";
        string hodName = "";
        string studentEmail = "";
        string studentName = "";
        string grievanceTitle = title;

        // ------------------------------------------------------------
        // Get Student Email and Name
        // ------------------------------------------------------------
        try
        {
            using (SqlConnection con = new SqlConnection(ConnStr))
            {
                string query = @"
                    SELECT
                        LTRIM(RTRIM(
                            ISNULL(stdFirstName, '') + ' ' +
                            ISNULL(stdLastName, '')
                        )) AS StudentName,
                        stdEmail
                    FROM StudentInfo
                    WHERE stdID = @stdID;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@stdID", SqlDbType.Int).Value = stdID;

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            studentName = reader["StudentName"] != DBNull.Value
                                ? reader["StudentName"].ToString()
                                : "";

                            studentEmail = reader["stdEmail"] != DBNull.Value
                                ? reader["stdEmail"].ToString()
                                : "";
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError(
                "Failed to retrieve Student email for Grievance ID " +
                newGrievanceID + ": " + ex);
        }

        // ------------------------------------------------------------
        // Get Active HOD ID and Email
        // HOD name is intentionally not queried here because the
        // current submitted file confirms deptHeadEmail but does not
        // establish the exact HOD name-column schema.
        // ------------------------------------------------------------
        try
        {
            using (SqlConnection con = new SqlConnection(ConnStr))
            {
                string query = @"
                    SELECT TOP 1
                        deptHeadID,
                        deptHeadEmail
                    FROM DepartmentHead
                    WHERE deptID = @deptID
                      AND deptHeadStatus = 'Active'
                    ORDER BY deptHeadID;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@deptID", SqlDbType.Int).Value = deptID;

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hodID = reader["deptHeadID"] != DBNull.Value
                                ? Convert.ToInt32(reader["deptHeadID"])
                                : 0;

                            hodEmail = reader["deptHeadEmail"] != DBNull.Value
                                ? reader["deptHeadEmail"].ToString()
                                : "";
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError(
                "Failed to retrieve HOD email for Grievance ID " +
                newGrievanceID + ": " + ex);
        }

        // ============================================================
        // 1. EXISTING IN-APP HOD NOTIFICATION
        // ============================================================
        try
        {
            if (hodID > 0)
            {
                DALSystemNotification.InsertNotification(
                    "HOD",
                    hodID,
                    newGrievanceID,
                    "New Grievance",
                    "A new Grievance (ID: " + newGrievanceID +
                    ") has been submitted to your department.");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError(
                "Failed to create HOD notification for Grievance ID " +
                newGrievanceID + ": " + ex);
        }

        // ============================================================
        // 2. EXISTING IN-APP STUDENT NOTIFICATION
        // ============================================================
        try
        {
            DALSystemNotification.InsertNotification(
                "Student",
                stdID,
                newGrievanceID,
                "Grievance Submitted",
                "Your Grievance (ID: " + newGrievanceID +
                ") has been submitted successfully.");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError(
                "Failed to create Student notification for Grievance ID " +
                newGrievanceID + ": " + ex);
        }

        // ============================================================
        // 3. EMAIL STUDENT
        // ============================================================
        try
        {
            if (!string.IsNullOrWhiteSpace(studentEmail))
            {
                string studentSubject =
                    "Grievance Submitted Successfully - GRV-" +
                    newGrievanceID;

                string studentBody = @"
<html>
<body style='font-family: Arial, sans-serif; color: #172033;'>
    <h2 style='color: #123a7a;'>Grievance Submitted Successfully</h2>

    <p>Dear " + HttpUtility.HtmlEncode(studentName) + @",</p>

    <p>
        Your Grievance has been submitted successfully through the
        Enhanced USP Online Grievance Redressal System.
    </p>

    <table cellpadding='6' cellspacing='0' border='0'>
        <tr>
            <td><strong>Grievance ID:</strong></td>
            <td>GRV-" + newGrievanceID + @"</td>
        </tr>
        <tr>
            <td><strong>Title:</strong></td>
            <td>" + HttpUtility.HtmlEncode(grievanceTitle) + @"</td>
        </tr>
        <tr>
            <td><strong>Status:</strong></td>
            <td>Pending</td>
        </tr>
    </table>

    <p>
        You can log in to your student portal to track the progress
        of your Grievance.
    </p>

    <p>
        Regards,<br />
        <strong>USP Online Grievance Redressal System</strong><br />
        University of Southern Punjab, Multan
    </p>
</body>
</html>";

                EmailService.SendEmail(
                    studentEmail,
                    studentSubject,
                    studentBody);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError(
                "Failed to send Student submission email for Grievance ID " +
                newGrievanceID + ": " + ex);
        }

        // ============================================================
        // 4. EMAIL HOD
        // ============================================================
        try
        {
            if (!string.IsNullOrWhiteSpace(hodEmail))
            {
                string hodSubject =
                    "New Grievance Submitted - GRV-" +
                    newGrievanceID;

                string hodBody = @"
<html>
<body style='font-family: Arial, sans-serif; color: #172033;'>
    <h2 style='color: #123a7a;'>New Grievance Submitted</h2>

    <p>Dear Department Head,</p>

    <p>
        A new Grievance has been submitted to your department
        through the Enhanced USP Online Grievance Redressal System.
    </p>

    <table cellpadding='6' cellspacing='0' border='0'>
        <tr>
            <td><strong>Grievance ID:</strong></td>
            <td>GRV-" + newGrievanceID + @"</td>
        </tr>
        <tr>
            <td><strong>Title:</strong></td>
            <td>" + HttpUtility.HtmlEncode(grievanceTitle) + @"</td>
        </tr>
        <tr>
            <td><strong>Status:</strong></td>
            <td>Pending</td>
        </tr>
        <tr>
            <td><strong>Student:</strong></td>
            <td>" + HttpUtility.HtmlEncode(studentName) + @"</td>
        </tr>
    </table>

    <p>
        Please log in to the Department Head portal to review
        and process this Grievance.
    </p>

    <p>
        Regards,<br />
        <strong>USP Online Grievance Redressal System</strong><br />
        University of Southern Punjab, Multan
    </p>
</body>
</html>";

                EmailService.SendEmail(
                    hodEmail,
                    hodSubject,
                    hodBody);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError(
                "Failed to send HOD submission email for Grievance ID " +
                newGrievanceID + ": " + ex);
        }

        // ============================================================
        // REDIRECT TO SUCCESS PAGE
        // ============================================================
        Response.Redirect(
            "~/Student/GrievanceSubmitted.aspx?ref=" + newGrievanceID);
    }


    private bool IsValidGrievanceRouting(int stdID, int categoryID, int deptID, int gvTypeID)
    {
        using (SqlConnection con = new SqlConnection(ConnStr))
        {
            string query = @"
                SELECT COUNT(1)
                FROM GrievanceCategory c
                INNER JOIN Department d ON d.deptID = @deptID
                INNER JOIN GrievanceType gt ON gt.gvTypeID = @gvTypeID
                                             AND gt.categoryID = c.categoryID
                INNER JOIN StudentInfo si ON si.stdID = @stdID
                WHERE c.categoryID = @categoryID
                  AND c.categoryStatus = 'Active'
                  AND d.deptStatus = 'Active'
                  AND d.categoryID = c.categoryID
                  AND
                  (
                      (LOWER(LTRIM(RTRIM(c.categoryName))) = 'academia'
                       AND si.deptID = d.deptID)
                      OR
                      (LOWER(LTRIM(RTRIM(c.categoryName))) = 'accounts'
                       AND LOWER(LTRIM(RTRIM(d.deptName))) = 'accounts')
                      OR
                      (LOWER(LTRIM(RTRIM(c.categoryName))) IN ('exam/degree', 'exam & degree', 'exam and degree')
                       AND LOWER(LTRIM(RTRIM(d.deptName))) IN ('exam/degree', 'exam & degree', 'exam and degree'))
                  )";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@deptID", SqlDbType.Int).Value = deptID;
                cmd.Parameters.Add("@gvTypeID", SqlDbType.Int).Value = gvTypeID;
                cmd.Parameters.Add("@stdID", SqlDbType.Int).Value = stdID;
                cmd.Parameters.Add("@categoryID", SqlDbType.Int).Value = categoryID;
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            }
        }
    }


    private void SaveEvidenceRecord(
        SqlConnection con,
        SqlTransaction transaction,
        int grievanceID,
        string fileName,
        string relativePath,
        string uploadedBy)
    {
        string query = @"
        INSERT INTO Grievanceevidence
        (
            evidenceName,
            evidenceDescription,
            evidenceDatetime,
            evidenceUploadedby,
            evidenceFilepath,
            grievanceID
        )
        VALUES
        (
            @evidenceName,
            @evidenceDescription,
            @evidenceDatetime,
            @evidenceUploadedby,
            @evidenceFilepath,
            @grievanceID
        )";

        using (SqlCommand cmd = new SqlCommand(query, con, transaction))
        {
            cmd.Parameters.AddWithValue("@evidenceName", fileName);
            cmd.Parameters.AddWithValue(
                "@evidenceDescription", "Supporting document");
            cmd.Parameters.AddWithValue("@evidenceDatetime", DateTime.Now);
            cmd.Parameters.AddWithValue("@evidenceUploadedby", uploadedBy);
            cmd.Parameters.AddWithValue("@evidenceFilepath", relativePath);
            cmd.Parameters.AddWithValue("@grievanceID", grievanceID);

            cmd.ExecuteNonQuery();
        }
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearForm();
        pnlAlert.Visible = false;
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Student/StudentDashboard.aspx");
    }

    private void ClearForm()
    {
        txtTitle.Text = "";
        txtDescription.Text = "";
        ddlCategory.SelectedValue = "0";
        LoadDepartments(0, Convert.ToInt32(Session["StdID"]));
        LoadGrievanceTypes(0);
        chkDeclaration.Checked = false;
    }

    private void ShowMessage(string message, bool isSuccess)
    {
        pnlAlert.Visible = true;
        pnlAlert.CssClass = isSuccess ? "alert-box alert-success-box" : "alert-box alert-error-box";
        lblMessage.Text = message;
    }

}
