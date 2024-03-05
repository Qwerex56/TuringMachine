namespace TuringMachine;

public static class StateManager {
    public record Foo(State.Flags Flag, State.Action NextMove = State.Action.Stay, string Writes = "");
    private static IEnumerable<State> _currentStateArr = [];

    public static HashSet<State> LoadFromFile(string path = "") {
        // TODO: Load from file logic

        return [];
    }

    public static Foo NextStep(string redValue) {
        if (!_currentStateArr.Any()) {
            return new Foo(State.Flags.ExitFailure);
        }
        
        var currentState = (from state in _currentStateArr
                            where state.Reads == redValue
                            select state).FirstOrDefault();
        
        var nextStateArr = from state in State.States
                         where state.StateName == currentState.SwitchesTo
                         select state;
        
        if (currentState is null || !nextStateArr.Any()) {
            return new Foo(State.Flags.ExitFailure);
        }

        var returnObj = new Foo(State.Flags.Continue, currentState.NextMove, currentState.Writes);
        ChangeState(nextStateArr);
        
        return returnObj;
    }

    public static bool AddState(State state) {
        return State.States.Add(state);
    }

    public static bool RemoveState(State state) {
        return State.States.Remove(state);
    }

    public static bool ChangeState(IEnumerable<State> nextState) {
        _currentStateArr = nextState;
        return true;
    }
}