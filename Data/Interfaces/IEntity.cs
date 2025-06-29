using System.ComponentModel.DataAnnotations;

namespace Zorro.Data.Interfaces;

public interface IEntity
{
    [Key]
    public int Id { get; set; }
}