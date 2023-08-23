using AutoMapper;
using ErrorOr;
using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using TwoHundred.Server.Abstractions;
using TwoHundred.Server.Entities;
using System;

namespace TwoHundred.Server.Handlers.Queries;

public class DeleteCompanyQuery : IRequest<ErrorOr<bool>>
{
    public Guid Id { get; }

    public DeleteCompanyQuery(Guid id)
    {
        Id = id;
    }
}

public class DeleteCompanyQueryHandler : IRequestHandler<DeleteCompanyQuery, ErrorOr<bool>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public DeleteCompanyQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteCompanyQuery request, CancellationToken token)
    {
        try
        {
            unitOfWork.Repository().Delete<Company>(new Company() { Id = request.Id });
        }
        catch (Exception ex) 
        {
            return Error.Failure(code: "Error happened", description: ex.Message);
        }

        return true;
    }
}

public class DeleteCompanyQueryValidator : AbstractValidator<DeleteCompanyQuery>
{
    public DeleteCompanyQueryValidator()
    {
        RuleFor(_ => _.Id).NotEmpty();
    }
}



