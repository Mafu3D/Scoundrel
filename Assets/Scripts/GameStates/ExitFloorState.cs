using Project.Core.StateMachineSystem;

namespace Project.GameStates
{
    public class ExitFloorState : State
    {
        public ExitFloorState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter() { }
        public override void Update(float time) { }
        public override void OnExit() { }
    }
}