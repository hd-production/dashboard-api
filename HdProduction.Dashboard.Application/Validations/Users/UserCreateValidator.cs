using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Application.Commands.Users;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Infrastructure.Validation;
using MediatR.Pipeline;

namespace HdProduction.Dashboard.Application.Validations.Users
{
    public class UserCreateValidator : Validator<CreateUserCmd>, IRequestPreProcessor<CreateUserCmd>
    {
        private readonly IUserRepository _repository;

        public UserCreateValidator(IUserRepository repository)
        {
            _repository = repository;
        }

        protected override Task SetValidations()
        {
            RuleFor(u => u.Email).NotEmpty().Must(IsValidEmail)
                .WithMessage("Email has invalid format");
            RuleFor(u => u.Email.Length).LessOrEqual(254)
                .WithMessage("Email is too long");
            RuleFor(u => u.Email).Must(async e => await _repository.FindByEmailAsync(e) == null)
                .WithMessage("User with same email already exists");

            RuleFor(u => u.FirstName).NotEmpty()
                .WithMessage("First name can't be empty");
            RuleFor(u => u.FirstName.Length).LessOrEqual(128)
                .WithMessage("First name is too long");

            RuleFor(u => u.LastName).NotEmpty()
                .WithMessage("Last name can't be empty");
            RuleFor(u => u.LastName.Length).LessOrEqual(128)
                .WithMessage("Last name is too long");

            return Task.CompletedTask;
        }

        public Task Process(CreateUserCmd request, CancellationToken cancellationToken)
        {
            return CheckAsync(request, cancellationToken);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}