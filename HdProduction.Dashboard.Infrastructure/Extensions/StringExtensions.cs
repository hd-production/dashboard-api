using Newtonsoft.Json;

namespace HdProduction.Dashboard.Infrastructure.Extensions
{
  public static class StringExtensions
  {
    public static T DeserializeFromJson<T>(this string str)
    {
      return str == null
        ? default(T)
        : JsonConvert.DeserializeObject<T>(str,
          new JsonSerializerSettings
          {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
          });
    }
  }
}