using System;

namespace TwoHundred.Server.Resources;

public class ContractHistoryRecource
{
    public Guid Id { get; init; }
    public Guid CompanyId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
