namespace SurveyBasket.Mapping;

public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email);

        config.NewConfig<ApplicationUser, UserProfileResponse>()
            .Map(dest => dest.Username, src => src.Email);

        //config.NewConfig<QuestionRequest, Question>()
        //    .Ignore(dest => dest.Answers);

        config.NewConfig<QuestionRequest, Question>()
            .Map(dest => dest.Answers, src => src.Answers.Select(answer => new Answer { Content = answer }))
            .Ignore(dest => dest.PollId);


    }
}
