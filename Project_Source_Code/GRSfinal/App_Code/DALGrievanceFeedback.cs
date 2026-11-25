
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class DALGrievanceFeedback
{
    public static bool HasFeedback(int grievanceID, int stdID)
    {
        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con =
            new SqlConnection(connectionString))
        {
            string query = @"
                SELECT COUNT(1)
                FROM dbo.GrievanceFeedback
                WHERE grievanceID = @grievanceID
                  AND stdID = @stdID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                    .Value = grievanceID;

                cmd.Parameters.Add("@stdID", SqlDbType.Int)
                    .Value = stdID;

                con.Open();

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
    }

    public static int SubmitFeedback(
        int grievanceID,
        int stdID,
        int rating,
        string comments)
    {
        if (rating < 1 || rating > 5)
        {
            return 0;
        }

        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con =
            new SqlConnection(connectionString))
        {
            string query = @"
                INSERT INTO dbo.GrievanceFeedback
                (
                    grievanceID,
                    stdID,
                    rating,
                    feedbackComments
                )
                SELECT
                    g.grievanceID,
                    g.stdID,
                    @rating,
                    @comments
                FROM dbo.Grievance g
                WHERE g.grievanceID = @grievanceID
                  AND g.stdID = @stdID
                  AND g.grievanceStatus = 'Resolved'
                  AND NOT EXISTS
                  (
                      SELECT 1
                      FROM dbo.GrievanceFeedback f
                      WHERE f.grievanceID = g.grievanceID
                  );";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                    .Value = grievanceID;

                cmd.Parameters.Add("@stdID", SqlDbType.Int)
                    .Value = stdID;

                cmd.Parameters.Add("@rating", SqlDbType.Int)
                    .Value = rating;

                cmd.Parameters.Add("@comments", SqlDbType.NVarChar, 1000)
                    .Value = string.IsNullOrWhiteSpace(comments)
                        ? (object)DBNull.Value
                        : comments.Trim();

                con.Open();

                return cmd.ExecuteNonQuery();
            }
        }
    }

    public static DataTable GetFeedback(int grievanceID, int stdID)
    {
        DataTable dt = new DataTable();

        string connectionString =
            ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;

        using (SqlConnection con =
            new SqlConnection(connectionString))
        {
            string query = @"
                SELECT
                    feedbackID,
                    grievanceID,
                    stdID,
                    rating,
                    feedbackComments,
                    feedbackDate
                FROM dbo.GrievanceFeedback
                WHERE grievanceID = @grievanceID
                  AND stdID = @stdID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                    .Value = grievanceID;

                cmd.Parameters.Add("@stdID", SqlDbType.Int)
                    .Value = stdID;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }

        return dt;
    }
}
