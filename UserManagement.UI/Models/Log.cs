using UserManagement.UI.Enums;

namespace UserManagement.UI.Models;
public class Log
{
    public long Id { get; set; }
    public LogType Type { get; set; }
    public string Description { get; set; } = default!;
    public DateTime DateTime { get; set; } = DateTime.UtcNow;
    public User? User { get; set; }
}

public class LogListViewModel
{
    public List<LogListItemViewModel> Items { get; set; } = new();
}

public class LogListItemViewModel
{
    public long Id { get; set; }
    public LogType Type { get; set; }
    public string? Description { get; set; }
    public DateTime DateTime { get; set; }
}

public static class LogExtensions
{
    public static LogListViewModel ToListViewModel(this ICollection<Log> logs) => new LogListViewModel
    {
        Items = logs.Select(x => x.ToListItemViewModel()).ToList()
    };

    public static LogListItemViewModel ToListItemViewModel(this Log log) => new LogListItemViewModel
    {
        Id = log.Id,
        Type = log.Type,
        Description = log.Description,
        DateTime = log.DateTime
    };
}
