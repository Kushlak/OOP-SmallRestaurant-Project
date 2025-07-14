using Microsoft.EntityFrameworkCore;
using SmallRestaurant.Data.Context;
using SmallRestaurant.Data.Models;
using SmallRestaurant.Data.Services;

public class AddressService : IAddressService
{
  private readonly AppDbContext _db;

  public AddressService(AppDbContext db)
    => _db = db;

  public Task<List<Address>> GetForUserAsync(string userId)
    => _db.Addresses
      .Where(a => a.UserId == userId)
      .OrderByDescending(a => a.IsDefault)
      .ToListAsync();

  public Task<Address?> GetByIdAsync(Guid id)
    => _db.Addresses.FindAsync(id).AsTask();

  public async Task CreateAsync(Address address)
  {
    if (address.IsDefault)
    {
      var others = await _db.Addresses
        .Where(a => a.UserId == address.UserId)
        .ToListAsync();
      others.ForEach(a => a.IsDefault = false);
    }

    _db.Addresses.Add(address);
    await _db.SaveChangesAsync();
  }

  public async Task UpdateAsync(Address address)
  {
    if (address.IsDefault)
    {
      var others = await _db.Addresses
        .Where(a => a.UserId == address.UserId && a.Id != address.Id)
        .ToListAsync();
      others.ForEach(a => a.IsDefault = false);
    }

    _db.Addresses.Update(address);
    await _db.SaveChangesAsync();
  }

  public async Task DeleteAsync(Guid id)
  {
    var a = await _db.Addresses.FindAsync(id);
    if (a != null)
    {
      _db.Addresses.Remove(a);
      await _db.SaveChangesAsync();
    }
  }

  public async Task SetDefaultAsync(string userId, Guid id)
  {
    var list = await _db.Addresses
      .Where(a => a.UserId == userId)
      .ToListAsync();

    foreach (var a in list)
      a.IsDefault = (a.Id == id);

    await _db.SaveChangesAsync();
  }
}