using Microsoft.EntityFrameworkCore;
using SmallRestaurant.Data.Models;

namespace SmallRestaurant.Data.Context
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options)
      : base(options)
    { }

    public DbSet<Dish>      Dishes      { get; set; }
    public DbSet<Comment>   Comments    { get; set; }
    public DbSet<User>      Users       { get; set; }
    public DbSet<Address>   Addresses   { get; set; }
    public DbSet<Order>     Orders      { get; set; }
    public DbSet<OrderItem> OrderItems  { get; set; }
  }
}