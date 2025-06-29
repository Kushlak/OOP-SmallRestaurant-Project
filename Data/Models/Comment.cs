using System;

namespace SmallRestaurant.Data.Models
{
  public class Comment
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DishId { get; set; }        // FK
    public string UserName { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  }
}