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
