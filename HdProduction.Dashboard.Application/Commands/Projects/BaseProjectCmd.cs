using HdProduction.Dashboard.Domain.Entities.Projects;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
    public class BaseProjectCmd
    {
        protected BaseProjectCmd(string name, SelfHostSettings selfHostSettings, DefaultAdminSettings defaultAdminSettings, long userId)
        {
            Name = name;
            UserId = userId;
            SelfHostSettings = selfHostSettings;
            DefaultAdminSettings = defaultAdminSettings;
        }

        public string Name { get; }
        public SelfHostSettings SelfHostSettings { get; }
        public DefaultAdminSettings DefaultAdminSettings { get; }
        public long UserId { get; }
    }
}