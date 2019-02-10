using HdProduction.Dashboard.Domain.Entities.Projects;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
    public class BaseProjectCmd
    {
        public BaseProjectCmd(string name, SelfHostSettings selfHostSettings, long userId)
        {
            Name = name;
            SelfHostSettings = selfHostSettings;
            UserId = userId;
        }

        public string Name { get; }
        public SelfHostSettings SelfHostSettings { get; }
        public long UserId { get; }
    }
}