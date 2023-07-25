using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using mesha_test_backend.Data;
using mesha_test_backend.Data.Dtos;
using mesha_test_backend.Models;
using Microsoft.AspNetCore.JsonPatch;
using Task = mesha_test_backend.Models.Task;

namespace mesha_test_backend.Services;

public class TasksService
{
    private readonly TasksDatabaseContext _dbContext;
    private readonly IMapper _mapper;
    private readonly AuthService _authService;

    public TasksService(TasksDatabaseContext dbContext, IMapper mapper, AuthService authService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _authService = authService;
    }

    public IEnumerable<ReadTaskDto> FindAll()
    {
        var tasks = _dbContext.Tasks.ToList();

        return _mapper.Map<IEnumerable<ReadTaskDto>>(tasks);
    }

    public IEnumerable<ReadTaskDto> FindAllByUser(string userId, string? find)
    {
        
        var tasks = new List<Task>();
        if(find == null)
        {
            tasks = _dbContext.Tasks.Where(t => t.UserId.ToString() == userId).ToList();
        }
        else
        {
            var findValue = find.ToLower();
            tasks = _dbContext.Tasks.Where(t =>
                t.UserId.ToString() == userId &&
                (t.Title.ToLower().Contains(findValue) || t.Description.ToLower().Contains(findValue))).ToList();
        }

        return _mapper.Map<IEnumerable<ReadTaskDto>>(tasks);
    }

    public ReadTaskDto?  FindOneById(string id)
    {
        var task = _dbContext.Tasks.FirstOrDefault(t => t.Id.ToString() == id);

        if (task == null) return null;

        return _mapper.Map<ReadTaskDto>(task);
    }
    
    public ReadTaskDto Create(CreateTaskDto createTaskDto, string authorization)
    {
        var userId = _authService.GetUserIdFromAuthorization(authorization);
        var task = _mapper.Map<Models.Task>(createTaskDto);
        task.UserId = Guid.Parse(userId);
        
        _dbContext.Tasks.Add(task);
        _dbContext.SaveChanges();

        return _mapper.Map<ReadTaskDto>(task);
    }

    public ReadTaskDto? Update(string id, UpdateTaskDto updateTaskDto)
    {

        var task = _dbContext.Tasks.FirstOrDefault(t => t.Id.ToString() == id);
        if (task == null) return null;

        var taskData = _mapper.Map(updateTaskDto, task);

        taskData.UpdatedAt = DateTime.Now;

        _dbContext.Tasks.Update(taskData);
        _dbContext.SaveChanges();

        return _mapper.Map<ReadTaskDto>(taskData);

    }

    public ReadTaskDto? UpdatePartial(string id, JsonPatchDocument patchUpdateTaskDto)
    {
        var task = _dbContext.Tasks.FirstOrDefault(u => u.Id.ToString() == id);
        if (task == null) return null;

        var taskData = _mapper.Map<UpdateTaskDto>(task);
        
        patchUpdateTaskDto.ApplyTo(taskData);

        _mapper.Map(taskData, task);
        
        task.UpdatedAt = DateTime.Now;
        
        _dbContext.Tasks.Update(task);
        
        _dbContext.SaveChanges();

        return _mapper.Map<ReadTaskDto>(task);
    }

    public void Delete(string id)
    {
        var task = _dbContext.Tasks.FirstOrDefault(t => t.Id.ToString() == id);

        if (task == null) return;

        _dbContext.Tasks.Remove(task);
        _dbContext.SaveChanges();

    }
}