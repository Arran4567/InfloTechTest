using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserManagement.Data.Enums;

namespace UserManagement.Models;
public class Log
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public LogType Type { get; set; }
    public string Description { get; set; } = default!;
    public DateTime DateTime { get; set; } = DateTime.UtcNow;
    public virtual User? User { get; set; }
}
