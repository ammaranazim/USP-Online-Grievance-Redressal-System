using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class DALStudent
{
    public static int RegisterStudent(MStudent Std)
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
            @"INSERT INTO StudentInfo
            (
                stdRegNo,
                stdFirstName,
                stdLastName,
                stdFName,
                stdCnic,
                stdPhoneNo,
                stdSemester,
                stdProgram,
                stdSession,
                stdDescription,
                stdGender,
                stdAddress,
                stdDateOfBirth,
                stdEnrollmentDate,
                stdCity,
                stdStatus,
                deptID,
                stdEmail
            )
            VALUES
            (
                @stdRegNo,
                @stdFirstName,
                @stdLastName,
                @stdFName,
                @stdCnic,
                @stdPhoneNo,
                @stdSemester,
                @stdProgram,
                @stdSession,
                @stdDescription,
                @stdGender,
                @stdAddress,
                @stdDateOfBirth,
                @stdEnrollmentDate,
                @stdCity,
                @stdStatus,
                @deptID,
                @stdEmail
            )", con);

            cmd.Parameters.Add(new SqlParameter("@stdRegNo", Std.StdRegNo));
            cmd.Parameters.Add(new SqlParameter("@stdFirstName", Std.StdFirstName));
            cmd.Parameters.Add(new SqlParameter("@stdLastName", Std.StdLastName));
            cmd.Parameters.Add(new SqlParameter("@stdFName", Std.StdFName));
            cmd.Parameters.Add(new SqlParameter("@stdCnic", Std.StdCnic));
            cmd.Parameters.Add(new SqlParameter("@stdPhoneNo", Std.StdPhoneNo));
            cmd.Parameters.Add(new SqlParameter("@stdSemester", Std.StdSemester));
            cmd.Parameters.Add(new SqlParameter("@stdProgram", Std.StdProgram));
            cmd.Parameters.Add(new SqlParameter("@stdSession", Std.StdSession));
            cmd.Parameters.Add(new SqlParameter("@stdDescription", Std.StdDescription));
            cmd.Parameters.Add(new SqlParameter("@stdGender", Std.StdGender));
            cmd.Parameters.Add(new SqlParameter("@stdAddress", Std.StdAddress));
            cmd.Parameters.Add(new SqlParameter("@stdDateOfBirth", Std.StdDateOfBirth));
            cmd.Parameters.Add(new SqlParameter("@stdEnrollmentDate", Std.StdEnrollmentDate));
            cmd.Parameters.Add(new SqlParameter("@stdCity", Std.StdCity));
            cmd.Parameters.Add(new SqlParameter("@stdStatus", Std.StdStatus));
            cmd.Parameters.Add(new SqlParameter("@deptID", Std.DeptID));
            cmd.Parameters.Add(new SqlParameter("@stdEmail", Std.StdEmail));

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

    public static int RegisterStudentWithLogin(MStudent Std, string password)
    {
        ConnectionStringSettings conSettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (conSettings == null ||
            string.IsNullOrWhiteSpace(conSettings.ConnectionString))
        {
            throw new Exception("Database Connection Error.");
        }

        using (SqlConnection con =
               new SqlConnection(conSettings.ConnectionString))
        {
            con.Open();

            using (SqlTransaction transaction = con.BeginTransaction())
            {
                try
                {
                    // Check duplicate registration number or email.
                    string duplicateQuery = @"
                    SELECT COUNT(*)
                    FROM StudentInfo
                    WHERE stdRegNo = @stdRegNo
                       OR stdEmail = @stdEmail;";

                    using (SqlCommand cmd = new SqlCommand(
                        duplicateQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue(
                            "@stdRegNo", Std.StdRegNo);

                        cmd.Parameters.AddWithValue(
                            "@stdEmail", Std.StdEmail);

                        int duplicateCount =
                            Convert.ToInt32(cmd.ExecuteScalar());

                        if (duplicateCount > 0)
                        {
                            throw new InvalidOperationException(
                                "Registration number or email already exists.");
                        }
                    }

                    // Insert student profile and retrieve its ID.
                    string studentQuery = @"
                    INSERT INTO StudentInfo
                    (
                        stdRegNo,
                        stdFirstName,
                        stdLastName,
                        stdFName,
                        stdCnic,
                        stdPhoneNo,
                        stdSemester,
                        stdProgram,
                        stdSession,
                        stdDescription,
                        stdGender,
                        stdAddress,
                        stdDateOfBirth,
                        stdEnrollmentDate,
                        stdCity,
                        stdStatus,
                        deptID,
                        stdEmail
                    )
                    VALUES
                    (
                        @stdRegNo,
                        @stdFirstName,
                        @stdLastName,
                        @stdFName,
                        @stdCnic,
                        @stdPhoneNo,
                        @stdSemester,
                        @stdProgram,
                        @stdSession,
                        @stdDescription,
                        @stdGender,
                        @stdAddress,
                        @stdDateOfBirth,
                        @stdEnrollmentDate,
                        @stdCity,
                        @stdStatus,
                        @deptID,
                        @stdEmail
                    );

                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    int stdID;

                    using (SqlCommand cmd = new SqlCommand(
                        studentQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue(
                            "@stdRegNo", Std.StdRegNo);
                        cmd.Parameters.AddWithValue(
                            "@stdFirstName", Std.StdFirstName);
                        cmd.Parameters.AddWithValue(
                            "@stdLastName", Std.StdLastName);
                        cmd.Parameters.AddWithValue(
                            "@stdFName", Std.StdFName);
                        cmd.Parameters.AddWithValue(
                            "@stdCnic", Std.StdCnic);
                        cmd.Parameters.AddWithValue(
                            "@stdPhoneNo", Std.StdPhoneNo);
                        cmd.Parameters.AddWithValue(
                            "@stdSemester", Std.StdSemester);
                        cmd.Parameters.AddWithValue(
                            "@stdProgram", Std.StdProgram);
                        cmd.Parameters.AddWithValue(
                            "@stdSession", Std.StdSession);
                        cmd.Parameters.AddWithValue(
                            "@stdDescription", Std.StdDescription);
                        cmd.Parameters.AddWithValue(
                            "@stdGender", Std.StdGender);
                        cmd.Parameters.AddWithValue(
                            "@stdAddress", Std.StdAddress);
                        cmd.Parameters.AddWithValue(
                            "@stdDateOfBirth", Std.StdDateOfBirth);
                        cmd.Parameters.AddWithValue(
                            "@stdEnrollmentDate", Std.StdEnrollmentDate);
                        cmd.Parameters.AddWithValue(
                            "@stdCity", Std.StdCity);
                        cmd.Parameters.AddWithValue(
                            "@stdStatus", Std.StdStatus);
                        cmd.Parameters.AddWithValue(
                            "@deptID", Std.DeptID);
                        cmd.Parameters.AddWithValue(
                            "@stdEmail", Std.StdEmail);

                        stdID = Convert.ToInt32(cmd.ExecuteScalar());
                    }

// Create the linked student login account.
                    string loginQuery = @"
    INSERT INTO studentLogin
    (
        stdLoginPassword,
        stdID,
        mustChangePassword
    )
    VALUES
    (
        @stdLoginPassword,
        @stdID,
        @mustChangePassword
    );";

using (SqlCommand cmd = new SqlCommand(
    loginQuery, con, transaction))
{
    cmd.Parameters.Add(
        "@stdLoginPassword", SqlDbType.NVarChar, 255)
        .Value = PasswordHelper.HashPassword(password);

    cmd.Parameters.Add("@stdID", SqlDbType.Int)
        .Value = stdID;
    cmd.Parameters.Add("@mustChangePassword", SqlDbType.Bit)
        .Value = true;

    cmd.ExecuteNonQuery();
}

                    transaction.Commit();
                    return 1;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
    public static List<MStudent> GetAllStudentList()
    {
        List<MStudent> lst = new List<MStudent>();
        
        ConnectionStringSettings consettings = ConfigurationManager.ConnectionStrings["GRS"];

        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;
        SqlConnection con = new SqlConnection(constring);
        SqlDataReader dr = null;
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT *  FROM [StudentInfo]", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
               MStudent  std = new MStudent((Convert.ToInt32(dr["stdID"])),dr["stdRegNo"].ToString(),dr["stdFirstName"].ToString(),dr["stdLastName"].ToString(),dr["stdFName"].ToString(),dr["stdCnic"].ToString(),dr["stdPhoneNo"].ToString(),dr["stdSemester"].ToString(),dr["stdProgram"].ToString(),dr["stdSession"].ToString(),dr["stdDescription"].ToString(),dr["stdGender"].ToString(),dr["stdAddress"].ToString(),Convert.ToDateTime(dr["stdDateOfBirth"]),Convert.ToDateTime(dr["stdEnrollmentDate"]),dr["stdCity"].ToString(),dr["stdStatus"].ToString(),Convert.ToInt32( dr["deptID"]),dr["stdEmail"].ToString());
               lst.Add(std);              
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

   
public static int UpdateStudent(MStudent Std)
{
    if (Std == null || Std.StdID <= 0)
        return 0;

    ConnectionStringSettings settings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (settings == null ||
        String.IsNullOrEmpty(settings.ConnectionString))
    {
        throw new Exception("Database Connection Error");
    }

    using (SqlConnection con =
        new SqlConnection(settings.ConnectionString))
    {
        con.Open();

        using (SqlTransaction transaction = con.BeginTransaction())
        {
            try
            {
                string query = @"
                    UPDATE StudentInfo
                    SET
                        stdRegNo = @stdRegNo,
                        stdFirstName = @stdFirstName,
                        stdLastName = @stdLastName,
                        stdFName = @stdFName,
                        stdCnic = @stdCnic,
                        stdPhoneNo = @stdPhoneNo,
                        stdSemester = @stdSemester,
                        stdProgram = @stdProgram,
                        stdSession = @stdSession,
                        stdDescription = @stdDescription,
                        stdGender = @stdGender,
                        stdAddress = @stdAddress,
                        stdDateOfBirth = @stdDateOfBirth,
                        stdEnrollmentDate = @stdEnrollmentDate,
                        stdCity = @stdCity,
                        stdStatus = @stdStatus,
                        deptID = @deptID,
                        stdEmail = @stdEmail
                    WHERE stdID = @stdID";

                using (SqlCommand cmd =
                    new SqlCommand(query, con, transaction))
                {
                    cmd.Parameters.AddWithValue("@stdID", Std.StdID);
                    cmd.Parameters.AddWithValue("@stdRegNo", Std.StdRegNo);
                    cmd.Parameters.AddWithValue("@stdFirstName", Std.StdFirstName);
                    cmd.Parameters.AddWithValue("@stdLastName", Std.StdLastName);
                    cmd.Parameters.AddWithValue("@stdFName", Std.StdFName);
                    cmd.Parameters.AddWithValue("@stdCnic", Std.StdCnic);
                    cmd.Parameters.AddWithValue("@stdPhoneNo", Std.StdPhoneNo);
                    cmd.Parameters.AddWithValue("@stdSemester", Std.StdSemester);
                    cmd.Parameters.AddWithValue("@stdProgram", Std.StdProgram);
                    cmd.Parameters.AddWithValue("@stdSession", Std.StdSession);
                    cmd.Parameters.AddWithValue("@stdDescription", Std.StdDescription);
                    cmd.Parameters.AddWithValue("@stdGender", Std.StdGender);
                    cmd.Parameters.AddWithValue("@stdAddress", Std.StdAddress);
                    cmd.Parameters.AddWithValue("@stdDateOfBirth", Std.StdDateOfBirth);
                    cmd.Parameters.AddWithValue("@stdEnrollmentDate", Std.StdEnrollmentDate);
                    cmd.Parameters.AddWithValue("@stdCity", Std.StdCity);
                    cmd.Parameters.AddWithValue("@stdStatus", Std.StdStatus);
                    cmd.Parameters.AddWithValue("@deptID", Std.DeptID);
                    cmd.Parameters.AddWithValue("@stdEmail", Std.StdEmail);

                    int result = cmd.ExecuteNonQuery();

                    if (result != 1)
                    {
                        throw new Exception(
                            "Student record was not updated.");
                    }

                    transaction.Commit();
                    return result;
                }
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}



    public static int DeleteStudent(int stdID)
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
                "DELETE FROM StudentInfo WHERE stdID=@stdID", con);

            cmd.Parameters.AddWithValue("@stdID", stdID);

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

    public static DataTable SearchStudent(string keyword)
    {
        DataTable dt = new DataTable();

        string connStr = ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connStr))
        {
            string query = @"
        SELECT * FROM StudentInfo
        WHERE stdRegNo LIKE '%' + @keyword + '%'
        OR stdFirstName LIKE '%' + @keyword + '%'
        OR stdCnic LIKE '%' + @keyword + '%'
        OR stdPhoneNo LIKE '%' + @keyword + '%'
        OR stdCity LIKE '%' + @keyword + '%'
        OR stdEmail LIKE '%' + @keyword + '%'";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@keyword", keyword);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
        }

        return dt;
    }
    public static DataTable GetAllStudents()
    {
        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;

        SqlConnection con = new SqlConnection(constring);

        DataTable dt = new DataTable();

        try
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT stdID, stdRegNo FROM StudentInfo ORDER BY stdRegNo", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dt);
        }
        finally
        {
            con.Close();
        }

        return dt;
    }

    public static MStudent GetStudentByID(int stdID)
    {
        MStudent std = null;

        ConnectionStringSettings consettings = ConfigurationManager.ConnectionStrings["GRS"];

        if (string.IsNullOrEmpty(consettings.ConnectionString))
            throw new Exception("Database Connection Error");

        string constring = consettings.ConnectionString;

        SqlConnection con = new SqlConnection(constring);
        SqlDataReader dr = null;

        try
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM StudentInfo WHERE stdID=@stdID", con);
            cmd.Parameters.AddWithValue("@stdID", stdID);

            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                std = new MStudent(
                    Convert.ToInt32(dr["stdID"]),
                    dr["stdRegNo"].ToString(),
                    dr["stdFirstName"].ToString(),
                    dr["stdLastName"].ToString(),
                    dr["stdFName"].ToString(),
                    dr["stdCnic"].ToString(),
                    dr["stdPhoneNo"].ToString(),
                    dr["stdSemester"].ToString(),
                    dr["stdProgram"].ToString(),
                    dr["stdSession"].ToString(),
                    dr["stdDescription"].ToString(),
                    dr["stdGender"].ToString(),
                    dr["stdAddress"].ToString(),
                    Convert.ToDateTime(dr["stdDateOfBirth"]),
                    Convert.ToDateTime(dr["stdEnrollmentDate"]),
                    dr["stdCity"].ToString(),
                    dr["stdStatus"].ToString(),
                    Convert.ToInt32(dr["deptID"]),
                    dr["stdEmail"].ToString()
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

        return std;
    }
    public static string GetStudentNameByID(int stdID)
    {
        string studentName = "";

        string connStr =
            System.Configuration.ConfigurationManager
            .ConnectionStrings["GRS"].ConnectionString;

        using (System.Data.SqlClient.SqlConnection con =
               new System.Data.SqlClient.SqlConnection(connStr))
        {
            string query = @"
            SELECT 
                stdFirstName + ' ' + ISNULL(stdLastName, '') 
            FROM StudentInfo
            WHERE stdID = @stdID";

            using (System.Data.SqlClient.SqlCommand cmd =
                   new System.Data.SqlClient.SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@stdID", stdID);

                con.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    studentName = result.ToString().Trim();
                }
            }
        }

        return studentName;
    }
    public static DataTable GetStudentsForAdmin(
    string keyword,
    int departmentID,
    string status,
    string program,
    string semester)
    {
        DataTable dt = new DataTable();

        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        string query = @"
        SELECT
            s.stdID,
            s.stdRegNo,
            s.stdFirstName,
            s.stdLastName,
            s.stdEmail,
            s.stdProgram,
            s.stdSemester,
            s.stdStatus,
            s.deptID,
            d.deptName
        FROM StudentInfo AS s
        LEFT JOIN Department AS d
            ON s.deptID = d.deptID
        WHERE
            (
                @keyword = ''
                OR s.stdRegNo LIKE @searchPattern
                OR s.stdFirstName LIKE @searchPattern
                OR s.stdLastName LIKE @searchPattern
                OR s.stdCnic LIKE @searchPattern
                OR s.stdPhoneNo LIKE @searchPattern
                OR s.stdCity LIKE @searchPattern
                OR s.stdEmail LIKE @searchPattern
            )
            AND (@departmentID = 0 OR s.deptID = @departmentID)
            AND (
                @status = ''
                OR LOWER(ISNULL(s.stdStatus, '')) = LOWER(@status)
            )
            AND (
                @program = ''
                OR ISNULL(s.stdProgram, '') = @program
            )
            AND (
                @semester = ''
                OR ISNULL(s.stdSemester, '') = @semester
            )
        ORDER BY s.stdRegNo;";

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(query, con))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            keyword = keyword ?? String.Empty;
            status = status ?? String.Empty;
            program = program ?? String.Empty;
            semester = semester ?? String.Empty;

            cmd.Parameters.Add("@keyword", SqlDbType.NVarChar, 100)
                .Value = keyword;

            cmd.Parameters.Add("@searchPattern", SqlDbType.NVarChar, 202)
                .Value = "%" + keyword + "%";

            cmd.Parameters.Add("@departmentID", SqlDbType.Int)
                .Value = departmentID;

            cmd.Parameters.Add("@status", SqlDbType.NVarChar, 20)
                .Value = status;

            cmd.Parameters.Add("@program", SqlDbType.NVarChar, 100)
                .Value = program;

            cmd.Parameters.Add("@semester", SqlDbType.NVarChar, 50)
                .Value = semester;

            da.Fill(dt);
        }

        return dt;
    }
}