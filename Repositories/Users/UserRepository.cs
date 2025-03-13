using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PalaganasTechnicalExam.Data;
using PalaganasTechnicalExam.Models.Entities;

namespace PalaganasTechnicalExam.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Get all users (Updated to include ProfilePictureUrl)
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users
                    .FromSqlRaw("EXEC GetAllUsers")
                    .ToListAsync();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error in GetAllUsersAsync: {ex.Message}");
                throw;
            }
        }

        // ✅ Get user by ID (Updated to include ProfilePictureUrl)
        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _context.Users
                    .FromSqlRaw("EXEC GetUserById @UserId", new SqlParameter("@UserId", userId))
                    .ToListAsync();

                return user.FirstOrDefault();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error in GetUserByIdAsync: {ex.Message}");
                throw;
            }
        }

        // ✅ Add user (Updated to include ProfilePictureUrl)
        public async Task AddUserAsync(User user)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC AddUser @FirstName, @LastName, @Email, @ProfilePictureUrl",
                    new SqlParameter("@FirstName", user.FirstName),
                    new SqlParameter("@LastName", user.LastName),
                    new SqlParameter("@Email", user.Email),
                    new SqlParameter("@ProfilePictureUrl", (object?)user.ProfilePictureUrl ?? DBNull.Value));
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error in AddUserAsync: {ex.Message}");
                throw;
            }
        }

        // ✅ Update user (Updated to include ProfilePictureUrl)
        public async Task UpdateUserAsync(User user)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC UpdateUser @UserId, @FirstName, @LastName, @Email, @ProfilePictureUrl",
                    new SqlParameter("@UserId", user.UserId),
                    new SqlParameter("@FirstName", user.FirstName),
                    new SqlParameter("@LastName", user.LastName),
                    new SqlParameter("@Email", user.Email),
                    new SqlParameter("@ProfilePictureUrl", (object?)user.ProfilePictureUrl ?? DBNull.Value));
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error in UpdateUserAsync: {ex.Message}");
                throw;
            }
        }

        // ✅ Delete user (No changes needed)
        public async Task DeleteUserAsync(int userId)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteUser @UserId",
                    new SqlParameter("@UserId", userId));
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error in DeleteUserAsync: {ex.Message}");
                throw;
            }
        }

        // ✅ Search users (Updated to include ProfilePictureUrl)
        public async Task<List<User>> SearchUsersAsync(string searchQuery)
        {
            return await _context.Users
                .FromSqlRaw("EXEC SearchUsers @SearchQuery", new SqlParameter("@SearchQuery", searchQuery))
                .ToListAsync();
        }

        // ✅ Get sorted users (Updated to include ProfilePictureUrl)
        public async Task<List<User>> GetSortedUsersAsync(string? searchQuery, string sortColumn, string sortOrder, int pageNumber, int pageSize)
        {
            return await _context.Users.FromSqlRaw(
                "EXEC GetSortedUsers @SearchQuery, @SortColumn, @SortOrder, @PageNumber, @PageSize",
                new SqlParameter("@SearchQuery", searchQuery ?? (object)DBNull.Value),
                new SqlParameter("@SortColumn", sortColumn),
                new SqlParameter("@SortOrder", sortOrder),
                new SqlParameter("@PageNumber", pageNumber),
                new SqlParameter("@PageSize", pageSize)
            ).ToListAsync();
        }

        // ✅ Get total user count (No changes needed)
        public int GetTotalUserCount()
        {
            return _context.Users.Count();
        }
    }
}
