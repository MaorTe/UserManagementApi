using UserManagementApi.Models;

namespace UserManagementApi.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByIdAsync(int id);
    Task<User> CreateAsync(User newUser);
    Task<bool> UpdateAsync(int id, User updatedUser);
    Task<bool> DeleteAsync(int id);
}
