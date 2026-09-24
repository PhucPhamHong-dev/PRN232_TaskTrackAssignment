using Microsoft.EntityFrameworkCore;
using TaskTrack.Repo.Models;

namespace TaskTrack.Repo;

public class TaskManagementContext(DbContextOptions<TaskManagementContext> options) : DbContext(options)
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<WorkTask> Tasks => Set<WorkTask>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<TaskTag> TaskTags => Set<TaskTag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Department");
            entity.HasKey(x => x.DepartmentId);
            entity.Property(x => x.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(x => x.DepartmentName).HasColumnName("DepartmentName").HasMaxLength(100).IsRequired();
            entity.Property(x => x.DepartmentDescription).HasColumnName("DepartmentDescription").HasMaxLength(300).IsRequired();
            entity.Property(x => x.IsActive).HasColumnName("IsActive");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Project");
            entity.HasKey(x => x.ProjectId);
            entity.Property(x => x.ProjectId).HasColumnName("ProjectID");
            entity.Property(x => x.ProjectName).HasColumnName("ProjectName").HasMaxLength(200).IsRequired();
            entity.Property(x => x.Description).HasColumnName("Description");
            entity.Property(x => x.StartDate).HasColumnName("StartDate");
            entity.Property(x => x.EndDate).HasColumnName("EndDate");
            entity.Property(x => x.Status).HasColumnName("Status");
            entity.Property(x => x.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(x => x.IsActive).HasColumnName("IsActive");
            entity.Property(x => x.CreatedDate).HasColumnName("CreatedDate");
            entity.HasOne(x => x.Department).WithMany(x => x.Projects).HasForeignKey(x => x.DepartmentId);
        });

        modelBuilder.Entity<WorkTask>(entity =>
        {
            entity.ToTable("Task");
            entity.HasKey(x => x.TaskId);
            entity.Property(x => x.TaskId).HasColumnName("TaskID");
            entity.Property(x => x.Title).HasColumnName("Title").HasMaxLength(300).IsRequired();
            entity.Property(x => x.Description).HasColumnName("Description");
            entity.Property(x => x.Status).HasColumnName("Status");
            entity.Property(x => x.Priority).HasColumnName("Priority");
            entity.Property(x => x.DueDate).HasColumnName("DueDate");
            entity.Property(x => x.ProjectId).HasColumnName("ProjectID");
            entity.Property(x => x.IsActive).HasColumnName("IsActive");
            entity.Property(x => x.CreatedDate).HasColumnName("CreatedDate");
            entity.Property(x => x.ModifiedDate).HasColumnName("ModifiedDate");
            entity.HasOne(x => x.Project).WithMany(x => x.Tasks).HasForeignKey(x => x.ProjectId);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tag");
            entity.HasKey(x => x.TagId);
            entity.Property(x => x.TagId).HasColumnName("TagID");
            entity.Property(x => x.TagName).HasColumnName("TagName").HasMaxLength(50).IsRequired();
            entity.Property(x => x.Color).HasColumnName("Color").HasMaxLength(7);
            entity.HasIndex(x => x.TagName).IsUnique();
        });

        modelBuilder.Entity<TaskTag>(entity =>
        {
            entity.ToTable("TaskTag");
            entity.HasKey(x => new { x.TaskId, x.TagId });
            entity.Property(x => x.TaskId).HasColumnName("TaskID");
            entity.Property(x => x.TagId).HasColumnName("TagID");
            entity.HasOne(x => x.Task).WithMany(x => x.TaskTags).HasForeignKey(x => x.TaskId);
            entity.HasOne(x => x.Tag).WithMany(x => x.TaskTags).HasForeignKey(x => x.TagId);
        });
    }
}
