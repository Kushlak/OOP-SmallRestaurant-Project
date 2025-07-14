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
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DeliveryType Delivery { get; set; } = DeliveryType.Pickup;
  }

  public enum DeliveryType
  {
    Pickup = 0,     
    Courier = 1     
  }

  public static class DeliveryHelper
  {
    public static decimal GetPrice(DeliveryType type) => type switch
    {
      DeliveryType.Pickup => 0,
      DeliveryType.Courier => 10,
      _ => 0
    };
  }
}