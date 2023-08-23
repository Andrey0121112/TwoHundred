using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using TwoHundred.Server.Handlers.Queries;

namespace TwoHundred.Server.Controllers;

[ApiController]
[Route("api/contract")]
public class ContractController : ApiController
{
    private readonly ISender mediator;

    public ContractController(ISender mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(ErrorOr.ErrorOr))]
    public async Task<IActionResult> SignContractBetweenCompanies(Guid firstCompanyId, Guid secondCompanyId, string contractTitle)
    {
        var result = await mediator.Send(new SignContractQuery(firstCompanyId, secondCompanyId, contractTitle));
        return result.Match(_ => StatusCode((int)HttpStatusCode.OK, _), errors => Problem(errors));
    }

    [HttpDelete]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(ErrorOr.ErrorOr))]
    public async Task<IActionResult> UndoExistingContract(Guid contractId)
    {
        var result = await mediator.Send(new UndoContractQuery(contractId));
        return result.Match(_ => StatusCode((int)HttpStatusCode.OK, _), errors => Problem(errors));
    }
}
