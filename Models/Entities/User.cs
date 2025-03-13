namespace PalaganasTechnicalExam.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? ProfilePictureUrl { get; set; } // Nullable field
    }

}
