using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class RoundActiveState : State
    {
        public RoundActiveState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter() { }
        public override void Update(float time) { }
        public override void OnExit() { }
    }
}