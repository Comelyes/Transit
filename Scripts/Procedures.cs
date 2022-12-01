using Transit.Extensions;

namespace Transit.Scripts;

public static class Procedures
{
    public static async Task AddPrInsertSeeker()
    {
        var sql =
            "CREATE PROCEDURE [sp_InsertSeeker] @name nvarchar(100),  @lastname nvarchar(100), @patronymic nvarchar(100), " +
            "@number int,  @postid int, @tasktime nvarchar(100),  @workerid int,  @time nvarchar(100) "
            +
            "AS INSERT INTO Seekers (Name, LastName, Patronymic, Number, PostId, TaskTime, WorkerId, FirstStatementTime) " +
            "VALUES (@name, @lastname, @patronymic, @number, @postid, @tasktime, @workerid, @time) "
            +
            "SELECT SCOPE_IDENTITY() GO";
        await CustomSqlCommands.PerformSqlCommand(sql);
    }
    
    public static async Task AddPrInsertStatement()
    {
        var sql =
            "CREATE PROCEDURE [sp_InsertStatement] @seekerid int, @status int, @value int, @supervisorid int, @passtime nvarchar(100)" +
            "AS INSERT INTO Statements (SeekerId, Status, Value, SuperVisorId, PassTime) VALUES (@seekerid, @status, @value, @supervisorid, @passtime) " +
            "SELECT SCOPE_IDENTITY() GO";
        await CustomSqlCommands.PerformSqlCommand(sql);
    }
    
    public static async Task AddPrUpdateStatement()
    {
        var sql =
            "CREATE PROCEDURE [sp_UpdateStatement] @statementid int, @status int, @value int, @passtime nvarchar(100) " +
            "AS UPDATE Statements SET Status = @status, Value = @value, PassTime = @passtime WHERE Id = @statementid ";
        await CustomSqlCommands.PerformSqlCommand(sql);
    }
    public static async Task AddPrGetAllSeekersInfo()
    {
        var sql = "CREATE PROCEDURE sp_GetAllSeekersInfo " +
                 "AS " +
                 "SELECT Seekers.Id, Seekers.Name, Seekers.LastName, Seekers.Patronymic, Posts.Name AS PostName, Workers.Name AS SuperVisorName, Seekers.FirstStatementTime, " +
                 "Statements.Status, Statements.Value, Statements.PassTime " +
                 "FROM Seekers" +
                 "INNER JOIN Statements ON Statements.SeekerId = Seekers.Id" +
                 "INNER JOIN Posts ON Posts.id = Seekers.PostId" +
                 "INNER JOIN Workers ON Seekers.WorkerId = Workers.Id" +
                 "GO";
            await CustomSqlCommands.PerformSqlCommand(sql);
    }
}