using TaskTrack.Service.Dtos;

namespace TaskTrack.Service.Interfaces;

public interface IDepartmentService
{
    Task<IReadOnlyList<DepartmentSummaryDto>> GetAllAsync(string? name = null);
    Task<DepartmentDto> GetByIdAsync(int id);
    Task<DepartmentSummaryDto> CreateAsync(DepartmentRequest request);
    Task<DepartmentSummaryDto> UpdateAsync(int id, DepartmentRequest request);
    Task DeleteAsync(int id);
}
