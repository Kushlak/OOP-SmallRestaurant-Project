using SmallRestaurant.Data.Models;

namespace SmallRestaurant.Data.Services
{
  public interface IAddressService
  {
    Task<List<Address>> GetForUserAsync(string userId);
    Task<Address?> GetByIdAsync(Guid id);
    Task CreateAsync(Address a);
    Task UpdateAsync(Address a);
    Task DeleteAsync(Guid id);
    Task SetDefaultAsync(string userId, Guid id);
  }
}