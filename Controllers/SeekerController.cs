using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Transit.Extensions.Serialization;
using Transit.Models;

namespace Transit.Controllers;

public class SeekerController : Controller
{
    [HttpPost]
    [Route("/api/newseeker")]
    public async Task<string> AddNewSeeker(string name, string lastName, int number, string patronymic, int postId, string taskTime, int workerId )
    {
        var s = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        Console.WriteLine($"New request from {s} at {DateTime.UtcNow.ToString()}");
        
        string sqlExpression = "sp_InsertSeeker"; 
 
        using (SqlConnection connection = new SqlConnection(Settings.ConnectionInfo))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.CommandType = CommandType.StoredProcedure;
            
            SqlParameter param1 = new SqlParameter("@name", name);
            command.Parameters.Add(param1);
            SqlParameter param2 = new SqlParameter("@lastname", lastName);
            command.Parameters.Add(param2);
            SqlParameter param3 = new SqlParameter("@patronymic", patronymic);
            command.Parameters.Add(param3);
            SqlParameter param4 = new SqlParameter("@number", number);
            command.Parameters.Add(param4);
            SqlParameter param5 = new SqlParameter("@postid", postId);
            command.Parameters.Add(param5);
            SqlParameter param6 = new SqlParameter("@tasktime", taskTime);
            command.Parameters.Add(param6);
            SqlParameter param7 = new SqlParameter("@workerid", workerId);
            command.Parameters.Add(param7);
            SqlParameter param8 = new SqlParameter("@time", DateTime.UtcNow.ToString());
            command.Parameters.Add(param8);
 
            var result = command.ExecuteScalar();
            
            return $"{result}";
        }
    }

    [HttpPost]
    [Route("/api/seekersinfo")]
    public async Task<string> GetAllSeekersInfo (int id, string fromTime = "0", string endTime = "0")
    {
        var s = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        Console.WriteLine($"New request from {s} at {DateTime.UtcNow.ToString()}");

        DateTime startTimeFilter = fromTime == "0" ? DateTime.MinValue : DateTime.Parse(fromTime);
        DateTime endTimeFilter = endTime == "0" ? DateTime.UtcNow : DateTime.Parse(endTime); 
        
        string checkId = $"SELECT * FROM Workers WHERE Id = {id}";
 
        using (SqlConnection connection = new SqlConnection(Settings.ConnectionInfo))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(checkId, connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            if (!reader.HasRows)
            {
                return "No Access";
            }
        }
        
        string sqlExpression = "sp_GetAllSeekersInfo";

        using (SqlConnection connection = new SqlConnection(Settings.ConnectionInfo))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows) 
            {
                List<SerializerSeekersInfo> seekers = new();
                while (await reader.ReadAsync())
                {
                    SerializerSeekersInfo seeker = new () 
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Patronymic = reader.GetString(3),
                        PostName = reader.GetString(4),
                        SuperVisorName = reader.GetString(5),
                        FirstMeetTime = reader.GetString(6),
                        Status = Statement.GetStringStatusRus((StatementStatus)reader.GetInt32(7)),
                        Value = reader.GetInt32(8),
                        PassTime = reader.GetString(9)
                    };
                    if (startTimeFilter > DateTime.Parse(seeker.FirstMeetTime) || endTimeFilter < DateTime.Parse(seeker.FirstMeetTime))
                        continue;
                    seekers.Add(seeker);
                }
                
                await reader.CloseAsync();
                return JsonConvert.SerializeObject(seekers, Formatting.Indented);
            }
            
            return $"No data";
        }
    }
}