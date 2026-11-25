using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class DALAdminLogin
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
                throw new ConfigurationErrorsException(
                    "GRS connection string is missing or empty.");
            }

            return settings.ConnectionString;
        }
    }

    // ---------------------------------------------------------
    // Check whether a password is already stored as a hash.
    //
    // PasswordHelper stores passwords in this format:
    // iterations.salt.hash
    // ---------------------------------------------------------
    private static bool IsPasswordHash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        string[] parts = password.Split('.');

        if (parts.Length != 3)
        {
            return false;
        }

        int iterations;

        if (!int.TryParse(parts[0], out iterations))
        {
            return false;
        }

        return iterations > 0 &&
               !string.IsNullOrWhiteSpace(parts[1]) &&
               !string.IsNullOrWhiteSpace(parts[2]);
    }

    // ---------------------------------------------------------
    // Prepare Admin parameters.
    //
    // If the password is already hashed, keep it as-is.
    // Otherwise, hash it before saving.
    // ---------------------------------------------------------
    private static void AddAdminParameters(
        SqlCommand cmd, MLogin admin)
    {
        cmd.Parameters.Add("@adminLoginName",
            SqlDbType.NVarChar, 50).Value =
            (object)admin.AdminLoginName ?? DBNull.Value;

        cmd.Parameters.Add("@adminLoginEmail",
            SqlDbType.NVarChar, 50).Value =
            (object)admin.AdminLoginEmail ?? DBNull.Value;

        string password = admin.AdminLoginPassword;

        if (!IsPasswordHash(password))
        {
            password = PasswordHelper.HashPassword(password);
        }

        cmd.Parameters.Add("@adminLoginPassword",
            SqlDbType.NVarChar, 255).Value =
            (object)password ?? DBNull.Value;

        cmd.Parameters.Add("@adminLoginPhoneNo",
            SqlDbType.NVarChar, 50).Value =
            (object)admin.AdminLoginPhoneNo ?? DBNull.Value;

        cmd.Parameters.Add("@adminLoginDescription",
            SqlDbType.NVarChar, 500).Value =
            (object)admin.AdminLoginDescription ?? DBNull.Value;
    }

    // ---------------------------------------------------------
    // Map database record to MLogin object.
    // ---------------------------------------------------------
    private static MLogin MapAdmin(SqlDataReader reader)
    {
        return new MLogin(
            Convert.ToInt32(reader["adminLoginID"]),
            Convert.ToString(reader["adminLoginName"]),
            Convert.ToString(reader["adminLoginEmail"]),
            Convert.ToString(reader["adminLoginPassword"]),
            Convert.ToString(reader["adminLoginPhoneNo"]),
            reader["adminLoginDescription"] == DBNull.Value
                ? null
                : Convert.ToString(reader["adminLoginDescription"])
        );
    }

    // ---------------------------------------------------------
    // Register a new Admin.
    //
    // AddAdminParameters() automatically hashes a new password.
    // ---------------------------------------------------------
    public static int RegisterAdmin(MLogin Admin)
    {
        const string query = @"
            INSERT INTO AdminLogin
            (
                adminLoginName,
                adminLoginEmail,
                adminLoginPassword,
                adminLoginPhoneNo,
                adminLoginDescription
            )
            VALUES
            (
                @adminLoginName,
                @adminLoginEmail,
                @adminLoginPassword,
                @adminLoginPhoneNo,
                @adminLoginDescription
            )";

        using (SqlConnection con =
            new SqlConnection(ConnectionString))
        using (SqlCommand cmd =
            new SqlCommand(query, con))
        {
            AddAdminParameters(cmd, Admin);

            con.Open();

            return cmd.ExecuteNonQuery() > 0 ? 1 : 0;
        }
    }

    // ---------------------------------------------------------
    // Admin Login
    //
    // Supports:
    // 1. Existing plain-text passwords
    // 2. New PBKDF2 hashed passwords
    //
    // Legacy plain-text passwords are automatically upgraded
    // to secure hashes after successful login.
    // ---------------------------------------------------------
    public static MLogin AdminLogin(
        string login,
        string password)
    {
        if (string.IsNullOrWhiteSpace(login) ||
            string.IsNullOrEmpty(password))
        {
            return null;
        }

        const string query = @"
            SELECT
                adminLoginID,
                adminLoginName,
                adminLoginEmail,
                adminLoginPassword,
                adminLoginPhoneNo,
                adminLoginDescription
            FROM AdminLogin
            WHERE
                adminLoginName = @login
                OR adminLoginEmail = @login";

        MLogin admin = null;

        using (SqlConnection con =
            new SqlConnection(ConnectionString))
        using (SqlCommand cmd =
            new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@login",
                SqlDbType.NVarChar, 50).Value = login;

            con.Open();

            using (SqlDataReader reader =
                cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    admin = MapAdmin(reader);
                }
            }
        }

        // No Admin account found.
        if (admin == null)
        {
            return null;
        }

        string storedPassword =
            admin.AdminLoginPassword;

        bool passwordValid = false;
        bool legacyPassword = false;

        // -----------------------------------------------------
        // New secure hashed password
        // -----------------------------------------------------
        if (IsPasswordHash(storedPassword))
        {
            try
            {
                passwordValid =
                    PasswordHelper.VerifyPassword(
                        password,
                        storedPassword);
            }
            catch
            {
                // Invalid/corrupted stored hash.
                passwordValid = false;
            }
        }
        else
        {
            // -------------------------------------------------
            // Legacy plain-text password
            // -------------------------------------------------
            legacyPassword =
                string.Equals(
                    storedPassword,
                    password,
                    StringComparison.Ordinal);
        }

        // -----------------------------------------------------
        // Invalid password
        // -----------------------------------------------------
        if (!passwordValid && !legacyPassword)
        {
            return null;
        }

        // -----------------------------------------------------
        // Upgrade legacy plain-text password to hash.
        // -----------------------------------------------------
        if (legacyPassword)
        {
            string newPasswordHash =
                PasswordHelper.HashPassword(password);

            UpdatePasswordHash(
                admin.AdminLoginID,
                newPasswordHash);

            // Keep the returned object consistent with the
            // database after migration.
            admin.AdminLoginPassword =
                newPasswordHash;
        }

        return admin;
    }

    // ---------------------------------------------------------
    // Update only the Admin password.
    //
    // Used when upgrading an old plain-text password after
    // successful login.
    // ---------------------------------------------------------
    private static int UpdatePasswordHash(
        int adminLoginID,
        string passwordHash)
    {
        const string query = @"
            UPDATE AdminLogin
            SET adminLoginPassword = @adminLoginPassword
            WHERE adminLoginID = @adminLoginID";

        using (SqlConnection con =
            new SqlConnection(ConnectionString))
        using (SqlCommand cmd =
            new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@adminLoginPassword",
                SqlDbType.NVarChar, 255).Value =
                passwordHash;

            cmd.Parameters.Add("@adminLoginID",
                SqlDbType.Int).Value =
                adminLoginID;

            con.Open();

            return cmd.ExecuteNonQuery();
        }
    }

    // ---------------------------------------------------------
    // Search / retrieve all Admin accounts.
    // ---------------------------------------------------------
    public static List<MLogin> SearchAdminLogin()
    {
        const string query = @"
            SELECT
                adminLoginID,
                adminLoginName,
                adminLoginEmail,
                adminLoginPassword,
                adminLoginPhoneNo,
                adminLoginDescription
            FROM AdminLogin
            ORDER BY adminLoginName";

        List<MLogin> admins =
            new List<MLogin>();

        using (SqlConnection con =
            new SqlConnection(ConnectionString))
        using (SqlCommand cmd =
            new SqlCommand(query, con))
        {
            con.Open();

            using (SqlDataReader reader =
                cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    admins.Add(MapAdmin(reader));
                }
            }
        }

        return admins;
    }

    // ---------------------------------------------------------
    // Update Admin information.
    //
    // AddAdminParameters() ensures:
    // - Plain-text new password -> hashed
    // - Existing hash -> kept unchanged
    // ---------------------------------------------------------
    public static int UpdateAdminLogin(
        MLogin adminlogin)
    {
        const string query = @"
            UPDATE AdminLogin
            SET
                adminLoginName = @adminLoginName,
                adminLoginEmail = @adminLoginEmail,
                adminLoginPassword = @adminLoginPassword,
                adminLoginPhoneNo = @adminLoginPhoneNo,
                adminLoginDescription = @adminLoginDescription
            WHERE adminLoginID = @adminLoginID";

        using (SqlConnection con =
            new SqlConnection(ConnectionString))
        using (SqlCommand cmd =
            new SqlCommand(query, con))
        {
            AddAdminParameters(cmd, adminlogin);

            cmd.Parameters.Add("@adminLoginID",
                SqlDbType.Int).Value =
                adminlogin.AdminLoginID;

            con.Open();

            return cmd.ExecuteNonQuery();
        }
    }

    // ---------------------------------------------------------
    // Delete Admin account.
    // ---------------------------------------------------------
    public static int DeleteAdminLogin(
        int adminLoginID)
    {
        const string query = @"
            DELETE FROM AdminLogin
            WHERE adminLoginID = @adminLoginID";

        using (SqlConnection con =
            new SqlConnection(ConnectionString))
        using (SqlCommand cmd =
            new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@adminLoginID",
                SqlDbType.Int).Value =
                adminLoginID;

            con.Open();

            return cmd.ExecuteNonQuery();
        }
    }
}