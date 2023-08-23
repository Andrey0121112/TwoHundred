namespace TwoHundred.Models;

public record ContractFullDto(string Id, string Name, int[] MemberIds) : ContractDto(Id, Name);

public record ContractDto(string Id, string Name);
