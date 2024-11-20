using AutoMapper;
using Dashboard.DAL.Models.Tests;
using Dashboard.DAL.ViewModels.Tests;

namespace Dashboard.BLL.MappingProfiles
{
    public class TestsMappingProfile : Profile
    {
        public TestsMappingProfile()
        {
            // TestVM -> Test
            CreateMap<TestVM, Test>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Id) ? Guid.NewGuid() : Guid.Parse(src.Id)))
                .ForMember(dest => dest.Questions, opt => opt.Ignore());

            // QuestionVM -> Question
            CreateMap<QuestionVM, Question>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Id) ? Guid.NewGuid() : Guid.Parse(src.Id)))
                .ForMember(dest => dest.Answers, opt => opt.Ignore());

            // AnswerVM -> Answer
            CreateMap<AnswerVM, Answer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Id) ? Guid.NewGuid() : Guid.Parse(src.Id)));

            // Test -> TestVM
            CreateMap<Test, TestVM>();

            // Question -> QuestionVM
            CreateMap<Question, QuestionVM>();

            // Answer -> AnswerVM
            CreateMap<Answer, AnswerVM>();
        }
    }
}
