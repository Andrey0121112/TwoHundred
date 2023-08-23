using Moq;
using NUnit.Framework;
using TwoHundred.Server.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using TwoHundred.Server.Handlers.Queries;
using AutoMapper;
using System;
using TwoHundred.Server.Entities;
using TwoHundred.Server.Resources;

namespace TwoHundred.Server.Tests;

public class CompanyUnitTest
{
    private UpdateCompanyQueryHandler updateCompanyQuery;

    [SetUp]
    public void Setup()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var repository = new Mock<IRepository>();
        var mapperMock = new Mock<IMapper>();

        repository.Setup(_ => _.Update(It.IsAny<Company>()))
            .Verifiable();

        repository.Setup(_ => _.GetById<Company>(Guid.Parse("6C2BBF5C-CB5A-415E-A8D8-E36C78C7D61A")))
            .Returns(async () => await Task.FromResult(new Company() 
            {
                Id = Guid.Parse("6C2BBF5C-CB5A-415E-A8D8-E36C78C7D61A"), 
                Type = "Supplier" 
            }));

        repository.Setup(_ => _.GetById<Company>(Guid.Parse("6F3EFF0F-A99C-4622-9EA2-6CB5DB908DF3")))
            .Returns(async () => await Task.FromResult(new Company()
            {
                Id = Guid.Parse("6F3EFF0F-A99C-4622-9EA2-6CB5DB908DF3")                                                            ,
                Type = "Supplier",
                ExistingContacts = "F2FB4BBF-701B-439A-9373-BEA9DF7C25F0",
            }));

        unitOfWorkMock.Setup(_ => _.Repository()).Returns(repository.Object);
        unitOfWorkMock.Setup(_ => _.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(async () => await Task.FromResult(0));

        mapperMock.Setup(_ => _.Map<CompanyResource>(It.IsAny<Company>())).
            Returns(() => new CompanyResource());

        updateCompanyQuery = new UpdateCompanyQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
    }

    [Test]
    public void Sucsessfully()
    {
        var result = updateCompanyQuery.Handle(new UpdateCompanyQuery(Guid.Parse("6C2BBF5C-CB5A-415E-A8D8-E36C78C7D61A"), 
                                                  "Test-1", "Supplier"), CancellationToken.None).Result;
 
        Assert.That(result.IsError, Is.False);
    }

    [Test]
    public void ChangeType()
    {
        var result = updateCompanyQuery.Handle(new UpdateCompanyQuery(Guid.Parse("6C2BBF5C-CB5A-415E-A8D8-E36C78C7D61A"),
                                                 "Test-1", "Vendor"), CancellationToken.None).Result;

        Assert.That(result.IsError, Is.False);
    }

    [Test]
    public void ChangeWithContact()
    {
        var result = updateCompanyQuery.Handle(new UpdateCompanyQuery(Guid.Parse("6F3EFF0F-A99C-4622-9EA2-6CB5DB908DF3"),
                                                 "Test-1", "Vendor"), CancellationToken.None).Result;
        Assert.That(result.IsError, Is.True);
    }
}