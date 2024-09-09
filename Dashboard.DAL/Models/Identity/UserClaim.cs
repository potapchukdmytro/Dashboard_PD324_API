using Microsoft.AspNetCore.Identity;

namespace Dashboard.DAL.Models.Identity
{
    public class UserClaim : IdentityUserClaim<Guid>
    {
        public virtual User? User { get; set; }
    }
}
