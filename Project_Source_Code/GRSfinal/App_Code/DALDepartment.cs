using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class DALDepartment
{
    public static int RegisterDept(MDepartment Dept)
    {
        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Database Connection error");

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(@"
        INSERT INTO Department
        (
            deptName,
            deptCode,
            deptStatus,
            deptDescription,
            deptEmail,
            deptContactNo,
            deptWH,
            deptLocation,
            categoryID
        )
        VALUES
        (
            @deptName,
            @deptCode,
            @deptStatus,
            @deptDescription,
            @deptEmail,
            @deptContactNo,
            @deptWH,
            @deptLocation,
            @categoryID
        )", con))
        {
            cmd.Parameters.AddWithValue("@deptName", Dept.DeptName);
            cmd.Parameters.AddWithValue("@deptCode", Dept.DeptCode);
            cmd.Parameters.AddWithValue("@deptStatus", Dept.DeptStatus);
            cmd.Parameters.AddWithValue("@deptDescription",
                (object)Dept.DeptDescription ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@deptEmail", Dept.DeptEmail);
            cmd.Parameters.AddWithValue("@deptContactNo", Dept.DeptContactNo);
            cmd.Parameters.AddWithValue("@deptWH", Dept.DeptWH);
            cmd.Parameters.AddWithValue("@deptLocation", Dept.DeptLocation);
            cmd.Parameters.AddWithValue("@categoryID", Dept.CategoryID);

            con.Open();
            return cmd.ExecuteNonQuery();
        }
    }
    public static DataTable GetAllDepartments()
    {
        ConnectionStringSettings consettings = ConfigurationManager.ConnectionStrings["GRS"];
        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection error");

        string constring = consettings.ConnectionString;
        SqlConnection con = new SqlConnection(constring);

        DataTable dt = new DataTable();

        try
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT deptID, deptName FROM Department", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
        }
        finally
        {
            con.Close();
        }

        return dt;
    }
    public static DataTable GetDepartmentsByCategory(int categoryID)
    {
        DataTable dt = new DataTable();

        ConnectionStringSettings consettings = ConfigurationManager.ConnectionStrings["GRS"];

        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection error");

        string constring = consettings.ConnectionString;
        SqlConnection con = new SqlConnection(constring);

        try
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT deptID, deptName FROM Department WHERE categoryID = @categoryID ORDER BY deptName",
                con);

            cmd.Parameters.AddWithValue("@categoryID", categoryID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
        }
        finally
        {
            con.Close();
        }

        return dt;
    }

    public static List<MDepartment> GetAllDepartmentList()
    {
        List<MDepartment> departments = new List<MDepartment>();

        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Database Connection error");

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(
            @"SELECT deptID, deptName, deptCode, deptStatus,
                 deptDescription, deptEmail, deptContactNo,
                 deptWH, deptLocation, categoryID
          FROM Department", con))
        {
            con.Open();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    MDepartment dept = new MDepartment(
                        Convert.ToInt32(dr["deptID"]),
                        dr["deptName"].ToString(),
                        dr["deptCode"].ToString(),
                        dr["deptStatus"].ToString(),
                        dr["deptDescription"].ToString(),
                        dr["deptEmail"].ToString(),
                        dr["deptContactNo"].ToString(),
                        dr["deptWH"].ToString(),
                        dr["deptLocation"].ToString(),
                        Convert.ToInt32(dr["categoryID"])
                    );

                    departments.Add(dept);
                }
            }
        }

        return departments;
    }
    public static int UpdateDepartment(MDepartment Dept)
    {
        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Database Connection error");

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(@"
        UPDATE Department
        SET
            deptName = @deptName,
            deptCode = @deptCode,
            deptStatus = @deptStatus,
            deptDescription = @deptDescription,
            deptEmail = @deptEmail,
            deptContactNo = @deptContactNo,
            deptWH = @deptWH,
            deptLocation = @deptLocation,
            categoryID = @categoryID
        WHERE deptID = @deptID", con))
        {
            cmd.Parameters.AddWithValue("@deptID", Dept.DeptID);
            cmd.Parameters.AddWithValue("@deptName", Dept.DeptName);
            cmd.Parameters.AddWithValue("@deptCode", Dept.DeptCode);
            cmd.Parameters.AddWithValue("@deptStatus", Dept.DeptStatus);
            cmd.Parameters.AddWithValue("@deptDescription",
                (object)Dept.DeptDescription ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@deptEmail", Dept.DeptEmail);
            cmd.Parameters.AddWithValue("@deptContactNo", Dept.DeptContactNo);
            cmd.Parameters.AddWithValue("@deptWH", Dept.DeptWH);
            cmd.Parameters.AddWithValue("@deptLocation", Dept.DeptLocation);
            cmd.Parameters.AddWithValue("@categoryID", Dept.CategoryID);

            con.Open();
            return cmd.ExecuteNonQuery();
        }
    }
    public static int DeleteDepartment(int deptID)
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
                "DELETE FROM Department WHERE deptID=@deptID", con);

            cmd.Parameters.AddWithValue("@deptID", deptID);

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
    public static DataTable SearchDepartment(string keyword)
    {
        DataTable dt = new DataTable();

        string connStr = ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connStr))
        {
            string query = @"
        SELECT * FROM Department
        WHERE deptName LIKE '%' + @keyword + '%'
        OR deptCode LIKE '%' + @keyword + '%'
        OR deptStatus LIKE '%' + @keyword + '%'";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@keyword", keyword);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
        }

        return dt;
    }
    public static MDepartment GetDepartmentByID(int deptID)
    {
        MDepartment dept = null;

        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Database Connection error");

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(
            @"SELECT deptID, deptName, deptCode, deptStatus,
                 deptDescription, deptEmail, deptContactNo,
                 deptWH, deptLocation, categoryID
          FROM Department
          WHERE deptID = @deptID", con))
        {
            cmd.Parameters.AddWithValue("@deptID", deptID);

            con.Open();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    dept = new MDepartment(
                        Convert.ToInt32(dr["deptID"]),
                        dr["deptName"].ToString(),
                        dr["deptCode"].ToString(),
                        dr["deptStatus"].ToString(),
                        dr["deptDescription"].ToString(),
                        dr["deptEmail"].ToString(),
                        dr["deptContactNo"].ToString(),
                        dr["deptWH"].ToString(),
                        dr["deptLocation"].ToString(),
                        Convert.ToInt32(dr["categoryID"])
                    );
                }
            }
        }

        return dept;
    }
    public static DataTable GetAdminDepartments(
    string searchText,
    int categoryID,
    string statusFilter)
    {
        DataTable dt = new DataTable();

        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(@"
        SELECT
            d.deptID,
            d.deptName,
            d.deptCode,
            d.deptStatus,
            d.categoryID,
            c.categoryName
        FROM Department d
        LEFT JOIN GrievanceCategory c
            ON d.categoryID = c.categoryID
        WHERE
            (
                @searchText = ''
                OR d.deptName LIKE '%' + @searchText + '%'
                OR d.deptCode LIKE '%' + @searchText + '%'
            )
            AND
            (
                @categoryID = 0
                OR d.categoryID = @categoryID
            )
            AND
            (
                @statusFilter = ''
                OR d.deptStatus = @statusFilter
            )
        ORDER BY d.deptID DESC;", con))
        {
            cmd.Parameters.Add("@searchText", SqlDbType.NVarChar, 100)
                .Value = searchText ?? "";

            cmd.Parameters.Add("@categoryID", SqlDbType.Int)
                .Value = categoryID;

            cmd.Parameters.Add("@statusFilter", SqlDbType.NVarChar, 50)
                .Value = statusFilter ?? "";

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }

        return dt;
    }


    public static DataTable GetDepartmentStatusCounts()
    {
        DataTable dt = new DataTable();

        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(@"
        SELECT
            COUNT(*) AS TotalDepartments,
            SUM(CASE
                WHEN LOWER(LTRIM(RTRIM(deptStatus))) = 'active'
                THEN 1 ELSE 0
            END) AS ActiveDepartments,
            SUM(CASE
                WHEN LOWER(LTRIM(RTRIM(deptStatus))) = 'inactive'
                THEN 1 ELSE 0
            END) AS InactiveDepartments
        FROM Department;", con))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }

        return dt;
    }


    public static int UpdateDepartmentStatus(int deptID, string status)
    {
        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(@"
        UPDATE Department
        SET deptStatus = @status
        WHERE deptID = @deptID;", con))
        {
            cmd.Parameters.Add("@status", SqlDbType.NVarChar, 50)
                .Value = status;

            cmd.Parameters.Add("@deptID", SqlDbType.Int)
                .Value = deptID;

            con.Open();
            return cmd.ExecuteNonQuery();
        }
    }
}