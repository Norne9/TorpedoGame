using System;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller {
    [Serializable]
    public class Settings {
        public GameObject enemyPrefab;
        public GameObject bulletPrefab;
    }

    [SerializeField] private Boat boat;
    [SerializeField] private GameUI gameUI;
    private Settings _settings;

    [Inject]
    public void Construct(Settings settings) {
        _settings = settings;
    }

    public override void InstallBindings() {
        SignalBusInstaller.Install(Container);
        
        Container.BindInterfacesAndSelfTo<TorpedoManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        
        Container.Bind<GameInput>().AsSingle();
        
        Container.BindMemoryPool<Bullet, Bullet.Pool>()
            .FromComponentInNewPrefab(_settings.bulletPrefab)
            .UnderTransformGroup("Bullets");
        Container.BindMemoryPool<Torpedo, Torpedo.Pool>()
            .FromComponentInNewPrefab(_settings.enemyPrefab)
            .UnderTransformGroup("Torpedoes");

        Container.Bind<Boat>().FromInstance(boat);
        Container.Bind<GameUI>().FromInstance(gameUI);
        
        Container.DeclareSignal<BeginGameSignal>();
        Container.BindSignal<BeginGameSignal>()
            .ToMethod<GameController>(x => x.OnBeginGame).FromResolve();
        
        Container.DeclareSignal<PlayerKilledSignal>();
        Container.BindSignal<PlayerKilledSignal>()
            .ToMethod<GameController>(x => x.OnPlayerKilled).FromResolve();
        
        Container.DeclareSignal<TorpedoDestroedSignal>();
        Container.BindSignal<TorpedoDestroedSignal>()
            .ToMethod<GameController>(x => x.OnTorpedoDestroed).FromResolve();
        
        Container.DeclareSignal<RespawnPlayerSignal>();
        Container.BindSignal<RespawnPlayerSignal>()
            .ToMethod<Boat>(x => x.Respawn).FromResolve();
        
        Container.DeclareSignal<WeaponChangedSignal>();
        Container.BindSignal<WeaponChangedSignal>()
            .ToMethod<GameController>(x => x.OnWeaponChanged).FromResolve();
    }
}