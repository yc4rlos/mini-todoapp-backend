using Microsoft.AspNetCore.Mvc;

namespace mesha_test_backend.Data.Dtos;

public class HeaderDto
{
    [FromHeader]
    public string Authorization { get; set; } = "";
}