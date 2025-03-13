using FluentValidation;
using PalaganasTechnicalExam.Models.ViewModels;

namespace PalaganasTechnicalExam.Validators
{
    public class EditUserValidator : AbstractValidator<EditUserViewModel>
    {
        public EditUserValidator()
        {
            RuleFor(u => u.FirstName)
                .MaximumLength(20).WithMessage("First Name cannot be longer than 20 characters!")
                .NotNull().WithMessage("First Name cannot be empty!") // Changed from NotEmpty() to NotNull()
                .NotEmpty().WithMessage("First Name cannot be empty!")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("First Name can only contain letters!");
            RuleFor(u => u.LastName)
                .MaximumLength(20).WithMessage("Last Name cannot be longer than 20 characters!")
                .NotNull().WithMessage("Last Name cannot be empty!") // Changed from NotEmpty() to NotNull()
                .NotEmpty().WithMessage("Last Name cannot be empty!")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Last Name can only contain letters!");
            RuleFor(u => u.Email)
                .EmailAddress().WithMessage("Invalid email format!")
                .NotNull().WithMessage("Email cannot be empty!") // Changed from NotEmpty() to NotNull()
                .NotEmpty().WithMessage("Email cannot be empty!");
            RuleFor(u => u.ProfilePictureUrl)
                .Must(BeAValidUrl).WithMessage("Profile Picture URL must be a valid URL!")
                .When(u => !string.IsNullOrEmpty(u.ProfilePictureUrl)); // Only validate if not empty

        }
        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }

    }
}
