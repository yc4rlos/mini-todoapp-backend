using mesha_test_backend.Data.Dtos;
using mesha_test_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace mesha_test_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController: ControllerBase
{
    private readonly TasksService _tasksService;

    public TasksController(TasksService tasksService)
    {
        _tasksService = tasksService;
    }
    
    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReadTaskDto>))]
    public ActionResult<IEnumerable<ReadTaskDto>> FindAll()
    {
        try
        {
            return Ok( _tasksService.FindAll());
        }
        catch (Exception e)
        {
            return InternalServerError(e);
        }
    }
    
    [Authorize]
    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReadTaskDto>))]
    public ActionResult<IEnumerable<ReadTaskDto>> FindAllByUser(string userId)
    {
        try
        {
            return Ok( _tasksService.FindAllByUser(userId));
        }
        catch (Exception e)
        {
            return InternalServerError(e);
        }
    }
    
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReadTaskDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<ReadTaskDto> FindOneById(string id)
    {
        try
        {
            return Ok( _tasksService.FindOneById(id));
        }
        catch (Exception e)
        {
            return InternalServerError(e);
        }
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReadTaskDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<ReadTaskDto> Create([FromBody] CreateTaskDto createTaskDto)
    {
        try
        {
            return CreatedAtAction(nameof(Create), _tasksService.Create(createTaskDto));
        }
        catch (Exception e)
        {
            return InternalServerError(e);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReadTaskDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<ReadTaskDto> Update(string id, UpdateTaskDto updateTaskDto)
    {
        try
        {
            var task = _tasksService.Update(id, updateTaskDto);

            if (task == null) return NotFound();
            
            return CreatedAtAction(nameof(Update), task);
        }
        catch (Exception e)
        {
            return InternalServerError(e);
        }
    }

    [Authorize]
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReadTaskDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<ReadTaskDto> UpdatePartial(string id, [FromBody] JsonPatchDocument patchUpdateTaskDto)
    {
        try
        {
            var task = _tasksService.UpdatePartial(id, patchUpdateTaskDto);

            if (task == null) return NotFound();
            
            return CreatedAtAction(nameof(UpdatePartial), task);
        }
        catch (Exception e)
        {
            return InternalServerError(e);
        }
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult<ReadTaskDto> Delete(string id)
    {
        try
        {
            _tasksService.Delete(id);
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