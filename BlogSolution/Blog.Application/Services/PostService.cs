using Blog.Application.Interfaces;
using Blog.Core.Entities;
using Blog.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blog.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IWebSocketManager _webSocketManager;

        public PostService(IPostRepository postRepository, IWebSocketManager webSocketManager)
        {
            _postRepository = postRepository;
            _webSocketManager = webSocketManager;
        }

        public async Task<Post> CreatePostAsync(string title, string content, string userId)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Título e conteúdo não podem ser vazios.");
            }

            // Precisamos buscar o usuário para ter acesso ao seu nome para a notificação
            // Esta é uma melhoria em relação ao rascunho anterior
            var post = new Post
            {
                Title = title,
                Content = content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _postRepository.AddAsync(post);
            await _postRepository.SaveChangesAsync();

            // A notificação em tempo real é enviada após a confirmação de que o post foi salvo.
            var notification = new
            {
                message = "Novo post publicado!",
                postTitle = post.Title
                // Se quiséssemos o nome do autor, teríamos que buscar o usuário no banco
            };
            await _webSocketManager.BroadcastMessageAsync(JsonSerializer.Serialize(notification));

            return post;
        }

        public async Task<bool> DeletePostAsync(int postId, string userId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                return false;
            }

            // Regra de negócio: apenas o dono pode deletar.
            if (post.UserId != userId)
            {
                throw new UnauthorizedAccessException("Usuário não tem permissão para deletar este post.");
            }

            _postRepository.Delete(post);
            return await _postRepository.SaveChangesAsync();
        }

        public Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return _postRepository.GetAllAsync();
        }

        public Task<Post?> GetPostByIdAsync(int id)
        {
            return _postRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdatePostAsync(int postId, string title, string content, string userId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                return false;
            }

            // Regra de negócio: apenas o dono pode editar.
            if (post.UserId != userId)
            {
                throw new UnauthorizedAccessException("Usuário não tem permissão para editar este post.");
            }

            post.Title = title;
            post.Content = content;

            _postRepository.Update(post);
            return await _postRepository.SaveChangesAsync();
        }
    }
}