using System.Collections.Generic;

namespace TwoHundred.Models;

public record CompanyWithContractDto(string Id, string Name, string Type, IEnumerable<ContractDto> ExistingContacts) : CompanyDto(Id, Name, Type);

public record CompanyDto(string Id, string Name, string Type) : CompanyBase(Name, Type);

public abstract record CompanyBase(string Name, string Type);