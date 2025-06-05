using Microsoft.EntityFrameworkCore;
using UserManagementApi.Data;
using UserManagementApi.Models;

namespace UserManagementApi.Services;

public class CarService : ICarService
{
    private readonly AutoShopDbContext _db;

    public CarService(AutoShopDbContext db) {
        _db = db;
    }

    public async Task<IEnumerable<Car>> GetAllAsync() {
        return await _db.Cars.ToListAsync();
    }

    public async Task<Car> GetByIdAsync(int id) {
        return await _db.Cars.FindAsync(id);
    }

    public async Task<Car> CreateAsync(Car newCar) {
        var now = DateTime.UtcNow;
        newCar.CreatedAt = now;
        newCar.UpdatedAt = now;

        _db.Cars.Add(newCar);
        await _db.SaveChangesAsync();
        return newCar;
    }

    public async Task<bool> UpdateAsync(int id, Car updatedCar) {
        var existing = await _db.Cars.FindAsync(id);
        if (existing is null) return false;

        existing.Company = updatedCar.Company;
        existing.Model = updatedCar.Model;
        existing.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id) {
        var existing = await _db.Cars.FindAsync(id);
        if (existing is null) return false;

        _db.Cars.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HasUsersAsync(int carId) {
        return await _db.Users.AnyAsync(u => u.CarId == carId);
    }
}

