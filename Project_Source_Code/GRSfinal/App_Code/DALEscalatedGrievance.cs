using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for DALEscalatedGrievance
/// </summary>
public class DALEscalatedGrievance
{
    public static int RegisterEscalation(MEscalatedGrievance Escalation)
    {
        int result = 0;

        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;

        SqlConnection con = new SqlConnection(constring);

        try
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
                @"INSERT INTO EscalatedGrievance
                (
                    grievanceID,
                    escalationReason,
                    escalationDescription,
                    requestedDate,
                    requestStatus,
                    hodRemarks,
                    hodDecisionDate,
                    reviewedByDeptHeadID,
                    adminRemarks,
                    adminDecisionDate,
                    decidedByAdminID
                )
                VALUES
                (
                    @grievanceID,
                    @escalationReason,
                    @escalationDescription,
                    @requestedDate,
                    @requestStatus,
                    @hodRemarks,
                    @hodDecisionDate,
                    @reviewedByDeptHeadID,
                    @adminRemarks,
                    @adminDecisionDate,
                    @decidedByAdminID
                )", con);

            cmd.Parameters.AddWithValue(
                "@grievanceID",
                Escalation.GrievanceID);

            cmd.Parameters.AddWithValue(
                "@escalationReason",
                Escalation.EscalationReason);

            cmd.Parameters.AddWithValue(
                "@escalationDescription",
                (object)Escalation.EscalationDescription ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@requestedDate",
                Escalation.RequestedDate);

            cmd.Parameters.AddWithValue(
                "@requestStatus",
                Escalation.RequestStatus);

            cmd.Parameters.AddWithValue(
                "@hodRemarks",
                (object)Escalation.HodRemarks ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@hodDecisionDate",
                (object)Escalation.HodDecisionDate ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@reviewedByDeptHeadID",
                (object)Escalation.ReviewedByDeptHeadID ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@adminRemarks",
                (object)Escalation.AdminRemarks ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@adminDecisionDate",
                (object)Escalation.AdminDecisionDate ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@decidedByAdminID",
                (object)Escalation.DecidedByAdminID ?? DBNull.Value);

            cmd.ExecuteNonQuery();

            result = 1;
        }
        catch
        {
            result = 0;
        }
        finally
        {
            con.Close();
        }

        return result;
    }
    public static DataTable GetAdminEscalatedGrievances()
    {
        DataTable dt = new DataTable();

        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;

        using (SqlConnection con = new SqlConnection(constring))
        {
            string query = @"
        SELECT
            e.escalationID,
            e.grievanceID,

            s.stdRegNo,

            LTRIM(RTRIM(
                ISNULL(s.stdFirstName, '') + ' ' +
                ISNULL(s.stdLastName, '')
            )) AS StudentName,

            c.categoryName,
            d.deptName,

            e.escalationReason,
            e.requestedDate,
            e.requestStatus

        FROM EscalatedGrievance e

        INNER JOIN Grievance g
            ON e.grievanceID = g.grievanceID

        LEFT JOIN StudentInfo s
            ON g.stdID = s.stdID

        LEFT JOIN GrievanceCategory c
            ON g.categoryID = c.categoryID

       LEFT JOIN Department d
    ON g.deptID = d.deptID

WHERE e.requestStatus IN
(
    'Pending Admin Review',
    'Action Required',
    'Action Taken',
    'Closed'
)

ORDER BY
    e.requestedDate DESC,
    e.escalationID DESC";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }

        return dt;
    }
    public static DataTable GetEscalatedGrievanceByID(int escalationID)
    {
        DataTable dt = new DataTable();

        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;

        using (SqlConnection con = new SqlConnection(constring))
        {
            string query = @"
        SELECT
            e.escalationID,
            e.grievanceID,

            s.stdID,
            s.stdRegNo,

            LTRIM(RTRIM(
                ISNULL(s.stdFirstName, '') + ' ' +
                ISNULL(s.stdLastName, '')
            )) AS StudentName,

            c.categoryID,
            c.categoryName,

            d.deptID,
            d.deptName,

            g.grievanceTitle,
            g.grievanceDescription,
            g.grievancePriority,
            g.grievanceStatus,

            g.grievanceSubmissionDate,
            g.grievanceResolutionDate,

            e.escalationReason,
            e.escalationDescription,
            e.requestedDate,
            e.requestStatus,

            e.hodRemarks,
            e.hodDecisionDate,
            e.reviewedByDeptHeadID,

            e.adminRemarks,
            e.adminDecisionDate,
            e.decidedByAdminID

        FROM EscalatedGrievance e

        INNER JOIN Grievance g
            ON e.grievanceID = g.grievanceID

        LEFT JOIN StudentInfo s
            ON g.stdID = s.stdID

        LEFT JOIN GrievanceCategory c
            ON g.categoryID = c.categoryID

        LEFT JOIN Department d
            ON g.deptID = d.deptID

        WHERE e.escalationID = @escalationID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue(
                    "@escalationID",
                    escalationID);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }

        return dt;
    }
    public static DataTable GetEscalationTimeline(int escalationID)
    {
        DataTable dt = new DataTable();

        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;

        using (SqlConnection con = new SqlConnection(constring))
        {
            string query = @"
        SELECT
            trackID,
            grievanceID,
            escalationID,
            loginID,
            trackingactivityDescription,
            trackingactivitydatetime

        FROM TrackGrievance

        WHERE escalationID = @escalationID

        ORDER BY
            trackingactivitydatetime ASC,
            trackID ASC";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue(
                    "@escalationID",
                    escalationID);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }

        return dt;
    }

    public static DataTable GetLatestHODAdminResponse(int escalationID)
    {
        DataTable dt = new DataTable();

        if (escalationID <= 0)
            return dt;

        ConnectionStringSettings settings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (settings == null ||
            string.IsNullOrWhiteSpace(settings.ConnectionString))
        {
            throw new Exception("Database Connection Error");
        }

        string query = @"
        SELECT TOP 1
            trackID,
            trackingactivityDescription,
            trackingactivitydatetime
        FROM TrackGrievance
        WHERE escalationID = @escalationID
          AND trackingactivityDescription LIKE
              N'HOD response to Admin%'
        ORDER BY trackID DESC;";

        using (SqlConnection con =
            new SqlConnection(settings.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@escalationID", SqlDbType.Int)
                .Value = escalationID;

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }

        return dt;
    }

    public static int UpdateAdminEscalation(
        int escalationID,
        string newStatus,
        string adminRemarks,
        int adminID,
        string activityDescription)
    {
        int result = 0;

        if (escalationID <= 0 || adminID <= 0)
            return result;

        if (string.IsNullOrWhiteSpace(activityDescription))
            return result;

        if (string.IsNullOrWhiteSpace(adminRemarks))
            return result;

        if (newStatus != "Action Required" &&
            newStatus != "Action Taken" &&
            newStatus != "Closed")
            return result;

        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (consettings == null ||
            string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;

        using (SqlConnection con = new SqlConnection(constring))
        {
            SqlTransaction transaction = null;

            try
            {
                con.Open();
                transaction = con.BeginTransaction();

                string updateQuery = @"
                    UPDATE EscalatedGrievance
                    SET
                        requestStatus = @requestStatus,
                        adminRemarks = @adminRemarks,
                        adminDecisionDate = GETDATE(),
                        decidedByAdminID = @adminID
                    WHERE escalationID = @escalationID
                      AND
                      (
                          requestStatus = 'Pending Admin Review'
                          OR
                          (
                              requestStatus IN ('Action Required', 'Action Taken')
                              AND
                              (
                                  SELECT TOP 1 h.trackID
                                  FROM TrackGrievance h
                                  WHERE h.escalationID = EscalatedGrievance.escalationID
                                    AND h.trackingactivityDescription LIKE N'HOD response to Admin%'
                                  ORDER BY h.trackID DESC
                              ) >
                              (
                                  SELECT TOP 1 a.trackID
                                  FROM TrackGrievance a
                                  WHERE a.escalationID = EscalatedGrievance.escalationID
                                    AND
                                    (
                                        a.trackingactivityDescription LIKE 'Admin requested additional information for the escalated grievance.%'
                                        OR a.trackingactivityDescription LIKE 'Admin issued directions regarding the escalated grievance.%'
                                        OR a.trackingactivityDescription LIKE 'Admin took administrative action on the escalated grievance.%'
                                        OR a.trackingactivityDescription LIKE 'Admin closed the escalation.%'
                                    )
                                  ORDER BY a.trackID DESC
                              )
                          )
                      )";

                using (SqlCommand cmd =
                    new SqlCommand(updateQuery, con, transaction))
                {
                    cmd.Parameters.AddWithValue("@requestStatus", newStatus);
                    cmd.Parameters.AddWithValue("@adminRemarks", adminRemarks.Trim());
                    cmd.Parameters.AddWithValue("@adminID", adminID);
                    cmd.Parameters.AddWithValue("@escalationID", escalationID);

                    result = cmd.ExecuteNonQuery();
                }

                if (result != 1)
                {
                    transaction.Rollback();
                    return 0;
                }

                string activityQuery = @"
                    INSERT INTO TrackGrievance
                    (
                        grievanceID,
                        escalationID,
                        loginID,
                        trackingactivityDescription,
                        trackingactivitydatetime
                    )
                    SELECT
                        grievanceID,
                        @escalationID,
                        @adminID,
                        @activityDescription,
                        GETDATE()
                    FROM EscalatedGrievance
                    WHERE escalationID = @escalationID";

                using (SqlCommand cmd =
                    new SqlCommand(activityQuery, con, transaction))
                {
                    cmd.Parameters.AddWithValue("@escalationID", escalationID);
                    cmd.Parameters.AddWithValue("@adminID", adminID);
                    cmd.Parameters.AddWithValue(
                        "@activityDescription", activityDescription.Trim());

                    result = cmd.ExecuteNonQuery();
                }

                if (result != 1)
                {
                    transaction.Rollback();
                    return 0;
                }

                transaction.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                if (transaction != null &&
                    transaction.Connection != null)
                {
                    transaction.Rollback();
                }

                throw new Exception(ex.Message);
            }
        }
    }

    // ============================================
    // HOD: Get Escalations for Own Department
    // ============================================
    public static DataTable GetHODEscalations(
        int deptID,
        string searchText,
        string statusFilter)
    {
        DataTable dt = new DataTable();

        if (deptID <= 0)
            return dt;

        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (consettings == null ||
            string.IsNullOrEmpty(consettings.ConnectionString))
        {
            throw new Exception("Database Connection Error");
        }

        string query = @"
            SELECT
                e.escalationID,
                e.grievanceID,
                g.grievanceTitle,

                LTRIM(RTRIM(
                    ISNULL(s.stdFirstName, '') + ' ' +
                    ISNULL(s.stdLastName, '')
                )) AS StudentName,

                s.stdRegNo,
                e.escalationReason,
                e.requestedDate,
                e.requestStatus

            FROM EscalatedGrievance e

            INNER JOIN Grievance g
                ON e.grievanceID = g.grievanceID

            LEFT JOIN StudentInfo s
                ON g.stdID = s.stdID

            WHERE g.deptID = @deptID

              AND (
                    @statusFilter = ''
                    OR e.requestStatus = @statusFilter
              )

              AND (
                    @searchText = ''
                    OR CONVERT(VARCHAR(20), e.escalationID)
                        LIKE '%' + @searchText + '%'
                    OR CONVERT(VARCHAR(20), e.grievanceID)
                        LIKE '%' + @searchText + '%'
                    OR ISNULL(g.grievanceTitle, '')
                        LIKE '%' + @searchText + '%'
                    OR ISNULL(s.stdRegNo, '')
                        LIKE '%' + @searchText + '%'
                    OR ISNULL(s.stdFirstName, '') + ' ' +
                       ISNULL(s.stdLastName, '')
                        LIKE '%' + @searchText + '%'
                    OR ISNULL(e.escalationReason, '')
                        LIKE '%' + @searchText + '%'
              )

            ORDER BY
                e.requestedDate DESC,
                e.escalationID DESC;";

        using (SqlConnection con =
            new SqlConnection(consettings.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@deptID", SqlDbType.Int).Value =
                deptID;

            cmd.Parameters.Add("@statusFilter", SqlDbType.NVarChar, 80).Value =
                string.IsNullOrWhiteSpace(statusFilter)
                    ? string.Empty
                    : statusFilter.Trim();

            cmd.Parameters.Add("@searchText", SqlDbType.NVarChar, 200).Value =
                string.IsNullOrWhiteSpace(searchText)
                    ? string.Empty
                    : searchText.Trim();

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }

        return dt;
    }


    // ============================================
    // HOD: Get One Escalation Within Own Department
    // ============================================
    public static DataTable GetHODEscalationByID(
        int escalationID,
        int deptID)
    {
        DataTable dt = new DataTable();

        if (escalationID <= 0 || deptID <= 0)
            return dt;

        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (consettings == null ||
            string.IsNullOrEmpty(consettings.ConnectionString))
        {
            throw new Exception("Database Connection Error");
        }

        string query = @"
            SELECT
                e.escalationID,
                e.grievanceID,

                s.stdID,
                s.stdRegNo,

                LTRIM(RTRIM(
                    ISNULL(s.stdFirstName, '') + ' ' +
                    ISNULL(s.stdLastName, '')
                )) AS StudentName,

                c.categoryID,
                c.categoryName,

                d.deptID,
                d.deptName,

                g.grievanceTitle,
                g.grievanceDescription,
                g.grievancePriority,
                g.grievanceStatus,
                g.grievanceSubmissionDate,
                g.grievanceResolutionDate,

                e.escalationReason,
                e.escalationDescription,
                e.requestedDate,
                e.requestStatus,

                e.hodRemarks,
                e.hodDecisionDate,
                e.reviewedByDeptHeadID,

                e.adminRemarks,
                e.adminDecisionDate,
                e.decidedByAdminID

            FROM EscalatedGrievance e

            INNER JOIN Grievance g
                ON e.grievanceID = g.grievanceID

            LEFT JOIN StudentInfo s
                ON g.stdID = s.stdID

            LEFT JOIN GrievanceCategory c
                ON g.categoryID = c.categoryID

            LEFT JOIN Department d
                ON g.deptID = d.deptID

            WHERE e.escalationID = @escalationID
              AND g.deptID = @deptID;";

        using (SqlConnection con =
            new SqlConnection(consettings.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@escalationID", SqlDbType.Int).Value =
                escalationID;

            cmd.Parameters.Add("@deptID", SqlDbType.Int).Value =
                deptID;

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }

        return dt;
    }

    // ============================================
    // HOD: Forward or Reject an Escalation
    // ============================================
    public static int UpdateHODEscalationDecision(
        int escalationID,
        int deptID,
        int deptHeadID,
        string newStatus,
        string hodRemarks,
        string activityDescription)
    {
        if (escalationID <= 0 || deptID <= 0 || deptHeadID <= 0)
            return 0;

        if (newStatus != "Pending Admin Review" &&
            newStatus != "Rejected by HOD")
            return 0;

        if (string.IsNullOrWhiteSpace(hodRemarks) ||
            hodRemarks.Trim().Length > 2000)
            return 0;

        if (string.IsNullOrWhiteSpace(activityDescription))
            return 0;

        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (consettings == null ||
            string.IsNullOrEmpty(consettings.ConnectionString))
        {
            throw new Exception("Database Connection Error");
        }

        using (SqlConnection con =
            new SqlConnection(consettings.ConnectionString))
        {
            SqlTransaction transaction = null;

            try
            {
                con.Open();
                transaction = con.BeginTransaction();

                // Verify department ownership and update only a request
                // that is still waiting for HOD review.
                string updateQuery = @"
                    UPDATE e
                    SET
                        e.requestStatus = @newStatus,
                        e.hodRemarks = @hodRemarks,
                        e.hodDecisionDate = GETDATE(),
                        e.reviewedByDeptHeadID = @deptHeadID
                    FROM EscalatedGrievance e
                    INNER JOIN Grievance g
                        ON e.grievanceID = g.grievanceID
                    WHERE e.escalationID = @escalationID
                      AND g.deptID = @deptID
                      AND e.requestStatus = 'Pending HOD Review';";

                int rowsUpdated;

                using (SqlCommand cmd =
                    new SqlCommand(updateQuery, con, transaction))
                {
                    cmd.Parameters.Add("@newStatus", SqlDbType.NVarChar, 80)
                        .Value = newStatus;

                    cmd.Parameters.Add("@hodRemarks", SqlDbType.NVarChar, 2000)
                        .Value = hodRemarks.Trim();

                    cmd.Parameters.Add("@deptHeadID", SqlDbType.Int)
                        .Value = deptHeadID;

                    cmd.Parameters.Add("@escalationID", SqlDbType.Int)
                        .Value = escalationID;

                    cmd.Parameters.Add("@deptID", SqlDbType.Int)
                        .Value = deptID;

                    rowsUpdated = cmd.ExecuteNonQuery();
                }

                // No update means wrong department, invalid request,
                // or another reviewer has already processed it.
                if (rowsUpdated != 1)
                {
                    transaction.Rollback();
                    return 0;
                }

                // Record the decision in the escalation activity history.
                string activityQuery = @"
                    INSERT INTO TrackGrievance
                    (
                        grievanceID,
                        escalationID,
                        loginID,
                        trackingactivityDescription,
                        trackingactivitydatetime
                    )
                    SELECT
                        e.grievanceID,
                        e.escalationID,
                        @deptHeadID,
                        @activityDescription,
                        GETDATE()
                    FROM EscalatedGrievance e
                    INNER JOIN Grievance g
                        ON e.grievanceID = g.grievanceID
                    WHERE e.escalationID = @escalationID
                      AND g.deptID = @deptID;";

                int rowsInserted;

                using (SqlCommand cmd =
                    new SqlCommand(activityQuery, con, transaction))
                {
                    cmd.Parameters.Add("@deptHeadID", SqlDbType.Int)
                        .Value = deptHeadID;

                    cmd.Parameters.Add("@activityDescription", SqlDbType.NVarChar, 500)
                        .Value = activityDescription.Trim();

                    cmd.Parameters.Add("@escalationID", SqlDbType.Int)
                        .Value = escalationID;

                    cmd.Parameters.Add("@deptID", SqlDbType.Int)
                        .Value = deptID;

                    rowsInserted = cmd.ExecuteNonQuery();
                }

                if (rowsInserted != 1)
                {
                    transaction.Rollback();
                    return 0;
                }

                transaction.Commit();
                return 1;
            }
            catch
            {
                if (transaction != null &&
                    transaction.Connection != null)
                {
                    transaction.Rollback();
                }

                throw;
            }
        }
    }

    public static int CreateStaffEscalationRequest(
        int grievanceID,
        int staffID,
        string escalationReason,
        string escalationDescription)
    {
        if (grievanceID <= 0 || staffID <= 0)
            return 0;

        if (string.IsNullOrWhiteSpace(escalationReason) ||
            escalationReason.Trim().Length > 500)
            return 0;

        if (string.IsNullOrWhiteSpace(escalationDescription) ||
            escalationDescription.Trim().Length > 2000)
            return 0;

        ConnectionStringSettings settings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (settings == null ||
            string.IsNullOrWhiteSpace(settings.ConnectionString))
        {
            throw new Exception("Database Connection Error");
        }

        using (SqlConnection con =
            new SqlConnection(settings.ConnectionString))
        {
            con.Open();

            using (SqlTransaction transaction =
                con.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    // Verify that this grievance is currently assigned
                    // to this staff member.
                    string assignmentQuery = @"
                    SELECT COUNT(1)
                    FROM dbo.GrievanceAssignment ga
                    WHERE ga.grievanceID = @grievanceID
                      AND ga.staffID = @staffID
                      AND ga.assignmentStatus = 'Assigned'
                      AND NOT EXISTS
                      (
                          SELECT 1
                          FROM dbo.GrievanceAssignment newer
                          WHERE newer.grievanceID = ga.grievanceID
                            AND
                            (
                                newer.assignedDate > ga.assignedDate
                                OR
                                (
                                    newer.assignedDate = ga.assignedDate
                                    AND newer.assignmentID > ga.assignmentID
                                )
                            )
                      );";

                    using (SqlCommand cmd =
                        new SqlCommand(assignmentQuery, con, transaction))
                    {
                        cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                            .Value = grievanceID;

                        cmd.Parameters.Add("@staffID", SqlDbType.Int)
                            .Value = staffID;

                        if (Convert.ToInt32(cmd.ExecuteScalar()) != 1)
                        {
                            transaction.Rollback();
                            return 0;
                        }
                    }

                    // Prevent another request while one is awaiting HOD review.
                    string duplicateQuery = @"
                    SELECT COUNT(1)
                    FROM dbo.EscalatedGrievance WITH (UPDLOCK, HOLDLOCK)
                    WHERE grievanceID = @grievanceID
                      AND requestStatus = 'Pending HOD Review';";

                    using (SqlCommand cmd =
                        new SqlCommand(duplicateQuery, con, transaction))
                    {
                        cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                            .Value = grievanceID;

                        if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                        {
                            transaction.Rollback();
                            return 2;
                        }
                    }

                    string insertQuery = @"
                    INSERT INTO dbo.EscalatedGrievance
                    (
                        grievanceID,
                        escalationReason,
                        escalationDescription,
                        requestedDate,
                        requestStatus
                    )
                    VALUES
                    (
                        @grievanceID,
                        @escalationReason,
                        @escalationDescription,
                        GETDATE(),
                        'Pending HOD Review'
                    );";

                    using (SqlCommand cmd =
                        new SqlCommand(insertQuery, con, transaction))
                    {
                        cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                            .Value = grievanceID;

                        cmd.Parameters.Add("@escalationReason",
                            SqlDbType.NVarChar, 500)
                            .Value = escalationReason.Trim();

                        cmd.Parameters.Add("@escalationDescription",
                            SqlDbType.NVarChar, -1)
                            .Value = escalationDescription.Trim();

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return 1;
                }
                catch
                {
                    if (transaction.Connection != null)
                        transaction.Rollback();

                    throw;
                }
            }
        }
    }
    // ============================================
    // HOD: Respond to Admin's Additional Information Request
    // ============================================
    public static int RecordHODAdditionalInfoResponse(
        int escalationID,
        int deptID,
        int deptHeadID,
        string response)
    {
        if (escalationID <= 0 || deptID <= 0 || deptHeadID <= 0)
            return 0;

        if (string.IsNullOrWhiteSpace(response) ||
            response.Trim().Length > 400)
            return 0;

        ConnectionStringSettings settings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (settings == null ||
            string.IsNullOrWhiteSpace(settings.ConnectionString))
        {
            throw new Exception("Database Connection Error");
        }

        using (SqlConnection con =
            new SqlConnection(settings.ConnectionString))
        {
            SqlTransaction transaction = null;

            try
            {
                con.Open();
                transaction = con.BeginTransaction();

                // Verify department ownership, Action Required status,
                // and that Admin's request for information exists.
                string validationQuery = @"
                SELECT COUNT(1)
                FROM EscalatedGrievance e
                INNER JOIN Grievance g
                    ON e.grievanceID = g.grievanceID
                WHERE e.escalationID = @escalationID
                  AND g.deptID = @deptID
                  AND e.requestStatus IN ('Action Required', 'Action Taken')
                  AND e.adminRemarks IS NOT NULL
                  AND LTRIM(RTRIM(e.adminRemarks)) <> '';";

                using (SqlCommand cmd =
                    new SqlCommand(validationQuery, con, transaction))
                {
                    cmd.Parameters.Add("@escalationID", SqlDbType.Int)
                        .Value = escalationID;

                    cmd.Parameters.Add("@deptID", SqlDbType.Int)
                        .Value = deptID;

                    if (Convert.ToInt32(cmd.ExecuteScalar()) != 1)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }

                // Store the HOD response in the timeline.
                // Keep the original Admin remarks unchanged.
                string activityDescription =
     "HOD response to Admin's escalation action: "
     + response.Trim();

                string insertQuery = @"
                INSERT INTO TrackGrievance
                (
                    grievanceID,
                    escalationID,
                    loginID,
                    trackingactivityDescription,
                    trackingactivitydatetime
                )
                SELECT
                    e.grievanceID,
                    e.escalationID,
                    @deptHeadID,
                    @activityDescription,
                    GETDATE()
                FROM EscalatedGrievance e
                INNER JOIN Grievance g
                    ON e.grievanceID = g.grievanceID
                WHERE e.escalationID = @escalationID
                  AND g.deptID = @deptID
                AND e.requestStatus IN ('Action Required', 'Action Taken')
                  AND e.adminRemarks IS NOT NULL
                  AND LTRIM(RTRIM(e.adminRemarks)) <> '';";

                int rowsInserted;

                using (SqlCommand cmd =
                    new SqlCommand(insertQuery, con, transaction))
                {
                    cmd.Parameters.Add("@deptHeadID", SqlDbType.Int)
                        .Value = deptHeadID;

                    cmd.Parameters.Add("@activityDescription",
                        SqlDbType.NVarChar, 500)
                        .Value = activityDescription;

                    cmd.Parameters.Add("@escalationID", SqlDbType.Int)
                        .Value = escalationID;

                    cmd.Parameters.Add("@deptID", SqlDbType.Int)
                        .Value = deptID;

                    rowsInserted = cmd.ExecuteNonQuery();
                }

                if (rowsInserted != 1)
                {
                    transaction.Rollback();
                    return 0;
                }

                transaction.Commit();
                return 1;
            }
            catch
            {
                if (transaction != null &&
                    transaction.Connection != null)
                {
                    transaction.Rollback();
                }

                throw;
            }
        }
    }
    public static DataTable GetLatestAdminEscalationActivity(int escalationID)
    {
        DataTable dt = new DataTable();

        if (escalationID <= 0)
            return dt;

        ConnectionStringSettings settings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (settings == null ||
            string.IsNullOrWhiteSpace(settings.ConnectionString))
        {
            throw new Exception("Database Connection Error");
        }

        string query = @"
        SELECT TOP 1
            trackID,
            trackingactivityDescription,
            trackingactivitydatetime
        FROM TrackGrievance
        WHERE escalationID = @escalationID
          AND (
                trackingactivityDescription LIKE
                    'Admin requested additional information for the escalated grievance.%'
             OR trackingactivityDescription LIKE
                    'Admin issued directions regarding the escalated grievance.%'
             OR trackingactivityDescription LIKE
                    'Admin took administrative action on the escalated grievance.%'
             OR trackingactivityDescription LIKE
                    'Admin closed the escalation.%'
          )
        ORDER BY trackID DESC;";

        using (SqlConnection con =
            new SqlConnection(settings.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@escalationID", SqlDbType.Int)
                .Value = escalationID;

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }

        return dt;
    }

    private static DataTable GetTimelineByVisibility(
        int escalationID,
        string visibility)
    {
        DataTable dt = new DataTable();

        if (escalationID <= 0)
            return dt;

        string activityFilter;

        if (visibility == "Admin")
        {
            activityFilter = @"
    (
        trackingactivityDescription LIKE N'Admin %'
        OR trackingactivityDescription LIKE N'HOD response to Admin%'
    )";
        }
        else if (visibility == "HOD")
        {
            activityFilter = "1 = 1";
        }
        else if (visibility == "Staff")
        {
            activityFilter = @"
    (
        trackingactivityDescription LIKE N'HOD instruction to Staff:%'
        OR trackingactivityDescription LIKE N'Staff progress update:%'
    )";
        }
        else
        {
            return dt;
        }

        ConnectionStringSettings settings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (settings == null ||
            string.IsNullOrWhiteSpace(settings.ConnectionString))
            throw new Exception("Database Connection Error");

        string query = @"
        SELECT
            trackID,
            grievanceID,
            escalationID,
            loginID,
            trackingactivityDescription,
            trackingactivitydatetime
        FROM TrackGrievance
        WHERE escalationID = @escalationID
          AND (" + activityFilter + @")
        ORDER BY trackingactivitydatetime ASC, trackID ASC;";

        using (SqlConnection con =
            new SqlConnection(settings.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@escalationID", SqlDbType.Int)
                .Value = escalationID;

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                da.Fill(dt);
        }

        return dt;
    }

    public static DataTable GetAdminEscalationTimeline(int escalationID)
    {
        return GetTimelineByVisibility(escalationID, "Admin");
    }

    public static DataTable GetHODEscalationTimeline(int escalationID)
    {
        return GetTimelineByVisibility(escalationID, "HOD");
    }

    public static DataTable GetStaffEscalationTimeline(int escalationID)
    {
        return GetTimelineByVisibility(escalationID, "Staff");
    }


    // ============================================
    // STAFF: Get latest escalation for assigned grievance
    // ============================================
    public static int GetStaffEscalationIDForGrievance(
        int grievanceID,
        int staffID)
    {
        if (grievanceID <= 0 || staffID <= 0)
            return 0;

        ConnectionStringSettings settings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (settings == null ||
            string.IsNullOrWhiteSpace(settings.ConnectionString))
            throw new Exception("Database Connection Error");

        string query = @"
            SELECT TOP 1 e.escalationID
            FROM dbo.EscalatedGrievance e
            WHERE e.grievanceID = @grievanceID
              AND EXISTS
              (
                  SELECT 1
                  FROM dbo.GrievanceAssignment ga
                  WHERE ga.grievanceID = e.grievanceID
                    AND ga.staffID = @staffID
                    AND ga.assignmentStatus = 'Assigned'
                    AND NOT EXISTS
                    (
                        SELECT 1
                        FROM dbo.GrievanceAssignment newer
                        WHERE newer.grievanceID = ga.grievanceID
                          AND
                          (
                              newer.assignedDate > ga.assignedDate
                              OR
                              (
                                  newer.assignedDate = ga.assignedDate
                                  AND newer.assignmentID > ga.assignmentID
                              )
                          )
                    )
              )
            ORDER BY e.escalationID DESC;";

        using (SqlConnection con =
      new SqlConnection(settings.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value = grievanceID;
            cmd.Parameters.Add("@staffID", SqlDbType.Int).Value = staffID;

            con.Open();

            object value = cmd.ExecuteScalar();

            return value == null || value == DBNull.Value
                ? 0
                : Convert.ToInt32(value);
        }
    }

    // ============================================
    // HOD: Save an instruction for Staff in the timeline
    // Follows the existing Admin/HOD TrackGrievance pattern.
    // ============================================
    public static int AddHODInstructionToStaff(
        int escalationID,
        int deptID,
        int deptHeadID,
        string instruction)
    {
        if (escalationID <= 0 || deptID <= 0 || deptHeadID <= 0)
            return 0;

        if (string.IsNullOrWhiteSpace(instruction) ||
            instruction.Trim().Length > 400)
            return 0;

        ConnectionStringSettings settings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (settings == null ||
            string.IsNullOrWhiteSpace(settings.ConnectionString))
            throw new Exception("Database Connection Error");

        using (SqlConnection con =
            new SqlConnection(settings.ConnectionString))
        {
            SqlTransaction transaction = null;

            try
            {
                con.Open();
                transaction = con.BeginTransaction();

                string query = @"
                    INSERT INTO dbo.TrackGrievance
                    (
                        grievanceID,
                        escalationID,
                        loginID,
                        trackingactivityDescription,
                        trackingactivitydatetime
                    )
                    SELECT
                        e.grievanceID,
                        e.escalationID,
                        @deptHeadID,
                        @activityDescription,
                        GETDATE()
                    FROM dbo.EscalatedGrievance e
                    INNER JOIN dbo.Grievance g
                        ON e.grievanceID = g.grievanceID
                    WHERE e.escalationID = @escalationID
                      AND g.deptID = @deptID
                      AND e.requestStatus IN
                          ('Action Required', 'Action Taken', 'Closed')
                      AND e.adminRemarks IS NOT NULL
                      AND LTRIM(RTRIM(e.adminRemarks)) <> '';";

                using (SqlCommand cmd =
                    new SqlCommand(query, con, transaction))
                {
                    cmd.Parameters.Add("@deptHeadID", SqlDbType.Int).Value = deptHeadID;
                    cmd.Parameters.Add("@activityDescription", SqlDbType.NVarChar, 500)
                        .Value = "HOD instruction to Staff: " + instruction.Trim();
                    cmd.Parameters.Add("@escalationID", SqlDbType.Int).Value = escalationID;
                    cmd.Parameters.Add("@deptID", SqlDbType.Int).Value = deptID;

                    if (cmd.ExecuteNonQuery() != 1)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }

                transaction.Commit();
                return 1;
            }
            catch
            {
                if (transaction != null && transaction.Connection != null)
                    transaction.Rollback();
                throw;
            }
        }
    }

    // ============================================
    // STAFF: Save progress after receiving an HOD instruction
    // ============================================
    public static int AddStaffEscalationProgress(
        int escalationID,
        int staffID,
        string progress)
    {
        if (escalationID <= 0 || staffID <= 0)
            return 0;

        if (string.IsNullOrWhiteSpace(progress) ||
            progress.Trim().Length > 400)
            return 0;

        ConnectionStringSettings settings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (settings == null ||
            string.IsNullOrWhiteSpace(settings.ConnectionString))
            throw new Exception("Database Connection Error");

        using (SqlConnection con =
            new SqlConnection(settings.ConnectionString))
        {
            SqlTransaction transaction = null;

            try
            {
                con.Open();
                transaction = con.BeginTransaction();

                string query = @"
                    INSERT INTO dbo.TrackGrievance
                    (
                        grievanceID,
                        escalationID,
                        loginID,
                        trackingactivityDescription,
                        trackingactivitydatetime
                    )
                    SELECT
                        e.grievanceID,
                        e.escalationID,
                        @staffID,
                        @activityDescription,
                        GETDATE()
                    FROM dbo.EscalatedGrievance e
                    WHERE e.escalationID = @escalationID
                      AND e.requestStatus <> 'Rejected by HOD'
                      AND EXISTS
                      (
                          SELECT 1
                          FROM dbo.GrievanceAssignment ga
                          WHERE ga.grievanceID = e.grievanceID
                            AND ga.staffID = @staffID
                            AND ga.assignmentStatus = 'Assigned'
                            AND NOT EXISTS
                            (
                                SELECT 1
                                FROM dbo.GrievanceAssignment newer
                                WHERE newer.grievanceID = ga.grievanceID
                                  AND
                                  (
                                      newer.assignedDate > ga.assignedDate
                                      OR
                                      (
                                          newer.assignedDate = ga.assignedDate
                                          AND newer.assignmentID > ga.assignmentID
                                      )
                                  )
                            )
                      )
                      AND EXISTS
                      (
                          SELECT 1
                          FROM dbo.TrackGrievance t
                          WHERE t.escalationID = e.escalationID
                            AND t.trackingactivityDescription LIKE
                                N'HOD instruction to Staff:%'
                      );";

                using (SqlCommand cmd =
                    new SqlCommand(query, con, transaction))
                {
                    cmd.Parameters.Add("@staffID", SqlDbType.Int).Value = staffID;
                    cmd.Parameters.Add("@activityDescription", SqlDbType.NVarChar, 500)
                        .Value = "Staff progress update: " + progress.Trim();
                    cmd.Parameters.Add("@escalationID", SqlDbType.Int).Value = escalationID;

                    if (cmd.ExecuteNonQuery() != 1)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }

                transaction.Commit();
                return 1;
            }
            catch
            {
                if (transaction != null && transaction.Connection != null)
                    transaction.Rollback();
                throw;
            }
        }
    }

    public static DataTable GetMyEscalatedRequests(int staffID)
    {
        DataTable dt = new DataTable();

        if (staffID <= 0)
            return dt;

        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        string query = @"
        SELECT
            e.escalationID,
            e.grievanceID,
            g.grievanceTitle,
            g.grievancePriority,
            g.grievanceStatus,
            e.escalationReason,
            e.requestStatus,
            e.requestedDate,
            e.hodRemarks,
            e.hodDecisionDate,
            e.adminRemarks,
            e.adminDecisionDate,
            c.categoryName,
            d.deptName
        FROM dbo.EscalatedGrievance e
        INNER JOIN dbo.Grievance g
            ON g.grievanceID = e.grievanceID
        LEFT JOIN dbo.GrievanceCategory c
            ON c.categoryID = g.categoryID
        LEFT JOIN dbo.Department d
            ON d.deptID = g.deptID
        WHERE EXISTS
        (
            SELECT 1
            FROM dbo.GrievanceAssignment ga
            WHERE ga.grievanceID = e.grievanceID
              AND ga.staffID = @staffID
              AND ga.assignmentStatus = 'Assigned'
              AND NOT EXISTS
              (
                  SELECT 1
                  FROM dbo.GrievanceAssignment newer
                  WHERE newer.grievanceID = ga.grievanceID
                    AND
                    (
                        newer.assignedDate > ga.assignedDate
                        OR
                        (
                            newer.assignedDate = ga.assignedDate
                            AND newer.assignmentID > ga.assignmentID
                        )
                    )
              )
        )
        ORDER BY e.requestedDate DESC, e.escalationID DESC;";

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(query, con))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            cmd.Parameters.Add("@staffID", SqlDbType.Int).Value = staffID;
            da.Fill(dt);
        }

        return dt;
    }


}