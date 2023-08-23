using System.ComponentModel.DataAnnotations;
using System;

namespace TwoHundred.Server.Entities;

public record IEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
