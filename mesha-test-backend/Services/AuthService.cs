using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using mesha_test_backend.Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace mesha_test_backend.Services;

public class AuthService
{
    private readonly UsersService _usersService;
    private readonly IConfiguration _configuration;
    private readonly RefreshTokensService _refreshTokensService;

    public AuthService(UsersService usersService,IConfiguration configuration, RefreshTokensService refreshTokensService )
    {
        _usersService = usersService;
        _configuration = configuration;
        _refreshTokensService = refreshTokensService;
    }
    
    public ReadLoginDataDto? Login(LoginDto loginDto)
    {
        var user = _usersService.CheckPassword(loginDto.Email, loginDto.Password);

        if (user == null) return null;
        
        var token = GenerateToken(user);
        
        var refreshToken = _refreshTokensService.Create(user.Id.ToString());
        
        var loginData = new ReadLoginDataDto
        {
            User = user,
            Token = token,
            RefreshToken = refreshToken.Id.ToString()
        };

        return loginData;
    }

    public ReadLoginDataDto? RefreshToken(GetNewTokenDto getNewTokenDto)
    {
        var refreshTokenDto = _refreshTokensService.FindOneById(getNewTokenDto.Token);

        if (refreshTokenDto == null) throw new BadHttpRequestException("Token inválido");
        var user = _usersService.FindOneById(refreshTokenDto.UserId.ToString());
        
        _refreshTokensService.Delete(refreshTokenDto.Id.ToString());
        
        if (user == null) throw new BadHttpRequestException("O usuário foi deletado");
        var token = GenerateToken(user);
        
        var refreshToken = _refreshTokensService.Create(user.Id.ToString());
        
        var loginData = new ReadLoginDataDto
        {
            User = user,
            Token = token,
            RefreshToken = refreshToken.Id.ToString()
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

    public string GetUserIdFromAuthorization(string authorization)
    {
        
        var token = authorization.Split("Bearer ")[1];
        
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        var id = jsonToken.Claims.First(c => c.Type == "id").Value.Trim();

        return id;
    }
}