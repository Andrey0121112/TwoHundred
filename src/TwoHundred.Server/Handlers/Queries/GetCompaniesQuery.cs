using AutoMapper;
using ErrorOr;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using TwoHundred.Server.Abstractions;
using TwoHundred.Server.Resources;
using TwoHundred.Server.Entities;
using System.Linq;

namespace TwoHundred.Server.Handlers.Queries;

public class GetCompaniesQuery : IRequest<ErrorOr<IEnumerable<CompanyResource>>>
{
}

public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, ErrorOr<IEnumerable<CompanyResource>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetCompaniesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ErrorOr<IEnumerable<CompanyResource>>> Handle(GetCompaniesQuery request, CancellationToken token)
    {
        var companies = await unitOfWork.Repository().FindAllAsync<Company>(token);

        if (companies is null || companies.Any() == false)
        {
            return Error.NotFound(code: "Collection is empty");
        }

        return companies.Select(mapper.Map<CompanyResource>).ToArray();
    }
}
