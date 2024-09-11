using Dashboard.DAL.Models.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Dashboard.DAL.Initializer
{
    public static class DataSeeder
    {
        public async static void SeedData(this IApplicationBuilder builder)
        {
            using(var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

                if(!roleManger.Roles.Any())
                {
                    var adminRole = new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = Settings.AdminRole,
                        NormalizedName = Settings.AdminRole.ToUpper()
                    };

                    var userRole = new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = Settings.UserRole,
                        NormalizedName = Settings.UserRole.ToUpper()
                    };

                    await roleManger.CreateAsync(userRole);
                    await roleManger.CreateAsync(adminRole);
                }
            }
        }
    }
}
