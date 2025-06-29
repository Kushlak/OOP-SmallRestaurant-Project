using SmallRestaurant.Data.Models;

namespace SmallRestaurant.Data.Services
{
  public interface IUserService
  {
    Task<bool>   RegisterAsync(string userName, string password);
    Task<User?>  ValidateCredentialsAsync(string userName, string password);
    Task<User?>  GetByUserNameAsync(string userName);
  }
}
