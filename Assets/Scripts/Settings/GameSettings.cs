using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSettings {
    public Boat.Settings boatSettings;
    public TorpedoManager.Settings waveSettings;
    public GameInstaller.Settings installerSettings;
}
