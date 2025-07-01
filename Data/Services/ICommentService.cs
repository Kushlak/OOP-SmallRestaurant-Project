public interface ICommentService
{
  Task AddCommentAsync(Comment comment);
  Task DeleteCommentAsync(Guid commentId);
  Task UpdateCommentAsync(Comment comment);
  Task<List<Comment>> GetByDishIdAsync(Guid dishId);
}