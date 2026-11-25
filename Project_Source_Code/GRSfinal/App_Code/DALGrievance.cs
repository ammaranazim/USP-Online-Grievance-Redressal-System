using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class DALGrievance
{
    public static int RegisterGrievance(MGrievance Grievance)
    {
        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (consettings == null ||
            string.IsNullOrEmpty(consettings.ConnectionString))
        {
            throw new Exception("Database Connection Error");
        }

        // Calculate SLA from the original submission date.
        int resolutionDays;

        switch ((Grievance.GrievancePriority ?? "Medium")
            .Trim().ToLowerInvariant())
        {
            case "urgent":
                resolutionDays = 1;
                break;

            case "high":
                resolutionDays = 3;
                break;

            case "medium":
                resolutionDays = 5;
                break;

            case "low":
                resolutionDays = 7;
                break;

            default:
                resolutionDays = 5;
                break;
        }

        DateTime submissionDate = Grievance.GrievanceSubmissionDate;
        DateTime dueDate = submissionDate.Date.AddDays(resolutionDays);

        int result = 0;

        using (SqlConnection con =
            new SqlConnection(consettings.ConnectionString))
        {
            string query = @"
            INSERT INTO dbo.Grievance
            (
                stdID,
                categoryID,
                deptID,
                grievanceTitle,
                grievanceDescription,
                grievancePriority,
                grievanceStatus,
                grievanceSubmissionDate,
                grievanceResolutionDate,
                grievanceDueDate
            )
            VALUES
            (
                @stdID,
                @categoryID,
                @deptID,
                @grievanceTitle,
                @grievanceDescription,
                @grievancePriority,
                @grievanceStatus,
                @grievanceSubmissionDate,
                @grievanceResolutionDate,
                @grievanceDueDate
            );";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@stdID", Grievance.StdID);
                cmd.Parameters.AddWithValue("@categoryID", Grievance.CategoryID);
                cmd.Parameters.AddWithValue("@deptID", Grievance.DeptID);
                cmd.Parameters.AddWithValue("@grievanceTitle", Grievance.GrievanceTitle);
                cmd.Parameters.AddWithValue("@grievanceDescription", Grievance.GrievanceDescription);
                cmd.Parameters.AddWithValue("@grievancePriority", Grievance.GrievancePriority);
                cmd.Parameters.AddWithValue("@grievanceStatus", Grievance.GrievanceStatus);
                cmd.Parameters.AddWithValue("@grievanceSubmissionDate", submissionDate);

                cmd.Parameters.Add("@grievanceResolutionDate", SqlDbType.DateTime)
                    .Value = Grievance.GrievanceResolutionDate == DateTime.MinValue
                        ? (object)DBNull.Value
                        : Grievance.GrievanceResolutionDate;

                cmd.Parameters.Add("@grievanceDueDate", SqlDbType.Date)
                    .Value = dueDate;

                con.Open();
                result = cmd.ExecuteNonQuery() > 0 ? 1 : 0;
            }
        }

        return result;
    }
    public static DataTable GetAllGrievances()
    {
        DataTable dt = new DataTable();

        ConnectionStringSettings consettings = ConfigurationManager.ConnectionStrings["GRS"];

        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;

        SqlConnection con = new SqlConnection(constring);

        try
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT grievanceID, grievanceTitle FROM Grievance ORDER BY grievanceTitle",
                con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
        }
        finally
        {
            con.Close();
        }

        return dt;
    }
    public static DataTable GetGrievancesByStudentID(int stdID)
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
                grievanceID,
                grievanceTitle,
                grievanceStatus,
                grievanceSubmissionDate
            FROM Grievance
            WHERE stdID = @stdID
            ORDER BY grievanceSubmissionDate DESC";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@stdID", stdID);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }

        return dt;
    }
    public static DataTable GetGrievanceStatisticsByStudentID(int stdID)
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
        COUNT(*) AS TotalGrievances,

        COALESCE(SUM(CASE
            WHEN grievanceStatus = 'Pending'
            THEN 1 ELSE 0
        END), 0) AS PendingGrievances,

        COALESCE(SUM(CASE
            WHEN grievanceStatus = 'In Process'
            THEN 1 ELSE 0
        END), 0) AS InProcessGrievances,

        COALESCE(SUM(CASE
            WHEN grievanceStatus = 'Resolved'
            THEN 1 ELSE 0
        END), 0) AS ResolvedGrievances


            FROM Grievance
            WHERE stdID = @stdID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@stdID", stdID);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }

        return dt;
    }
    public static List<MGrievance> GetAllGrievanceList()
    {
        List<MGrievance> lst = new List<MGrievance>();

        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;

        SqlConnection con = new SqlConnection(constring);

        SqlDataReader dr = null;

        try
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM Grievance", con);

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                MGrievance grievance = new MGrievance(

                    Convert.ToInt32(dr["grievanceID"]),
                    Convert.ToInt32(dr["stdID"]),
                    Convert.ToInt32(dr["categoryID"]),
                    Convert.ToInt32(dr["deptID"]),
                    dr["grievanceTitle"].ToString(),
                    dr["grievanceDescription"].ToString(),
                    dr["grievancePriority"].ToString(),
                    dr["grievanceStatus"].ToString(),
                    Convert.ToDateTime(dr["grievanceSubmissionDate"]),
                    Convert.ToDateTime(dr["grievanceResolutionDate"])

                    );

                lst.Add(grievance);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            if (dr != null)
                dr.Close();

            con.Close();
        }

        return lst;
    }
    public static int UpdateGrievance(MGrievance Grievance)
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
            @"UPDATE Grievance
          SET
            stdID=@stdID,
            categoryID=@categoryID,
            deptID=@deptID,
            grievanceTitle=@grievanceTitle,
            grievanceDescription=@grievanceDescription,
            grievancePriority=@grievancePriority,
            grievanceStatus=@grievanceStatus,
            grievanceSubmissionDate=@grievanceSubmissionDate,
            grievanceResolutionDate=@grievanceResolutionDate
          WHERE
            grievanceID=@grievanceID", con);

            cmd.Parameters.AddWithValue("@grievanceID", Grievance.GrievanceID);
            cmd.Parameters.AddWithValue("@stdID", Grievance.StdID);
            cmd.Parameters.AddWithValue("@categoryID", Grievance.CategoryID);
            cmd.Parameters.AddWithValue("@deptID", Grievance.DeptID);
            cmd.Parameters.AddWithValue("@grievanceTitle", Grievance.GrievanceTitle);
            cmd.Parameters.AddWithValue("@grievanceDescription", Grievance.GrievanceDescription);
            cmd.Parameters.AddWithValue("@grievancePriority", Grievance.GrievancePriority);
            cmd.Parameters.AddWithValue("@grievanceStatus", Grievance.GrievanceStatus);
            cmd.Parameters.AddWithValue("@grievanceSubmissionDate", Grievance.GrievanceSubmissionDate);
            cmd.Parameters.AddWithValue("@grievanceResolutionDate", Grievance.GrievanceResolutionDate);

            result = cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            con.Close();
        }

        return result;
    }
    public static int DeleteGrievance(int grievanceID)
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
                "DELETE FROM Grievance WHERE grievanceID=@grievanceID", con);

            cmd.Parameters.AddWithValue("@grievanceID", grievanceID);

            result = cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            con.Close();
        }

        return result;
    }
    public static DataTable SearchGrievance(string keyword)
    {
        DataTable dt = new DataTable();

        string connStr = ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connStr))
        {
            string query = @"
        SELECT
    g.grievanceID,
    s.stdRegNo,
    c.categoryName,
    d.deptName,
    g.grievanceTitle,
    g.grievanceDescription,
    g.grievancePriority,
    g.grievanceStatus,
    g.grievanceSubmissionDate,
    g.grievanceResolutionDate
FROM Grievance g
INNER JOIN StudentInfo s
    ON g.stdID = s.stdID
INNER JOIN GrievanceCategory c
    ON g.categoryID = c.categoryID
INNER JOIN Department d
    ON g.deptID = d.deptID
WHERE
    s.stdRegNo LIKE '%' + @keyword + '%'
    OR c.categoryName LIKE '%' + @keyword + '%'
    OR d.deptName LIKE '%' + @keyword + '%'
    OR g.grievanceTitle LIKE '%' + @keyword + '%'
    OR g.grievanceDescription LIKE '%' + @keyword + '%'
    OR g.grievancePriority LIKE '%' + @keyword + '%'
    OR g.grievanceStatus LIKE '%' + @keyword + '%'
ORDER BY g.grievanceID DESC ";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@keyword", keyword);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
        }

        return dt;
    }
    public static MGrievance GetGrievanceByID(int grievanceID)
    {
        MGrievance grievance = null;

        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;

        SqlConnection con = new SqlConnection(constring);

        SqlDataReader dr = null;

        try
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
    @"SELECT
        g.*,
        c.categoryName AS CategoryName,
        d.deptName AS DepartmentName
      FROM Grievance g
      INNER JOIN GrievanceCategory c
          ON g.categoryID = c.categoryID
      INNER JOIN Department d
          ON g.deptID = d.deptID
      WHERE g.grievanceID = @grievanceID", con);

            cmd.Parameters.AddWithValue("@grievanceID", grievanceID);

            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                grievance = new MGrievance(
                    Convert.ToInt32(dr["grievanceID"]),
                    Convert.ToInt32(dr["stdID"]),
                    Convert.ToInt32(dr["categoryID"]),
                    Convert.ToInt32(dr["deptID"]),
                    dr["grievanceTitle"].ToString(),
                    dr["grievanceDescription"].ToString(),
                    dr["grievancePriority"].ToString(),
                    dr["grievanceStatus"].ToString(),
                    Convert.ToDateTime(dr["grievanceSubmissionDate"]),
                    dr["grievanceResolutionDate"] == DBNull.Value
        ? DateTime.MinValue
        : Convert.ToDateTime(dr["grievanceResolutionDate"])
                );
                grievance.CategoryName = dr["CategoryName"].ToString();
                grievance.DepartmentName = dr["DepartmentName"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (dr != null)
                dr.Close();

            con.Close();
        }

        return grievance;
    }
    public static DataTable GetAdminDashboardStatistics()
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
                COUNT(*) AS TotalGrievances,

                COALESCE(SUM(
                    CASE
                        WHEN grievanceStatus = 'Pending'
                        THEN 1
                        ELSE 0
                    END
                ), 0) AS PendingGrievances,

                COALESCE(SUM(
                    CASE
                        WHEN grievanceStatus = 'In Process'
                        THEN 1
                        ELSE 0
                    END
                ), 0) AS InProcessGrievances,

                COALESCE(SUM(
                    CASE
                        WHEN grievanceStatus = 'Resolved'
                        THEN 1
                        ELSE 0
                    END
                ), 0) AS ResolvedGrievances

            FROM Grievance";

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
    public static DataTable GetAdminDashboardCategoryStatistics()
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
                c.categoryID,
                c.categoryName,
                COUNT(g.grievanceID) AS GrievanceCount
            FROM GrievanceCategory c
            LEFT JOIN Grievance g
                ON c.categoryID = g.categoryID
            WHERE c.categoryStatus = 'Active'
            GROUP BY
                c.categoryID,
                c.categoryName
            ORDER BY
                c.categoryID";

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
    public static DataTable GetAdminDashboardDepartmentStatistics()
    {
        DataTable dt = new DataTable();

        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (consettings == null || string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;

        using (SqlConnection con = new SqlConnection(constring))
        {
            string query = @"
            SELECT TOP 5
                d.deptID,
                d.deptName,
                COUNT(g.grievanceID) AS TotalGrievances,
                COALESCE(SUM(
                    CASE 
                        WHEN g.grievanceStatus = 'Pending' 
                        THEN 1 ELSE 0 
                    END
                ), 0) AS PendingGrievances,
                COALESCE(SUM(
                    CASE 
                        WHEN g.grievanceStatus = 'In Process' 
                        THEN 1 ELSE 0 
                    END
                ), 0) AS InProcessGrievances,
                COALESCE(SUM(
                    CASE 
                        WHEN g.grievanceStatus = 'Resolved' 
                        THEN 1 ELSE 0 
                    END
                ), 0) AS ResolvedGrievances
            FROM Department d
            LEFT JOIN Grievance g 
                ON d.deptID = g.deptID
            GROUP BY 
                d.deptID, 
                d.deptName
            ORDER BY 
                COUNT(g.grievanceID) DESC,
                d.deptName ASC";

            using (SqlCommand cmd = new SqlCommand(query, con))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }

        return dt;
    }
    public static DataTable GetRecentAdminGrievances()
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
            SELECT TOP 5
                g.grievanceID,
                LTRIM(RTRIM(
                    ISNULL(s.stdFirstName, '') + ' ' +
                    ISNULL(s.stdLastName, '')
                )) AS StudentName,
                g.grievanceTitle,
                c.categoryName,
                d.deptName,
                g.grievanceStatus,
                g.grievanceSubmissionDate

            FROM Grievance g

            INNER JOIN StudentInfo s
                ON g.stdID = s.stdID

            LEFT JOIN GrievanceCategory c
                ON g.categoryID = c.categoryID

            LEFT JOIN Department d
                ON g.deptID = d.deptID

            ORDER BY
                g.grievanceSubmissionDate DESC,
                g.grievanceID DESC";

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
    public static DataTable GetAdminDashboardGrievanceTrend()
    {
        DataTable dt = new DataTable();

        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (consettings == null ||
            string.IsNullOrEmpty(consettings.ConnectionString))
        {
            throw new Exception("Database Connection Error");
        }

        string constring = consettings.ConnectionString;

        using (SqlConnection con = new SqlConnection(constring))
        {
            string query = @"
            SELECT
                YEAR(grievanceSubmissionDate) AS GrievanceYear,
                MONTH(grievanceSubmissionDate) AS GrievanceMonth,
                COUNT(*) AS GrievanceCount
            FROM Grievance
            GROUP BY
                YEAR(grievanceSubmissionDate),
                MONTH(grievanceSubmissionDate)
            ORDER BY
                GrievanceYear,
                GrievanceMonth";

            using (SqlCommand cmd =
                new SqlCommand(query, con))
            {
                using (SqlDataAdapter da =
                    new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }

        return dt;
    }

    public static DataTable GetAdminDashboardAttentionStatistics()
    {
        DataTable dt = new DataTable();

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
            string query = @"
            SELECT
                (
                    SELECT COUNT(*)
                    FROM EscalatedGrievance
                    WHERE requestStatus = 'Pending Admin Review'
                ) AS EscalatedGrievances,

                (
                    SELECT COUNT(*)
                    FROM Grievance
                    WHERE grievanceStatus = 'Pending'
                ) AS PendingGrievances";

            using (SqlCommand cmd = new SqlCommand(query, con))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }

        return dt;
    }
    public static DataTable GetAdminAllGrievances(
    string searchKeyword,
    string status,
    int categoryID,
    int deptID,
    DateTime? fromDate,
    DateTime? toDate)
    {
        DataTable dt = new DataTable();

        string connStr =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connStr))
        {
            string query = @"
        SELECT
            g.grievanceID,
            s.stdRegNo,
            LTRIM(RTRIM(
                ISNULL(s.stdFirstName, '') + ' ' +
                ISNULL(s.stdLastName, '')
            )) AS StudentName,
            c.categoryName,
            d.deptName,
            g.grievanceTitle,
            g.grievancePriority,
            g.grievanceStatus,
            g.grievanceSubmissionDate

        FROM Grievance g

        LEFT JOIN StudentInfo s
            ON g.stdID = s.stdID

        LEFT JOIN GrievanceCategory c
            ON g.categoryID = c.categoryID

        LEFT JOIN Department d
            ON g.deptID = d.deptID

        WHERE
        (
            @searchKeyword = ''
            OR CAST(g.grievanceID AS VARCHAR(50)) LIKE '%' + @searchKeyword + '%'
            OR s.stdRegNo LIKE '%' + @searchKeyword + '%'
            OR s.stdFirstName LIKE '%' + @searchKeyword + '%'
            OR s.stdLastName LIKE '%' + @searchKeyword + '%'
            OR (
                ISNULL(s.stdFirstName, '') + ' ' +
                ISNULL(s.stdLastName, '')
            ) LIKE '%' + @searchKeyword + '%'
        )

        AND
        (
            @status = ''
            OR g.grievanceStatus = @status
        )

        AND
        (
            @categoryID = 0
            OR g.categoryID = @categoryID
        )

        AND
        (
            @deptID = 0
            OR g.deptID = @deptID
        )

       AND
(
    @fromDate IS NULL
    OR g.grievanceSubmissionDate >= @fromDate
)

AND
(
    @toDate IS NULL
    OR g.grievanceSubmissionDate < DATEADD(DAY, 1, @toDate)
)

        ORDER BY
            g.grievanceSubmissionDate DESC,
            g.grievanceID DESC";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue(
                    "@searchKeyword",
                    searchKeyword ?? "");

                cmd.Parameters.AddWithValue(
                    "@status",
                    status ?? "");

                cmd.Parameters.AddWithValue(
                    "@categoryID",
                    categoryID);

                cmd.Parameters.AddWithValue(
                    "@deptID",
                    deptID);

                cmd.Parameters.AddWithValue(
                    "@fromDate",
                    (object)fromDate ?? DBNull.Value);

                cmd.Parameters.AddWithValue(
                    "@toDate",
                    (object)toDate ?? DBNull.Value);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }

        return dt;
    }
    public static DataTable GetAdminGrievanceDetailsByID(int grievanceID)
    {
        DataTable dt = new DataTable();

        string connStr =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connStr))
        {
            string query = @"
        SELECT
            g.grievanceID,
            s.stdID,
            s.stdRegNo,
            LTRIM(RTRIM(
                ISNULL(s.stdFirstName, '') + ' ' +
                ISNULL(s.stdLastName, '')
            )) AS StudentName,

            g.categoryID,
            c.categoryName,

            g.deptID,
            d.deptName,

            g.grievanceTitle,
            g.grievanceDescription,
            g.grievancePriority,
            g.grievanceStatus,
            g.grievanceSubmissionDate,
            g.grievanceResolutionDate

        FROM Grievance g

        LEFT JOIN StudentInfo s
            ON g.stdID = s.stdID

        LEFT JOIN GrievanceCategory c
            ON g.categoryID = c.categoryID

        LEFT JOIN Department d
            ON g.deptID = d.deptID

        WHERE g.grievanceID = @grievanceID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@grievanceID", grievanceID);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }

        return dt;
    }
    public static DataTable GetAdminGrievanceTimeline(int grievanceID)
    {
        DataTable dt = new DataTable();

        ConnectionStringSettings settings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (settings == null ||
            string.IsNullOrEmpty(settings.ConnectionString))
        {
            throw new Exception("Database Connection Error");
        }

        string query = @"
        SELECT
            g.grievanceID,
            g.grievanceStatus,
            g.grievanceSubmissionDate,
            g.grievanceResolutionDate,
            g.grievanceDueDate,
            g.deptID,
            d.deptName,

            (
                SELECT TOP 1 tg.trackingactivitydatetime
                FROM dbo.TrackGrievance tg
                WHERE tg.grievanceID = g.grievanceID
                  AND LOWER(tg.trackingactivityDescription)
                      LIKE '%department review%'
                ORDER BY tg.trackingactivitydatetime ASC
            ) AS DepartmentReviewDate,

            (
                SELECT TOP 1 tg.trackingactivitydatetime
                FROM dbo.TrackGrievance tg
                WHERE tg.grievanceID = g.grievanceID
                  AND (
                      LOWER(tg.trackingactivityDescription)
                          LIKE '%assigned to staff%'
                      OR LOWER(tg.trackingactivityDescription)
                          LIKE '%staff assigned%'
                      OR LOWER(tg.trackingactivityDescription)
                          LIKE '%assigned staff%'
                  )
                ORDER BY tg.trackingactivitydatetime ASC
            ) AS StaffAssignedDate,

            (
                SELECT TOP 1 tg.trackingactivitydatetime
                FROM dbo.TrackGrievance tg
                WHERE tg.grievanceID = g.grievanceID
                  AND (
                      LOWER(tg.trackingactivityDescription)
                          LIKE '%resolution under review%'
                      OR LOWER(tg.trackingactivityDescription)
                          LIKE '%resolution submitted for review%'
                  )
                ORDER BY tg.trackingactivitydatetime ASC
            ) AS ResolutionReviewDate

        FROM dbo.Grievance g
        LEFT JOIN dbo.Department d
            ON g.deptID = d.deptID
        WHERE g.grievanceID = @grievanceID;";

        using (SqlConnection con =
            new SqlConnection(settings.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                .Value = grievanceID;

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }

        return dt;
    }
public static DataTable GetHODDashboardStatistics(int deptID)
{
    DataTable dt = new DataTable();

    ConnectionStringSettings consettings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (consettings == null ||
        string.IsNullOrEmpty(consettings.ConnectionString))
    {
        throw new Exception("Database Connection Error");
    }

    string constring = consettings.ConnectionString;

    using (SqlConnection con = new SqlConnection(constring))
    {
        string query = @"
            SELECT
                COUNT(*) AS TotalGrievances,

                COALESCE(SUM(
                    CASE WHEN grievanceStatus = 'Pending'
                    THEN 1 ELSE 0 END
                ), 0) AS PendingGrievances,

                COALESCE(SUM(
                    CASE WHEN grievanceStatus = 'In Process'
                    THEN 1 ELSE 0 END
                ), 0) AS InProcessGrievances,

                COALESCE(SUM(
                    CASE WHEN grievanceStatus = 'Resolved'
                    THEN 1 ELSE 0 END
                ), 0) AS ResolvedGrievances

            FROM Grievance
            WHERE deptID = @deptID";

        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@deptID", deptID);

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }
    }

    return dt;
}


public static DataTable GetHODDashboardRecentGrievances(int deptID)
{
    DataTable dt = new DataTable();

    ConnectionStringSettings consettings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (consettings == null ||
        string.IsNullOrEmpty(consettings.ConnectionString))
    {
        throw new Exception("Database Connection Error");
    }

    string constring = consettings.ConnectionString;

    using (SqlConnection con = new SqlConnection(constring))
    {
        string query = @"
            SELECT TOP 5
                g.grievanceID,
                LTRIM(RTRIM(
                    s.stdFirstName + ' ' + s.stdLastName
                )) AS studentName,
                g.grievanceTitle,
                g.grievanceStatus,
                g.grievanceSubmissionDate AS submissionDate
            FROM Grievance g
            INNER JOIN StudentInfo s
                ON g.stdID = s.stdID
            WHERE g.deptID = @deptID
            ORDER BY
                g.grievanceSubmissionDate DESC,
                g.grievanceID DESC";

        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@deptID", deptID);

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }
    }

    return dt;
}


public static DataTable GetHODDepartmentName(int deptID)
{
    DataTable dt = new DataTable();

    ConnectionStringSettings consettings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (consettings == null ||
        string.IsNullOrEmpty(consettings.ConnectionString))
    {
        throw new Exception("Database Connection Error");
    }

    string constring = consettings.ConnectionString;

    using (SqlConnection con = new SqlConnection(constring))
    {
        string query = @"
            SELECT deptName
            FROM Department
            WHERE deptID = @deptID";

        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@deptID", deptID);

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }
    }

    return dt;
}
public static DataTable GetHODDashboardGrievanceTrend(int deptID)
{
    DataTable dt = new DataTable();

    ConnectionStringSettings consettings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (consettings == null ||
        string.IsNullOrEmpty(consettings.ConnectionString))
    {
        throw new Exception("Database Connection Error");
    }

    string query = @"
        WITH Months AS
        (
            SELECT DATEADD(
                MONTH,
                -v.MonthOffset,
                DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)
            ) AS MonthStart
            FROM (VALUES (5), (4), (3), (2), (1), (0))
                AS v(MonthOffset)
        )
        SELECT
            YEAR(m.MonthStart) AS GrievanceYear,
            MONTH(m.MonthStart) AS GrievanceMonth,
            COUNT(g.grievanceID) AS GrievanceCount
        FROM Months m
        LEFT JOIN Grievance g
            ON g.deptID = @deptID
            AND g.grievanceSubmissionDate >= m.MonthStart
            AND g.grievanceSubmissionDate < DATEADD(
                MONTH, 1, m.MonthStart
            )
        GROUP BY m.MonthStart
        ORDER BY m.MonthStart;";

    using (SqlConnection con =
        new SqlConnection(consettings.ConnectionString))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        cmd.Parameters.AddWithValue("@deptID", deptID);

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}

public static DataTable GetHODGrievances(
    int deptID,
    string searchText,
    string statusFilter,
    string priorityFilter,
    DateTime? dateFrom,
    DateTime? dateTo)
{
    DataTable dt = new DataTable();

    ConnectionStringSettings settings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (settings == null ||
        string.IsNullOrEmpty(settings.ConnectionString))
    {
        throw new Exception("Database Connection Error");
    }

    string query = @"
        SELECT
            g.grievanceID,
            LTRIM(RTRIM(
                ISNULL(s.stdFirstName, '') + ' ' +
                ISNULL(s.stdLastName, '')
            )) AS studentName,
                 s.stdRegNo,
            g.grievanceTitle,
            g.grievancePriority,
            g.grievanceStatus,
            g.grievanceSubmissionDate AS submissionDate
        FROM Grievance g
        LEFT JOIN StudentInfo s ON g.stdID = s.stdID
        WHERE g.deptID = @deptID
          AND (
                @searchText = ''
                OR CAST(g.grievanceID AS VARCHAR(20))
                    LIKE '%' + @searchText + '%'
                OR ISNULL(s.stdFirstName, '')
                    LIKE '%' + @searchText + '%'
                OR ISNULL(s.stdLastName, '')
                    LIKE '%' + @searchText + '%'
OR ISNULL(s.stdRegNo, '')
    LIKE '%' + @searchText + '%'
                OR LTRIM(RTRIM(
                    ISNULL(s.stdFirstName, '') + ' ' +
                    ISNULL(s.stdLastName, '')
                )) LIKE '%' + @searchText + '%'
                OR g.grievanceTitle LIKE '%' + @searchText + '%'
          )
          AND (@statusFilter = ''
               OR g.grievanceStatus = @statusFilter)
          AND (@priorityFilter = ''
               OR g.grievancePriority = @priorityFilter)
          AND (@dateFrom IS NULL
               OR g.grievanceSubmissionDate >= @dateFrom)
          AND (@dateTo IS NULL
               OR g.grievanceSubmissionDate < DATEADD(DAY, 1, @dateTo))
        ORDER BY
            g.grievanceSubmissionDate DESC,
            g.grievanceID DESC;";

    using (SqlConnection con =
        new SqlConnection(settings.ConnectionString))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        string keyword = (searchText ?? "").Trim();

        if (keyword.StartsWith("GRV-",
            StringComparison.OrdinalIgnoreCase))
        {
            keyword = keyword.Substring(4).Trim();
        }

        cmd.Parameters.Add("@deptID", SqlDbType.Int).Value = deptID;

        cmd.Parameters.Add("@searchText", SqlDbType.NVarChar, 200)
            .Value = keyword;

        cmd.Parameters.Add("@statusFilter", SqlDbType.NVarChar, 30)
            .Value = statusFilter ?? "";

        cmd.Parameters.Add("@priorityFilter", SqlDbType.NVarChar, 30)
            .Value = priorityFilter ?? "";

        cmd.Parameters.Add("@dateFrom", SqlDbType.Date)
            .Value = dateFrom.HasValue
                ? (object)dateFrom.Value.Date
                : DBNull.Value;

        cmd.Parameters.Add("@dateTo", SqlDbType.Date)
            .Value = dateTo.HasValue
                ? (object)dateTo.Value.Date
                : DBNull.Value;

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}

public static DataTable GetHODGrievanceDetailsByID(int grievanceID, int deptID)
{
    DataTable dt = new DataTable();

    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    using (SqlConnection con = new SqlConnection(connStr))
    {
        string query = @"
            SELECT
                g.grievanceID,
                s.stdID,
                s.stdRegNo,
                LTRIM(RTRIM(
                    ISNULL(s.stdFirstName, '') + ' ' +
                    ISNULL(s.stdLastName, '')
                )) AS StudentName,
                c.categoryName,
                d.deptName,
                g.grievanceTitle,
                g.grievanceDescription,
                g.grievancePriority,
                g.grievanceStatus,
                g.grievanceSubmissionDate,
                g.grievanceResolutionDate,
                  g.grievanceDueDate
            FROM Grievance g
            LEFT JOIN StudentInfo s
                ON g.stdID = s.stdID
            LEFT JOIN GrievanceCategory c
                ON g.categoryID = c.categoryID
            LEFT JOIN Department d
                ON g.deptID = d.deptID
            WHERE g.grievanceID = @grievanceID
              AND g.deptID = @deptID;";

        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value =
                grievanceID;

            cmd.Parameters.Add("@deptID", SqlDbType.Int).Value =
                deptID;

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }
    }

    return dt;
}
public static DataTable GetActiveStaffByDepartment(int deptID)
{
    DataTable dt = new DataTable();

    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    using (SqlConnection con = new SqlConnection(connStr))
    using (SqlCommand cmd = new SqlCommand(@"
        SELECT staffID, staffName, staffDesignation
        FROM dbo.Staff
        WHERE deptID = @deptID
          AND staffStatus = 'Active'
        ORDER BY staffName;", con))
    {
        cmd.Parameters.Add("@deptID", SqlDbType.Int).Value = deptID;

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}

public static DataTable GetHODAssignmentDetails(int grievanceID, int deptID)
{
    DataTable dt = new DataTable();

    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    using (SqlConnection con = new SqlConnection(connStr))
    using (SqlCommand cmd = new SqlCommand(@"
        SELECT TOP 1
            ga.assignmentID,
            ga.staffID,
            ga.assignedDate,
            ga.assignmentStatus,
            s.staffName,
            s.staffDesignation
        FROM dbo.GrievanceAssignment ga
        INNER JOIN dbo.Staff s ON s.staffID = ga.staffID
        INNER JOIN dbo.Grievance g ON g.grievanceID = ga.grievanceID
        WHERE ga.grievanceID = @grievanceID
          AND g.deptID = @deptID
        ORDER BY ga.assignedDate DESC, ga.assignmentID DESC;", con))
    {
        cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value = grievanceID;
        cmd.Parameters.Add("@deptID", SqlDbType.Int).Value = deptID;

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}


public static int AssignHODGrievanceToStaff(
    int grievanceID,
    int staffID,
    int deptHeadID,
    int deptID)
{
    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    using (SqlConnection con = new SqlConnection(connStr))
    {
        con.Open();

        using (SqlTransaction tran =
            con.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
            try
            {
                // 1. Confirm the grievance belongs to this HOD's department.
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT COUNT(1)
                    FROM dbo.Grievance WITH (UPDLOCK, HOLDLOCK)
                    WHERE grievanceID = @grievanceID
                      AND deptID = @deptID;", con, tran))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value =
                        grievanceID;
                    cmd.Parameters.Add("@deptID", SqlDbType.Int).Value =
                        deptID;

                    if (Convert.ToInt32(cmd.ExecuteScalar()) != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }

                // 2. Confirm the staff member is active and in this department.
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT COUNT(1)
                    FROM dbo.Staff
                    WHERE staffID = @staffID
                      AND deptID = @deptID
                      AND staffStatus = 'Active';", con, tran))
                {
                    cmd.Parameters.Add("@staffID", SqlDbType.Int).Value =
                        staffID;
                    cmd.Parameters.Add("@deptID", SqlDbType.Int).Value =
                        deptID;

                    if (Convert.ToInt32(cmd.ExecuteScalar()) != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }

                // 3. Prevent duplicate active assignments.
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT COUNT(1)
                    FROM dbo.GrievanceAssignment WITH (UPDLOCK, HOLDLOCK)
                    WHERE grievanceID = @grievanceID
                      AND assignmentStatus = 'Assigned';", con, tran))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value =
                        grievanceID;

                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                    {
                        tran.Rollback();
                        return 2;
                    }
                }

                // 4. Insert the staff assignment.
                using (SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO dbo.GrievanceAssignment
                    (
                        grievanceID,
                        staffID,
                        assignedBy,
                        assignedDate,
                        assignmentStatus
                    )
                    VALUES
                    (
                        @grievanceID,
                        @staffID,
                        @deptHeadID,
                        GETDATE(),
                        'Assigned'
                    );", con, tran))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value =
                        grievanceID;
                    cmd.Parameters.Add("@staffID", SqlDbType.Int).Value =
                        staffID;
                    cmd.Parameters.Add("@deptHeadID", SqlDbType.Int).Value =
                        deptHeadID;

                    cmd.ExecuteNonQuery();
                }

                // 5. Update grievance status in the same transaction.
                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE dbo.Grievance
                    SET grievanceStatus = 'In Process'
                    WHERE grievanceID = @grievanceID
                      AND deptID = @deptID
                      AND grievanceStatus = 'Pending';", con, tran))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value =
                        grievanceID;
                    cmd.Parameters.Add("@deptID", SqlDbType.Int).Value =
                        deptID;

                    int rowsUpdated = cmd.ExecuteNonQuery();
                  
                    if (rowsUpdated != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }

                
                // 6. Record the staff assignment in the tracking history.
                using (SqlCommand cmd = new SqlCommand(@"
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
        'Assigned to Staff',
        GETDATE()
    );", con, tran))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value =
                        grievanceID;

                    cmd.Parameters.Add("@loginID", SqlDbType.Int).Value =
                        deptHeadID;

                    cmd.ExecuteNonQuery();
                }

                // 7. Commit assignment, status update and tracking event together.
                tran.Commit();
                return 1;
            }
            catch
            {
                try
                {
                    tran.Rollback();
                }
                catch
                {
                    // Preserve the original database exception.
                }

                throw;
            }
        }
    }
}


public static DataTable GetHODStaffAssignmentOverview(
    int deptID,
    string searchText,
    string assignmentStatusFilter)
{
    DataTable dt = new DataTable();

    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    string keyword = (searchText ?? "").Trim();

    if (keyword.StartsWith("GRV-", StringComparison.OrdinalIgnoreCase))
    {
        keyword = keyword.Substring(4).Trim();
    }

    string query = @"
        SELECT
            g.grievanceID,
            g.grievanceTitle,
            LTRIM(RTRIM(
                ISNULL(s.stdFirstName, '') + ' ' +
                ISNULL(s.stdLastName, '')
            )) AS studentName,
            s.stdRegNo,
            g.grievancePriority,
            g.grievanceStatus,
            latest.staffName,
           CASE
    WHEN latest.assignmentStatus = 'Assigned' THEN 'Assigned'
    ELSE 'Unassigned'
END AS assignmentStatus,
            latest.assignedDate
        FROM dbo.Grievance g
        LEFT JOIN dbo.StudentInfo s
            ON s.stdID = g.stdID
        OUTER APPLY
        (
            SELECT TOP 1
                ga.assignmentStatus,
                ga.assignedDate,
                st.staffName
            FROM dbo.GrievanceAssignment ga
            INNER JOIN dbo.Staff st
                ON st.staffID = ga.staffID
            WHERE ga.grievanceID = g.grievanceID
            ORDER BY ga.assignedDate DESC, ga.assignmentID DESC
        ) latest
        WHERE g.deptID = @deptID
          AND
          (
              @searchText = ''
              OR CAST(g.grievanceID AS VARCHAR(20))
                    LIKE '%' + @searchText + '%'
              OR g.grievanceTitle LIKE '%' + @searchText + '%'
              OR ISNULL(s.stdRegNo, '')
                    LIKE '%' + @searchText + '%'
              OR ISNULL(s.stdFirstName, '')
                    LIKE '%' + @searchText + '%'
              OR ISNULL(s.stdLastName, '')
                    LIKE '%' + @searchText + '%'
              OR LTRIM(RTRIM(
                    ISNULL(s.stdFirstName, '') + ' ' +
                    ISNULL(s.stdLastName, '')
                 )) LIKE '%' + @searchText + '%'
          )
          AND
          (
              @assignmentStatusFilter = ''
              OR
              (
                  @assignmentStatusFilter = 'Assigned'
                  AND latest.assignmentStatus = 'Assigned'
              )
              OR
              (
                  @assignmentStatusFilter = 'Unassigned'
                  AND
                  (
                      latest.assignmentStatus IS NULL
                      OR latest.assignmentStatus <> 'Assigned'
                  )
              )
          )
        ORDER BY
            g.grievanceSubmissionDate DESC,
            g.grievanceID DESC;";

    using (SqlConnection con = new SqlConnection(connStr))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        cmd.Parameters.Add("@deptID", SqlDbType.Int).Value = deptID;
        cmd.Parameters.Add("@searchText", SqlDbType.NVarChar, 250)
            .Value = keyword;
        cmd.Parameters.Add("@assignmentStatusFilter", SqlDbType.NVarChar, 20)
            .Value = assignmentStatusFilter ?? "";

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}

public static DataTable GetHODStaffAssignmentCounts(int deptID)
{
    DataTable dt = new DataTable();

    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    string query = @"
    SELECT
        COUNT(*) AS TotalGrievances,

        ISNULL(SUM(CASE
            WHEN latest.assignmentStatus = 'Assigned' THEN 1
            ELSE 0
        END), 0) AS AssignedGrievances,

        ISNULL(SUM(CASE
            WHEN latest.assignmentStatus IS NULL
                 OR latest.assignmentStatus <> 'Assigned' THEN 1
            ELSE 0
        END), 0) AS UnassignedGrievances

    FROM dbo.Grievance g

    OUTER APPLY
    (
        SELECT TOP 1
            ga.assignmentStatus
        FROM dbo.GrievanceAssignment ga
        WHERE ga.grievanceID = g.grievanceID
        ORDER BY ga.assignedDate DESC, ga.assignmentID DESC
    ) latest

    WHERE g.deptID = @deptID;";


    using (SqlConnection con = new SqlConnection(connStr))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        cmd.Parameters.Add("@deptID", SqlDbType.Int).Value = deptID;

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}

public static int UpdateHODGrievancePriorityAndDueDate(
int grievanceID,
int deptID,
string priority,
DateTime dueDate)
{
    int rowsAffected = 0;

    ConnectionStringSettings settings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (settings == null ||
        string.IsNullOrEmpty(settings.ConnectionString))
    {
        throw new Exception("Database Connection Error");
    }

    using (SqlConnection con =
        new SqlConnection(settings.ConnectionString))
    {
        string query = @"
            UPDATE dbo.Grievance
            SET grievancePriority = @Priority,
                grievanceDueDate = @DueDate
            WHERE grievanceID = @GrievanceID
              AND deptID = @DeptID
              AND grievanceStatus NOT IN ('Resolved', 'Rejected');";

        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@Priority", priority);
            cmd.Parameters.AddWithValue("@DueDate", dueDate.Date);
            cmd.Parameters.AddWithValue("@GrievanceID", grievanceID);
            cmd.Parameters.AddWithValue("@DeptID", deptID);

            con.Open();
            rowsAffected = cmd.ExecuteNonQuery();
        }
    }

    return rowsAffected;
}
public static DataTable GetStudentGrievanceTimeline(
int grievanceID,
int studentID)
{
    DataTable dt = new DataTable();

    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    string query = @"
        SELECT
            g.grievanceStatus,

            CASE
                WHEN EXISTS
                (
                    SELECT 1
                    FROM dbo.GrievanceAssignment ga
                    WHERE ga.grievanceID = g.grievanceID
                      AND ga.assignmentStatus = 'Assigned'
                )
                THEN 1 ELSE 0
            END AS HasAssignment,

            CASE
                WHEN EXISTS
                (
                    SELECT 1
                    FROM dbo.TrackGrievance tg
                    WHERE tg.grievanceID = g.grievanceID
                      AND
                      (
                          tg.trackingactivityDescription
                              LIKE '%Resolution Under Review%'
                          OR tg.trackingactivityDescription
                              LIKE '%Resolution Submitted for Review%'
                      )
                )
                THEN 1 ELSE 0
            END AS HasResolutionReview

        FROM dbo.Grievance g
        WHERE g.grievanceID = @grievanceID
          AND g.stdID = @studentID;";

    using (SqlConnection con = new SqlConnection(connStr))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value =
            grievanceID;

        cmd.Parameters.Add("@studentID", SqlDbType.Int).Value =
            studentID;

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}
public static DataTable GetHODGrievanceTimeline(
int grievanceID,
int deptID)
{
    DataTable dt = new DataTable();

    ConnectionStringSettings settings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (settings == null ||
        string.IsNullOrEmpty(settings.ConnectionString))
    {
        throw new Exception("Database Connection Error");
    }

    string query = @"
        SELECT
            g.grievanceID,
            g.grievanceStatus,
            g.grievanceSubmissionDate,
            g.grievanceResolutionDate,
            g.grievanceDueDate,
            g.deptID,
            d.deptName,

            (
                SELECT TOP 1 tg.trackingactivitydatetime
                FROM dbo.TrackGrievance tg
                WHERE tg.grievanceID = g.grievanceID
                  AND LOWER(tg.trackingactivityDescription)
                      LIKE '%department review%'
                ORDER BY tg.trackingactivitydatetime ASC
            ) AS DepartmentReviewDate,

            (
                SELECT TOP 1 tg.trackingactivitydatetime
                FROM dbo.TrackGrievance tg
                WHERE tg.grievanceID = g.grievanceID
                  AND (
                      LOWER(tg.trackingactivityDescription)
                          LIKE '%assigned to staff%'
                      OR LOWER(tg.trackingactivityDescription)
                          LIKE '%staff assigned%'
                      OR LOWER(tg.trackingactivityDescription)
                          LIKE '%assigned staff%'
                  )
                ORDER BY tg.trackingactivitydatetime ASC
            ) AS StaffAssignedDate,

            (
                SELECT TOP 1 tg.trackingactivitydatetime
                FROM dbo.TrackGrievance tg
                WHERE tg.grievanceID = g.grievanceID
                  AND (
                      LOWER(tg.trackingactivityDescription)
                          LIKE '%resolution under review%'
                      OR LOWER(tg.trackingactivityDescription)
                          LIKE '%resolution submitted for review%'
                  )
                ORDER BY tg.trackingactivitydatetime ASC
            ) AS ResolutionReviewDate

        FROM dbo.Grievance g
        LEFT JOIN dbo.Department d
            ON g.deptID = d.deptID
        WHERE g.grievanceID = @grievanceID
          AND g.deptID = @deptID;";

    using (SqlConnection con =
        new SqlConnection(settings.ConnectionString))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
            .Value = grievanceID;

        cmd.Parameters.Add("@deptID", SqlDbType.Int)
            .Value = deptID;

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}

public static DataTable GetHODResolutionByGrievanceID(
    int grievanceID,
    int deptID)
{
    DataTable dt = new DataTable();

    ConnectionStringSettings settings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (settings == null ||
        string.IsNullOrEmpty(settings.ConnectionString))
    {
        throw new Exception("Database Connection Error");
    }

    string query = @"
        SELECT TOP 1
            r.resolutionID,
            r.grievanceID,
            r.staffID,
            s.staffName,
            r.proposedResolution,
            r.staffRemarks,
            r.submittedDate,
            r.reviewStatus,
            r.hodRemarks,
            r.reviewedByDeptHeadID,
            r.reviewedDate
        FROM dbo.Grievance g
        INNER JOIN dbo.GrievanceResolution r
            ON r.grievanceID = g.grievanceID
        LEFT JOIN dbo.Staff s
            ON s.staffID = r.staffID
        WHERE g.grievanceID = @grievanceID
          AND g.deptID = @deptID
        ORDER BY
            r.submittedDate DESC,
            r.resolutionID DESC;";

    using (SqlConnection con =
        new SqlConnection(settings.ConnectionString))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
            .Value = grievanceID;

        cmd.Parameters.Add("@deptID", SqlDbType.Int)
            .Value = deptID;

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}

public static int ReviewStaffResolution(
    int grievanceID,
    int deptID,
    int deptHeadID,
    string decision,
    string hodRemarks)
{
    decision = (decision ?? "").Trim();
    hodRemarks = (hodRemarks ?? "").Trim();

    if (grievanceID <= 0 || deptID <= 0 || deptHeadID <= 0)
        return 0;

    if (decision != "Approve" && decision != "Return")
        return 0;

    if (decision == "Return" &&
        string.IsNullOrWhiteSpace(hodRemarks))
        return 2;

    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    using (SqlConnection con = new SqlConnection(connStr))
    {
        con.Open();

        using (SqlTransaction tran =
            con.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
            try
            {
                // Verify HOD department ownership and lock the grievance.
                string ownershipQuery = @"
                    SELECT COUNT(1)
                    FROM dbo.Grievance g WITH (UPDLOCK, HOLDLOCK)
                    INNER JOIN dbo.DepartmentHead dh
                        ON dh.deptHeadID = @deptHeadID
                       AND dh.deptID = g.deptID
                    WHERE g.grievanceID = @grievanceID
                      AND g.deptID = @deptID
                      AND g.grievanceStatus = 'In Process';";

                using (SqlCommand cmd =
                    new SqlCommand(ownershipQuery, con, tran))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                        .Value = grievanceID;
                    cmd.Parameters.Add("@deptID", SqlDbType.Int)
                        .Value = deptID;
                    cmd.Parameters.Add("@deptHeadID", SqlDbType.Int)
                        .Value = deptHeadID;

                    if (Convert.ToInt32(cmd.ExecuteScalar()) != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }

                // Lock and retrieve the latest resolution awaiting review.
                string resolutionQuery = @"
                    SELECT TOP 1 r.resolutionID
                    FROM dbo.GrievanceResolution r WITH (UPDLOCK, HOLDLOCK)
                    WHERE r.grievanceID = @grievanceID
                      AND r.reviewStatus = 'Pending'
                    ORDER BY r.submittedDate DESC, r.resolutionID DESC;";

                int resolutionID;

                using (SqlCommand cmd =
                    new SqlCommand(resolutionQuery, con, tran))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                        .Value = grievanceID;

                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                    {
                        tran.Rollback();
                        return 0;
                    }

                    resolutionID = Convert.ToInt32(result);
                }

                // Save the HOD decision on the selected resolution.
                string updateResolutionQuery = @"
                    UPDATE dbo.GrievanceResolution
                    SET reviewStatus = @reviewStatus,
                        hodRemarks = @hodRemarks,
                        reviewedByDeptHeadID = @deptHeadID,
                        reviewedDate = GETDATE()
                    WHERE resolutionID = @resolutionID
                      AND grievanceID = @grievanceID
                      AND reviewStatus = 'Pending';";

                string newReviewStatus =
                    decision == "Approve" ? "Approved" : "Returned";

                using (SqlCommand cmd =
                    new SqlCommand(updateResolutionQuery, con, tran))
                {
                    cmd.Parameters.Add("@reviewStatus", SqlDbType.NVarChar, 30)
                        .Value = newReviewStatus;

                    cmd.Parameters.Add("@hodRemarks", SqlDbType.NVarChar, 2000)
                        .Value = string.IsNullOrWhiteSpace(hodRemarks)
                            ? (object)DBNull.Value
                            : hodRemarks;

                    cmd.Parameters.Add("@deptHeadID", SqlDbType.Int)
                        .Value = deptHeadID;

                    cmd.Parameters.Add("@resolutionID", SqlDbType.Int)
                        .Value = resolutionID;

                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                        .Value = grievanceID;

                    if (cmd.ExecuteNonQuery() != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }

                // Keep the overall grievance status within the
                // project's finalized status workflow.
                string newGrievanceStatus =
                    decision == "Approve" ? "Resolved" : "In Process";

                string updateGrievanceQuery = @"
                    UPDATE dbo.Grievance
                    SET grievanceStatus = @grievanceStatus,
                        grievanceResolutionDate =
                            CASE
                                WHEN @grievanceStatus = 'Resolved'
                                THEN GETDATE()
                                ELSE NULL
                            END
                    WHERE grievanceID = @grievanceID
                      AND deptID = @deptID
                      AND grievanceStatus = 'In Process';";

                using (SqlCommand cmd =
                    new SqlCommand(updateGrievanceQuery, con, tran))
                {
                    cmd.Parameters.Add("@grievanceStatus", SqlDbType.NVarChar, 50)
                        .Value = newGrievanceStatus;

                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                        .Value = grievanceID;

                    cmd.Parameters.Add("@deptID", SqlDbType.Int)
                        .Value = deptID;

                    if (cmd.ExecuteNonQuery() != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }

                // Record the decision in the grievance timeline.
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
                        @deptHeadID,
                        @description,
                        GETDATE()
                    );";

                using (SqlCommand cmd =
                    new SqlCommand(trackingQuery, con, tran))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                        .Value = grievanceID;

                    cmd.Parameters.Add("@deptHeadID", SqlDbType.Int)
                        .Value = deptHeadID;

                    cmd.Parameters.Add("@description", SqlDbType.NVarChar)
                        .Value = decision == "Approve"
                            ? "Resolution Approved"
                            : "Resolution Returned";

                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
                return 1;
            }
            catch
            {
                try
                {
                    tran.Rollback();
                }
                catch
                {
                    // Preserve the original exception.
                }

                throw;
            }
        }
    }
}

public static DataTable GetAdminAssignmentDetails(int grievanceID)
{
    DataTable dt = new DataTable();

    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    using (SqlConnection con = new SqlConnection(connStr))
    using (SqlCommand cmd = new SqlCommand(@"
        SELECT TOP 1
            d.deptName,
            dh.deptHeadName,
            s.staffName,
            s.staffDesignation,
            ga.assignedDate,
            ga.assignmentStatus
        FROM dbo.Grievance g
        LEFT JOIN dbo.Department d
            ON d.deptID = g.deptID
        LEFT JOIN dbo.DepartmentHead dh
            ON dh.deptID = g.deptID
        LEFT JOIN dbo.GrievanceAssignment ga
            ON ga.grievanceID = g.grievanceID
           AND ga.assignmentStatus = 'Assigned'
        LEFT JOIN dbo.Staff s
            ON s.staffID = ga.staffID
        WHERE g.grievanceID = @grievanceID
        ORDER BY ga.assignedDate DESC, ga.assignmentID DESC;", con))
    {
        cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value =
            grievanceID;

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}

public static DataTable GetHODProgressResolutionStatistics(int deptID)
{
    DataTable dt = new DataTable();

    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    string query = @"
    SELECT
        COUNT(*) AS TotalGrievances,

        COALESCE(SUM(
            CASE
                WHEN g.grievanceStatus = 'In Process'
                THEN 1 ELSE 0
            END
        ), 0) AS InProgressGrievances,

        COALESCE(SUM(
            CASE
                WHEN review.GrievanceID IS NOT NULL
                     AND g.grievanceStatus NOT IN ('Resolved', 'Rejected')
                THEN 1 ELSE 0
            END
        ), 0) AS AwaitingReview,

        COALESCE(SUM(
            CASE
                WHEN g.grievanceStatus = 'Resolved'
                THEN 1 ELSE 0
            END
        ), 0) AS ResolvedGrievances

    FROM dbo.Grievance g

    LEFT JOIN (
        SELECT DISTINCT grievanceID AS GrievanceID
        FROM dbo.TrackGrievance
        WHERE trackingactivityDescription LIKE '%Resolution Under Review%'
           OR trackingactivityDescription LIKE '%Resolution Submitted for Review%'
    ) review
        ON review.GrievanceID = g.grievanceID

    WHERE g.deptID = @deptID;";

    using (SqlConnection con = new SqlConnection(connStr))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        cmd.Parameters.Add("@deptID", SqlDbType.Int).Value = deptID;

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}


public static DataTable GetHODProgressResolutionOverview(
    int deptID,
    string searchText,
    string statusFilter,
    string priorityFilter,
    string stageFilter,
    string dueDateFilter)
{
    DataTable dt = new DataTable();

    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    string keyword = (searchText ?? "").Trim();

    if (keyword.StartsWith("GRV-", StringComparison.OrdinalIgnoreCase))
    {
        keyword = keyword.Substring(4).Trim();
    }

    string query = @"
        WITH ProgressData AS
        (
            SELECT
                g.grievanceID,
                g.grievanceTitle,
                g.grievancePriority,
                g.grievanceStatus,
                g.grievanceDueDate,
                g.grievanceSubmissionDate,

                LTRIM(RTRIM(
                    ISNULL(stu.stdFirstName, '') + ' ' +
                    ISNULL(stu.stdLastName, '')
                )) AS studentName,

                stu.stdRegNo,

                CASE
                    WHEN g.grievanceStatus = 'Rejected'
                        THEN 'Rejected'

                    WHEN g.grievanceStatus = 'Resolved'
                        THEN 'Resolved'

                    WHEN EXISTS
                    (
                        SELECT 1
                        FROM dbo.TrackGrievance tg
                        WHERE tg.grievanceID = g.grievanceID
                          AND
                          (
                              tg.trackingactivityDescription
                                  LIKE '%Resolution Under Review%'
                              OR tg.trackingactivityDescription
                                  LIKE '%Resolution Submitted for Review%'
                          )
                    )
                        THEN 'Resolution Under Review'

                    WHEN latest.assignmentStatus = 'Assigned'
                        THEN 'Assigned to Staff'

                    ELSE 'Department Review'
                END AS progressStage,

                CASE
                    WHEN latest.assignmentStatus = 'Assigned'
                        THEN latest.staffName
                    ELSE NULL
                END AS staffName

            FROM dbo.Grievance g

            LEFT JOIN dbo.StudentInfo stu
                ON stu.stdID = g.stdID

            OUTER APPLY
            (
                SELECT TOP 1
                    ga.assignmentStatus,
                    s.staffName
                FROM dbo.GrievanceAssignment ga
                INNER JOIN dbo.Staff s
                    ON s.staffID = ga.staffID
                WHERE ga.grievanceID = g.grievanceID
                ORDER BY
                    ga.assignedDate DESC,
                    ga.assignmentID DESC
            ) latest

            WHERE g.deptID = @deptID
        )

        SELECT
            grievanceID,
            grievanceTitle,
            studentName,
            stdRegNo,
            staffName,
            progressStage,
            grievancePriority,
            grievanceStatus,
            grievanceDueDate
        FROM ProgressData
        WHERE
            (
                @searchText = ''
                OR CAST(grievanceID AS VARCHAR(20))
                    LIKE '%' + @searchText + '%'
                OR ISNULL(grievanceTitle, '')
                    LIKE '%' + @searchText + '%'
                OR ISNULL(studentName, '')
                    LIKE '%' + @searchText + '%'
                OR ISNULL(stdRegNo, '')
                    LIKE '%' + @searchText + '%'
            )

            AND
            (
                @statusFilter = ''
                OR grievanceStatus = @statusFilter
            )

            AND
            (
                @priorityFilter = ''
                OR grievancePriority = @priorityFilter
            )

            AND
            (
                @stageFilter = ''
                OR progressStage = @stageFilter
            )

            AND
            (
                @dueDateFilter = ''

                OR
                (
                    @dueDateFilter = 'Overdue'
                    AND grievanceDueDate < CAST(GETDATE() AS DATE)
                    AND grievanceStatus NOT IN ('Resolved', 'Rejected')
                )

                OR
                (
                    @dueDateFilter = 'Today'
                    AND grievanceDueDate = CAST(GETDATE() AS DATE)
                    AND grievanceStatus NOT IN ('Resolved', 'Rejected')
                )

                OR
                (
                    @dueDateFilter = 'Upcoming'
                    AND grievanceDueDate > CAST(GETDATE() AS DATE)
                    AND grievanceStatus NOT IN ('Resolved', 'Rejected')
                )

                OR
                (
                    @dueDateFilter = 'None'
                    AND grievanceDueDate IS NULL
                )
            )

        ORDER BY grievanceSubmissionDate DESC, grievanceID DESC;";

    using (SqlConnection con = new SqlConnection(connStr))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        cmd.Parameters.Add("@deptID", SqlDbType.Int).Value = deptID;

        cmd.Parameters.Add("@searchText", SqlDbType.NVarChar, 250)
            .Value = keyword;

        cmd.Parameters.Add("@statusFilter", SqlDbType.NVarChar, 30)
            .Value = statusFilter ?? "";

        cmd.Parameters.Add("@priorityFilter", SqlDbType.NVarChar, 30)
            .Value = priorityFilter ?? "";

        cmd.Parameters.Add("@stageFilter", SqlDbType.NVarChar, 50)
            .Value = stageFilter ?? "";

        cmd.Parameters.Add("@dueDateFilter", SqlDbType.NVarChar, 20)
            .Value = dueDateFilter ?? "";

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}

public static DataTable GetStaffDashboardStatistics(int staffID)
{
    DataTable dt = new DataTable();

    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    string query = @"
        SELECT
            COUNT(*) AS TotalAssignedGrievances,

            COALESCE(SUM(
                CASE
                    WHEN g.grievanceStatus = 'Pending'
                    THEN 1 ELSE 0
                END
            ), 0) AS PendingGrievances,

            COALESCE(SUM(
                CASE
                    WHEN g.grievanceStatus = 'In Process'
                    THEN 1 ELSE 0
                END
            ), 0) AS InProcessGrievances,

            COALESCE(SUM(
                CASE
                    WHEN g.grievanceStatus = 'Resolved'
                    THEN 1 ELSE 0
                END
            ), 0) AS ResolvedGrievances,

            COALESCE(SUM(
                CASE
                    WHEN g.grievanceStatus = 'Rejected'
                    THEN 1 ELSE 0
                END
            ), 0) AS RejectedGrievances

        FROM dbo.GrievanceAssignment ga

        INNER JOIN dbo.Grievance g
            ON g.grievanceID = ga.grievanceID

        WHERE ga.staffID = @staffID
          AND ga.assignmentStatus = 'Assigned';";

    using (SqlConnection con = new SqlConnection(connStr))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        cmd.Parameters.Add("@staffID", SqlDbType.Int).Value = staffID;

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}


public static DataTable GetStaffAssignedGrievances(int staffID)
{
    DataTable dt = new DataTable();

    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    string query = @"
        SELECT
            g.grievanceID,
            g.grievanceTitle,
            g.grievancePriority,
            g.grievanceStatus,
            g.grievanceSubmissionDate,
            g.grievanceDueDate,
            ga.assignmentID,
            ga.assignedDate,
            c.categoryName,
            d.deptName

        FROM dbo.GrievanceAssignment ga

        INNER JOIN dbo.Grievance g
            ON g.grievanceID = ga.grievanceID

        LEFT JOIN dbo.GrievanceCategory c
            ON c.categoryID = g.categoryID

        LEFT JOIN dbo.Department d
            ON d.deptID = g.deptID

        WHERE ga.staffID = @staffID
          AND ga.assignmentStatus = 'Assigned'

        ORDER BY
            ga.assignedDate DESC,
            ga.assignmentID DESC;";

    using (SqlConnection con = new SqlConnection(connStr))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        cmd.Parameters.Add("@staffID", SqlDbType.Int).Value = staffID;

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}

public static DataTable GetStaffGrievanceDetailsByID(
    int grievanceID,
    int staffID)
{
    DataTable dt = new DataTable();

    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    string query = @"
        SELECT
            g.grievanceID,
            g.stdID,
            g.categoryID,
            g.deptID,
            g.grievanceTitle,
            g.grievanceDescription,
            g.grievancePriority,
            g.grievanceStatus,
            g.grievanceSubmissionDate,
            g.grievanceResolutionDate,
            g.grievanceDueDate,
            c.categoryName,
            d.deptName,
            ga.assignmentID,
            ga.assignedDate,
            ga.assignmentStatus
        FROM dbo.GrievanceAssignment ga
        INNER JOIN dbo.Grievance g
            ON g.grievanceID = ga.grievanceID
        LEFT JOIN dbo.GrievanceCategory c
            ON c.categoryID = g.categoryID
        LEFT JOIN dbo.Department d
            ON d.deptID = g.deptID
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

    using (SqlConnection con = new SqlConnection(connStr))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value =
            grievanceID;

        cmd.Parameters.Add("@staffID", SqlDbType.Int).Value =
            staffID;

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}


public static int StartStaffGrievanceWork(
    int grievanceID,
    int staffID)
{
    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    using (SqlConnection con = new SqlConnection(connStr))
    {
        con.Open();

        using (SqlTransaction tran =
            con.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
            try
            {
                // Confirm that this Staff member owns the latest active assignment.
                string validationQuery = @"
                    SELECT COUNT(1)
                    FROM dbo.GrievanceAssignment ga
                    INNER JOIN dbo.Grievance g
                        ON g.grievanceID = ga.grievanceID
                    WHERE ga.grievanceID = @grievanceID
                      AND ga.staffID = @staffID
                      AND ga.assignmentStatus = 'Assigned'
                      AND g.grievanceStatus = 'In Process'
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
                    new SqlCommand(validationQuery, con, tran))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value =
                        grievanceID;

                    cmd.Parameters.Add("@staffID", SqlDbType.Int).Value =
                        staffID;

                    if (Convert.ToInt32(cmd.ExecuteScalar()) != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }

                // Prevent duplicate Start Working tracking entries
                // for the same current assignment.
                string duplicateQuery = @"
                    SELECT COUNT(1)
                    FROM dbo.TrackGrievance tg
                    INNER JOIN dbo.GrievanceAssignment ga
                        ON ga.grievanceID = tg.grievanceID
                       AND ga.staffID = @staffID
                       AND ga.assignmentStatus = 'Assigned'
                    WHERE tg.grievanceID = @grievanceID
                      AND tg.loginID = @staffID
                      AND tg.trackingactivityDescription =
                          'Staff Started Working'
                      AND tg.trackingactivitydatetime >= ga.assignedDate
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
                    new SqlCommand(duplicateQuery, con, tran))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value =
                        grievanceID;

                    cmd.Parameters.Add("@staffID", SqlDbType.Int).Value =
                        staffID;

                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                    {
                        tran.Commit();
                        return 2;
                    }
                }

                // Record the Staff action in the existing tracking table.
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
                        @staffID,
                        'Staff Started Working',
                        GETDATE()
                    );";

                using (SqlCommand cmd =
                    new SqlCommand(trackingQuery, con, tran))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value =
                        grievanceID;

                    cmd.Parameters.Add("@staffID", SqlDbType.Int).Value =
                        staffID;

                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
                return 1;
            }
            catch
            {
                try
                {
                    tran.Rollback();
                }
                catch
                {
                    // Preserve the original exception.
                }

                throw;
            }
        }
    }
}
public static bool HasStaffStartedWorking(
int grievanceID,
int staffID)
{
    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    using (SqlConnection con = new SqlConnection(connStr))
    {
        string query = @"
            SELECT COUNT(1)
            FROM dbo.TrackGrievance tg
            WHERE tg.grievanceID = @grievanceID
              AND tg.loginID = @staffID
              AND tg.trackingactivityDescription =
                  'Staff Started Working'
              AND EXISTS
              (
                  SELECT 1
                  FROM dbo.GrievanceAssignment ga
                  WHERE ga.grievanceID = @grievanceID
                    AND ga.staffID = @staffID
                    AND ga.assignmentStatus = 'Assigned'
                    AND tg.trackingactivitydatetime >= ga.assignedDate
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
              );";

        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value =
                grievanceID;

            cmd.Parameters.Add("@staffID", SqlDbType.Int).Value =
                staffID;

            con.Open();

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
    }
}

public static int SubmitStaffResolution(
    int grievanceID,
    int staffID,
    string proposedResolution,
    string staffRemarks,
    string uploadedBy,
    List<MEvidence> evidenceList)
{
    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    using (SqlConnection con = new SqlConnection(connStr))
    {
        con.Open();

        using (SqlTransaction tran =
            con.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
            try
            {
                // Verify that the grievance is assigned to this Staff
                // member and that the assignment is still current.
                string validationQuery = @"
                    SELECT COUNT(1)
                    FROM dbo.GrievanceAssignment ga
                    INNER JOIN dbo.Grievance g
                        ON g.grievanceID = ga.grievanceID
                    WHERE ga.grievanceID = @grievanceID
                      AND ga.staffID = @staffID
                      AND ga.assignmentStatus = 'Assigned'
                      AND g.grievanceStatus = 'In Process'
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
                    new SqlCommand(validationQuery, con, tran))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                        .Value = grievanceID;

                    cmd.Parameters.Add("@staffID", SqlDbType.Int)
                        .Value = staffID;

                    if (Convert.ToInt32(cmd.ExecuteScalar()) != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }

                // Do not allow a second submission while an earlier
                // resolution is awaiting HOD review.
                string pendingQuery = @"
                    SELECT COUNT(1)
                    FROM dbo.GrievanceResolution
                    WHERE grievanceID = @grievanceID
                      AND reviewStatus = 'Pending';";

                using (SqlCommand cmd =
                    new SqlCommand(pendingQuery, con, tran))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                        .Value = grievanceID;

                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                    {
                        tran.Rollback();
                        return 2;
                    }
                }

                // Save the proposed resolution.
                string resolutionQuery = @"
                    INSERT INTO dbo.GrievanceResolution
                    (
                        grievanceID,
                        staffID,
                        proposedResolution,
                        staffRemarks,
                        submittedDate,
                        reviewStatus
                    )
                    VALUES
                    (
                        @grievanceID,
                        @staffID,
                        @proposedResolution,
                        @staffRemarks,
                        GETDATE(),
                        'Pending'
                    );";

                using (SqlCommand cmd =
                    new SqlCommand(resolutionQuery, con, tran))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                        .Value = grievanceID;

                    cmd.Parameters.Add("@staffID", SqlDbType.Int)
                        .Value = staffID;

                    cmd.Parameters.Add("@proposedResolution", SqlDbType.NVarChar, -1)
                        .Value = proposedResolution.Trim();

                    cmd.Parameters.Add("@staffRemarks", SqlDbType.NVarChar, 2000)
                        .Value = string.IsNullOrWhiteSpace(staffRemarks)
                            ? (object)DBNull.Value
                            : staffRemarks.Trim();

                    cmd.ExecuteNonQuery();
                }

                // Save evidence metadata within the same transaction.
                if (evidenceList != null)
                {
                    foreach (MEvidence evidence in evidenceList)
                    {
                        string evidenceQuery = @"
                            INSERT INTO dbo.Grievanceevidence
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
                                GETDATE(),
                                @uploadedBy,
                                @evidenceFilepath,
                                @grievanceID
                            );";

                        using (SqlCommand cmd =
                            new SqlCommand(evidenceQuery, con, tran))
                        {
                            cmd.Parameters.Add("@evidenceName", SqlDbType.NVarChar, 255)
                                .Value = evidence.EvidenceName;

                            cmd.Parameters.Add("@evidenceDescription", SqlDbType.NVarChar, 500)
                                .Value = evidence.EvidenceDescription;

                            cmd.Parameters.Add("@uploadedBy", SqlDbType.NVarChar, 100)
                                .Value = uploadedBy ?? "";

                            cmd.Parameters.Add("@evidenceFilepath", SqlDbType.NVarChar, 500)
                                .Value = evidence.EvidenceFilepath;

                            cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                                .Value = grievanceID;

                            cmd.ExecuteNonQuery();
                        }
                    }
                }

              

                // Record the activity in the existing grievance timeline.
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
                        @staffID,
                        'Resolution Under Review',
                        GETDATE()
                    );";

                using (SqlCommand cmd =
                    new SqlCommand(trackingQuery, con, tran))
                {
                    cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                        .Value = grievanceID;

                    cmd.Parameters.Add("@staffID", SqlDbType.Int)
                        .Value = staffID;

                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
                return 1;
            }
            catch
            {
                try
                {
                    tran.Rollback();
                }
                catch
                {
                    // Preserve the original database exception.
                }

                throw;
            }
        }
    }
}

public static DataTable GetStaffResolutionByGrievanceID(
    int grievanceID,
    int staffID)
{
    DataTable dt = new DataTable();

    ConnectionStringSettings consettings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (consettings == null ||
        string.IsNullOrEmpty(consettings.ConnectionString))
    {
        throw new Exception("Database Connection Error");
    }

    string query = @"
        SELECT TOP 1
    resolutionID,
    proposedResolution,
    staffRemarks,
    submittedDate,
    reviewStatus,
    hodRemarks,
    reviewedDate
        FROM dbo.GrievanceResolution
        WHERE grievanceID = @grievanceID
          AND staffID = @staffID
        ORDER BY submittedDate DESC, resolutionID DESC;";

    using (SqlConnection con =
        new SqlConnection(consettings.ConnectionString))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        cmd.Parameters.Add("@grievanceID", SqlDbType.Int).Value =
            grievanceID;

        cmd.Parameters.Add("@staffID", SqlDbType.Int).Value =
            staffID;

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}

public static DataTable GetAdminDetailedGrievanceReport(
    DateTime fromDate,
    DateTime toDate,
    string status,
    int categoryID,
    int deptID)
{
    DataTable dt = new DataTable();

    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    using (SqlConnection con = new SqlConnection(connStr))
    using (SqlCommand cmd = new SqlCommand(@"
        SELECT
            g.grievanceID,
            g.grievanceTitle,
            g.grievanceStatus,
            g.grievancePriority,
            g.grievanceSubmissionDate,
            g.grievanceResolutionDate,
            LTRIM(RTRIM(
                ISNULL(s.stdFirstName, '') + ' ' +
                ISNULL(s.stdLastName, '')
            )) AS StudentName,
            s.stdRegNo,
            c.categoryName,
            d.deptName
        FROM Grievance g
        LEFT JOIN StudentInfo s
            ON g.stdID = s.stdID
        LEFT JOIN GrievanceCategory c
            ON g.categoryID = c.categoryID
        LEFT JOIN Department d
            ON g.deptID = d.deptID
        WHERE
            g.grievanceSubmissionDate >= @fromDate
            AND g.grievanceSubmissionDate < @toDateExclusive
            AND (@status = '' OR g.grievanceStatus = @status)
            AND (@categoryID = 0 OR g.categoryID = @categoryID)
            AND (@deptID = 0 OR g.deptID = @deptID)
        ORDER BY g.grievanceSubmissionDate DESC;", con))
    {
        cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value =
            fromDate.Date;

        cmd.Parameters.Add("@toDateExclusive", SqlDbType.DateTime).Value =
            toDate.Date.AddDays(1);

        cmd.Parameters.Add("@status", SqlDbType.NVarChar, 50).Value =
            status ?? "";

        cmd.Parameters.Add("@categoryID", SqlDbType.Int).Value =
            categoryID;

        cmd.Parameters.Add("@deptID", SqlDbType.Int).Value =
            deptID;

        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }

    return dt;
}

public static int GetHODOverdueGrievanceCount(int deptID)
{
    string connStr =
        ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

    string query = @"
        SELECT COUNT(*)
        FROM dbo.Grievance
        WHERE deptID = @deptID
          AND grievanceDueDate < CAST(GETDATE() AS DATE)
          AND grievanceStatus NOT IN ('Resolved', 'Rejected');";

    using (SqlConnection con = new SqlConnection(connStr))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        cmd.Parameters.Add("@deptID", SqlDbType.Int).Value = deptID;

        con.Open();

        return Convert.ToInt32(cmd.ExecuteScalar());
    }
}


public static int RejectHODGrievance(
    int grievanceID,
    int deptID,
    int deptHeadID,
    string rejectionReason)
{
    rejectionReason = (rejectionReason ?? "").Trim();

    if (rejectionReason.Length == 0 ||
        rejectionReason.Length > 255)
    {
        return -1;
    }

    ConnectionStringSettings settings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (settings == null ||
        string.IsNullOrEmpty(settings.ConnectionString))
    {
        throw new Exception("Database Connection Error");
    }

    using (SqlConnection con =
        new SqlConnection(settings.ConnectionString))
    {
        con.Open();

        using (SqlTransaction tran =
            con.BeginTransaction(IsolationLevel.Serializable))
        {
            try
            {
                int studentID;

                // Verify department ownership and Pending status.
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT stdID
                    FROM dbo.Grievance WITH (UPDLOCK, HOLDLOCK)
                    WHERE grievanceID = @grievanceID
                      AND deptID = @deptID
                      AND grievanceStatus = 'Pending';",
                    con, tran))
                {
                    cmd.Parameters.Add("@grievanceID",
                        SqlDbType.Int).Value = grievanceID;

                    cmd.Parameters.Add("@deptID",
                        SqlDbType.Int).Value = deptID;

                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                    {
                        tran.Rollback();
                        return 0;
                    }

                    studentID = Convert.ToInt32(result);
                }

                // Update only a Pending grievance in this department.
                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE dbo.Grievance
                    SET grievanceStatus = 'Rejected'
                    WHERE grievanceID = @grievanceID
                      AND deptID = @deptID
                      AND grievanceStatus = 'Pending';",
                    con, tran))
                {
                    cmd.Parameters.Add("@grievanceID",
                        SqlDbType.Int).Value = grievanceID;

                    cmd.Parameters.Add("@deptID",
                        SqlDbType.Int).Value = deptID;

                    if (cmd.ExecuteNonQuery() != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }

                // Preserve the rejection reason in tracking history.
                string trackingDescription =
                    "Grievance Rejected. Reason: " + rejectionReason;

                using (SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO dbo.TrackGrievance
                    (
                        grievanceID,
                        loginID,
                        trackingactivityDescription,
                        trackingactivitydatetime,
                        escalationID
                    )
                    VALUES
                    (
                        @grievanceID,
                        @loginID,
                        @description,
                        GETDATE(),
                        NULL
                    );",
                    con, tran))
                {
                    cmd.Parameters.Add("@grievanceID",
                        SqlDbType.Int).Value = grievanceID;

                    cmd.Parameters.Add("@loginID",
                        SqlDbType.Int).Value = deptHeadID;

                    cmd.Parameters.Add("@description",
                        SqlDbType.NVarChar, 500).Value =
                        trackingDescription;

                    cmd.ExecuteNonQuery();
                }

                // Notify the student in the same transaction.
                string notificationMessage =
                    ("Your grievance #" + grievanceID +
                     " has been rejected. Reason: " +
                     rejectionReason);

                if (notificationMessage.Length > 255)
                {
                    notificationMessage =
                        notificationMessage.Substring(0, 255);
                }

                using (SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO dbo.SystemNotification
                    (
                        recipientRole,
                        recipientID,
                        grievanceID,
                        notificationType,
                        message,
                        createdDate,
                        isRead
                    )
                    VALUES
                    (
                        @recipientRole,
                        @recipientID,
                        @grievanceID,
                        @notificationType,
                        @message,
                        GETDATE(),
                        0
                    );",
                    con, tran))
                {
                    cmd.Parameters.Add("@recipientRole",
                        SqlDbType.NVarChar, 20).Value = "Student";

                    cmd.Parameters.Add("@recipientID",
                        SqlDbType.Int).Value = studentID;

                    cmd.Parameters.Add("@grievanceID",
                        SqlDbType.Int).Value = grievanceID;

                    cmd.Parameters.Add("@notificationType",
                        SqlDbType.NVarChar, 50).Value =
                        "Grievance Rejected";

                    cmd.Parameters.Add("@message",
                        SqlDbType.NVarChar, 255).Value =
                        notificationMessage;

                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
                return 1;
            }
            catch
            {
                try
                {
                    tran.Rollback();
                }
                catch
                {
                    // Preserve the original exception.
                }

                throw;
            }
        }
    }
}

}