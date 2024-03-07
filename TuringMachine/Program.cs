using TuringMachine;

var tape = ">1101001-";
var tapePosition = 0;

if (args.Length > 0) {
    tape = args.First();
}


State.States = StateManager.LoadFromFile("Path to file");

var states = new List<State> {
    new State("s", new Dictionary<string, State.Actions> {
        { "0", new State.Actions(">", "q0", State.Action.MoveRight, State.Flags.Continue) },
        { "1", new State.Actions(">", "q1", State.Action.MoveRight, State.Flags.Continue) },
        { ">", new State.Actions(">", "s", State.Action.MoveRight, State.Flags.Continue) },
        { "-", new State.Actions("-", "s", State.Action.Stay, State.Flags.ExitSuccess) },
    }),
    new State("q0", new Dictionary<string, State.Actions> {
        { "0", new State.Actions("0", "q0", State.Action.MoveRight, State.Flags.Continue) },
        { "1", new State.Actions("1", "q0", State.Action.MoveRight, State.Flags.Continue) },
        { "-", new State.Actions("-", "q0`", State.Action.MoveLeft, State.Flags.Continue) },
    }),
    new State("q1", new Dictionary<string, State.Actions> {
        { "0", new State.Actions("0", "q1", State.Action.MoveRight, State.Flags.Continue) },
        { "1", new State.Actions("1", "q1", State.Action.MoveRight, State.Flags.Continue) },
        { "-", new State.Actions("-", "q1`", State.Action.MoveLeft, State.Flags.Continue) },
    }),
    new State("q0`", new Dictionary<string, State.Actions> {
        { "0", new State.Actions("-", "q", State.Action.MoveLeft, State.Flags.Continue) },
        { "1", new State.Actions("1", "q0`", State.Action.Stay, State.Flags.ExitFailure) },
        { ">", new State.Actions("-", "q0`", State.Action.MoveRight, State.Flags.ExitSuccess) },
    }),
    new State("q1`", new Dictionary<string, State.Actions> {
        { "0", new State.Actions("1", "q1`", State.Action.Stay, State.Flags.ExitFailure) },
        { "1", new State.Actions("-", "q", State.Action.MoveLeft, State.Flags.Continue) },
        { ">", new State.Actions(">", "q1`", State.Action.MoveRight, State.Flags.ExitSuccess) },
    }),
    new State("q", new Dictionary<string, State.Actions> {
        { "0", new State.Actions("0", "q", State.Action.MoveLeft, State.Flags.Continue) },
        { "1", new State.Actions("1", "q", State.Action.MoveLeft, State.Flags.Continue) },
        { ">", new State.Actions(">", "s", State.Action.MoveRight, State.Flags.Continue) },
    })
};

foreach (var state in states) {
    StateManager.AddState(state);
}

StateManager.ChangeState(states.First());
var counter = 1;

while (true) {
    var redValue = tape[tapePosition];
    var prevState = StateManager.CurrentState;
    var actions = StateManager.NextStep(redValue.ToString());

    Console.Write($"{counter++}: ");
    for (var i = 0; i < tape.Length; ++i) {
        Console.ForegroundColor = i == tapePosition ? ConsoleColor.Green : ConsoleColor.White;

        Console.Write(tape[i]);
    }

    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine($", {prevState?.StateName} -> {
        (actions.Flag == State.Flags.Continue ?
             actions.ChangesTo : Enum.GetNames(typeof(State.Flags))[(int)actions.Flag])
    }, {tape[tapePosition]} -> {actions.Writes}");

    if (actions.Writes.Length == 1) {
        tape = tape.Remove(tapePosition, 1)
                   .Insert(tapePosition, actions.Writes);
    }

    tapePosition += actions.HeaderAction switch {
                        State.Action.MoveLeft => -1,
                        State.Action.MoveRight => 1,
                        State.Action.Stay => 0,
                        _ => 0
                    };

    if (actions.Flag != State.Flags.Continue) {
        Console.WriteLine($"{counter}: {tape}");
        break;
    }
}

