using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class TorpedoManager : ITickable {
    [Serializable]
    public class Settings {
        public int initialWaveSize = 10;
        public float waveSpawnTimeout = 10f;
        public int additionalWaveSize = 3;
        public float initialEnemySpeed = 2.0f;
        public float enemySpeedIncrease = 0.1f;
    }

    private Settings _settings;
    private Torpedo.Pool _torpedoPool;
    private Boat _boat;
    private List<Torpedo> _torpedoes;
    private float _lastSpawn, _speed;
    private int _waveSize;
    private bool _active;

    [Inject]
    public void Construct(Settings settings, Torpedo.Pool torpedoPool, Boat boat) {
        _settings = settings;
        _torpedoPool = torpedoPool;
        _boat = boat;
        _torpedoes = new List<Torpedo>();
    }

    public void Begin() {
        _active = true;
        _lastSpawn = float.MinValue;
        _speed = _settings.initialEnemySpeed;
        _waveSize = _settings.initialWaveSize;
    }

    public void Stop() {
        _active = false;
        foreach (var torpedo in _torpedoes) {
            _torpedoPool.Despawn(torpedo);
        }
        _torpedoes.Clear();
    }
    
    public void Tick() {
        if (!_active) return;

        if (Time.time > _lastSpawn + _settings.waveSpawnTimeout) {
            _lastSpawn = Time.time;
            SpawnWave();
        }
    }

    private void SpawnWave() {
        for (var i = 0; i < _waveSize; i++) {
            var point = _boat.Position + Random.insideUnitCircle.normalized * 15f;
            var torpedo = _torpedoPool.Spawn(point, _speed);
            _torpedoes.Add(torpedo);
        }

        _speed += _settings.enemySpeedIncrease;
        _waveSize += _settings.additionalWaveSize;
    }

    public void Despawn(Torpedo torpedo) {
        _torpedoes.Remove(torpedo);
        _torpedoPool.Despawn(torpedo);
    }
}
