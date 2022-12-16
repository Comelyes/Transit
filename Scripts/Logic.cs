using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Transit.DbJSON;
using Transit.Extensions;
using Transit.Models;
using Transit.Scripts.ForTest;

namespace Transit;

public static class Logic
{
    public static async Task Initialize()
    {
        try
        {
            //await InitializeDB();
            JsonProcedures.StartTest();
            JsonCrud.Test();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
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