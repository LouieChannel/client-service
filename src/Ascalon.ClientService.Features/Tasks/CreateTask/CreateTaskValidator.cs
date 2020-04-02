using FluentValidation;

namespace Ascalon.ClientService.Features.Tasks.CreateTask
{
    public class CreateTaskValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskValidator()
        {
            RuleFor(u => u.StartLongitude).GreaterThan(0);
            RuleFor(u => u.StartLatitude).GreaterThan(0);
            RuleFor(u => u.EndLatitude).GreaterThan(0);
            RuleFor(u => u.EndLongitude).GreaterThan(0);
            RuleFor(u => u.DriverId).GreaterThan(0);
            RuleFor(u => u.Description).NotEmpty().NotNull();
            RuleFor(u => u.Entity).NotEmpty().NotNull();
        }
    }
}
