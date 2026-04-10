using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafu.Singletons
{
    public class BaseSingleton : Singleton<BaseSingleton>
    {
        // Just inherits from Singleton class so it can be attached to any game object
        // that needs to persist scene-to-scene, even if it doesn't have any logic inside the class
    }
}
