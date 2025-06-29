// OrderService.cs
using Microsoft.EntityFrameworkCore;
using SmallRestaurant.Data.Context;
using SmallRestaurant.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmallRestaurant.Data.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _db;
        // тимчасово тримаємо кошик в пам'яті
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


        // ——————————————————————————————
        // Тепер — збережені замовлення в БД
        public async Task PlaceOrderAsync(Guid addressId)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                AddressId = addressId,
                Items = _cart.Select(i => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    DishId = i.DishId,
                    DishName = i.DishName,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            };
            order.Total = order.Items.Sum(i => i.Price * i.Quantity);

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

    }
}
