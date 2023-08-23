using AutoMapper;
using TwoHundred.Server.Entities;
using TwoHundred.Server.Resources;

namespace TwoHundred.Server.Mapper;

public class ContractHistoryProfile : Profile
{
    public ContractHistoryProfile()
    {
        CreateMap<ContractHistory, ContractHistoryRecource>()
                    .ForMember(t => t.Id, o => o.MapFrom(t => t.Id))
                    .ForMember(t => t.CreatedAt, o => o.MapFrom(t => t.CreatedAt))
                    .ForMember(t => t.UpdatedAt, o => o.MapFrom(t => t.UpdatedAt))
                    .ForMember(t => t.Description, o => o.MapFrom(t => t.Description))
                    .ForMember(t => t.CompanyId, o => o.MapFrom(t => t.CompanyId))
                    .ReverseMap();
    }
}
