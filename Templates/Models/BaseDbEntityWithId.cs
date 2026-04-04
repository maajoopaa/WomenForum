using System.ComponentModel.DataAnnotations;

namespace Templates.Models;

public class BaseDbEntityWithId
{
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();
}