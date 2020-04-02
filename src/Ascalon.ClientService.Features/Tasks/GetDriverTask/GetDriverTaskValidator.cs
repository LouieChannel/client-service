using FluentValidation;

namespace Ascalon.ClientService.Features.Tasks.GetDriverTask
{
    public class GetDriverTaskValidator : AbstractValidator<GetDriverTaskQuery>
    {
        public GetDriverTaskValidator()
        {
            RuleFor(u => u.DriverId).GreaterThan(0);
        }
    }
}
