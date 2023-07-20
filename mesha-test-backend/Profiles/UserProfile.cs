using AutoMapper;
using mesha_test_backend.Data.Dtos;
using mesha_test_backend.Model;

namespace mesha_test_backend.Profiles;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, User>();
        CreateMap<User, ReadUserDto>();
        CreateMap<UpdateUserDto, User>();
        CreateMap<User, UpdateUserDto>();
    }
}