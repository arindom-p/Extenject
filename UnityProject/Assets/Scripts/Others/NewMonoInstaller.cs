using Zenject;

public class NewMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CarController>().FromComponentInHierarchy().AsSingle(); //what whould be the better way?
        Container.Bind<ObstacleSpawner>().FromComponentInHierarchy().AsSingle();
        Container.Bind<MotionController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<OutsideScreenObstacleCollector>().FromComponentInHierarchy().AsSingle();

        #region Signal bind
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<MatchStartedSignal>();
        Container.BindSignal<MatchStartedSignal>()
            .ToMethod<CarController>((refInstance, sigInstance) => refInstance.OnRaceStart(sigInstance.carIndex))
            .FromResolve();
        Container.BindSignal<MatchStartedSignal>()
            .ToMethod<MotionController>((refInstance, sigInstance) => refInstance.OnRaceStart())
            .FromResolve();
        Container.BindSignal<MatchStartedSignal>()
            .ToMethod<ObstacleSpawner>((refInstance, sigInstance) => refInstance.OnRaceStart())
            .FromResolve();

        Container.DeclareSignal<GameEndedSignal>();
        Container.BindSignal<GameEndedSignal>()
            .ToMethod<MotionController>((refInstance, sigInstance) => refInstance.OnRaceEnd())
            .FromResolve();
        Container.BindSignal<GameEndedSignal>()
            .ToMethod<GameController>((refInstance, sigInstance) => refInstance.OnRaceEnd())
            .FromResolve();

        Container.DeclareSignal<SpawnObstacleSignal>();
        Container.BindSignal<SpawnObstacleSignal>()
            .ToMethod<ObstacleSpawner>((refInstance, sigInstance) => refInstance.SpawnObstacle())
            .FromResolve();

        Container.DeclareSignal<CollectObstacle>();
        Container.BindSignal<CollectObstacle>()
            .ToMethod<ObstacleSpawner>((refInstance, sigInstance) => refInstance.MakeObstacleIdle(sigInstance.t))
            .FromResolve();
        #endregion
    }
}