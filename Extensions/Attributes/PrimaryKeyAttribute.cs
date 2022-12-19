namespace Transit.Extensions.Attributes;

public class PrimaryKeyAttribute : Attribute
{
    public bool isPrimaryKey;
    public PrimaryKeyAttribute()
    {
        isPrimaryKey = true;
    }
}