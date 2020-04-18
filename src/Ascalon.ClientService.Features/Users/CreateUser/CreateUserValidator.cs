using FluentValidation;

namespace Ascalon.ClientService.Features.Users.CreateUser
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(u => u.FullName).NotNull().NotEmpty().Length(1, 120);

            RuleFor(u => u.Password).NotNull().NotEmpty().Length(1, 60);

            RuleFor(u => u.Role).NotNull().NotEmpty().IsInEnum();

            RuleFor(u => u.Login).NotNull().NotEmpty().Length(1, 60);
        }
    }
}
