using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace TwoHundred.Server.Entities;

public record Company : IEntity
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string ExistingContacts { get; set; } = string.Empty;

}
