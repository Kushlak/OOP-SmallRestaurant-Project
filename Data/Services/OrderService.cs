using Microsoft.EntityFrameworkCore;
using SmallRestaurant.Data.Context;
using SmallRestaurant.Data.Models;


namespace SmallRestaurant.Data.Services
{
    public class OrderService : IOrderService
    {
        public async Task DeleteAsync(Guid orderId)
        {
            var order = await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order != null)
            {
                _db.OrderItems.RemoveRange(order.Items);
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
            }
        }
        
        private readonly AppDbContext _db;
        private readonly List<OrderItem> _cart = new();

        public OrderService(AppDbContext db) => _db = db;

        public Task AddToCartAsync(Dish dish)
        {
            var existing = _cart.FirstOrDefault(i => i.DishId == dish.Id);
            if (existing != null)
                existing.Quantity++;
            else
                _cart.Add(new OrderItem {
                    Id = Guid.NewGuid(),
                    DishId = dish.Id,
                    DishName = dish.Name,
                    Price = dish.Price,
                    Quantity = 1
                });
            return Task.CompletedTask;
        }

        public Task RemoveOneFromCartAsync(Guid dishId)
        {
            var item = _cart.FirstOrDefault(i => i.DishId == dishId);
            if (item != null)
            {
                if (item.Quantity > 1) item.Quantity--;
                else _cart.Remove(item);
            }
            return Task.CompletedTask;
        }

        public Task RemoveFromCartAsync(Guid dishId)
        {
            _cart.RemoveAll(i => i.DishId == dishId);
            return Task.CompletedTask;
        }

        public Task ClearCartAsync()
        {
            _cart.Clear();
            return Task.CompletedTask;
        }

        public Task<List<OrderItem>> GetCartItemsAsync()
            => Task.FromResult(_cart.ToList());

        public Task<decimal> GetCartTotalAsync()
            => Task.FromResult(_cart.Sum(i => i.Price * i.Quantity));


        public async Task PlaceOrderAsync(Guid? addressId, Guid userId,decimal totalPrice)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                AddressId = addressId, 
                UserId = userId,
                Delivery = addressId != null  ?  Models.DeliveryType.Courier : Models.DeliveryType.Pickup,
                Items = _cart.Select(i => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    DishId = i.DishId,
                    DishName = i.DishName,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            };
            order.Total = totalPrice;

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            _cart.Clear();
        }


        public Task<List<Order>> GetAllAsync()
            => _db.Orders
                  .Include(o => o.Items)
                  .OrderByDescending(o => o.CreatedAt)
                  .ToListAsync();

        public Task<Order?> GetByIdAsync(Guid id)
            => _db.Orders
                .Include(o => o.Items)
                .Include(o => o.Address)
                .FirstOrDefaultAsync(o => o.Id == id);
     
        public Task<List<Order>> GetByUserIdAsync(Guid userId)
        {
            return _db.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                .Include(o => o.Address)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
        
        public async Task UpdateOrderAsync(Order updatedOrder)
        {
            var existing = await _db.Orders
                .Include(o => o.Address)
                .FirstOrDefaultAsync(o => o.Id == updatedOrder.Id);

            if (existing is not null)
            {
                existing.Status = updatedOrder.Status;

                if (existing.Address is not null && updatedOrder.Address is not null)
                {
                    existing.Address.Street = updatedOrder.Address.Street;
                    existing.Address.City = updatedOrder.Address.City;
                    existing.Address.PostalCode = updatedOrder.Address.PostalCode;
                }

                await _db.SaveChangesAsync();
            }
        }

    }
}
