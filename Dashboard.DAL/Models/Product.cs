using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.DAL.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        [MaxLength]
        public string? ShortDescription { get; set; }
        [MaxLength]
        public string? Description { get; set; }
        [Range(0.0, 1000000000.0)]
        public double Price { get; set; }
        public string? Image { get; set; }

        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
