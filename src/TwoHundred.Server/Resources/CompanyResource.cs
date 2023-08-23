using System;
using System.Collections.Generic;

namespace TwoHundred.Server.Resources;

public class CompanyResource
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string ExistingContacts { get; set; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
