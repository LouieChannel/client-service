using FluentValidation;
using System;

namespace Ascalon.ClientService.Features.Tasks.GetAllTask
{
    public class GetAllTaskValidator : AbstractValidator<GetAllTaskQuery>
    {
        public GetAllTaskValidator()
        {
            RuleFor(u => u.DateFilter).NotEqual(default(DateTime));
        }
    }
}
