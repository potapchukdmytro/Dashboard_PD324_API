namespace Dashboard.DAL.ViewModels
{
    public class UserVM
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string? Image { get; set; }
    }
}
