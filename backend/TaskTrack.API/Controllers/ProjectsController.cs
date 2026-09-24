using Microsoft.AspNetCore.Mvc;
using TaskTrack.Service.Dtos;
using TaskTrack.Service.Interfaces;

namespace TaskTrack.API.Controllers;

[ApiController, Route("api/projects")]
public class ProjectsController(IProjectService service) : ControllerBase
{
    [HttpGet] public async Task<ActionResult> GetAll([FromQuery] string? name, [FromQuery] short? status, [FromQuery] int? departmentId) => Ok(await service.GetAllAsync(name, status, departmentId));
    [HttpGet("search")] public async Task<ActionResult> Search([FromQuery] string? name, [FromQuery] short? status, [FromQuery] int? departmentId) => Ok(await service.GetAllAsync(name, status, departmentId));
    [HttpGet("department/{departmentId:int}")] public async Task<ActionResult> GetByDepartment(int departmentId) => Ok(await service.GetByDepartmentAsync(departmentId));
    [HttpGet("{id:int}")] public async Task<ActionResult> GetById(int id) => Ok(await service.GetByIdAsync(id));
    [HttpPost] public async Task<ActionResult> Create(ProjectRequest request) { var created = await service.CreateAsync(request); return CreatedAtAction(nameof(GetById), new { id = created.ProjectId }, created); }
    [HttpPut("{id:int}")] public async Task<ActionResult> Update(int id, ProjectRequest request) => Ok(await service.UpdateAsync(id, request));
    [HttpDelete("{id:int}")] public async Task<ActionResult> Delete(int id) { await service.DeleteAsync(id); return NoContent(); }
}
