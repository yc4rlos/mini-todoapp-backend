using System.Security.Authentication;
using mesha_test_backend.Data.Dtos;
using mesha_test_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mesha_test_backend.Controllers;

[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class AuthController: ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReadLoginDataDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<ReadLoginDataDto> Login([FromBody] LoginDto loginDto)
    {
        var loginDataDto = _authService.Login(loginDto);
        if (loginDataDto == null) return BadRequest("E-mail ou senha inválidos.");
        
        return Ok(loginDataDto);
    }

}