using System.Collections.Generic;
using System.Threading.Tasks;
using Department = TaskTrack.Repo.Models.Department;
using Project = TaskTrack.Repo.Models.Project;
using Tag = TaskTrack.Repo.Models.Tag;
using WorkTask = TaskTrack.Repo.Models.Task;

namespace TaskTrack.Repo.Repositories;

public interface ITaskTrackRepository
{
    Task<List<Department>> GetDepartmentsAsync(string? name = null);
    Task<Department?> GetDepartmentAsync(int id, bool includeProjects = false);
    Task<bool> DepartmentExistsAsync(int id);
    Task<bool> DepartmentHasProjectsAsync(int id);
    Task AddDepartmentAsync(Department department);
    void UpdateDepartment(Department department);
    void DeleteDepartment(Department department);

    Task<List<Project>> GetProjectsAsync(string? name = null, short? status = null, int? departmentId = null);
    Task<Project?> GetProjectAsync(int id, bool includeTasks = false);
    Task<bool> ProjectExistsAsync(int id);
    Task<bool> ProjectHasTasksAsync(int id);
    Task AddProjectAsync(Project project);
    void UpdateProject(Project project);
    void DeleteProject(Project project);

    Task<List<WorkTask>> GetTasksAsync(string? title = null, short? status = null, short? priority = null, int? projectId = null, int? tagId = null);
    Task<WorkTask?> GetTaskAsync(int id, bool includeTags = false);
    Task<bool> TaskExistsAsync(int id);
    Task AddTaskAsync(WorkTask task);
    void UpdateTask(WorkTask task);
    Task ReplaceTaskTagsAsync(WorkTask task, IEnumerable<int> tagIds);
    void SoftDeleteTask(WorkTask task);

    Task<List<Tag>> GetTagsAsync();
    Task<Tag?> GetTagAsync(int id);
    Task<bool> TagExistsAsync(int id);
    Task<bool> TagNameExistsAsync(string name, int? exceptId = null);
    Task<bool> TagIsUsedAsync(int id);
    Task<List<Tag>> GetTagsByIdsAsync(IEnumerable<int> ids);
    Task AddTagAsync(Tag tag);
    void UpdateTag(Tag tag);
    void DeleteTag(Tag tag);

    Task<int> SaveChangesAsync();
}
