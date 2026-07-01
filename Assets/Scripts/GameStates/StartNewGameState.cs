using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class StartNewGameState : State
    {
        public StartNewGameState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter() { }
        public override void Update(float time) { }
        public override void OnExit() { }
    }
}
