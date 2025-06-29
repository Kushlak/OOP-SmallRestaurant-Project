namespace SmallRestaurant.Data.Models
{
  public class Order
  {
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItem>? Items { get; set; }
    public decimal Total { get; set; }
    public Guid? AddressId { get; set; }
    public Address? Address { get; set; } = null!;
    public Guid? UserId { get; set; }
    public User? User { get; set; } = null!;
  }
}