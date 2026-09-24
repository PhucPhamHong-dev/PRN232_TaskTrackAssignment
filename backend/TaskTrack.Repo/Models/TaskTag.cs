namespace TaskTrack.Repo.Models;

public class TaskTag
{
    public int TaskId { get; set; }
    public int TagId { get; set; }
    public WorkTask Task { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
