namespace SmallRestaurant.Data.Repositories
{
  public interface IRepository<T>
  {
    Task<List<T>>    GetAllAsync();
    Task<T?>         GetByIdAsync(Guid id);
    Task             AddAsync(T item);
    Task             UpdateAsync(T item);
    Task             DeleteAsync(Guid id);
  }
}