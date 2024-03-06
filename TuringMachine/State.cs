namespace TuringMachine;

public record State {
  public class Actions(string writes, string changesTo, Action headerAction, Flags flag) {
    public readonly string Writes = writes;
    public readonly string ChangesTo = changesTo;
    public readonly Action HeaderAction = headerAction;
    public readonly Flags Flag = flag;
  }

  public enum Flags {
    Start,
    Continue,
    ExitSuccess,
    ExitFailure
  }

  public enum Action {
    Stay,
    MoveLeft,
    MoveRight
  }

  public static HashSet<State> States { get; set; } = [];

  public string StateName { get; }
  public Dictionary<string, Actions> Reads { get; set; }

  public State(string stateName, Dictionary<string, Actions> reads) {
    StateName = stateName;
    Reads = reads;

    States.Append(this);
  }
}