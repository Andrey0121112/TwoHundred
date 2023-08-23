using System;
using System.Collections.Generic;

namespace TwoHundred.Server.Entities;

public record Contract : IEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid VendorMemberId { get; set; }
    public Guid SupplierMemberId { get; set; }
}
