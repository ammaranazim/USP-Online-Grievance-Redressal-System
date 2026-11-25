using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class DALGrievanceCategory
{
    public static DataTable GetAllCategories()
    {
        DataTable dt = new DataTable();

        string conStr = ConfigurationManager.ConnectionStrings["GRS"].ConnectionString;


        using (SqlConnection con = new SqlConnection(conStr))
        {
            string query = "SELECT categoryID, categoryName FROM GrievanceCategory ORDER BY categoryName";

            SqlDataAdapter da = new SqlDataAdapter(query, con);

            da.Fill(dt);
        }

        return dt;
    }
    public static int RegisterCategory(MGrievanceCategory Category)
    {
        int result = 0;

        ConnectionStringSettings consettings =
        ConfigurationManager.ConnectionStrings["GRS"];

        string constring = consettings.ConnectionString;

        SqlConnection con = new SqlConnection(constring);

        try
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
            @"INSERT INTO [dbo].[GrievanceCategory]
            (
                [categoryName],
                [categoryDescription],
                [categoryStatus]
            )
            VALUES
            (
                @categoryName,
                @categoryDescription,
                @categoryStatus
            )", con);


            cmd.Parameters.AddWithValue("@categoryName",
                Category.CategoryName);

            cmd.Parameters.AddWithValue("@categoryDescription",
                Category.CategoryDescription);

            cmd.Parameters.AddWithValue("@categoryStatus",
                Category.CategoryStatus);


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