using Microsoft.EntityFrameworkCore;
using UserManagementApi.Data;
using UserManagementApi.Models;

namespace UserManagementApi.Services;

public class UserService : IUserService
{
    private readonly AutoShopDbContext _db;

    public UserService(AutoShopDbContext db) {
        _db = db;
    }

    public async Task<IEnumerable<User>> GetAllAsync() {
        return await _db.Users
                         .Include(u => u.Car)
                         .ToListAsync();
    }

    public async Task<User> GetByIdAsync(int id) {
        return await _db.Users
                        .Include(u => u.Car)
                        .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> CreateAsync(User newUser) {
        var now = DateTime.UtcNow;
        newUser.CreatedAt = now;
        newUser.UpdatedAt = now;

        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();
        return newUser;
    }

    public async Task<bool> UpdateAsync(int id, User updatedUser) {
        var existing = await _db.Users.FindAsync(id);
        if (existing is null) return false;

        existing.Name = updatedUser.Name;
        existing.Email = updatedUser.Email;
        existing.Password = updatedUser.Password;
        existing.CarId = updatedUser.CarId;       
        existing.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id) {
        var existing = await _db.Users.FindAsync(id);
        if (existing is null) return false;

        _db.Users.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }
}
