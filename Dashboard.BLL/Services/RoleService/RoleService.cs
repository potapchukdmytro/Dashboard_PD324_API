using AutoMapper;
using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.Repositories.RoleRepository;
using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private IRoleRepository _roleRepository;
        private IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> CreateAsync(RoleVM model)
        {
            var isUnique = await _roleRepository.IsUniqueNameAsync(model.Name);

            if (!isUnique)
            {
                return ServiceResponse.GetBadRequestResponse($"Роль {model.Name} вже існує");
            }

            var role = _mapper.Map<Role>(model);
            var result = await _roleRepository.CreateAsync(role);

            if(!result.Succeeded)
            {
                return ServiceResponse.GetBadRequestResponse("Не вдалося створити роль", errors: result.Errors.Select(e => e.Description).ToArray());
            }

            return ServiceResponse.GetOkResponse($"Роль {role.Name} успішно створено");
        }

        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            var role = await _roleRepository.GetByIdAsync(Guid.Parse(id));

            if(role == null)
            {
                if (role == null)
                {
                    return ServiceResponse.GetBadRequestResponse($"Роль не знайдена");
                }
            }

            var result = await _roleRepository.DeleteAsync(role);

            if (!result.Succeeded)
            {
                return ServiceResponse.GetBadRequestResponse("Не вдалося видалити роль", errors: result.Errors.Select(e => e.Description).ToArray());
            }

            return ServiceResponse.GetOkResponse($"Роль {role.Name} успішно видалена");
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var roles = await _roleRepository.GetAllAsync();

            var models = _mapper.Map<List<RoleVM>>(roles);

            return ServiceResponse.GetOkResponse("Ролі отримано", models);
        }

        public async Task<ServiceResponse> GetAsync<T>(Func<T, Task<Role?>> func, T value)
        {
            var role = await func(value);

            if (role == null)
            {
                return ServiceResponse.GetBadRequestResponse($"Роль не знайдена");
            }

            var model = _mapper.Map<RoleVM>(role);

            return ServiceResponse.GetOkResponse($"Роль отримана", model);
        }

        public async Task<ServiceResponse> GetByIdAsync(string id)
        {
            return await GetAsync(_roleRepository.GetByIdAsync, Guid.Parse(id));
        }

        public async Task<ServiceResponse> GetByNameAsync(string name)
        {
            return await GetAsync(_roleRepository.GetByNameAsync, name);
        }

        public async Task<ServiceResponse> UpdateAsync(RoleVM model)
        {
            var role = await _roleRepository.GetByNameAsync(model.Name);

            if(role == null)
            {
                return ServiceResponse.GetBadRequestResponse($"Роль {model.Name} не знайдена");
            }

            if (role.Name != model.Name)
            {
                role.Name = model.Name;
                role.NormalizedName = model.Name.ToUpper();
                var result = await _roleRepository.UpdateAsync(role);

                if (!result.Succeeded)
                {
                    return ServiceResponse.GetBadRequestResponse("Не вдалося оновити роль", errors: result.Errors.Select(e => e.Description).ToArray());
                }

                return ServiceResponse.GetOkResponse($"Роль {role.Name} успішно оновлено");
            }

            return ServiceResponse.GetOkResponse("Успіх");
        }
    }
}
