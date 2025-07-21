using System.Collections.ObjectModel;

namespace Application.Common.Interfaces;

public interface IStateManager
{
    public ReadOnlyCollection<IState> States { get; }
    public IState ChooseState(string? state);
}
