using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmallRestaurant.Data.Context;
using SmallRestaurant.Data.Models;

namespace SmallRestaurant.Data.Services
{
  public class UserService : IUserService
  {
    private readonly AppDbContext             _db;
    private readonly IPasswordHasher<User>     _hasher;

    public UserService(AppDbContext db, IPasswordHasher<User> hasher)
    {
      _db     = db;
      _hasher = hasher;
    }

    public async Task<bool> RegisterAsync(string userName, string password)
    {
      if (await _db.Users.AnyAsync(u => u.UserName == userName))
        return false;

      var user = new User { UserName = userName, Role = "User" };
      user.PasswordHash = _hasher.HashPassword(user, password);

      _db.Users.Add(user);
      await _db.SaveChangesAsync();
      return true;
    }

    public async Task<User?> ValidateCredentialsAsync(string userName, string password)
    {
      var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userName);
      if (user == null) return null;

      var res = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
      return res == PasswordVerificationResult.Success ? user : null;
    }

    public Task<User?> GetByUserNameAsync(string userName)
      => _db.Users.FirstOrDefaultAsync(u => u.UserName == userName);
  }
}