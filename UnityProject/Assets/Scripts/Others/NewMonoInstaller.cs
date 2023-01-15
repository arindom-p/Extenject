using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Zenject;

public class NewMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CarController>().FromComponentInHierarchy().AsSingle(); //what whould be the better way?


        #region Signal bind
        Container.DeclareSignal<MatchStartedSignal>();
        Container.BindSignal<MatchStartedSignal>()
            .ToMethod<CarController>((refInstance, sigInstance) => refInstance.OnRaceStart(sigInstance.carIndex))
            .FromResolve();
        Container.BindSignal<MatchStartedSignal>()
            .ToMethod<MotionController>((refInstance, sigInstance) => refInstance.OnRaceStart())
            .FromResolve();

        Container.DeclareSignal<GameEndedSignal>();
        Container.BindSignal<GameEndedSignal>()
            .ToMethod<MotionController>((refInstance, sigInstance) => refInstance.OnRaceEnd())
            .FromResolve();
        Container.BindSignal<GameEndedSignal>()
            .ToMethod<GameController>((refInstance, sigInstance) => refInstance.OnRaceEnd())
            .FromResolve();

        Container.DeclareSignal<SpawnCollidedSignal>();
        Container.BindSignal<SpawnCollidedSignal>()
            .ToMethod<ObstacleSpawner>((refInstance, sigInstance) => refInstance.SpawnObstacle())
            .FromResolve();
        #endregion
    }
}