using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace mesha_test_backend.Data.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "O E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "O E-mail precisa ser válido")]
    public string Email { get; set; }
    [Required(ErrorMessage = "A Senha é obrigatória")]
    public string Password { get; set; }
}