using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SmallRestaurant.Data.Models;

namespace SmallRestaurant.Data.Services
{
  public class AuthService : AuthenticationStateProvider
  {
    private const string SessionKey = "smallrestaurant_user";
    private readonly ProtectedSessionStorage _session;
    private readonly IUserService            _users;
    private ClaimsPrincipal                  _anon = new ClaimsPrincipal(new ClaimsIdentity());

    public AuthService(ProtectedSessionStorage session, IUserService users)
    {
      _session = session;
      _users   = users;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
      var stored = await _session.GetAsync<string>(SessionKey);
      if (stored.Success && !string.IsNullOrEmpty(stored.Value))
      {
        var parts = stored.Value.Split('|');
        var user = await _users.GetByUserNameAsync(parts[0])!;
        var idt = new ClaimsIdentity(new[]
        {
          new Claim(ClaimTypes.Name, parts[0]),
          new Claim(ClaimTypes.Role, parts[1]),
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        }, "custom");
        return new AuthenticationState(new ClaimsPrincipal(idt));
      }
      return new AuthenticationState(_anon);
    }

    public async Task<bool> RegisterAsync(string userName, string password)
    {
      var ok = await _users.RegisterAsync(userName, password);
      if (!ok) return false;

      // після успішної реєстрації — автологін
      var user = await _users.GetByUserNameAsync(userName)!;
      await _session.SetAsync(SessionKey, $"{user.UserName}|{user.Role}");
      NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
      return true;
    }

    public async Task<bool> LoginAsync(string userName, string password)
    {
      var user = await _users.ValidateCredentialsAsync(userName, password);
      if (user == null) return false;

      await _session.SetAsync(SessionKey, $"{user.UserName}|{user.Role}");
      NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
      return true;
    }

    public async Task LogoutAsync()
    {
      Console.WriteLine("Logout clicked");
      await _session.DeleteAsync(SessionKey);
      NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anon)));
    }
  }
}
