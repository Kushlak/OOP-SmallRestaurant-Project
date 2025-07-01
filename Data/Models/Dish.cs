using System;

namespace SmallRestaurant.Data.Models
{
  public class Dish
  {
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }

    // ← Додано для посилання на картинку в wwwroot/images/dishes
    public string? ImageUrl { get; set; }
    public List<Comment> Comments { get; set; } = new();
  }
}