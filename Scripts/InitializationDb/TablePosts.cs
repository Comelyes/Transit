using Microsoft.Data.SqlClient;

namespace Transit.Models;

public static class TablePosts
{
    public static async Task AddData(string name)
    {
        string sqlExpression = "INSERT INTO Posts (Name) VALUES (@name)";

        using (SqlConnection connection = new SqlConnection(Settings.ConnectionInfo))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(sqlExpression, connection);

            SqlParameter nameParam = new SqlParameter("@name", name);
            command.Parameters.Add(nameParam);

            int number = await command.ExecuteNonQueryAsync();
            Console.WriteLine($"Добавлено объектов: {number}");
        }
    }
}