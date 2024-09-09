using Microsoft.AspNetCore.Identity;

namespace Dashboard.DAL.Models.Identity
{
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual Role? Role { get; set; }
    }
}
