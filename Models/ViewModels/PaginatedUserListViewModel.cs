using PalaganasTechnicalExam.Models.Entities;

namespace PalaganasTechnicalExam.Models.ViewModels
{
    public class PaginatedUserListViewModel
    {
        public List<User> Users { get; set; } = new List<User>();
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public string? SearchQuery { get; set; }
    }
}
