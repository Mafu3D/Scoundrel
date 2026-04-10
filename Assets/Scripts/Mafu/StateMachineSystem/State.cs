namespace Mafu.StateMachineSystem
{
    public abstract class State : IState
    {
        protected StateMachine StateMachine;
        public float TimeInState { get; private set; } = 0f;

        public State(StateMachine stateMachine)
        {
            this.StateMachine = stateMachine;
        }

        public virtual void OnEnter() { }
        public virtual void Update(float time) { }
        public virtual void OnExit() { }

        public void UpdateTimeInState(float deltaTime) { TimeInState += deltaTime; }
    }
}