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
using System;
using TwoHundred.Server.Extensions;
using TwoHundred.Server.EnumModels;

namespace TwoHundred.Server.Handlers.Queries;

public class SignContractQuery : IRequest<ErrorOr<ContractResource>>
{
    
    public Guid FirstCompanyId { get; }
    public Guid SecondCompanyId { get; }
    public string ContractTitle { get; }


    public SignContractQuery(Guid firstCompanyId, Guid secondCompanyId, string contractTitle)
    {
        FirstCompanyId = firstCompanyId;
        SecondCompanyId = secondCompanyId;
        ContractTitle = contractTitle;
    }
}


public class SignContractQueryHandler : IRequestHandler<SignContractQuery, ErrorOr<ContractResource>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public SignContractQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ErrorOr<ContractResource>> Handle(SignContractQuery request, CancellationToken token)
    {
        var firstCompany = await unitOfWork.Repository().GetById<Company>(request.FirstCompanyId);
        var secondCompany = await unitOfWork.Repository().GetById<Company>(request.SecondCompanyId);

        if (firstCompany is null || secondCompany is null)
        {
            return Error.NotFound(code: "Cannot restore company by id");
        }

        if (firstCompany?.Type.Equals(secondCompany?.Type, System.StringComparison.OrdinalIgnoreCase) == true)
        {
            return Error.Validation(code: "To create a contract, companies must be of different types");
        }

        var contract = unitOfWork.Repository().Add<Contract>(
                                GetNewContract(new Company[] { firstCompany!, secondCompany! }, request.ContractTitle));


        SingContract(firstCompany!, secondCompany!.Name, contract!.Id, request.ContractTitle);
        SingContract(secondCompany!, firstCompany!.Name, contract!.Id, request.ContractTitle);

        if (contract is null)
        {
            return Error.Failure(code: "Error when trying to save model");
        }


        await unitOfWork.CommitAsync(token); 
        return mapper.Map<ContractResource>(contract);
    }

    private static Contract GetNewContract(IEnumerable<Company> companies, string contractTitle)
    {
        var contract = new Contract() { Name = contractTitle, Id = Guid.NewGuid()};

        foreach (var company in companies) 
        {
            switch(company.Type.ToEnum<CompanyType>())
            { 
                case CompanyType.Supplier:
                    contract.SupplierMemberId = company.Id;
                break;
                case CompanyType.Vendor:
                    contract.VendorMemberId = company.Id;
                break;
            }
        }

        return contract;
    }

    private void SingContract(Company companyMain, string targetCompanyName, Guid contractId, string contractTitle)
    {
        unitOfWork.Repository().Update<Company>(AddContractToModel(companyMain, contractId));
        unitOfWork.Repository().Add<ContractHistory>(new ContractHistory()
        {
            CompanyId = companyMain.Id,
            Description = $"{DateTime.Now} - Singed contract ({contractTitle}) with company {targetCompanyName}"
        });
    }

    private static Company AddContractToModel(Company companyEntity, Guid contractId)
    {
        var collection = new List<string>();
        if (companyEntity.ExistingContacts.Equals(string.Empty) == false)
        {
            collection.AddRange(companyEntity.ExistingContacts.Split(','));
        }

        collection.Add(contractId.ToString());
        companyEntity.ExistingContacts = string.Join(',', collection);

        return companyEntity;
    }
}

public class SignContractQueryValidator : AbstractValidator<SignContractQuery>
{
    public SignContractQueryValidator()
    {
        RuleFor(_ => _.ContractTitle).NotEmpty();
        RuleFor(_ => _.FirstCompanyId).NotEmpty();
        RuleFor(_ => _.SecondCompanyId).NotEmpty();
    }
}
