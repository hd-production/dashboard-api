using HdProduction.Dashboard.Application.Models;

namespace HdProduction.Dashboard.Api.Models.Projects
{
    public class ProjectUpdateRequestModel : ProjectCreateRequestModel
    {
        public SelfHostSettingsReadModel SelfHostSettings { get; set; }
        public DefaultAdminSettingsReadModel DefaultAdminSettings { get; set; }
    }
}