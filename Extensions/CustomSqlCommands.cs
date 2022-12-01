using Microsoft.Data.SqlClient;

namespace Transit.Extensions;

public class CustomSqlCommands
{
    public static async Task PerformSqlCommand(string sql)
    {
        using (SqlConnection connection = new SqlConnection(Settings.ConnectionInfo))
        {
            await connection.OpenAsync();

            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Connection = connection;
            var s = await  command.ExecuteNonQueryAsync();
             
            Console.WriteLine($"Sql запрос выполнен: {sql}");
        }
    }
}