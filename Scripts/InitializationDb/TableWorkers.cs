using Microsoft.Data.SqlClient;

namespace Transit.Models;

public class TableWorkers
{
    public static async Task AddData(string name, string lastName, int postId)
    {
        string sqlExpression = "INSERT INTO Workers (Name, LastName, PostId) VALUES (@name, @lastname, @postid)";

        using (SqlConnection connection = new SqlConnection(Settings.ConnectionInfo))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(sqlExpression, connection);

            SqlParameter nameParam = new SqlParameter("@name", name);
            command.Parameters.Add(nameParam);
            SqlParameter lastNameParam = new SqlParameter("@lastname", lastName);
            command.Parameters.Add(lastNameParam);
            SqlParameter postIdParam = new SqlParameter("@postid", postId);
            command.Parameters.Add(postIdParam);

            int number = await command.ExecuteNonQueryAsync();
            Console.WriteLine($"Добавлено объектов: {number}");
        }
    }
}