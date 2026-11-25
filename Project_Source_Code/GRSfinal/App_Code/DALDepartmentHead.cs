using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class DALDepartmentHead
{
    public static int RegisterDepartmentHead(MDepartmentHead DeptHead)
    {
        string connStr =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        if (string.IsNullOrWhiteSpace(connStr))
            throw new Exception("Database Connection Error");

        string query = @"
        INSERT INTO DepartmentHead
        (
            deptHeadName,
            deptHeadUserName,
            deptHeadPassword,
            deptHeadEmail,
            deptHeadPhoneNo,
            deptHeadDesignation,
            deptHeadStatus,
            deptID,
            mustChangePassword
        )
        VALUES
        (
            @deptHeadName,
            @deptHeadUserName,
            @deptHeadPassword,
            @deptHeadEmail,
            @deptHeadPhoneNo,
            @deptHeadDesignation,
            @deptHeadStatus,
            @deptID,
            @mustChangePassword
        );";

        using (SqlConnection con = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@deptHeadName", SqlDbType.NVarChar, 50)
                .Value = DeptHead.DeptHeadName;

            cmd.Parameters.Add("@deptHeadUserName", SqlDbType.NVarChar, 50)
                .Value = DeptHead.DeptHeadUserName;

            cmd.Parameters.Add("@deptHeadPassword", SqlDbType.NVarChar, 255)
                .Value = DeptHead.DeptHeadPassword;

            cmd.Parameters.Add("@deptHeadEmail", SqlDbType.NVarChar, 50)
                .Value = DeptHead.DeptHeadEmail;

            cmd.Parameters.Add("@deptHeadPhoneNo", SqlDbType.NVarChar, 50)
                .Value = DeptHead.DeptHeadPhoneNo;

            cmd.Parameters.Add("@deptHeadDesignation", SqlDbType.NVarChar, 50)
                .Value = DeptHead.DeptHeadDesignation;

            cmd.Parameters.Add("@deptHeadStatus", SqlDbType.NVarChar, 50)
                .Value = DeptHead.DeptHeadStatus;

            cmd.Parameters.Add("@deptID", SqlDbType.Int)
                .Value = DeptHead.DeptID;

            cmd.Parameters.Add("@mustChangePassword", SqlDbType.Bit)
                .Value = true;

            con.Open();

            return cmd.ExecuteNonQuery();
        }
    }

    public static List<MDepartmentHead> GetAllDepartmentHeadsList()
    {
        List<MDepartmentHead> lst = new List<MDepartmentHead>();

        ConnectionStringSettings consettings = ConfigurationManager.ConnectionStrings["GRS"];

        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;

        SqlConnection con = new SqlConnection(constring);

        SqlDataReader dr = null;

        try
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM DepartmentHead", con);

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                MDepartmentHead obj = new MDepartmentHead(

                    Convert.ToInt32(dr["deptHeadID"]),
                    dr["deptHeadName"].ToString(),
                    dr["deptHeadUserName"].ToString(),
                    dr["deptHeadPassword"].ToString(),
                    dr["deptHeadEmail"].ToString(),
                    dr["deptHeadPhoneNo"].ToString(),
                    dr["deptHeadDesignation"].ToString(),
                    dr["deptHeadStatus"].ToString(),
                    Convert.ToInt32(dr["deptID"])

                    );

                lst.Add(obj);
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
    public static int UpdateDepartmentHead(MDepartmentHead DeptHead)
    {
        int result = 0;

        ConnectionStringSettings consettings = ConfigurationManager.ConnectionStrings["GRS"];

        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;

        SqlConnection con = new SqlConnection(constring);

        try
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
            @"UPDATE DepartmentHead
        SET
            deptHeadName=@deptHeadName,
            deptHeadUserName=@deptHeadUserName,
            deptHeadPassword=@deptHeadPassword,
            deptHeadEmail=@deptHeadEmail,
            deptHeadPhoneNo=@deptHeadPhoneNo,
            deptHeadDesignation=@deptHeadDesignation,
            deptHeadStatus=@deptHeadStatus,
            deptID=@deptID
        WHERE deptHeadID=@deptHeadID", con);

            cmd.Parameters.AddWithValue("@deptHeadID", DeptHead.DeptHeadID);
            cmd.Parameters.AddWithValue("@deptHeadName", DeptHead.DeptHeadName);
            cmd.Parameters.AddWithValue("@deptHeadUserName", DeptHead.DeptHeadUserName);
            cmd.Parameters.AddWithValue("@deptHeadPassword", DeptHead.DeptHeadPassword);
            cmd.Parameters.AddWithValue("@deptHeadEmail", DeptHead.DeptHeadEmail);
            cmd.Parameters.AddWithValue("@deptHeadPhoneNo", DeptHead.DeptHeadPhoneNo);
            cmd.Parameters.AddWithValue("@deptHeadDesignation", DeptHead.DeptHeadDesignation);
            cmd.Parameters.AddWithValue("@deptHeadStatus", DeptHead.DeptHeadStatus);
            cmd.Parameters.AddWithValue("@deptID", DeptHead.DeptID);

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
    public static int DeleteDepartmentHead(int deptHeadID)
    {
        int result = 0;

        ConnectionStringSettings consettings = ConfigurationManager.ConnectionStrings["GRS"];

        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;

        SqlConnection con = new SqlConnection(constring);

        try
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
                "DELETE FROM DepartmentHead WHERE deptHeadID=@deptHeadID", con);

            cmd.Parameters.AddWithValue("@deptHeadID", deptHeadID);

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
    public static DataTable SearchDepartmentHead(string keyword)
    {
        DataTable dt = new DataTable();

        string connStr = ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connStr))
        {
            string query = @"
SELECT
    DH.deptHeadID,
    DH.deptHeadName,
    DH.deptHeadUserName,
    DH.deptHeadEmail,
    DH.deptHeadPhoneNo,
    DH.deptHeadDesignation,
    DH.deptHeadStatus,
    DH.deptID,
    D.deptName
FROM DepartmentHead DH
INNER JOIN Department D
ON DH.deptID = D.deptID
WHERE
    DH.deptHeadName LIKE '%' + @keyword + '%'
    OR DH.deptHeadUserName LIKE '%' + @keyword + '%'
    OR DH.deptHeadEmail LIKE '%' + @keyword + '%'
    OR DH.deptHeadPhoneNo LIKE '%' + @keyword + '%'
    OR D.deptName LIKE '%' + @keyword + '%'";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@keyword", keyword);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
        }

        return dt;
    }
    public static MDepartmentHead GetDepartmentHeadByID(int deptHeadID)
    {
        MDepartmentHead obj = null;

        ConnectionStringSettings consettings = ConfigurationManager.ConnectionStrings["GRS"];

        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;

        SqlConnection con = new SqlConnection(constring);

        SqlDataReader dr = null;

        try
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM DepartmentHead WHERE deptHeadID=@deptHeadID", con);

            cmd.Parameters.AddWithValue("@deptHeadID", deptHeadID);

            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                obj = new MDepartmentHead(
                    Convert.ToInt32(dr["deptHeadID"]),
                    dr["deptHeadName"].ToString(),
                    dr["deptHeadUserName"].ToString(),
                    dr["deptHeadPassword"].ToString(),
                    dr["deptHeadEmail"].ToString(),
                    dr["deptHeadPhoneNo"].ToString(),
                    dr["deptHeadDesignation"].ToString(),
                    dr["deptHeadStatus"].ToString(),
                    Convert.ToInt32(dr["deptID"])
                );
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

        return obj;
    }
    public static DataTable GetAdminDepartmentHeads(
    string keyword,
    string departmentFilter,
    string statusFilter)
    {
        DataTable dt = new DataTable();

        string connStr =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        string query = @"
        SELECT
            DH.deptHeadID,
            DH.deptHeadName,
            DH.deptHeadUserName,
            DH.deptHeadEmail,
            DH.deptHeadPhoneNo,
            DH.deptHeadDesignation,
            DH.deptHeadStatus,
            DH.deptID,
            D.deptName
        FROM DepartmentHead DH
        INNER JOIN Department D
            ON DH.deptID = D.deptID
        WHERE
            (
                @keyword = ''
                OR DH.deptHeadName LIKE '%' + @keyword + '%'
                OR DH.deptHeadUserName LIKE '%' + @keyword + '%'
                OR DH.deptHeadEmail LIKE '%' + @keyword + '%'
                OR DH.deptHeadPhoneNo LIKE '%' + @keyword + '%'
                OR D.deptName LIKE '%' + @keyword + '%'
            )
            AND
            (
                @departmentID = 0
                OR DH.deptID = @departmentID
            )
            AND
            (
                @status = ''
                OR LOWER(DH.deptHeadStatus) = LOWER(@status)
            )
        ORDER BY DH.deptHeadID DESC;";

        using (SqlConnection con = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            int departmentID = 0;
            int.TryParse(departmentFilter, out departmentID);

            cmd.Parameters.Add("@keyword", SqlDbType.NVarChar, 100)
                .Value = keyword ?? "";

            cmd.Parameters.Add("@departmentID", SqlDbType.Int)
                .Value = departmentID;

            cmd.Parameters.Add("@status", SqlDbType.NVarChar, 50)
                .Value = statusFilter ?? "";

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }

        return dt;
    }

    public static DataTable GetDepartmentHeadStatusCounts()
    {
        DataTable dt = new DataTable();

        string connStr =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        string query = @"
        SELECT
            COUNT(*) AS TotalCount,
            COALESCE(SUM(
                CASE WHEN LOWER(deptHeadStatus) = 'active'
                     THEN 1 ELSE 0 END
            ), 0) AS ActiveCount,
            COALESCE(SUM(
                CASE WHEN LOWER(deptHeadStatus) = 'inactive'
                     THEN 1 ELSE 0 END
            ), 0) AS InactiveCount
        FROM DepartmentHead;";

        using (SqlConnection con = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(query, con))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }

        return dt;
    }

    public static DataTable GetDepartmentHeadDepartments()
    {
        DataTable dt = new DataTable();

        string connStr =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        string query = @"
        SELECT DISTINCT D.deptID, D.deptName
        FROM Department D
        INNER JOIN DepartmentHead DH
            ON DH.deptID = D.deptID
        ORDER BY D.deptName;";

        using (SqlConnection con = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(query, con))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }

        return dt;
    }
    public static bool UpdateDepartmentHeadStatus(int deptHeadID, string status)
    {
        if (status != "active" && status != "inactive")
        {
            return false;
        }

        string connStr =
            System.Configuration.ConfigurationManager
            .ConnectionStrings["GRS"].ConnectionString;

        string query = @"
        UPDATE DepartmentHead
        SET deptHeadStatus = @status
        WHERE deptHeadID = @deptHeadID";

        using (System.Data.SqlClient.SqlConnection con =
            new System.Data.SqlClient.SqlConnection(connStr))
        using (System.Data.SqlClient.SqlCommand cmd =
            new System.Data.SqlClient.SqlCommand(query, con))
        {
            cmd.Parameters.Add("@status",
                System.Data.SqlDbType.NVarChar, 50).Value = status;

            cmd.Parameters.Add("@deptHeadID",
                System.Data.SqlDbType.Int).Value = deptHeadID;

            con.Open();

            return cmd.ExecuteNonQuery() == 1;
        }
    }
    public static bool DepartmentHeadUsernameExists(string username)
    {
        string connStr =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        string query = @"
        SELECT COUNT(*)
        FROM DepartmentHead
        WHERE deptHeadUserName = @username";

        using (SqlConnection con = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50)
                .Value = username.Trim();

            con.Open();

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
    }

    public static bool DepartmentHeadEmailExists(string email)
    {
        string connStr =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        string query = @"
        SELECT COUNT(*)
        FROM DepartmentHead
        WHERE deptHeadEmail = @email";

        using (SqlConnection con = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50)
                .Value = email.Trim();

            con.Open();

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
    }
    public static MDepartmentHead ValidateLogin(string username, string password)
    {
        string connStr =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        if (string.IsNullOrWhiteSpace(connStr))
            throw new Exception("Database Connection Error");

        string query = @"
        SELECT
            deptHeadID,
            deptHeadName,
            deptHeadUserName,
            deptHeadPassword,
            deptHeadEmail,
            deptHeadPhoneNo,
            deptHeadDesignation,
            deptHeadStatus,
            deptID,
            mustChangePassword
        FROM DepartmentHead
        WHERE deptHeadUserName = @username;";

        using (SqlConnection con = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50)
                .Value = username.Trim();

            con.Open();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (!dr.Read())
                    return null;

                // Inactive accounts cannot log in.
                string status = dr["deptHeadStatus"].ToString();

                if (!string.Equals(
                    status, "active",
                    StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                string storedPasswordHash =
                    dr["deptHeadPassword"].ToString();

                // Verify the submitted password against the stored hash.
                if (!PasswordHelper.VerifyPassword(
                    password, storedPasswordHash))
                {
                    return null;
                }

                MDepartmentHead hod = new MDepartmentHead(
                    Convert.ToInt32(dr["deptHeadID"]),
                    dr["deptHeadName"].ToString(),
                    dr["deptHeadUserName"].ToString(),
                    storedPasswordHash,
                    dr["deptHeadEmail"].ToString(),
                    dr["deptHeadPhoneNo"].ToString(),
                    dr["deptHeadDesignation"].ToString(),
                    status,
                    Convert.ToInt32(dr["deptID"])
                );

                hod.MustChangePassword =
                    Convert.ToBoolean(dr["mustChangePassword"]);

                return hod;
            }
        }
    }
    public static bool ChangeDepartmentHeadPassword(
    int deptHeadID,
    string currentPassword,
    string newPassword)
    {
        using (System.Data.SqlClient.SqlConnection con =
            new System.Data.SqlClient.SqlConnection(
                System.Configuration.ConfigurationManager
                    .ConnectionStrings["GRS"].ConnectionString))
        {
            con.Open();

            using (System.Data.SqlClient.SqlTransaction transaction =
                con.BeginTransaction())
            {
                try
                {
                    string storedPassword = "";
                    string status = "";

                    using (System.Data.SqlClient.SqlCommand cmd =
                        new System.Data.SqlClient.SqlCommand(
                            @"SELECT deptHeadPassword, deptHeadStatus
                          FROM DepartmentHead WITH (UPDLOCK, ROWLOCK)
                          WHERE deptHeadID = @deptHeadID",
                            con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@deptHeadID", deptHeadID);

                        using (System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (!dr.Read())
                            {
                                transaction.Rollback();
                                return false;
                            }

                            storedPassword = dr["deptHeadPassword"].ToString();
                            status = dr["deptHeadStatus"].ToString();
                        }
                    }

                    if (!status.Equals("active",
                        StringComparison.OrdinalIgnoreCase) ||
                        !PasswordHelper.VerifyPassword(currentPassword, storedPassword))
                    {
                        transaction.Rollback();
                        return false;
                    }

                    string newPasswordHash = PasswordHelper.HashPassword(newPassword);

                    using (System.Data.SqlClient.SqlCommand cmd =
                        new System.Data.SqlClient.SqlCommand(
                            @"UPDATE DepartmentHead
                          SET deptHeadPassword = @newPassword,
                              mustChangePassword = 0
                          WHERE deptHeadID = @deptHeadID
                            AND LOWER(deptHeadStatus) = 'active'",
                            con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@newPassword", newPasswordHash);
                        cmd.Parameters.AddWithValue("@deptHeadID", deptHeadID);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected != 1)
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
    public static bool UpdateDepartmentHeadProfile(MDepartmentHead hod)
    {
        string connStr =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(@"
        UPDATE DepartmentHead
        SET
            deptHeadName = @name,
            deptHeadUserName = @username,
            deptHeadEmail = @email,
            deptHeadPhoneNo = @phone,
            deptHeadDesignation = @designation,
            deptHeadStatus = @status,
            deptID = @deptID
        WHERE deptHeadID = @id;", con))
        {
            cmd.Parameters.Add("@id", SqlDbType.Int)
                .Value = hod.DeptHeadID;

            cmd.Parameters.Add("@name", SqlDbType.NVarChar, 50)
                .Value = hod.DeptHeadName;

            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50)
                .Value = hod.DeptHeadUserName;

            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50)
                .Value = hod.DeptHeadEmail;

            cmd.Parameters.Add("@phone", SqlDbType.NVarChar, 50)
                .Value = hod.DeptHeadPhoneNo;

            cmd.Parameters.Add("@designation", SqlDbType.NVarChar, 50)
                .Value = hod.DeptHeadDesignation;

            cmd.Parameters.Add("@status", SqlDbType.NVarChar, 50)
                .Value = hod.DeptHeadStatus;

            cmd.Parameters.Add("@deptID", SqlDbType.Int)
                .Value = hod.DeptID;

            con.Open();

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}