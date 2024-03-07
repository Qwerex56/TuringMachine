namespace TuringMachine;

public static class StateManager {
    private static State? _currentState;
    public static State? CurrentState => _currentState;

    public static HashSet<State> LoadFromFile(string path = "") {
        // TODO: Load from file logic

        return [];
    }

    public static State.Actions NextStep(string redValue) {
        if (_currentState is null) {
            return new State.Actions(redValue, "", State.Action.Stay, State.Flags.ExitFailure);
        }

        var nextState =
            State.States.FirstOrDefault(state => state.StateName == _currentState.Reads[redValue].ChangesTo);
        
        if (nextState is null) {
            return new State.Actions(redValue, "", State.Action.Stay, State.Flags.ExitFailure);
        }

        var actions = _currentState.Reads[redValue];
        ChangeState(nextState);
        
        return actions;
    }

    public static bool AddState(State state) {
        return State.States.Add(state);
    }

    public static bool RemoveState(State state) {
        return State.States.Remove(state);
    }

    public static bool ChangeState(State nextState) {
        _currentState = nextState;
        return true;
    }
}