using UserManagementApi.Models;

namespace UserManagementApi.Services;

public interface ICarService
{
    Task<IEnumerable<Car>> GetAllAsync();
    Task<Car> GetByIdAsync(int id);
    Task<Car> CreateAsync(Car newCar);
    Task<bool> UpdateAsync(int id, Car updatedCar);
    Task<bool> DeleteAsync(int id);
}
