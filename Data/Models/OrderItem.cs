using System;

namespace SmallRestaurant.Data.Models
{
  public class OrderItem
  {
    public Guid Id { get; set; }

    // foreign key to Dish
    public Guid DishId { get; set; }

    // new: store snapshot of the dish’s name and price
    public string DishName { get; set; } = null!;
    public decimal Price { get; set; }

    public int Quantity { get; set; }
  }
}