using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller> {
    public GameSettings settings;
    public override void InstallBindings() {
        Container.BindInstance(settings.boatSettings);
        Container.BindInstance(settings.waveSettings);
        Container.BindInstance(settings.installerSettings);
    }
}