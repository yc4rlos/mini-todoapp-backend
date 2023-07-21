using System.ComponentModel.DataAnnotations;
using mesha_test_backend.Models;

namespace mesha_test_backend.Data.Dtos;

public class CreateTaskDto
{
    [Required(ErrorMessage = "O Título é obrigatório")]
    [MaxLength(255)]
    public string Title { get; set; }
 
    public string Description { get; set; }

    [Required(ErrorMessage = "O ID do usuário criador é obrigatório")]
    public string UserId { get; set; }
}