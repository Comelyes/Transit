namespace Transit.Models;

public class Seeker
{
    public int Id { get; set; }
    public int? Number { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; } 
    public string? Patronymic { get; set; }
    public int PostId { get; set; }
    public DateTime FirstStatementTime { get; set; }
    public int WorkerId { get; set; }
    public DateTime TaskTime { get; set; }
}