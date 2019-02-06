using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace HdProduction.Dashboard.Infrastructure.EfConfigurations.ValueConverters
{
  internal class JsonValueConverter<T> : ValueConverter<T, string> where T : class
  {
    public JsonValueConverter() : base(
      v => v == null ? null : JsonConvert.SerializeObject(v),
      v => v == null ? null : JsonConvert.DeserializeObject<T>(v))
    {
    }
  }
}