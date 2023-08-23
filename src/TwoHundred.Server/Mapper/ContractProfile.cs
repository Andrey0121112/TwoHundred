using AutoMapper;
using TwoHundred.Server.Entities;
using TwoHundred.Server.Resources;

namespace TwoHundred.Server.Mapper;

public class ContractProfile : Profile
{

    public ContractProfile()
    {
        CreateMap<Contract, ContractResource>()
                    .ForMember(t => t.Id, o => o.MapFrom(t => t.Id))
                    .ForMember(t => t.CreatedAt, o => o.MapFrom(t => t.CreatedAt))
                    .ForMember(t => t.UpdatedAt, o => o.MapFrom(t => t.UpdatedAt))
                    .ForMember(t => t.Name, o => o.MapFrom(t => t.Name))
                    .ForMember(t => t.SupplierMemberId, o => o.MapFrom(t => t.SupplierMemberId))
                    .ForMember(t => t.VendorMemberId, o => o.MapFrom(t => t.VendorMemberId))
                    .ReverseMap();
    }
}
