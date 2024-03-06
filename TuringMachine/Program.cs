using TuringMachine;

var tape = "sb";
var tapePosition = 0;

State.States = StateManager.LoadFromFile("Path to file");

var states = new List<State> {
  new State("a", new Dictionary<string, State.Actions> {
    { "s", new State.Actions("b", "b", State.Action.MoveRight, State.Flags.Continue) },
    { "b", new State.Actions("s", "a", State.Action.Stay, State.Flags.ExitSuccess)}
  }),
  new State("b", new Dictionary<string, State.Actions> {
    { "b", new State.Actions("b", "b", State.Action.Stay, State.Flags.ExitSuccess) },
    { "s", new State.Actions("s", "b", State.Action.MoveLeft, State.Flags.Continue) }
  }),
};

foreach (var state in states) {
  StateManager.AddState(state);
}

StateManager.ChangeState(states.First());

while (true) {
  Console.WriteLine(tape);
  var redValue = tape[tapePosition];
  var actions = StateManager.NextStep(redValue.ToString());

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
    break;
  }
}

Console.WriteLine(tape);