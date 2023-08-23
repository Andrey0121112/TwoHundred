using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using TwoHundred.Server.Abstractions;
using TwoHundred.Server.Entities;
using TwoHundred.Server.Handlers.Queries;
using TwoHundred.Server.Resources;

namespace TwoHundred.Server.Tests;

public class SignContractTest
{
    private SignContractQueryHandler signContractQuery;

    [SetUp]
    public void Setup()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var repository = new Mock<IRepository>();
        var mapperMock = new Mock<IMapper>();

        repository.Setup(_ => _.Add(It.IsAny<Contract>()))
            .Returns(() => new Contract());

        repository.Setup(_ => _.Add(It.IsAny<ContractHistory>()))
            .Returns(() => new ContractHistory());


        repository.Setup(_ => _.Update(It.IsAny<Contract>()))
            .Verifiable();
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
                Id = Guid.Parse("6F3EFF0F-A99C-4622-9EA2-6CB5DB908DF3"),
                Type = "Vendor",
            }));

        repository.Setup(_ => _.GetById<Company>(Guid.Parse("EC498093-58A4-42CB-9EA3-0E3D8A952CE3")))
            .Returns(async () => await Task.FromResult(new Company()
            {
                Id = Guid.Parse("EC498093-58A4-42CB-9EA3-0E3D8A952CE3"),
                Type = "Vendor",
            }));
        repository.Setup(_ => _.GetById<Company>(Guid.Parse("EC498085-58A4-42CB-9EA3-0E3D8A952CE3")))
            .Returns(async () => await Task.FromResult<Company>(null));


        unitOfWorkMock.Setup(_ => _.Repository()).Returns(repository.Object);
        unitOfWorkMock.Setup(_ => _.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(async () => await Task.FromResult(0));

        mapperMock.Setup(_ => _.Map<CompanyResource>(It.IsAny<Company>())).
            Returns(() => new CompanyResource());


        signContractQuery = new SignContractQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
    }


    [Test]
    public void Sucsessfully()
    {
        var result = signContractQuery.Handle(new SignContractQuery(Guid.Parse("6F3EFF0F-A99C-4622-9EA2-6CB5DB908DF3"),
            Guid.Parse("6C2BBF5C-CB5A-415E-A8D8-E36C78C7D61A"), string.Empty), CancellationToken.None).Result;
        Assert.That(result.IsError, Is.False);
    }

    [Test]
    public void NotFoundCompany()
    {
        var result = signContractQuery.Handle(new SignContractQuery(Guid.Parse("6F3EFF0F-A99C-4622-9EA2-6CB5DB908DF3"),
            Guid.Parse("EC498085-58A4-42CB-9EA3-0E3D8A952CE3"), string.Empty), CancellationToken.None).Result;
        Assert.That(result.IsError, Is.True);
    }

    [Test]
    public void CompanyTypeTest()
    {
        var result = signContractQuery.Handle(new SignContractQuery(Guid.Parse("6F3EFF0F-A99C-4622-9EA2-6CB5DB908DF3"), 
            Guid.Parse("EC498093-58A4-42CB-9EA3-0E3D8A952CE3"), string.Empty), CancellationToken.None).Result;
        Assert.That(result.IsError, Is.True);
    }
}