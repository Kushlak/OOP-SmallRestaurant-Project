using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmallRestaurant.Data.Context;
using SmallRestaurant.Data.Models;
using SmallRestaurant.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// EF Core + Postgres
builder.Services.AddDbContext<AppDbContext>(opt =>
  opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Identity-style hashing
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Protected session + наш AuthService
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<IUserService,  UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<AuthService>());

builder.Services.AddAuthorizationCore();

// Решта вашої реєстрації сервісів (DishService, OrderService тощо)
builder.Services.AddScoped<IDishService,  DishService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<ICommentService, CommentService>();


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// автоматичні міграції + начальний seed адміна
using (var scope = app.Services.CreateScope())
{
  var db     = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

  // 1) Автоматично застосувати всі міграції
  db.Database.Migrate();

  // 2) Якщо в таблиці користувачів нема admin — створити
  if (!db.Users.Any(u => u.UserName == "admin"))
  {
    var admin = new User
    {
      UserName = "admin",
      Role     = "Admin"
    };
    // Hash-пароль “admin” (міняйте на щось складніше!)
    admin.PasswordHash = hasher.HashPassword(admin, "admin");
    db.Users.Add(admin);
    db.SaveChanges();
  }
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();