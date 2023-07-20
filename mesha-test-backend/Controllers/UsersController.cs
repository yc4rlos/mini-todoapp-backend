using mesha_test_backend.Data.Dtos;
using mesha_test_backend.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace mesha_test_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController: ControllerBase
{
    private UsersService _usersService;

    public UsersController(UsersService usersService)
    {
        _usersService = usersService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReadUserDto>))]
     public ActionResult<IEnumerable<ReadUserDto>> FindAll()
     {
         try
         {
             return Ok(_usersService.FindAll());
         }
         catch (Exception e)
         {
             return Problem(detail: e.ToString() );
         }
     }

     [HttpGet("{id}")]
     [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReadUserDto))]
     [ProducesResponseType(StatusCodes.Status404NotFound)]
     public ActionResult<ReadUserDto> FindOneById(string id)
     {
         try
         {
             var user = _usersService.FindOneById(id);

             if (user == null) return NotFound();
             return Ok(user);
         }
         catch (Exception e)
         {
             return InternalServerError(e);
         }
     }

     [HttpPost] 
     [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReadUserDto))]
     [ProducesResponseType(StatusCodes.Status400BadRequest)]
     public ActionResult<ReadUserDto> Create(CreateUserDto createUserDto)
     {
         try
         {
    
             return CreatedAtAction(nameof(Create), _usersService.Create(createUserDto));
         }
         catch (Exception e)
         {
             return InternalServerError(e);
         }
     }

     [HttpPatch("{id}")]
     [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReadUserDto))]
     [ProducesResponseType(StatusCodes.Status400BadRequest)]
     [ProducesResponseType(StatusCodes.Status404NotFound)]
     public ActionResult<ReadUserDto> Update(string id, [FromBody] JsonPatchDocument<UpdateUserDto> patchUpdateUserDto)
     {
         try
         {
             var user = _usersService.Update(id, patchUpdateUserDto);

             if (user == null) return NotFound();
             return CreatedAtAction(nameof(Update), user);
         }
         catch (Exception e)
         {
             return InternalServerError(e);
         }
     }

     [HttpDelete("{id}")]
     [ProducesResponseType(StatusCodes.Status204NoContent)]
     public IActionResult Delete(string id)
     {
         try
         {
             _usersService.Delete(id);
             return NoContent();
         }
         catch (Exception e)
         {
             return InternalServerError(e);
         }
     }

     private ObjectResult InternalServerError(Exception e)
     {
         return Problem(
             detail: e.ToString(),
             title: "Erro interno."
             );
     }
     
     
}