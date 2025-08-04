using System;

namespace Blog.Core.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // Chave estrangeira para o usuário (dono do post)
        public string UserId { get; set; }
        public User User { get; set; }
    }
}