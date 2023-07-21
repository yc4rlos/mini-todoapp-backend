using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using mesha_test_backend.Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace mesha_test_backend.Services;

public class AuthService
{
    private readonly UsersService _usersService;
    private readonly IConfiguration _configuration;

    public AuthService(UsersService usersService,IConfiguration configuration )
    {
        _usersService = usersService;
        _configuration = configuration;
    }
    
    
    public ReadLoginDataDto? Login(LoginDto loginDto)
    {
        var user = _usersService.CheckPassword(loginDto.Email, loginDto.Password);

        if (user == null) return null;
        
        var token = GenerateToken(user);
        var loginData = new ReadLoginDataDto
        {
            User = user,
            Token = token
        };

        return loginData;
    }

    public ReadLoginDataDto? UdpateToken(string oldToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(oldToken) as JwtSecurityToken;

        var id = jsonToken.Claims.First(c => c.Type == "id").Value;

        var user = _usersService.FindOneById(id);

        if (user == null) return null;

        var token = GenerateToken(user);

        var loginData = new ReadLoginDataDto
        {
            User = user,
            Token = token
        };

        return loginData;
    }

    private string GenerateToken(ReadUserDto readUserDto)
    {

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new []
            {
                new Claim("id", readUserDto.Id.ToString()),
                new Claim("name", readUserDto.Name),
                new Claim("lastname", readUserDto.Lastname),
                new Claim("email", readUserDto.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}