namespace TuringMachine;

public record State {
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

    private string _stateName;
    public string StateName => _stateName;
    
    private string _switchesTo;
    public string SwitchesTo => _switchesTo;
    
    
    private string _reads;
    public string Reads => _reads;
    
    private string _writes;
    public string Writes => _writes;
    
    private Action _nextMove;
    public Action NextMove => _nextMove;
    
    public State(string stateName, string switchesTo, string reads, string writes, Action nextMove) {
        _stateName = stateName;
        _switchesTo = switchesTo;
        _reads = reads;
        _writes = writes;
        _nextMove = nextMove;

        States.Add(this);
    }
}