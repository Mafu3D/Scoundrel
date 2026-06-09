using System;

namespace Project.StateMachineSystem
{
    public interface IState
    {
        void OnEnter();
        void Update(float deltaTime);
        void OnExit();
    }
}