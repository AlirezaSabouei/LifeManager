using System.Collections.ObjectModel;
using Application.Common.Interfaces;
using Application.Telegram.States;

namespace Infrustructure.States;

public class StateManager : IStateManager
{
    private List<IState> states;
    public ReadOnlyCollection<IState> States => states.AsReadOnly();

    public StateManager()
    {
        states = [];
        AddState(new Start());
    }

    private void AddState(IState state)
    {
        states.Add(state);
    }

    public IState ChooseState(string? state)
    {
        if (string.IsNullOrWhiteSpace(state))
        {
            return new Invalid();
        }  
        var chosenState = states.FirstOrDefault(a => a.State.ToLower() == state.ToLower());
        return chosenState ?? new Invalid();
    }
}
