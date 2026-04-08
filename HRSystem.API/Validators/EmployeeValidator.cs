namespace HRSystem.API.Validators
{
    using FluentValidation;
    

    namespace HRSystem.API.Validators
    {
        public class EmployeeValidator : AbstractValidator<Employee>
        {
            public EmployeeValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("الاسم مطلوب")
                    .MaximumLength(100);

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("الإيميل مطلوب")
                    .EmailAddress().WithMessage("الإيميل غير صحيح");

                RuleFor(x => x.Phone)
                    .NotEmpty().WithMessage("رقم الجوال مطلوب");

                RuleFor(x => x.Salary)
                    .GreaterThan(0).WithMessage("الراتب لازم يكون أكبر من 0");

                RuleFor(x => x.BranchId)
                    .GreaterThan(0).WithMessage("لازم تختار فرع");

               
            }
        }
    }
}
