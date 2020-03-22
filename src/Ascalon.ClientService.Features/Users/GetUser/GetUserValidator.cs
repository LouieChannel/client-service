using FluentValidation;

namespace Ascalon.ClientService.Features.Users.GetUser
{
    public class GetUserValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserValidator()
        {
            RuleFor(u => u.Login).NotNull().NotEmpty();

            RuleFor(u => u.Password).NotNull().NotEmpty();
        }
    }
}
