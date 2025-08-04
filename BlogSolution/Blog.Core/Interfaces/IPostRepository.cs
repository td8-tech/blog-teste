using Blog.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Core.Interfaces
{
    public interface IPostRepository
    {
        Task<Post?> GetByIdAsync(int id);
        Task<IEnumerable<Post>> GetAllAsync();
        Task AddAsync(Post post);
        void Update(Post post);
        void Delete(Post post);
        Task<bool> SaveChangesAsync();
    }
}