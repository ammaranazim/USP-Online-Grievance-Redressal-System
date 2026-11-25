using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

public class DALPasswordReset
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

    // =========================================================
    // 1. Generate a cryptographically secure reset token
    // =========================================================
    public static string GenerateResetToken()
    {
        byte[] tokenBytes = new byte[32];

        using (RandomNumberGenerator rng =
            RandomNumberGenerator.Create())
        {
            rng.GetBytes(tokenBytes);
        }

        return Convert.ToBase64String(tokenBytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }

    // =========================================================
    // 2. Generate SHA-256 hash of reset token
    //
    // Only this hash is stored in the database.
    // The original token is sent by email.
    // =========================================================
    public static string HashResetToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException(
                "Reset token cannot be empty.",
                "token");
        }

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] tokenBytes =
                Encoding.UTF8.GetBytes(token);

            byte[] hashBytes =
                sha256.ComputeHash(tokenBytes);

            StringBuilder builder =
                new StringBuilder();

            foreach (byte b in hashBytes)
            {
                builder.Append(
                    b.ToString("x2"));
            }

            return builder.ToString();
        }
    }

    // =========================================================
    // 3. Find account by role and email
    //
    // Returns:
    // userID
    // email
    // role
    //
    // Returns null if account does not exist.
    // =========================================================
    public static PasswordResetUser
        GetUserByEmail(
            string userRole,
            string email)
    {
        if (string.IsNullOrWhiteSpace(userRole) ||
            string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        PasswordResetUser user = null;

        using (SqlConnection con =
            new SqlConnection(ConnectionString))
        {
            con.Open();

            if (userRole.Equals(
                    "Student",
                    StringComparison.OrdinalIgnoreCase))
            {
                const string query = @"
                    SELECT
                        sl.stdID,
                        si.stdEmail
                    FROM studentLogin sl
                    INNER JOIN StudentInfo si
                        ON sl.stdID = si.stdID
                    WHERE si.stdEmail = @email";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.Add(
                        "@email",
                        SqlDbType.NVarChar, 100).Value =
                        email.Trim();

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new PasswordResetUser
                            {
                                UserID =
                                    Convert.ToInt32(
                                        reader["stdID"]),

                                Email =
                                    Convert.ToString(
                                        reader["stdEmail"]),

                                UserRole = "Student"
                            };
                        }
                    }
                }
            }
            else if (userRole.Equals(
                         "Admin",
                         StringComparison.OrdinalIgnoreCase))
            {
                const string query = @"
                    SELECT
                        adminLoginID,
                        adminLoginEmail
                    FROM AdminLogin
                    WHERE adminLoginEmail = @email";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.Add(
                        "@email",
                        SqlDbType.NVarChar, 100).Value =
                        email.Trim();

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new PasswordResetUser
                            {
                                UserID =
                                    Convert.ToInt32(
                                        reader["adminLoginID"]),

                                Email =
                                    Convert.ToString(
                                        reader["adminLoginEmail"]),

                                UserRole = "Admin"
                            };
                        }
                    }
                }
            }
            else if (userRole.Equals(
                         "HOD",
                         StringComparison.OrdinalIgnoreCase))
            {
                const string query = @"
                    SELECT
                        deptHeadID,
                        deptHeadEmail
                    FROM DepartmentHead
                    WHERE deptHeadEmail = @email";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.Add(
                        "@email",
                        SqlDbType.NVarChar, 100).Value =
                        email.Trim();

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new PasswordResetUser
                            {
                                UserID =
                                    Convert.ToInt32(
                                        reader["deptHeadID"]),

                                Email =
                                    Convert.ToString(
                                        reader["deptHeadEmail"]),

                                UserRole = "HOD"
                            };
                        }
                    }
                }
            }
            else if (userRole.Equals(
                         "Staff",
                         StringComparison.OrdinalIgnoreCase))
            {
                const string query = @"
                    SELECT
                        staffID,
                        staffEmail
                    FROM Staff
                    WHERE staffEmail = @email";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.Add(
                        "@email",
                        SqlDbType.NVarChar, 100).Value =
                        email.Trim();

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new PasswordResetUser
                            {
                                UserID =
                                    Convert.ToInt32(
                                        reader["staffID"]),

                                Email =
                                    Convert.ToString(
                                        reader["staffEmail"]),

                                UserRole = "Staff"
                            };
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException(
                    "Invalid user role.",
                    "userRole");
            }
        }

        return user;
    }

    // =========================================================
    // 4. Invalidate previous unused tokens
    // =========================================================
    private static void InvalidateExistingTokens(
        SqlConnection con,
        SqlTransaction transaction,
        int userID,
        string userRole)
    {
        const string query = @"
            UPDATE PasswordResetToken
            SET isUsed = 1
            WHERE userID = @userID
              AND userRole = @userRole
              AND isUsed = 0
              AND expiryDate > GETDATE()";

        using (SqlCommand cmd =
            new SqlCommand(
                query,
                con,
                transaction))
        {
            cmd.Parameters.Add(
                "@userID",
                SqlDbType.Int).Value = userID;

            cmd.Parameters.Add(
                "@userRole",
                SqlDbType.NVarChar, 20).Value =
                userRole;

            cmd.ExecuteNonQuery();
        }
    }

    // =========================================================
    // 5. Save a new password reset token
    //
    // Token expiry = 30 minutes
    // =========================================================
    public static bool CreatePasswordResetToken(
        int userID,
        string userRole,
        string tokenHash)
    {
        if (userID <= 0)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(userRole) ||
            string.IsNullOrWhiteSpace(tokenHash))
        {
            return false;
        }

        using (SqlConnection con =
            new SqlConnection(ConnectionString))
        {
            con.Open();

            using (SqlTransaction transaction =
                con.BeginTransaction())
            {
                try
                {
                    InvalidateExistingTokens(
                        con,
                        transaction,
                        userID,
                        userRole);

                    const string query = @"
                        INSERT INTO PasswordResetToken
                        (
                            userID,
                            userRole,
                            tokenHash,
                            expiryDate,
                            isUsed
                        )
                        VALUES
                        (
                            @userID,
                            @userRole,
                            @tokenHash,
                            DATEADD(MINUTE, 30, GETDATE()),
                            0
                        )";

                    using (SqlCommand cmd =
                        new SqlCommand(
                            query,
                            con,
                            transaction))
                    {
                        cmd.Parameters.Add(
                            "@userID",
                            SqlDbType.Int).Value =
                            userID;

                        cmd.Parameters.Add(
                            "@userRole",
                            SqlDbType.NVarChar, 20).Value =
                            userRole;

                        cmd.Parameters.Add(
                            "@tokenHash",
                            SqlDbType.NVarChar, 256).Value =
                            tokenHash;

                        int rows =
                            cmd.ExecuteNonQuery();

                        if (rows <= 0)
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
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                    }

                    throw;
                }
            }
        }
    }

    // =========================================================
    // 6. Validate reset token
    //
    // Conditions:
    // - token hash matches
    // - correct role
    // - token unused
    // - token not expired
    // =========================================================
    public static PasswordResetTokenInfo
        ValidateResetToken(
            string token,
            string userRole)
    {
        if (string.IsNullOrWhiteSpace(token) ||
            string.IsNullOrWhiteSpace(userRole))
        {
            return null;
        }

        string tokenHash =
            HashResetToken(token);

        const string query = @"
            SELECT TOP 1
                resetID,
                userID,
                userRole,
                expiryDate,
                isUsed
            FROM PasswordResetToken
            WHERE tokenHash = @tokenHash
              AND userRole = @userRole
              AND isUsed = 0
              AND expiryDate > GETDATE()";

        using (SqlConnection con =
            new SqlConnection(ConnectionString))
        using (SqlCommand cmd =
            new SqlCommand(query, con))
        {
            cmd.Parameters.Add(
                "@tokenHash",
                SqlDbType.NVarChar, 256).Value =
                tokenHash;

            cmd.Parameters.Add(
                "@userRole",
                SqlDbType.NVarChar, 20).Value =
                userRole;

            con.Open();

            using (SqlDataReader reader =
                cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new PasswordResetTokenInfo
                    {
                        ResetID =
                            Convert.ToInt32(
                                reader["resetID"]),

                        UserID =
                            Convert.ToInt32(
                                reader["userID"]),

                        UserRole =
                            Convert.ToString(
                                reader["userRole"]),

                        ExpiryDate =
                            Convert.ToDateTime(
                                reader["expiryDate"]),

                        IsUsed =
                            Convert.ToBoolean(
                                reader["isUsed"])
                    };
                }
            }
        }

        return null;
    }

    // =========================================================
    // 7. Reset password
    //
    // Updates the appropriate account and marks the token
    // as used in the SAME transaction.
    // =========================================================
    public static bool ResetPassword(
        int resetID,
        int userID,
        string userRole,
        string newPassword)
    {
        if (resetID <= 0 ||
            userID <= 0 ||
            string.IsNullOrWhiteSpace(userRole) ||
            string.IsNullOrEmpty(newPassword))
        {
            return false;
        }

        string passwordHash =
            PasswordHelper.HashPassword(newPassword);

        using (SqlConnection con =
            new SqlConnection(ConnectionString))
        {
            con.Open();

            using (SqlTransaction transaction =
                con.BeginTransaction())
            {
                try
                {
                    int passwordRows = 0;

                    // -------------------------------------------------
                    // STUDENT
                    // -------------------------------------------------
                    if (userRole.Equals(
                            "Student",
                            StringComparison.OrdinalIgnoreCase))
                    {
                        const string query = @"
                            UPDATE studentLogin
                            SET stdLoginPassword =
                                @password
                            WHERE stdID = @userID";

                        using (SqlCommand cmd =
                            new SqlCommand(
                                query,
                                con,
                                transaction))
                        {
                            cmd.Parameters.Add(
                                "@password",
                                SqlDbType.NVarChar, 255).Value =
                                passwordHash;

                            cmd.Parameters.Add(
                                "@userID",
                                SqlDbType.Int).Value =
                                userID;

                            passwordRows =
                                cmd.ExecuteNonQuery();
                        }
                    }

                    // -------------------------------------------------
                    // ADMIN
                    // -------------------------------------------------
                    else if (userRole.Equals(
                                 "Admin",
                                 StringComparison.OrdinalIgnoreCase))
                    {
                        const string query = @"
                            UPDATE AdminLogin
                            SET adminLoginPassword =
                                @password
                            WHERE adminLoginID = @userID";

                        using (SqlCommand cmd =
                            new SqlCommand(
                                query,
                                con,
                                transaction))
                        {
                            cmd.Parameters.Add(
                                "@password",
                                SqlDbType.NVarChar, 255).Value =
                                passwordHash;

                            cmd.Parameters.Add(
                                "@userID",
                                SqlDbType.Int).Value =
                                userID;

                            passwordRows =
                                cmd.ExecuteNonQuery();
                        }
                    }

                    // -------------------------------------------------
                    // HOD
                    // -------------------------------------------------
                    else if (userRole.Equals(
                                 "HOD",
                                 StringComparison.OrdinalIgnoreCase))
                    {
                        const string query = @"
                            UPDATE DepartmentHead
                            SET deptHeadPassword =
                                @password
                            WHERE deptHeadID = @userID";

                        using (SqlCommand cmd =
                            new SqlCommand(
                                query,
                                con,
                                transaction))
                        {
                            cmd.Parameters.Add(
                                "@password",
                                SqlDbType.NVarChar, 255).Value =
                                passwordHash;

                            cmd.Parameters.Add(
                                "@userID",
                                SqlDbType.Int).Value =
                                userID;

                            passwordRows =
                                cmd.ExecuteNonQuery();
                        }
                    }

                    // -------------------------------------------------
                    // STAFF
                    // -------------------------------------------------
                    else if (userRole.Equals(
                                 "Staff",
                                 StringComparison.OrdinalIgnoreCase))
                    {
                        const string query = @"
                            UPDATE Staff
                            SET staffPasswordHash =
                                @password
                            WHERE staffID = @userID";

                        using (SqlCommand cmd =
                            new SqlCommand(
                                query,
                                con,
                                transaction))
                        {
                            cmd.Parameters.Add(
                                "@password",
                                SqlDbType.NVarChar, 255).Value =
                                passwordHash;

                            cmd.Parameters.Add(
                                "@userID",
                                SqlDbType.Int).Value =
                                userID;

                            passwordRows =
                                cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        transaction.Rollback();

                        throw new ArgumentException(
                            "Invalid user role.",
                            "userRole");
                    }

                    // -------------------------------------------------
                    // Account must actually exist.
                    // -------------------------------------------------
                    if (passwordRows != 1)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    // -------------------------------------------------
                    // Mark token as used.
                    //
                    // The conditions are repeated here so that a token
                    // cannot be reused or expired during the reset.
                    // -------------------------------------------------
                    const string tokenQuery = @"
                        UPDATE PasswordResetToken
                        SET isUsed = 1
                        WHERE resetID = @resetID
                          AND userID = @userID
                          AND userRole = @userRole
                          AND isUsed = 0
                          AND expiryDate > GETDATE()";

                    int tokenRows;

                    using (SqlCommand cmd =
                        new SqlCommand(
                            tokenQuery,
                            con,
                            transaction))
                    {
                        cmd.Parameters.Add(
                            "@resetID",
                            SqlDbType.Int).Value =
                            resetID;

                        cmd.Parameters.Add(
                            "@userID",
                            SqlDbType.Int).Value =
                            userID;

                        cmd.Parameters.Add(
                            "@userRole",
                            SqlDbType.NVarChar, 20).Value =
                            userRole;

                        tokenRows =
                            cmd.ExecuteNonQuery();
                    }

                    // -------------------------------------------------
                    // If token could not be consumed, don't keep the
                    // password change.
                    // -------------------------------------------------
                    if (tokenRows != 1)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    transaction.Commit();

                    return true;
                }
                catch
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                    }

                    throw;
                }
            }
        }
    }

    // =========================================================
    // Helper class containing account information
    // =========================================================
    public class PasswordResetUser
    {
        public int UserID { get; set; }

        public string Email { get; set; }

        public string UserRole { get; set; }
    }

    // =========================================================
    // Helper class containing validated token information
    // =========================================================
    public class PasswordResetTokenInfo
    {
        public int ResetID { get; set; }

        public int UserID { get; set; }

        public string UserRole { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsUsed { get; set; }
    }
}