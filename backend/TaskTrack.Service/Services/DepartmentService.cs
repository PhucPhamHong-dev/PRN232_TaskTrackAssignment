using Department = TaskTrack.Repo.Models.Department;
using TaskTrack.Repo.Repositories;
using TaskTrack.Service.Dtos;
using TaskTrack.Service.Exceptions;
using TaskTrack.Service.Interfaces;

namespace TaskTrack.Service.Services;

public class DepartmentService(ITaskTrackRepository repository) : IDepartmentService
{
    public async Task<IReadOnlyList<DepartmentSummaryDto>> GetAllAsync(string? name = null) =>
        (await repository.GetDepartmentsAsync(name)).Select(MapSummary).ToList();

    public async Task<DepartmentDto> GetByIdAsync(int id)
    {
        var department = await repository.GetDepartmentAsync(id, true) ?? throw new ServiceException("Department not found.", 404);
        return new(department.DepartmentId, department.DepartmentName, department.DepartmentDescription, department.IsActive,
            department.Projects.Select(p => new ProjectSummaryDto(p.ProjectId, p.ProjectName, p.Description, p.StartDate, p.EndDate, p.Status, p.DepartmentId, department.DepartmentName, p.IsActive, p.CreatedDate)).ToList());
    }

    public async Task<DepartmentSummaryDto> CreateAsync(DepartmentRequest request)
    {
        var entity = new Department { DepartmentName = request.DepartmentName.Trim(), DepartmentDescription = request.DepartmentDescription.Trim(), IsActive = true };
        await repository.AddDepartmentAsync(entity); await repository.SaveChangesAsync();
        return MapSummary(entity);
    }

    public async Task<DepartmentSummaryDto> UpdateAsync(int id, DepartmentRequest request)
    {
        var entity = await repository.GetDepartmentAsync(id) ?? throw new ServiceException("Department not found.", 404);
        entity.DepartmentName = request.DepartmentName.Trim(); entity.DepartmentDescription = request.DepartmentDescription.Trim();
        repository.UpdateDepartment(entity); await repository.SaveChangesAsync();
        return MapSummary(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await repository.GetDepartmentAsync(id) ?? throw new ServiceException("Department not found.", 404);
        if (await repository.DepartmentHasProjectsAsync(id)) throw new ServiceException("Cannot delete a department that has projects.");
        repository.DeleteDepartment(entity); await repository.SaveChangesAsync();
    }

    private static DepartmentSummaryDto MapSummary(Department x) => new(x.DepartmentId, x.DepartmentName);
}
