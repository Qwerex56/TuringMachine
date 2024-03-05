using TuringMachine;

var tape = "sb";
var tapePosition = 0;

State.States = StateManager.LoadFromFile("Path to file");

var states = new List<State> {
    new("a", "b", "s", "u", State.Action.MoveRight),
    new("b", "b", "b", "p", State.Action.Stay),
};

foreach (var state in states) {
    StateManager.AddState(state);
}

StateManager.ChangeState([states.First()]);

while (true) {
    Console.WriteLine(tape);
    var redValue = tape[tapePosition];
    var actions = StateManager.NextStep(redValue.ToString());

    if (actions.Writes.Length == 1) {
        tape = tape.Remove(tapePosition, 1)
                   .Insert(tapePosition, actions.Writes);
    }

    tapePosition += actions.NextMove switch {
                        State.Action.MoveLeft => -1,
                        State.Action.MoveRight => 1,
                        State.Action.Stay => 0,
                        _ => 0
                    };

    if (actions.Flag != State.Flags.Continue) {
        break;
    }
}