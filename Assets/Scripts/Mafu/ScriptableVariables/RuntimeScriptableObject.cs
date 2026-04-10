using System.Collections.Generic;
using UnityEngine;

namespace Mafu.ScriptableVariables
{
    public abstract class RuntimeScriptableObject : ScriptableObject
    {
        static readonly List<RuntimeScriptableObject> Instances = new();

        void OnEnable() => Instances.Add(this);
        void OnDisable() => Instances.Remove(this);

        protected abstract void OnReset();
        public void Reset()
        {
            OnReset();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void ResetAllInstances()
        {
            foreach(RuntimeScriptableObject instance in Instances)
            {
                instance.OnReset();
            }
        }
    }
}