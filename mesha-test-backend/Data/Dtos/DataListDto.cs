namespace mesha_test_backend.Data.Dtos;

public class DataListDto<T>
{
    public int Quantity { get; set; }
    public bool HasNextPage { get; set; }
    
    public int CurrentPage { get; set; }
    public IEnumerable<T> Data { get; set; }
}