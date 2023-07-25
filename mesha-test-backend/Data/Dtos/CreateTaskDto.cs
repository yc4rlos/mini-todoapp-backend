using System.ComponentModel.DataAnnotations;

namespace mesha_test_backend.Data.Dtos;

public class CreateTaskDto
{
    [Required(ErrorMessage = "O Título é obrigatório")]
    [MaxLength(255)]
    public string Title { get; set; }
 
    public string Description { get; set; }
}