using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Exceptions;
using MediatR;
using Newtonsoft.Json;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
    public class RunProjectCmdHandler : IRequestHandler<RunProjectCmd>
    {
        private readonly IProjectRepository _projectRepository;

        public RunProjectCmdHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Unit> Handle(RunProjectCmd request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.FindAsync(request.Id)
                ?? throw new EntityNotFoundException("Project");
            var httpClient = new HttpClient();
            await httpClient.PostAsync("http://localhost:5001/projects",
                new StringContent(JsonConvert.SerializeObject(project)), cancellationToken);
            return Unit.Value;
        }
    }
}