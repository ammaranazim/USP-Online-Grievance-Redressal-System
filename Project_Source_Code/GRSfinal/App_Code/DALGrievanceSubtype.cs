using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class DALGrievanceSubtype
{
    public static int RegisterGrievanceSubtype(MGrievanceSubtype Subtype)
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
                @"INSERT INTO GrievanceSubtype
                (
                    gvSubtypeName,
                    gvSubtypeDescription,
                    categoryID
                )
                VALUES
                (
                    @gvSubtypeName,
                    @gvSubtypeDescription,
                    @categoryID
                )", con);

            cmd.Parameters.Add(new SqlParameter("@gvSubtypeName", Subtype.GvSubtypeName));
            cmd.Parameters.Add(new SqlParameter("@gvSubtypeDescription", Subtype.GvSubtypeDescription));
            cmd.Parameters.Add(new SqlParameter("@categoryID", Subtype.CategoryID));

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
    public static DataTable GetAllCategories()
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
                "SELECT categoryID, categoryName FROM GrievanceCategory ORDER BY categoryName",
                con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
        }
        catch
        {
            dt = null;
        }
        finally
        {
            con.Close();
        }

        return dt;
    }
}