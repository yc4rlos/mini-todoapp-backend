namespace mesha_test_backend.Data.Dtos;

public class ReadTaskDto: BaseDto
{
    public string Title { get; set; }
 
    public string Description { get; set; }

    public bool Complete { get; set; }
    
}