using UnityEngine;

namespace Mafu.UnityServiceLocator
{
    [AddComponentMenu("ServiceLocator/Service Locator Global")]
    public class ServiceLocatorGlobalBootstrapper : Boostrapper
    {
        [SerializeField] bool dontDestroyOnLoad = true;
        protected override void Bootstrap()
        {
            Container.ConfigureAsGlobal(dontDestroyOnLoad);
        }
    }
}