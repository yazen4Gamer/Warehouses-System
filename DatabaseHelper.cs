using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class DatabaseHelper
{
    private static SqlConnection connection;

    private static string ConnectionString =>
        ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

    public static SqlConnection Connection
    {
        get
        {
            if (connection == null)
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();
            }
            else if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }
    }

    public static object ExecuteScalar(string query)
    {
        using (SqlCommand cmd = new SqlCommand(query, Connection))
        {
            return cmd.ExecuteScalar();
        }
    }

    public static int ExecuteNonQuery(string query)
    {
        using (SqlCommand cmd = new SqlCommand(query, Connection))
        {
            return cmd.ExecuteNonQuery();
        }
    }

    public static DataTable ExecuteQuery(string query)
    {
        using (SqlCommand cmd = new SqlCommand(query, Connection))
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }

    public static void CloseConnection()
    {
        if (connection != null && connection.State != ConnectionState.Closed)
            connection.Close();
    }
}
