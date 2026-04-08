using System.Security.Claims;
using HRSystem.API.DTOs;

namespace HRSystem.API.Validators
{
    using FluentValidation;
    

    public class CreateBranchValidator : AbstractValidator<CreateBranchDto>
    {
        public CreateBranchValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Branch name is required")
                .MaximumLength(100);

            RuleFor(x => x.Address)
                .MaximumLength(200);
        }
    }

}
