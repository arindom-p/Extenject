using Zenject;

public class NewMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CarController>().FromComponentInHierarchy().AsSingle(); //what whould be the better way?
    
        Container.DeclareSignal<MatchStartedSignal>();
    }
}