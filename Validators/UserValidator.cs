using FluentValidation;
using PalaganasTechnicalExam.Models.Entities;

namespace PalaganasTechnicalExam.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("First Name cannot be empty!")
                .MaximumLength(20).WithMessage("First Name cannot be longer than 20 characters!");
            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("Last Name cannot be empty!")
                .MaximumLength(20).WithMessage("Last Name cannot be longer than 20 characters!");
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email cannot be empty!")
                .EmailAddress().WithMessage("Invalid email format!");
            //RuleFor(u => u.FirstName)
            //    .MaximumLength(20).WithMessage("First Name cannot be longer than 20 characters!")
            //    .NotEmpty().WithMessage("First Name cannot be empty!");
            //RuleFor(u => u.LastName)
            //    .MaximumLength(20).WithMessage("Last Name cannot be longer than 20 characters!")
            //    .NotEmpty().WithMessage("Last Name cannot be empty!");
            //RuleFor(u => u.Email)
            //    .EmailAddress().WithMessage("Invalid email format!")
            //    .NotEmpty().WithMessage("Email cannot be empty!");
        }
    }
}
