using System;

namespace Project.Core.StateMachineSystem
{
    public interface IState
    {
        void OnEnter();
        void Update(float deltaTime);
        void OnExit();
    }
}