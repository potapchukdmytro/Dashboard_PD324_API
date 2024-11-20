using AutoMapper;
using Dashboard.DAL.Models.Tests;
using Dashboard.DAL.Repositories.TestRepository;
using Dashboard.DAL.ViewModels.Tests;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.BLL.Services.TestService
{
    public class TestService : ITestService
    {
        private readonly IMapper _mapper;
        private readonly ITestRepository _testRepository;

        public TestService(IMapper mapper, ITestRepository testRepository)
        {
            _mapper = mapper;
            _testRepository = testRepository;
        }

        public async Task<ServiceResponse> CreateAsync(TestVM model, string userId)
        {
            var test = _mapper.Map<Test>(model);
            test.UserId = Guid.Parse(userId);

            foreach (var q in model.Questions)
            {
                var question = _mapper.Map<Question>(q);
                question.TestId = test.Id;

                foreach (var a in q.Answers)
                {
                    var answer = _mapper.Map<Answer>(a);
                    answer.QuestionId = question.Id;
                    question.Answers.Add(answer);
                }
                test.Questions.Add(question);
            }

            var result = await _testRepository.CreateAsync(test);

            if(result)
            {
                return ServiceResponse.GetOkResponse("Тест успішно додано");
            }

            return ServiceResponse.GetBadRequestResponse("Не вдалося створити тест", "Не вдалося створити тест");
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var tests = await _testRepository
                .GetAll()
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .ToListAsync();

            var models = _mapper.Map<List<TestVM>>(tests);

            return ServiceResponse.GetOkResponse("Тести отримано", models);
        }

        public Task<ServiceResponse> GetByUserAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
