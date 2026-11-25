
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class DALSystemNotification
{
    private static string ConnectionString
    {
        get
        {
            return ConfigurationManager
                .ConnectionStrings["GRS"].ConnectionString;
        }
    }

    public static int InsertNotification( string recipientRole, int recipientID, int? grievanceID,  string notificationType,  string message)
    {
        const string query = @"
            INSERT INTO SystemNotification
            (
                recipientRole,
                recipientID,
                grievanceID,
                notificationType,
                message
            )
            VALUES
            (
                @recipientRole,
                @recipientID,
                @grievanceID,
                @notificationType,
                @message
            )";

        using (SqlConnection con =
            new SqlConnection(ConnectionString))
        using (SqlCommand cmd =
            new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@recipientRole",
                SqlDbType.NVarChar, 20).Value = recipientRole;

            cmd.Parameters.Add("@recipientID",
                SqlDbType.Int).Value = recipientID;

            cmd.Parameters.Add("@grievanceID",
                SqlDbType.Int).Value =
                (object)grievanceID ?? DBNull.Value;

            cmd.Parameters.Add("@notificationType",
                SqlDbType.NVarChar, 50).Value = notificationType;

            cmd.Parameters.Add("@message",
                SqlDbType.NVarChar, 255).Value = message;

            con.Open();
            return cmd.ExecuteNonQuery();
        }
    }

    public static DataTable GetNotifications(
        string recipientRole,
        int recipientID)
    {
        const string query = @"
            SELECT
                notificationID,
                recipientRole,
                recipientID,
                grievanceID,
                notificationType,
                message,
                createdDate,
                isRead
            FROM SystemNotification
            WHERE recipientRole = @recipientRole
              AND recipientID = @recipientID
            ORDER BY createdDate DESC, notificationID DESC";

        DataTable dt = new DataTable();

        using (SqlConnection con =
            new SqlConnection(ConnectionString))
        using (SqlCommand cmd =
            new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@recipientRole",
                SqlDbType.NVarChar, 20).Value = recipientRole;

            cmd.Parameters.Add("@recipientID",
                SqlDbType.Int).Value = recipientID;

            using (SqlDataAdapter da =
                new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }

        return dt;
    }

    public static int GetUnreadCount(
        string recipientRole,
        int recipientID)
    {
        const string query = @"
            SELECT COUNT(*)
            FROM SystemNotification
            WHERE recipientRole = @recipientRole
              AND recipientID = @recipientID
              AND isRead = 0";

        using (SqlConnection con =
            new SqlConnection(ConnectionString))
        using (SqlCommand cmd =
            new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@recipientRole",
                SqlDbType.NVarChar, 20).Value = recipientRole;

            cmd.Parameters.Add("@recipientID",
                SqlDbType.Int).Value = recipientID;

            con.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }

    public static int MarkAsRead(
        int notificationID,
        string recipientRole,
        int recipientID)
    {
        const string query = @"
            UPDATE SystemNotification
            SET isRead = 1
            WHERE notificationID = @notificationID
              AND recipientRole = @recipientRole
              AND recipientID = @recipientID
              AND isRead = 0";

        using (SqlConnection con =
            new SqlConnection(ConnectionString))
        using (SqlCommand cmd =
            new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@notificationID",
                SqlDbType.Int).Value = notificationID;

            cmd.Parameters.Add("@recipientRole",
                SqlDbType.NVarChar, 20).Value = recipientRole;

            cmd.Parameters.Add("@recipientID",
                SqlDbType.Int).Value = recipientID;

            con.Open();
            return cmd.ExecuteNonQuery();
        }
    }

    public static int MarkAllAsRead(
        string recipientRole,
        int recipientID)
    {
        const string query = @"
            UPDATE SystemNotification
            SET isRead = 1
            WHERE recipientRole = @recipientRole
              AND recipientID = @recipientID
              AND isRead = 0";

        using (SqlConnection con =
            new SqlConnection(ConnectionString))
        using (SqlCommand cmd =
            new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@recipientRole",
                SqlDbType.NVarChar, 20).Value = recipientRole;

            cmd.Parameters.Add("@recipientID",
                SqlDbType.Int).Value = recipientID;

            con.Open();
            return cmd.ExecuteNonQuery();
        }
    }
}
