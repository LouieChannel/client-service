using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

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
            RuleFor(u => u.DriverId).GreaterThan(0);
            RuleFor(u => u.Description).NotEmpty().NotNull();
            RuleFor(u => u.Entity).NotEmpty().NotNull();
            RuleFor(u => u.Id).GreaterThan(0);
        }
    }
}
