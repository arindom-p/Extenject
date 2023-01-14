using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CarDataInstaller", menuName = "Installers/CarDataInstaller")]
public class CarDataInstaller : ScriptableObjectInstaller<CarDataInstaller>
{
    public CarData[] cars;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CarData[]>().FromInstance(cars).AsSingle();
    }
}

[System.Serializable]
public class CarData
{
    [field: SerializeField] public Sprite sprite { get; private set; }
    [field: SerializeField] public float initialSpeed { get; private set; }
    [field: SerializeField] public float acceleration { get; private set; }
}
