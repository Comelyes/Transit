using System.Data;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Transit.Controllers;
using Transit.Extensions;
using Transit.Extensions.Serialization;
using Transit.Models;

namespace Transit.Scripts;

public class ControlSystem
{
    private bool _stop = false;
    public bool Stop
    {
        get => _stop;
        set => _stop = value;
    } 
    public void Start()
    {
        while (true)
        {
            Thread.Sleep(10000);
            if (_stop)
            {
                continue;
            }
            Tick();
        }
    }

    private void Tick()
    {
        try
        {
            var t = CheckStatementPassTime();
            t.Wait();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task CheckStatementPassTime()
    {
        Console.WriteLine($"Check statements for time expiration");
        string sqlExpression = "sp_CheckStatementPassTime";

        using SqlConnection connection = new SqlConnection(Settings.ConnectionInfo); 
        connection.Open(); 
        SqlCommand command = new SqlCommand(sqlExpression, connection);
        command.CommandType = CommandType.StoredProcedure;

        SqlDataReader reader = await command.ExecuteReaderAsync();
        if (reader.HasRows) 
        {
            while (await reader.ReadAsync())
            {
                 var id = reader.GetInt32(0);
                 var status = reader.GetInt32(1);
                 var value = reader.GetInt32(2);
                 var taskTime = DateTime.Parse(reader.GetString(4));
                 
                if (status == 1 && DateTime.UtcNow > taskTime)
                {
                    Console.WriteLine($"New expired statement with id - {id}");
                    status = 4;
                    value = 0;
                    var passTime = DateTime.UtcNow;
                    
                    //TODO: Update data and test 
                    var data = $"statementId={id}&status={status}&value={value}&passTime={passTime}";
                    var response = ApiRequests.PostRequest("https://localhost:7179/api/updatestatement", data);
                }
            }
            await reader.CloseAsync();
        }
        else
        {
            Console.WriteLine($"No expired statement");
        }
    }

    
}