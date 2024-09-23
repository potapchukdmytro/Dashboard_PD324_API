using AutoMapper;
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

        public async Task<ServiceResponse> GetAllAsync()
        {
            var roles = await _roleRepository.GetAllAsync();

            var models = _mapper.Map<List<RoleVM>>(roles);

            return ServiceResponse.GetOkResponse("Ролі отримано", models);
        }
    }
}
