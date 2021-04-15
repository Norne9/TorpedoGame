using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class GameUI : MonoBehaviour {
    public int Score {
        set {
            _score = value;
            UpdateScore();
        }
    }
    public int BestScore {
        set {
            _bestScore = value;
            UpdateScore();
        }
    }
    public string CurrentWeapon {
        set => weaponText.text = $"Weapon\n{value}";
    }
    
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI weaponText;
    [SerializeField] private GameObject gameOverWindow;

    private SignalBus _signalBus;
    private int _score, _bestScore;

    [Inject]
    public void Construct(SignalBus signalBus) {
        _signalBus = signalBus;
    }

    private void UpdateScore() {
        scoreText.text = $"Score: {_score}\nBest: {_bestScore}";
    }

    public void ShowGameOver() {
        gameOverWindow.SetActive(true);
    }

    public void OnRestartClick() {
        gameOverWindow.SetActive(false);
        _signalBus.Fire(new BeginGameSignal());
    }
}
