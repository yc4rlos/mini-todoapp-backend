using AutoMapper;
using mesha_test_backend.Data;
using mesha_test_backend.Data.Dtos;
using mesha_test_backend.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace mesha_test_backend.Services;

public class UsersService
{
    private readonly int _salt = 12;
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

        var alreadyRegistered = _dbcontext.Users.FirstOrDefault(u => u.Email.ToLower() == createUserDto.Email) != null;

        if (alreadyRegistered) throw new BadHttpRequestException( "Endereço de e-mail já cadastrado");

        user.Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password, _salt);

        _dbcontext.Users.Add(user);
        _dbcontext.SaveChanges();
        return _mapper.Map<ReadUserDto>(user);
    }

    public ReadUserDto Update(string id, UpdateUserDto updateUserDto)
    {
        var user = _dbcontext.Users.FirstOrDefault(u => u.Id.ToString() == id);
        if (user == null) return null;

        var alreadyRegistered = false;
        if(user.Email != updateUserDto.Email) 
            alreadyRegistered = _dbcontext.Users.FirstOrDefault(u => u.Email.ToLower() == updateUserDto.Email) != null;

        if (alreadyRegistered)
            throw new BadHttpRequestException("Endereço de e-mail já cadastrado");
        

        var userData = _mapper.Map(updateUserDto, user);

        userData.UpdatedAt = DateTime.Now;

        _dbcontext.Users.Update(userData);
        _dbcontext.SaveChanges();

        return _mapper.Map<ReadUserDto>(userData);

    }

    public ReadUserDto? UpdatePartial(string id, JsonPatchDocument<UpdateUserDto> patchUpdateUserDto)
    {
        var user = _dbcontext.Users.FirstOrDefault(u => u.Id.ToString() == id);
        if (user == null) return null;

        var userData = _mapper.Map<UpdateUserDto>(user);
        
        patchUpdateUserDto.ApplyTo(userData);

        var alreadyRegistered = false;
        if (user.Email != userData.Email)
            alreadyRegistered = _dbcontext.Users.FirstOrDefault(u => u.Email == userData.Email) != null;

        if (alreadyRegistered) throw new BadHttpRequestException("Endereço de e-mail já cadastrado");

        if(userData.Password != user.Password)
            userData.Password = BCrypt.Net.BCrypt.HashPassword(userData.Password, _salt);

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

    public ReadUserDto? CheckPassword(string email, string password)
    {
        var user = _dbcontext.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

        if (user == null) return null;

        var verified = BCrypt.Net.BCrypt.Verify(password, user.Password);

        if (!verified) return null;

        return _mapper.Map<ReadUserDto>(user);
    }
}