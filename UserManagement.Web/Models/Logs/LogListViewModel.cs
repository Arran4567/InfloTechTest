using System;
using UserManagement.Data.Enums;

namespace UserManagement.Web.Models.Logs;

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
