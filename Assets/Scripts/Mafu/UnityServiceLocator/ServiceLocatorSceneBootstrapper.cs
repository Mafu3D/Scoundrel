using UnityEngine;

namespace Mafu.UnityServiceLocator
{
    [AddComponentMenu("ServiceLocator/Service Locator Scene")]
    public class ServiceLocatorSceneBootstrapper : Boostrapper
    {
        [SerializeField] bool dontDestroyOnLoad = true;
        protected override void Bootstrap()
        {
            Container.ConfigureForScene(dontDestroyOnLoad);
        }
    }
}