using Dashboard.DAL.Models.Tests;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.DAL.ViewModels.Tests
{
    public class AnswerVM
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; } = false;
    }
}
