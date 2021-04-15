using System;
using UnityEngine;

public class GameController {
    [Serializable]
    public class Settings {
        public int initialWaveSize = 10;
        public float waveSpawnTimeout = 10f;
        public int additionalWaveSize = 3;
        public float initialEnemySpeed = 2.0f;
        public float enemySpeedIncrease = 0.1f;
    }
}