using AutoMapper;
using ErrorOr;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using TwoHundred.Server.Abstractions;
using TwoHundred.Server.Entities;
using TwoHundred.Server.Resources;
using FluentValidation;

namespace TwoHundred.Server.Handlers.Queries;

public class CreateCompanyQuery : IRequest<ErrorOr<CompanyResource>>
{
    public string Name { get; }
    public string Type { get; }

    public CreateCompanyQuery(string name, string type)
    {
        Name = name;
        Type = type;
    }
}


public class CreateCompanyQueryHandler : IRequestHandler<CreateCompanyQuery, ErrorOr<CompanyResource>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CreateCompanyQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ErrorOr<CompanyResource>> Handle(CreateCompanyQuery request, CancellationToken token)
    {
        var company = unitOfWork.Repository().Add<Company>(new Company() { Name = request.Name, Type = request.Type });

        if (company is null)
        {
            return Error.Failure(code: "Error when trying to save model");
        }

        await unitOfWork.CommitAsync(token);
        return mapper.Map<CompanyResource>(company);
    }
}

public class CreateCompanyQueryValidator : AbstractValidator<CreateCompanyQuery>
{
    public CreateCompanyQueryValidator()
    {
        RuleFor(_ => _.Name).NotEmpty();
        RuleFor(_ => _.Type).Must(_ => _.Equals("Vendor", System.StringComparison.OrdinalIgnoreCase) 
                                    || _.Equals("Supplier", System.StringComparison.OrdinalIgnoreCase));
    }
}

