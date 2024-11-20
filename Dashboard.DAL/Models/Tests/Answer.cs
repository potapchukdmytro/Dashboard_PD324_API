using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.DAL.Models.Tests
{
    public class Answer
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength]
        public required string Text { get; set; }
        public bool IsCorrect { get; set; } = false;

        [ForeignKey("Question")]
        public Guid QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
