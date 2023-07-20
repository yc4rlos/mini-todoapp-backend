using AutoMapper;
using mesha_test_backend.Data.Dtos;
using Task = mesha_test_backend.Models.Task;

namespace mesha_test_backend.Profiles;

public class TaskProfile: Profile
{
    public TaskProfile()
    {
        CreateMap<CreateTaskDto, Models.Task>();
        CreateMap<Models.Task, ReadTaskDto>();
        CreateMap<UpdateTaskDto, Models.Task>();
        CreateMap<Models.Task, UpdateTaskDto>();
    }
}