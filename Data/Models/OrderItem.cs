using System;

namespace SmallRestaurant.Data.Models
{
  public class OrderItem
  {
    public Guid Id { get; set; }

    public Guid DishId { get; set; }

    public string DishName { get; set; } = null!;
    public decimal Price { get; set; }

    public int Quantity { get; set; }
  }
}