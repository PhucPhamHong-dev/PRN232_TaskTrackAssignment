using TaskTrack.Repo.Models;
using TaskTrack.Repo.Repositories;
using TaskTrack.Service.Dtos;
using TaskTrack.Service.Exceptions;
using TaskTrack.Service.Interfaces;

namespace TaskTrack.Service.Services;

public class TaskService(ITaskTrackRepository repository) : ITaskService
{
    public async Task<IReadOnlyList<TaskDto>> GetAllAsync(string? title = null, short? status = null, short? priority = null, int? projectId = null, int? tagId = null) =>
        (await repository.GetTasksAsync(title, status, priority, projectId, tagId)).Select(Map).ToList();

    public async Task<TaskDto> GetByIdAsync(int id) => Map(await repository.GetTaskAsync(id, true) ?? throw new ServiceException("Task not found.", 404));

    public async Task<IReadOnlyList<TaskDto>> GetByProjectAsync(int projectId)
    {
        if (!await repository.ProjectExistsAsync(projectId)) throw new ServiceException("Project not found.", 404);
        return (await repository.GetTasksAsync(projectId: projectId)).Select(Map).ToList();
    }

    public async Task<TaskDto> CreateAsync(TaskRequest request)
    {
        if (!await repository.ProjectExistsAsync(request.ProjectId)) throw new ServiceException("ProjectId does not reference an active project.", 400, new Dictionary<string, string[]> { ["projectId"] = ["The selected project does not exist."] });
        var tags = await ValidateTags(request.TagIds);
        var entity = new WorkTask { Title = request.Title.Trim(), Description = request.Description?.Trim(), Status = request.Status, Priority = request.Priority, DueDate = request.DueDate, ProjectId = request.ProjectId, IsActive = true, CreatedDate = DateTime.UtcNow };
        await repository.AddTaskAsync(entity); await repository.SaveChangesAsync();
        if (tags.Count > 0) { await repository.ReplaceTaskTagsAsync(entity, tags.Select(x => x.TagId)); await repository.SaveChangesAsync(); }
        return Map(await repository.GetTaskAsync(entity.TaskId, true) ?? entity);
    }

    public async Task<TaskDto> UpdateAsync(int id, TaskRequest request)
    {
        var entity = await repository.GetTaskAsync(id, true) ?? throw new ServiceException("Task not found.", 404);
        if (!await repository.ProjectExistsAsync(request.ProjectId)) throw new ServiceException("ProjectId does not reference an active project.");
        var tags = await ValidateTags(request.TagIds);
        entity.Title = request.Title.Trim(); entity.Description = request.Description?.Trim(); entity.Status = request.Status; entity.Priority = request.Priority; entity.DueDate = request.DueDate; entity.ProjectId = request.ProjectId; entity.ModifiedDate = DateTime.UtcNow;
        repository.UpdateTask(entity); await repository.ReplaceTaskTagsAsync(entity, tags.Select(x => x.TagId)); await repository.SaveChangesAsync();
        return Map(await repository.GetTaskAsync(id, true) ?? entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await repository.GetTaskAsync(id) ?? throw new ServiceException("Task not found.", 404);
        repository.SoftDeleteTask(entity); await repository.SaveChangesAsync();
    }

    private async Task<List<Tag>> ValidateTags(IEnumerable<int> tagIds)
    {
        var ids = tagIds.Distinct().ToList();
        var tags = await repository.GetTagsByIdsAsync(ids);
        if (tags.Count != ids.Count) throw new ServiceException("One or more TagIds do not exist.", 400, new Dictionary<string, string[]> { ["tagIds"] = ["All selected tags must exist."] });
        return tags;
    }

    public static TaskDto Map(WorkTask x) => new(x.TaskId, x.Title, x.Description, x.Status, x.Priority, x.DueDate, x.ProjectId, x.Project?.ProjectName ?? string.Empty, x.IsActive, x.CreatedDate, x.ModifiedDate, x.TaskTags?.Select(tt => new TagDto(tt.TagId, tt.Tag.TagName, tt.Tag.Color)).ToList() ?? []);
}
