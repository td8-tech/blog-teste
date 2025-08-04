using System.ComponentModel.DataAnnotations;

namespace Blog.Api.DTOs
{
    public class PostCreateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}