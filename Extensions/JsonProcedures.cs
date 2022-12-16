using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace Transit.Scripts.ForTest;

public class JsonProcedures
{
    public static void StartTest()
    {
        
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