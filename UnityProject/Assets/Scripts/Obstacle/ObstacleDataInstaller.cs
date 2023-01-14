using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ObstacleDataInstaller", menuName = "Installers/ObstacleDataInstaller")]
public class ObstacleDataInstaller : ScriptableObjectInstaller<ObstacleDataInstaller>
{
    public ObstacleData[] obstacles;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ObstacleData[]>().FromInstance(obstacles).AsSingle();
    }
}

[System.Serializable]
public class ObstacleData
{
    [field: SerializeField] public GameObject prefab { get; private set; }
    [field: SerializeField] public Vector2 rotationRange { get; private set; }
    [field: SerializeField] public Vector2 size { get; private set; }
}