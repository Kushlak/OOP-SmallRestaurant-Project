namespace SmallRestaurant.Data.Services
{
  using SmallRestaurant.Data.Models;
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  public interface IOrderService
  {
    Task AddToCartAsync(Dish dish);
    Task RemoveOneFromCartAsync(Guid dishId);
    Task RemoveFromCartAsync(Guid dishId);
    Task ClearCartAsync();
    Task<List<OrderItem>> GetCartItemsAsync();
    Task<decimal> GetCartTotalAsync();
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(Guid id);
    Task PlaceOrderAsync(Guid addressId);
  }
}