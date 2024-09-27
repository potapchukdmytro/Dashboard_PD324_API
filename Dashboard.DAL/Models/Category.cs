using System.ComponentModel.DataAnnotations;

namespace Dashboard.DAL.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public required string NormalizedName { get; set; }

        public List<Product> Products { get; set; } = new();
    }
}
