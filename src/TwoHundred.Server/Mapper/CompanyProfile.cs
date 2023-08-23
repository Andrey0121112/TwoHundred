using AutoMapper;
using System;
using TwoHundred.Server.Entities;
using TwoHundred.Server.Resources;

namespace TwoHundred.Server.Mapper;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<Company, CompanyResource>()
                    .ForMember(t => t.Id, o => o.MapFrom(t => t.Id))
                    .ForMember(t => t.CreatedAt, o => o.MapFrom(t => t.CreatedAt))
                    .ForMember(t => t.UpdatedAt, o => o.MapFrom(t => t.UpdatedAt))
                    .ForMember(t => t.Name, o => o.MapFrom(t => t.Name))
                    .ForMember(t => t.Type, o => o.MapFrom(t => t.Type))
                    .ForMember(t => t.ExistingContacts, o => o.MapFrom(t => t.ExistingContacts))
                    .ReverseMap();
    }
}
