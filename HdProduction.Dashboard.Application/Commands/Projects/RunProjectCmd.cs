using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
    public class RunProjectCmd : IRequest
    {
        public RunProjectCmd(long id, long userId)
        {
            Id = id;
            UserId = userId;
        }

        public long Id { get; }
        public long UserId { get; }
    }
}