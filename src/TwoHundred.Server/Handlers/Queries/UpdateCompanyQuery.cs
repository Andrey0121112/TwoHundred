using AutoMapper;
using ErrorOr;
using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using TwoHundred.Server.Abstractions;
using TwoHundred.Server.Entities;
using TwoHundred.Server.Resources;
using System;
using System.Linq;

namespace TwoHundred.Server.Handlers.Queries;

public class UpdateCompanyQuery : IRequest<ErrorOr<CompanyResource>>
{
    public Guid Id { get;}
    public string Name { get; }
    public string Type { get; }

    public UpdateCompanyQuery(Guid id, string name, string type)
    {
        Id = id; 
        Name = name;
        Type = type;
    }
}

public class UpdateCompanyQueryHandler : IRequestHandler<UpdateCompanyQuery, ErrorOr<CompanyResource>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public UpdateCompanyQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ErrorOr<CompanyResource>> Handle(UpdateCompanyQuery request, CancellationToken token)
    {
        var company = await unitOfWork.Repository().GetById<Company>(request.Id);

        if (company is null)
        {
            return Error.NotFound(code: "Model not found");
        }

        company.Name = request.Name;


        if (company.Type.Equals(request.Type, StringComparison.OrdinalIgnoreCase) == false && company.ExistingContacts.Any())
        {
            return Error.Validation(code: "It is forbidden to change the type of company under existing contracts");
        }

        company.Type = request.Type;
        
        unitOfWork.Repository().Update(company);
        await unitOfWork.CommitAsync(token);

        return mapper.Map<CompanyResource>(company);
    }
}

public class UpdateCompanyQueryValidator : AbstractValidator<UpdateCompanyQuery>
{
    public UpdateCompanyQueryValidator()
    {
        RuleFor(_ => _.Name).NotEmpty();
        RuleFor(_ => _.Type).NotEmpty();    
    }
}
