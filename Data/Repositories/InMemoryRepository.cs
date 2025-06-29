using System.Collections.Concurrent;

namespace SmallRestaurant.Data.Repositories
{
  public class InMemoryRepository<T> : IRepository<T> where T : class
  {
    private readonly ConcurrentDictionary<Guid, T> _store = new();

    public Task<List<T>> GetAllAsync() =>
      Task.FromResult(_store.Values.ToList());

    public Task<T?> GetByIdAsync(Guid id) =>
      Task.FromResult(_store.TryGetValue(id, out var item) ? item : null);

    public Task AddAsync(T item)
    {
      var prop = item.GetType().GetProperty("Id");
      if (prop == null) throw new InvalidOperationException("No Id prop");
      var id = (Guid)prop.GetValue(item)!;
      _store[id] = item;
      return Task.CompletedTask;
    }

    public Task UpdateAsync(T item)
    {
      var prop = item.GetType().GetProperty("Id");
      var id = (Guid)prop.GetValue(item)!;
      _store[id] = item;
      return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
      _store.TryRemove(id, out _);
      return Task.CompletedTask;
    }
  }
}