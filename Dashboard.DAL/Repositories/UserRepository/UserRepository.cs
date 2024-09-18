using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Dashboard.DAL.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddToRoleAsync(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"User {id} not found" });
            }

            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        public async Task<bool> CheckPasswordAsync(UserVM model, string password)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());

            if(user == null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<bool> CheckUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName) != null;
        }

        public async Task<IdentityResult> CreateAsync(CreateUserVM model)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = model.EmailConfirmed,
                PhoneNumberConfirmed = model.PhoneNumberConfirmed
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);

            if(!createResult.Succeeded)
            {
                return createResult;
            }

            return await _userManager.AddToRoleAsync(user, model.Role);
        }

        public async Task<IdentityResult> DeleteAsync(UserVM model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());

            if(user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"User {model.Id} not found" });
            }

            return await _userManager.DeleteAsync(user);
        }

        public async Task<string?> GenerateEmailConfirmationTokenAsync(UserVM model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());

            if (user == null)
            {
                return null;
            }

            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<List<UserVM>> GetAllAsync()
        {
            var users = _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role);

            var models = users.Select(u => new UserVM
            {
                Id = u.Id,
                Email = u.Email,
                UserName = u.UserName,
                LastName = u.LastName,
                FirstName = u.FirstName

            });

            return await models.ToListAsync();
        }

        public async Task<UserVM?> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            var model = new UserVM
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                UserName = user.UserName
            };

            return model;

        }

        public async Task<UserVM?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return null;
            }

            var model = new UserVM
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                UserName = user.UserName
            };

            return model;
        }

        public async Task<UserVM?> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return null;
            }

            var model = new UserVM
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                UserName = user.UserName
            };

            return model;
        }

        public async Task<UserVM?> SignUpAsync(SignUpVM model)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                NormalizedUserName = model.UserName.ToUpper()
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if(!result.Succeeded)
            {
                return null;
            }

            return new UserVM
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<IdentityResult> UpdateAsync(UserVM model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"User {model.Id} not found" });
            }

            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;

            return await _userManager.UpdateAsync(user);
        }
    }
}
