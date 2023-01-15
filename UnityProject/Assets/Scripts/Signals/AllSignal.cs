public class MatchStartedSignal
{
    public readonly int carIndex;
    public MatchStartedSignal(int carIndex)
    {
        this.carIndex = carIndex;
    }
}

public class GameEndedSignal { }

public class SpawnCollidedSignal { }
