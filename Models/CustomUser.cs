using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PoetAPI.Models
{
    public class CustomUser: IdentityUser
    {
        public ICollection<Poem> SavedPoems { get; set; } = [];

    }
}
