using Microsoft.AspNetCore.Mvc;
using TaskTrack.Service.Dtos;
using TaskTrack.Service.Interfaces;

namespace TaskTrack.API.Controllers;

[ApiController, Route("api/tags")]
public class TagsController(ITagService service) : ControllerBase
{
    [HttpGet] public async Task<ActionResult> GetAll() => Ok(await service.GetAllAsync());
    [HttpPost] public async Task<ActionResult> Create(TagRequest request) => Ok(await service.CreateAsync(request));
    [HttpPut("{id:int}")] public async Task<ActionResult> Update(int id, TagRequest request) => Ok(await service.UpdateAsync(id, request));
    [HttpDelete("{id:int}")] public async Task<ActionResult> Delete(int id) { await service.DeleteAsync(id); return NoContent(); }
}
