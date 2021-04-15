using System;
using UnityEngine;
using Zenject;

public class GameController: IInitializable {
    private SignalBus _signalBus;
    private TorpedoManager _torpedoManager;
    private GameUI _gameUI;

    private int _playerScore = 0;
    private int _maxScore = 0;
    
    [Inject]
    public void Construct(SignalBus signalBus, TorpedoManager torpedoManager, GameUI gameUI) {
        _signalBus = signalBus;
        _torpedoManager = torpedoManager;
        _gameUI = gameUI;
    }

    public void Initialize() {
        _signalBus.Fire(new BeginGameSignal());
    }

    public void OnBeginGame() {
        _torpedoManager.Begin();
        _signalBus.Fire(new RespawnPlayerSignal());
        _playerScore = 0;
        _gameUI.Score = _playerScore;
        _gameUI.BestScore = _maxScore;
    }

    public void OnPlayerKilled() {
        _gameUI.ShowGameOver();
        _torpedoManager.Stop();
    }

    public void OnTorpedoDestroed() {
        _playerScore += 1;
        _gameUI.Score = _playerScore;
        if (_playerScore > _maxScore) {
            _maxScore = _gameUI.BestScore = _playerScore;
        }
    }

    public void OnWeaponChanged(WeaponChangedSignal signal) {
        _gameUI.CurrentWeapon = signal.NewWeapon;
    }
}