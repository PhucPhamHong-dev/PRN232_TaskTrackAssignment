using System.ComponentModel.DataAnnotations;

namespace TaskTrack.Service.Dtos;

public class DepartmentRequest
{
    [Required, StringLength(100)] public string DepartmentName { get; set; } = string.Empty;
    [Required, StringLength(300)] public string DepartmentDescription { get; set; } = string.Empty;
}

public class ProjectRequest
{
    [Required, StringLength(200)] public string ProjectName { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required] public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    [Range(0, 3)] public short Status { get; set; }
    [Range(1, int.MaxValue)] public int DepartmentId { get; set; }
}

public class TaskRequest
{
    [Required, StringLength(300)] public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Range(0, 3)] public short Status { get; set; }
    [Range(0, 3)] public short Priority { get; set; } = 1;
    public DateOnly? DueDate { get; set; }
    [Range(1, int.MaxValue)] public int ProjectId { get; set; }
    public List<int> TagIds { get; set; } = [];
}

public class TagRequest
{
    [Required, StringLength(50)] public string TagName { get; set; } = string.Empty;
    [RegularExpression("^#[0-9A-Fa-f]{6}$", ErrorMessage = "Color must be a 6-digit HEX value.")] public string? Color { get; set; }
}

public record TagDto(int TagId, string TagName, string? Color);
public record DepartmentDto(int DepartmentId, string DepartmentName, string DepartmentDescription, bool IsActive, IReadOnlyList<ProjectSummaryDto> Projects);
public record DepartmentSummaryDto(int DepartmentId, string DepartmentName, string DepartmentDescription, bool IsActive);
public record ProjectSummaryDto(int ProjectId, string ProjectName, string? Description, DateOnly StartDate, DateOnly? EndDate, short Status, int DepartmentId, string DepartmentName, bool IsActive, DateTime CreatedDate);
public record ProjectDto(int ProjectId, string ProjectName, string? Description, DateOnly StartDate, DateOnly? EndDate, short Status, int DepartmentId, string DepartmentName, bool IsActive, DateTime CreatedDate, IReadOnlyList<TaskDto> Tasks);
public record TaskDto(int TaskId, string Title, string? Description, short Status, short Priority, DateOnly? DueDate, int ProjectId, string ProjectName, bool IsActive, DateTime CreatedDate, DateTime? ModifiedDate, IReadOnlyList<TagDto> Tags);
