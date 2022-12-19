using Transit.Extensions.Attributes;

namespace Transit.Models;

public class TestTable
{
    [PrimaryKey] public int Id;
    [Varchar(150)] public string Name;

    public int SomeInt;
}