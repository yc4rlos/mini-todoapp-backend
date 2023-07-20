namespace mesha_test_backend.Data.Dtos;

public class UpdateTaskDto: CreateTaskDto
{
    public bool Complete { get; set; }
}