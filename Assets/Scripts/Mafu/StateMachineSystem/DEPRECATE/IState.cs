using UnityEngine;

namespace Mafu.StateMachineSystemDeprecate
{
    public interface IState {
        void OnEnter();
        void Update();
        void FixedUpdate();
        void OnExit();
    }
}