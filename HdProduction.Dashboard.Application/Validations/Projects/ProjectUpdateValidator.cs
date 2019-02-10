using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Application.Commands.Projects;
using HdProduction.Dashboard.Domain.Contracts;
using MediatR.Pipeline;

namespace HdProduction.Dashboard.Application.Validations.Projects
{
    public class ProjectUpdateValidator : ProjectBaseValidator<UpdateProjectCmd>, IRequestPreProcessor<UpdateProjectCmd>
    {
        private readonly IProjectRepository _repository;

        public ProjectUpdateValidator(IProjectRepository repository)
        {
            _repository = repository;
        }

        protected override Task SetValidations()
        {
            RuleFor(p => p.Id).Must(async id => await _repository.FindAsync(id) != null)
                .WithMessage("Project is not found")
                .ThrowsNotFound();
            
            return base.SetValidations();
        }

        public Task Process(UpdateProjectCmd request, CancellationToken cancellationToken)
        {
            return CheckAsync(request, cancellationToken);
        }
    }
}