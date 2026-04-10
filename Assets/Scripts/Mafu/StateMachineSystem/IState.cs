using System;

namespace Mafu.StateMachineSystem
{
    public interface IState
    {
        void OnEnter();
        void Update(float deltaTime);
        void OnExit();
    }
}