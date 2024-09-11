using Dashboard.DAL.ViewModels;
using FluentValidation;

namespace Dashboard.BLL.Validators
{
    public class SignUpValidator : AbstractValidator<SignUpVM>
    {
        public SignUpValidator() 
        {
            RuleFor(m => m.Email)
                .EmailAddress().WithMessage("Невірний формат пошти")
                .NotEmpty().WithMessage("Вкажіть пошту");
            RuleFor(m => m.Password)
                .MinimumLength(6).WithMessage("Пароль повинен містити мінімум 6 символів");
            RuleFor(m => m.ConfirmedPassword)
                .Equal(m => m.Password).WithMessage("Паролі не збігаються");
            RuleFor(m => m.Username)
                .NotEmpty().WithMessage("Вкажіть ім'я користувача");
        }
    }
}
