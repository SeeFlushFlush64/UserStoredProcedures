namespace PalaganasTechnicalExam.Models.ViewModels
{
    public class AddUserViewModel
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? ProfilePictureUrl { get; set; } // Optional
    }
}
