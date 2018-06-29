using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ApplicationSettingsInstaller", menuName = "Installers/ApplicationSettingsInstaller")]
public class ApplicationSettingsInstaller : ScriptableObjectInstaller<ApplicationSettingsInstaller>
{
    //public GaiaNetworkManager.Settings GaiaNetworkManagerSettings;

    public override void InstallBindings()
    {
        //Container.BindInstance(GaiaNetworkManagerSettings);
    }
}