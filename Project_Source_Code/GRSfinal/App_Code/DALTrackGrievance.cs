using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class DALTrackGrievance
{
    public static int RegisterTrackGrievance(MTrackGrievance Track)
    {
        int result = 0;

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
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO TrackGrievance
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
                    @escalationID,
                    @loginID,
                    @trackingactivityDescription,
                    @trackingactivitydatetime
                )", con);

                cmd.Parameters.Add(
                    new SqlParameter("@grievanceID", Track.GrievanceID));

                // Nullable escalation ID
                if (Track.EscalationID.HasValue)
                {
                    cmd.Parameters.Add(
                        new SqlParameter(
                            "@escalationID",
                            Track.EscalationID.Value));
                }
                else
                {
                    cmd.Parameters.Add(
                        new SqlParameter(
                            "@escalationID",
                            DBNull.Value));
                }

                cmd.Parameters.Add(
                    new SqlParameter("@loginID", Track.LoginID));

                cmd.Parameters.Add(
                    new SqlParameter(
                        "@trackingactivityDescription",
                        Track.TrackingactivityDescription));

                cmd.Parameters.Add(
                    new SqlParameter(
                        "@trackingactivitydatetime",
                        Track.Trackingactivitydatetime));

                cmd.ExecuteNonQuery();

                result = 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
}