using AutoMapper;
using mesha_test_backend.Data;
using mesha_test_backend.Data.Dtos;
using mesha_test_backend.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace mesha_test_backend.Services;

public class UsersService
{
    private readonly int _salt = 12;
    private readonly TasksDatabaseContext _dbContext;
    private readonly IMapper _mapper;

    public UsersService(TasksDatabaseContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public DataListDto<ReadUserDto> FindAll(QueryParamsDto queryParamsDto)
    {
        var usersData = _dbContext.Users.Include(u => u.Tasks).Take(queryParamsDto.Take).Skip(queryParamsDto.Take * (queryParamsDto.Page - 1));
        
        if(queryParamsDto.Find != null)
        {
            var findValue = queryParamsDto.Find.ToLower();
            usersData = usersData.Where(t =>
                t.Name.ToLower().Contains(findValue) || 
                t.Lastname.ToLower().Contains(findValue) ||
                t.Email.ToLower().Contains(findValue));
        }

        var quantity = usersData.Count();

        var hasNextPage = quantity > queryParamsDto.Take * queryParamsDto.Page ;

        var resp = new DataListDto<ReadUserDto>
        {
            CurrentPage = queryParamsDto.Page,
            Quantity = quantity,
            HasNextPage = hasNextPage,
            Data =  _mapper.Map<IEnumerable<ReadUserDto>>(usersData.ToList())
        };

        return resp;
    }

    public ReadUserDto? FindOneById(string id)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id.ToString() == id);
        return _mapper.Map<ReadUserDto>(user);
    }

    public ReadUserDto Create(CreateUserDto createUserDto)
    {
        var user = _mapper.Map<User>(createUserDto);

        var alreadyRegistered = _dbContext.Users.FirstOrDefault(u => u.Email.ToLower() == createUserDto.Email) != null;

        if (alreadyRegistered) throw new BadHttpRequestException( "Endereço de e-mail já cadastrado");

        user.Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password, _salt);

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return _mapper.Map<ReadUserDto>(user);
    }

    public ReadUserDto Update(string id, UpdateUserDto updateUserDto)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id.ToString() == id);
        if (user == null) return null;

        var alreadyRegistered = false;
        if(user.Email != updateUserDto.Email) 
            alreadyRegistered = _dbContext.Users.FirstOrDefault(u => u.Email.ToLower() == updateUserDto.Email) != null;

        if (alreadyRegistered)
            throw new BadHttpRequestException("Endereço de e-mail já cadastrado");
        

        var userData = _mapper.Map(updateUserDto, user);

        userData.UpdatedAt = DateTime.Now;

        _dbContext.Users.Update(userData);
        _dbContext.SaveChanges();

        return _mapper.Map<ReadUserDto>(userData);

    }

    public ReadUserDto? UpdatePartial(string id, JsonPatchDocument<UpdateUserDto> patchUpdateUserDto)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id.ToString() == id);
        if (user == null) return null;

        var userData = _mapper.Map<UpdateUserDto>(user);
        
        patchUpdateUserDto.ApplyTo(userData);

        var alreadyRegistered = false;
        if (user.Email != userData.Email)
            alreadyRegistered = _dbContext.Users.FirstOrDefault(u => u.Email == userData.Email) != null;

        if (alreadyRegistered) throw new BadHttpRequestException("Endereço de e-mail já cadastrado");

        if(userData.Password != user.Password)
            userData.Password = BCrypt.Net.BCrypt.HashPassword(userData.Password, _salt);

        _mapper.Map(userData, user);
        
        user.UpdatedAt = DateTime.Now;
        
        _dbContext.Users.Update(user);
        
        _dbContext.SaveChanges();

        return _mapper.Map<ReadUserDto>(user);
    }
    
    public void Delete(string id)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id.ToString() == id);
        if (user == null) return;

        _dbContext.Users.Remove(user);
        _dbContext.SaveChanges();
    }

    public ReadUserDto? CheckPassword(string email, string password)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

        if (user == null) return null;

        var verified = BCrypt.Net.BCrypt.Verify(password, user.Password);

        if (!verified) return null;

        return _mapper.Map<ReadUserDto>(user);
    }
}