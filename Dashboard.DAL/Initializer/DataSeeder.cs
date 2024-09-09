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
            }
        }
    }
}
