using Microsoft.Data.SqlClient;
using System.Data;

namespace InvestmentResearch.Data;

public class DALAccess : IDALAccess
{
    string connectionString = "Server=santu;Database=InvestmentResearch;Integrated Security=True;TrustServerCertificate=True;";
    public DataTable GetDataByQuery(string Sql)
    {
        DataTable dataTable = new DataTable();
        // The 'using' statements ensure objects are properly disposed of after use
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();

            using (SqlDataAdapter adapter = new SqlDataAdapter(Sql, connection))
            {
                try
                {
                    // Fills the DataTable with the query results
                    adapter.Fill(dataTable);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error fetching data: " + ex.Message);
                }
            }
        }

        return dataTable;
    }

    public int SaveDataByQuery(string Sql)
    {
        int rowsAffected = -1;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(Sql, connection))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
        }

        return rowsAffected;
    }
}
