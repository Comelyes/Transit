namespace Transit.Scripts;

public class Test
{
    string sql = "Select * " +
                 "FROM TestTable " +
                 "FOR JSON AUTO, ROOT('TestTable'), INCLUDE_NULL_VALUES ";
}