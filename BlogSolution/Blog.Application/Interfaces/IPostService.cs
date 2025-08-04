using Blog.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post?> GetPostByIdAsync(int id);

        // Usamos parâmetros primitivos aqui para manter o serviço agnóstico a DTOs da camada de API.
        Task<Post> CreatePostAsync(string title, string content, string userId);

        Task<bool> UpdatePostAsync(int postId, string title, string content, string userId);

        Task<bool> DeletePostAsync(int postId, string userId);
    }
}