using System;

namespace HdProduction.Dashboard.Domain.Entities.Projects
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