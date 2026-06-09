using Mafu.UnityServiceLocator;

namespace Project.StateMachineSystem
{
    public abstract class State : IState
    {
        protected StateMachine StateMachine;
        public float TimeInState { get; private set; } = 0f;

        protected readonly GameManager gameManager;

        public State(StateMachine stateMachine)
        {
            this.StateMachine = stateMachine;
            ServiceLocator.Global.Get(out gameManager);
        }

        public abstract void OnEnter();
        public abstract void Update(float time);
        public abstract void OnExit();

        public void UpdateTimeInState(float deltaTime) { TimeInState += deltaTime; }
    }
}