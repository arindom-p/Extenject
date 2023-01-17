using UnityEngine;
using Zenject;

public class Obstacle : MonoBehaviour
{
    public class Factory : PlaceholderFactory<GameObject, Obstacle> { }
}

public class ObstacleFactory : IFactory<GameObject, Obstacle>
{
    private DiContainer container;

    public ObstacleFactory(DiContainer container)
    {
        this.container = container;
    }

    public Obstacle Create(GameObject prefab)
    {
        return container.InstantiatePrefabForComponent<Obstacle>(prefab);
    }
}
