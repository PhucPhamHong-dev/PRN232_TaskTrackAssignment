using TaskTrack.Service.Dtos;

namespace TaskTrack.Service.Interfaces;

public interface IProjectService
{
    Task<IReadOnlyList<ProjectSummaryDto>> GetAllAsync(string? name = null, short? status = null, int? departmentId = null);
    Task<ProjectDto> GetByIdAsync(int id);
    Task<IReadOnlyList<ProjectSummaryDto>> GetByDepartmentAsync(int departmentId);
    Task<ProjectSummaryDto> CreateAsync(ProjectRequest request);
    Task<ProjectSummaryDto> UpdateAsync(int id, ProjectRequest request);
    Task DeleteAsync(int id);
}
