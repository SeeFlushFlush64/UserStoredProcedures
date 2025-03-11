﻿using PalaganasTechnicalExam.Models.Entities;

namespace PalaganasTechnicalExam.Repositories.Users
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
        Task<List<User>> SearchUsersAsync(string searchQuery);
    }

}
