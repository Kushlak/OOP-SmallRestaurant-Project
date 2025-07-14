using SmallRestaurant.Data.Models;

public class Comment
{
  public Guid Id { get; set; }
  public string Text { get; set; } = null!;
  public int Rating { get; set; } 
  public DateTime CreatedAt { get; set; }

  public Guid DishId { get; set; }
  public Dish Dish { get; set; } = null!;

  public Guid UserId { get; set; }
  public User User { get; set; } = null!;
}