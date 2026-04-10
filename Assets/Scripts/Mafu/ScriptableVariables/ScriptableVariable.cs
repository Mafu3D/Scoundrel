using System;
using UnityEngine;
using UnityEngine.Events;

namespace Mafu.ScriptableVariables
{
    public class ScriptableVariable<T>: RuntimeScriptableObject
    {
        [SerializeField] T initialValue;
        [SerializeField, DebugInspector] T value;

        public event UnityAction<T> OnValueChanged = delegate { };

        public T Value
        {
            get => value;
            set
            {
                if (this.value.Equals(value)) return;
                this.value = value;
                OnValueChanged.Invoke(value);
            }
        }

        protected override void OnReset()
        {
            OnValueChanged.Invoke(value = initialValue);
        }
    }
}