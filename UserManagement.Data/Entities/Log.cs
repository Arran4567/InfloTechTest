using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UserManagement.Data.Enums;

namespace UserManagement.Models;
public class Log
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public LogType Type { get; set; }
    public string Description { get; set; } = default!;
    public DateTime DateTime { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public virtual User? User { get; set; }
}
