using AutoMapper;
using ErrorOr;
using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using TwoHundred.Server.Abstractions;
using TwoHundred.Server.Entities;
using TwoHundred.Server.Resources;
using System.Collections.Generic;
using System.Linq;
using System;

namespace TwoHundred.Server.Handlers.Queries;

public class GetCompanyContractHistoryQuery : IRequest<ErrorOr<IEnumerable<ContractHistoryRecource>>>
{
    public Guid Id { get; }

    public GetCompanyContractHistoryQuery(Guid id)
    {
        Id = id;
    }
}

public class GetCompanyContractHistoryQueryHandler : IRequestHandler<GetCompanyContractHistoryQuery, ErrorOr<IEnumerable<ContractHistoryRecource>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetCompanyContractHistoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ErrorOr<IEnumerable<ContractHistoryRecource>>> Handle(GetCompanyContractHistoryQuery request, CancellationToken token)
    {
        var histories = unitOfWork.Repository().FindQueryable<ContractHistory>(_ => _.CompanyId.Equals(request.Id));

        if (histories is null || histories.Any() == false)
        {
            return Error.NotFound(code: "Storage have not history for that contract id");
        }

        await unitOfWork.CommitAsync(token);
        return histories.Select(mapper.Map<ContractHistoryRecource>).ToArray();
    }
}

public class GetCompanyContractHistoryQueryValidator : AbstractValidator<GetCompanyContractHistoryQuery>
{
    public GetCompanyContractHistoryQueryValidator()
    {
        RuleFor(_ => _.Id).NotEmpty();
    }
}

