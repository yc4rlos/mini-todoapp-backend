namespace mesha_test_backend.Data.Dtos;

public class QueryParamsDto
{
    public string? Find { get; set; }
    public int Take { get; set; } = 99999;
    public int Page { get; set; } = 1;
}