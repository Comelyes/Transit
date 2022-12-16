using System.Data;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Transit.Extensions.Attributes;
using Transit.Models;
using Transit.Scripts;
using Transit.Scripts.ForTest;

namespace Transit.DbJSON;

public static class JsonCrud
{
    public static void Test()
    {
        TestTable account = new TestTable
        {
            Name = "List Json - multiple ",
            SomeInt = 60
        };
        TestTable account2 = new TestTable
        {
            Name = "Multiple",
            SomeInt = 62
        };
        List<TestTable> tables = new List<TestTable>();
        tables.Add(account);
        tables.Add(account2);
        Dictionary<string, List<TestTable>> dict = new();
        dict.Add(account.GetType().Name, tables);
            
        string jsonTable = JsonConvert.SerializeObject(dict, Formatting.Indented);
            
        Console.WriteLine(jsonTable);
        
        //TestAttr<TestTable>();
        Console.WriteLine($"Result = {GetFieldData<TestTable>()}");

        var t = InsertJsonToTable<TestTable>(jsonTable);
        t.Wait();

        var b = ReadTableJson<TestTable>();
        b.Wait();
        Console.WriteLine(b.Result);
    }

    private static string GetFieldData<T>(
        BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
    {
        StringBuilder builder = new StringBuilder();
        
        Type type = typeof(T);
        
        var fields = type.GetFields(bindingFlags);
        foreach (var field in fields)
        {
            if (builder.Length != 0)
            {
                builder.Append(", ");
            }

            VarcharAttribute? varcharAttribute = field.GetCustomAttribute<VarcharAttribute>();
            if (varcharAttribute != null )
            {
                builder.Append($" {field.Name} varchar({varcharAttribute.Count}) '$.{field.Name}' ");
            }
            else
            {
                builder.Append(
                    $" {field.Name} {GetTypeSqlFromtype(field.FieldType)} '$.{field.Name}' ");
            }
            Console.WriteLine($"Builder now is {builder}");
        }

        return builder.ToString();
    }

    private static string GetTypeSqlFromtype(Type type)
    {
        if (type == typeof(string))
            return "VARCHAR(200)";
        else if (type == typeof(int))
            return "INT";
        throw new ArgumentException("Invalid type if method");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns>JSON string</returns>
    private static async Task<List<T>> ReadTableJson<T>()
    {
        string sqlExpression = $" SELECT * FROM {typeof(T).Name} " +
                               $"FOR JSON AUTO, ROOT('{typeof(T).Name}'), INCLUDE_NULL_VALUES ";
        // FOR JSON AUTO/PATH(when declare AS), ROOT('name'), INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
        using (SqlConnection connection = new SqlConnection(Settings.ConnectionInfo))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            /*
            command.Parameters.Add(new SqlParameter("@res", DbType.String)
                .Direction == ParameterDirection.ReturnValue);
            */
            
            var result = await command.ExecuteScalarAsync();
            Console.WriteLine($"Result db = {result} and ");

            var dict = JsonConvert.DeserializeObject<Dictionary<string,List<T>>>(result.ToString());
            return dict.Values.GetEnumerator().Current;
        }
    }
    
    private static async Task InsertJsonToTable<T>(string json)
    {
        string sqlExpression = $"INSERT INTO {typeof(T).Name} SELECT * FROM OPENJSON(@json, '$.{typeof(T).Name}') " +
                               $"WITH  ( {GetFieldData<T>()} )"; // Name   varchar(60)     '$.Name', SomeInt   INT  '$.SomeInt'
        using (SqlConnection connection = new SqlConnection(Settings.ConnectionInfo))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.Parameters.Add(new SqlParameter("@json", json));
            
            Console.WriteLine($"Try insert json: {json}");
            var result = await command.ExecuteNonQueryAsync();
            Console.WriteLine($"Result db = {result}");
        }
    }
}