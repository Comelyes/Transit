namespace Transit.Extensions.Attributes;

public class VarcharAttribute : Attribute
{
    public int Count { get;}

    public VarcharAttribute() { Count = Settings.VarcharLenght;}
    public VarcharAttribute(int count) => Count = count;
}