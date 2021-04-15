using System;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller {
    [Serializable]
    public class Settings {
        public GameObject boatPrefab;
        public GameObject enemyPrefab;
        public GameObject bulletPrefab;
    }

    private Settings _settings;

    [Inject]
    public void Construct(Settings settings) {
        _settings = settings;
    }

    public override void InstallBindings() { }
}