using TaskTrack.Repo.Models;
using TaskTrack.Repo.Repositories;
using TaskTrack.Service.Dtos;
using TaskTrack.Service.Exceptions;
using TaskTrack.Service.Interfaces;

namespace TaskTrack.Service.Services;

public class ProjectService(ITaskTrackRepository repository) : IProjectService
{
    public async Task<IReadOnlyList<ProjectSummaryDto>> GetAllAsync(string? name = null, short? status = null, int? departmentId = null) =>
        (await repository.GetProjectsAsync(name, status, departmentId)).Select(MapSummary).ToList();

    public async Task<ProjectDto> GetByIdAsync(int id)
    {
        var project = await repository.GetProjectAsync(id, true) ?? throw new ServiceException("Project not found.", 404);
        return new(project.ProjectId, project.ProjectName, project.Description, project.StartDate, project.EndDate, project.Status, project.DepartmentId, project.Department.DepartmentName, project.IsActive, project.CreatedDate,
            project.Tasks.Select(TaskService.Map).ToList());
    }

    public async Task<IReadOnlyList<ProjectSummaryDto>> GetByDepartmentAsync(int departmentId)
    {
        if (!await repository.DepartmentExistsAsync(departmentId)) throw new ServiceException("Department not found.", 404);
        return (await repository.GetProjectsAsync(departmentId: departmentId)).Select(MapSummary).ToList();
    }

    public async Task<ProjectSummaryDto> CreateAsync(ProjectRequest request)
    {
        if (!await repository.DepartmentExistsAsync(request.DepartmentId)) throw new ServiceException("DepartmentId does not reference an active department.", 400, new Dictionary<string, string[]> { ["departmentId"] = ["The selected department does not exist."] });
        ValidateDates(request.StartDate, request.EndDate);
        var entity = new Project { ProjectName = request.ProjectName.Trim(), Description = request.Description?.Trim(), StartDate = request.StartDate!.Value, EndDate = request.EndDate, Status = request.Status, DepartmentId = request.DepartmentId, IsActive = true, CreatedDate = DateTime.UtcNow };
        await repository.AddProjectAsync(entity); await repository.SaveChangesAsync();
        var saved = await repository.GetProjectAsync(entity.ProjectId) ?? entity;
        return MapSummary(saved);
    }

    public async Task<ProjectSummaryDto> UpdateAsync(int id, ProjectRequest request)
    {
        var entity = await repository.GetProjectAsync(id) ?? throw new ServiceException("Project not found.", 404);
        if (!await repository.DepartmentExistsAsync(request.DepartmentId)) throw new ServiceException("DepartmentId does not reference an active department.");
        ValidateDates(request.StartDate, request.EndDate);
        entity.ProjectName = request.ProjectName.Trim(); entity.Description = request.Description?.Trim(); entity.StartDate = request.StartDate!.Value; entity.EndDate = request.EndDate; entity.Status = request.Status; entity.DepartmentId = request.DepartmentId;
        repository.UpdateProject(entity); await repository.SaveChangesAsync();
        return MapSummary(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await repository.GetProjectAsync(id) ?? throw new ServiceException("Project not found.", 404);
        if (await repository.ProjectHasTasksAsync(id)) throw new ServiceException("Cannot delete a project that has tasks.");
        repository.DeleteProject(entity); await repository.SaveChangesAsync();
    }

    private static void ValidateDates(DateOnly? start, DateOnly? end)
    {
        if (end.HasValue && start.HasValue && end < start) throw new ServiceException("EndDate must be on or after StartDate.", 400, new Dictionary<string, string[]> { ["endDate"] = ["EndDate must be on or after StartDate."] });
    }

    private static ProjectSummaryDto MapSummary(Project x) => new(x.ProjectId, x.ProjectName, x.Description, x.StartDate, x.EndDate, x.Status, x.DepartmentId, x.Department?.DepartmentName ?? string.Empty, x.IsActive, x.CreatedDate);
}
