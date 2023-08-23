using AutoMapper;
using ErrorOr;
using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using TwoHundred.Server.Abstractions;
using TwoHundred.Server.Entities;
using System.Linq;

namespace TwoHundred.Server.Handlers.Queries;

public class UndoContractQuery : IRequest<ErrorOr<bool>>
{
    public Guid Id { get; }

    public UndoContractQuery(Guid id)
    {
        Id = id;
    }
}

public class UndoContractQueryHandler : IRequestHandler<UndoContractQuery, ErrorOr<bool>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public UndoContractQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ErrorOr<bool>> Handle(UndoContractQuery request, CancellationToken token)
    {
        try
        {
            var contract = await unitOfWork.Repository().GetById<Contract>(request.Id);

            if (contract is null)
            {
                return Error.NotFound(code: "Cannot restore contat by that id");
            }

            var supplierCompany = await unitOfWork.Repository().GetById<Company>(contract!.SupplierMemberId);
            var vendorCompany = await unitOfWork.Repository().GetById<Company>(contract!.VendorMemberId);

            unitOfWork.Repository().Delete<Contract>(new Contract() { Id = request.Id });

            if (supplierCompany is not null)
            {
                unitOfWork.Repository().Update(UpdateModel(supplierCompany, request.Id.ToString()));
            }

            if(vendorCompany is not null)
            {
                unitOfWork.Repository().Update(UpdateModel(vendorCompany, request.Id.ToString()));
            }

            await unitOfWork.CommitAsync(token);
        }
        catch (Exception ex)
        {
            return Error.Failure(code: "Error happened", description: ex.Message);
        }

        await unitOfWork.CommitAsync(token);
        return true;
    }

    private static Company UpdateModel(Company company, string id)
    {
        company.ExistingContacts = string.Join(',', company.ExistingContacts.Split(',').Where(_ => _.Equals(id) == false));
        return company;
    }
}

public class UndoContractQueryValidator : AbstractValidator<DeleteCompanyQuery>
{
    public UndoContractQueryValidator()
    {
        RuleFor(_ => _.Id).NotEmpty();
    }
}