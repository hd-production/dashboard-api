using NpgsqlTypes;

namespace HdProduction.Dashboard.Infrastructure.Extensions
{
  public static class NpgsqlExtensions
  {
    public static string ToSql(this NpgsqlDbType dbType)
    {
      return dbType.ToString().ToUpper();
    }

    public static string ToSql(this NpgsqlDbType dbType, int length)
    {
      return $"{dbType.ToSql()}({length})";
    }
  }
}