using Microsoft.AspNetCore.Identity;

namespace Dashboard.DAL.Models.Identity
{
    public class UserToken : IdentityUserToken<Guid>
    {
        public virtual User? User { get; set; }
    }
}
