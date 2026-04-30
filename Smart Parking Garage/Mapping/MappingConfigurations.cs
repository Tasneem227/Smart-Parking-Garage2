using Mapster;
using Smart_Parking_Garage.Contracts.User;

namespace SurveyBasket.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        

        config.NewConfig<(ApplicationUser user, IList<string> roles), UserResponse>()
            .Map(dest => dest, src => src.user)
            .Map(dest => dest.Roles, src => src.roles);

        config.NewConfig<CreateUserRequest, ApplicationUser>()
            .Map(dest => dest.EmailConfirmed, src => true);

        TypeAdapterConfig<UpdateUserRequest, ApplicationUser>
            .NewConfig()
            .IgnoreNullValues(true);

        config.NewConfig<UpdateUserRequest, ApplicationUser>()
            .IgnoreNullValues(true);

    }
}