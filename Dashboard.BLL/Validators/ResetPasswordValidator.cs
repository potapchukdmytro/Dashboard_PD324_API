using Dashboard.DAL.ViewModels;
using FluentValidation;

namespace Dashboard.BLL.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordVM>
    {
        public ResetPasswordValidator() 
        {
            RuleFor(m => m.Password)
                 .MinimumLength(6).WithMessage("Пароль повинен містити мінімум 6 символів");
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id не може бути порожнім");
            RuleFor(m => m.Token)
                .NotEmpty().WithMessage("Токен не може бути порожнім");
        }
    }
}
