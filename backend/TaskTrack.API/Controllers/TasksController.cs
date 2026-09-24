using Microsoft.AspNetCore.Mvc;
using TaskTrack.Service.Dtos;
using TaskTrack.Service.Interfaces;

namespace TaskTrack.API.Controllers;

[ApiController, Route("api/tasks")]
public class TasksController(ITaskService service) : ControllerBase
{
    [HttpGet] public async Task<ActionResult> GetAll([FromQuery] string? title, [FromQuery] short? status, [FromQuery] short? priority, [FromQuery] int? projectId, [FromQuery] int? tagId) => Ok(await service.GetAllAsync(title, status, priority, projectId, tagId));
    [HttpGet("search")] public async Task<ActionResult> Search([FromQuery] string? title, [FromQuery] short? status, [FromQuery] short? priority, [FromQuery] int? projectId, [FromQuery] int? tagId) => Ok(await service.GetAllAsync(title, status, priority, projectId, tagId));
    [HttpGet("project/{projectId:int}")] public async Task<ActionResult> GetByProject(int projectId) => Ok(await service.GetByProjectAsync(projectId));
    [HttpGet("{id:int}")] public async Task<ActionResult> GetById(int id) => Ok(await service.GetByIdAsync(id));
    [HttpPost] public async Task<ActionResult> Create(TaskRequest request) { var created = await service.CreateAsync(request); return CreatedAtAction(nameof(GetById), new { id = created.TaskId }, created); }
    [HttpPut("{id:int}")] public async Task<ActionResult> Update(int id, TaskRequest request) => Ok(await service.UpdateAsync(id, request));
    [HttpDelete("{id:int}")] public async Task<ActionResult> Delete(int id) { await service.DeleteAsync(id); return NoContent(); }
}
