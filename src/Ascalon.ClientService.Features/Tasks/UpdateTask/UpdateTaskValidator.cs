using FluentValidation;

namespace Ascalon.ClientService.Features.Tasks.UpdateTask
{
    class UpdateTaskValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskValidator()
        {
            RuleFor(u => u.StartLongitude).GreaterThan(0);
            RuleFor(u => u.StartLatitude).GreaterThan(0);

            RuleFor(u => u.EndLatitude).GreaterThan(0);
            RuleFor(u => u.EndLongitude).GreaterThan(0);

            RuleFor(u => u.Description).NotEmpty().NotNull();

            RuleFor(u => u.Entity).NotEmpty().NotNull();

            RuleFor(u => u.Id).GreaterThan(0);

            RuleFor(u => u.Driver).NotNull();
            RuleFor(u => u.Driver.Id).GreaterThan(0);
            RuleFor(u => u.Driver.FullName).NotEmpty().NotNull();

            RuleFor(u => u.Logist).NotNull();
            RuleFor(u => u.Logist.Id).GreaterThan(0);
            RuleFor(u => u.Logist.FullName).NotEmpty().NotNull();
        }
    }
}
