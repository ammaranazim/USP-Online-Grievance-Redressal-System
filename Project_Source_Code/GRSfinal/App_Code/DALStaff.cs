using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class DALStaff
{
    private static string ConnectionString
    {
        get
        {
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings["GRS"];

            if (settings == null ||
                string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new Exception("Database connection 'GRS' was not found.");
            }

            return settings.ConnectionString;
        }
    }

    public static DataTable GetStaffStatusCounts()
    {
        DataTable dt = new DataTable();

        using (SqlConnection con = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
            SELECT
                COUNT(*) AS TotalCount,
                COALESCE(SUM(CASE
                    WHEN LOWER(LTRIM(RTRIM(staffStatus))) = 'active'
                    THEN 1 ELSE 0 END), 0) AS ActiveCount,
                COALESCE(SUM(CASE
                    WHEN LOWER(LTRIM(RTRIM(staffStatus))) = 'inactive'
                    THEN 1 ELSE 0 END), 0) AS InactiveCount
            FROM dbo.Staff;", con))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }

        return dt;
    }

    public static DataTable GetAdminStaff(
        string searchText,
        string departmentFilter,
        string statusFilter)
    {
        DataTable dt = new DataTable();

        using (SqlConnection con = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
            SELECT
                s.staffID,
                s.staffName,
                s.staffUserName,
                s.staffEmail,
                s.staffPhoneNo,
                s.staffDesignation,
                s.staffStatus,
                s.deptID,
                d.deptName
            FROM dbo.Staff s
            INNER JOIN dbo.Department d
                ON d.deptID = s.deptID
            WHERE
                (
                    @searchText = ''
                    OR s.staffName LIKE '%' + @searchText + '%'
                    OR s.staffUserName LIKE '%' + @searchText + '%'
                    OR s.staffEmail LIKE '%' + @searchText + '%'
                    OR s.staffDesignation LIKE '%' + @searchText + '%'
                )
                AND
                (
                    @deptID = 0 OR s.deptID = @deptID
                )
                AND
                (
                    @status = ''
                    OR LOWER(LTRIM(RTRIM(s.staffStatus))) = @status
                )
            ORDER BY s.staffName ASC;", con))
        {
            cmd.Parameters.Add("@searchText", SqlDbType.NVarChar, 200)
                .Value = (searchText ?? string.Empty).Trim();

            int deptID;
            if (!int.TryParse(departmentFilter, out deptID))
            {
                deptID = 0;
            }

            cmd.Parameters.Add("@deptID", SqlDbType.Int).Value = deptID;

            cmd.Parameters.Add("@status", SqlDbType.NVarChar, 20)
                .Value = (statusFilter ?? string.Empty).Trim().ToLowerInvariant();

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }

        return dt;
    }

    public static bool UpdateStaffStatus(int staffID, string newStatus)
    {
        if (newStatus != "Active" && newStatus != "Inactive")
        {
            return false;
        }

        using (SqlConnection con = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
            UPDATE dbo.Staff
            SET staffStatus = @status
            WHERE staffID = @staffID;", con))
        {
            cmd.Parameters.Add("@status", SqlDbType.NVarChar, 20)
                .Value = newStatus;

            cmd.Parameters.Add("@staffID", SqlDbType.Int).Value = staffID;

            con.Open();

            return cmd.ExecuteNonQuery() == 1;
        }
    }

    public static bool IsStaffUsernameExists(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return false;
        }

        using (SqlConnection con = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
        SELECT COUNT(1)
        FROM dbo.Staff
        WHERE staffUserName = @username;", con))
        {
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50)
                .Value = username.Trim();

            con.Open();

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
    }

    public static int RegisterStaff(MStaff staff, string temporaryPassword)
    {
        if (staff == null ||
            string.IsNullOrWhiteSpace(staff.StaffName) ||
            string.IsNullOrWhiteSpace(staff.StaffUserName) ||
            string.IsNullOrWhiteSpace(staff.StaffEmail) ||
            string.IsNullOrWhiteSpace(staff.StaffPhoneNo) ||
            string.IsNullOrWhiteSpace(staff.StaffDesignation) ||
            string.IsNullOrWhiteSpace(temporaryPassword) ||
            staff.DeptID <= 0)
        {
            return 0;
        }

        string passwordHash = PasswordHelper.HashPassword(temporaryPassword);

        using (SqlConnection con = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
        INSERT INTO dbo.Staff
        (
            staffName,
            staffUserName,
            staffPasswordHash,
            staffEmail,
            staffPhoneNo,
            staffDesignation,
            staffStatus,
            deptID,
            mustChangePassword
        )
        VALUES
        (
            @staffName,
            @staffUserName,
            @passwordHash,
            @staffEmail,
            @staffPhoneNo,
            @staffDesignation,
            'Active',
            @deptID,
            1
        );", con))
        {
            cmd.Parameters.Add("@staffName", SqlDbType.NVarChar, 100)
                .Value = staff.StaffName.Trim();

            cmd.Parameters.Add("@staffUserName", SqlDbType.NVarChar, 50)
                .Value = staff.StaffUserName.Trim();

            cmd.Parameters.Add("@passwordHash", SqlDbType.NVarChar, 255)
                .Value = passwordHash;

            cmd.Parameters.Add("@staffEmail", SqlDbType.NVarChar, 100)
                .Value = staff.StaffEmail.Trim();

            cmd.Parameters.Add("@staffPhoneNo", SqlDbType.NVarChar, 50)
                .Value = staff.StaffPhoneNo.Trim();

            cmd.Parameters.Add("@staffDesignation", SqlDbType.NVarChar, 50)
                .Value = staff.StaffDesignation.Trim();

            cmd.Parameters.Add("@deptID", SqlDbType.Int)
                .Value = staff.DeptID;

            con.Open();

            return cmd.ExecuteNonQuery();
        }
    }

    public static MStaff GetStaffByID(int staffID)
    {
        if (staffID <= 0)
            return null;

        using (SqlConnection con = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
            SELECT
                s.staffID,
                s.staffName,
                s.staffUserName,
                s.staffEmail,
                s.staffPhoneNo,
                s.staffDesignation,
                s.staffStatus,
                s.deptID,
                s.mustChangePassword,
                d.deptName
            FROM dbo.Staff s
            INNER JOIN dbo.Department d
                ON d.deptID = s.deptID
            WHERE s.staffID = @staffID;", con))
        {
            cmd.Parameters.Add("@staffID", SqlDbType.Int).Value = staffID;

            con.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (!reader.Read())
                    return null;

                return new MStaff(
                    Convert.ToInt32(reader["staffID"]),
                    Convert.ToString(reader["staffName"]),
                    Convert.ToString(reader["staffUserName"]),
                    Convert.ToString(reader["staffEmail"]),
                    Convert.ToString(reader["staffPhoneNo"]),
                    Convert.ToString(reader["staffDesignation"]),
                    Convert.ToString(reader["staffStatus"]),
                    Convert.ToInt32(reader["deptID"]),
                    Convert.ToBoolean(reader["mustChangePassword"]),
                    Convert.ToString(reader["deptName"])
                );
            }
        }
    }

    public static bool IsStaffUsernameExists(
        string username, int excludeStaffID)
    {
        if (string.IsNullOrWhiteSpace(username))
            return false;

        using (SqlConnection con = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
            SELECT COUNT(1)
            FROM dbo.Staff
            WHERE staffUserName = @username
              AND staffID <> @staffID;", con))
        {
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50)
                .Value = username.Trim();

            cmd.Parameters.Add("@staffID", SqlDbType.Int)
                .Value = excludeStaffID;

            con.Open();

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
    }

    public static bool UpdateStaff(MStaff staff)
    {
        if (staff == null ||
            staff.StaffID <= 0 ||
            string.IsNullOrWhiteSpace(staff.StaffName) ||
            string.IsNullOrWhiteSpace(staff.StaffUserName) ||
            string.IsNullOrWhiteSpace(staff.StaffEmail) ||
            string.IsNullOrWhiteSpace(staff.StaffPhoneNo) ||
            string.IsNullOrWhiteSpace(staff.StaffDesignation) ||
            staff.DeptID <= 0 ||
            (staff.StaffStatus != "Active" &&
             staff.StaffStatus != "Inactive"))
        {
            return false;
        }

        if (IsStaffUsernameExists(
            staff.StaffUserName, staff.StaffID))
        {
            return false;
        }

        using (SqlConnection con = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
            UPDATE dbo.Staff
            SET
                staffName = @staffName,
                staffUserName = @staffUserName,
                staffEmail = @staffEmail,
                staffPhoneNo = @staffPhoneNo,
                staffDesignation = @staffDesignation,
                staffStatus = @staffStatus,
                deptID = @deptID
            WHERE staffID = @staffID;", con))
        {
            cmd.Parameters.Add("@staffName", SqlDbType.NVarChar, 100)
                .Value = staff.StaffName.Trim();

            cmd.Parameters.Add("@staffUserName", SqlDbType.NVarChar, 50)
                .Value = staff.StaffUserName.Trim();

            cmd.Parameters.Add("@staffEmail", SqlDbType.NVarChar, 100)
                .Value = staff.StaffEmail.Trim();

            cmd.Parameters.Add("@staffPhoneNo", SqlDbType.NVarChar, 50)
                .Value = staff.StaffPhoneNo.Trim();

            cmd.Parameters.Add("@staffDesignation", SqlDbType.NVarChar, 50)
                .Value = staff.StaffDesignation.Trim();

            cmd.Parameters.Add("@staffStatus", SqlDbType.NVarChar, 20)
                .Value = staff.StaffStatus;

            cmd.Parameters.Add("@deptID", SqlDbType.Int)
                .Value = staff.DeptID;

            cmd.Parameters.Add("@staffID", SqlDbType.Int)
                .Value = staff.StaffID;

            con.Open();

            return cmd.ExecuteNonQuery() == 1;
        }
    }

    public static MStaff ValidateLogin(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrEmpty(password))
        {
            return null;
        }

        using (SqlConnection con = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
        SELECT
            s.staffID,
            s.staffName,
            s.staffUserName,
            s.staffPasswordHash,
            s.staffEmail,
            s.staffPhoneNo,
            s.staffDesignation,
            s.staffStatus,
            s.deptID,
            s.mustChangePassword,
            d.deptName
        FROM dbo.Staff s
        INNER JOIN dbo.Department d
            ON d.deptID = s.deptID
        WHERE s.staffUserName = @username;", con))
        {
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50)
                .Value = username.Trim();

            con.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (!reader.Read())
                    return null;

                string status = Convert.ToString(reader["staffStatus"]);

                if (!string.Equals(
                    status.Trim(), "Active",
                    StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                string passwordHash =
                    Convert.ToString(reader["staffPasswordHash"]);

                if (!PasswordHelper.VerifyPassword(password, passwordHash))
                    return null;

                return new MStaff(
                    Convert.ToInt32(reader["staffID"]),
                    Convert.ToString(reader["staffName"]),
                    Convert.ToString(reader["staffUserName"]),
                    Convert.ToString(reader["staffEmail"]),
                    Convert.ToString(reader["staffPhoneNo"]),
                    Convert.ToString(reader["staffDesignation"]),
                    status,
                    Convert.ToInt32(reader["deptID"]),
                    Convert.ToBoolean(reader["mustChangePassword"]),
                    Convert.ToString(reader["deptName"])
                );
            }
        }
    }

    public static bool ChangeStaffPassword(
        int staffID,
        string currentPassword,
        string newPassword)
    {
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();

            using (SqlTransaction transaction = con.BeginTransaction())
            {
                try
                {
                    string storedPassword = "";
                    string status = "";

                    using (SqlCommand cmd = new SqlCommand(@"
                    SELECT staffPasswordHash, staffStatus
                    FROM dbo.Staff WITH (UPDLOCK, ROWLOCK)
                    WHERE staffID = @staffID;",
                        con, transaction))
                    {
                        cmd.Parameters.Add("@staffID", SqlDbType.Int)
                            .Value = staffID;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                transaction.Rollback();
                                return false;
                            }

                            storedPassword =
                                Convert.ToString(reader["staffPasswordHash"]);

                            status = Convert.ToString(reader["staffStatus"]);
                        }
                    }

                    if (!string.Equals(
                            status.Trim(), "Active",
                            StringComparison.OrdinalIgnoreCase) ||
                        !PasswordHelper.VerifyPassword(
                            currentPassword, storedPassword))
                    {
                        transaction.Rollback();
                        return false;
                    }

                    string newPasswordHash =
                        PasswordHelper.HashPassword(newPassword);

                    using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE dbo.Staff
                    SET staffPasswordHash = @passwordHash,
                        mustChangePassword = 0
                    WHERE staffID = @staffID
                      AND LOWER(LTRIM(RTRIM(staffStatus))) = 'active';",
                        con, transaction))
                    {
                        cmd.Parameters.Add("@passwordHash", SqlDbType.NVarChar, 255)
                            .Value = newPasswordHash;

                        cmd.Parameters.Add("@staffID", SqlDbType.Int)
                            .Value = staffID;

                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }


}