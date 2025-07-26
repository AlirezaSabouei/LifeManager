using Application.Common.Interfaces;
using Application.Updates.States;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Updates.Commands;

public record ProcessUpdateCommand : IRequest
{
    public long UpdateId { get; set; }
    public long UserId { get; set; }
    public bool IsBot { get; set; }
    public string FirstName { get; set; } = "Unknown";
    public string UserName { get; set; } = string.Empty;
    public string LanguageCode { get; set; } = string.Empty;
    public long ChatId { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class ProcessUpdateCommandHandler(IServiceProvider provider) : IRequestHandler<ProcessUpdateCommand>
{
    private readonly IServiceProvider _provider = provider;

    public async Task Handle(ProcessUpdateCommand request, CancellationToken cancellationToken)
    {
        var state = GetSuitableState(request.Text);
        await state.HandleAsync(request);
    }

    private IState GetSuitableState(string? key)
    {
        return key switch
        {
            "/start" => _provider.GetRequiredService<Start>(),
            "/water" => _provider.GetRequiredService<Water>(),
            _ => _provider.GetRequiredService<Invalid>()
        };
    }
}
