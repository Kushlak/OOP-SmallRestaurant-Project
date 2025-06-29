using Microsoft.EntityFrameworkCore;
using SmallRestaurant.Data.Context;
using SmallRestaurant.Data.Models;

namespace SmallRestaurant.Data.Services
{
  public class DishService : IDishService
  {
    private readonly AppDbContext _db;
    public DishService(AppDbContext db) => _db = db;

    public async Task<List<Dish>> GetAllAsync(string? searchTerm = null) =>
      await _db.Dishes
        .Where(d => string.IsNullOrWhiteSpace(searchTerm)
                    || d.Name.Contains(searchTerm!))
        .ToListAsync();

    public Task<Dish?> GetByIdAsync(Guid id) =>
      _db.Dishes.FindAsync(id).AsTask();

    public async Task CreateAsync(Dish dish)
    {
      _db.Dishes.Add(dish);
      await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Dish dish)
    {
      _db.Dishes.Update(dish);
      await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
      var d = await _db.Dishes.FindAsync(id);
      if (d != null) { _db.Dishes.Remove(d); await _db.SaveChangesAsync(); }
    }
  }

}
