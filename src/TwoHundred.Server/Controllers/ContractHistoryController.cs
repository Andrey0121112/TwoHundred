using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using TwoHundred.Server.Handlers.Queries;

namespace TwoHundred.Server.Controllers;

[ApiController]
[Route("api/history")]
public class ContractHistoryController : ApiController
{

    private readonly ISender mediator;

    public ContractHistoryController(ISender mediator)
    {
        this.mediator = mediator;
    }


    [HttpGet]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(ErrorOr.ErrorOr))]
    public async Task<IActionResult> GetCompanyContractHistory(Guid companyId)
    {
        var result = await mediator.Send(new GetCompanyContractHistoryQuery(companyId));
        return result.Match(_ => StatusCode((int)HttpStatusCode.OK, _), errors => Problem(errors));
    }
}
