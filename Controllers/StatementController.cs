using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Transit.Models;
using Transit.Scripts;

namespace Transit.Controllers;

public class StatementController : Controller
{
    [HttpPost]
    [Route("/api/newstatement")]
    public async Task<string> AddNewStatement (int seekerId, int status, int superVisorId, int value)
    {
        var s = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        Console.WriteLine($"New request from {s} at {DateTime.UtcNow.ToString()}");

        string sqlExpression = "sp_InsertStatement";
        using (SqlConnection connection = new SqlConnection(Settings.ConnectionInfo))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.CommandType = CommandType.StoredProcedure;
            
            SqlParameter param1 = new SqlParameter("@status", status);
            command.Parameters.Add(param1);
            SqlParameter param2 = new SqlParameter("@value", value);
            command.Parameters.Add(param2);
            SqlParameter param3 = new SqlParameter("@seekerid", seekerId);
            command.Parameters.Add(param3);
            SqlParameter param4 = new SqlParameter("@supervisorid", superVisorId);
            command.Parameters.Add(param4);
            SqlParameter param5 = new SqlParameter("@passtime", DateTime.MaxValue.ToString());
            command.Parameters.Add(param5);

            var result = command.ExecuteScalar();

            Notifications.Instance.StatementStatusChanged?.Invoke(Decimal.ToInt32((decimal)result), status);
            return $"{result}";
        }
    }

    [HttpPost]
    [Route("/api/updatestatement")]
    public async Task<string> UpdateStatement(int statementId, int status, int value, string passTime = "0")
    {
        var s = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        Console.WriteLine($"New request from {s} at {DateTime.UtcNow.ToString()}");
        
        string sqlExpression = "sp_UpdateStatement";

        using (SqlConnection connection = new SqlConnection(Settings.ConnectionInfo))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = new SqlParameter("@status", status);
            command.Parameters.Add(param1);
            SqlParameter param2 = new SqlParameter("@value", value);
            command.Parameters.Add(param2);
            SqlParameter param3 = new SqlParameter("@statementid", statementId);
            command.Parameters.Add(param3);
            if ( (StatementStatus)status == StatementStatus.Done )
            {
                passTime = passTime == "0" ? DateTime.UtcNow.ToString() : passTime;
            }
            SqlParameter param4 = new SqlParameter("@passtime", passTime);
            command.Parameters.Add(param4);


            var result = command.ExecuteNonQuery();

            Notifications.Instance.StatementStatusChanged?.Invoke(statementId, status);
            return $"{result}";
        }
    }

    [HttpPost]
    [Route("/api/getstatement")]
    public async Task<string> GetStatement (int statementId)
    {
        string sqlExpression = $"SELECT * FROM Statements WHERE Id = {statementId}";
 
        using (SqlConnection connection = new SqlConnection(Settings.ConnectionInfo))
        {
            await connection.OpenAsync();
 
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();
 
            if (reader.HasRows) 
            {
                Statement statement;
                await reader.ReadAsync();
                
                
                statement = new Statement()
                {
                    Id = reader.GetInt32(0),
                    Status = reader.GetInt32(1),
                    SeekerId = reader.GetInt32(2),
                    Value = reader.GetInt32(3),
                    PassTime = reader.GetString(4),
                    SuperVisorId = reader.GetInt32(5)
                };
                await reader.CloseAsync();
                return JsonConvert.SerializeObject(statement, Formatting.Indented);
            }
            return " 0 ";
        }
    }
}