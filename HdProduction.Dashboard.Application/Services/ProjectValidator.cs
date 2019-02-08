using System;
using System.Threading.Tasks;
using HdProduction.Dashboard.Application.Commands.Projects;
using HdProduction.Dashboard.Domain.Entities.Projects;
using HdProduction.Dashboard.Infrastructure.Validation;

namespace HdProduction.Dashboard.Application.Services
{
  public class ProjectValidator : ValidatorR<CreateProjectCmd, long>
  {
    protected override Task SetValidations()
    {
      RuleFor(t => t.Name).Must(n => !string.IsNullOrWhiteSpace(n))
        .WithMessage("Name can not be empty");

      RuleFor(t => t.SelfHostSettings.BuildConfiguration)
        .Must(b => Enum.IsDefined(typeof(SelfHostBuildConfiguration), b))
        .WithMessage("Name can not be empty");

      return Task.CompletedTask;
    }
  }
}