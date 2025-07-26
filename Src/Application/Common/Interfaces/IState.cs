using Application.Updates.Commands;

namespace Application.Common.Interfaces;

public interface IState
{
    public string StateText { get; }
    public Task HandleAsync(ProcessUpdateCommand request);
}