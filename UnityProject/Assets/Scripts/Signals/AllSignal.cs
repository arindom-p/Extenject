using UnityEngine;

public class MatchStartedSignal
{
    public readonly int carIndex;
    public MatchStartedSignal(int carIndex)
    {
        this.carIndex = carIndex;
    }
}

public class GameEndedSignal { }

public class SpawnObstacleSignal { }

public class CollectObstacle
{
    public readonly Transform t;
    public CollectObstacle(Transform t)
    {
        this.t = t;
    }
}
