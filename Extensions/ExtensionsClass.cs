using System.Reflection;

namespace Transit.Extensions;

public class ExtensionsClass
{
    /// <summary>
    /// Получение словаря имя свойтва - значение по экземпляру класса
    /// </summary>
    /// <param name="data"> Класс </param>
    /// <param name="toUpperCase"> Ключ словаря To Upper Case </param>
    /// <returns></returns>
    public static Dictionary<string, object> PropertyToDictionaryStrObj<T>(
        T data, bool toUpperCase = false,
        BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        where T : class
    {
        Type type = typeof(T);
        var fieldsPropertyes = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        Dictionary<string, object> dict = new();
        foreach (var property in fieldsPropertyes)
        {
            string name = toUpperCase ? property.Name.ToUpper() : property.Name;
            dict.Add(name, (object)property.GetValue(data));
        }
        
        return dict;
    }
}