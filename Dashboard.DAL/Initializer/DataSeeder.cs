using Dashboard.DAL.Models;
using Dashboard.DAL.Models.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dashboard.DAL.Initializer
{
    public static class DataSeeder
    {
        public async static void SeedData(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await context.Database.MigrateAsync();

                if (!roleManger.Roles.Any())
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

                if (!userManager.Users.Any())
                {
                    var admin = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = "admin@gmail.com",
                        EmailConfirmed = true,
                        FirstName = "admin",
                        LastName = "dashboard",
                        UserName = "admin"
                    };

                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = "user@gmail.com",
                        EmailConfirmed = true,
                        FirstName = "user",
                        LastName = "dashboard",
                        UserName = "user"
                    };

                    await userManager.CreateAsync(user, "qwerty");
                    await userManager.CreateAsync(admin, "qwerty");

                    await userManager.AddToRoleAsync(user, Settings.UserRole);
                    await userManager.AddToRoleAsync(admin, Settings.AdminRole);
                }

                // Categories and products

                if (!context.Categories.Any())
                {
                    var foodCategory = new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Їжа",
                        NormalizedName = "ЇЖА"
                    };

                    await context.Categories.AddAsync(foodCategory);
                    await context.SaveChangesAsync();

                    var apple = new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Яблуко",
                        Price = 20,
                        Description = "Смачне та червоне",
                        ShortDescription = "Смачне та червоне",
                        CategoryId = foodCategory.Id
                    };

                    await context.Products.AddAsync(apple);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
