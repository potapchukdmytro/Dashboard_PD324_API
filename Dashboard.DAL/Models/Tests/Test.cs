using Dashboard.DAL.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.DAL.Models.Tests
{
    public class Test
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }
        [Required]
        [MaxLength]
        public required string Description { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public List<Question> Questions { get; set; } = new();
    }
}
