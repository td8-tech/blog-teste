using Blog.Core.Entities;
using Blog.Core.Interfaces;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        // O AppDbContext será injetado aqui pelo sistema de Injeção de Dependência do ASP.NET Core.
        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
        }

        public void Delete(Post post)
        {
            // O EF Core rastreia a entidade e a marca como "Deleted".
            _context.Posts.Remove(post);
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            // Usamos .Include() para carregar os dados do usuário relacionado (eager loading).
            // Isso evita o problema de N+1 queries.
            return await _context.Posts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Update(Post post)
        {
            // Marca a entidade inteira como "Modified".
            _context.Posts.Update(post);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // O SaveChangesAsync efetivamente aplica todas as alterações rastreadas (Add, Update, Delete)
            // no banco de dados em uma única transação.
            return await _context.SaveChangesAsync() > 0;
        }
    }
}