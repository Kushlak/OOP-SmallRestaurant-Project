using Microsoft.EntityFrameworkCore;
using SmallRestaurant.Data.Context;

public class CommentService : ICommentService
{
  private readonly AppDbContext _db;

  public CommentService(AppDbContext db) => _db = db;

  public async Task AddCommentAsync(Comment comment)
  {
    comment.Id = Guid.NewGuid();
    comment.CreatedAt = DateTime.UtcNow;
    _db.Comments.Add(comment);
    await _db.SaveChangesAsync();
  }

  public async Task DeleteCommentAsync(Guid commentId)
  {
    var comment = await _db.Comments.FindAsync(commentId);
    if (comment is not null)
    {
      _db.Comments.Remove(comment);
      await _db.SaveChangesAsync();
    }
  }

  public async Task UpdateCommentAsync(Comment comment)
  {
    var existing = await _db.Comments.FirstOrDefaultAsync(c => c.Id == comment.Id);
    if (existing is not null)
    {
      existing.Text = comment.Text;
      existing.Rating = comment.Rating;
      await _db.SaveChangesAsync();
    }
  }

  public Task<List<Comment>> GetByDishIdAsync(Guid dishId)
  {
    return _db.Comments
      .Where(c => c.DishId == dishId)
      .Include(c => c.User)
      .OrderByDescending(c => c.CreatedAt)
      .ToListAsync();
  }
}