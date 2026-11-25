using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class DALStudentLogin
{
  
public static int RegisterStudent(MstudentLogin Std)
{
    if (Std == null || Std.StdID <= 0 ||
        String.IsNullOrWhiteSpace(Std.StdLoginPassword))
    {
        return 0;
    }

    ConnectionStringSettings settings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (settings == null ||
        String.IsNullOrWhiteSpace(settings.ConnectionString))
    {
        throw new Exception("Database Connection Error.");
    }

    using (SqlConnection con =
        new SqlConnection(settings.ConnectionString))
    using (SqlCommand cmd = new SqlCommand(@"
        INSERT INTO studentLogin
            (stdLoginPassword, stdID)
        VALUES
            (@password, @stdID);", con))
    {
        cmd.Parameters.Add("@password", SqlDbType.NVarChar, 255)
          .Value = PasswordHelper.HashPassword(Std.StdLoginPassword);

        cmd.Parameters.Add("@stdID", SqlDbType.Int)
            .Value = Std.StdID;

        con.Open();
        return cmd.ExecuteNonQuery();
    }
}

   
public static MstudentLogin ValidateLogin( string regNo, string password)
{
    if (String.IsNullOrWhiteSpace(regNo) ||
        String.IsNullOrEmpty(password))
    {
        return null;
    }

    ConnectionStringSettings settings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (settings == null ||
        String.IsNullOrWhiteSpace(settings.ConnectionString))
    {
        throw new Exception("Database Connection Error.");
    }

    using (SqlConnection con =
        new SqlConnection(settings.ConnectionString))
    {
        string query = @"
            SELECT
    sl.stdLoginID,
    si.stdRegNo,
    sl.stdLoginPassword,
    si.stdEmail,
    si.stdStatus,
    sl.stdID,
    sl.mustChangePassword
            FROM studentLogin AS sl
            INNER JOIN StudentInfo AS si
                ON si.stdID = sl.stdID
            WHERE si.stdRegNo = @regNo
              AND LOWER(ISNULL(si.stdStatus, '')) = 'active';";

        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@regNo", SqlDbType.NVarChar, 100)
                .Value = regNo.Trim();

            con.Open();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (!dr.Read())
                    return null;

                string storedPassword =
                    Convert.ToString(dr["stdLoginPassword"]);

                int loginID = Convert.ToInt32(dr["stdLoginID"]);
                int studentID = Convert.ToInt32(dr["stdID"]);
                string studentRegNo = Convert.ToString(dr["stdRegNo"]);
                string studentEmail = Convert.ToString(dr["stdEmail"]);
                string studentStatus = Convert.ToString(dr["stdStatus"]);

                bool mustChangePassword =
    Convert.ToBoolean(dr["mustChangePassword"]);

                MstudentLogin studentLogin = new MstudentLogin(
                    loginID,
                    storedPassword,
                    studentID,
                    mustChangePassword);

                bool passwordVerified;

                if (IsPasswordHashFormat(storedPassword))
                {
                    passwordVerified =
                        PasswordHelper.VerifyPassword(
                            password, storedPassword);
                }
                else
                {
                    passwordVerified =
                        String.Equals(
                            password,
                            storedPassword,
                            StringComparison.Ordinal);
                }

                if (!passwordVerified)
                    return null;

                if (!IsPasswordHashFormat(storedPassword))
                {
                    string newHash =
                        PasswordHelper.HashPassword(password);

                    dr.Close();

                    string updateQuery = @"
                        UPDATE studentLogin
                        SET stdLoginPassword = @newPassword
                        WHERE stdLoginID = @loginID
                          AND stdID = @stdID
                          AND stdLoginPassword = @oldPassword
                          AND EXISTS
                          (
                              SELECT 1
                              FROM StudentInfo AS si
                              WHERE si.stdID = studentLogin.stdID
                                AND LOWER(ISNULL(si.stdStatus, '')) = 'active'
                          );";

                    using (SqlCommand updateCmd =
                        new SqlCommand(updateQuery, con))
                    {
                        updateCmd.Parameters.Add(
                            "@newPassword", SqlDbType.NVarChar, 255)
                            .Value = newHash;

                        updateCmd.Parameters.Add(
                            "@loginID", SqlDbType.Int)
                            .Value = loginID;

                        updateCmd.Parameters.Add(
                            "@stdID", SqlDbType.Int)
                            .Value = studentID;

                        updateCmd.Parameters.Add(
                            "@oldPassword", SqlDbType.NVarChar, 255)
                            .Value = storedPassword;

                        if (updateCmd.ExecuteNonQuery() != 1)
                            return null;
                    }

                    studentLogin.StdLoginPassword = newHash;
                }

                return studentLogin;
            }
        }
    }
}

    private static bool IsPasswordHashFormat(string storedPassword)
    {
        if (string.IsNullOrWhiteSpace(storedPassword))
        {
            return false;
        }

        string[] parts = storedPassword.Split('.');

        if (parts.Length != 3)
        {
            return false;
        }

        int iterations;

        if (!int.TryParse(parts[0], out iterations) ||
            iterations < 1)
        {
            return false;
        }

        try
        {
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] hash = Convert.FromBase64String(parts[2]);

            return salt.Length == 16 && hash.Length == 32;
        }
        catch (FormatException)
        {
            return false;
        }
    }
public static int ChangeStudentPassword( int loginID, int studentID, string currentPassword, string newPassword)
{
    if (loginID <= 0 ||
        studentID <= 0 ||
        String.IsNullOrEmpty(currentPassword) ||
        String.IsNullOrEmpty(newPassword) ||
        newPassword.Length < 8 ||
        newPassword.Length > 128)
    {
        return 0;
    }

    ConnectionStringSettings settings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (settings == null ||
        String.IsNullOrWhiteSpace(settings.ConnectionString))
    {
        throw new Exception("Database Connection Error.");
    }

    using (SqlConnection con =
        new SqlConnection(settings.ConnectionString))
    {
        con.Open();

        using (SqlTransaction transaction = con.BeginTransaction())
        {
            try
            {
                string selectQuery = @"
                    SELECT sl.stdLoginPassword
                    FROM studentLogin AS sl
                        WITH (UPDLOCK, ROWLOCK)
                    INNER JOIN StudentInfo AS si
                        ON si.stdID = sl.stdID
                    WHERE sl.stdLoginID = @loginID
                      AND sl.stdID = @studentID
                      AND LOWER(ISNULL(si.stdStatus, '')) = 'active';";

                string storedPassword;

                using (SqlCommand cmd =
                    new SqlCommand(selectQuery, con, transaction))
                {
                    cmd.Parameters.Add("@loginID", SqlDbType.Int)
                        .Value = loginID;

                    cmd.Parameters.Add("@studentID", SqlDbType.Int)
                        .Value = studentID;

                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                    {
                        transaction.Rollback();
                        return 0;
                    }

                    storedPassword = Convert.ToString(result);
                }

                bool passwordVerified;

                if (IsPasswordHashFormat(storedPassword))
                {
                    passwordVerified =
                        PasswordHelper.VerifyPassword(
                            currentPassword, storedPassword);
                }
                else
                {
                    passwordVerified = String.Equals(
                        currentPassword,
                        storedPassword,
                        StringComparison.Ordinal);
                }

                if (!passwordVerified)
                {
                    transaction.Rollback();
                    return 0;
                }

                string newPasswordHash =
                    PasswordHelper.HashPassword(newPassword);

                string updateQuery = @"
                    UPDATE studentLogin
                    SET stdLoginPassword = @newPassword,
                        mustChangePassword = 0
                    WHERE stdLoginID = @loginID
                      AND stdID = @studentID
                      AND EXISTS
                      (
                          SELECT 1
                          FROM StudentInfo AS si
                          WHERE si.stdID = studentLogin.stdID
                            AND LOWER(ISNULL(si.stdStatus, '')) = 'active'
                      );";

                using (SqlCommand cmd =
                    new SqlCommand(updateQuery, con, transaction))
                {
                    cmd.Parameters.Add(
                        "@newPassword", SqlDbType.NVarChar, 255)
                        .Value = newPasswordHash;

                    cmd.Parameters.Add("@loginID", SqlDbType.Int)
                        .Value = loginID;

                    cmd.Parameters.Add("@studentID", SqlDbType.Int)
                        .Value = studentID;

                    int rowsUpdated = cmd.ExecuteNonQuery();

                    if (rowsUpdated != 1)
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
                transaction.Rollback();
                throw;
            }
        }
    }
}
    
public static bool? GetMustChangePassword(int loginID, int studentID)
{
    if (loginID <= 0 || studentID <= 0)
        return null;

    ConnectionStringSettings settings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (settings == null ||
        String.IsNullOrWhiteSpace(settings.ConnectionString))
    {
        throw new Exception("Database Connection Error.");
    }

    string query = @"
        SELECT sl.mustChangePassword
        FROM studentLogin AS sl
        INNER JOIN StudentInfo AS si
            ON si.stdID = sl.stdID
        WHERE sl.stdLoginID = @loginID
          AND sl.stdID = @studentID
          AND LOWER(ISNULL(si.stdStatus, '')) = 'active';";

    using (SqlConnection con =
        new SqlConnection(settings.ConnectionString))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        cmd.Parameters.Add("@loginID", SqlDbType.Int).Value = loginID;
        cmd.Parameters.Add("@studentID", SqlDbType.Int).Value = studentID;

        con.Open();

        object result = cmd.ExecuteScalar();

        if (result == null || result == DBNull.Value)
            return null;

        return Convert.ToBoolean(result);
    }
}

public static List<MstudentLogin> SearchStudentLogin()
{
    List<MstudentLogin> list = new List<MstudentLogin>();

    ConnectionStringSettings settings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (settings == null ||
        String.IsNullOrWhiteSpace(settings.ConnectionString))
    {
        throw new Exception("Database Connection Error.");
    }

    string query = @"
        SELECT
            sl.stdLoginID,
            si.stdRegNo,
            si.stdEmail,
            si.stdStatus,
            sl.stdID
        FROM studentLogin AS sl
        INNER JOIN StudentInfo AS si
            ON si.stdID = sl.stdID
        ORDER BY sl.stdLoginID DESC;";

    using (SqlConnection con =
        new SqlConnection(settings.ConnectionString))
    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        con.Open();

        using (SqlDataReader dr = cmd.ExecuteReader())
        {
            while (dr.Read())
            {
                MstudentLogin login = new MstudentLogin(
     Convert.ToInt32(dr["stdLoginID"]),
     String.Empty,
     Convert.ToInt32(dr["stdID"]));
                list.Add(login);
            }
        }
    }

    return list;
}



 
public static int UpdateStudentLogin(MstudentLogin stdlogin)
{
    if (stdlogin == null ||
        stdlogin.StdLoginID <= 0 ||
        stdlogin.StdID <= 0 ||
        String.IsNullOrWhiteSpace(stdlogin.StdLoginPassword))
    {
        return 0;
    }

    ConnectionStringSettings settings =
        ConfigurationManager.ConnectionStrings["GRS"];

    if (settings == null ||
        String.IsNullOrWhiteSpace(settings.ConnectionString))
    {
        throw new Exception("Database Connection Error.");
    }

    using (SqlConnection con =
        new SqlConnection(settings.ConnectionString))
    using (SqlCommand cmd = new SqlCommand(@"
        UPDATE studentLogin
        SET stdLoginPassword = @password
        WHERE stdLoginID = @loginID
          AND stdID = @stdID;", con))
    {
        cmd.Parameters.Add("@password", SqlDbType.NVarChar, 255)
            .Value = PasswordHelper.HashPassword(
                stdlogin.StdLoginPassword);

        cmd.Parameters.Add("@loginID", SqlDbType.Int)
            .Value = stdlogin.StdLoginID;

        cmd.Parameters.Add("@stdID", SqlDbType.Int)
            .Value = stdlogin.StdID;

        con.Open();
        return cmd.ExecuteNonQuery();
    }
}


    public static int DeleteStudentLogin(int stdLoginID)
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
                "DELETE FROM studentLogin WHERE stdLoginID=@stdLoginID", con);

            cmd.Parameters.AddWithValue("@stdLoginID", stdLoginID);

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
}
