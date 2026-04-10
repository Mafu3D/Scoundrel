using System;
using UnityEngine;

namespace Mafu.StateMachineSystem
{
    /// <summary>
    /// Base class for unit state machines. Ticks the current state every Update.
    /// </summary>
    public class StateMachine
    {
        public StateMachine() { }

        public State CurrentState;

        public State PreviousState;

        public event Action OnSwitchStateEvent;

        public void Update()
        {
            float deltaTime = Time.deltaTime;
            CurrentState?.Update(deltaTime);
            CurrentState?.UpdateTimeInState(deltaTime);
        }

        public void SwitchState(State newState)
        {
            if (CurrentState == newState) return;

            // Exit the previous state
            CurrentState?.OnExit();

            // Cache the previous state
            if (CurrentState != null) PreviousState = CurrentState;

            // Set the new state and its parent state
            CurrentState = newState;

            // Enter the new state
            CurrentState?.OnEnter();

            // Callback
            OnSwitchStateEvent?.Invoke();
        }

        public void SetInitialState(State newState) {
            CurrentState = newState;
            CurrentState.OnEnter();
        }

    }
}