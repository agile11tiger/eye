using System.Reflection;
namespace Memory.Extensions;

public static class AttributeExtensions
{
    public static TAttribute? GetAttribute<TAttribute>(this Enum enumValue)
           where TAttribute : Attribute
    {
        return enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<TAttribute>();
    }
}
