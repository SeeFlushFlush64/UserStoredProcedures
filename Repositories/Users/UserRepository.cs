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

        // ✅ Get all users (handles non-composable SQL issue)
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

        // ✅ Get user by ID (handles non-composable SQL issue)
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

        // ✅ Add user (Insert operation)
        public async Task AddUserAsync(User user)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC AddUser @FirstName, @LastName, @Email",
                    new SqlParameter("@FirstName", user.FirstName),
                    new SqlParameter("@LastName", user.LastName),
                    new SqlParameter("@Email", user.Email));
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error in AddUserAsync: {ex.Message}");
                throw;
            }
        }

        // ✅ Update user (Update operation)
        public async Task UpdateUserAsync(User user)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC UpdateUser @UserId, @FirstName, @LastName, @Email",
                    new SqlParameter("@UserId", user.UserId),
                    new SqlParameter("@FirstName", user.FirstName),
                    new SqlParameter("@LastName", user.LastName),
                    new SqlParameter("@Email", user.Email));
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error in UpdateUserAsync: {ex.Message}");
                throw;
            }
        }

        // ✅ Delete user (Delete operation)
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

        public async Task<List<User>> SearchUsersAsync(string searchQuery)
        {
            return await _context.Users
                .FromSqlInterpolated($"EXEC SearchUsers {searchQuery}")
                .ToListAsync();
        }

    }
}
