using System;
using System.Collections.Generic;

namespace TwoHundred.Server.Resources;

public class ContractResource
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public Guid VendorMemberId { get; set; }
    public Guid SupplierMemberId { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }

}
