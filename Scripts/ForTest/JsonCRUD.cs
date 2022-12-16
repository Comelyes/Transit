using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace Transit.Scripts.ForTest;

public class JsonCRUD
{
    public static void StartTest()
    {
        Account account = new Account
        {
            Name = "James",
            SomeInt = 30
        };
        Dictionary<string, Account> dict = new();
        dict.Add("TestTable", account);
            
        string jsonTable = JsonConvert.SerializeObject(dict, Formatting.Indented);
            
        Console.WriteLine(jsonTable);
        
        var t = TestInsertJson(jsonTable);
        t.Wait();
    }
    public class Account
    {
        public string Name { get; set; }
        public int SomeInt { get; set; }
    }

    private static async Task<string> TestReadJson()
    {
        string sqlExpression = "SELECT * FROM TestTable ";
        using (SqlConnection connection = new SqlConnection(Settings.ConnectionInfo))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.Parameters.Add(new SqlParameter("@json", SqlDbType.Int)
                .Direction == ParameterDirection.ReturnValue);
            
            
            var result = await command.ExecuteNonQueryAsync();
            Console.WriteLine($"Result db = {result}");
            return "";
        }
        
    }
    
    private static async Task TestProcedureAdd(string json)
    {
        string sqlExpression = "sp_TestInsertJson";
        using (SqlConnection connection = new SqlConnection(Settings.ConnectionInfo))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = new SqlParameter("@json", json);
            command.Parameters.Add(param1);
            
            var result = command.ExecuteScalar();
            Console.WriteLine($"Result db = {result}");
        }
    }
    private static async Task TestInsertJson(string json)
    {
        string sqlExpression = "INSERT INTO TestTable SELECT * FROM OPENJSON(@json, '$.TestTable') " +
                               "WITH  (  Name   varchar(60)     '$.Name', " +
                               "SomeInt   INT  '$.SomeInt' )";
        using (SqlConnection connection = new SqlConnection(Settings.ConnectionInfo))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.Parameters.Add(new SqlParameter("@json", json));
            
            var result = await command.ExecuteNonQueryAsync();
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
}