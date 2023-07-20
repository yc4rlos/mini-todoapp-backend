namespace mesha_test_backend.Data.Dtos;

public class ReadUserDto: BaseDto
{

    public string Name { get; set; }
    
    public string?  Lastname { get; set; }

    public string Email { get; set; }
    
}