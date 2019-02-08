using System;
using HdProduction.Dashboard.Domain.Entities.Projects;

namespace HdProduction.Dashboard.Infrastructure.Synchronization
{
  public class AppBuildLocker : IDisposable
  {
    private readonly SelfHostBuildConfiguration _key;
    private static readonly KeyLocker<SelfHostBuildConfiguration> Locker = new KeyLocker<SelfHostBuildConfiguration>();

    public AppBuildLocker(SelfHostBuildConfiguration key)
    {
      _key = key;
      Locker.Lock(_key);
    }

    public void Dispose()
    {
      Locker.Release(_key);
    }
  }
}