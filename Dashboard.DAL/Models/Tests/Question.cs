using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.DAL.Models.Tests
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength]
        public required string Title { get; set; }

        [ForeignKey("Test")]
        public Guid TestId { get; set; }
        public Test? Test { get; set; }

        public List<Answer> Answers { get; set; } = new();
    }
}
