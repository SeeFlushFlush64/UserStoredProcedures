using FluentValidation;
using PalaganasTechnicalExam.Models.ViewModels;

namespace PalaganasTechnicalExam.Validators
{
    public class AddUserValidator : AbstractValidator<AddUserViewModel>
    {
        public AddUserValidator()
        {
            RuleFor(u => u.FirstName)
                .MaximumLength(20).WithMessage("First Name cannot be longer than 20 characters!")
                .NotNull().WithMessage("First Name cannot be empty!") // Changed from NotEmpty() to NotNull()
                .NotEmpty().WithMessage("First Name cannot be empty!");
            RuleFor(u => u.LastName)
                .MaximumLength(20).WithMessage("Last Name cannot be longer than 20 characters!")
                .NotNull().WithMessage("Last Name cannot be empty!") // Changed from NotEmpty() to NotNull()
                .NotEmpty().WithMessage("Last Name cannot be empty!");
            RuleFor(u => u.Email)
                .EmailAddress().WithMessage("Invalid email format!")
                .NotNull().WithMessage("Email cannot be empty!") // Changed from NotEmpty() to NotNull()
                .NotEmpty().WithMessage("Email cannot be empty!");
        }
    }
}
