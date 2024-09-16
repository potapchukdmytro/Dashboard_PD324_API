namespace Dashboard.DAL.ViewModels
{
    public class SignUpVM
    {
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string ConfirmedPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
