using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Transit.Extensions;
using Transit.Models;

namespace Transit;

public static class Logic
{
    public static async Task Initialize()
    {
        try
        {
            //await InitializeDB();
            //await TestInsertJson(); // test json
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public class Account
    {
        public string Name { get; set; }
        public int SomeInt { get; set; }
    }
    private static async Task TestInsertJson()
    {
        string sqlExpression = "sp_TestInsertJson";
        using (SqlConnection connection = new SqlConnection(Settings.ConnectionInfo))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.CommandType = CommandType.StoredProcedure;
            
            Account account = new Account
            {
                Name = "james",
                SomeInt = 28
            };
            Dictionary<string, Account> dict = new();
            dict.Add("TestTable", account);

            string json = JsonConvert.SerializeObject(account, Formatting.Indented);
            string jsonTable = JsonConvert.SerializeObject(dict, Formatting.Indented);

            Console.WriteLine(json);
            Console.WriteLine(jsonTable);
            
            SqlParameter param1 = new SqlParameter("@json", jsonTable);
            command.Parameters.Add(param1);
            
            var result = command.ExecuteScalar();
            Console.WriteLine($"Result db = {result}");

        }
    }
    /*CREATE PROCEDURE sp_TestInsertJson @json NVARCHAR(MAX)
AS 
INSERT INTO TestTable
SELECT * 
FROM OPENJSON(@json, '$.TestTable')
WITH  (  
        Name   varchar(60)     '$.Name', 
        SomeInt   INT      '$.SomeInt'
    )
GO*/
    
    /*
     * DECLARE @json NVARCHAR(MAX) = N'{ 
    "TestTable" : {
            "Name" : "Name2",
            "SomeInt" : 26 
    }
}';

INSERT INTO TestTable
SELECT * 
FROM OPENJSON(@json, '$.TestTable')
WITH  (  
        Name   varchar(60)     '$.Name', 
        SomeInt   INT      '$.SomeInt'
    )
     */
    
    private static async Task InitializeDB()
    {
        await CustomSqlCommands.PerformSqlCommand("CREATE TABLE Posts (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(150) NOT NULL)");
        await CustomSqlCommands.PerformSqlCommand("CREATE TABLE Workers (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(100) NOT NULL, LastName NVARCHAR(100) NOT NULL, PostId INT REFERENCES Posts (Id))");
        await CustomSqlCommands.PerformSqlCommand("INSERT INTO Posts (Name) VALUES ('Без должности')");
        await TablePosts.AddData("Специалист отдела кадров"); // id 2
        await TablePosts.AddData("Программист"); // id 3
        await TableWorkers.AddData("HR", "HR last name", 2); // отдел кадров
        await TableWorkers.AddData("Александр", "Петрович", 3); // Программист
        await CustomSqlCommands.PerformSqlCommand("CREATE TABLE Seekers (Id INT PRIMARY KEY IDENTITY, Number INT, Name NVARCHAR(100) NOT NULL, LastName NVARCHAR(100) NOT NULL, Patronymic NVARCHAR(100) NOT NULL, PostId INT REFERENCES Posts (Id), WorkerId INT REFERENCES Workers (Id), FirstStatementTime NVARCHAR(200) NOT NULL, TaskTime NVARCHAR(200) NOT NULL )");
        await CustomSqlCommands.PerformSqlCommand("CREATE TABLE Statements (Id INT PRIMARY KEY IDENTITY, SeekerId INT REFERENCES Seekers (Id), Status INT, Value INT, PassTime NVARCHAR(200), SuperVisorId INT REFERENCES Workers (Id))");
        
    }
}