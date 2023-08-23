using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using TwoHundred.Server.Handlers.Queries;

namespace TwoHundred.Server.Controllers;

[ApiController]
[Route("api/company")]
public class CompanyController : ApiController
{
    private readonly ISender mediator;

    public CompanyController(ISender mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(ErrorOr.ErrorOr))]
    public async Task<IActionResult> GetAll() 
    {
        var result = await mediator.Send(new GetCompaniesQuery());
        return result.Match(_ => StatusCode((int)HttpStatusCode.OK, _), errors => Problem(errors));
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(ErrorOr.ErrorOr))]
    public async Task<IActionResult> Create(string name, string type) 
    {
        var result = await mediator.Send(new CreateCompanyQuery(name, type));
        return result.Match(_ => StatusCode((int)HttpStatusCode.OK, _), errors => Problem(errors));
    }

    [HttpPut]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(ErrorOr.ErrorOr))]
    public async Task<IActionResult> Edit(Guid id, string name, string type) 
    {
        var result = await mediator.Send(new UpdateCompanyQuery(id, name, type));
        return result.Match(_ => StatusCode((int)HttpStatusCode.OK, _), errors => Problem(errors));
    }
    
    [HttpDelete]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(ErrorOr.ErrorOr))]
    public async Task<IActionResult> Delete(Guid id) 
    {
        var result = await mediator.Send(new DeleteCompanyQuery(id));
        return result.Match(_ => StatusCode((int)HttpStatusCode.OK, _), errors => Problem(errors));
    }
}
