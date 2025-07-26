using FluentValidation;
using MediatR;
using NUnit.Framework;
using Shouldly;

namespace UnitTests.Common;

public abstract class ValidatorTestsBase<TValidator, TRequest> 
    where TValidator : AbstractValidator<TRequest>
    where TRequest : IBaseRequest
{
    protected TValidator Validator { get; set; }
    protected TRequest Request { get; set; }

    [SetUp]
    protected void Init()
    {
        Request = CreateRequest();
        Validator = CreateValidator();
        SetupDependencies();
    }

    protected abstract TRequest CreateRequest();
    protected abstract TValidator CreateValidator();
    protected virtual void SetupDependencies()
    {
    }

    [Test]
    public async Task Validation_ValidRequest_NoException()
    {
        var result = await Validator.ValidateAsync(Request);

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }
}

