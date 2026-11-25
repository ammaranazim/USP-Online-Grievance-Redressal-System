using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class DALEvidence
{
    public static int RegisterEvidence(MEvidence Evidence)
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
                @"INSERT INTO Grievanceevidence
                (
                    evidenceName,
                    evidenceDescription,
                    evidenceDatetime,
                    evidenceUploadedby,
                    evidenceFilepath,
                    grievanceID
                )
                VALUES
                (
                    @evidenceName,
                    @evidenceDescription,
                    @evidenceDatetime,
                    @evidenceUploadedby,
                    @evidenceFilepath,
                    @grievanceID
                )", con);

            cmd.Parameters.Add(new SqlParameter("@evidenceName", Evidence.EvidenceName));
            cmd.Parameters.Add(new SqlParameter("@evidenceDescription", Evidence.EvidenceDescription));
            cmd.Parameters.Add(new SqlParameter("@evidenceDatetime", Evidence.EvidenceDatetime));
            cmd.Parameters.Add(new SqlParameter("@evidenceUploadedby", Evidence.EvidenceUploadedby));
             cmd.Parameters.Add(new SqlParameter("@evidenceFilepath", Evidence.EvidenceFilepath));
            cmd.Parameters.Add(new SqlParameter("@grievanceID", Evidence.GrievanceID));

            cmd.ExecuteNonQuery();

            result = 1;
        }
        catch
        {
            result = 0;
        }
        finally
        {
            con.Close();
        }

        return result;
    }

    public static List<MEvidence> GetEvidenceByGrievanceID(int grievanceID)
    {
        List<MEvidence> lst = new List<MEvidence>();

        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (consettings == null ||
            string.IsNullOrEmpty(consettings.ConnectionString))
        {
            throw new Exception("Database Connection Error");
        }

        using (SqlConnection con =
            new SqlConnection(consettings.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
        SELECT
            e.evidenceID,
            e.evidenceName,
            e.evidenceDescription,
            e.evidenceDatetime,
            e.evidenceUploadedby,
            e.evidenceFilepath,
            e.grievanceID
        FROM dbo.Grievanceevidence e
        INNER JOIN dbo.Grievance g
            ON g.grievanceID = e.grievanceID
        INNER JOIN dbo.StudentInfo si
            ON si.stdID = g.stdID
        WHERE e.grievanceID = @grievanceID
          AND e.evidenceUploadedby =
              LTRIM(RTRIM(
                  ISNULL(si.stdFirstName, '') + ' ' +
                  ISNULL(si.stdLastName, '')
              ))
          AND (
              e.evidenceUploadedby IS NULL
              OR e.evidenceUploadedby NOT LIKE 'Staff:%'
          )
        ORDER BY e.evidenceDatetime DESC, e.evidenceID DESC;", con))
        {
            cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                .Value = grievanceID;

            con.Open();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    MEvidence evidence = new MEvidence(
                        Convert.ToInt32(dr["evidenceID"]),
                        dr["evidenceName"].ToString(),
                        dr["evidenceDescription"].ToString(),
                        Convert.ToDateTime(dr["evidenceDatetime"]),
                        dr["evidenceUploadedby"].ToString(),
                        dr["evidenceFilepath"].ToString(),
                        Convert.ToInt32(dr["grievanceID"])
                    );

                    lst.Add(evidence);
                }
            }
        }

        return lst;
    }


    public static List<MEvidence> GetResolutionEvidenceByGrievanceID(
        int grievanceID,
        int staffID)
    {
        List<MEvidence> lst = new List<MEvidence>();

        ConnectionStringSettings consettings =
            ConfigurationManager.ConnectionStrings["GRS"];

        if (consettings == null ||
            string.IsNullOrEmpty(consettings.ConnectionString))
        {
            throw new Exception("Database Connection Error");
        }

        string constring = consettings.ConnectionString;

        using (SqlConnection con = new SqlConnection(constring))
        using (SqlCommand cmd = new SqlCommand(@"
        SELECT
            e.evidenceID,
            e.evidenceName,
            e.evidenceDescription,
            e.evidenceDatetime,
            e.evidenceUploadedby,
            e.evidenceFilepath,
            e.grievanceID
        FROM dbo.Grievanceevidence e
        WHERE e.grievanceID = @grievanceID
          AND e.evidenceUploadedby = @uploadedBy
        ORDER BY e.evidenceDatetime DESC, e.evidenceID DESC;", con))
        {
            cmd.Parameters.Add("@grievanceID", SqlDbType.Int)
                .Value = grievanceID;

            cmd.Parameters.Add("@uploadedBy", SqlDbType.NVarChar, 100)
                .Value = "Staff:" + staffID;

            con.Open();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    MEvidence evidence = new MEvidence(
                        Convert.ToInt32(dr["evidenceID"]),
                        dr["evidenceName"].ToString(),
                        dr["evidenceDescription"].ToString(),
                        Convert.ToDateTime(dr["evidenceDatetime"]),
                        dr["evidenceUploadedby"].ToString(),
                        dr["evidenceFilepath"].ToString(),
                        Convert.ToInt32(dr["grievanceID"])
                    );

                    lst.Add(evidence);
                }
            }
        }


        return lst;
    }

}





