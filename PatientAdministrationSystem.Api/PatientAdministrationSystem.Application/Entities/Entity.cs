using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientAdministrationSystem.Application.Entities;

public abstract class Entity : Entity<Guid>
{
}

public abstract class Entity<TKey> where TKey : struct
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TKey Id { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedDate { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedDate { get; set; }
}