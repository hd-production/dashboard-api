using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Exceptions;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

            (await new HttpClient().PostAsync(
                    "http://localhost:5001/projects",
                    new StringContent(
                        JsonConvert.SerializeObject(project, new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        }),
                        Encoding.UTF8,
                        "application/json"
                    ), 
                    cancellationToken)
                ).EnsureSuccessStatusCode();

            project.Run();
            await _projectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}