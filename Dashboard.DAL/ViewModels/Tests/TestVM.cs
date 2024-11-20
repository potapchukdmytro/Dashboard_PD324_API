using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.Models.Tests;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.DAL.ViewModels.Tests
{
    public class TestVM
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public required string Description { get; set; }
        public List<QuestionVM> Questions { get; set; } = new();
    }
}
