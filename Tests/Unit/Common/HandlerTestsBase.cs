using Domain.Common;
using MediatR;
using NUnit.Framework;
using Shouldly;

namespace UnitTests.Common;

public abstract class HandlerTestsBase<THandler, TRequest, TEntity>
    where THandler : IRequestHandler<TRequest,TEntity>
    where TRequest : IRequest<TEntity>
    where TEntity : BaseEntity
{
    protected THandler Handler { get; set; }
    protected TRequest Request { get; set; }

    [SetUp]
    protected void Init()
    {
        SetupDependencies();
        Request = CreateRequest();
        Handler = CreateHandler();        
    }

    protected abstract TRequest CreateRequest();
    protected abstract THandler CreateHandler();
    protected virtual void SetupDependencies()
    {
    }

    [Test]
    public async Task Handle_ValidRequest_NoException()
    {
        var result = await Handler.Handle(Request, new CancellationToken());

        result.ShouldNotBeNull();
    }
}

public abstract class HandlerTestsBase<THandler, TRequest>
    where THandler : IRequestHandler<TRequest>
    where TRequest : IRequest
{
    protected THandler Handler { get; set; }
    protected TRequest Request { get; set; }

    [SetUp]
    protected void Init()
    {
        SetupDependencies();
        Request = CreateRequest();
        Handler = CreateHandler();
    }

    protected abstract TRequest CreateRequest();
    protected abstract THandler CreateHandler();
    protected virtual void SetupDependencies()
    {
    }

    [Test]
    public async Task Handle_ValidRequest_NoException()
    {
        await Handler.Handle(Request, new CancellationToken());
        true.ShouldBeTrue();
    }
}
