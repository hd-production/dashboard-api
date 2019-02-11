using System.Linq;
using System.Reflection;

namespace HdProduction.Dashboard.Domain.Entities
{
  public abstract class EntityMetadata<T>
  {
    protected static string InQuotes(string str) => $"\"{str}\"";

    public static string[] All =>
      typeof(T)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.Name != "Table")
        .Select(f => f.GetValue(null))
        .Cast<string>()
        .ToArray();
  }
}