using Dashboard.DAL.ViewModels;
using FluentValidation;

namespace Dashboard.BLL.Validators
{
    public class SignInValidator : AbstractValidator<SignInVM>
    {
        public SignInValidator() 
        {
            RuleFor(m => m.Email)
                .EmailAddress().WithMessage("Невірний формат пошти")
                .NotEmpty().WithMessage("Вкажіть пошту");
            RuleFor(m => m.Password)
                .MinimumLength(6).WithMessage("Пароль повинен містити мінімум 6 символів");
        }
    }
}
