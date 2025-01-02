using System.ComponentModel.DataAnnotations;

namespace PoetAPI.Models
{
    public class Poem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "";

        [Required]
        public string Author { get; set; } = "";

        [Required]
        public string Lines { get; set; } = "";

        public ICollection<CustomUser> SavedByUsers { get; set; } = new List<CustomUser>();
    }
}
