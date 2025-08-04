using Blog.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Data
{
    // Herda de IdentityDbContext para que o EF Core gerencie as tabelas do Identity (Users, Roles, etc.)
    // e também as nossas entidades customizadas.
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Mapeia a entidade Post para uma tabela "Posts" no banco de dados.
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Aqui podemos adicionar configurações detalhadas do modelo (Fluent API) se necessário.
            // Ex: Configurar chaves, índices, relacionamentos, etc.
            builder.Entity<Post>()
                .HasOne(p => p.User)        // Um Post tem um User
                .WithMany(u => u.Posts)     // Um User tem muitos Posts
                .HasForeignKey(p => p.UserId); // A chave estrangeira é UserId
        }
    }
}