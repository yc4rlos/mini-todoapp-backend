using AutoMapper;
using mesha_test_backend.Data;
using mesha_test_backend.Data.Dtos;
using mesha_test_backend.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace mesha_test_backend.Services;

public class UsersService
{
    
    private readonly TasksDatabaseContext _dbcontext;
    private readonly IMapper _mapper;

    public UsersService(TasksDatabaseContext dbcontext, IMapper mapper)
    {
        _dbcontext = dbcontext;
        _mapper = mapper;
    }
    
    public IEnumerable<ReadUserDto> FindAll()
    {
        var users = _dbcontext.Users.ToList();

        return _mapper.Map<List<ReadUserDto>>(users);
    }

    public ReadUserDto? FindOneById(string id)
    {
        var user = _dbcontext.Users.FirstOrDefault(u => u.Id.ToString() == id);
        return _mapper.Map<ReadUserDto>(user);
    }

    public ReadUserDto Create(CreateUserDto createUserDto)
    {
        var user = _mapper.Map<User>(createUserDto);
        
        user.Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

        _dbcontext.Users.Add(user);
        _dbcontext.SaveChanges();
        return _mapper.Map<ReadUserDto>(user);
    }

    public ReadUserDto? Update(string id, JsonPatchDocument<UpdateUserDto> patchUpdateUserDto)
    {
        var user = _dbcontext.Users.FirstOrDefault(u => u.Id.ToString() == id);
        if (user == null) return null;

        var userData = _mapper.Map<UpdateUserDto>(user);
        
        patchUpdateUserDto.ApplyTo(userData);

        
        Console.WriteLine("User Data:" + userData.Password);
        Console.WriteLine("User:" + user.Password);

        if(userData.Password != user.Password)
            userData.Password = BCrypt.Net.BCrypt.HashPassword(userData.Password);

        _mapper.Map(userData, user);
        
        user.UpdatedAt = DateTime.Now;
        
        _dbcontext.Users.Update(user);
        
        _dbcontext.SaveChanges();

        return _mapper.Map<ReadUserDto>(user);
    }
    
    public void Delete(string id)
    {
        var user = _dbcontext.Users.FirstOrDefault(u => u.Id.ToString() == id);
        if (user == null) return;

        _dbcontext.Users.Remove(user);
        _dbcontext.SaveChanges();
    }

    public bool CheckPassword(string email, string password)
    {
        var user = _dbcontext.Users.FirstOrDefault(u => u.Email == email);

        if (user == null) return false;

        return BCrypt.Net.BCrypt.Verify(user.Password, password);
    }
}