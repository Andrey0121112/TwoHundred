using System;

namespace TwoHundred.Server.Entities;

public record ContractHistory : IEntity
{
    public Guid CompanyId { get; set; }
    public string Description { get; set;} = string.Empty;
}
