using System.ComponentModel.DataAnnotations;

namespace mesha_test_backend.Data.Dtos;

public class CreateUserDto
{
    [Required(ErrorMessage = "O Nome de usuário é obrigatório.")]
    public string Name { get; set; }
    
    public string?  Lastname { get; set; }
    
    [Required(ErrorMessage = "O Endereço de e-mail é obrigatório.")]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required(ErrorMessage= "A Senha é obrigatória.")]
    [MinLength(8, ErrorMessage = "A Senha deve ter pelo menos 8 caracteres.")]
    public string Password { get; set; }
    
}