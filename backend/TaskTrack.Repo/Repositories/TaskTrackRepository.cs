using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Department = TaskTrack.Repo.Models.Department;
using Project = TaskTrack.Repo.Models.Project;
using Tag = TaskTrack.Repo.Models.Tag;
using WorkTask = TaskTrack.Repo.Models.Task;

namespace TaskTrack.Repo.Repositories;

public class TaskTrackRepository(TaskManagementContext db) : ITaskTrackRepository
{
    public async Task<List<Department>> GetDepartmentsAsync(string? name = null) =>
        await db.Departments.AsNoTracking().Where(x => x.IsActive && (name == null || x.DepartmentName.ToLower().Contains(name.ToLower()))).OrderBy(x => x.DepartmentName).ToListAsync();

    public async Task<Department?> GetDepartmentAsync(int id, bool includeProjects = false)
    {
        IQueryable<Department> query = db.Departments;
        if (includeProjects) query = query.Include(x => x.Projects.Where(p => p.IsActive));
        return await query.FirstOrDefaultAsync(x => x.DepartmentId == id && x.IsActive);
    }

    public Task<bool> DepartmentExistsAsync(int id) => db.Departments.AnyAsync(x => x.DepartmentId == id && x.IsActive);
    public Task<bool> DepartmentHasProjectsAsync(int id) => db.Projects.AnyAsync(x => x.DepartmentId == id);
    public Task AddDepartmentAsync(Department department) { db.Departments.Add(department); return Task.CompletedTask; }
    public void UpdateDepartment(Department department) => db.Departments.Update(department);
    public void DeleteDepartment(Department department) => db.Departments.Remove(department);

    public async Task<List<Project>> GetProjectsAsync(string? name = null, short? status = null, int? departmentId = null) =>
        await db.Projects.AsNoTracking().Include(x => x.Department).Where(x => x.IsActive && (name == null || x.ProjectName.ToLower().Contains(name.ToLower())) && (status == null || x.Status == status) && (departmentId == null || x.DepartmentId == departmentId)).OrderByDescending(x => x.CreatedDate).ToListAsync();

    public async Task<Project?> GetProjectAsync(int id, bool includeTasks = false)
    {
        IQueryable<Project> query = db.Projects.Include(x => x.Department);
        if (includeTasks) query = query.AsSplitQuery().Include(x => x.Tasks.Where(t => t.IsActive)).ThenInclude(x => x.Tags);
        return await query.FirstOrDefaultAsync(x => x.ProjectId == id && x.IsActive);
    }

    public Task<bool> ProjectExistsAsync(int id) => db.Projects.AnyAsync(x => x.ProjectId == id && x.IsActive);
    public Task<bool> ProjectHasTasksAsync(int id) => db.Tasks.AnyAsync(x => x.ProjectId == id);
    public Task AddProjectAsync(Project project) { db.Projects.Add(project); return Task.CompletedTask; }
    public void UpdateProject(Project project) => db.Projects.Update(project);
    public void DeleteProject(Project project) => db.Projects.Remove(project);

    public async Task<List<WorkTask>> GetTasksAsync(string? title = null, short? status = null, short? priority = null, int? projectId = null, int? tagId = null) =>
        await db.Tasks.AsNoTracking().Include(x => x.Project).Include(x => x.Tags).Where(x => x.IsActive && (title == null || x.Title.ToLower().Contains(title.ToLower())) && (status == null || x.Status == status) && (priority == null || x.Priority == priority) && (projectId == null || x.ProjectId == projectId) && (tagId == null || x.Tags.Any(tag => tag.TagId == tagId))).OrderBy(x => x.DueDate).ToListAsync();

    public async Task<WorkTask?> GetTaskAsync(int id, bool includeTags = false)
    {
        IQueryable<WorkTask> query = db.Tasks.Include(x => x.Project);
        if (includeTags) query = query.Include(x => x.Project).ThenInclude(x => x.Department).Include(x => x.Tags);
        return await query.FirstOrDefaultAsync(x => x.TaskId == id && x.IsActive);
    }

    public Task<bool> TaskExistsAsync(int id) => db.Tasks.AnyAsync(x => x.TaskId == id && x.IsActive);
    public Task AddTaskAsync(WorkTask task) { db.Tasks.Add(task); return Task.CompletedTask; }
    public void UpdateTask(WorkTask task) => db.Tasks.Update(task);

    public async Task ReplaceTaskTagsAsync(WorkTask task, IEnumerable<int> tagIds)
    {
        var ids = tagIds.Distinct().ToList();
        var tags = await db.Tags.Where(x => ids.Contains(x.TagId)).ToListAsync();
        task.Tags.Clear();
        foreach (var tag in tags) task.Tags.Add(tag);
    }

    public void SoftDeleteTask(WorkTask task) => task.IsActive = false;

    public Task<List<Tag>> GetTagsAsync() => db.Tags.AsNoTracking().OrderBy(x => x.TagName).ToListAsync();
    public Task<Tag?> GetTagAsync(int id) => db.Tags.FirstOrDefaultAsync(x => x.TagId == id);
    public Task<bool> TagExistsAsync(int id) => db.Tags.AnyAsync(x => x.TagId == id);
    public Task<bool> TagNameExistsAsync(string name, int? exceptId = null) => db.Tags.AnyAsync(x => x.TagName.ToLower() == name.ToLower() && (exceptId == null || x.TagId != exceptId));
    public Task<bool> TagIsUsedAsync(int id) => db.Tags.AnyAsync(x => x.TagId == id && x.Tasks.Any());
    public Task<List<Tag>> GetTagsByIdsAsync(IEnumerable<int> ids) => db.Tags.Where(x => ids.Contains(x.TagId)).ToListAsync();
    public Task AddTagAsync(Tag tag) { db.Tags.Add(tag); return Task.CompletedTask; }
    public void UpdateTag(Tag tag) => db.Tags.Update(tag);
    public void DeleteTag(Tag tag) => db.Tags.Remove(tag);
    public Task<int> SaveChangesAsync() => db.SaveChangesAsync();
}
