using Transit.Extensions.Attributes;

namespace Transit.Models;

public class TestTable
{
    [Varchar(150)] public string Name;

    public int SomeInt;
}