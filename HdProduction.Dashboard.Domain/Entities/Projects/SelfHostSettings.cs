namespace HdProduction.Dashboard.Domain.Entities.Projects
{
  public class SelfHostSettings
  {
    public SelfHostSettings(SelfHostBuildConfiguration buildConfiguration)
    {
      BuildConfiguration = buildConfiguration;
    }

    public SelfHostBuildConfiguration BuildConfiguration { get; }
  }
}