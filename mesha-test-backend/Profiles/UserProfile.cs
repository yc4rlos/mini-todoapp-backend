using AutoMapper;
using mesha_test_backend.Data.Dtos;
using mesha_test_backend.Models;

namespace mesha_test_backend.Profiles;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, User>();
        CreateMap<User, ReadUserDto>()
            .ForMember(userDto => userDto.Tasks, 
            opt => opt.MapFrom(user => user.Tasks));
        CreateMap<User, UpdateUserDto>();
    }
}