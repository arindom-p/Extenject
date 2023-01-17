using UnityEngine;
using Zenject;

public class OutsideScreenObstacleCollector : MonoBehaviour
{
    private SignalBus signalBus;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        this.signalBus = signalBus;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        signalBus.Fire(new CollectObstacle(collision.GetComponent<Obstacle>()));
    }
}
