using Microsoft.AspNetCore.Identity;

namespace Dashboard.DAL.Models.Identity
{
    public class UserLogin : IdentityUserLogin<Guid>
    {
        public virtual User? User { get; set; }
    }
}
