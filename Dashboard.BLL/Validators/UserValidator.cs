using Dashboard.DAL.ViewModels;
using FluentValidation;

namespace Dashboard.BLL.Validators
{
    public class UserValidator : AbstractValidator<UserVM>
    {
        public UserValidator() 
        {
            RuleFor(m => m.Email)
                .EmailAddress().WithMessage("Невірний формат пошти")
                .NotEmpty().WithMessage("Вкажіть пошту");
            RuleFor(m => m.UserName)
                .NotEmpty().WithMessage("Вкажіть ім'я користувача");
        }
    }
}
