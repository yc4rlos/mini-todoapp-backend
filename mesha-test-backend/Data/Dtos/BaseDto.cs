namespace mesha_test_backend.Data.Dtos;

public class BaseDto
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}