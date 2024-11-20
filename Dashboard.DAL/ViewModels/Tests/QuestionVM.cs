using Dashboard.DAL.Models.Tests;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.DAL.ViewModels.Tests
{
    public class QuestionVM
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<AnswerVM> Answers { get; set; } = new();
    }
}
