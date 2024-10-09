using Dashboard.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.DAL.ViewModels
{
    public class ProductVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }
        public string Category { get; set; }
    }
}
