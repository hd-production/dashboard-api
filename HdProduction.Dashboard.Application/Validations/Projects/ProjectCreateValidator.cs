using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Application.Commands.Projects;
using MediatR.Pipeline;

namespace HdProduction.Dashboard.Application.Validations.Projects
{
  public class ProjectCreateValidator : ProjectBaseValidator<CreateProjectCmd>, IRequestPreProcessor<CreateProjectCmd>
  {
    public Task Process(CreateProjectCmd request, CancellationToken cancellationToken)
    {
      return CheckAsync(request, cancellationToken);
    }
  }
}