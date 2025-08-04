using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Blog.Core.Entities
{
    public class User : IdentityUser
    {

        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}