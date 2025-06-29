using SmallRestaurant.Data.Models;

namespace SmallRestaurant.Data.Services
{
  public interface IDishService
  {
    Task<List<Dish>> GetAllAsync(string? searchTerm = null);
    Task<Dish?>    GetByIdAsync(Guid id);
    Task          CreateAsync(Dish dish);
    Task          UpdateAsync(Dish dish);
    Task          DeleteAsync(Guid id);
  }
}