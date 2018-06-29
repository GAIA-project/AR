using UnityEngine;
using Zenject;

public class Installer : MonoInstaller<Installer>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GaiaNetworkManager>().FromInstance(FindObjectOfType<GaiaNetworkManager>()).AsSingle();
    }
}