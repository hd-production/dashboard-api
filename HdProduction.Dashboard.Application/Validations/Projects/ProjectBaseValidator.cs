using System.Threading.Tasks;
using HdProduction.Dashboard.Application.Commands.Projects;
using HdProduction.Dashboard.Domain.Entities.Projects;
using HdProduction.Dashboard.Infrastructure.Validation;

namespace HdProduction.Dashboard.Application.Validations.Projects
{
    public class ProjectBaseValidator<T> : Validator<T> where T : BaseProjectCmd
    {
        protected override Task SetValidations()
        {
            RuleFor(t => t.Name).NotEmpty()
                .WithMessage("Name can not be empty");
            RuleFor(t => t.Name.Length).LessOrEqual(128)
                .WithMessage("Name is too long");

            RuleFor(t => t.SelfHostSettings)
                .SetModelValidator(new SelfHostSettingsValidator())
                .When(t => t.SelfHostSettings != null);

            return Task.CompletedTask;
        }
    }

    public class SelfHostSettingsValidator : Validator<SelfHostSettings>
    {
        protected override Task SetValidations()
        {
            RuleFor(t => t.BuildConfiguration).ValidEnum()
                .WithMessage("Invalid build configuration");

            return Task.CompletedTask;
        }
    }
}