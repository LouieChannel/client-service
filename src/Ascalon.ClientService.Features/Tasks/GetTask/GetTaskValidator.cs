using FluentValidation;

namespace Ascalon.ClientService.Features.Tasks.GetTask
{
    public class GetTaskValidator : AbstractValidator<GetTaskQuery>
    {
        public GetTaskValidator()
        {
            RuleFor(u => u.Id).GreaterThan(0);
        }
    }
}
