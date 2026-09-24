using TaskTrack.Repo.Models;
using TaskTrack.Repo.Repositories;
using TaskTrack.Service.Dtos;
using TaskTrack.Service.Exceptions;
using TaskTrack.Service.Interfaces;

namespace TaskTrack.Service.Services;

public class TagService(ITaskTrackRepository repository) : ITagService
{
    public async Task<IReadOnlyList<TagDto>> GetAllAsync() => (await repository.GetTagsAsync()).Select(Map).ToList();

    public async Task<TagDto> CreateAsync(TagRequest request)
    {
        if (await repository.TagNameExistsAsync(request.TagName.Trim())) throw new ServiceException("TagName already exists.", 400, new Dictionary<string, string[]> { ["tagName"] = ["TagName must be unique."] });
        var entity = new Tag { TagName = request.TagName.Trim(), Color = request.Color };
        await repository.AddTagAsync(entity); await repository.SaveChangesAsync(); return Map(entity);
    }

    public async Task<TagDto> UpdateAsync(int id, TagRequest request)
    {
        var entity = await repository.GetTagAsync(id) ?? throw new ServiceException("Tag not found.", 404);
        if (await repository.TagNameExistsAsync(request.TagName.Trim(), id)) throw new ServiceException("TagName already exists.");
        entity.TagName = request.TagName.Trim(); entity.Color = request.Color; repository.UpdateTag(entity); await repository.SaveChangesAsync(); return Map(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await repository.GetTagAsync(id) ?? throw new ServiceException("Tag not found.", 404);
        if (await repository.TagIsUsedAsync(id)) throw new ServiceException("Cannot delete a tag that is used by a task.");
        repository.DeleteTag(entity); await repository.SaveChangesAsync();
    }

    private static TagDto Map(Tag x) => new(x.TagId, x.TagName, x.Color);
}
