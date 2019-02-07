using System;

namespace HdProduction.Dashboard.Domain.Entities
{
  [Flags]
  public enum SelfHostBuildConfiguration
  {
    MySql,
    SqlServer,
    PostgresSql,
    Sqlite,
  }
}