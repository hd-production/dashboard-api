using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
  public class CreateProjectCmd : IRequest<long>
  {
    public CreateProjectCmd(string name, int userId)
    {
      Name = name;
      UserId = userId;
    }

    public string Name { get; }
    public int UserId { get; }
  }
}